<template>
  <div class="dashboard-root">
    <!-- Ambient background effects -->
    <div class="ambient-orb orb-1"></div>
    <div class="ambient-orb orb-2"></div>
    <div class="grid-overlay"></div>

    <div class="dashboard-scroll">
      <!-- Header -->
      <header class="dash-header">
        <div class="header-right">
          <div class="page-icon">
            <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="3" y="3" width="7" height="7" rx="1.5" />
              <rect x="14" y="3" width="7" height="7" rx="1.5" />
              <rect x="3" y="14" width="7" height="7" rx="1.5" />
              <rect x="14" y="14" width="7" height="7" rx="1.5" />
            </svg>
          </div>
          <div>
            <h1 class="page-title">داشبورد مدیریتی جامع فینومال</h1>
            <p class="page-subtitle">خلاصه‌ای از وضعیت مالی، فروش و انبار سازمان</p>
          </div>
        </div>
        <div class="header-left">
          <div class="date-badge">
            <span class="date-dot"></span>
            {{ currentDate }} - {{ currentTime }}
          </div>
          <button class="btn-new-tx" @click="createNewDoc">
            <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <path d="M12 5v14M5 12h14" />
            </svg>
            سند حسابداری جدید
          </button>
        </div>
      </header>

      <!-- Stats Cards Row 1 -->
      <section class="stats-grid">
        <div v-for="(card, i) in mainCards" :key="card.title" class="stat-card glass-panel hover-lift"
          :style="{ '--delay': i * 0.1 + 's' }">
          <div class="stat-card-top">
            <div class="stat-icon"
              :style="{ background: card.iconBg, color: card.iconColor, boxShadow: `0 0 15px ${card.iconBg}` }">
              <v-icon size="24">{{ card.icon }}</v-icon>
            </div>
            <div class="stat-badge" :class="card.positive ? 'badge-up' : 'badge-down'">
              <v-icon size="14" class="mr-1">{{ card.positive ? 'mdi-trending-up' : 'mdi-trending-down' }}</v-icon>
              {{ card.change }}٪
            </div>
          </div>
          <div class="stat-label">{{ card.title }}</div>
          <div class="stat-value mono-num">{{ card.value }} <span class="currency">ریال</span></div>
        </div>
      </section>

      <!-- Charts & Content Row -->
      <section class="main-content-grid mt-6">
        <!-- Main Chart -->
        <div class="glass-panel p-5 grid-span-2 slide-up" style="--delay: 0.4s">
          <div class="panel-header mb-4">
            <h3 class="panel-title"><v-icon color="#38bdf8" class="mr-2">mdi-chart-areaspline</v-icon> تحلیل روند درآمد
              و هزینه (سالیانه)</h3>
            <div class="toggle-group">
              <button class="toggle-btn active">ماهانه</button>
              <button class="toggle-btn">فصلی</button>
            </div>
          </div>
          <div class="chart-container">
            <!-- Simulated complex SVG chart for aesthetic -->
            <svg class="w-100 h-100" viewBox="0 0 1000 300" preserveAspectRatio="none">
              <defs>
                <linearGradient id="income-grad" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0%" stop-color="#4ade80" stop-opacity="0.4" />
                  <stop offset="100%" stop-color="#4ade80" stop-opacity="0" />
                </linearGradient>
                <linearGradient id="expense-grad" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0%" stop-color="#f87171" stop-opacity="0.4" />
                  <stop offset="100%" stop-color="#f87171" stop-opacity="0" />
                </linearGradient>
              </defs>
              <!-- Grid Lines -->
              <path d="M0 50 L1000 50 M0 150 L1000 150 M0 250 L1000 250" stroke="rgba(255,255,255,0.05)"
                stroke-width="1" stroke-dasharray="5 5" fill="none" />
              <!-- Areas -->
              <path d="M0 250 L0 180 Q100 140 200 170 T400 100 T600 120 T800 60 T1000 40 L1000 250 Z"
                fill="url(#income-grad)" />
              <path d="M0 250 L0 220 Q100 200 200 240 T400 190 T600 210 T800 180 T1000 150 L1000 250 Z"
                fill="url(#expense-grad)" />
              <!-- Lines -->
              <path d="M0 180 Q100 140 200 170 T400 100 T600 120 T800 60 T1000 40" stroke="#4ade80" stroke-width="4"
                fill="none" class="chart-line" />
              <path d="M0 220 Q100 200 200 240 T400 190 T600 210 T800 180 T1000 150" stroke="#f87171" stroke-width="4"
                fill="none" class="chart-line" />
              <!-- Points -->
              <circle cx="200" cy="170" r="4" fill="#05070a" stroke="#4ade80" stroke-width="3" />
              <circle cx="400" cy="100" r="4" fill="#05070a" stroke="#4ade80" stroke-width="3" />
              <circle cx="600" cy="120" r="4" fill="#05070a" stroke="#4ade80" stroke-width="3" />
              <circle cx="800" cy="60" r="4" fill="#05070a" stroke="#4ade80" stroke-width="3" />
              <circle cx="400" cy="190" r="4" fill="#05070a" stroke="#f87171" stroke-width="3" />
              <circle cx="600" cy="210" r="4" fill="#05070a" stroke="#f87171" stroke-width="3" />
              <circle cx="800" cy="180" r="4" fill="#05070a" stroke="#f87171" stroke-width="3" />
            </svg>
            <div class="x-axis-labels mt-2">
              <span>فروردین</span><span>اردیبهشت</span><span>خرداد</span><span>تیر</span><span>مرداد</span><span>شهریور</span><span>مهر</span><span>آبان</span><span>آذر</span><span>دی</span><span>بهمن</span><span>اسفند</span>
            </div>
            <div class="chart-legend-box mt-4">
              <span class="legend-item"><i class="dot bg-success"></i>درآمد عملیاتی</span>
              <span class="legend-item"><i class="dot bg-error"></i>هزینه‌ها</span>
            </div>
          </div>
        </div>

        <!-- Accounts & Balances -->
        <div class="glass-panel p-5 slide-up" style="--delay: 0.5s">
          <div class="panel-header mb-4">
            <h3 class="panel-title"><v-icon color="#a78bfa" class="mr-2">mdi-bank</v-icon> موجودی حساب‌ها</h3>
            <v-btn icon="mdi-dots-horizontal" variant="text" size="small" color="grey"></v-btn>
          </div>
          <div class="account-list">
            <div class="acc-item" v-for="acc in topAccounts" :key="acc.id">
              <div class="acc-icon"><v-icon :color="acc.color">{{ acc.icon }}</v-icon></div>
              <div class="acc-details">
                <span class="acc-name">{{ acc.name }}</span>
                <span class="acc-no">{{ acc.accountNo }}</span>
              </div>
              <div class="acc-balance mono-num">{{ acc.balance }}</div>
            </div>
          </div>
          <div class="mt-4 pt-4 border-t text-center">
            <span class="text-caption text-grey">جمع کل موجودی نقد و بانک:</span>
            <div class="total-balance text-violet mono-num font-weight-bold mt-1">۱۴,۵۸۰,۰۰۰,۰۰۰ <span
                class="text-caption">ریال</span></div>
          </div>
        </div>

        <!-- Recent Transactions -->
        <div class="glass-panel p-5 grid-span-2 slide-up" style="--delay: 0.6s">
          <div class="panel-header mb-4">
            <h3 class="panel-title"><v-icon color="#fb923c" class="mr-2">mdi-history</v-icon> آخرین اسناد و تراکنش‌ها
            </h3>
            <v-btn variant="text" color="info" size="small" class="font-weight-bold">مشاهده همه گردش‌ها</v-btn>
          </div>
          <table class="custom-table w-100">
            <thead>
              <tr>
                <th>شرح عملیات</th>
                <th>ارجاع</th>
                <th>تاریخ</th>
                <th class="text-left">مبلغ (ریال)</th>
                <th class="text-center">وضعیت</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="tx in recentActivities" :key="tx.id" class="table-row">
                <td class="font-weight-bold">
                  <v-icon :color="tx.isCredit ? 'success' : 'error'" size="18" class="mr-2">{{ tx.isCredit ?
                    'mdi-arrow-down-left' : 'mdi-arrow-up-right' }}</v-icon>
                  {{ tx.title }}
                </td>
                <td class="text-grey-lighten-1 mono-num">{{ tx.refNo }}</td>
                <td class="mono-num">{{ tx.date }}</td>
                <td class="text-left mono-num" :class="tx.isCredit ? 'text-success' : 'text-error'">
                  {{ tx.isCredit ? '+' : '-' }} {{ tx.amount }}
                </td>
                <td class="text-center">
                  <v-chip size="x-small" :color="tx.statusColor" variant="tonal" class="font-weight-bold">{{ tx.status
                    }}</v-chip>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Operations Summary (Small Widgets) -->
        <div class="metrics-column slide-up" style="--delay: 0.7s">
          <div class="glass-panel p-4 mb-4 mini-widget">
            <div class="widget-icon bg-info-subtle text-info"><v-icon>mdi-file-document-outline</v-icon></div>
            <div class="widget-content">
              <span class="widget-label">فاکتورهای فروش تسویه‌نشده</span>
              <span class="widget-value mono-num">۲۴ <span class="text-caption text-grey">مورد</span></span>
            </div>
            <v-progress-linear model-value="45" color="info" height="4" rounded class="mt-2 w-100"></v-progress-linear>
          </div>
          <div class="glass-panel p-4 mb-4 mini-widget">
            <div class="widget-icon bg-warning-subtle text-warning"><v-icon>mdi-ticket-confirmation</v-icon></div>
            <div class="widget-content">
              <span class="widget-label">چک‌های پرداختی سررسید نزدیک</span>
              <span class="widget-value mono-num">۷ <span class="text-caption text-grey">طی ۵ روز آینده</span></span>
            </div>
            <v-progress-linear model-value="80" color="warning" height="4" rounded
              class="mt-2 w-100"></v-progress-linear>
          </div>
          <div class="glass-panel p-4 mini-widget">
            <div class="widget-icon bg-error-subtle text-error"><v-icon>mdi-warehouse</v-icon></div>
            <div class="widget-content">
              <span class="widget-label">کالاهای نیازمند سفارش</span>
              <span class="widget-value mono-num">۱۲ <span class="text-caption text-grey">آیتم زیر نقطه
                  سفارش</span></span>
            </div>
            <v-progress-linear model-value="20" color="error" height="4" rounded class="mt-2 w-100"></v-progress-linear>
          </div>
        </div>

      </section>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const currentTime = ref('');
const currentDate = ref('');
let timer;

const mainCards = ref([
  { title: 'درآمد کل (ماه جاری)', value: '۸,۴۵۰,۰۰۰,۰۰۰', change: 12.5, positive: true, icon: 'mdi-cash-plus', iconColor: '#4ade80', iconBg: 'rgba(74,222,128,0.1)' },
  { title: 'هزینه‌ها (ماه جاری)', value: '۳,۱۲۰,۰۰۰,۰۰۰', change: -4.2, positive: true, icon: 'mdi-cash-minus', iconColor: '#f87171', iconBg: 'rgba(248,113,113,0.1)' },
  { title: 'مطالبات (حساب‌های دریافتنی)', value: '۱۲,۶۰۰,۰۰۰,۰۰۰', change: 8.4, positive: true, icon: 'mdi-account-cash', iconColor: '#38bdf8', iconBg: 'rgba(56,189,248,0.1)' },
  { title: 'بدهی‌ها (حساب‌های پرداختنی)', value: '۵,۸۰۰,۰۰۰,۰۰۰', change: 1.2, positive: false, icon: 'mdi-credit-card-outline', iconColor: '#fb923c', iconBg: 'rgba(251,146,60,0.1)' },
]);

const topAccounts = ref([
  { id: 1, name: 'بانک ملت - جاری مرکزی', accountNo: '۴۵۶۲۱۸۹۳۲۰', balance: '۵,۴۵۰,۰۰۰,۰۰۰', icon: 'mdi-bank', color: '#ef4444' }, // red for mellat
  { id: 2, name: 'بانک سامان - سپرده', accountNo: 'IR15056...', balance: '۸,۵۰۰,۰۰۰,۰۰۰', icon: 'mdi-bank', color: '#3b82f6' }, // blue for saman
  { id: 3, name: 'صندوق ریالی شرکت', accountNo: 'صندوق ۱', balance: '۶۳۰,۰۰۰,۰۰۰', icon: 'mdi-vault', color: '#10b981' }, // green for cash
]);

const recentActivities = ref([
  { id: 1, title: 'فاکتور فروش - پتروشیمی زاگرس', refNo: 'INV-403-112', date: '۱۴۰۳/۱۱/۲۸', amount: '۱,۴۵۰,۰۰۰,۰۰۰', isCredit: true, status: 'تسویه شده', statusColor: 'success' },
  { id: 2, title: 'پرداخت حقوق و دستمزد بهمن', refNo: 'PAY-403-55', date: '۱۴۰۳/۱۱/۲۸', amount: '۲,۸۵۰,۰۰۰,۰۰۰', isCredit: false, status: 'پرداخت موفق', statusColor: 'success' },
  { id: 3, title: 'خرید مواد اولیه - فولادین', refNo: 'PUR-403-88', date: '۱۴۰۳/۱۱/۲۷', amount: '۸۶۰,۰۰۰,۰۰۰', isCredit: false, status: 'در انتظار پرداخت', statusColor: 'warning' },
  { id: 4, title: 'وصول چک دریافتی (شماره ۴۵۲۱)', refNo: 'CHQ-4521', date: '۱۴۰۳/۱۱/۲۶', amount: '۳۲۰,۰۰۰,۰۰۰', isCredit: true, status: 'پاس شده', statusColor: 'success' },
  { id: 5, title: 'برگشت از فروش - قطعات صنعتی', refNo: 'RET-403-10', date: '۱۴۰۳/۱۱/۲۶', amount: '۴۵,۰۰۰,۰۰۰', isCredit: false, status: 'تایید انبار', statusColor: 'info' },
]);

function createNewDoc() {
  router.push('/accounting-document/create');
}

const updateTime = () => {
  const now = new Date();
  currentTime.value = now.toLocaleTimeString('fa-IR');
  currentDate.value = now.toLocaleDateString('fa-IR', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
};

onMounted(() => {
  updateTime();
  timer = setInterval(updateTime, 1000);
});

onUnmounted(() => {
  clearInterval(timer);
});
</script>

<style scoped>
/* ── Root & Layout ── */
.dashboard-root {
  position: relative;
  width: 100%;
  height: 100%;
  background: #06090f;
  /* Deep dark background */
  overflow: hidden;
  font-family: 'Vazirmatn', sans-serif;
  direction: rtl;
}

.dashboard-scroll {
  position: relative;
  z-index: 2;
  height: 100%;
  overflow-y: auto;
  overflow-x: hidden;
  padding: 32px 40px;
  scrollbar-width: thin;
  scrollbar-color: rgba(255, 255, 255, 0.08) transparent;
}

/* ── Ambient Effects ── */
.ambient-orb {
  position: absolute;
  border-radius: 50%;
  filter: blur(100px);
  pointer-events: none;
  z-index: 0;
  opacity: 0.6;
}

.orb-1 {
  width: 600px;
  height: 600px;
  background: radial-gradient(circle, rgba(56, 189, 248, 0.08), transparent 70%);
  top: -200px;
  left: -150px;
}

.orb-2 {
  width: 500px;
  height: 500px;
  background: radial-gradient(circle, rgba(167, 139, 250, 0.08), transparent 70%);
  bottom: -100px;
  right: -100px;
}

.grid-overlay {
  position: absolute;
  inset: 0;
  z-index: 1;
  background-image: linear-gradient(rgba(255, 255, 255, 0.015) 1px, transparent 1px), linear-gradient(90deg, rgba(255, 255, 255, 0.015) 1px, transparent 1px);
  background-size: 40px 40px;
  pointer-events: none;
}

/* ── Generic Glass Panel ── */
.glass-panel {
  background: rgba(13, 17, 23, 0.7);
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 20px;
  backdrop-filter: blur(20px);
  -webkit-backdrop-filter: blur(20px);
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.2);
}

.hover-lift {
  transition: transform 0.3s ease, box-shadow 0.3s ease, border-color 0.3s ease;
}

.hover-lift:hover {
  transform: translateY(-5px);
  box-shadow: 0 16px 48px rgba(0, 0, 0, 0.3);
  border-color: rgba(255, 255, 255, 0.1);
}

.slide-up {
  animation: fadeSlideUp 0.6s cubic-bezier(0.16, 1, 0.3, 1) both;
  animation-delay: var(--delay, 0s);
}

@keyframes fadeSlideUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }

  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* ── Header ── */
.dash-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 32px;
  animation: fadeSlideUp 0.4s ease both;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 16px;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 16px;
}

.page-icon {
  width: 48px;
  height: 48px;
  border-radius: 14px;
  background: rgba(56, 189, 248, 0.1);
  border: 1px solid rgba(56, 189, 248, 0.2);
  display: flex;
  align-items: center;
  justify-content: center;
  color: #38bdf8;
  flex-shrink: 0;
  box-shadow: 0 0 20px rgba(56, 189, 248, 0.15);
}

.page-title {
  font-size: 22px;
  font-weight: 800;
  color: #ffffff;
  margin: 0;
  letter-spacing: -0.5px;
}

.page-subtitle {
  font-size: 13px;
  color: #94a3b8;
  margin: 0;
  margin-top: 4px;
}

.date-badge {
  display: flex;
  align-items: center;
  gap: 8px;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 99px;
  padding: 8px 16px;
  font-size: 13px;
  color: #cbd5e1;
  font-family: inherit;
}

.date-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: #4ade80;
  box-shadow: 0 0 8px #4ade80;
  animation: blink 2s infinite;
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

.btn-new-tx {
  display: flex;
  align-items: center;
  gap: 8px;
  background: linear-gradient(135deg, #0ea5e9, #3b82f6);
  color: #fff;
  border: none;
  border-radius: 12px;
  padding: 10px 20px;
  font-size: 14px;
  font-weight: 700;
  cursor: pointer;
  transition: transform 0.2s, box-shadow 0.2s;
  box-shadow: 0 6px 20px rgba(14, 165, 233, 0.3);
}

.btn-new-tx:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(14, 165, 233, 0.4);
}

/* ── Stats Grid Row 1 ── */
.stats-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 24px;
}

.stat-card {
  padding: 24px;
  display: flex;
  flex-direction: column;
}

.stat-card-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 20px;
}

.stat-icon {
  width: 48px;
  height: 48px;
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.stat-badge {
  display: flex;
  align-items: center;
  font-size: 13px;
  font-weight: 800;
  border-radius: 20px;
  padding: 4px 10px;
  font-family: inherit;
  direction: ltr;
}

.badge-up {
  background: rgba(74, 222, 128, 0.15);
  color: #4ade80;
}

.badge-down {
  background: rgba(248, 113, 113, 0.15);
  color: #f87171;
}

.stat-label {
  font-size: 13px;
  color: #94a3b8;
  font-weight: 600;
  margin-bottom: 8px;
}

.stat-value {
  font-size: 26px;
  font-weight: 900;
  color: #ffffff;
  letter-spacing: 0.5px;
}

.currency {
  font-size: 12px;
  font-weight: 400;
  color: #64748b;
  margin-right: 4px;
}

.mono-num {
  font-family: inherit;
}

/* ── Main Content Grid ── */
.main-content-grid {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 24px;
}

.grid-span-2 {
  grid-column: span 1;
}

/* adjust based on space later, let's keep it responsive */
@media(min-width: 1200px) {
  .main-content-grid {
    grid-template-columns: 2fr 1.2fr;
  }
}

@media(max-width: 1024px) {
  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
  }

  .main-content-grid {
    grid-template-columns: 1fr;
  }
}

/* ── Panel Shared ── */
.panel-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.panel-title {
  font-size: 16px;
  font-weight: 800;
  color: #f8fafc;
  margin: 0;
  display: flex;
  align-items: center;
}

.border-t {
  border-top: 1px solid rgba(255, 255, 255, 0.05);
}

/* ── Chart Section ── */
.toggle-group {
  display: flex;
  background: rgba(255, 255, 255, 0.05);
  border-radius: 10px;
  padding: 4px;
}

.toggle-btn {
  background: transparent;
  border: none;
  color: #94a3b8;
  padding: 6px 16px;
  border-radius: 8px;
  cursor: pointer;
  font-family: inherit;
  font-size: 12px;
  font-weight: 700;
  transition: all 0.2s;
}

.toggle-btn.active {
  background: rgba(255, 255, 255, 0.1);
  color: #fff;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
}

.chart-container {
  position: relative;
  width: 100%;
  height: 320px;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
}

.chart-line {
  stroke-linejoin: round;
  stroke-linecap: round;
  filter: drop-shadow(0 8px 8px rgba(0, 0, 0, 0.3));
  animation: drawLine 2s ease-out forwards;
}

@keyframes drawLine {
  from {
    stroke-dasharray: 2000;
    stroke-dashoffset: 2000;
  }

  to {
    stroke-dasharray: 2000;
    stroke-dashoffset: 0;
  }
}

.x-axis-labels {
  display: flex;
  justify-content: space-between;
  padding: 0 20px;
  color: #64748b;
  font-size: 12px;
}

.chart-legend-box {
  display: flex;
  justify-content: center;
  gap: 20px;
}

.dot {
  display: inline-block;
  width: 10px;
  height: 10px;
  border-radius: 50%;
  margin-left: 8px;
}

.bg-success {
  background: #4ade80;
  box-shadow: 0 0 10px #4ade80;
}

.bg-error {
  background: #f87171;
  box-shadow: 0 0 10px #f87171;
}

.legend-item {
  font-size: 13px;
  color: #cbd5e1;
  display: flex;
  align-items: center;
}

/* ── Accounts List ── */
.account-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.acc-item {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 16px;
  background: rgba(255, 255, 255, 0.02);
  border-radius: 14px;
  border: 1px solid rgba(255, 255, 255, 0.03);
  transition: background 0.2s;
  cursor: pointer;
}

.acc-item:hover {
  background: rgba(255, 255, 255, 0.05);
}

.acc-icon {
  width: 44px;
  height: 44px;
  background: rgba(255, 255, 255, 0.05);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.acc-details {
  display: flex;
  flex-direction: column;
  flex: 1;
}

.acc-name {
  font-size: 14px;
  font-weight: 700;
  color: #e2e8f0;
}

.acc-no {
  font-size: 12px;
  color: #64748b;
  margin-top: 2px;
}

.acc-balance {
  font-size: 16px;
  font-weight: 800;
  color: #f8fafc;
}

/* ── Custom Table ── */
.custom-table {
  border-collapse: collapse;
  text-align: right;
}

.custom-table th {
  padding: 12px 16px;
  border-bottom: 2px solid rgba(255, 255, 255, 0.05);
  color: #94a3b8;
  font-size: 12px;
  font-weight: 600;
}

.custom-table td {
  padding: 14px 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.02);
  font-size: 13px;
  vertical-align: middle;
  color: #e2e8f0;
}

.table-row:hover td {
  background: rgba(255, 255, 255, 0.02);
}

/* ── Mini Widgets ── */
.metrics-column {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.mini-widget {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 16px;
}

.widget-icon {
  width: 50px;
  height: 50px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
}

.widget-content {
  display: flex;
  flex-direction: column;
  flex: 1;
}

.widget-label {
  font-size: 13px;
  color: #cbd5e1;
  font-weight: 600;
  margin-bottom: 4px;
}

.widget-value {
  font-size: 18px;
  font-weight: 800;
  color: #fff;
}

.bg-info-subtle {
  background: rgba(56, 189, 248, 0.15);
}

.text-info {
  color: #38bdf8 !important;
}

.bg-warning-subtle {
  background: rgba(251, 146, 60, 0.15);
}

.text-warning {
  color: #fb923c !important;
}

.bg-error-subtle {
  background: rgba(248, 113, 113, 0.15);
}

.text-error {
  color: #f87171 !important;
}

/* Utils */
.text-violet {
  color: #a78bfa !important;
}

.text-grey-lighten-1 {
  color: #94a3b8 !important;
}

.text-success {
  color: #4ade80 !important;
}
</style>
