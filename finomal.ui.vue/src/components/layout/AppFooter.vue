<template>
  <footer class="app-footer" dir="rtl">
    <div class="footer-inner">

      <!-- Right Side: System Status -->
      <div class="footer-group">
        <div class="status-indicator">
          <span class="pulse-dot"></span>
          <span class="status-text">سیستم آنلاین</span>
        </div>

        <div class="footer-divider"></div>

        <div class="footer-item">
          <v-icon size="14" class="footer-icon">mdi-database-outline</v-icon>
          <span class="footer-label">دوره مالی:</span>
          <span class="footer-value">{{ toPersian('1403') }}</span>
        </div>

        <div class="footer-divider"></div>

        <div class="footer-item">
          <v-icon size="14" class="footer-icon">mdi-office-building-outline</v-icon>
          <span class="footer-label">شعبه:</span>
          <span class="footer-value">مرکزی</span>
        </div>
      </div>

      <!-- Center: App Branding/Quick Stats (Optional) -->
      <div class="footer-center">
        <div class="branding-pill">
          <span class="brand-text">FINOMAL</span>
          <span class="version-tag">v1.0.0</span>
        </div>
      </div>

      <!-- Left Side: Time & Date -->
      <div class="footer-group footer-left">
        <div class="footer-item">
          <v-icon size="14" class="footer-icon">mdi-calendar-range</v-icon>
          <span class="footer-value">{{ persianDate }}</span>
        </div>

        <div class="footer-divider"></div>

        <div class="footer-item clock-item">
          <v-icon size="14" class="footer-icon">mdi-clock-outline</v-icon>
          <span class="footer-value clock-font">{{ currentTime }}</span>
        </div>

        <div class="footer-divider"></div>

        <div class="footer-item user-item">
          <v-icon size="14" class="footer-icon">mdi-account-outline</v-icon>
          <span class="footer-value">{{ userName }}</span>
        </div>
      </div>

    </div>
  </footer>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';

const props = defineProps({
  userName: { type: String, default: 'مدیر سیستم' }
});

const currentTime = ref('');
const persianDate = ref('');

const PD = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
function toPersian(n) {
  return String(n).split('').map(c => PD[+c] ?? c).join('');
}

const updateDateTime = () => {
  const now = new Date();

  // Update time
  currentTime.value = now.toLocaleTimeString('fa-IR', {
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit'
  });

  // Update date (simplified Persian date display)
  persianDate.value = now.toLocaleDateString('fa-IR', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
};

let timer;
onMounted(() => {
  updateDateTime();
  timer = setInterval(updateDateTime, 1000);
});

onUnmounted(() => {
  if (timer) clearInterval(timer);
});
</script>

<style scoped>
.app-footer {
  width: 100%;
  height: 38px;
  background: #080c14;
  border-top: 1px solid rgba(56, 189, 248, 0.1);
  box-shadow: 0 -4px 20px rgba(0, 0, 0, 0.4);
  z-index: 1000;
  font-family: 'Vazirmatn', sans-serif;
  padding: 0 16px;
  display: flex;
  align-items: center;
}

.footer-inner {
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.footer-group {
  display: flex;
  align-items: center;
  gap: 12px;
}

.footer-left {
  flex-direction: row-reverse;
}

.footer-item {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 11px;
  color: #64748b;
  transition: color 0.2s;
}

.footer-item:hover {
  color: #94a3b8;
}

.footer-icon {
  color: #334155;
  transition: color 0.2s;
}

.footer-item:hover .footer-icon {
  color: #38bdf8;
}

.footer-label {
  color: #3d5166;
  font-weight: 500;
}

.footer-value {
  color: #94a3b8;
  font-weight: 600;
}

.clock-font {
  font-family: monospace;
  letter-spacing: 0.5px;
  min-width: 65px;
  text-align: center;
}

.footer-divider {
  width: 1px;
  height: 14px;
  background: rgba(255, 255, 255, 0.05);
}

/* Status Indicator */
.status-indicator {
  display: flex;
  align-items: center;
  gap: 8px;
}

.pulse-dot {
  width: 7px;
  height: 7px;
  background-color: #22c55e;
  border-radius: 50%;
  position: relative;
  box-shadow: 0 0 8px rgba(34, 197, 94, 0.4);
}

.pulse-dot::after {
  content: '';
  position: absolute;
  inset: -2px;
  border-radius: 50%;
  border: 1px solid #22c55e;
  animation: pulse-ring 2s infinite;
}

@keyframes pulse-ring {
  0% {
    transform: scale(0.7);
    opacity: 0.8;
  }

  100% {
    transform: scale(1.8);
    opacity: 0;
  }
}

.status-text {
  font-size: 10px;
  font-weight: 700;
  color: #22c55e;
  letter-spacing: 0.2px;
}

/* Branding Pill */
.footer-center {
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
}

.branding-pill {
  background: rgba(56, 189, 248, 0.05);
  border: 1px solid rgba(56, 189, 248, 0.1);
  padding: 2px 10px;
  border-radius: 99px;
  display: flex;
  align-items: center;
  gap: 8px;
}

.brand-text {
  font-size: 9px;
  font-weight: 900;
  letter-spacing: 1.5px;
  color: #38bdf8;
  opacity: 0.8;
}

.version-tag {
  font-size: 8px;
  font-weight: 700;
  color: #1e3a4a;
  background: rgba(56, 189, 248, 0.1);
  padding: 0 5px;
  border-radius: 4px;
}

.user-item {
  color: #38bdf8;
  background: rgba(56, 189, 248, 0.05);
  padding: 2px 8px;
  border-radius: 6px;
  border: 1px solid rgba(56, 189, 248, 0.1);
}

.user-item .footer-icon {
  color: #38bdf8;
}

.user-item .footer-value {
  color: #38bdf8;
}
</style>
