<template>
  <div class="page-root" dir="rtl">

    <!-- ══ BG Glow ══ -->
    <div class="bg-glow bg-glow-1" />
    <div class="bg-glow bg-glow-2" />
    <div class="bg-grid" />

    <div class="page-inner">

      <!-- ══════════ HEADER ══════════ -->
      <header class="page-header">
        <div class="brand">
          <div class="brand-mark">
            <v-icon size="20" color="#63b3ed">mdi-account-group</v-icon>
          </div>
          <div>
            <div class="brand-name">سیستم فینومال</div>
            <div class="brand-sub">مدیریت منابع انسانی</div>
          </div>
        </div>
        <router-link to="/personnel" class="header-back">
          <v-icon size="14" class="ml-1">mdi-arrow-left</v-icon>
          بازگشت به لیست پرسنل
        </router-link>
      </header>

      <!-- ══════════ STEPPER NAV ══════════ -->
      <nav class="stepper-nav">
        <div v-for="(step, i) in steps" :key="step.id" class="step-item" :class="{
          'step-done': currentStep > step.id,
          'step-active': currentStep === step.id
        }">
          <!-- connector -->
          <div v-if="i > 0" class="step-connector" />

          <div class="step-node" :class="{ clickable: currentStep > step.id }"
            @click="currentStep > step.id && goToStep(step.id)">
            <div class="step-circle">
              <v-icon v-if="currentStep > step.id" size="15" color="white">mdi-check</v-icon>
              <v-icon v-else size="15">{{ step.icon }}</v-icon>
            </div>
            <div class="step-text">
              <div class="step-label">{{ step.label }}</div>
              <div class="step-desc">{{ step.desc }}</div>
            </div>
          </div>
        </div>
      </nav>

      <!-- ══════════ PROGRESS BAR ══════════ -->
      <div class="progress-track">
        <div class="progress-fill" :style="{ width: progressPct + '%' }" />
      </div>

      <!-- ══════════ FORM CARD ══════════ -->
      <div class="form-card">

        <!-- Step Header -->
        <div class="form-card-header" :style="{ '--hue': steps[currentStep - 1]?.hue }">
          <div class="section-icon" :class="steps[currentStep - 1]?.iconClass">
            <v-icon size="22">{{ steps[currentStep - 1]?.icon }}</v-icon>
          </div>
          <div class="section-meta">
            <div class="section-title">{{ steps[currentStep - 1]?.title }}</div>
            <div class="section-sub">{{ steps[currentStep - 1]?.subtitle }}</div>
          </div>
          <div class="section-badge" :class="steps[currentStep - 1]?.badgeClass">
            {{ currentStep < 5 ? `گام ${toPersian(currentStep)} از ۴` : 'آماده ثبت' }} </div>
          </div>

          <!-- ──────── STEP 1: هویتی ──────── -->
          <transition name="step-fade">
            <div v-if="currentStep === 1" class="form-body">

              <div class="group-block">
                <div class="group-label">مشخصات فردی</div>
                <v-row :gutter="14">
                  <v-col cols="12" sm="4">
                    <PField label="نام" required>
                      <v-text-field v-model="form.firstName" placeholder="نام" :error-messages="errors.firstName"
                        prepend-inner-icon="mdi-account" variant="outlined" density="compact"
                        @input="clearError('firstName')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="نام خانوادگی" required>
                      <v-text-field v-model="form.lastName" placeholder="نام خانوادگی" :error-messages="errors.lastName"
                        prepend-inner-icon="mdi-account" variant="outlined" density="compact"
                        @input="clearError('lastName')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="نام پدر" required>
                      <v-text-field v-model="form.fatherName" placeholder="نام پدر" :error-messages="errors.fatherName"
                        prepend-inner-icon="mdi-account-child" variant="outlined" density="compact"
                        @input="clearError('fatherName')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="کد ملی" required hint="۱۰ رقم">
                      <v-text-field v-model="form.nationalId" placeholder="۰۰۱۲۳۴۵۶۷۸۹"
                        :error-messages="errors.nationalId" prepend-inner-icon="mdi-card-account-details"
                        variant="outlined" density="compact" maxlength="10" dir="ltr"
                        @input="clearError('nationalId')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="شماره شناسنامه" required>
                      <v-text-field v-model="form.idNumber" placeholder="شماره شناسنامه"
                        :error-messages="errors.idNumber" prepend-inner-icon="mdi-file-document" variant="outlined"
                        density="compact" dir="ltr" @input="clearError('idNumber')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="تاریخ تولد">
                      <v-text-field v-model="form.birthDate" placeholder="۱۳۷۰/۰۱/۰۱" prepend-inner-icon="mdi-calendar"
                        variant="outlined" density="compact" />
                    </PField>
                  </v-col>

                  <!-- جنسیت -->
                  <v-col cols="12" sm="4">
                    <PField label="جنسیت">
                      <div class="radio-group">
                        <RadioCard v-for="opt in genderOptions" :key="opt.value" v-model="form.gender"
                          :value="opt.value" :label="opt.label" />
                      </div>
                    </PField>
                  </v-col>

                  <!-- تأهل -->
                  <v-col cols="12" sm="4">
                    <PField label="وضعیت تأهل">
                      <div class="radio-group">
                        <RadioCard v-for="opt in maritalOptions" :key="opt.value" v-model="form.maritalStatus"
                          :value="opt.value" :label="opt.label" />
                      </div>
                    </PField>
                  </v-col>

                  <!-- تعداد فرزندان -->
                  <v-col cols="12" sm="4">
                    <PField label="تعداد فرزندان">
                      <NumStepper v-model="form.childrenCount" :min="0" :max="10" />
                    </PField>
                  </v-col>
                </v-row>
              </div>

              <div class="group-block">
                <div class="group-label">اطلاعات تماس</div>
                <v-row>
                  <v-col cols="12" sm="6">
                    <PField label="شماره همراه" required>
                      <v-text-field v-model="form.mobile" placeholder="۰۹۱۲۱۲۳۴۵۶۷" :error-messages="errors.mobile"
                        prepend-inner-icon="mdi-cellphone" variant="outlined" density="compact" dir="ltr"
                        @input="clearError('mobile')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="تلفن ثابت">
                      <v-text-field v-model="form.phone" placeholder="۰۲۱۱۲۳۴۵۶۷۸" prepend-inner-icon="mdi-phone"
                        variant="outlined" density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                  <v-col cols="12">
                    <PField label="آدرس محل سکونت">
                      <v-textarea v-model="form.address" placeholder="استان، شهر، خیابان، پلاک..." variant="outlined"
                        density="compact" rows="3" no-resize />
                    </PField>
                  </v-col>
                </v-row>
              </div>

            </div>
          </transition>

          <!-- ──────── STEP 2: شغلی ──────── -->
          <transition name="step-fade">
            <div v-if="currentStep === 2" class="form-body">

              <div class="group-block">
                <div class="group-label">اطلاعات قرارداد</div>
                <v-row>
                  <v-col cols="12" sm="4">
                    <PField label="کد پرسنلی" required>
                      <v-text-field v-model="form.personnelCode" placeholder="P-0001"
                        :error-messages="errors.personnelCode" prepend-inner-icon="mdi-identifier" variant="outlined"
                        density="compact" dir="ltr" @input="clearError('personnelCode')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="تاریخ استخدام" required>
                      <v-text-field v-model="form.hireDate" placeholder="۱۴۰۰/۰۱/۰۱" :error-messages="errors.hireDate"
                        prepend-inner-icon="mdi-calendar-check" variant="outlined" density="compact"
                        @input="clearError('hireDate')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="تاریخ ترک کار">
                      <v-text-field v-model="form.leaveDate" placeholder="خالی = مشغول به کار"
                        prepend-inner-icon="mdi-calendar-remove" variant="outlined" density="compact" />
                    </PField>
                  </v-col>
                </v-row>
              </div>

              <div class="group-block">
                <div class="group-label">نوع استخدام</div>
                <div class="radio-group radio-group-lg">
                  <RadioCard v-for="opt in employmentOptions" :key="opt.value" v-model="form.employmentType"
                    :value="opt.value" :label="opt.label" />
                </div>
              </div>

              <div class="group-block">
                <div class="group-label">پست سازمانی</div>
                <v-row>
                  <v-col cols="12" sm="6">
                    <PField label="واحد / دپارتمان" required>
                      <v-select v-model="form.department" :items="departments" placeholder="انتخاب واحد..."
                        :error-messages="errors.department" prepend-inner-icon="mdi-office-building" variant="outlined"
                        density="compact" @update:model-value="clearError('department')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="عنوان شغلی" required>
                      <v-text-field v-model="form.jobTitle" placeholder="مثال: حسابدار ارشد"
                        :error-messages="errors.jobTitle" prepend-inner-icon="mdi-briefcase" variant="outlined"
                        density="compact" @input="clearError('jobTitle')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="مرکز هزینه">
                      <v-select v-model="form.costCenter" :items="costCenters" placeholder="انتخاب مرکز هزینه..."
                        prepend-inner-icon="mdi-chart-pie" variant="outlined" density="compact" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="محل خدمت">
                      <v-text-field v-model="form.workLocation" placeholder="تهران - دفتر مرکزی"
                        prepend-inner-icon="mdi-map-marker" variant="outlined" density="compact" />
                    </PField>
                  </v-col>
                </v-row>
              </div>

            </div>
          </transition>

          <!-- ──────── STEP 3: مالی ──────── -->
          <transition name="step-fade">
            <div v-if="currentStep === 3" class="form-body">

              <div class="info-box">
                <v-icon size="16" color="#63b3ed">mdi-information</v-icon>
                <p>شماره شبا برای <strong>خروجی فایل پرداخت گروهی بانک</strong> استفاده می‌شود. وارد کردن آن الزامی است.
                </p>
              </div>

              <div class="group-block">
                <div class="group-label">حساب بانکی</div>
                <v-row>
                  <v-col cols="12" sm="6">
                    <PField label="نام بانک" required>
                      <v-select v-model="form.bankName" :items="banks" placeholder="انتخاب بانک..."
                        :error-messages="errors.bankName" prepend-inner-icon="mdi-bank" variant="outlined"
                        density="compact" @update:model-value="clearError('bankName')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="شماره حساب">
                      <v-text-field v-model="form.accountNumber" placeholder="شماره حساب"
                        prepend-inner-icon="mdi-credit-card" variant="outlined" density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                  <v-col cols="12">
                    <PField label="شماره شبا (IBAN)" required>
                      <v-text-field v-model="form.iban" placeholder="00 010 0000 0000 0000 0000 00"
                        :error-messages="errors.iban" variant="outlined" density="compact" dir="ltr" maxlength="26"
                        @input="clearError('iban')">
                        <template #prepend-inner>
                          <span class="iban-ir">IR</span>
                        </template>
                      </v-text-field>
                    </PField>
                  </v-col>
                </v-row>
              </div>

              <div class="group-block">
                <div class="group-label">تخصیص کدینگ حسابداری</div>
                <v-row>
                  <v-col cols="12" sm="4">
                    <PField label="کد حساب جاری کارکنان">
                      <v-text-field v-model="form.accountCodeCurrent" placeholder="۱۴۱۱۰۰۰x" variant="outlined"
                        density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="کد حساب وام">
                      <v-text-field v-model="form.accountCodeLoan" placeholder="۱۴۱۲۰۰۰x" variant="outlined"
                        density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="کد حساب مساعده">
                      <v-text-field v-model="form.accountCodeAdvance" placeholder="۱۴۱۳۰۰۰x" variant="outlined"
                        density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                </v-row>
              </div>

              <div class="group-block">
                <div class="group-label">
                  حقوق پایه و مزایا
                  <span class="group-unit">(ریال)</span>
                </div>
                <v-row>
                  <v-col cols="12" sm="4">
                    <PField label="حقوق پایه" required>
                      <v-text-field v-model="form.baseSalary" placeholder="۱۵,۰۰۰,۰۰۰"
                        :error-messages="errors.baseSalary" prepend-inner-icon="mdi-currency-usd" variant="outlined"
                        density="compact" dir="ltr" @input="formatSalary('baseSalary'); clearError('baseSalary')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="حق مسکن">
                      <v-text-field v-model="form.housingAllowance" placeholder="۶,۰۰۰,۰۰۰" variant="outlined"
                        density="compact" dir="ltr" @input="formatSalary('housingAllowance')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="بن خواروبار">
                      <v-text-field v-model="form.foodAllowance" placeholder="۴,۵۰۰,۰۰۰" variant="outlined"
                        density="compact" dir="ltr" @input="formatSalary('foodAllowance')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="حق اولاد">
                      <v-text-field v-model="form.childAllowance" placeholder="محاسبه خودکار" variant="outlined"
                        density="compact" dir="ltr" @input="formatSalary('childAllowance')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="اضافه‌کار ثابت">
                      <v-text-field v-model="form.overtimeFixed" placeholder="۰" variant="outlined" density="compact"
                        dir="ltr" @input="formatSalary('overtimeFixed')" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="4">
                    <PField label="سایر مزایا">
                      <v-text-field v-model="form.otherAllowance" placeholder="۰" variant="outlined" density="compact"
                        dir="ltr" @input="formatSalary('otherAllowance')" />
                    </PField>
                  </v-col>
                </v-row>

                <!-- حقوق ناخالص محاسبه‌شده -->
                <div class="salary-total">
                  <span class="salary-total-label">حقوق ناخالص کل</span>
                  <span class="salary-total-val">{{ grossSalary }} ریال</span>
                </div>
              </div>

            </div>
          </transition>

          <!-- ──────── STEP 4: بیمه و مالیات ──────── -->
          <transition name="step-fade">
            <div v-if="currentStep === 4" class="form-body">

              <div class="group-block">
                <div class="group-label">اطلاعات بیمه تأمین اجتماعی</div>
                <v-row>
                  <v-col cols="12" sm="6">
                    <PField label="شماره بیمه">
                      <v-text-field v-model="form.insuranceNumber" placeholder="شماره بیمه تأمین اجتماعی"
                        prepend-inner-icon="mdi-shield-account" variant="outlined" density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="نوع بیمه">
                      <v-select v-model="form.insuranceType" :items="insuranceTypes" placeholder="انتخاب نوع بیمه..."
                        prepend-inner-icon="mdi-shield-check" variant="outlined" density="compact" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="درصد بیمه کارمند">
                      <v-text-field v-model="form.employeeInsurancePct" placeholder="۷ درصد" variant="outlined"
                        density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="درصد بیمه کارفرما">
                      <v-text-field v-model="form.employerInsurancePct" placeholder="۲۳ درصد" variant="outlined"
                        density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                </v-row>

                <div class="toggles-stack">
                  <ToggleRow v-model="form.unemploymentInsurance" title="مشمول بیمه بیکاری"
                    desc="۱ درصد اضافه از حقوق پرسنل کسر می‌شود" />
                </div>
              </div>

              <div class="group-block">
                <div class="group-label">اطلاعات مالیاتی</div>
                <v-row>
                  <v-col cols="12" sm="6">
                    <PField label="نوع معافیت مالیاتی">
                      <v-select v-model="form.taxExemptionType" :items="taxExemptions"
                        placeholder="انتخاب نوع معافیت..." prepend-inner-icon="mdi-file-document-edit"
                        variant="outlined" density="compact" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="محل خدمت مالیاتی">
                      <v-text-field v-model="form.taxServiceLocation" placeholder="تهران - اداره مالیاتی منطقه ..."
                        prepend-inner-icon="mdi-map-marker" variant="outlined" density="compact" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="کد اقتصادی کارفرما">
                      <v-text-field v-model="form.employerEconomicCode" placeholder="۱۰۰۰۰۰۰۰۰۰۰۰" variant="outlined"
                        density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <PField label="شماره شناسه ملی کارفرما">
                      <v-text-field v-model="form.employerNationalId" placeholder="۱۴۰۰۰۰۰۰۰۰۰" variant="outlined"
                        density="compact" dir="ltr" />
                    </PField>
                  </v-col>
                </v-row>

                <div class="toggles-stack">
                  <ToggleRow v-model="form.incomeTaxable" title="مشمول مالیات بر درآمد حقوق"
                    desc="بر اساس ماده ۸۶ قانون مالیات‌های مستقیم" />
                  <ToggleRow v-model="form.includeInInsuranceList" title="ارسال لیست دیسکت بیمه"
                    desc="در لیست ماهانه ارسالی به تأمین اجتماعی لحاظ می‌شود" />
                </div>
              </div>

            </div>
          </transition>

          <!-- ──────── STEP 5: تأیید ──────── -->
          <transition name="step-fade">
            <div v-if="currentStep === 5" class="form-body">
              <div v-for="section in summaryData" :key="section.title" class="summary-section">
                <div class="summary-section-title">{{ section.title }}</div>
                <div class="summary-grid">
                  <div v-for="field in section.fields" :key="field.key" class="summary-row">
                    <div class="summary-key">{{ field.key }}</div>
                    <div class="summary-val" :class="{ empty: !field.val || field.val === '—' }">
                      {{ field.val || 'وارد نشده' }}
                    </div>
                    <button v-if="field.step" class="summary-edit" @click="goToStep(field.step)">
                      <v-icon size="12">mdi-pencil</v-icon>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </transition>

          <!-- ══════════ ACTIONS ══════════ -->
          <div class="form-actions">
            <div class="actions-right">
              <button v-if="currentStep > 1" class="btn btn-ghost" @click="prevStep">
                <v-icon size="15">mdi-arrow-right</v-icon>
                مرحله قبل
              </button>
              <span class="step-counter">
                مرحله <b>{{ toPersian(Math.min(currentStep, 4)) }}</b> از <b>۴</b>
              </span>
            </div>
            <div class="actions-left">
              <button class="btn btn-ghost" @click="saveDraft">
                <v-icon size="15">mdi-content-save</v-icon>
                ذخیره پیش‌نویس
              </button>
              <button v-if="currentStep < 5" class="btn btn-primary" @click="nextStep">
                مرحله بعد
                <v-icon size="15">mdi-arrow-left</v-icon>
              </button>
              <button v-if="currentStep === 5" class="btn btn-success" :disabled="submitting" @click="submitForm">
                <span v-if="submitting" class="btn-spinner" />
                <v-icon v-else size="15">mdi-check</v-icon>
                {{ submitting ? 'در حال ثبت...' : 'ثبت پرسنل' }}
              </button>
            </div>
          </div>

        </div><!-- /form-card -->

      </div><!-- /page-inner -->

      <!-- ══ Toast ══ -->
      <transition name="toast-anim">
        <div v-if="toast.show" class="toast" :class="`toast-${toast.type}`">
          <v-icon size="16" class="ml-2">{{ toast.type === 'success' ? 'mdi-check-circle' : 'mdi-alert-circle'
          }}</v-icon>
          {{ toast.text }}
        </div>
      </transition>

    </div>
</template>

<script setup>
import { ref, computed, h, resolveComponent } from 'vue'

// ── Sub-components ────────────────────────────────────────────────────────

// PField: wrapper label - using render function (runtime-only compatible)
const PField = {
  props: { label: String, required: Boolean, hint: String },
  setup(props, { slots }) {
    return () => h('div', { class: 'p-field' }, [
      h('label', { class: 'p-label' }, [
        props.label,
        props.required ? h('span', { class: 'p-required' }, '*') : null,
        props.hint ? h('span', { class: 'p-hint' }, props.hint) : null,
      ]),
      slots.default?.()
    ])
  }
}

// RadioCard - using render function
const RadioCard = {
  props: { modelValue: String, value: String, label: String },
  emits: ['update:modelValue'],
  setup(props, { emit }) {
    return () => h('div', {
      class: ['radio-card', { selected: props.modelValue === props.value }],
      onClick: () => emit('update:modelValue', props.value)
    }, [
      h('div', { class: ['radio-dot', { active: props.modelValue === props.value }] }, [
        props.modelValue === props.value ? h('div', { class: 'radio-inner' }) : null
      ]),
      props.label
    ])
  }
}

// NumStepper - using render function
const NumStepper = {
  props: {
    modelValue: { type: Number, default: 0 },
    min: { type: Number, default: 0 },
    max: { type: Number, default: 10 }
  },
  emits: ['update:modelValue'],
  setup(props, { emit }) {
    const persianNums = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹']
    const toPersian = n => String(n).split('').map(c => persianNums[+c] ?? c).join('')
    const dec = () => emit('update:modelValue', Math.max(props.min, props.modelValue - 1))
    const inc = () => emit('update:modelValue', Math.min(props.max, props.modelValue + 1))
    return () => h('div', { class: 'num-stepper' }, [
      h('button', { class: 'num-btn', onClick: dec }, '−'),
      h('div', { class: 'num-val' }, toPersian(props.modelValue)),
      h('button', { class: 'num-btn', onClick: inc }, '+'),
    ])
  }
}

// ToggleRow - using render function
const ToggleRow = {
  props: { modelValue: Boolean, title: String, desc: String },
  emits: ['update:modelValue'],
  setup(props, { emit }) {
    return () => h('div', { class: 'toggle-row' }, [
      h('div', { class: 'toggle-info' }, [
        h('div', { class: 'toggle-title' }, props.title),
        h('div', { class: 'toggle-desc' }, props.desc),
      ]),
      h('label', { class: 'toggle-switch' }, [
        h('input', {
          type: 'checkbox',
          checked: props.modelValue,
          onChange: (e) => emit('update:modelValue', e.target.checked)
        }),
        h('div', { class: 'toggle-track' }, [
          h('div', { class: 'toggle-thumb' })
        ])
      ])
    ])
  }
}

// ── Steps config ──────────────────────────────────────────────────────────
const steps = [
  { id: 1, label: 'هویتی', desc: 'مشخصات فردی', icon: 'mdi-account', iconClass: 'icon-blue', hue: '210', title: 'اطلاعات هویتی و شناسنامه‌ای', subtitle: 'مشخصات فردی، شناسنامه‌ای و اطلاعات تماس پرسنل', badgeClass: '' },
  { id: 2, label: 'شغلی', desc: 'قرارداد و پست', icon: 'mdi-briefcase', iconClass: 'icon-green', hue: '145', title: 'اطلاعات شغلی و قراردادی', subtitle: 'کد پرسنلی، تاریخ استخدام، نوع قرارداد و پست سازمانی', badgeClass: '' },
  { id: 3, label: 'مالی', desc: 'بانک و حقوق', icon: 'mdi-bank', iconClass: 'icon-amber', hue: '38', title: 'اطلاعات مالی و بانکی', subtitle: 'حساب بانکی، تخصیص کدینگ حسابداری و حقوق پایه', badgeClass: '' },
  { id: 4, label: 'بیمه', desc: 'مالیات و بیمه', icon: 'mdi-shield', iconClass: 'icon-purple', hue: '270', title: 'اطلاعات بیمه و مالیات', subtitle: 'شماره بیمه، معافیت‌های مالیاتی و محل خدمت مالیاتی', badgeClass: '' },
  { id: 5, label: 'تأیید', desc: 'بررسی نهایی', icon: 'mdi-check-all', iconClass: 'icon-green', hue: '145', title: 'بررسی و تأیید نهایی', subtitle: 'اطلاعات وارد شده را مرور و در صورت نیاز ویرایش کنید', badgeClass: 'badge-success' },
]

// ── Form state ────────────────────────────────────────────────────────────
const currentStep = ref(1)
const submitting = ref(false)
const errors = ref({})
const toast = ref({ show: false, text: '', type: 'success' })

const form = ref({
  // step 1
  firstName: '', lastName: '', fatherName: '',
  nationalId: '', idNumber: '', birthDate: '',
  gender: 'male', maritalStatus: 'single', childrenCount: 0,
  mobile: '', phone: '', address: '',
  // step 2
  personnelCode: '', hireDate: '', leaveDate: '',
  employmentType: 'permanent',
  department: '', jobTitle: '', costCenter: '', workLocation: '',
  // step 3
  bankName: '', accountNumber: '', iban: '',
  accountCodeCurrent: '', accountCodeLoan: '', accountCodeAdvance: '',
  baseSalary: '', housingAllowance: '', foodAllowance: '',
  childAllowance: '', overtimeFixed: '', otherAllowance: '',
  // step 4
  insuranceNumber: '', insuranceType: '', employeeInsurancePct: '', employerInsurancePct: '',
  unemploymentInsurance: true,
  taxExemptionType: '', taxServiceLocation: '',
  employerEconomicCode: '', employerNationalId: '',
  incomeTaxable: true, includeInInsuranceList: true,
})

// ── Lists ─────────────────────────────────────────────────────────────────
const genderOptions = [{ value: 'male', label: 'مرد' }, { value: 'female', label: 'زن' }]
const maritalOptions = [{ value: 'single', label: 'مجرد' }, { value: 'married', label: 'متأهل' }]
const employmentOptions = [{ value: 'permanent', label: 'رسمی' }, { value: 'contract', label: 'پیمانی' }, { value: 'hourly', label: 'ساعتی' }, { value: 'project', label: 'پروژه‌ای' }]
const departments = ['اداری و منابع انسانی', 'مالی و حسابداری', 'فروش و بازاریابی', 'تولید و عملیات', 'فناوری اطلاعات', 'انبار و لجستیک']
const costCenters = ['هزینه‌های اداری', 'هزینه‌های فروش', 'هزینه‌های تولید', 'هزینه‌های تحقیق و توسعه']
const banks = ['بانک ملت', 'بانک ملی ایران', 'بانک صادرات', 'بانک تجارت', 'بانک رفاه کارگران', 'بانک آینده', 'بانک پاسارگاد', 'بانک اقتصاد نوین']
const insuranceTypes = ['تأمین اجتماعی', 'بیمه درمان تکمیلی', 'هر دو', 'بدون بیمه']
const taxExemptions = ['بدون معافیت - مالیات عادی', 'معافیت مناطق کمتر توسعه‌یافته', 'معافیت مناطق آزاد تجاری', 'معافیت سازمان‌های خاص', 'معافیت کامل (ماده ۸۴ قانون مالیات)']

// ── Computed ──────────────────────────────────────────────────────────────
const progressPct = computed(() =>
  currentStep.value === 5 ? 100 : (currentStep.value / 4) * 100
)

const grossSalary = computed(() => {
  const fields = ['baseSalary', 'housingAllowance', 'foodAllowance', 'childAllowance', 'overtimeFixed', 'otherAllowance']
  const total = fields.reduce((sum, f) => {
    const raw = (form.value[f] || '').replace(/[,،\s]/g, '').replace(/[۰-۹]/g, d => '۰۱۲۳۴۵۶۷۸۹'.indexOf(d))
    return sum + (parseInt(raw) || 0)
  }, 0)
  return total.toLocaleString('fa-IR')
})

const summaryData = computed(() => [
  {
    title: 'اطلاعات هویتی',
    fields: [
      { key: 'نام و نام خانوادگی', val: [form.value.firstName, form.value.lastName].filter(Boolean).join(' '), step: 1 },
      { key: 'نام پدر', val: form.value.fatherName, step: 1 },
      { key: 'کد ملی', val: form.value.nationalId, step: 1 },
      { key: 'شماره شناسنامه', val: form.value.idNumber, step: 1 },
      { key: 'شماره همراه', val: form.value.mobile, step: 1 },
      { key: 'تعداد فرزندان', val: toPersian(form.value.childrenCount), step: 1 },
    ]
  },
  {
    title: 'اطلاعات شغلی',
    fields: [
      { key: 'کد پرسنلی', val: form.value.personnelCode, step: 2 },
      { key: 'تاریخ استخدام', val: form.value.hireDate, step: 2 },
      { key: 'واحد', val: form.value.department, step: 2 },
      { key: 'عنوان شغلی', val: form.value.jobTitle, step: 2 },
      { key: 'نوع استخدام', val: ({ permanent: 'رسمی', contract: 'پیمانی', hourly: 'ساعتی', project: 'پروژه‌ای' })[form.value.employmentType], step: 2 },
      { key: 'مرکز هزینه', val: form.value.costCenter, step: 2 },
    ]
  },
  {
    title: 'اطلاعات مالی',
    fields: [
      { key: 'نام بانک', val: form.value.bankName, step: 3 },
      { key: 'شماره شبا', val: form.value.iban ? 'IR' + form.value.iban : '—', step: 3 },
      { key: 'حقوق پایه', val: form.value.baseSalary ? form.value.baseSalary + ' ریال' : '—', step: 3 },
      { key: 'حق مسکن', val: form.value.housingAllowance || '—', step: 3 },
    ]
  },
  {
    title: 'بیمه و مالیات',
    fields: [
      { key: 'شماره بیمه', val: form.value.insuranceNumber || '—', step: 4 },
      { key: 'نوع بیمه', val: form.value.insuranceType || '—', step: 4 },
      { key: 'معافیت مالیاتی', val: form.value.taxExemptionType || '—', step: 4 },
      { key: 'محل خدمت مالیاتی', val: form.value.taxServiceLocation || '—', step: 4 },
    ]
  },
])

// ── Helpers ────────────────────────────────────────────────────────────────
const persianNums = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹']
function toPersian(n) {
  return String(n).split('').map(c => persianNums[+c] ?? c).join('')
}

function clearError(field) { delete errors.value[field] }

function formatSalary(field) {
  const raw = (form.value[field] || '').replace(/[^۰-۹0-9]/g, '')
  if (!raw) return
  const num = parseInt(raw.replace(/[۰-۹]/g, d => '۰۱۲۳۴۵۶۷۸۹'.indexOf(d)))
  if (!isNaN(num)) form.value[field] = num.toLocaleString('fa-IR')
}

let toastTimer
function showToast(text, type = 'success') {
  toast.value = { show: true, text, type }
  clearTimeout(toastTimer)
  toastTimer = setTimeout(() => { toast.value.show = false }, 3500)
}

// ── Navigation ─────────────────────────────────────────────────────────────
const required = {
  1: ['firstName', 'lastName', 'fatherName', 'nationalId', 'idNumber', 'mobile'],
  2: ['personnelCode', 'hireDate', 'department', 'jobTitle'],
  3: ['bankName', 'iban', 'baseSalary'],
  4: [],
}

function validateStep(step) {
  const fields = required[step] || []
  const errs = {}
  fields.forEach(f => { if (!form.value[f]?.trim?.()) errs[f] = 'این فیلد الزامی است' })
  errors.value = errs
  if (Object.keys(errs).length) {
    showToast('لطفاً فیلدهای الزامی را تکمیل کنید', 'error')
    return false
  }
  return true
}

function nextStep() {
  if (!validateStep(currentStep.value)) return
  currentStep.value = Math.min(currentStep.value + 1, 5)
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

function prevStep() {
  currentStep.value = Math.max(currentStep.value - 1, 1)
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

function goToStep(n) {
  currentStep.value = n
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

function saveDraft() { showToast('پیش‌نویس ذخیره شد', 'success') }

async function submitForm() {
  submitting.value = true
  await new Promise(r => setTimeout(r, 1800))
  submitting.value = false
  showToast('پرسنل با موفقیت ثبت شد ✓', 'success')
}
</script>

<style>
/* ── Vuetify overrides ─────────────────────────────────────────────────── */
.v-field {
  --v-field-border-color: rgba(255, 255, 255, 0.08) !important;
  --v-field-border-opacity: 1 !important;
}

.v-field__outline__start,
.v-field__outline__notch,
.v-field__outline__end {
  border-color: rgba(255, 255, 255, 0.08) !important
}

.v-field--focused .v-field__outline__start,
.v-field--focused .v-field__outline__notch,
.v-field--focused .v-field__outline__end {
  border-color: rgba(99, 179, 237, 0.45) !important
}

.v-field--error .v-field__outline__start,
.v-field--error .v-field__outline__notch,
.v-field--error .v-field__outline__end {
  border-color: rgba(252, 129, 129, 0.5) !important
}

.v-field--focused {
  background: rgba(26, 32, 48, 0.9) !important;
  box-shadow: 0 0 0 3px rgba(99, 179, 237, 0.08) !important
}

.v-field,
.v-field__field {
  background: rgba(20, 24, 32, 0.9) !important
}

.v-field__input {
  color: #e8edf5 !important;
  font-family: 'Vazirmatn', sans-serif !important;
  font-size: 13px !important
}

.v-field__prepend-inner .v-icon {
  color: #3d4560 !important;
  transition: color .2s
}

.v-field--focused .v-field__prepend-inner .v-icon {
  color: #63b3ed !important
}

.v-label {
  color: #5a6480 !important;
  font-family: 'Vazirmatn', sans-serif !important;
  font-size: 12px !important
}

.v-messages__message {
  font-family: 'Vazirmatn', sans-serif !important;
  font-size: 11px !important
}

.v-select__selection-text {
  color: #e8edf5 !important;
  font-family: 'Vazirmatn', sans-serif !important;
  font-size: 13px !important
}

.v-list {
  background: #141820 !important;
  border: 1px solid rgba(255, 255, 255, .07) !important
}

.v-list-item {
  color: #e8edf5 !important;
  font-family: 'Vazirmatn', sans-serif !important;
  font-size: 12.5px !important
}

.v-list-item:hover {
  background: rgba(99, 179, 237, .07) !important
}

.v-textarea .v-field__input {
  color: #e8edf5 !important
}

input::placeholder,
textarea::placeholder {
  color: #3d4560 !important
}
</style>

<style scoped>
/* ── Root ── */
.page-root {
  min-height: 100vh;
  background: var(--bg);
  color: var(--text);
  font-family: var(--font);
  direction: rtl;
  position: relative;
  overflow-x: hidden;
}

/* ── BG ── */
.bg-glow {
  position: fixed;
  border-radius: 50%;
  pointer-events: none;
  z-index: 0;
  filter: blur(80px)
}

.bg-glow-1 {
  width: 600px;
  height: 400px;
  top: -10%;
  right: -5%;
  background: radial-gradient(ellipse, rgba(99, 179, 237, .05), transparent)
}

.bg-glow-2 {
  width: 500px;
  height: 350px;
  bottom: 5%;
  left: -5%;
  background: radial-gradient(ellipse, rgba(72, 187, 120, .04), transparent)
}

.bg-grid {
  position: fixed;
  inset: 0;
  pointer-events: none;
  z-index: 0;
  background:
    repeating-linear-gradient(0deg, transparent, transparent 40px, rgba(255, 255, 255, .008) 40px, rgba(255, 255, 255, .008) 41px),
    repeating-linear-gradient(90deg, transparent, transparent 40px, rgba(255, 255, 255, .008) 40px, rgba(255, 255, 255, .008) 41px);
}

.page-inner {
  position: relative;
  z-index: 1;
  margin: 0 auto;
  padding: 32px 20px 60px;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

/* ── Header ── */
.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between
}

.brand {
  display: flex;
  align-items: center;
  gap: 12px
}

.brand-mark {
  width: 42px;
  height: 42px;
  border-radius: 12px;
  background: linear-gradient(135deg, rgba(99, 179, 237, .2), rgba(99, 179, 237, .05));
  border: 1px solid rgba(99, 179, 237, .25);
  display: flex;
  align-items: center;
  justify-content: center
}

.brand-name {
  font-size: 13px;
  font-weight: 700;
  letter-spacing: .3px
}

.brand-sub {
  font-size: 11px;
  color: var(--muted);
  margin-top: 1px
}

.header-back {
  font-size: 12px;
  color: var(--muted);
  display: flex;
  align-items: center;
  gap: 5px;
  text-decoration: none;
  transition: color .2s
}

.header-back:hover {
  color: var(--accent)
}

/* ── Stepper ── */
.stepper-nav {
  display: flex;
  align-items: center;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  padding: 18px 28px;
  position: relative;
  overflow: hidden;
}

.stepper-nav::after {
  content: '';
  position: absolute;
  bottom: 0;
  right: 0;
  left: 0;
  height: 2px;
  background: linear-gradient(to left, transparent, rgba(99, 179, 237, .12), transparent);
}

.step-item {
  flex: 1;
  display: flex;
  align-items: center;
  position: relative
}

.step-connector {
  flex: 1;
  height: 1px;
  background: rgba(255, 255, 255, .07);
  margin: 0 10px;
  transition: background .4s
}

.step-item.step-done .step-connector,
.step-item.step-active .step-connector {
  background: var(--green)
}

.step-node {
  display: flex;
  align-items: center;
  gap: 10px;
  white-space: nowrap
}

.step-node.clickable {
  cursor: pointer
}

.step-node.clickable:hover .step-circle {
  transform: scale(1.05)
}

.step-circle {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 2px solid rgba(255, 255, 255, .1);
  color: var(--muted2);
  transition: all .3s;
}

.step-item.step-done .step-circle {
  background: var(--green);
  border-color: var(--green);
  color: white
}

.step-item.step-active .step-circle {
  background: linear-gradient(135deg, var(--accent), var(--accent2));
  border-color: var(--accent);
  color: white;
  box-shadow: 0 0 18px rgba(99, 179, 237, .3)
}

.step-text {
  display: flex;
  flex-direction: column
}

.step-label {
  font-size: 12px;
  font-weight: 700;
  color: var(--muted);
  transition: color .3s
}

.step-item.step-done .step-label {
  color: var(--green)
}

.step-item.step-active .step-label {
  color: var(--accent)
}

.step-desc {
  font-size: 10px;
  color: var(--muted2);
  margin-top: 1px
}

/* ── Progress ── */
.progress-track {
  height: 3px;
  background: rgba(255, 255, 255, .04);
  border-radius: 99px;
  overflow: hidden
}

.progress-fill {
  height: 100%;
  background: linear-gradient(to left, var(--accent), var(--green));
  transition: width .5s cubic-bezier(.4, 0, .2, 1);
  border-radius: 99px
}

/* ── Form Card ── */
.form-card {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  overflow: hidden;
  box-shadow: var(--shadow)
}

.form-card-header {
  padding: 22px 32px 18px;
  border-bottom: 1px solid var(--border);
  display: flex;
  align-items: center;
  gap: 14px;
  background: linear-gradient(to left, transparent, rgba(99, 179, 237, .03));
  position: relative;
}

.form-card-header::after {
  content: '';
  position: absolute;
  bottom: 0;
  right: 0;
  left: 0;
  height: 1px;
  background: linear-gradient(to left, transparent, rgba(99, 179, 237, .12), transparent)
}

.section-icon {
  width: 44px;
  height: 44px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0
}

.icon-blue {
  background: rgba(99, 179, 237, .1);
  border: 1px solid rgba(99, 179, 237, .2);
  color: var(--accent) !important
}

.icon-green {
  background: rgba(72, 187, 120, .1);
  border: 1px solid rgba(72, 187, 120, .2);
  color: var(--green) !important
}

.icon-amber {
  background: rgba(246, 173, 85, .1);
  border: 1px solid rgba(246, 173, 85, .2);
  color: var(--amber) !important
}

.icon-purple {
  background: rgba(183, 148, 244, .1);
  border: 1px solid rgba(183, 148, 244, .2);
  color: var(--purple) !important
}

.section-meta {
  flex: 1
}

.section-title {
  font-size: 15px;
  font-weight: 800
}

.section-sub {
  font-size: 11.5px;
  color: var(--muted);
  margin-top: 3px
}

.section-badge {
  padding: 4px 12px;
  border-radius: 99px;
  font-size: 10.5px;
  font-weight: 700;
  background: rgba(99, 179, 237, .08);
  border: 1px solid rgba(99, 179, 237, .15);
  color: var(--accent);
  white-space: nowrap
}

.badge-success {
  background: rgba(72, 187, 120, .08) !important;
  border-color: rgba(72, 187, 120, .2) !important;
  color: var(--green) !important
}

/* ── Form Body ── */
.form-body {
  padding: 28px 32px
}

/* ── Group ── */
.group-block {
  margin-bottom: 28px
}

.group-label {
  font-size: 10.5px;
  font-weight: 800;
  color: var(--muted2);
  letter-spacing: .8px;
  text-transform: uppercase;
  margin-bottom: 14px;
  display: flex;
  align-items: center;
  gap: 8px;
}

.group-label::after {
  content: '';
  flex: 1;
  height: 1px;
  background: var(--border)
}

.group-unit {
  font-weight: 400;
  color: var(--muted2);
  letter-spacing: 0;
  font-size: 10px
}

/* ── PField ── */
.p-field {
  display: flex;
  flex-direction: column;
  gap: 5px
}

.p-label {
  font-size: 11.5px;
  font-weight: 600;
  color: var(--muted);
  display: flex;
  align-items: center;
  gap: 5px
}

.p-required {
  color: var(--red);
  font-size: 13px;
  line-height: 1
}

.p-hint {
  font-size: 10px;
  color: var(--muted2);
  margin-right: auto
}

/* ── RadioCard ── */
.radio-group {
  display: flex;
  flex-wrap: wrap;
  gap: 7px
}

.radio-group-lg .radio-card {
  padding: 10px 18px
}

.radio-card {
  display: flex;
  align-items: center;
  gap: 7px;
  padding: 8px 13px;
  border-radius: 9px;
  background: var(--surface2);
  border: 1.5px solid rgba(255, 255, 255, .07);
  cursor: pointer;
  transition: all .2s;
  font-size: 12px;
  font-weight: 600;
  color: var(--muted);
}

.radio-card:hover {
  border-color: rgba(99, 179, 237, .25);
  color: var(--text)
}

.radio-card.selected {
  background: rgba(99, 179, 237, .08);
  border-color: rgba(99, 179, 237, .35);
  color: var(--accent)
}

.radio-dot {
  width: 14px;
  height: 14px;
  border-radius: 50%;
  border: 2px solid var(--muted2);
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all .2s;
  flex-shrink: 0
}

.radio-dot.active {
  border-color: var(--accent);
  background: var(--accent)
}

.radio-inner {
  width: 5px;
  height: 5px;
  border-radius: 50%;
  background: white
}

/* ── NumStepper ── */
.num-stepper {
  display: flex;
  align-items: center;
  background: var(--surface2);
  border: 1.5px solid rgba(255, 255, 255, .07);
  border-radius: 10px;
  overflow: hidden;
  width: 140px
}

.num-btn {
  width: 38px;
  height: 40px;
  background: none;
  border: none;
  color: var(--muted);
  cursor: pointer;
  font-size: 18px;
  font-weight: 300;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all .15s;
  flex-shrink: 0
}

.num-btn:hover {
  background: rgba(255, 255, 255, .04);
  color: var(--text)
}

.num-val {
  flex: 1;
  text-align: center;
  font-size: 14px;
  font-weight: 700;
  color: var(--text)
}

/* ── ToggleRow ── */
.toggles-stack {
  display: flex;
  flex-direction: column;
  gap: 8px;
  margin-top: 12px
}

.toggle-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  background: var(--surface2);
  border: 1.5px solid rgba(255, 255, 255, .06);
  border-radius: 10px
}

.toggle-info {
  display: flex;
  flex-direction: column;
  gap: 3px
}

.toggle-title {
  font-size: 12.5px;
  font-weight: 600
}

.toggle-desc {
  font-size: 11px;
  color: var(--muted)
}

.toggle-switch {
  position: relative;
  width: 44px;
  height: 24px;
  cursor: pointer;
  flex-shrink: 0
}

.toggle-switch input {
  display: none
}

.toggle-track {
  position: absolute;
  inset: 0;
  border-radius: 99px;
  background: var(--surface3);
  border: 1.5px solid rgba(255, 255, 255, .08);
  transition: all .2s
}

.toggle-switch input:checked~.toggle-track {
  background: rgba(99, 179, 237, .2);
  border-color: rgba(99, 179, 237, .4)
}

.toggle-thumb {
  position: absolute;
  top: 3px;
  right: 3px;
  width: 16px;
  height: 16px;
  border-radius: 50%;
  background: var(--muted2);
  transition: all .25s cubic-bezier(.4, 0, .2, 1)
}

.toggle-switch input:checked~.toggle-track .toggle-thumb {
  right: 23px;
  background: var(--accent)
}

/* ── Info box ── */
.info-box {
  display: flex;
  gap: 10px;
  padding: 12px 14px;
  background: rgba(99, 179, 237, .05);
  border: 1px solid rgba(99, 179, 237, .12);
  border-radius: 9px;
  margin-bottom: 20px
}

.info-box p {
  font-size: 11.5px;
  color: var(--muted);
  line-height: 1.7
}

.info-box strong {
  color: var(--accent)
}

/* ── IBAN ── */
.iban-ir {
  font-size: 12px;
  font-weight: 800;
  color: var(--accent);
  padding-left: 6px;
  border-left: 1px solid rgba(255, 255, 255, .1);
  margin-left: 4px
}

/* ── Salary total ── */
.salary-total {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: 16px;
  padding: 12px 16px;
  background: linear-gradient(to left, rgba(99, 179, 237, .05), rgba(72, 187, 120, .04));
  border: 1px solid rgba(99, 179, 237, .15);
  border-radius: 10px;
}

.salary-total-label {
  font-size: 12px;
  font-weight: 700;
  color: var(--muted)
}

.salary-total-val {
  font-size: 15px;
  font-weight: 800;
  color: var(--green);
  font-variant-numeric: tabular-nums
}

/* ── Summary ── */
.summary-section {
  margin-bottom: 22px
}

.summary-section-title {
  font-size: 10.5px;
  font-weight: 800;
  color: var(--muted2);
  letter-spacing: .7px;
  text-transform: uppercase;
  margin-bottom: 10px;
  display: flex;
  align-items: center;
  gap: 8px
}

.summary-section-title::after {
  content: '';
  flex: 1;
  height: 1px;
  background: var(--border)
}

.summary-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 8px
}

.summary-row {
  display: flex;
  flex-direction: column;
  gap: 2px;
  padding: 10px 14px;
  background: var(--surface2);
  border-radius: 9px;
  border: 1px solid var(--border);
  position: relative
}

.summary-row:hover .summary-edit {
  opacity: 1
}

.summary-key {
  font-size: 10.5px;
  color: var(--muted2);
  font-weight: 600
}

.summary-val {
  font-size: 12.5px;
  font-weight: 600
}

.summary-val.empty {
  color: var(--muted2);
  font-style: italic
}

.summary-edit {
  position: absolute;
  top: 8px;
  left: 8px;
  background: rgba(99, 179, 237, .08);
  border: 1px solid rgba(99, 179, 237, .2);
  border-radius: 5px;
  padding: 3px 6px;
  color: var(--accent);
  cursor: pointer;
  opacity: 0;
  transition: opacity .2s;
  font-size: 11px
}

/* ── Actions ── */
.form-actions {
  padding: 18px 32px 24px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-top: 1px solid var(--border);
  background: rgba(0, 0, 0, .12)
}

.actions-right,
.actions-left {
  display: flex;
  align-items: center;
  gap: 10px
}

.step-counter {
  font-size: 11.5px;
  color: var(--muted);
  font-weight: 600
}

.step-counter b {
  color: var(--accent)
}

/* ── Buttons ── */
.btn {
  display: inline-flex;
  align-items: center;
  gap: 7px;
  border-radius: 10px;
  font-family: var(--font);
  font-weight: 700;
  cursor: pointer;
  border: none;
  transition: all .2s;
  font-size: 12.5px;
  padding: 10px 20px;
  white-space: nowrap
}

.btn-ghost {
  background: transparent;
  color: var(--muted);
  border: 1.5px solid var(--border)
}

.btn-ghost:hover {
  border-color: rgba(255, 255, 255, .15);
  color: var(--text)
}

.btn-primary {
  background: linear-gradient(135deg, #4299e1, #3182ce);
  color: white;
  box-shadow: 0 4px 16px rgba(66, 153, 225, .2)
}

.btn-primary:hover {
  transform: translateY(-1px);
  box-shadow: 0 6px 22px rgba(66, 153, 225, .3)
}

.btn-success {
  background: linear-gradient(135deg, #48bb78, #38a169);
  color: white;
  box-shadow: 0 4px 16px rgba(72, 187, 120, .2);
  padding: 10px 24px
}

.btn-success:hover {
  transform: translateY(-1px);
  box-shadow: 0 6px 22px rgba(72, 187, 120, .3)
}

.btn:disabled {
  opacity: .5;
  cursor: not-allowed;
  transform: none !important
}

.btn-spinner {
  width: 14px;
  height: 14px;
  border: 2px solid rgba(255, 255, 255, .3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin .7s linear infinite
}

@keyframes spin {
  to {
    transform: rotate(360deg)
  }
}

/* ── Toast ── */
.toast {
  position: fixed;
  bottom: 28px;
  left: 50%;
  transform: translateX(-50%);
  background: var(--surface3);
  border: 1px solid var(--border);
  border-radius: 12px;
  padding: 12px 20px;
  display: flex;
  align-items: center;
  font-size: 12.5px;
  font-weight: 600;
  box-shadow: 0 12px 40px rgba(0, 0, 0, .5);
  z-index: 9999;
  white-space: nowrap
}

.toast-success {
  border-color: rgba(72, 187, 120, .3);
  color: var(--green)
}

.toast-error {
  border-color: rgba(252, 129, 129, .3);
  color: var(--red)
}

.toast-anim-enter-active,
.toast-anim-leave-active {
  transition: all .3s
}

.toast-anim-enter-from,
.toast-anim-leave-to {
  opacity: 0;
  transform: translateX(-50%) translateY(10px)
}

/* ── Step fade ── */
.step-fade-enter-active {
  animation: fadeUp .3s ease
}

@keyframes fadeUp {
  from {
    opacity: 0;
    transform: translateY(8px)
  }

  to {
    opacity: 1;
    transform: translateY(0)
  }
}

/* ── Responsive ── */
@media(max-width:600px) {
  .stepper-nav {
    padding: 14px 12px
  }

  .step-desc {
    display: none
  }

  .step-label {
    font-size: 10.5px
  }

  .form-body {
    padding: 18px 16px
  }

  .form-actions {
    flex-direction: column;
    gap: 12px;
    padding: 16px
  }

  .actions-right,
  .actions-left {
    width: 100%;
    justify-content: center
  }

  .summary-grid {
    grid-template-columns: 1fr
  }
}
</style>
