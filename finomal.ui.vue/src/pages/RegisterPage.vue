<template>
  <div class="register-root">
    <div class="bg-grid"></div>
    <div class="bg-orb orb-tl"></div>
    <div class="bg-orb orb-br"></div>

    <div class="register-layout">
      <!-- ===== Brand Panel ===== -->
      <aside class="brand-panel">
        <div class="brand-content">
          <div class="brand-logo">
            <svg width="32" height="32" viewBox="0 0 40 40" fill="none">
              <rect width="40" height="40" rx="12" fill="rgba(56,189,248,0.15)" stroke="rgba(56,189,248,0.3)"
                stroke-width="1" />
              <path d="M10 28L20 12L30 28" stroke="#38bdf8" stroke-width="2.5" stroke-linecap="round"
                stroke-linejoin="round" />
              <path d="M14 22h12" stroke="#38bdf8" stroke-width="2" stroke-linecap="round" opacity="0.5" />
            </svg>
            <span class="brand-name">فینومال</span>
          </div>

          <div class="brand-headline">
            <h2>سیستم یکپارچه<br>مدیریت مالی</h2>
            <p>با ثبت‌نام در فینومال، کنترل کامل امور مالی کسب‌وکار خود را به دست بگیرید.</p>
          </div>

          <div class="brand-features">
            <div v-for="f in features" :key="f.text" class="feature-item">
              <div class="feature-icon">
                <component :is="f.icon" />
              </div>
              <span>{{ f.text }}</span>
            </div>
          </div>

          <div class="brand-footer">
            <span>نسخه ۱.۰.۰</span>
            <span class="dot-sep">·</span>
            <span>Finomal © ۱۴۰۳</span>
          </div>
        </div>
      </aside>

      <!-- ===== Form Panel ===== -->
      <main class="form-panel">
        <div class="form-box">

          <div class="form-header">
            <div class="form-tag">ثبت‌نام</div>
            <h1 class="form-title">ایجاد حساب کاربری</h1>
            <p class="form-subtitle">اطلاعات زیر را با دقت وارد کنید</p>
          </div>

          <!-- Steps -->
          <div class="steps-bar">
            <div v-for="(step, i) in STEPS" :key="i" class="step"
              :class="{ 'step-done': currentStep > i, 'step-active': currentStep === i }">
              <div class="step-circle">
                <svg v-if="currentStep > i" width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                  stroke-width="3">
                  <polyline points="20 6 9 17 4 12" />
                </svg>
                <span v-else>{{ i + 1 }}</span>
              </div>
              <span class="step-label">{{ step }}</span>
              <div v-if="i < STEPS.length - 1" class="step-connector" :class="{ 'connector-done': currentStep > i }" />
            </div>
          </div>

          <!-- Form body -->
          <form @submit.prevent="handleSubmit" novalidate>

            <transition name="slide-fade" mode="out-in">

              <!-- Step 0: Personal info -->
              <div v-if="currentStep === 0" key="s0" class="step-fields">
                <div class="field-row">
                  <div class="field-group">
                    <label class="f-label">نام</label>
                    <div class="f-input-wrap" :class="fieldClass('firstName')">
                      <input v-model="form.firstName" type="text" class="f-input" placeholder="نام"
                        @focus="focused = 'firstName'" @blur="onBlur('firstName')" />
                    </div>
                    <span v-if="errors.firstName" class="f-error">{{ errors.firstName }}</span>
                  </div>

                  <div class="field-group">
                    <label class="f-label">نام خانوادگی</label>
                    <div class="f-input-wrap" :class="fieldClass('lastName')">
                      <input v-model="form.lastName" type="text" class="f-input" placeholder="نام خانوادگی"
                        @focus="focused = 'lastName'" @blur="onBlur('lastName')" />
                    </div>
                    <span v-if="errors.lastName" class="f-error">{{ errors.lastName }}</span>
                  </div>
                </div>

                <div class="field-group">
                  <label class="f-label">ایمیل</label>
                  <div class="f-input-wrap" :class="fieldClass('email')">
                    <input v-model="form.email" type="email" class="f-input ltr-input" placeholder="example@email.com"
                      dir="ltr" @focus="focused = 'email'" @blur="onBlur('email')" />
                  </div>
                  <span v-if="errors.email" class="f-error">{{ errors.email }}</span>
                </div>
              </div>

              <!-- Step 1: Credentials -->
              <div v-else-if="currentStep === 1" key="s1" class="step-fields">
                <div class="field-group">
                  <label class="f-label">نام کاربری</label>
                  <div class="f-input-wrap" :class="fieldClass('userName')">
                    <input v-model="form.userName" type="text" class="f-input ltr-input" placeholder="username"
                      dir="ltr" @focus="focused = 'userName'" @blur="onBlur('userName')" />
                    <div class="input-suffix" :class="usernameSuffixClass">
                      <div class="suffix-spinner" v-if="checkingUsername" />
                      <svg v-else-if="usernameAvailable === true" width="13" height="13" viewBox="0 0 24 24" fill="none"
                        stroke="currentColor" stroke-width="2.5">
                        <polyline points="20 6 9 17 4 12" />
                      </svg>
                      <svg v-else-if="usernameAvailable === false" width="13" height="13" viewBox="0 0 24 24"
                        fill="none" stroke="currentColor" stroke-width="2.5">
                        <line x1="18" y1="6" x2="6" y2="18" />
                        <line x1="6" y1="6" x2="18" y2="18" />
                      </svg>
                      <div v-else class="input-dot" />
                    </div>
                  </div>
                  <span v-if="errors.userName" class="f-error">{{ errors.userName }}</span>
                  <span v-else-if="usernameAvailable === false" class="f-error">این نام کاربری قبلاً استفاده شده</span>
                </div>

                <div class="field-group">
                  <label class="f-label">رمز عبور</label>
                  <div class="f-input-wrap" :class="fieldClass('password')">
                    <input v-model="form.password" :type="showPassword ? 'text' : 'password'" class="f-input ltr-input"
                      placeholder="حداقل ۸ کاراکتر" dir="ltr" @focus="focused = 'password'"
                      @blur="onBlur('password')" />
                    <button type="button" class="toggle-pass" @click="showPassword = !showPassword" tabindex="-1">
                      <svg v-if="!showPassword" width="14" height="14" viewBox="0 0 24 24" fill="none"
                        stroke="currentColor" stroke-width="2">
                        <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
                        <circle cx="12" cy="12" r="3" />
                      </svg>
                      <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                        stroke-width="2">
                        <path
                          d="M17.94 17.94A10.07 10.07 0 0112 20c-7 0-11-8-11-8a18.45 18.45 0 015.06-5.94M9.9 4.24A9.12 9.12 0 0112 4c7 0 11 8 11 8a18.5 18.5 0 01-2.16 3.19m-6.72-1.07a3 3 0 11-4.24-4.24" />
                        <line x1="1" y1="1" x2="23" y2="23" />
                      </svg>
                    </button>
                  </div>
                  <div class="pass-strength">
                    <div class="strength-bars">
                      <div v-for="i in 4" :key="i" class="strength-bar"
                        :class="passwordStrength >= i ? `str-${passwordStrength}` : ''" />
                    </div>
                    <span class="strength-label" :class="`stl-${passwordStrength}`">{{ STRENGTH_LABELS[passwordStrength]
                      }}</span>
                  </div>
                  <span v-if="errors.password" class="f-error">{{ errors.password }}</span>
                </div>

                <div class="field-group">
                  <label class="f-label">تکرار رمز عبور</label>
                  <div class="f-input-wrap" :class="fieldClass('confirmPassword')">
                    <input v-model="form.confirmPassword" :type="showPassword ? 'text' : 'password'"
                      class="f-input ltr-input" placeholder="رمز عبور را تکرار کنید" dir="ltr"
                      @focus="focused = 'confirmPassword'" @blur="onBlur('confirmPassword')" />
                  </div>
                  <span v-if="errors.confirmPassword" class="f-error">{{ errors.confirmPassword }}</span>
                </div>
              </div>

              <!-- Step 2: Role + Review -->
              <div v-else-if="currentStep === 2" key="s2" class="step-fields">
                <div class="field-group">
                  <label class="f-label">نقش کاربری</label>
                  <div v-if="rolesLoading" class="roles-loading">
                    <div class="spinner-sm" /><span>در حال بارگذاری نقش‌ها...</span>
                  </div>
                  <div v-else class="roles-grid">
                    <div v-for="role in rolesList" :key="role.id" class="role-card"
                      :class="{ 'role-selected': form.roleId === role.id }"
                      @click="form.roleId = role.id; errors.roleId = ''">
                      <div class="role-icon">
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                          stroke-width="2">
                          <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
                        </svg>
                      </div>
                      <div class="role-info">
                        <span class="role-name">{{ role.name }}</span>
                      </div>
                      <div class="role-check">
                        <svg v-if="form.roleId === role.id" width="11" height="11" viewBox="0 0 24 24" fill="none"
                          stroke="currentColor" stroke-width="3">
                          <polyline points="20 6 9 17 4 12" />
                        </svg>
                      </div>
                    </div>
                  </div>
                  <span v-if="errors.roleId" class="f-error">{{ errors.roleId }}</span>
                </div>

                <div class="review-box">
                  <div class="review-title">خلاصه اطلاعات</div>
                  <div class="review-grid">
                    <div class="review-row"><span class="rv-key">نام کامل</span><span class="rv-val">{{ form.firstName
                        }} {{ form.lastName }}</span></div>
                    <div class="review-row"><span class="rv-key">ایمیل</span><span class="rv-val ltr-text">{{ form.email
                        }}</span></div>
                    <div class="review-row"><span class="rv-key">نام کاربری</span><span class="rv-val ltr-text">{{
                        form.userName }}</span></div>
                    <div class="review-row"><span class="rv-key">نقش</span><span class="rv-val">{{ selectedRoleName
                        }}</span></div>
                  </div>
                </div>
              </div>

              <!-- Step 3: Success -->
              <div v-else-if="currentStep === 3" key="s3" class="success-state">
                <div class="success-anim">
                  <svg class="success-ring" width="72" height="72" viewBox="0 0 72 72">
                    <circle cx="36" cy="36" r="32" fill="none" stroke="rgba(74,222,128,0.15)" stroke-width="2" />
                    <circle class="ring-progress" cx="36" cy="36" r="32" fill="none" stroke="#4ade80" stroke-width="2"
                      stroke-dasharray="201" stroke-dashoffset="0" stroke-linecap="round" />
                    <polyline points="22,36 32,46 50,26" fill="none" stroke="#4ade80" stroke-width="3"
                      stroke-linecap="round" stroke-linejoin="round" />
                  </svg>
                </div>
                <h3 class="success-title">ثبت‌نام با موفقیت انجام شد!</h3>
                <p class="success-body">حساب کاربری شما ایجاد شده اما هنوز فعال نیست. پس از تأیید توسط مدیر سیستم
                  می‌توانید وارد شوید.</p>
                <div class="success-meta">
                  <div class="meta-item">
                    <span class="meta-key">کد کاربری</span>
                    <span class="meta-val ltr-text">{{ shortUserId }}</span>
                  </div>
                  <div class="meta-item">
                    <span class="meta-key">نام کاربری</span>
                    <span class="meta-val ltr-text">{{ registerResult?.userName }}</span>
                  </div>
                </div>
                <button type="button" class="btn-login" @click="router.push('/login')">
                  رفتن به صفحه ورود
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                    <path d="M5 12h14M12 5l7 7-7 7" />
                  </svg>
                </button>
              </div>

            </transition>

            <!-- Navigation buttons -->
            <div v-if="currentStep < 3" class="form-nav">
              <button v-if="currentStep > 0" type="button" class="btn-back" @click="currentStep--">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                  <path d="M19 12H5M12 19l-7-7 7-7" />
                </svg>
                مرحله قبل
              </button>
              <div v-else class="nav-spacer" />

              <button v-if="currentStep < 2" type="button" class="btn-next" @click="goNext">
                مرحله بعد
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                  <path d="M5 12h14M12 5l7 7-7 7" />
                </svg>
              </button>

              <button v-else type="submit" class="btn-submit" :disabled="loading || !canSubmit">
                <span v-if="!loading">
                  ثبت‌نام
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                    <polyline points="20 6 9 17 4 12" />
                  </svg>
                </span>
                <span v-else class="spinner" />
              </button>
            </div>

          </form>

          <div v-if="currentStep < 3" class="login-link">
            قبلاً ثبت‌نام کرده‌اید؟
            <a href="#" @click.prevent="router.push('/login')">وارد شوید</a>
          </div>
        </div>
      </main>
    </div>

    <!-- Toast notification -->
    <transition name="toast-anim">
      <div v-if="toast.show" class="toast" :class="`toast-${toast.type}`">
        <svg v-if="toast.type === 'error'" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
          stroke-width="2.5">
          <circle cx="12" cy="12" r="10" />
          <line x1="12" y1="8" x2="12" y2="12" />
          <line x1="12" y1="16" x2="12.01" y2="16" />
        </svg>
        {{ toast.text }}
      </div>
    </transition>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthHub } from '@/composables/useAuthHub'

// ── Router & Hub ──────────────────────────────────────────────────────────────
const router = useRouter()
const {
  rolesList,
  registrationResult,
  fetchRoles,
  register,
  checkUsernameAvailability,
} = useAuthHub()

// ── Constants ────────────────────────────────────────────────────────────────
const STEPS = ['اطلاعات شخصی', 'اطلاعات ورود', 'نقش و تأیید']

const STRENGTH_LABELS = { 0: '', 1: 'ضعیف', 2: 'متوسط', 3: 'خوب', 4: 'عالی' }

// ── Form state ────────────────────────────────────────────────────────────────
const form = ref({
  firstName: '',
  lastName: '',
  email: '',
  userName: '',
  password: '',
  confirmPassword: '',
  roleId: null,
})

const errors = ref({})
const focused = ref(null)
const currentStep = ref(0)
const loading = ref(false)
const rolesLoading = ref(false)
const showPassword = ref(false)
const checkingUsername = ref(false)
const usernameAvailable = ref(null)
const registerResult = ref(null)
const toast = ref({ show: false, text: '', type: 'error' })

// ── Username check debounce timer ─────────────────────────────────────────────
let usernameTimer = null

// ── Static data ───────────────────────────────────────────────────────────────
const features = [
  { text: 'مدیریت اسناد حسابداری', icon: { template: `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/><polyline points="14 2 14 8 20 8"/></svg>` } },
  { text: 'گزارش‌های مالی آنلاین', icon: { template: `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><line x1="18" y1="20" x2="18" y2="10"/><line x1="12" y1="20" x2="12" y2="4"/><line x1="6" y1="20" x2="6" y2="14"/></svg>` } },
  { text: 'داشبورد مدیریتی پیشرفته', icon: { template: `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="3" width="7" height="7" rx="1"/><rect x="14" y="3" width="7" height="7" rx="1"/><rect x="3" y="14" width="7" height="7" rx="1"/><rect x="14" y="14" width="7" height="7" rx="1"/></svg>` } },
  { text: 'امنیت در سطح سازمانی', icon: { template: `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="11" width="18" height="11" rx="2"/><path d="M7 11V7a5 5 0 0110 0v4"/></svg>` } },
]

// ── Computed ──────────────────────────────────────────────────────────────────
const passwordStrength = computed(() => {
  const p = form.value.password
  if (!p) return 0
  return [p.length >= 8, /[A-Z]/.test(p), /[0-9]/.test(p), /[^A-Za-z0-9]/.test(p)]
    .filter(Boolean).length
})

const canSubmit = computed(() =>
  form.value.roleId !== null && usernameAvailable.value !== false
)

const selectedRoleName = computed(() =>
  rolesList.value.find((r) => r.id === form.value.roleId)?.name ?? '—'
)

const shortUserId = computed(() => {
  const id = registerResult.value?.userId
  return id ? id.slice(0, 8) + '...' : '—'
})

const usernameSuffixClass = computed(() => {
  if (checkingUsername.value) return 'suf-checking'
  if (usernameAvailable.value === true) return 'suf-ok'
  if (usernameAvailable.value === false) return 'suf-err'
  return ''
})

// ── Field helpers ─────────────────────────────────────────────────────────────
function fieldClass(name) {
  return {
    'is-focused': focused.value === name,
    'has-error': !!errors.value[name],
  }
}

function onBlur(name) {
  focused.value = null
  validateField(name)
}

// ── Watchers ──────────────────────────────────────────────────────────────────

// بررسی نام کاربری با debounce
watch(() => form.value.userName, (val) => {
  usernameAvailable.value = null
  clearTimeout(usernameTimer)
  if (val.length < 3) return

  usernameTimer = setTimeout(async () => {
    checkingUsername.value = true
    try {
      usernameAvailable.value = await checkUsernameAvailability(val)
    } finally {
      checkingUsername.value = false
    }
  }, 600)
})

// وقتی سرور پاسخ ثبت‌نام را فرستاد — دقیقاً مثل watch(loginResult) در LoginView
watch(registrationResult, (result) => {
  if (!result) return
  loading.value = false

  if (result.isSuccess) {
    registerResult.value = result.data
    currentStep.value = 3
  } else {
    showToast(result.message ?? 'خطا در ثبت‌نام', 'error')
  }
})

// ── Lifecycle ────────────────────────────────────────────────────────────────
onMounted(_loadRoles)

// ── Navigation ────────────────────────────────────────────────────────────────
function goNext() {
  if (validateStep(currentStep.value)) currentStep.value++
}

// ── Submit ────────────────────────────────────────────────────────────────────
async function handleSubmit() {
  if (!validateStep(2) || !canSubmit.value) return

  loading.value = true

  try {
    // دقیقاً مثل Login — invoke به Hub، پاسخ از طریق watch(registrationResult) می‌رسد
    await register({
      userName: form.value.userName,
      password: form.value.password,
      email: form.value.email,
      firstName: form.value.firstName,
      lastName: form.value.lastName,
      roleId: form.value.roleId,
    })
  } catch (err) {
    loading.value = false
    showToast(_mapError(err), 'error')
  }
}

function _mapError(err) {
  const msg = err?.message ?? ''
  if (msg.includes('Timeout')) return 'سرور پاسخ نمی‌دهد — دوباره تلاش کنید'
  return 'خطا در اتصال به سرور'
}

// ── Private helpers ───────────────────────────────────────────────────────────
async function _loadRoles() {
  if (rolesList.value.length > 0) return
  rolesLoading.value = true
  try {
    await fetchRoles()
  } catch {
    showToast('خطا در بارگذاری نقش‌ها', 'error')
  } finally {
    rolesLoading.value = false
  }
}

function showToast(text, type = 'error') {
  toast.value = { show: true, text, type }
  setTimeout(() => { toast.value.show = false }, 4000)
}

// ── Validation ────────────────────────────────────────────────────────────────
const VALIDATORS = {
  firstName: (v) => !v.firstName.trim() ? 'نام الزامی است' : '',
  lastName: (v) => !v.lastName.trim() ? 'نام خانوادگی الزامی است' : '',
  email: (v) => !v.email ? 'ایمیل الزامی است'
    : !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v.email) ? 'فرمت ایمیل صحیح نیست' : '',
  userName: (v) => !v.userName ? 'نام کاربری الزامی است'
    : v.userName.length < 3 ? 'حداقل ۳ کاراکتر'
      : !/^[a-zA-Z0-9_]+$/.test(v.userName) ? 'فقط حروف انگلیسی، عدد و _' : '',
  password: (v) => !v.password ? 'رمز عبور الزامی است'
    : v.password.length < 8 ? 'حداقل ۸ کاراکتر' : '',
  confirmPassword: (v) => v.confirmPassword !== v.password ? 'رمزهای عبور مطابقت ندارند' : '',
  roleId: (v) => !v.roleId ? 'یک نقش انتخاب کنید' : '',
}

function validateField(field) {
  errors.value[field] = VALIDATORS[field]?.(form.value) ?? ''
}

const STEP_FIELDS = {
  0: ['firstName', 'lastName', 'email'],
  1: ['userName', 'password', 'confirmPassword'],
  2: ['roleId'],
}

function validateStep(step) {
  STEP_FIELDS[step]?.forEach(validateField)
  return STEP_FIELDS[step]?.every((f) => !errors.value[f]) ?? true
}
</script>

<style scoped>
.register-root {
  min-height: 100vh;
  height: 100%;
  background: #05070d;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif;
  direction: rtl;
  overflow: hidden;
  position: relative;
}

.bg-grid {
  position: fixed;
  inset: 0;
  background-image: linear-gradient(rgba(255, 255, 255, 0.018) 1px, transparent 1px), linear-gradient(90deg, rgba(255, 255, 255, 0.018) 1px, transparent 1px);
  background-size: 36px 36px;
  pointer-events: none;
  z-index: 0;
}

.bg-orb {
  position: fixed;
  border-radius: 50%;
  filter: blur(90px);
  pointer-events: none;
  z-index: 0;
}

.orb-tl {
  width: 600px;
  height: 600px;
  background: radial-gradient(circle, rgba(56, 189, 248, 0.06), transparent 70%);
  top: -200px;
  right: -100px;
}

.orb-br {
  width: 500px;
  height: 500px;
  background: radial-gradient(circle, rgba(167, 139, 250, 0.05), transparent 70%);
  bottom: -150px;
  left: -80px;
}

.register-layout {
  position: relative;
  z-index: 1;
  display: flex;
  height: 100vh;
}

/* Brand */
.brand-panel {
  width: 380px;
  flex-shrink: 0;
  background: rgba(8, 12, 22, 0.9);
  border-left: 1px solid rgba(255, 255, 255, 0.05);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40px;
  backdrop-filter: blur(8px);
}

.brand-content {
  display: flex;
  flex-direction: column;
  gap: 32px;
  max-width: 280px;
}

.brand-logo {
  display: flex;
  align-items: center;
  gap: 12px;
}

.brand-name {
  font-size: 22px;
  font-weight: 900;
  color: #e2e8f0;
  font-family: 'Vazirmatn', sans-serif;
  letter-spacing: -0.5px;
}

.brand-headline h2 {
  font-size: 24px;
  font-weight: 800;
  color: #e2e8f0;
  margin: 0 0 12px;
  line-height: 1.5;
  font-family: 'Vazirmatn', sans-serif;
}

.brand-headline p {
  font-size: 13px;
  color: #475569;
  line-height: 1.8;
  margin: 0;
  font-family: 'Vazirmatn', sans-serif;
}

.brand-features {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.feature-item {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 12.5px;
  color: #64748b;
  font-family: 'Vazirmatn', sans-serif;
}

.feature-icon {
  width: 30px;
  height: 30px;
  border-radius: 8px;
  background: rgba(56, 189, 248, 0.08);
  border: 1px solid rgba(56, 189, 248, 0.15);
  display: flex;
  align-items: center;
  justify-content: center;
  color: #38bdf8;
  flex-shrink: 0;
}

.brand-footer {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 11px;
  color: #253347;
  font-family: 'Vazirmatn', sans-serif;
}

.dot-sep {
  opacity: 0.4;
}

/* Form panel */
.form-panel {
  flex: 1;
  overflow-y: auto;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 32px 24px;
  scrollbar-width: thin;
  scrollbar-color: rgba(255, 255, 255, 0.06) transparent;
}

.form-box {
  width: 100%;
  max-width: 480px;
  animation: fadeUp 0.4s ease both;
}

@keyframes fadeUp {
  from {
    opacity: 0;
    transform: translateY(16px)
  }

  to {
    opacity: 1;
    transform: translateY(0)
  }
}

.form-header {
  margin-bottom: 28px;
}

.form-tag {
  display: inline-flex;
  font-size: 10px;
  font-weight: 800;
  letter-spacing: 1.5px;
  color: #38bdf8;
  background: rgba(56, 189, 248, 0.1);
  border: 1px solid rgba(56, 189, 248, 0.2);
  padding: 3px 10px;
  border-radius: 99px;
  margin-bottom: 12px;
  font-family: 'Vazirmatn', sans-serif;
}

.form-title {
  font-size: 26px;
  font-weight: 900;
  color: #f1f5f9;
  margin: 0 0 6px;
  font-family: 'Vazirmatn', sans-serif;
}

.form-subtitle {
  font-size: 13px;
  color: #475569;
  margin: 0;
  font-family: 'Vazirmatn', sans-serif;
}

/* Steps */
.steps-bar {
  display: flex;
  align-items: center;
  margin-bottom: 28px;
}

.step {
  display: flex;
  align-items: center;
  gap: 8px;
  flex: 1;
}

.step-circle {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 11px;
  font-weight: 700;
  color: #334155;
  flex-shrink: 0;
  transition: all 0.3s;
  font-family: 'Vazirmatn', sans-serif;
}

.step-active .step-circle {
  background: rgba(56, 189, 248, 0.15);
  border-color: rgba(56, 189, 248, 0.4);
  color: #38bdf8;
  box-shadow: 0 0 14px rgba(56, 189, 248, 0.2);
}

.step-done .step-circle {
  background: rgba(74, 222, 128, 0.15);
  border-color: rgba(74, 222, 128, 0.35);
  color: #4ade80;
}

.step-label {
  font-size: 11px;
  color: #253347;
  white-space: nowrap;
  font-family: 'Vazirmatn', sans-serif;
  transition: color 0.3s;
}

.step-active .step-label {
  color: #64748b;
}

.step-done .step-label {
  color: #4ade80;
}

.step-connector {
  flex: 1;
  height: 1px;
  background: rgba(255, 255, 255, 0.06);
  margin: 0 8px;
  transition: background 0.3s;
}

.connector-done {
  background: rgba(74, 222, 128, 0.3);
}

/* Fields */
.step-fields {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.field-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

@media(max-width:480px) {
  .field-row {
    grid-template-columns: 1fr;
  }
}

.field-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.f-label {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 11.5px;
  font-weight: 700;
  color: #475569;
  font-family: 'Vazirmatn', sans-serif;
}

.f-input-wrap {
  position: relative;
  display: flex;
  align-items: center;
  background: rgba(0, 0, 0, 0.4);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 10px;
  transition: border-color 0.2s, box-shadow 0.2s;
}

.f-input-wrap.is-focused {
  border-color: rgba(56, 189, 248, 0.45);
  box-shadow: 0 0 0 3px rgba(56, 189, 248, 0.07);
}

.f-input-wrap.has-error {
  border-color: rgba(248, 113, 113, 0.45);
}

.f-input {
  flex: 1;
  background: none;
  border: none;
  outline: none;
  padding: 11px 14px;
  font-size: 13px;
  color: #cbd5e1;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif;
  width: 100%;
}

.f-input::placeholder {
  color: #334155;
}

.ltr-input {
  text-align: left;
  direction: ltr;
}

.f-error {
  font-size: 11px;
  color: #f87171;
  font-family: 'Vazirmatn', sans-serif;
}

/* Input suffix */
.input-suffix {
  width: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: color 0.2s;
}

.suf-ok {
  color: #4ade80;
}

.suf-err {
  color: #f87171;
}

.suf-checking {
  color: #fbbf24;
}

.input-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: #334155;
}

.suffix-spinner {
  width: 12px;
  height: 12px;
  border: 2px solid rgba(251, 191, 36, 0.3);
  border-top-color: #fbbf24;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}

/* Toggle pass */
.toggle-pass {
  background: none;
  border: none;
  cursor: pointer;
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #334155;
  transition: color 0.2s;
}

.toggle-pass:hover {
  color: #64748b;
}

/* Password strength */
.pass-strength {
  display: flex;
  align-items: center;
  gap: 8px;
}

.strength-bars {
  display: flex;
  gap: 4px;
}

.strength-bar {
  width: 28px;
  height: 3px;
  border-radius: 2px;
  background: rgba(255, 255, 255, 0.06);
  transition: background 0.3s;
}

.str-1 {
  background: #f87171
}

.str-2 {
  background: #fbbf24
}

.str-3 {
  background: #60a5fa
}

.str-4 {
  background: #4ade80
}

.strength-label {
  font-size: 10px;
  font-weight: 700;
  font-family: 'Vazirmatn', sans-serif;
}

.stl-1 {
  color: #f87171
}

.stl-2 {
  color: #fbbf24
}

.stl-3 {
  color: #60a5fa
}

.stl-4 {
  color: #4ade80
}

/* Roles */
.roles-loading {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 12px;
  color: #475569;
  padding: 16px 0;
  font-family: 'Vazirmatn', sans-serif;
}

.spinner-sm {
  width: 14px;
  height: 14px;
  border: 2px solid rgba(56, 189, 248, 0.2);
  border-top-color: #38bdf8;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}

.roles-grid {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.role-card {
  display: flex;
  align-items: center;
  gap: 14px;
  padding: 12px 16px;
  border-radius: 11px;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.07);
  cursor: pointer;
  transition: all 0.2s;
}

.role-card:hover {
  background: rgba(255, 255, 255, 0.04);
  border-color: rgba(255, 255, 255, 0.12);
}

.role-selected {
  background: rgba(56, 189, 248, 0.07) !important;
  border-color: rgba(56, 189, 248, 0.35) !important;
}

.role-icon {
  width: 34px;
  height: 34px;
  border-radius: 9px;
  background: rgba(56, 189, 248, 0.08);
  border: 1px solid rgba(56, 189, 248, 0.15);
  display: flex;
  align-items: center;
  justify-content: center;
  color: #38bdf8;
  flex-shrink: 0;
}

.role-info {
  flex: 1;
}

.role-name {
  font-size: 13px;
  font-weight: 700;
  color: #cbd5e1;
  font-family: 'Vazirmatn', sans-serif;
}

.role-check {
  width: 20px;
  height: 20px;
  border-radius: 50%;
  border: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  color: #38bdf8;
  transition: all 0.2s;
}

.role-selected .role-check {
  background: rgba(56, 189, 248, 0.2);
  border-color: rgba(56, 189, 248, 0.4);
}

.role-selected .role-icon {
  background: rgba(56, 189, 248, 0.15);
}

/* Review */
.review-box {
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.06);
  border-radius: 11px;
  padding: 14px 16px;
}

.review-title {
  font-size: 11px;
  font-weight: 700;
  color: #334155;
  margin-bottom: 10px;
  font-family: 'Vazirmatn', sans-serif;
}

.review-grid {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.review-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.rv-key {
  font-size: 12px;
  color: #334155;
  font-family: 'Vazirmatn', sans-serif;
}

.rv-val {
  font-size: 12px;
  color: #64748b;
  font-family: 'Vazirmatn', sans-serif;
}

.ltr-text {
  direction: ltr;
  text-align: left;
}

/* Navigation */
.form-nav {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: 24px;
  gap: 12px;
}

.nav-spacer {
  flex: 1;
}

.btn-back {
  display: flex;
  align-items: center;
  gap: 7px;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.08);
  color: #475569;
  border-radius: 10px;
  padding: 10px 18px;
  font-size: 13px;
  font-weight: 600;
  font-family: 'Vazirmatn', sans-serif;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-back:hover {
  background: rgba(255, 255, 255, 0.06);
  color: #64748b;
}

.btn-next {
  display: flex;
  align-items: center;
  gap: 7px;
  background: rgba(56, 189, 248, 0.1);
  border: 1px solid rgba(56, 189, 248, 0.25);
  color: #38bdf8;
  border-radius: 10px;
  padding: 10px 22px;
  font-size: 13px;
  font-weight: 700;
  font-family: 'Vazirmatn', sans-serif;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-next:hover {
  background: rgba(56, 189, 248, 0.18);
}

.btn-submit {
  display: flex;
  align-items: center;
  gap: 7px;
  background: linear-gradient(135deg, #0369a1, #0ea5e9);
  border: none;
  color: #fff;
  border-radius: 10px;
  padding: 10px 28px;
  font-size: 13px;
  font-weight: 700;
  font-family: 'Vazirmatn', sans-serif;
  cursor: pointer;
  box-shadow: 0 4px 18px rgba(14, 165, 233, 0.28);
  transition: opacity 0.2s, transform 0.2s;
  min-width: 110px;
  justify-content: center;
}

.btn-submit:hover:not(:disabled) {
  opacity: 0.88;
  transform: translateY(-1px);
}

.btn-submit:disabled {
  background: #1a2235;
  color: #334155;
  box-shadow: none;
  cursor: not-allowed;
}

.spinner {
  width: 16px;
  height: 16px;
  border: 2px solid rgba(255, 255, 255, 0.2);
  border-top-color: #fff;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg)
  }
}

.login-link {
  text-align: center;
  margin-top: 20px;
  font-size: 12.5px;
  color: #334155;
  font-family: 'Vazirmatn', sans-serif;
}

.login-link a {
  color: #38bdf8;
  text-decoration: none;
  font-weight: 700;
}

.login-link a:hover {
  text-decoration: underline;
}

/* Success */
.success-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  padding: 20px 0;
  gap: 16px;
}

.success-anim {
  position: relative;
  width: 72px;
  height: 72px;
}

.success-ring {
  animation: ringIn 0.6s ease both;
}

@keyframes ringIn {
  from {
    opacity: 0;
    transform: scale(0.6)
  }

  to {
    opacity: 1;
    transform: scale(1)
  }
}

.ring-progress {
  transform-origin: 36px 36px;
  animation: strokeIn 0.8s ease 0.3s both;
}

@keyframes strokeIn {
  from {
    stroke-dashoffset: 201
  }

  to {
    stroke-dashoffset: 0
  }
}

.success-title {
  font-size: 20px;
  font-weight: 800;
  color: #e2e8f0;
  margin: 0;
  font-family: 'Vazirmatn', sans-serif;
}

.success-body {
  font-size: 13px;
  color: #475569;
  line-height: 1.7;
  margin: 0;
  max-width: 340px;
  font-family: 'Vazirmatn', sans-serif;
}

.success-meta {
  display: flex;
  gap: 20px;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.06);
  border-radius: 10px;
  padding: 14px 22px;
}

.meta-item {
  display: flex;
  flex-direction: column;
  gap: 3px;
}

.meta-key {
  font-size: 10px;
  color: #334155;
  font-family: 'Vazirmatn', sans-serif;
}

.meta-val {
  font-size: 13px;
  font-weight: 700;
  color: #64748b;
  font-family: 'Vazirmatn', sans-serif;
}

.btn-login {
  display: flex;
  align-items: center;
  gap: 8px;
  background: linear-gradient(135deg, #064e3b, #059669);
  border: none;
  color: #fff;
  border-radius: 10px;
  padding: 11px 24px;
  font-size: 13px;
  font-weight: 700;
  font-family: 'Vazirmatn', sans-serif;
  cursor: pointer;
  box-shadow: 0 4px 16px rgba(5, 150, 105, 0.25);
  transition: opacity 0.2s;
}

.btn-login:hover {
  opacity: 0.88;
}

/* Transitions */
.slide-fade-enter-active,
.slide-fade-leave-active {
  transition: all 0.25s ease;
}

.slide-fade-enter-from {
  opacity: 0;
  transform: translateX(-12px);
}

.slide-fade-leave-to {
  opacity: 0;
  transform: translateX(12px);
}

/* Toast */
.toast {
  position: fixed;
  bottom: 24px;
  left: 24px;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 11px 18px;
  border-radius: 11px;
  font-size: 13px;
  font-weight: 700;
  font-family: 'Vazirmatn', sans-serif;
  border: 1px solid;
  z-index: 9999;
  backdrop-filter: blur(14px);
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.4);
}

.toast-error {
  background: rgba(248, 113, 113, 0.12);
  border-color: rgba(248, 113, 113, 0.25);
  color: #f87171;
}

.toast-anim-enter-active,
.toast-anim-leave-active {
  transition: all 0.3s ease;
}

.toast-anim-enter-from,
.toast-anim-leave-to {
  opacity: 0;
  transform: translateY(10px);
}

@media(max-width:900px) {
  .brand-panel {
    display: none
  }

  .form-box {
    max-width: 440px
  }
}
</style>
