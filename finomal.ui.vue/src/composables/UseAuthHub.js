// composables/useAuthHub.js
// ─────────────────────────────────────────────────────────────────────────────
// Composable مشترک برای اتصال SignalR در صفحات Login و Register
// نصب: npm install @microsoft/signalr
// ─────────────────────────────────────────────────────────────────────────────

import { ref, onUnmounted } from 'vue'
import * as signalR from '@microsoft/signalr'

// ── ثابت‌ها ──────────────────────────────────────────────────────────────────
const HUB_URL = 'https://localhost:7272/AccountingHub/AuthUser'

const RETRY_DELAYS = [0, 1000, 2000, 5000, 10000, 30000]

// ── Composable ────────────────────────────────────────────────────────────────
export function useAuthHub() {
  // ── State ──────────────────────────────────────────────────────────────────
  const connection     = ref(null)
  const isConnected    = ref(false)
  const isConnecting   = ref(false)
  const connectionError = ref(null)

  // ── Event refs (مقادیر رویدادهای دریافتی از سرور) ─────────────────────────
  const loginResult        = ref(null)   // { isSuccess, data: { token, userId, userName, roleId } }
  const rolesList          = ref([])     // [{ id, name }]
  const registrationResult = ref(null)   // { isSuccess, data: { userId, userName, message } }
  const accountActivated   = ref(null)   // { message, userName, activatedAt }

  // ── اتصال به Hub ──────────────────────────────────────────────────────────
  async function connect() {
    if (isConnected.value || isConnecting.value) return

    isConnecting.value = true
    connectionError.value = null

    try {
      const hub = new signalR.HubConnectionBuilder()
        .withUrl(HUB_URL, {
          transport: signalR.HttpTransportType.WebSockets
                   | signalR.HttpTransportType.LongPolling,
        })
        .withAutomaticReconnect({
          nextRetryDelayInMilliseconds: (ctx) =>
            RETRY_DELAYS[ctx.previousRetryCount] ?? 30000,
        })
        .configureLogging(signalR.LogLevel.Warning)
        .build()

      _registerListeners(hub)

      hub.onreconnecting(() => { isConnected.value = false })
      hub.onreconnected(()  => { isConnected.value = true  })
      hub.onclose((err) => {
        isConnected.value = false
        if (err) connectionError.value = err.message
      })

      await hub.start()
      connection.value  = hub
      isConnected.value = true

    } catch (err) {
      connectionError.value = err?.message ?? 'خطا در اتصال به سرور'
      throw err
    } finally {
      isConnecting.value = false
    }
  }

  // ── ثبت تمام listener‌های سرور ─────────────────────────────────────────────
  function _registerListeners(hub) {
    hub.on('LoginResult',        (data) => { loginResult.value        = data })
    hub.on('RolesList',          (data) => { rolesList.value          = _normalizeRoles(data) })
    hub.on('RegistrationResult', (data) => { registrationResult.value = data })
    hub.on('AccountActivated',   (data) => { accountActivated.value   = data })

    // خطاهای سرور
    hub.on('RolesError', (err) => { console.warn('[Hub] RolesError:', err) })
    hub.on('LoginError', (err) => { console.warn('[Hub] LoginError:', err) })
  }

  // ── نرمال‌سازی فرمت نقش‌ها ─────────────────────────────────────────────────
  function _normalizeRoles(data) {
    const list = Array.isArray(data) ? data : (data?.data ?? data?.value ?? [])
    return list.map((r) => ({
      id:   r.id   ?? r.Id,
      name: r.name ?? r.Name ?? 'نامشخص',
    }))
  }

  // ── اطمینان از اتصال قبل از invoke ────────────────────────────────────────
  async function _ensureConnected() {
    if (!isConnected.value) await connect()
  }

  // ── متدهای قابل استفاده از خارج ────────────────────────────────────────────

  /**
   * ارسال درخواست ورود به سرور
   * @param {{ userName: string, password: string, roleId: string }} payload
   */
  async function login(payload) {
    await _ensureConnected()
    await connection.value.invoke('Login', {
      UserName: payload.userName.trim(),
      Password: payload.password,
      RoleId:   payload.roleId,
    })
  }

  /**
   * دریافت لیست نقش‌ها از سرور
   * تایم‌اوت: ۱۰ ثانیه
   */
  async function fetchRoles() {
    await _ensureConnected()

    return new Promise((resolve, reject) => {
      const timer = setTimeout(() => reject(new Error('Timeout: دریافت نقش‌ها')), 10_000)

      const onList  = (data) => { clearTimeout(timer); connection.value.off('RolesList', onList); connection.value.off('RolesError', onErr); rolesList.value = _normalizeRoles(data); resolve(rolesList.value) }
      const onErr   = (err)  => { clearTimeout(timer); connection.value.off('RolesList', onList); connection.value.off('RolesError', onErr); reject(new Error(err?.message ?? 'خطا در بارگذاری نقش‌ها')) }

      connection.value.on('RolesList',  onList)
      connection.value.on('RolesError', onErr)

      connection.value.invoke('GetAvailableRoles').catch(onErr)
    })
  }

  /**
   * بررسی در دسترس بودن نام کاربری
   * @param {string} userName
   * @returns {Promise<boolean>}
   */
  async function checkUsernameAvailability(userName) {
    await _ensureConnected()

    return new Promise((resolve, reject) => {
      const timer = setTimeout(() => { connection.value.off('UsernameCheckResult', handler); resolve(null) }, 5_000)

      const handler = ({ isAvailable }) => {
        clearTimeout(timer)
        connection.value.off('UsernameCheckResult', handler)
        resolve(isAvailable)
      }

      connection.value.on('UsernameCheckResult', handler)
      connection.value.invoke('CheckUsernameAvailability', userName).catch(() => { clearTimeout(timer); resolve(null) })
    })
  }

  /**
   * ارسال درخواست ثبت‌نام به Hub — دقیقاً مثل login
   * پاسخ از طریق watch(registrationResult) دریافت می‌شود
   */
  async function register(payload) {
    await _ensureConnected()
    await connection.value.invoke('Register', {
      UserName:  payload.userName.trim(),
      Password:  payload.password,
      Email:     payload.email,
      FirstName: payload.firstName,
      LastName:  payload.lastName,
      RoleId:    payload.roleId,
    })
  }

  /**
   * قطع اتصال
   */
  async function disconnect() {
    if (!connection.value) return
    await connection.value.stop()
    connection.value  = null
    isConnected.value = false
  }

  // ── پاکسازی خودکار هنگام unmount کامپوننت ─────────────────────────────────
  onUnmounted(disconnect)

  return {
    // state
    isConnected,
    isConnecting,
    connectionError,
    // events
    loginResult,
    rolesList,
    registrationResult,
    accountActivated,
    // methods
    connect,
    disconnect,
    login,
    register,
    fetchRoles,
    checkUsernameAvailability,
  }
}
