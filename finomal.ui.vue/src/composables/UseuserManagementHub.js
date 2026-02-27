// composables/useUserManagementHub.js
// ─────────────────────────────────────────────────────────────────────────────
// Composable مشترک برای اتصال SignalR در صفحه مدیریت کاربران
// نصب: npm install @microsoft/signalr
// ─────────────────────────────────────────────────────────────────────────────

import { ref, onUnmounted } from 'vue'
import * as signalR from '@microsoft/signalr'

// ── ثابت‌ها ───────────────────────────────────────────────────────────────────
const HUB_URL     = 'https://localhost:7272/AccountingHub/UserManagement'
const RETRY_DELAYS = [0, 1000, 2000, 5000, 10000, 30000]
const INVOKE_TIMEOUT_MS = 15_000

// ── نگاشت نام متد به برچسب فارسی ──────────────────────────────────────────
const METHOD_LABEL = {
  CreateUser:        'ایجاد کاربر',
  UpdateUser:        'ویرایش کاربر',
  DeleteUser:        'حذف کاربر',
  ToggleUserStatus:  'تغییر وضعیت',
  ResetPassword:     'ریست رمز عبور',
  GetUsers:          'دریافت کاربران',
  GetRoles:          'دریافت نقش‌ها',
  GetUserById:       'دریافت اطلاعات کاربر',
}

// ── Composable ────────────────────────────────────────────────────────────────
export function useUserManagementHub() {

  // ── State اتصال ─────────────────────────────────────────────────────────────
  const connection      = ref(null)
  const isConnected     = ref(false)
  const isConnecting    = ref(false)
  const connectionError = ref(null)

  // ── State داده ──────────────────────────────────────────────────────────────
  const usersList           = ref([])
  const rolesList           = ref([])
  const updatedUser         = ref(null)
  const statusChange        = ref(null)
  const deletedUserId       = ref(null)
  const passwordResetUserId = ref(null)

  // ── State خطا ───────────────────────────────────────────────────────────────
  const hubError       = ref(null)   // خطای broadcast از سرور  (ReceiveError)
  const operationError = ref(null)   // خطای آخرین invoke
  const lastOperation  = ref(null)   // نام متد آخرین invoke

  const clearOperationError = () => {
    operationError.value = null
    lastOperation.value  = null
  }

  // ══════════════════════════════════════════════════════════════════════════
  //  safeInvoke — invoke با timeout و تشخیص نوع خطا
  // ══════════════════════════════════════════════════════════════════════════
  async function safeInvoke(method, ...args) {
    if (!connection.value || !isConnected.value) {
      const msg = 'اتصال به سرور برقرار نیست. لطفاً صفحه را رفرش کنید.'
      operationError.value = msg
      lastOperation.value  = method
      throw new Error(msg)
    }

    lastOperation.value = method
    clearOperationError()

    let timeoutId
    try {
      const result = await Promise.race([
        connection.value.invoke(method, ...args),
        new Promise((_, reject) => {
          timeoutId = setTimeout(
            () => reject(Object.assign(new Error(`Timeout: ${method}`), { isTimeout: true })),
            INVOKE_TIMEOUT_MS
          )
        }),
      ])
      clearTimeout(timeoutId)
      return result

    } catch (err) {
      clearTimeout(timeoutId)

      // ── تشخیص نوع خطا ───────────────────────────────────────────────────
      const raw     = err?.message ?? err?.toString() ?? ''
      const label   = METHOD_LABEL[method] ?? method
      let   message = ''

      if (err.isTimeout) {
        message = `مهلت زمانی ${label} به پایان رسید. دوباره تلاش کنید.`

      } else if (raw.includes('401') || raw.toLowerCase().includes('unauthorized')) {
        message = 'دسترسی غیرمجاز. لطفاً دوباره وارد شوید.'

      } else if (raw.includes('403') || raw.toLowerCase().includes('forbidden')) {
        message = 'شما مجاز به انجام این عملیات نیستید.'

      } else if (raw.includes('404') || raw.toLowerCase().includes('not found')) {
        message = 'مورد درخواست‌شده یافت نشد.'

      } else if (raw.includes('409') || raw.toLowerCase().includes('conflict')) {
        message = 'این مقدار قبلاً ثبت شده است.'

      } else if (
        raw.toLowerCase().includes('validation') ||
        raw.toLowerCase().includes('invalid')
      ) {
        message = 'داده‌های وارد شده نامعتبر است.'

      } else if (
        raw.toLowerCase().includes('network') ||
        raw.toLowerCase().includes('connection')
      ) {
        message = 'خطای شبکه. اتصال اینترنت خود را بررسی کنید.'

      } else if (raw.startsWith('خطا') || /[\u0600-\u06FF]/.test(raw)) {
        // پیام فارسی مستقیم از سرور
        message = raw

      } else {
        message = `خطا در ${label}. لطفاً دوباره تلاش کنید.`
      }

      operationError.value = message
      console.error(`[UserManagementHub] ${method} failed:`, err)
      throw Object.assign(new Error(message), { original: err, method })
    }
  }

  // ══════════════════════════════════════════════════════════════════════════
  //  Listeners
  // ══════════════════════════════════════════════════════════════════════════
  function _registerListeners(hub) {
    // ── Query results ────────────────────────────────────────────────────────
    hub.on('ReceiveUsers',  (data)    => { usersList.value  = _normalizeUsers(data) })
    hub.on('ReceiveRoles',  (data)    => { rolesList.value  = _normalizeRoles(data) })

    // ── Broadcast events ──────────────────────────────────────────────────────
    hub.on('UserUpdated', (user) => {
      const normalized = _normalizeUser(user)
      updatedUser.value = normalized
      const idx = usersList.value.findIndex(u => u.id === normalized.id)
      if (idx !== -1) usersList.value[idx] = normalized
      else usersList.value.unshift(normalized)
    })

    hub.on('UserStatusUpdated', (payload) => {
      const userId   = payload?.userId   ?? payload?.UserId
      const isActive = payload?.isActive ?? payload?.IsActive
      statusChange.value = { userId, isActive }
      const u = usersList.value.find(u => u.id === userId)
      if (u) u.isActive = isActive
    })

    hub.on('UserDeleted', (userId) => {
      deletedUserId.value = userId
      usersList.value = usersList.value.filter(u => u.id !== userId)
    })

    hub.on('PasswordResetSuccess', (userId) => {
      passwordResetUserId.value = userId
    })

    // ── خطای سرور ─────────────────────────────────────────────────────────────
    hub.on('ReceiveError', (msg) => {
      const message = msg || 'خطایی از سمت سرور دریافت شد'
      hubError.value       = message
      operationError.value = message   // هم‌زمان در operationError هم ست می‌شود
      console.warn('[UserManagementHub] Server ReceiveError:', message)
    })
  }

  // ══════════════════════════════════════════════════════════════════════════
  //  اتصال
  // ══════════════════════════════════════════════════════════════════════════
  async function connect(tokenFactory = null) {
    if (isConnected.value || isConnecting.value) return

    isConnecting.value    = true
    connectionError.value = null
    clearOperationError()

    try {
      const urlOptions = tokenFactory ? { accessTokenFactory: tokenFactory } : {}

      const hub = new signalR.HubConnectionBuilder()
        .withUrl(HUB_URL, {
          ...urlOptions,
          transport:
            signalR.HttpTransportType.WebSockets |
            signalR.HttpTransportType.LongPolling,
        })
        .withAutomaticReconnect({
          nextRetryDelayInMilliseconds: (ctx) =>
            RETRY_DELAYS[ctx.previousRetryCount] ?? 60_000,
        })
        .configureLogging(signalR.LogLevel.Warning)
        .build()

      _registerListeners(hub)

      hub.onreconnecting(() => { isConnected.value = false })
      hub.onreconnected(async () => {
        isConnected.value = true
        await refreshAllData()
      })
      hub.onclose((err) => {
        isConnected.value = false
        connectionError.value = err?.message
          ? `اتصال قطع شد: ${err.message}`
          : 'اتصال به سرور قطع شد'
      })

      await hub.start()
      connection.value  = hub
      isConnected.value = true
      await refreshAllData()

    } catch (err) {
      const raw = err?.message ?? ''
      connectionError.value =
        raw.includes('401') || raw.toLowerCase().includes('unauthorized')
          ? 'لطفاً ابتدا وارد حساب کاربری خود شوید.'
          : raw.toLowerCase().includes('network') || raw.toLowerCase().includes('fetch')
            ? 'اتصال به سرور ممکن نیست. اتصال اینترنت را بررسی کنید.'
            : 'ناتوانی در برقراری ارتباط با سرور. لطفاً بعداً تلاش کنید.'
      console.error('[UserManagementHub] Connection failed:', err)
      throw err
    } finally {
      isConnecting.value = false
    }
  }

  async function refreshAllData() {
    await Promise.allSettled([
      safeInvoke('GetUsers').catch(() => {}),
      safeInvoke('GetRoles').catch(() => {}),
    ])
  }

  async function disconnect() {
    if (!connection.value) return
    await connection.value.stop()
    connection.value  = null
    isConnected.value = false
  }

  // ══════════════════════════════════════════════════════════════════════════
  //  Queries
  // ══════════════════════════════════════════════════════════════════════════
  const getUsers    = ()       => safeInvoke('GetUsers')
  const getRoles    = ()       => safeInvoke('GetRoles')
  const getUserById = (userId) => safeInvoke('GetUserById', userId)

  // ══════════════════════════════════════════════════════════════════════════
  //  Commands
  // ══════════════════════════════════════════════════════════════════════════
  const createUser = (p) =>
    safeInvoke('CreateUser', {
      FirstName:   p.firstName?.trim() ?? '',
      LastName:    p.lastName?.trim()  ?? '',
      UserName:    p.userName?.trim(),
      Email:       p.email?.trim()       || null,
      PhoneNumber: p.phoneNumber?.trim() || null,
      Password:    p.password,
      RoleId:      p.roleId,
    })

  const updateUser = (p) =>
    safeInvoke('UpdateUser', {
      UserId:      p.userId,
      FirstName:   p.firstName?.trim() ?? '',
      LastName:    p.lastName?.trim()  ?? '',
      Email:       p.email?.trim()       || null,
      PhoneNumber: p.phoneNumber?.trim() || null,
      RoleId:      p.roleId,
    })

  const toggleUserStatus = (userId, isActive) =>
    safeInvoke('ToggleUserStatus', userId, isActive)

  const deleteUser = (userId) =>
    safeInvoke('DeleteUser', userId)

  const resetPassword = (p) =>
    safeInvoke('ResetPassword', {
      UserId:      p.userId,
      NewPassword: p.newPassword,
    })

  // ══════════════════════════════════════════════════════════════════════════
  //  Normalizers
  // ══════════════════════════════════════════════════════════════════════════
  function _normalizeUser(u) {
    if (!u) return null
    return {
      id:            u.id            ?? u.Id,
      userName:      u.userName      ?? u.UserName      ?? '',
      email:         u.email         ?? u.Email         ?? '',
      phoneNumber:   u.phoneNumber   ?? u.PhoneNumber   ?? '',
      fullName:      u.fullName      ?? u.FullName      ?? '',
      firstName:     u.firstName     ?? u.FirstName     ?? '',
      lastName:      u.lastName      ?? u.LastName      ?? '',
      roles:        (u.roles ?? u.Roles ?? []).map(r =>
                      typeof r === 'string' ? r : (r.name ?? r.Name ?? '')),
      isActive:      u.isActive      ?? u.IsActive      ?? false,
      isLockedOut:   u.isLockedOut   ?? u.IsLockedOut   ?? false,
      createdDate:   u.createdDate   ?? u.CreatedDate   ?? null,
      lastLoginDate: u.lastLoginDate ?? u.LastLoginDate ?? null,
    }
  }

  function _normalizeUsers(data) {
    const list = Array.isArray(data) ? data : (data?.data ?? data?.value ?? [])
    return list.map(_normalizeUser).filter(Boolean)
  }

  function _normalizeRole(r) {
    if (!r) return null
    return {
      id:          r.id          ?? r.Id,
      name:        r.name        ?? r.Name        ?? '',
      displayName: r.displayName ?? r.DisplayName ?? r.name ?? r.Name ?? '',
    }
  }

  function _normalizeRoles(data) {
    const list = Array.isArray(data) ? data : (data?.data ?? data?.value ?? [])
    return list.map(_normalizeRole).filter(Boolean)
  }

  // ── پاکسازی خودکار ──────────────────────────────────────────────────────────
  onUnmounted(disconnect)

  // ── خروجی ───────────────────────────────────────────────────────────────────
  return {
    // state اتصال
    isConnected,
    isConnecting,
    connectionError,
    // state داده
    usersList,
    rolesList,
    updatedUser,
    statusChange,
    deletedUserId,
    passwordResetUserId,
    // state خطا
    hubError,
    operationError,
    lastOperation,
    clearOperationError,
    // lifecycle
    connect,
    disconnect,
    refreshAllData,
    // queries
    getUsers,
    getRoles,
    getUserById,
    // commands
    createUser,
    updateUser,
    toggleUserStatus,
    deleteUser,
    resetPassword,
  }
}
