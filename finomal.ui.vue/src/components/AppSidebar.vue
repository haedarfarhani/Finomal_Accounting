<template>
  <aside class="sidebar" :class="{ collapsed: rail }" dir="rtl">

    <!-- ══ HEADER ══ -->
    <div class="sb-header">
      <div class="sb-logo">
        <div class="sb-logo-gem">
          <v-icon size="16" color="#38bdf8">mdi-chart-finance</v-icon>
        </div>
        <transition name="sb-fade">
          <div v-if="!rail" class="sb-brand">
            <div class="sb-brand-name">فینومال</div>
            <div class="sb-brand-tag">سیستم یکپارچه مالی</div>
          </div>
        </transition>
      </div>
      <button class="sb-toggle" @click="rail = !rail">
        <v-icon size="16">{{ rail ? 'mdi-menu-open' : 'mdi-menu' }}</v-icon>
      </button>
    </div>

    <div class="sb-divider" />

    <!-- ══ NAV ══ -->
    <nav class="sb-nav">
      <template v-for="(item, idx) in menuList" :key="item.title">

        <!-- Section label -->
        <div v-if="item.separator && !rail" class="sb-section-label">{{ item.separator }}</div>

        <!-- Menu group -->
        <div class="sb-group" :class="{ 'sb-group--open': openGroup === item.title }">

          <!-- Group header -->
          <button class="sb-item" :class="{ 'sb-item--active-group': openGroup === item.title }"
            @click="toggleGroup(item.title)" :title="rail ? item.title : ''">
            <div class="sb-item-icon" :style="{ '--g': item.glow }">
              <v-icon size="16" :class="`sb-color-${item.color}`">{{ item.icon }}</v-icon>
            </div>
            <transition name="sb-fade">
              <span v-if="!rail" class="sb-item-title">{{ item.title }}</span>
            </transition>
            <transition name="sb-fade">
              <v-icon v-if="!rail" size="13" class="sb-chevron">
                {{ openGroup === item.title ? 'mdi-chevron-up' : 'mdi-chevron-down' }}
              </v-icon>
            </transition>
          </button>

          <!-- Sub items -->
          <transition name="sb-expand">
            <div v-if="!rail && openGroup === item.title" class="sb-sub-list">
              <button v-for="sub in item.subMenus" :key="sub.title" class="sb-sub-item"
                :class="{ 'sb-sub-item--active': sub.path && isActive(sub.path) }" @click="navigate(sub)">
                <span class="sb-sub-dot" />
                <span class="sb-sub-title">{{ sub.title }}</span>
                <span v-if="sub.tag" class="sb-sub-tag">{{ sub.tag }}</span>
              </button>
            </div>
          </transition>

        </div>
      </template>
    </nav>

    <!-- ══ FOOTER ══ -->
    <div class="sb-footer">
      <div class="sb-divider" style="margin-bottom: 10px;" />
      <div class="sb-user">
        <div class="sb-user-av">
          <img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="user" />
          <span class="sb-online" />
        </div>
        <transition name="sb-fade">
          <div v-if="!rail" class="sb-user-info">
            <div class="sb-user-name">{{ userName }}</div>
            <div class="sb-user-role">{{ userRole }}</div>
          </div>
        </transition>
        <transition name="sb-fade">
          <button v-if="!rail" class="sb-logout" @click="$emit('logout')" title="خروج">
            <v-icon size="15">mdi-logout-variant</v-icon>
          </button>
        </transition>
      </div>
    </div>

  </aside>
</template>

<script setup>
import { ref, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()

const props = defineProps({
  userName: { type: String, default: 'مدیر سیستم' },
  userRole: { type: String, default: 'ادمین ارشد مالی' },
})

defineEmits(['logout'])

const rail = ref(false)
const openGroup = ref(null)

function toggleGroup(title) {
  if (rail.value) {
    rail.value = false
    openGroup.value = title
  } else {
    openGroup.value = openGroup.value === title ? null : title
  }
}

function isActive(path) {
  if (!path) return false
  return route.path === path || (path !== '/' && route.path.startsWith(path))
}

function navigate(sub) {
  if (sub.path) {
    router.push(sub.path)
  }
}

// ── Menu Data ──────────────────────────────────────────────────────────
const menuList = ref([
  {
    title: 'حسابداری', icon: 'mdi-book-open-variant-outline', color: 'blue', glow: '56,189,248',
    separator: 'عملیات مالی',
    subMenus: [
      { title: 'صدور سند حسابداری', path: '/accounting-document/create' },
      { title: 'لیست اسناد', path: '/accounting-document/list' },
      { title: 'مرور حساب‌ها' },
      { title: 'دفتر روزنامه' },
      { title: 'دفتر کل' },
      { title: 'تراز آزمایشی' },
      { title: 'تعریف سرفصل‌ها', path: '/chart-of-accounts' },
      { title: 'سال مالی', path: '/fiscal-year' },
      { title: 'عملیات پایان سال' },
    ],
  },
  {
    title: 'خزانه‌داری', icon: 'mdi-cash-multiple', color: 'emerald', glow: '52,211,153',
    subMenus: [
      { title: 'تعریف صندوق و بانک', path: '/treasury/banks-cash' },
      { title: 'دریافت وجه', path: '/treasury/receipts' },
      { title: 'پرداخت وجه', path: '/treasury/payments' },
      { title: 'انتقال بین حساب‌ها' },
      { title: 'مدیریت چک‌ها', path: '/treasury/cheques' },
      { title: 'گردش صندوق' },
    ],
  },
  {
    title: 'فروش', icon: 'mdi-receipt-text-outline', color: 'violet', glow: '167,139,250',
    separator: 'تجاری',
    subMenus: [
      { title: 'تعریف مشتریان', path: '/sales/customers' },
      { title: 'پیش‌فاکتور', path: '/sales/proforma' },
      { title: 'فاکتور فروش', path: '/sales/invoice' },
      { title: 'برگشت از فروش', path: '/sales/returns' },
      { title: 'گزارش‌های فروش', path: '/sales/reports' },
    ],
  },
  {
    title: 'خرید', icon: 'mdi-cart-outline', color: 'orange', glow: '251,146,60',
    subMenus: [
      { title: 'تعریف تامین‌کنندگان', path: '/purchase/suppliers' },
      { title: 'سفارش خرید', path: '/purchase/orders' },
      { title: 'فاکتور خرید', path: '/purchase/invoice' },
      { title: 'برگشت از خرید', path: '/purchase/returns' },
    ],
  },
  {
    title: 'انبارداری', icon: 'mdi-warehouse', color: 'amber', glow: '251,191,36',
    subMenus: [
      { title: 'تعریف انبار', path: '/inventory/warehouses' },
      { title: 'تعریف کالا', path: '/inventory/products' },
      { title: 'رسید انبار', path: '/inventory/receipts' },
      { title: 'حواله انبار', path: '/inventory/issues' },
      { title: 'انتقال بین انبارها', path: '/inventory/transfers' },
      { title: 'کاردکس کالا', path: '/inventory/kardex' },
      { title: 'موجودی انبار', path: '/inventory/stock' },
    ],
  },
  {
    title: 'حقوق و دستمزد', icon: 'mdi-account-group-outline', color: 'pink', glow: '244,114,182',
    separator: 'پرسنلی',
    subMenus: [
      { title: 'لیست پرسنل', path: '/personnel' },
      { title: 'تعریف پرسنل', path: '/personnel/create-personnel' },
      { title: 'قراردادها', path: '/personnel/contracts' },
      { title: 'کارکرد ماهانه', path: '/personnel/attendance' },
      { title: 'وام و مساعده' },
      { title: 'محاسبه حقوق', path: '/personnel/payroll' },
      { title: 'فیش حقوقی', path: '/personnel/payslip' },
      { title: 'دیسکت بیمه و مالیات' },
      { title: 'تسویه حساب پرسنل' },
    ],
  },
  {
    title: 'گزارش‌ها', icon: 'mdi-chart-bar', color: 'cyan', glow: '34,211,238',
    separator: 'تحلیل',
    subMenus: [
      { title: 'سود و زیان', path: '/reports/income-statement' },
      { title: 'ترازنامه', path: '/reports/balance-sheet' },
      { title: 'جریان وجه نقد', path: '/reports/cash-flow' },
      { title: 'گزارش مودیان', path: '/reports/taxpayers' },
    ],
  },
  {
    title: 'تنظیمات', icon: 'mdi-cog-outline', color: 'slate', glow: '148,163,184',
    separator: 'سیستم',
    subMenus: [
      { title: 'اطلاعات شرکت' },
      { title: 'مدیریت کاربران', path: '/setting/user-managment' },
      { title: 'دوره مالی' },
      { title: 'پشتیبان‌گیری' },
    ],
  },
])

// Auto-open the active group based on current route (must be after menuList is defined)
watch(() => route.path, (currentPath) => {
  for (const item of menuList.value) {
    if (item.subMenus?.some(s => s.path && (currentPath === s.path || (s.path !== '/' && currentPath.startsWith(s.path))))) {
      if (openGroup.value !== item.title) {
        openGroup.value = item.title
      }
      return
    }
  }
}, { immediate: true })
</script>

<style scoped>
/* ── Root ─────────────────────────────────────────────────────────── */
.sidebar {
  width: 272px;
  height: 100%;
  background: #080c14;
  border-left: 1px solid rgba(255, 255, 255, 0.055);
  box-shadow: -12px 0 40px rgba(0, 0, 0, 0.4);
  display: flex;
  flex-direction: column;
  flex-shrink: 0;
  font-family: 'Vazirmatn', sans-serif;
  direction: rtl;
  transition: width 0.28s cubic-bezier(0.4, 0, 0.2, 1);
  overflow: hidden;
}

.sidebar.collapsed {
  width: 64px;
}

/* ── Header ───────────────────────────────────────────────────────── */
.sb-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 14px 12px 12px;
  min-height: 62px;
  flex-shrink: 0;
}

.sidebar.collapsed .sb-header {
  justify-content: center;
  padding: 14px 8px;
}

.sb-logo {
  display: flex;
  align-items: center;
  gap: 10px;
  overflow: hidden;
}

.sb-logo-gem {
  width: 36px;
  height: 36px;
  border-radius: 11px;
  background: linear-gradient(145deg, #0c1e35, #0a1628);
  border: 1px solid rgba(56, 189, 248, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  box-shadow: 0 0 12px rgba(56, 189, 248, 0.15);
}

.sb-brand {
  overflow: hidden;
}

.sb-brand-name {
  font-size: 14px;
  font-weight: 800;
  color: #e2e8f0;
  white-space: nowrap;
}

.sb-brand-tag {
  font-size: 9.5px;
  color: #2a3a4a;
  font-weight: 500;
  margin-top: 1px;
  white-space: nowrap;
}

.sb-toggle {
  width: 28px;
  height: 28px;
  border-radius: 8px;
  border: 1px solid rgba(255, 255, 255, 0.06);
  background: rgba(255, 255, 255, 0.04);
  color: #334155;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: all 0.18s;
}

.sb-toggle:hover {
  border-color: rgba(56, 189, 248, 0.3);
  color: #38bdf8;
  background: rgba(56, 189, 248, 0.07);
}

.sidebar.collapsed .sb-toggle {
  margin-top: 8px;
}

/* ── Divider ──────────────────────────────────────────────────────── */
.sb-divider {
  height: 1px;
  background: linear-gradient(to left, transparent, rgba(255, 255, 255, 0.06), transparent);
  flex-shrink: 0;
}

/* ── Nav ──────────────────────────────────────────────────────────── */
.sb-nav {
  flex: 1;
  overflow-y: auto;
  overflow-x: hidden;
  padding: 10px 8px;
  scrollbar-width: thin;
  scrollbar-color: #1a2535 transparent;
}

.sb-nav::-webkit-scrollbar {
  width: 3px;
}

.sb-nav::-webkit-scrollbar-thumb {
  background: #1a2535;
  border-radius: 10px;
}

/* Section label */
.sb-section-label {
  font-size: 9px;
  font-weight: 800;
  letter-spacing: 1px;
  color: #1e3a4a;
  text-transform: uppercase;
  padding: 12px 10px 5px;
}

/* Group */
.sb-group {
  margin-bottom: 1px;
}

/* Item (group header) */
.sb-item {
  width: 100%;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 0 8px;
  height: 40px;
  border-radius: 10px;
  border: none;
  background: transparent;
  color: #3d5166;
  cursor: pointer;
  transition: all 0.18s;
  font-family: 'Vazirmatn', sans-serif;
  text-align: right;
}

.sb-item:hover {
  background: rgba(255, 255, 255, 0.04);
  color: #64748b;
}

.sb-item--active-group {
  background: rgba(56, 189, 248, 0.07) !important;
  color: #94a3b8 !important;
}

/* Icon */
.sb-item-icon {
  width: 30px;
  height: 30px;
  border-radius: 8px;
  border: 1px solid rgba(255, 255, 255, 0.05);
  background: rgba(255, 255, 255, 0.03);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: all 0.2s;
}

.sb-item:hover .sb-item-icon,
.sb-item--active-group .sb-item-icon {
  border-color: rgba(56, 189, 248, 0.2);
  background: rgba(56, 189, 248, 0.07);
  box-shadow: 0 0 12px rgba(var(--g), 0.15);
}

/* Icon colors */
.sb-color-blue {
  color: #38bdf8 !important;
}

.sb-color-emerald {
  color: #34d399 !important;
}

.sb-color-violet {
  color: #a78bfa !important;
}

.sb-color-orange {
  color: #fb923c !important;
}

.sb-color-amber {
  color: #fbbf24 !important;
}

.sb-color-pink {
  color: #f472b6 !important;
}

.sb-color-cyan {
  color: #22d3ee !important;
}

.sb-color-slate {
  color: #94a3b8 !important;
}

/* Item title */
.sb-item-title {
  flex: 1;
  font-size: 12.5px;
  font-weight: 600;
  white-space: nowrap;
  overflow: hidden;
  color: inherit;
  transition: color 0.18s;
  text-align: right;
}

.sb-item:hover .sb-item-title {
  color: #94a3b8;
}

.sb-item--active-group .sb-item-title {
  color: #cbd5e1 !important;
}

/* Chevron */
.sb-chevron {
  color: #253347 !important;
  flex-shrink: 0;
  transition: color 0.2s !important;
}

.sb-item--active-group .sb-chevron {
  color: #38bdf8 !important;
}

/* Sub list */
.sb-sub-list {
  border-right: 1px solid rgba(255, 255, 255, 0.05);
  margin-right: 19px;
  margin-top: 2px;
  padding: 2px 0 6px;
  display: flex;
  flex-direction: column;
  gap: 1px;
}

/* Sub item */
.sb-sub-item {
  width: 100%;
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 0 14px;
  height: 31px;
  border-radius: 8px;
  border: none;
  background: transparent;
  color: #3d5166;
  cursor: pointer;
  font-family: 'Vazirmatn', sans-serif;
  font-size: 11.5px;
  font-weight: 500;
  text-align: right;
  transition: all 0.14s;
}

.sb-sub-item:hover {
  background: rgba(255, 255, 255, 0.03);
  color: #64748b;
}

.sb-sub-item:hover .sb-sub-dot {
  background: #475569;
}

/* Active sub */
.sb-sub-item--active {
  background: rgba(56, 189, 248, 0.09) !important;
  color: #38bdf8 !important;
  font-weight: 700 !important;
}

.sb-sub-item--active .sb-sub-dot {
  background: #38bdf8 !important;
  box-shadow: 0 0 6px rgba(56, 189, 248, 0.7) !important;
  width: 5px !important;
  height: 5px !important;
}

/* Sub dot */
.sb-sub-dot {
  display: inline-block;
  width: 3px;
  height: 3px;
  border-radius: 50%;
  background: #253347;
  flex-shrink: 0;
  transition: all 0.2s;
}

/* Sub tag */
.sb-sub-tag {
  font-size: 9.5px;
  padding: 2px 6px;
  border-radius: 99px;
  background: rgba(56, 189, 248, 0.1);
  color: #38bdf8;
  border: 1px solid rgba(56, 189, 248, 0.15);
  margin-right: auto;
}

/* Sub title */
.sb-sub-title {
  flex: 1;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

/* ── Footer ───────────────────────────────────────────────────────── */
.sb-footer {
  padding: 0 8px 10px;
  flex-shrink: 0;
}

.sb-user {
  display: flex;
  align-items: center;
  gap: 9px;
  padding: 8px 6px;
  border-radius: 10px;
  transition: background 0.2s;
}

.sb-user:hover {
  background: rgba(255, 255, 255, 0.03);
}

.sb-user-av {
  position: relative;
  flex-shrink: 0;
  width: 32px;
  height: 32px;
}

.sb-user-av img {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: 1.5px solid rgba(56, 189, 248, 0.2);
  object-fit: cover;
}

.sb-online {
  position: absolute;
  bottom: 0;
  right: 0;
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: #22c55e;
  border: 2px solid #080c14;
  box-shadow: 0 0 6px rgba(34, 197, 94, 0.5);
}

.sb-user-info {
  flex: 1;
  min-width: 0;
  overflow: hidden;
}

.sb-user-name {
  font-size: 12px;
  font-weight: 700;
  color: #94a3b8;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.sb-user-role {
  font-size: 10px;
  color: #253347;
  margin-top: 1px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.sb-logout {
  width: 28px;
  height: 28px;
  border-radius: 8px;
  border: none;
  background: transparent;
  color: #3d5166;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: all 0.18s;
}

.sb-logout:hover {
  color: #f87171;
  background: rgba(248, 113, 113, 0.08);
}

/* ── Collapse state icon-only ─────────────────────────────────────── */
.sidebar.collapsed .sb-item {
  justify-content: center;
  padding: 0;
}

.sidebar.collapsed .sb-item-icon {
  width: 36px;
  height: 36px;
}

.sidebar.collapsed .sb-user {
  justify-content: center;
  padding: 8px 4px;
}

.sidebar.collapsed .sb-toggle {
  width: 36px;
  height: 36px;
}

/* ── Transitions ──────────────────────────────────────────────────── */
.sb-fade-enter-active,
.sb-fade-leave-active {
  transition: opacity 0.2s, transform 0.2s;
}

.sb-fade-enter-from,
.sb-fade-leave-to {
  opacity: 0;
  transform: translateX(8px);
}

.sb-expand-enter-active {
  transition: all 0.22s ease-out;
}

.sb-expand-leave-active {
  transition: all 0.18s ease-in;
}

.sb-expand-enter-from,
.sb-expand-leave-to {
  opacity: 0;
  transform: translateY(-6px);
  max-height: 0;
}

.sb-expand-enter-to,
.sb-expand-leave-from {
  opacity: 1;
  max-height: 500px;
}
</style>
