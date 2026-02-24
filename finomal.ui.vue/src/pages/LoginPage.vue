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
            <v-form ref="loginForm" @submit.prevent="handleLogin">

              <!-- Username -->
              <div class="field-wrap mb-5">
                <label class="field-label">نام کاربری</label>
                <v-text-field v-model="form.username" :rules="[rules.required]" placeholder="نام کاربری یا ایمیل"
                  variant="outlined" density="comfortable" class="login-field"
                  :class="{ 'field-focused': focusedField === 'username' }" @focus="focusedField = 'username'"
                  @blur="focusedField = ''" hide-details="auto" autocomplete="username">
                  <template #prepend-inner>
                    <v-icon size="18" class="field-icon">mdi-account-outline</v-icon>
                  </template>
                </v-text-field>
              </div>

              <!-- Password -->
              <div class="field-wrap mb-2">
                <label class="field-label">رمز عبور</label>
                <v-text-field v-model="form.password" :rules="[rules.required]" :type="showPass ? 'text' : 'password'"
                  placeholder="رمز عبور خود را وارد کنید" variant="outlined" density="comfortable" class="login-field"
                  :class="{ 'field-focused': focusedField === 'password' }" @focus="focusedField = 'password'"
                  @blur="focusedField = ''" hide-details="auto" autocomplete="current-password">
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
                <v-select v-model="form.role" :items="roles" :rules="[rules.required]" item-title="name" item-value="id"
                  placeholder="نقش خود را انتخاب کنید" variant="outlined" density="comfortable" class="login-field"
                  :class="{ 'field-focused': focusedField === 'role' }" @focus="focusedField = 'role'"
                  @blur="focusedField = ''" hide-details="auto" :loading="rolesLoading">
                  <template #prepend-inner>
                    <v-icon size="18" class="field-icon">mdi-shield-account-outline</v-icon>
                  </template>
                </v-select>
              </div>

              <!-- Forgot + Remember -->
              <div class="d-flex align-center justify-space-between mb-7">
                <v-checkbox v-model="form.remember" label="مرا به خاطر بسپار" density="compact" hide-details
                  class="remember-check" />
                <a href="#" class="forgot-link">فراموشی رمز عبور</a>
              </div>

              <!-- Error -->
              <v-alert v-if="errorMsg" type="error" variant="tonal" class="mb-5 login-alert" rounded="lg"
                density="compact">
                {{ errorMsg }}
              </v-alert>

              <!-- Submit -->
              <v-btn type="submit" block size="large" class="login-btn" :loading="loading" elevation="0" rounded="lg">
                <template #loader>
                  <v-progress-circular indeterminate size="20" width="2" color="white" />
                </template>
                <v-icon class="me-2" size="18">mdi-login-variant</v-icon>
                ورود به سیستم
              </v-btn>

            </v-form>

            <!-- Divider -->
            <div class="or-divider my-6">
              <div class="or-line" />
              <span class="or-text">یا</span>
              <div class="or-line" />
            </div>

            <!-- Guest login -->
            <v-btn block variant="outlined" size="large" class="guest-btn" rounded="lg" elevation="0"
              @click="loginAsGuest">
              <v-icon class="me-2" size="18">mdi-account-eye-outline</v-icon>
              ورود به عنوان مهمان
            </v-btn>

            <!-- Footer note -->
            <div class="form-footer mt-6">
              <span>نسخه آزمایشی ۱.۰.۰ — </span>
              <a href="#" class="footer-link">تماس با پشتیبانی</a>
            </div>

          </div>
        </v-col>

      </v-row>
    </v-container> </v-main>
</template>

<script>
import * as signalR from "@microsoft/signalr";

export default {
  name: "LoginPage",

  data() {
    return {
      form: {
        username: "",
        password: "",
        remember: false,
        role: null,
      },
      showPass: false,
      loading: false,
      errorMsg: "",
      rolesLoading: false,
      roles: [],
      connection: null,           // اتصال SignalR مشترک
      connectionPromise: null,    // جلوگیری از اتصال تکراری
      rules: {
        required: (v) => !!v?.toString().trim() || "این فیلد الزامی است",
      },
      chartBars: [28, 42, 35, 55, 38, 62, 48, 70, 52, 78, 60, 88, 65, 95, 72],
      features: [
        "مدیریت کامل اسناد حسابداری",
        "انبارداری و کنترل موجودی",
        "حقوق و دستمزد پرسنل",
        "گزارش‌های مالی جامع",
        "سازگار با سامانه مودیان",
      ],
    };
  },

  computed: {
    isFormValid() {
      return (
        this.form.username.trim() &&
        this.form.password.trim() &&
        this.form.role !== null
      );
    },
  },

  methods: {
    // اتصال پایدار به SignalR (فقط یک بار ساخته می‌شود)
    async ensureConnection() {
      if (this.connectionPromise) {
        return this.connectionPromise;
      }

      this.connectionPromise = new Promise(async (resolve, reject) => {
        try {
          this.connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7272/AccountingHub")
            .withAutomaticReconnect({
              nextRetryDelayInMilliseconds: (retryContext) => {
                return Math.min(1000 * Math.pow(2, retryContext.previousRetryCount), 30000);
              },
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();

          // Listenerهای عمومی
          this.connection.on("LoginResult", (result) => {
            this.handleLoginResponse(result);
          });

          await this.connection.start();
          console.log("SignalR Connected");
          resolve();
        } catch (err) {
          console.error("SignalR connection failed:", err);
          this.connection = null;
          this.connectionPromise = null;
          reject(err);
        }
      });

      return this.connectionPromise;
    },

    // درخواست لیست نقش‌ها
    async fetchRoles() {
      if (this.rolesLoading || this.roles.length > 0) return;

      this.rolesLoading = true;
      try {
        await this.ensureConnection();

        // دریافت نقش‌ها
        const roles = await new Promise((resolve, reject) => {
          const timeout = setTimeout(() => reject(new Error("Timeout")), 10000);

          this.connection.on("RolesList", (data) => {
            clearTimeout(timeout);
            resolve(Array.isArray(data) ? data : data?.data || []);
          });

          this.connection.on("RolesError", (err) => {
            clearTimeout(timeout);
            reject(err?.message || "خطا در بارگذاری نقش‌ها");
          });

          this.connection.invoke("GetAvailableRoles").catch(reject);
        });

        this.roles = roles.map(role => ({
          id: role.id,
          name: role.name || role.Name || "نامشخص"
        }));

      } catch (err) {
        console.error("Fetch roles error:", err);
        this.errorMsg = "خطا در بارگذاری نقش‌ها";
      } finally {
        this.rolesLoading = false;
      }
    },

    // مدیریت پاسخ ورود
    handleLoginResponse(result) {
      this.loading = false;

      if (result?.isSuccess) {
        const { token, userId, username, roleId, expiresIn, refreshToken, refreshTokenExpiry } = result.data || result.value || {};

        localStorage.setItem("jwt", token);
        localStorage.setItem("userRole", roleId || this.form.role);
        localStorage.setItem("username", username || this.form.username);
        localStorage.setItem("tokenTime", Date.now().toString());

        // اگر refresh token دارید
        if (refreshToken) {
          localStorage.setItem("refreshToken", refreshToken);
          localStorage.setItem("refreshTokenExpiry", refreshTokenExpiry);
        }

        this.$router.push("/");
      } else {
        this.errorMsg = result?.message || result?.errors?.[0]?.message || "ورود ناموفق";
      }

      // قطع اتصال بعد از پاسخ (اختیاری)
      // this.connection?.stop();
    },

    async handleLogin() {
      const { valid } = await this.$refs.loginForm.validate();
      if (!valid || !this.isFormValid) return;

      this.loading = true;
      this.errorMsg = "";

      try {
        await this.ensureConnection();
        const roleId = this.form.role?.id ?? this.form.role;

        if (!roleId || roleId === "") {
          this.errorMsg = "لطفاً یک نقش معتبر انتخاب کنید";
          this.loading = false;
          return;
        }

        await this.connection.invoke("Login", {
          UserName: this.form.username.trim(),
          Password: this.form.password,
          RoleId: roleId, // ✅ عدد صحیح دسیمال (مثلاً 43 به جای "2b")
        });

      } catch (err) {
        this.errorMsg = err.message?.includes("401")
          ? "نام کاربری یا رمز عبور اشتباه است"
          : "خطا در ارتباط با سرور";
        console.error("Login failed:", err);
      } finally {
        this.loading = false;
      }
    },

    loginAsGuest() {
      localStorage.setItem("userRole", "0");
      localStorage.setItem("isGuest", "true");
      this.$router.push("/");
    },
  },

  mounted() {
    this.fetchRoles();
  },

  beforeUnmount() {
    if (this.connection) {
      this.connection.off("LoginResult");
      this.connection.off("RolesList");
      this.connection.off("RolesError");
      this.connection.stop();
      this.connection = null;
    }
  },
};
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
  background-image: linear-gradient(rgba(56, 189, 248, 0.03) 1px, transparent 1px), linear-gradient(90deg, rgba(56, 189, 248, 0.03) 1px, transparent 1px);
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
  transition: background 0.2s;
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

/* ===== Role Selector ===== */
.role-selector {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 8px;
  direction: rtl;
}

.role-btn {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 6px;
  padding: 10px 6px;
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.08);
  background: rgba(255, 255, 255, 0.03);
  cursor: pointer;
  transition: all 0.2s ease;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif;
}

.role-btn:hover {
  border-color: rgba(56, 189, 248, 0.3);
  background: rgba(56, 189, 248, 0.05);
}

.role-btn--active {
  border-color: rgba(56, 189, 248, 0.6) !important;
  background: rgba(56, 189, 248, 0.1) !important;
  box-shadow: 0 0 12px rgba(56, 189, 248, 0.2);
}

.role-btn--active .role-icon {
  color: #38bdf8 !important;
}

.role-btn--active .role-label {
  color: #38bdf8 !important;
}

.role-icon {
  color: #475569 !important;
  transition: color 0.2s;
}

.role-label {
  font-size: 11px;
  color: #64748b;
  transition: color 0.2s;
  white-space: nowrap;
}

/* Fields */
.field-label {
  display: block;
  font-size: 12px;
  font-weight: 600;
  color: #64748b;
  margin-bottom: 8px;
  text-align: right;
}

:deep(.login-field .v-field) {
  background: rgba(255, 255, 255, 0.03) !important;
  border-radius: 10px !important;
  transition: all 0.2s !important;
  font-family: 'Vazirmatn', 'Tahoma', sans-serif !important;
  direction: rtl !important;
}

:deep(.login-field .v-field--outlined .v-field__outline) {
  --v-field-border-opacity: 1;
  color: rgba(255, 255, 255, 0.08) !important;
}

:deep(.login-field.field-focused .v-field) {
  background: rgba(56, 189, 248, 0.05) !important;
  box-shadow: 0 0 0 2px rgba(56, 189, 248, 0.2) !important;
}

:deep(.login-field .v-field__input) {
  font-family: 'Vazirmatn', 'Tahoma', sans-serif !important;
  font-size: 13px !important;
  color: #e2e8f0 !important;
  direction: rtl !important;
  text-align: right !important;
  padding-right: 0 !important;
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

:deep(.remember-check .v-checkbox-btn) {
  color: #475569 !important;
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

.login-btn:hover {
  box-shadow: 0 6px 32px rgba(14, 165, 233, 0.45) !important;
  transform: translateY(-1px);
}

.login-btn:active {
  transform: translateY(0);
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
