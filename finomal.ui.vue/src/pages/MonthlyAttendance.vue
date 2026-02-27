<template>
    <div class="page-root" dir="rtl">

        <!-- BG -->
        <div class="bg-glow bg-glow-1" />
        <div class="bg-glow bg-glow-2" />
        <div class="bg-grid" />

        <div class="page-inner">

            <!-- ══ HEADER ══ -->
            <header class="page-header">
                <div class="brand">
                    <div class="brand-mark">
                        <v-icon size="20" color="#f472b6">mdi-calendar-clock</v-icon>
                    </div>
                    <div>
                        <div class="brand-name">کارکرد ماهیانه</div>
                        <div class="brand-sub">ثبت و مدیریت حضور و غیاب پرسنل</div>
                    </div>
                </div>
                <div class="header-actions">
                    <button class="btn btn-ghost" @click="exportExcel">
                        <v-icon size="15">mdi-microsoft-excel</v-icon>
                        خروجی Excel
                    </button>
                    <button class="btn btn-primary" @click="saveAll">
                        <v-icon size="15">mdi-content-save</v-icon>
                        ذخیره همه
                    </button>
                </div>
            </header>

            <!-- ══ MONTH SELECTOR ══ -->
            <div class="month-bar">
                <button class="month-nav-btn" @click="prevMonth">
                    <v-icon size="18">mdi-chevron-right</v-icon>
                </button>
                <div class="month-display">
                    <v-icon size="16" color="#f472b6" class="ml-2">mdi-calendar-month</v-icon>
                    <span class="month-label">{{ currentMonthLabel }}</span>
                </div>
                <button class="month-nav-btn" @click="nextMonth" :disabled="isCurrentMonth">
                    <v-icon size="18">mdi-chevron-left</v-icon>
                </button>

                <div class="month-sep" />

                <!-- Period toggle -->
                <div class="period-tabs">
                    <button v-for="p in periods" :key="p.val" class="period-tab"
                        :class="{ active: activePeriod === p.val }" @click="activePeriod = p.val">{{ p.label }}</button>
                </div>

                <div class="month-sep" />

                <!-- Search -->
                <div class="search-mini">
                    <v-icon size="14" class="search-ic">mdi-magnify</v-icon>
                    <input v-model="search" placeholder="جستجو پرسنل..." class="search-mini-input" />
                </div>
            </div>

            <!-- ══ STATS ══ -->
            <div class="stats-row">
                <div class="stat-card" v-for="s in summaryStats" :key="s.label" :style="{ '--c': s.color }">
                    <div class="stat-icon-wrap">
                        <v-icon size="18" :color="s.color">{{ s.icon }}</v-icon>
                    </div>
                    <div>
                        <div class="stat-val">{{ s.val }}</div>
                        <div class="stat-label">{{ s.label }}</div>
                    </div>
                </div>
            </div>

            <!-- ══ TABLE ══ -->
            <div class="table-card">
                <div class="table-scroll">
                    <table class="att-table">
                        <thead>
                            <tr>
                                <th class="th-sticky th-name">نام پرسنل</th>
                                <th class="th-sticky th-code">کد</th>
                                <th v-for="d in workingDays" :key="d.num" class="th-day"
                                    :class="{ 'th-weekend': d.isWeekend }">
                                    <div class="day-num">{{ toPersian(d.num) }}</div>
                                    <div class="day-name">{{ d.name }}</div>
                                </th>
                                <th class="th-total">کارکرد</th>
                                <th class="th-total">غیبت</th>
                                <th class="th-total">مرخصی</th>
                                <th class="th-total">اضافه‌کار</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="p in filteredPersonnel" :key="p.id" class="att-row">
                                <td class="td-sticky td-name">
                                    <div class="person-cell">
                                        <div class="avatar" :style="{ background: avatarColor(p.name) }">{{
                                            p.name.charAt(0) }}</div>
                                        <div>
                                            <div class="person-name">{{ p.name }}</div>
                                            <div class="person-dept">{{ p.dept }}</div>
                                        </div>
                                    </div>
                                </td>
                                <td class="td-sticky td-code">
                                    <span class="code-badge">{{ p.code }}</span>
                                </td>
                                <td v-for="d in workingDays" :key="d.num" class="td-day"
                                    :class="{ 'td-weekend': d.isWeekend }">
                                    <div v-if="d.isWeekend" class="day-off-mark">—</div>
                                    <button v-else class="day-cell" :class="getDayClass(p.id, d.num)"
                                        @click="cycleStatus(p.id, d.num)">
                                        <v-icon size="12">{{ getDayIcon(p.id, d.num) }}</v-icon>
                                    </button>
                                </td>
                                <td class="td-total td-work">{{ toPersian(getWorkDays(p.id)) }}</td>
                                <td class="td-total td-absent">{{ toPersian(getAbsentDays(p.id)) }}</td>
                                <td class="td-total td-leave">{{ toPersian(getLeaveDays(p.id)) }}</td>
                                <td class="td-total td-overtime">{{ toPersian(getOvertimeDays(p.id)) }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!-- Legend -->
                <div class="legend-bar">
                    <span class="legend-item" v-for="l in legend" :key="l.label">
                        <span class="legend-dot" :class="l.cls"></span>
                        {{ l.label }}
                    </span>
                    <span class="legend-note">کلیک روی روز برای تغییر وضعیت</span>
                </div>
            </div>

            <!-- ══ DETAIL PANEL ══ -->
            <div class="detail-grid">

                <!-- غیبت‌های ثبت‌شده -->
                <div class="detail-card">
                    <div class="detail-card-title">
                        <v-icon size="15" color="#f87171">mdi-account-off</v-icon>
                        غیبت‌های این ماه
                    </div>
                    <div class="detail-list">
                        <div v-for="item in absentSummary" :key="item.name" class="detail-row">
                            <div class="detail-avatar" :style="{ background: avatarColor(item.name) }">{{
                                item.name.charAt(0) }}</div>
                            <div class="detail-info">
                                <div class="detail-name">{{ item.name }}</div>
                                <div class="detail-sub">{{ toPersian(item.days) }} روز غیبت</div>
                            </div>
                            <div class="detail-badge badge-red">{{ toPersian(item.days) }}</div>
                        </div>
                        <div v-if="absentSummary.length === 0" class="detail-empty">
                            <v-icon size="28" color="#334155">mdi-check-circle</v-icon>
                            <p>غیبتی ثبت نشده</p>
                        </div>
                    </div>
                </div>

                <!-- مرخصی‌ها -->
                <div class="detail-card">
                    <div class="detail-card-title">
                        <v-icon size="15" color="#fbbf24">mdi-beach</v-icon>
                        مرخصی‌های این ماه
                    </div>
                    <div class="detail-list">
                        <div v-for="item in leaveSummary" :key="item.name" class="detail-row">
                            <div class="detail-avatar" :style="{ background: avatarColor(item.name) }">{{
                                item.name.charAt(0) }}</div>
                            <div class="detail-info">
                                <div class="detail-name">{{ item.name }}</div>
                                <div class="detail-sub">{{ toPersian(item.days) }} روز مرخصی</div>
                            </div>
                            <div class="detail-badge badge-yellow">{{ toPersian(item.days) }}</div>
                        </div>
                        <div v-if="leaveSummary.length === 0" class="detail-empty">
                            <v-icon size="28" color="#334155">mdi-check-circle</v-icon>
                            <p>مرخصی ثبت نشده</p>
                        </div>
                    </div>
                </div>

                <!-- اضافه‌کار -->
                <div class="detail-card">
                    <div class="detail-card-title">
                        <v-icon size="15" color="#a78bfa">mdi-clock-plus</v-icon>
                        اضافه‌کار این ماه
                    </div>
                    <div class="detail-list">
                        <div v-for="item in overtimeSummary" :key="item.name" class="detail-row">
                            <div class="detail-avatar" :style="{ background: avatarColor(item.name) }">{{
                                item.name.charAt(0) }}</div>
                            <div class="detail-info">
                                <div class="detail-name">{{ item.name }}</div>
                                <div class="detail-sub">{{ toPersian(item.days) }} روز اضافه‌کار</div>
                            </div>
                            <div class="detail-badge badge-purple">{{ toPersian(item.days) }}</div>
                        </div>
                        <div v-if="overtimeSummary.length === 0" class="detail-empty">
                            <v-icon size="28" color="#334155">mdi-moon-waning-crescent</v-icon>
                            <p>اضافه‌کاری ثبت نشده</p>
                        </div>
                    </div>
                </div>

            </div>

        </div>

        <!-- Toast -->
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
import { ref, computed } from 'vue'

// ── Helpers ──────────────────────────────────────────────────────────────
const PD = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹']
function toPersian(n) { return String(n).split('').map(c => PD[+c] ?? c).join('') }

const COLORS = [
    'linear-gradient(135deg,#63b3ed,#4299e1)',
    'linear-gradient(135deg,#48bb78,#38a169)',
    'linear-gradient(135deg,#ed8936,#dd6b20)',
    'linear-gradient(135deg,#a78bfa,#7c3aed)',
    'linear-gradient(135deg,#f687b3,#d53f8c)',
    'linear-gradient(135deg,#38bdf8,#0284c7)',
    'linear-gradient(135deg,#fbbf24,#d97706)',
]
function avatarColor(name) { return COLORS[name.charCodeAt(0) % COLORS.length] }

// ── Persian month config ─────────────────────────────────────────────────
const MONTHS = ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند']
const DAY_NAMES_SHORT = ['ش', 'ی', 'د', 'س', 'چ', 'پ', 'ج']  // شنبه=0 ... جمعه=6

const currentYear = ref(1403)
const currentMonth = ref(11) // 1-based (اسفند = 12)

function daysInMonth(m) { return m <= 6 ? 31 : m <= 11 ? 30 : 29 }

const workingDays = computed(() => {
    const total = daysInMonth(currentMonth.value)
    // month 1 day 1 offset — شنبه=0, جمعه=6
    // simplified: start offset for 1403/01/01 = 5 (پنجشنبه)
    const startOffset = ((currentMonth.value - 1) * 2 + 5) % 7
    return Array.from({ length: total }, (_, i) => {
        const dayOfWeek = (startOffset + i) % 7  // 0=شنبه…6=جمعه
        return {
            num: i + 1,
            name: DAY_NAMES_SHORT[dayOfWeek],
            isWeekend: dayOfWeek === 6, // جمعه
        }
    })
})

const currentMonthLabel = computed(() =>
    `${MONTHS[currentMonth.value - 1]} ${toPersian(currentYear.value)}`
)

const isCurrentMonth = computed(() => false) // demo: allow forward navigation

function prevMonth() {
    if (currentMonth.value === 1) { currentMonth.value = 12; currentYear.value-- }
    else currentMonth.value--
    resetAttendance()
}
function nextMonth() {
    if (currentMonth.value === 12) { currentMonth.value = 1; currentYear.value++ }
    else currentMonth.value++
    resetAttendance()
}

// ── Personnel ─────────────────────────────────────────────────────────────
const personnel = ref([
    { id: 1, name: 'علی محمدی', code: 'P-0001', dept: 'مالی و حسابداری' },
    { id: 2, name: 'فاطمه رضایی', code: 'P-0002', dept: 'اداری و منابع انسانی' },
    { id: 3, name: 'محمد کریمی', code: 'P-0003', dept: 'فناوری اطلاعات' },
    { id: 4, name: 'زهرا احمدی', code: 'P-0004', dept: 'فروش و بازاریابی' },
    { id: 5, name: 'حسن موسوی', code: 'P-0005', dept: 'تولید و عملیات' },
    { id: 6, name: 'مریم صادقی', code: 'P-0006', dept: 'مالی و حسابداری' },
    { id: 7, name: 'رضا جعفری', code: 'P-0007', dept: 'انبار و لجستیک' },
    { id: 8, name: 'نرگس حسینی', code: 'P-0008', dept: 'اداری و منابع انسانی' },
])

// status: 'work' | 'absent' | 'leave' | 'overtime'
// key: `${pid}_${day}`
const attendance = ref({})

function resetAttendance() { attendance.value = {} }

function getStatus(pid, day) { return attendance.value[`${pid}_${day}`] || 'work' }

const STATUS_CYCLE = ['work', 'absent', 'leave', 'overtime']
function cycleStatus(pid, day) {
    const cur = getStatus(pid, day)
    const idx = STATUS_CYCLE.indexOf(cur)
    attendance.value[`${pid}_${day}`] = STATUS_CYCLE[(idx + 1) % STATUS_CYCLE.length]
}

function getDayClass(pid, day) {
    const s = getStatus(pid, day)
    return {
        'ds-work': s === 'work',
        'ds-absent': s === 'absent',
        'ds-leave': s === 'leave',
        'ds-overtime': s === 'overtime',
    }
}

function getDayIcon(pid, day) {
    const icons = { work: 'mdi-check', absent: 'mdi-close', leave: 'mdi-umbrella', overtime: 'mdi-clock-plus-outline' }
    return icons[getStatus(pid, day)]
}

const weekdays = computed(() => workingDays.value.filter(d => !d.isWeekend).map(d => d.num))

function getWorkDays(pid) { return weekdays.value.filter(d => getStatus(pid, d) === 'work').length }
function getAbsentDays(pid) { return weekdays.value.filter(d => getStatus(pid, d) === 'absent').length }
function getLeaveDays(pid) { return weekdays.value.filter(d => getStatus(pid, d) === 'leave').length }
function getOvertimeDays(pid) { return weekdays.value.filter(d => getStatus(pid, d) === 'overtime').length }

// ── Filter & period ──────────────────────────────────────────────────────
const search = ref('')
const activePeriod = ref('all')
const periods = [
    { val: 'all', label: 'کل ماه' },
    { val: 'first', label: 'نیمه اول' },
    { val: 'last', label: 'نیمه دوم' },
]

const filteredPersonnel = computed(() => {
    if (!search.value.trim()) return personnel.value
    const q = search.value.trim().toLowerCase()
    return personnel.value.filter(p =>
        p.name.toLowerCase().includes(q) || p.code.toLowerCase().includes(q)
    )
})

// ── Stats ────────────────────────────────────────────────────────────────
const summaryStats = computed(() => {
    const total = personnel.value.length
    const workDays = weekdays.value.length
    const absent = personnel.value.reduce((s, p) => s + getAbsentDays(p.id), 0)
    const leave = personnel.value.reduce((s, p) => s + getLeaveDays(p.id), 0)
    const overtime = personnel.value.reduce((s, p) => s + getOvertimeDays(p.id), 0)
    return [
        { label: 'پرسنل فعال', val: toPersian(total), icon: 'mdi-account-multiple', color: '#63b3ed' },
        { label: 'روزهای کاری', val: toPersian(workDays), icon: 'mdi-calendar-check', color: '#48bb78' },
        { label: 'جمع غیبت', val: toPersian(absent), icon: 'mdi-account-cancel', color: '#f87171' },
        { label: 'جمع مرخصی', val: toPersian(leave), icon: 'mdi-umbrella', color: '#fbbf24' },
        { label: 'جمع اضافه‌کار', val: toPersian(overtime), icon: 'mdi-clock-plus', color: '#a78bfa' },
    ]
})

const absentSummary = computed(() =>
    personnel.value
        .map(p => ({ name: p.name, days: getAbsentDays(p.id) }))
        .filter(x => x.days > 0)
        .sort((a, b) => b.days - a.days)
)
const leaveSummary = computed(() =>
    personnel.value
        .map(p => ({ name: p.name, days: getLeaveDays(p.id) }))
        .filter(x => x.days > 0)
        .sort((a, b) => b.days - a.days)
)
const overtimeSummary = computed(() =>
    personnel.value
        .map(p => ({ name: p.name, days: getOvertimeDays(p.id) }))
        .filter(x => x.days > 0)
        .sort((a, b) => b.days - a.days)
)

// ── Legend ────────────────────────────────────────────────────────────────
const legend = [
    { label: 'حضور', cls: 'l-work' },
    { label: 'غیبت', cls: 'l-absent' },
    { label: 'مرخصی', cls: 'l-leave' },
    { label: 'اضافه‌کار', cls: 'l-overtime' },
    { label: 'تعطیل', cls: 'l-off' },
]

// ── Actions ───────────────────────────────────────────────────────────────
function saveAll() { showToast('کارکرد ماه ذخیره شد ✓', 'success') }
function exportExcel() { showToast('خروجی اکسل آماده دانلود است', 'success') }

let toastTimer
const toast = ref({ show: false, text: '', type: 'success' })
function showToast(text, type = 'success') {
    toast.value = { show: true, text, type }
    clearTimeout(toastTimer)
    toastTimer = setTimeout(() => { toast.value.show = false }, 3000)
}
</script>

<style scoped>
/* ── Root ─────────────────────────────────────────────────────────────── */
.page-root {
    min-height: 100vh;
    background: var(--bg);
    color: var(--text);
    font-family: var(--font);
    direction: rtl;
    position: relative;
    overflow-x: hidden;
}

.bg-glow {
    position: fixed;
    border-radius: 50%;
    pointer-events: none;
    z-index: 0;
    filter: blur(90px);
}

.bg-glow-1 {
    width: 700px;
    height: 500px;
    top: -15%;
    right: -5%;
    background: radial-gradient(ellipse, rgba(244, 114, 182, .05), transparent);
}

.bg-glow-2 {
    width: 500px;
    height: 400px;
    bottom: 5%;
    left: -5%;
    background: radial-gradient(ellipse, rgba(167, 139, 250, .04), transparent);
}

.bg-grid {
    position: fixed;
    inset: 0;
    pointer-events: none;
    z-index: 0;
    background:
        repeating-linear-gradient(0deg, transparent, transparent 40px, rgba(255, 255, 255, .007) 40px, rgba(255, 255, 255, .007) 41px),
        repeating-linear-gradient(90deg, transparent, transparent 40px, rgba(255, 255, 255, .007) 40px, rgba(255, 255, 255, .007) 41px);
}

.page-inner {
    position: relative;
    z-index: 1;
    padding: 28px 24px 60px;
    display: flex;
    flex-direction: column;
    gap: 18px;
}

/* ── Header ─────────────────────────────────────────────────────────── */
.page-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.brand {
    display: flex;
    align-items: center;
    gap: 12px;
}

.brand-mark {
    width: 42px;
    height: 42px;
    border-radius: 12px;
    background: linear-gradient(135deg, rgba(244, 114, 182, .2), rgba(244, 114, 182, .05));
    border: 1px solid rgba(244, 114, 182, .25);
    display: flex;
    align-items: center;
    justify-content: center;
}

.brand-name {
    font-size: 16px;
    font-weight: 700;
}

.brand-sub {
    font-size: 11px;
    color: var(--muted);
    margin-top: 2px;
}

.header-actions {
    display: flex;
    align-items: center;
    gap: 10px;
}

/* ── Month bar ──────────────────────────────────────────────────────── */
.month-bar {
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: var(--radius);
    padding: 12px 20px;
    display: flex;
    align-items: center;
    gap: 14px;
    flex-wrap: wrap;
}

.month-nav-btn {
    width: 34px;
    height: 34px;
    border-radius: 9px;
    border: 1px solid var(--border);
    background: rgba(255, 255, 255, .04);
    color: var(--muted);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all .2s;
}

.month-nav-btn:hover:not(:disabled) {
    border-color: #f472b6;
    color: #f472b6;
    background: rgba(244, 114, 182, .07);
}

.month-nav-btn:disabled {
    opacity: .3;
    cursor: default;
}

.month-display {
    display: flex;
    align-items: center;
    font-size: 15px;
    font-weight: 700;
    cursor: default;
    background: rgba(244, 114, 182, .07);
    border: 1px solid rgba(244, 114, 182, .15);
    padding: 6px 16px;
    border-radius: 10px;
}

.month-label {
    color: var(--text);
}

.month-sep {
    width: 1px;
    height: 28px;
    background: var(--border);
}

.period-tabs {
    display: flex;
    gap: 4px;
}

.period-tab {
    padding: 6px 14px;
    border-radius: 8px;
    border: 1px solid var(--border);
    background: transparent;
    color: var(--muted);
    font-family: var(--font);
    font-size: 12px;
    cursor: pointer;
    transition: all .2s;
}

.period-tab:hover {
    border-color: rgba(255, 255, 255, .15);
    color: var(--text);
}

.period-tab.active {
    background: rgba(244, 114, 182, .12);
    border-color: rgba(244, 114, 182, .3);
    color: #f472b6;
    font-weight: 700;
}

.search-mini {
    position: relative;
    display: flex;
    align-items: center;
}

.search-ic {
    position: absolute;
    right: 10px;
    color: var(--muted);
    pointer-events: none;
}

.search-mini-input {
    padding: 7px 30px 7px 12px;
    border-radius: 9px;
    background: rgba(255, 255, 255, .04);
    border: 1px solid var(--border);
    color: var(--text);
    font-family: var(--font);
    font-size: 12px;
    outline: none;
    width: 180px;
    transition: border-color .2s;
}

.search-mini-input:focus {
    border-color: rgba(244, 114, 182, .4);
}

.search-mini-input::placeholder {
    color: var(--muted);
}

/* ── Stats ──────────────────────────────────────────────────────────── */
.stats-row {
    display: flex;
    gap: 14px;
    flex-wrap: wrap;
}

.stat-card {
    flex: 1;
    min-width: 140px;
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: var(--radius);
    padding: 16px 18px;
    display: flex;
    align-items: center;
    gap: 12px;
    transition: border-color .2s, transform .2s;
}

.stat-card:hover {
    border-color: rgba(255, 255, 255, .12);
    transform: translateY(-1px);
}

.stat-icon-wrap {
    width: 38px;
    height: 38px;
    border-radius: 10px;
    flex-shrink: 0;
    background: rgba(255, 255, 255, .04);
    border: 1px solid var(--border);
    display: flex;
    align-items: center;
    justify-content: center;
}

.stat-val {
    font-size: 20px;
    font-weight: 700;
    line-height: 1;
}

.stat-label {
    font-size: 11px;
    color: var(--muted);
    margin-top: 4px;
}

/* ── Table ──────────────────────────────────────────────────────────── */
.table-card {
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: var(--radius);
    overflow: hidden;
}

.table-scroll {
    overflow-x: auto;
}

.att-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 12px;
}

.att-table thead tr {
    border-bottom: 1px solid var(--border);
    background: rgba(255, 255, 255, .025);
    position: sticky;
    top: 0;
    z-index: 4;
}

.att-table th {
    padding: 10px 6px;
    text-align: center;
    color: var(--muted);
    font-size: 11px;
    font-weight: 600;
    white-space: nowrap;
}

.th-name {
    text-align: right;
    padding-right: 20px;
    min-width: 180px;
}

.th-code {
    text-align: center;
    min-width: 70px;
}

.th-day {
    min-width: 36px;
    padding: 6px 4px;
}

.th-total {
    min-width: 64px;
    font-size: 11px;
}

.th-weekend {
    background: rgba(255, 255, 255, .012);
    color: #f87171 !important;
}

/* Sticky cols */
.th-sticky,
.td-sticky {
    position: sticky;
    background: #0d1117;
    z-index: 3;
}

.th-name,
.td-name {
    right: 0;
}

.th-code,
.td-code {
    right: 180px;
    border-left: 1px solid var(--border);
}

.att-row {
    border-bottom: 1px solid rgba(255, 255, 255, .04);
    transition: background .12s;
}

.att-row:last-child {
    border-bottom: none;
}

.att-row:hover {
    background: rgba(255, 255, 255, .018);
}

.att-row:hover .td-sticky {
    background: #111829;
}

.att-table td {
    padding: 8px 6px;
    vertical-align: middle;
    text-align: center;
}

.td-name {
    text-align: right;
    padding: 8px 20px 8px 8px;
}

.td-code {
    border-left: 1px solid var(--border);
}

/* Person cell */
.person-cell {
    display: flex;
    align-items: center;
    gap: 9px;
}

.avatar {
    width: 30px;
    height: 30px;
    border-radius: 8px;
    flex-shrink: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 12px;
    font-weight: 700;
    color: white;
}

.person-name {
    font-size: 12px;
    font-weight: 600;
    white-space: nowrap;
}

.person-dept {
    font-size: 10px;
    color: var(--muted);
    white-space: nowrap;
}

.code-badge {
    font-family: monospace;
    font-size: 10px;
    background: rgba(255, 255, 255, .05);
    border: 1px solid var(--border);
    padding: 2px 6px;
    border-radius: 5px;
    color: #94a3b8;
}

/* Weekend */
.td-weekend {
    background: rgba(255, 255, 255, .012);
}

.day-off-mark {
    color: #334155;
    font-size: 11px;
}

/* Day cell button */
.day-cell {
    width: 26px;
    height: 26px;
    border-radius: 7px;
    border: 1px solid transparent;
    background: rgba(255, 255, 255, .04);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0 auto;
    transition: all .15s;
}

.day-cell:hover {
    transform: scale(1.15);
}

.ds-work {
    background: rgba(52, 211, 153, .12);
    border-color: rgba(52, 211, 153, .25);
    color: #34d399;
}

.ds-absent {
    background: rgba(248, 113, 113, .15);
    border-color: rgba(248, 113, 113, .3);
    color: #f87171;
}

.ds-leave {
    background: rgba(251, 191, 36, .12);
    border-color: rgba(251, 191, 36, .25);
    color: #fbbf24;
}

.ds-overtime {
    background: rgba(167, 139, 250, .12);
    border-color: rgba(167, 139, 250, .25);
    color: #a78bfa;
}

/* Day header */
.day-num {
    font-size: 11px;
    font-weight: 700;
}

.day-name {
    font-size: 9px;
    margin-top: 1px;
}

/* Totals */
.td-total {
    font-weight: 700;
    font-size: 13px;
}

.td-work {
    color: #34d399;
}

.td-absent {
    color: #f87171;
}

.td-leave {
    color: #fbbf24;
}

.td-overtime {
    color: #a78bfa;
}

/* Legend */
.legend-bar {
    display: flex;
    align-items: center;
    gap: 20px;
    padding: 12px 20px;
    border-top: 1px solid var(--border);
    flex-wrap: wrap;
}

.legend-item {
    display: flex;
    align-items: center;
    gap: 6px;
    font-size: 11px;
    color: var(--muted);
}

.legend-dot {
    width: 10px;
    height: 10px;
    border-radius: 3px;
}

.l-work {
    background: rgba(52, 211, 153, .4);
}

.l-absent {
    background: rgba(248, 113, 113, .4);
}

.l-leave {
    background: rgba(251, 191, 36, .4);
}

.l-overtime {
    background: rgba(167, 139, 250, .4);
}

.l-off {
    background: rgba(255, 255, 255, .08);
}

.legend-note {
    color: #334155;
    font-size: 10.5px;
    margin-right: auto;
}

/* ── Detail grid ─────────────────────────────────────────────────────── */
.detail-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 16px;
}

.detail-card {
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: var(--radius);
    overflow: hidden;
}

.detail-card-title {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 14px 18px;
    border-bottom: 1px solid var(--border);
    font-size: 13px;
    font-weight: 700;
    color: var(--text);
    background: rgba(255, 255, 255, .02);
}

.detail-list {
    padding: 10px 12px;
    display: flex;
    flex-direction: column;
    gap: 6px;
    max-height: 240px;
    overflow-y: auto;
}

.detail-list::-webkit-scrollbar {
    width: 3px;
}

.detail-list::-webkit-scrollbar-thumb {
    background: #1a2535;
    border-radius: 10px;
}

.detail-row {
    display: flex;
    align-items: center;
    gap: 10px;
    padding: 8px 10px;
    border-radius: 10px;
    transition: background .15s;
}

.detail-row:hover {
    background: rgba(255, 255, 255, .03);
}

.detail-avatar {
    width: 30px;
    height: 30px;
    border-radius: 8px;
    flex-shrink: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 12px;
    font-weight: 700;
    color: white;
}

.detail-info {
    flex: 1;
    min-width: 0;
}

.detail-name {
    font-size: 12px;
    font-weight: 600;
}

.detail-sub {
    font-size: 10px;
    color: var(--muted);
    margin-top: 1px;
}

.detail-badge {
    padding: 3px 10px;
    border-radius: 99px;
    font-size: 12px;
    font-weight: 700;
}

.badge-red {
    background: rgba(248, 113, 113, .12);
    color: #f87171;
    border: 1px solid rgba(248, 113, 113, .2);
}

.badge-yellow {
    background: rgba(251, 191, 36, .12);
    color: #fbbf24;
    border: 1px solid rgba(251, 191, 36, .2);
}

.badge-purple {
    background: rgba(167, 139, 250, .12);
    color: #a78bfa;
    border: 1px solid rgba(167, 139, 250, .2);
}

.detail-empty {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 8px;
    padding: 30px 20px;
}

.detail-empty p {
    color: var(--muted);
    font-size: 12px;
    margin: 0;
}

/* ── Buttons ────────────────────────────────────────────────────────── */
.btn {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 9px 16px;
    border-radius: 10px;
    border: none;
    font-family: var(--font);
    font-size: 13px;
    font-weight: 600;
    cursor: pointer;
    transition: all .22s;
    text-decoration: none;
}

.btn-primary {
    background: linear-gradient(135deg, #f472b6, #ec4899);
    color: white;
    box-shadow: 0 4px 16px rgba(244, 114, 182, .3);
}

.btn-primary:hover {
    transform: translateY(-1px);
    box-shadow: 0 6px 20px rgba(244, 114, 182, .4);
}

.btn-ghost {
    background: rgba(255, 255, 255, .05);
    color: var(--muted);
    border: 1px solid var(--border);
}

.btn-ghost:hover {
    border-color: rgba(255, 255, 255, .15);
    color: var(--text);
}

/* ── Toast ──────────────────────────────────────────────────────────── */
.toast {
    position: fixed;
    bottom: 28px;
    left: 50%;
    transform: translateX(-50%);
    padding: 12px 22px;
    border-radius: 12px;
    font-size: 13px;
    font-weight: 600;
    z-index: 200;
    display: flex;
    align-items: center;
    gap: 6px;
    box-shadow: 0 10px 40px rgba(0, 0, 0, .5);
    white-space: nowrap;
}

.toast-success {
    background: rgba(52, 211, 153, .15);
    border: 1px solid rgba(52, 211, 153, .3);
    color: #34d399;
}

.toast-error {
    background: rgba(248, 113, 113, .15);
    border: 1px solid rgba(248, 113, 113, .3);
    color: #f87171;
}

.toast-anim-enter-active,
.toast-anim-leave-active {
    transition: all .3s;
}

.toast-anim-enter-from {
    opacity: 0;
    transform: translate(-50%, 16px);
}

.toast-anim-leave-to {
    opacity: 0;
    transform: translate(-50%, 16px);
}

@media (max-width: 900px) {
    .detail-grid {
        grid-template-columns: 1fr;
    }

    .stats-row .stat-card {
        min-width: 120px;
    }
}
</style>
