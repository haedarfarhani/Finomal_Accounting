<template>
  <!-- Background layers -->
  <div class="bg-layer bg-base" />
  <div class="bg-layer bg-grid" />
  <div class="bg-layer bg-glow-1" />
  <div class="bg-layer bg-glow-2" />

  <!-- Floating geometric shapes -->
  <div class="geo geo-1" />
  <div class="geo geo-2" />
  <div class="geo geo-3" />
  <div class="geo geo-4" />

  <v-main class="login-main">
    <v-container fluid class="fill-height pa-0">
      <v-row no-gutters class="fill-height">

        <!-- ===== Left Panel - Branding ===== -->
        <v-col cols="12" md="6" class="brand-panel d-none d-md-flex flex-column justify-space-between pa-12">
          <div class="brand-top">
            <div class="brand-logo-wrap">
              <v-icon size="36" class="brand-logo-icon">mdi-chart-finance</v-icon>
            </div>
            <div class="brand-name mt-4">فینومال</div>
            <div class="brand-tagline">نرم‌افزار حسابداری یکپارچه</div>
          </div>

          <div class="brand-center">
            <div class="stats-card">
              <div class="stats-card-header">خلاصه وضعیت مالی</div>
              <div class="stats-row">
                <div class="stats-item">
                  <div class="stats-val positive">+۱۲٪</div>
                  <div class="stats-lbl">رشد درآمد</div>
                </div>
                <div class="stats-sep" />
                <div class="stats-item">
                  <div class="stats-val">۱۲۵M</div>
                  <div class="stats-lbl">گردش ماهانه</div>
                </div>
                <div class="stats-sep" />
                <div class="stats-item">
                  <div class="stats-val accent-val">۲۳</div>
                  <div class="stats-lbl">فاکتور صادره</div>
                </div>
              </div>
              <div class="mini-chart">
                <div v-for="(h, i) in chartBars" :key="i" class="mini-bar"
                  :style="{ height: h + 'px', animationDelay: (i * 0.06) + 's' }"
                  :class="{ active: i === chartBars.length - 1 }" />
              </div>
            </div>

            <div class="feature-list mt-8">
              <div v-for="f in features" :key="f" class="feature-item">
                <v-icon size="16" color="#38bdf8" class="me-3">mdi-check-circle-outline</v-icon>
                <span>{{ f }}</span>
              </div>
            </div>
          </div>

          <div class="brand-bottom">
            <div class="copyright">© ۱۴۰۳ فینومال — تمام حقوق محفوظ است</div>
          </div>
        </v-col>

        <!-- ===== Right Panel - Login Form ===== -->
        <v-col cols="12" md="6" class="form-panel d-flex align-center justify-center pa-6">
          <div class="login-card">

            <!-- Mobile logo -->
            <div class="d-flex d-md-none align-center mb-8 justify-center">
              <v-icon size="32" class="brand-logo-icon me-3">mdi-chart-finance</v-icon>
              <span class="brand-name-sm">فینومال</span>
            </div>

            <!-- Heading -->
            <div class="form-heading mb-8">
              <div class="form-title">خوش آمدید</div>
              <div class="form-subtitle">برای ادامه وارد حساب کاربری خود شوید</div>
            </div>

            <!-- Form -->
            <v-form ref="formRef" @submit.prevent="handleLogin">

              <!-- Username -->
              <div class="field-wrap mb-5">
                <label class="field-label">نام کاربری</label>
                <v-text-field v-model="form.username" :rules="[rules.required]" placeholder="نام کاربری یا ایمیل"
                  variant="outlined" density="comfortable" class="login-field"
                  :class="{ 'field-focused': focusedField === 'username' }" @focus="focusedField = 'username'"
                  @blur="focusedField = ''" hide-details="auto" autocomplete="username" :disabled="loading">
                  <template #prepend-inner>
                    <v-icon size="18" class="field-icon">mdi-account-outline</v-icon>
                  </template>
                </v-text-field>
              </div>

              <!-- Password -->
              <div class="field-wrap mb-5">
                <label class="field-label">رمز عبور</label>
                <v-text-field v-model="form.password" :rules="[rules.required]" :type="showPass ? 'text' : 'password'"
                  placeholder="رمز عبور خود را وارد کنید" variant="outlined" density="comfortable" class="login-field"
                  :class="{ 'field-focused': focusedField === 'password' }" @focus="focusedField = 'password'"
                  @blur="focusedField = ''" hide-details="auto" autocomplete="current-password" :disabled="loading">
                  <template #prepend-inner>
                    <v-icon size="18" class="field-icon">mdi-lock-outline</v-icon>
                  </template>
                  <template #append-inner>
                    <v-icon size="18" class="field-icon-btn" @click="showPass = !showPass">
                      {{ showPass ? 'mdi-eye-off-outline' : 'mdi-eye-outline' }}
                    </v-icon>
                  </template>
                </v-text-field>
              </div>

              <!-- Role Select -->
              <div class="field-wrap mb-5">
                <label class="field-label">نقش کاربری</label>
                <v-select v-model="form.roleId" :items="rolesList" :rules="[rules.required]" item-title="name"
                  item-value="id" placeholder="نقش خود را انتخاب کنید" variant="outlined" density="comfortable"
                  class="login-field" :class="{ 'field-focused': focusedField === 'role' }"
                  @focus="focusedField = 'role'" @blur="focusedField = ''" hide-details="auto" :loading="rolesLoading"
                  :disabled="loading || rolesLoading" no-data-text="نقشی یافت نشد">
                  <template #prepend-inner>
                    <v-icon size="18" class="field-icon">mdi-shield-account-outline</v-icon>
                  </template>
                </v-select>
              </div>

              <!-- Remember & Forgot -->
              <div class="d-flex align-center justify-space-between mb-7">
                <v-checkbox v-model="form.remember" label="مرا به خاطر بسپار" density="compact" hide-details
                  class="remember-check" />
                <a href="#" class="forgot-link" @click.prevent="handleForgotPassword">
                  فراموشی رمز عبور
                </a>
              </div>

              <!-- Error alert -->
              <v-alert v-if="errorMsg" type="error" variant="tonal" class="mb-5 login-alert" rounded="lg"
                density="compact" closable @click:close="errorMsg = ''">
                {{ errorMsg }}
              </v-alert>

              <!-- Submit -->
              <v-btn type="submit" block size="large" class="login-btn" :loading="loading" elevation="0" rounded="lg"
                :disabled="!isFormReady">
                <template #loader>
                  <v-progress-circular indeterminate size="20" width="2" color="white" />
                </template>
                <v-icon class="me-2" size="18">mdi-login-variant</v-icon>
                ورود به سیستم
              </v-btn>

            </v-form>

            <!-- Connection status indicator -->
            <div class="connection-status mt-3" :class="connectionStatusClass">
              <span class="status-dot"></span>
              <span class="status-text">{{ connectionStatusText }}</span>
            </div>

            <!-- Divider -->
            <div class="or-divider my-6">
              <div class="or-line" />
              <span class="or-text">یا</span>
              <div class="or-line" />
            </div>

            <!-- Register link -->
            <v-btn block variant="outlined" size="large" class="guest-btn" rounded="lg" elevation="0" to="/register">
              <v-icon class="me-2" size="18">mdi-account-plus-outline</v-icon>
              ثبت‌نام در فینومال
            </v-btn>

            <!-- Footer -->
            <div class="form-footer mt-6">
              <span>نسخه آزمایشی ۱.۰.۰ — </span>
              <a href="#" class="footer-link">تماس با پشتیبانی</a>
            </div>

          </div>
        </v-col>

      </v-row>
    </v-container>
  </v-main>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthHub } from '@/composables/useAuthHub'

// ── Router & Hub ──────────────────────────────────────────────────────────────
const router = useRouter()
const {
  isConnected, isConnecting, connectionError,
  loginResult, rolesList,
  connect, login, fetchRoles,
} = useAuthHub()

// ── Template refs ────────────────────────────────────────────────────────────
const formRef = ref(null)

// ── Form state ────────────────────────────────────────────────────────────────
const form = ref({
  username: '',
  password: '',
  roleId: null,
  remember: false,
})

const showPass = ref(false)
const focusedField = ref('')
const loading = ref(false)
const rolesLoading = ref(false)
const errorMsg = ref('')

// ── Static data ───────────────────────────────────────────────────────────────
const chartBars = [28, 42, 35, 55, 38, 62, 48, 70, 52, 78, 60, 88, 65, 95, 72]

const features = [
  'مدیریت کامل اسناد حسابداری',
  'انبارداری و کنترل موجودی',
  'حقوق و دستمزد پرسنل',
  'گزارش‌های مالی جامع',
  'سازگار با سامانه مودیان',
]

// ── Validation rules ──────────────────────────────────────────────────────────
const rules = {
  required: (v) => !!v?.toString().trim() || 'این فیلد الزامی است',
}

// ── Computed ──────────────────────────────────────────────────────────────────
const isFormReady = computed(() =>
  form.value.username.trim() &&
  form.value.password.trim() &&
  form.value.roleId !== null &&
  !loading.value
)

const connectionStatusClass = computed(() => {
  if (isConnecting.value) return 'status-connecting'
  if (isConnected.value) return 'status-ok'
  return 'status-err'
})

const connectionStatusText = computed(() => {
  if (isConnecting.value) return 'در حال اتصال...'
  if (isConnected.value) return 'متصل به سرور'
  if (connectionError.value) return 'خطا در اتصال'
  return 'قطع اتصال'
})

// ── Watch login result from hub ───────────────────────────────────────────────
watch(loginResult, (result) => {
  if (!result) return
  loading.value = false

  if (result.isSuccess) {
    _persistSession(result.data ?? result.value ?? result)
    router.push('/')
  } else {
    errorMsg.value = result.message ?? result.errors?.[0]?.message ?? 'نام کاربری یا رمز عبور اشتباه است'
  }
})

// ── Lifecycle ────────────────────────────────────────────────────────────────
onMounted(async () => {
  await _loadRoles()
  _restoreRemembered()
})

// ── Handlers ─────────────────────────────────────────────────────────────────
async function handleLogin() {
  const { valid } = await formRef.value.validate()
  if (!valid || !isFormReady.value) return

  loading.value = true
  errorMsg.value = ''

  try {
    await login({
      userName: form.value.username,
      password: form.value.password,
      roleId: form.value.roleId,
    })
    // پاسخ از طریق watch(loginResult) دریافت می‌شود
  } catch (err) {
    loading.value = false
    errorMsg.value = _mapError(err)
  }
}

function handleForgotPassword() {
  router.push('/forgot-password')
}

// ── Private helpers ───────────────────────────────────────────────────────────
async function _loadRoles() {
  if (rolesList.value.length > 0) return
  rolesLoading.value = true
  try {
    await fetchRoles()
  } catch (err) {
    errorMsg.value = 'خطا در بارگذاری نقش‌ها — صفحه را رفرش کنید'
  } finally {
    rolesLoading.value = false
  }
}

function _persistSession(data) {
  const storage = form.value.remember ? localStorage : sessionStorage

  storage.setItem('jwt', data.token ?? '')
  storage.setItem('userId', data.userId ?? '')
  storage.setItem('userName', data.userName ?? form.value.username)
  storage.setItem('userRole', data.roleId ?? form.value.roleId)
  storage.setItem('tokenTime', Date.now().toString())

  if (data.refreshToken) {
    storage.setItem('refreshToken', data.refreshToken)
    storage.setItem('refreshTokenExpiry', data.refreshTokenExpiry ?? '')
  }

  if (form.value.remember) {
    localStorage.setItem('rememberedUser', form.value.username)
  } else {
    localStorage.removeItem('rememberedUser')
  }
}

function _restoreRemembered() {
  const saved = localStorage.getItem('rememberedUser')
  if (saved) {
    form.value.username = saved
    form.value.remember = true
  }
}

function _mapError(err) {
  const msg = err?.message ?? ''
  if (msg.includes('401')) return 'نام کاربری یا رمز عبور اشتباه است'
  if (msg.includes('403')) return 'دسترسی غیرمجاز'
  if (msg.includes('Timeout')) return 'سرور پاسخ نمی‌دهد — دوباره تلاش کنید'
  return 'خطا در ارتباط با سرور'
}
</script>

<style scoped>
/* ===== Base ===== */
.login-app {
  font-family: "Vazirmatn", "Tahoma", sans-serif;
  position: relative;
  overflow: hidden;
}

/* ===== Background ===== */
.bg-layer {
  position: fixed;
  inset: 0;
  pointer-events: none;
  z-index: 0;
}

.bg-base {
  background: #070b12;
}

.bg-grid {
  background-image:
    linear-gradient(rgba(56, 189, 248, 0.03) 1px, transparent 1px),
    linear-gradient(90deg, rgba(56, 189, 248, 0.03) 1px, transparent 1px);
  background-size: 48px 48px;
}

.bg-glow-1 {
  background: radial-gradient(ellipse 60% 50% at 20% 50%, rgba(56, 189, 248, 0.07) 0%, transparent 70%);
}

.bg-glow-2 {
  background: radial-gradient(ellipse 40% 40% at 80% 80%, rgba(99, 102, 241, 0.06) 0%, transparent 70%);
}

/* ===== Geometric shapes ===== */
.geo {
  position: fixed;
  pointer-events: none;
  z-index: 0;
  border: 1px solid rgba(56, 189, 248, 0.07);
  border-radius: 4px;
}

.geo-1 {
  width: 160px;
  height: 160px;
  top: 8%;
  left: 10%;
  transform: rotate(15deg);
  animation: geoFloat 8s ease-in-out infinite;
}

.geo-2 {
  width: 80px;
  height: 80px;
  top: 30%;
  left: 5%;
  transform: rotate(-20deg);
  animation: geoFloat 11s ease-in-out infinite reverse;
  border-color: rgba(99, 102, 241, 0.08);
}

.geo-3 {
  width: 120px;
  height: 120px;
  bottom: 15%;
  left: 18%;
  transform: rotate(35deg);
  animation: geoFloat 9s ease-in-out infinite 2s;
}

.geo-4 {
  width: 48px;
  height: 48px;
  top: 15%;
  left: 40%;
  transform: rotate(10deg);
  animation: geoFloat 7s ease-in-out infinite 1s;
  border-color: rgba(56, 189, 248, 0.05);
}

@keyframes geoFloat {

  0%,
  100% {
    transform: translateY(0) rotate(var(--r, 15deg));
  }

  50% {
    transform: translateY(-16px) rotate(var(--r, 15deg));
  }
}

/* ===== Main ===== */
.login-main {
  position: relative;
  z-index: 1;
}

/* ===== Brand Panel ===== */
.brand-panel {
  position: relative;
  background: linear-gradient(135deg, rgba(56, 189, 248, 0.04) 0%, rgba(99, 102, 241, 0.03) 100%);
  border-left: 1px solid rgba(255, 255, 255, 0.05);
  overflow: hidden;
}

.brand-panel::before {
  content: '';
  position: absolute;
  top: -100px;
  right: -100px;
  width: 400px;
  height: 400px;
  border-radius: 50%;
  background: radial-gradient(circle, rgba(56, 189, 248, 0.06) 0%, transparent 70%);
  pointer-events: none;
}

.brand-logo-wrap {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 64px;
  height: 64px;
  border-radius: 18px;
  background: linear-gradient(135deg, rgba(56, 189, 248, 0.15), rgba(99, 102, 241, 0.1));
  border: 1px solid rgba(56, 189, 248, 0.2);
  box-shadow: 0 0 30px rgba(56, 189, 248, 0.15);
}

.brand-logo-icon {
  color: #38bdf8 !important;
  filter: drop-shadow(0 0 8px rgba(56, 189, 248, 0.6));
}

.brand-name {
  font-size: 32px;
  font-weight: 800;
  color: #e2e8f0;
  letter-spacing: -0.5px;
}

.brand-name-sm {
  font-size: 22px;
  font-weight: 800;
  color: #e2e8f0;
}

.brand-tagline {
  font-size: 14px;
  color: #64748b;
  margin-top: 4px;
}

.stats-card {
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.07);
  border-radius: 16px;
  padding: 20px 24px;
  backdrop-filter: blur(10px);
}

.stats-card-header {
  font-size: 12px;
  color: #64748b;
  margin-bottom: 16px;
  text-align: right;
}

.stats-row {
  display: flex;
  align-items: center;
  gap: 16px;
  margin-bottom: 20px;
  direction: rtl;
}

.stats-item {
  flex: 1;
  text-align: center;
}

.stats-val {
  font-size: 22px;
  font-weight: 700;
  color: #e2e8f0;
}

.stats-val.positive {
  color: #4ade80;
}

.stats-val.accent-val {
  color: #38bdf8;
}

.stats-lbl {
  font-size: 11px;
  color: #475569;
  margin-top: 4px;
}

.stats-sep {
  width: 1px;
  height: 40px;
  background: rgba(255, 255, 255, 0.07);
  flex-shrink: 0;
}

.mini-chart {
  display: flex;
  align-items: flex-end;
  gap: 4px;
  height: 60px;
  direction: ltr;
}

.mini-bar {
  flex: 1;
  background: rgba(56, 189, 248, 0.2);
  border-radius: 3px 3px 0 0;
  animation: barGrow 0.6s ease-out both;
}

.mini-bar.active {
  background: #38bdf8;
  box-shadow: 0 0 8px rgba(56, 189, 248, 0.4);
}

@keyframes barGrow {
  from {
    transform: scaleY(0);
    transform-origin: bottom;
  }

  to {
    transform: scaleY(1);
    transform-origin: bottom;
  }
}

.feature-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.feature-item {
  display: flex;
  align-items: center;
  font-size: 13px;
  color: #94a3b8;
  direction: rtl;
}

.brand-bottom .copyright {
  font-size: 11px;
  color: #334155;
}

/* ===== Form Panel ===== */
.form-panel {
  background: rgba(7, 11, 18, 0.6);
}

.login-card {
  width: 100%;
  max-width: 420px;
  animation: cardIn 0.5s ease-out both;
}

@keyframes cardIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }

  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.form-heading {
  text-align: right;
}

.form-title {
  font-size: 28px;
  font-weight: 700;
  color: #e2e8f0;
  line-height: 1.2;
}

.form-subtitle {
  font-size: 13px;
  color: #64748b;
  margin-top: 6px;
}

.field-label {
  display: block;
  font-size: 12px;
  font-weight: 600;
  color: #64748b;
  margin-bottom: 8px;
  text-align: right;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif;
}

:deep(.login-field .v-field) {
  background: rgba(255, 255, 255, 0.03) !important;
  border-radius: 10px !important;
  transition: all 0.2s !important;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif !important;
}

:deep(.login-field.field-focused .v-field) {
  background: rgba(56, 189, 248, 0.05) !important;
  box-shadow: 0 0 0 2px rgba(56, 189, 248, 0.2) !important;
}

:deep(.login-field .v-field__input),
:deep(.login-field input) {
  font-family: 'Vazirmatn', 'Tahoma', sans-serif !important;
  font-size: 13px !important;
  color: #e2e8f0 !important;
  direction: rtl !important;
  text-align: right !important;
}

:deep(.login-field input::placeholder) {
  color: #334155 !important;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif !important;
}

.field-icon {
  color: #475569 !important;
  margin-left: 6px;
}

.field-icon-btn {
  color: #475569 !important;
  cursor: pointer;
  transition: color 0.2s;
  margin-right: 4px;
}

.field-icon-btn:hover {
  color: #94a3b8 !important;
}

:deep(.remember-check .v-label) {
  font-family: 'Vazirmatn', 'Tahoma', sans-serif !important;
  font-size: 12px !important;
  color: #64748b !important;
  opacity: 1 !important;
}

.forgot-link {
  font-size: 12px;
  color: #38bdf8;
  text-decoration: none;
  transition: color 0.2s;
}

.forgot-link:hover {
  color: #7dd3fc;
}

:deep(.login-alert) {
  font-family: 'Vazirmatn', 'Tahoma', sans-serif !important;
  font-size: 13px !important;
  direction: rtl !important;
}

.login-btn {
  background: linear-gradient(135deg, #0ea5e9, #6366f1) !important;
  color: #fff !important;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif !important;
  font-size: 15px !important;
  font-weight: 700 !important;
  letter-spacing: 0 !important;
  height: 52px !important;
  box-shadow: 0 4px 24px rgba(14, 165, 233, 0.3) !important;
  transition: all 0.2s !important;
}

.login-btn:hover:not(:disabled) {
  box-shadow: 0 6px 32px rgba(14, 165, 233, 0.45) !important;
  transform: translateY(-1px);
}

/* ── Connection status ── */
.connection-status {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 11px;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif;
  justify-content: center;
}

.status-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: currentColor;
  animation: blink 1.8s infinite;
}

@keyframes blink {

  0%,
  100% {
    opacity: 1
  }

  50% {
    opacity: 0.3
  }
}

.status-ok {
  color: #4ade80;
}

.status-connecting {
  color: #fbbf24;
}

.status-err {
  color: #f87171;
}

.status-text {
  color: inherit;
}

.or-divider {
  display: flex;
  align-items: center;
  gap: 12px;
}

.or-line {
  flex: 1;
  height: 1px;
  background: rgba(255, 255, 255, 0.07);
}

.or-text {
  font-size: 12px;
  color: #334155;
  flex-shrink: 0;
}

.guest-btn {
  border-color: rgba(255, 255, 255, 0.1) !important;
  color: #64748b !important;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif !important;
  font-size: 14px !important;
  font-weight: 600 !important;
  letter-spacing: 0 !important;
  height: 48px !important;
  transition: all 0.2s !important;
}

.guest-btn:hover {
  border-color: rgba(56, 189, 248, 0.3) !important;
  color: #94a3b8 !important;
  background: rgba(56, 189, 248, 0.05) !important;
}

.form-footer {
  font-size: 11px;
  color: #334155;
  text-align: center;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif;
}

.footer-link {
  color: #475569;
  text-decoration: none;
  transition: color 0.2s;
}

.footer-link:hover {
  color: #64748b;
}
</style>
