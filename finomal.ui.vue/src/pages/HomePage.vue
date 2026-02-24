<template>
  <v-app theme="dark" dir="rtl" class="app-root">
    <AppSidebar @menu-selected="setActivePage" />

    <v-main class="main-content">
      <v-container fluid class="pa-8">

        <v-row class="mb-6 align-center">
          <v-col>
            <div class="d-flex align-center">
              <v-icon color="primary" size="32" class="me-3">mdi-view-dashboard-outline</v-icon>
              <div>
                <h1 class="text-h5 font-weight-bold slate-100">{{ activePage }}</h1>
                <div class="text-caption text-slate-400">خوش آمدید، گزارش‌های امروز آماده است.</div>
              </div>
            </div>
          </v-col>
          <v-spacer />
          <v-btn prepend-icon="mdi-plus" color="primary" rounded="lg" elevation="0">تراکنش جدید</v-btn>
        </v-row>

        <v-row>
          <v-col v-for="card in dashboardCards" :key="card.title" cols="12" sm="6" lg="3">
            <v-card class="glass-card stat-card" border="thin">
              <v-card-text class="pa-5">
                <div class="d-flex justify-space-between align-start">
                  <v-avatar :color="card.iconBg" rounded="lg" size="48">
                    <v-icon :color="card.iconColor" size="26">{{ card.icon }}</v-icon>
                  </v-avatar>
                  <v-chip :color="card.positive ? 'success' : 'error'" size="x-small" variant="tonal" class="font-weight-bold">
                    {{ card.change }}
                    <v-icon end size="12">{{ card.positive ? 'mdi-arrow-up' : 'mdi-arrow-down' }}</v-icon>
                  </v-chip>
                </div>
                <div class="mt-4">
                  <div class="text-h5 font-weight-black text-slate-100">{{ card.value }}</div>
                  <div class="text-caption text-slate-400 font-weight-medium">{{ card.title }}</div>
                </div>
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>

        <v-row class="mt-4">
          <v-col cols="12" lg="8">
            <v-card class="glass-card h-100" border="thin">
              <v-card-title class="pa-5 d-flex align-center">
                <span class="text-subtitle-1 font-weight-bold">تحلیل جریان نقدینگی</span>
                <v-spacer />
                <v-btn-toggle v-model="chartPeriod" density="compact" mandatory variant="outlined" color="primary">
                  <v-btn value="week" size="x-small">هفته</v-btn>
                  <v-btn value="month" size="x-small">ماه</v-btn>
                </v-btn-toggle>
              </v-card-title>
              <v-card-text class="pa-5">
                <div class="chart-mockup d-flex align-end justify-space-around px-4">
                  <div v-for="i in 7" :key="i" class="chart-bar-wrap">
                    <div class="bar-income" :style="{ height: (Math.random() * 80 + 20) + '%' }"></div>
                    <div class="bar-expense" :style="{ height: (Math.random() * 50 + 10) + '%' }"></div>
                  </div>
                </div>
                <div class="d-flex justify-center mt-4 gap-4">
                  <div class="d-flex align-center text-caption"><v-icon color="success" size="12" class="me-1">mdi-circle</v-icon> درآمد</div>
                  <div class="d-flex align-center text-caption"><v-icon color="error" size="12" class="me-1">mdi-circle</v-icon> هزینه</div>
                </div>
              </v-card-text>
            </v-card>
          </v-col>

          <v-col cols="12" lg="4">
            <v-card class="glass-card" border="thin">
              <v-card-title class="pa-5 text-subtitle-1 font-weight-bold">تراکنش‌های اخیر</v-card-title>
              <v-list bg-color="transparent" class="px-2 pb-4">
                <v-list-item v-for="tx in recentTransactions" :key="tx.id" class="tx-item rounded-lg mb-2">
                  <template #prepend>
                    <v-avatar :color="tx.type === 'in' ? 'rgba(74,222,128,0.1)' : 'rgba(248,113,113,0.1)'" rounded="lg" size="40">
                      <v-icon :color="tx.type === 'in' ? 'success' : 'error'" size="18">
                        {{ tx.type === 'in' ? 'mdi-plus' : 'mdi-minus' }}
                      </v-icon>
                    </v-avatar>
                  </template>
                  <v-list-item-title class="text-body-2 font-weight-bold">{{ tx.title }}</v-list-item-title>
                  <v-list-item-subtitle class="text-caption">{{ tx.date }}</v-list-item-subtitle>
                  <template #append>
                    <span :class="['text-body-2 font-weight-black', tx.type === 'in' ? 'text-success' : 'text-error']">
                      {{ tx.amount }}
                    </span>
                  </template>
                </v-list-item>
              </v-list>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-main>

    <!-- ===== Status Bar (Bottom) ===== -->
    <v-footer class="status-bar px-4" height="36" app>
      <div class="d-flex align-center w-100 status-bar-inner">
        <!-- سمت راست -->
        <div class="status-item">
          <v-icon size="13" color="#4ade80" class="me-1">mdi-circle</v-icon>
          <span>آنلاین</span>
        </div>
        <div class="status-divider" />
        <div class="status-item">
          <v-icon size="13" class="me-1">mdi-database-outline</v-icon>
          <span>دوره مالی: ۱۴۰۳</span>
        </div>
        <div class="status-divider" />
        <div class="status-item">
          <v-icon size="13" class="me-1">mdi-calendar-outline</v-icon>
          <span>{{ currentDate }}</span>
        </div>
        <div class="status-divider" />
        <div class="status-item">
          <v-icon size="13" class="me-1">mdi-clock-outline</v-icon>
          <span>{{ currentTime }}</span>
        </div>

        <v-spacer />

        <!-- سمت چپ -->
        <div class="status-item">
          <v-icon size="13" class="me-1">mdi-information-outline</v-icon>
          <span>فینومال v1.0.0</span>
        </div>
        <div class="status-divider" />
        <div class="status-item">
          <v-icon size="13" class="me-1">mdi-account-outline</v-icon>
          <span>کاربر: مدیر مالی</span>
        </div>
      </div>
    </v-footer>

  </v-app>
</template>

<script>
  import AppSidebar from "../components/AppSidebar.vue";

  export default {
    components: { AppSidebar },
    data: () => ({
      activePage: "داشبورد مدیریت",
      currentTime: "",
      chartPeriod: "week",
      dashboardCards: [
        { title: "درآمد خالص ماه", value: "۱۲۵,۰۰۰,۰۰۰", change: "۱۲٪", positive: true, icon: "mdi-currency-usd", iconColor: "#4ade80", iconBg: "rgba(74,222,128,0.1)" },
        { title: "هزینه‌های جاری", value: "۴۸,۳۰۰,۰۰۰", change: "۵٪", positive: false, icon: "mdi-cart-outline", iconColor: "#f87171", iconBg: "rgba(248,113,113,0.1)" },
        { title: "فاکتورهای باز", value: "۲۳ مورد", change: "۸٪", positive: true, icon: "mdi-file-document-outline", iconColor: "#38bdf8", iconBg: "rgba(56,189,248,0.1)" },
        { title: "موجودی کل", value: "۶۸۰,۷۵۰,۰۰۰", change: "۳٪", positive: true, icon: "mdi-bank-outline", iconColor: "#a78bfa", iconBg: "rgba(167,139,250,0.1)" },
      ],
      recentTransactions: [
        { id: 1, title: "فروش محصول A", date: "۱۴۰۳/۱۰/۱۵", amount: "۱۲.۵M", type: "in" },
        { id: 2, title: "اجاره دفتر", date: "۱۴۰۳/۱۰/۱۴", amount: "۴.۲M", type: "out" },
        { id: 3, title: "خدمات مشاوره", date: "۱۴۰۳/۱۰/۱۳", amount: "۸.۰M", type: "in" },
      ]
    }),
    mounted() {
      this.updateTime();
      setInterval(this.updateTime, 1000);
    },
    methods: {
      updateTime() {
        this.currentTime = new Date().toLocaleTimeString("fa-IR");
      }
    }
  };
</script>

<style scoped>
  .app-root {
    background: #05070a !important;
    font-family: 'Vazirmatn', sans-serif;
  }

  .main-content {
    background: radial-gradient(circle at top right, #0a101f, #05070a) !important;
  }

  /* Glassmorphism Effect */
  .glass-card {
    background: rgba(13, 17, 23, 0.8) !important;
    backdrop-filter: blur(12px);
    border-color: rgba(255, 255, 255, 0.05) !important;
    border-radius: 16px !important;
    transition: all 0.3s ease;
  }

    .glass-card:hover {
      border-color: rgba(56, 189, 248, 0.3) !important;
      transform: translateY(-5px);
      box-shadow: 0 10px 30px rgba(0,0,0,0.5);
    }

  /* Chart Mockup Style */
  .chart-mockup {
    height: 250px;
    background: rgba(255,255,255,0.02);
    border-radius: 12px;
    padding: 20px 0;
  }

  .chart-bar-wrap {
    width: 30px;
    height: 100%;
    display: flex;
    align-items: flex-end;
    gap: 4px;
  }

  .bar-income {
    width: 12px;
    background: #4ade80;
    border-radius: 4px 4px 0 0;
  }

  .bar-expense {
    width: 12px;
    background: #f87171;
    border-radius: 4px 4px 0 0;
    opacity: 0.7;
  }

  .tx-item:hover {
    background: rgba(255,255,255,0.03) !important;
  }

  .status-bar {
    background: #080c14 !important;
    border-top: 1px solid rgba(255,255,255,0.05) !important;
  }

  .text-slate-100 {
    color: #f1f5f9;
  }

  .text-slate-400 {
    color: #94a3b8;
  }

  /* ===== Status Bar ===== */
  :deep(.status-bar) {
    background: #0a0e18 !important;
    border-top: 1px solid rgba(255, 255, 255, 0.07) !important;
    direction: rtl;
  }

  .status-bar-inner {
    direction: rtl;
    gap: 0;
  }

  .status-item {
    display: flex;
    align-items: center;
    font-size: 11px;
    color: #475569;
    gap: 2px;
    white-space: nowrap;
    direction: rtl;
  }

  .status-divider {
    width: 1px;
    height: 14px;
    background: rgba(255, 255, 255, 0.08);
    margin: 0 10px;
  }
</style>
