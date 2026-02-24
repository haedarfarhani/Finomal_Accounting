<template>
  <v-navigation-drawer v-model="drawer"
                       :rail="rail"
                       location="right"
                       permanent
                       elevation="0"
                       class="sidebar-drawer"
                       :width="280">
    <div class="sidebar-header d-flex align-center pa-4">
      <v-avatar color="primary-darken-1" size="38" rounded="lg" class="logo-container">
        <v-icon color="cyan-accent-2" size="24">mdi-chart-box-outline</v-icon>
      </v-avatar>

      <div v-if="!rail" class="brand-wrapper ms-3">
        <div class="brand-title">فینومال</div>
        <div class="brand-subtitle">سیستم یکپارچه مالی</div>
      </div>

      <v-spacer />

      <v-btn :icon="rail ? 'mdi-menu' : 'mdi-chevron-right'"
             variant="tonal"
             size="x-small"
             color="slate-400"
             @click="rail = !rail"
             class="toggle-btn" />
    </div>

    <v-divider class="mx-4 opacity-10" />

    <v-list nav
            class="px-2 mt-2"
            :opened="openedGroups"
            expand-strategy="single">
      <template v-for="menu in menuList" :key="menu.title">
        <v-list-group :value="menu.title" v-if="!rail">
          <template #activator="{ props }">
            <v-list-item v-bind="props"
                         :prepend-icon="menu.icon"
                         :title="menu.title"
                         class="menu-item py-2"
                         rounded="lg" />
          </template>

          <v-list-item v-for="sub in menu.subMenus"
                       :key="sub.title"
                       :title="sub.title"
                       :to="sub.href"
                       class="submenu-item ps-10"
                       density="compact"
                       @click="onSubMenuClick(sub.title)">
            <template #prepend>
              <div class="dot-indicator" />
            </template>
          </v-list-item>
        </v-list-group>

        <v-list-item v-else
                     :prepend-icon="menu.icon"
                     :value="menu.title"
                     class="menu-item py-3"
                     rounded="lg"
                     @click="rail = false">
          <v-tooltip activator="parent" location="left">{{ menu.title }}</v-tooltip>
        </v-list-item>
      </template>
    </v-list>

    <template #append>
      <div class="footer-container">
        <v-divider class="mx-4 opacity-10" />
        <div class="sidebar-footer pa-4 d-flex align-center">
          <v-badge dot color="success" offset-x="3" offset-y="3">
            <v-avatar size="36" class="user-avatar shadow-sm">
              <v-img src="https://cdn.vuetifyjs.com/images/john.jpg" alt="User" />
            </v-avatar>
          </v-badge>

          <div v-if="!rail" class="ms-3 overflow-hidden">
            <div class="user-name text-truncate">مدیر سیستم</div>
            <div class="user-role text-truncate">ادمین ارشد مالی</div>
          </div>

          <v-spacer v-if="!rail" />

          <v-btn v-if="!rail"
                 icon="mdi-power"
                 variant="text"
                 size="small"
                 color="error"
                 class="logout-btn" />
        </div>
      </div>
    </template>
  </v-navigation-drawer>
</template>

<script setup>
  import { ref, watch } from 'vue'

  const emit = defineEmits(['menu-selected'])
  const drawer = ref(true)
  const rail = ref(false)
  const openedGroups = ref([])

  // وقتی منو جمع می‌شود، تمام گروه‌های باز را می‌بندیم تا در باز شدن مجدد تمیز باشد
  watch(rail, (newVal) => {
    if (newVal) openedGroups.value = []
  })

  function onSubMenuClick(title) {
    emit('menu-selected', title)
  }

  const menuList = ref([

    {

      title: 'حسابداری',

      icon: 'mdi-book-open-variant',

      subMenus: [

        { title: 'اسناد حسابداری', href: 'CreateDocument' },

        { title: 'مرور حساب‌ها', href: 'account-review' },

        { title: 'دفاتر قانونی', href: 'legal-books' },

        { title: 'عملیات پایان سال', href: 'year-end' },

        { title: 'صدور سند اختتامیه و افتتاحیه', href: 'opening-closing' },

        { title: 'تعریف سرفصل‌های حساب', href: 'account-coding' },

      ],

    },

    {

      title: 'انبارداری',

      icon: 'mdi-warehouse',

      subMenus: [

        { title: 'تعریف انبار', href: 'CreateWarehouse' },

        { title: 'تعریف کالا و خدمات', href: 'Products' },

        { title: 'رسید انبار', href: 'WarehouseReceipt' },

        { title: 'حواله انبار', href: 'WarehouseIssue' },

        { title: 'انتقال بین انبارها', href: 'WarehouseTransfer' },

        { title: 'انبارگردانی و شمارش', href: 'InventoryCounting' },

        { title: 'گزارش کاردکس کالا', href: 'cardex-report' },

        { title: 'گزارش موجودی انبار', href: 'inventory-report' },

      ],

    },

    {

      title: 'فروش و مشتریان',

      icon: 'mdi-receipt',

      subMenus: [

        { title: 'تعریف مشتریان', href: 'customers' },

        { title: 'سفارش فروش', href: 'sales-order' },

        { title: 'پیش‌فاکتور فروش', href: 'proforma' },

        { title: 'فاکتور فروش', href: 'invoice' },

        { title: 'برگشت از فروش', href: 'sales-return' },

        { title: 'گزارش‌های فروش', href: 'sales-reports' },

      ],

    },

    {

      title: 'حقوق و دستمزد',

      icon: 'mdi-account-group',

      subMenus: [

        { title: 'تعریف پرسنل', href: 'PersonnelInfo' },

        { title: 'تعریف قرارداد پرسنل', href: 'Contracts' },

        { title: 'ثبت کارکرد ماهانه', href: 'AttendanceCalculation' },

        { title: 'محاسبه حقوق و دستمزد', href: 'PayrollCalculation' },

        { title: 'صدور فیش حقوقی', href: 'payslip' },

        { title: 'تهیه فایل‌های بیمه و مالیات', href: 'insurance-tax' },

        { title: 'تسویه حساب پرسنل', href: 'personnel-settlement' },

      ],

    },

    {

      title: 'تنظیمات',

      icon: 'mdi-cog-outline',

      subMenus: [

        { title: 'اطلاعات پایه شرکت', href: 'company-info' },

        { title: 'مدیریت کاربران', href: 'user-management' },

        { title: 'تنظیمات دوره مالی', href: 'fiscal-period' },

        { title: 'پشتیبان‌گیری و بازیابی', href: 'backup' },

      ],

    },

  ])
</script>

<style scoped>
  /* کانتینر اصلی با پس‌زمینه تیره و مدرن */
  :deep(.v-navigation-drawer) {
    background: #0d1117 !important;
    border-left: 1px solid rgba(255, 255, 255, 0.05) !important;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1) !important;
  }

  .brand-title {
    font-size: 1.1rem;
    font-weight: 800;
    letter-spacing: 0.5px;
    color: #f8fafc;
  }

  .brand-subtitle {
    font-size: 0.7rem;
    color: #64748b;
    font-weight: 500;
  }

  .logo-container {
    background: linear-gradient(135deg, #1e293b 0%, #0f172a 100%);
    border: 1px solid rgba(56, 189, 248, 0.2);
  }

  /* استایل آیتم‌های منو */
  .menu-item {
    color: #94a3b8 !important;
    margin-bottom: 4px;
  }

  :deep(.v-list-item__prepend > .v-icon) {
    opacity: 1;
    margin-inline-end: 16px !important;
  }

  .menu-item:hover {
    background: rgba(56, 189, 248, 0.08) !important;
    color: #38bdf8 !important;
  }

  :deep(.v-list-group--active > .v-list-group__header) {
    color: #38bdf8 !important;
    background: rgba(56, 189, 248, 0.04) !important;
  }

  /* استایل زیرمنوها */
  .submenu-item {
    min-height: 38px !important;
    font-size: 0.85rem !important;
    color: #64748b !important;
    position: relative;
  }

  .dot-indicator {
    width: 4px;
    height: 4px;
    border-radius: 50%;
    background: currentColor;
    margin-inline-end: 12px;
    opacity: 0.5;
  }

  .submenu-item:hover {
    color: #e2e8f0 !important;
  }

  :deep(.v-list-item--active.submenu-item) {
    color: #38bdf8 !important;
    font-weight: 600;
  }

  /* بخش فوتر و کاربر */
  .user-name {
    font-size: 0.85rem;
    font-weight: 700;
    color: #f1f5f9;
  }

  .user-role {
    font-size: 0.7rem;
    color: #475569;
  }

  .user-avatar {
    border: 2px solid rgba(56, 189, 248, 0.2);
  }

  /* اصلاح جهت فلش بازشونده در RTL */
  :deep(.v-list-group__header .v-list-item__append .v-icon) {
    font-size: 1.2rem;
    transition: transform 0.2s;
  }

  :deep(.v-list-group--open .v-list-item__append .v-icon) {
    transform: rotate(-180deg) !important;
  }

  /* Scrollbar شخصی‌سازی شده */
  :deep(.v-navigation-drawer__content::-webkit-scrollbar) {
    width: 4px;
  }

  :deep(.v-navigation-drawer__content::-webkit-scrollbar-thumb) {
    background: #1e293b;
    border-radius: 10px;
  }
</style>
