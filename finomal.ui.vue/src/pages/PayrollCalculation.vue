<template>
    <div class="payroll-page" dir="rtl">
        <!-- Header Section -->
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title">
                    <v-icon color="primary" class="mr-2" size="28">mdi-calculator-variant-outline</v-icon>
                    محاسبه حقوق و دستمزد
                </h1>
                <p class="page-subtitle">محاسبه گروهی و انفرادی حقوق پرسنل بر اساس کارکرد ماهانه</p>
            </div>

            <div class="header-actions">
                <!-- Year/Month Selection -->
                <div class="period-selectors">
                    <v-select v-model="selectedYear" :items="years" variant="solo-filled" density="comfortable"
                        hide-details bg-color="rgba(30, 41, 59, 0.4)" class="period-select" rounded="lg"></v-select>
                    <v-select v-model="selectedMonth" :items="months" variant="solo-filled" density="comfortable"
                        hide-details bg-color="rgba(30, 41, 59, 0.4)" class="period-select" rounded="lg"></v-select>
                </div>

                <v-btn color="primary" prepend-icon="mdi-calculator" elevation="2" class="calculate-btn" rounded="lg"
                    @click="calculatePayroll" :loading="isCalculating">
                    محاسبه حقوق
                </v-btn>
            </div>
        </header>

        <!-- Stats Review Summary -->
        <v-row class="mb-5 stats-row">
            <v-col cols="12" md="3" v-for="stat in summaryStats" :key="stat.title">
                <v-card class="stat-card" elevation="0">
                    <div class="stat-icon-wrapper" :class="`bg-${stat.color}-lighten-4`">
                        <v-icon :color="stat.color" size="24">{{ stat.icon }}</v-icon>
                    </div>
                    <div class="stat-info">
                        <p class="stat-title">{{ stat.title }}</p>
                        <h3 class="stat-value mono-number">{{ stat.value }}</h3>
                    </div>
                </v-card>
            </v-col>
        </v-row>

        <!-- Main Data Grid -->
        <v-card class="content-card" elevation="0">
            <div class="table-toolbar">
                <div class="search-box">
                    <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجوی پرسنل..."
                        variant="solo-filled" density="comfortable" hide-details rounded="lg"
                        bg-color="rgba(255, 255, 255, 0.03)"></v-text-field>
                </div>
                <div class="toolbar-actions">
                    <v-btn variant="outlined" color="primary" class="mr-2" rounded="lg" prepend-icon="mdi-printer">
                        چاپ فیش‌ها
                    </v-btn>
                    <v-btn variant="outlined" color="success" rounded="lg" prepend-icon="mdi-file-export-outline">
                        تهیه فایل بانک
                    </v-btn>
                    <v-btn variant="outlined" color="teal" rounded="lg" prepend-icon="mdi-clipboard-text-outline"
                        class="mr-2">
                        تهیه دیسکت بیمه
                    </v-btn>
                </div>
            </div>

            <div class="table-container">
                <table class="payroll-table">
                    <thead>
                        <tr>
                            <th class="checkbox-col">
                                <v-checkbox hide-details density="compact" color="primary"></v-checkbox>
                            </th>
                            <th>کد - نام پرسنل</th>
                            <th class="text-center">کارکرد (روز)</th>
                            <th class="text-left">حقوق پایه (ریال)</th>
                            <th class="text-left text-success">جمع مزایا (ریال)</th>
                            <th class="text-left text-error">جمع کسورات (ریال)</th>
                            <th class="text-left font-weight-bold">خالص پرداختی (ریال)</th>
                            <th class="text-center">وضعیت</th>
                            <th class="text-left text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in filteredList" :key="item.id" class="table-row">
                            <td class="checkbox-col">
                                <v-checkbox hide-details density="compact" color="primary"></v-checkbox>
                            </td>
                            <!-- Employee Info -->
                            <td>
                                <div class="employee-info">
                                    <span class="emp-id">{{ item.code }}</span>
                                    <span class="emp-name">{{ item.name }}</span>
                                </div>
                            </td>

                            <td class="text-center mono-number">{{ item.workDays }}</td>

                            <td class="text-left mono-number">{{ formatCurrency(item.baseSalary) }}</td>
                            <td class="text-left text-success mono-number">{{ formatCurrency(item.totalAllowances) }}
                            </td>
                            <td class="text-left text-error mono-number">{{ formatCurrency(item.totalDeductions) }}</td>
                            <td class="text-left font-weight-bold mono-number text-highlight">{{
                                formatCurrency(item.netPay) }}</td>

                            <td class="text-center">
                                <v-chip :color="item.status === 'محاسبه شده' ? 'success' : 'grey'" size="x-small" label
                                    variant="flat" class="font-weight-bold">
                                    {{ item.status }}
                                </v-chip>
                            </td>

                            <td class="text-center action-buttons">
                                <!-- View Slip -->
                                <v-tooltip text="مشاهده فیش حقوقی" location="top">
                                    <template v-slot:activator="{ props }">
                                        <v-btn icon="mdi-file-document-outline" variant="text" size="small" color="info"
                                            v-bind="props" @click="openSlipDialog(item)"></v-btn>
                                    </template>
                                </v-tooltip>
                                <!-- Edit Detail -->
                                <v-tooltip text="اصلاح دستی عوامل" location="top">
                                    <template v-slot:activator="{ props }">
                                        <v-btn icon="mdi-pencil-outline" variant="text" size="small" color="warning"
                                            v-bind="props"></v-btn>
                                    </template>
                                </v-tooltip>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <div v-if="filteredList.length === 0" class="empty-state py-12 text-center">
                    <v-icon size="64" color="grey-darken-2" class="mb-4">mdi-text-box-search-outline</v-icon>
                    <h3 class="text-h6 text-grey-lighten-1">رکوردی یافت نشد</h3>
                </div>
            </div>
        </v-card>

        <!-- Pay Slip Dialog (Modal) -->
        <v-dialog v-model="slipDialog" max-width="850">
            <v-card class="slip-dialog-card" elevation="24">
                <!-- Dialog Header -->
                <div class="slip-header">
                    <div class="slip-brand">
                        <v-icon size="28" color="primary" class="mr-2">mdi-chart-finance</v-icon>
                        <span class="brand-text">فینومال</span>
                    </div>
                    <div class="slip-title">
                        <h3>فیش حقوقی پرسنل</h3>
                        <span class="slip-period">{{ selectedMonth }} ماه {{ selectedYear }}</span>
                    </div>
                    <v-btn icon="mdi-close" variant="text" @click="slipDialog = false" color="grey"></v-btn>
                </div>

                <div class="slip-body" v-if="selectedEmployee">

                    <!-- Employee Base Info -->
                    <div class="emp-header-info">
                        <div class="info-cell"><span class="label">نام و نام خانوادگی:</span> <span class="val">{{
                                selectedEmployee.name }}</span></div>
                        <div class="info-cell"><span class="label">کد پرسنلی:</span> <span class="val">{{
                                selectedEmployee.code
                                }}</span></div>
                        <div class="info-cell"><span class="label">شماره حساب:</span> <span
                                class="val">۶۱۰۴۳۳۷۱...</span></div>
                        <div class="info-cell"><span class="label">کارکرد (روز):</span> <span class="val">{{
                                selectedEmployee.workDays }}</span></div>
                    </div>

                    <v-row>
                        <!-- Allowances (مزایا) -->
                        <v-col cols="12" md="6">
                            <div class="slip-panel panel-allowance">
                                <div class="panel-title text-success">
                                    <v-icon size="16" class="mr-1">mdi-plus-circle</v-icon>
                                    مزایا
                                </div>
                                <div class="panel-list">
                                    <div class="list-item"><span>حقوق پایه</span> <span>{{
                                            formatCurrency(selectedEmployee.baseSalary) }}</span></div>
                                    <div class="list-item"><span>حق مسکن</span> <span>{{ formatCurrency(9000000)
                                            }}</span></div>
                                    <div class="list-item"><span>بن خواروبار</span> <span>{{ formatCurrency(11000000)
                                            }}</span>
                                    </div>
                                    <div class="list-item"><span>حق اولاد</span> <span>{{ formatCurrency(5308284)
                                            }}</span>
                                    </div>
                                    <div class="list-item"><span>اضافه کاری</span> <span>{{ formatCurrency(12000000)
                                            }}</span>
                                    </div>
                                </div>
                                <div class="panel-total text-success">
                                    <span>جمع مزایا</span>
                                    <span class="font-weight-bold">{{ formatCurrency(selectedEmployee.totalAllowances)
                                        }}</span>
                                </div>
                            </div>
                        </v-col>

                        <!-- Deductions (کسورات) -->
                        <v-col cols="12" md="6">
                            <div class="slip-panel panel-deduction">
                                <div class="panel-title text-error">
                                    <v-icon size="16" class="mr-1">mdi-minus-circle</v-icon>
                                    کسورات
                                </div>
                                <div class="panel-list">
                                    <div class="list-item"><span>بیمه تامین اجتماعی (۷٪)</span> <span>{{
                                            formatCurrency(6040520)
                                            }}</span></div>
                                    <div class="list-item"><span>مالیات بر درآمد</span> <span>{{ formatCurrency(2500000)
                                            }}</span></div>
                                    <div class="list-item"><span>مساعده</span> <span>{{ formatCurrency(0) }}</span>
                                    </div>
                                    <div class="list-item"><span>جریمه / غیبت</span> <span>{{ formatCurrency(0)
                                            }}</span></div>
                                    <!-- Filler for height balance -->
                                    <div class="list-item filler"></div>
                                </div>
                                <div class="panel-total text-error">
                                    <span>جمع کسورات</span>
                                    <span class="font-weight-bold">{{ formatCurrency(selectedEmployee.totalDeductions)
                                        }}</span>
                                </div>
                            </div>
                        </v-col>
                    </v-row>

                    <div class="net-pay-box">
                        <span class="net-label">خالص پرداختی:</span>
                        <span class="net-val">{{ formatCurrency(selectedEmployee.netPay) }} ریال</span>
                    </div>

                </div>

                <div class="slip-actions">
                    <v-btn variant="outlined" color="primary" @click="slipDialog = false" class="px-6"
                        rounded="lg">بستن</v-btn>
                    <v-btn color="primary" prepend-icon="mdi-printer" class="px-6" rounded="lg">چاپ فیش</v-btn>
                </div>
            </v-card>
        </v-dialog>
    </div>
</template>

<script setup>
import { ref, computed } from 'vue';

const years = ['۱۴۰۲', '۱۴۰۳', '۱۴۰۴'];
const months = ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'];

const selectedYear = ref('۱۴۰۳');
const selectedMonth = ref('بهمن');
const search = ref('');
const isCalculating = ref(false);
const slipDialog = ref(false);
const selectedEmployee = ref(null);

const payrollData = ref([
    { id: 1, code: '1001', name: 'علی احمدی', workDays: 30, baseSalary: 71661840, totalAllowances: 108970124, totalDeductions: 8540520, netPay: 100429604, status: 'محاسبه شده' },
    { id: 2, code: '1002', name: 'سارا کریمی', workDays: 30, baseSalary: 71661840, totalAllowances: 91661840, totalDeductions: 6040520, netPay: 85621320, status: 'محاسبه شده' },
    { id: 3, code: '1003', name: 'محمد رضایی', workDays: 28, baseSalary: 66884384, totalAllowances: 86884384, totalDeductions: 5800000, netPay: 81084384, status: 'محاسبه شده' },
    { id: 4, code: '1004', name: 'نگین تهرانی', workDays: 30, baseSalary: 95000000, totalAllowances: 125000000, totalDeductions: 12500000, netPay: 112500000, status: 'تایید نهایی' },
    { id: 5, code: '1005', name: 'حسین جعفری', workDays: 15, baseSalary: 35830920, totalAllowances: 45830920, totalDeductions: 3020260, netPay: 42810660, status: 'خام' },
]);

// Summaries based on data
const summaryStats = computed(() => {
    const count = payrollData.value.length;
    const tNet = payrollData.value.reduce((acc, curr) => acc + curr.netPay, 0);
    const tDed = payrollData.value.reduce((acc, curr) => acc + curr.totalDeductions, 0);

    return [
        { title: 'تعداد پرسنل', value: String(count), icon: 'mdi-account-group', color: 'blue' },
        { title: 'مجموع ناخالص', value: formatCurrency(tNet + tDed), icon: 'mdi-cash-multiple', color: 'orange' },
        { title: 'مجموع کسورات', value: formatCurrency(tDed), icon: 'mdi-minus-circle-outline', color: 'error' },
        { title: 'مجموع خالص پرداختی', value: formatCurrency(tNet), icon: 'mdi-currency-usd', color: 'success' },
    ];
});

const filteredList = computed(() => {
    if (!search.value) return payrollData.value;
    return payrollData.value.filter(item => item.name.includes(search.value) || item.code.includes(search.value));
});

function formatCurrency(val) {
    return Number(val).toLocaleString('en-US'); // To display with commas
}

function calculatePayroll() {
    isCalculating.value = true;
    // Simulate delay
    setTimeout(() => {
        isCalculating.value = false;
        // In real scenario we would refresh data here
    }, 1000);
}

function openSlipDialog(emp) {
    selectedEmployee.value = emp;
    slipDialog.value = true;
}

</script>

<style scoped>
.payroll-page {
    font-family: 'Vazirmatn', sans-serif;
    color: #e2e8f0;
    max-width: 1400px;
    margin: 0 auto;
}

/* Header */
.page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 24px;
}

.page-title {
    font-size: 24px;
    font-weight: 800;
    color: #f8fafc;
    display: flex;
    align-items: center;
    margin-bottom: 4px;
}

.page-subtitle {
    font-size: 13px;
    color: #94a3b8;
}

.header-actions {
    display: flex;
    align-items: center;
    gap: 16px;
}

.period-selectors {
    display: flex;
    gap: 8px;
    width: 250px;
}

.period-select :deep(.v-field) {
    border: 1px solid rgba(255, 255, 255, 0.05);
}

.period-select :deep(.v-field:hover) {
    border-color: rgba(56, 189, 248, 0.3);
}

.calculate-btn {
    font-weight: 600;
    letter-spacing: 0;
    background: linear-gradient(135deg, #0ea5e9, #2563eb);
}

/* Stats */
.stat-card {
    background: rgba(15, 23, 42, 0.6);
    border: 1px solid rgba(255, 255, 255, 0.05);
    backdrop-filter: blur(10px);
    border-radius: 16px;
    padding: 16px 20px;
    display: flex;
    align-items: center;
    gap: 16px;
    transition: transform 0.2s, border-color 0.2s;
}

.stat-card:hover {
    transform: translateY(-2px);
    border-color: rgba(56, 189, 248, 0.2);
}

.stat-icon-wrapper {
    width: 48px;
    height: 48px;
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
}

.bg-blue-lighten-4 {
    background: rgba(56, 189, 248, 0.1) !important;
}

.bg-orange-lighten-4 {
    background: rgba(249, 115, 22, 0.1) !important;
}

.bg-error-lighten-4 {
    background: rgba(239, 68, 68, 0.1) !important;
}

.bg-success-lighten-4 {
    background: rgba(34, 197, 94, 0.1) !important;
}

.stat-title {
    font-size: 12px;
    color: #94a3b8;
    margin-bottom: 2px;
}

.stat-value {
    font-size: 20px;
    font-weight: 800;
    color: #e2e8f0;
}

.mono-number {
    font-family: inherit;
    letter-spacing: 0.5px;
}

/* Table Area */
.content-card {
    background: rgba(15, 23, 42, 0.6);
    border: 1px solid rgba(255, 255, 255, 0.05);
    border-radius: 16px;
    overflow: hidden;
}

.table-toolbar {
    padding: 16px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}

.search-box {
    width: 300px;
}

.search-box :deep(.v-field) {
    border-radius: 12px;
    border: 1px solid rgba(255, 255, 255, 0.05);
}

.toolbar-actions .v-btn {
    text-transform: none;
    font-weight: 500;
    letter-spacing: 0;
    font-size: 12px;
}

.table-container {
    overflow-x: auto;
}

.payroll-table {
    width: 100%;
    border-collapse: collapse;
    text-align: right;
    min-width: 900px;
}

.payroll-table th {
    padding: 14px 16px;
    font-size: 11px;
    font-weight: 700;
    color: #94a3b8;
    background: rgba(255, 255, 255, 0.015);
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
    white-space: nowrap;
}

.payroll-table td {
    padding: 12px 16px;
    font-size: 13px;
    color: #cbd5e1;
    border-bottom: 1px solid rgba(255, 255, 255, 0.02);
    vertical-align: middle;
}

.checkbox-col {
    width: 50px;
    text-align: center !important;
    padding-right: 8px !important;
}

.table-row:hover td {
    background: rgba(255, 255, 255, 0.02);
}

.employee-info {
    display: flex;
    align-items: center;
    gap: 12px;
}

.emp-id {
    font-size: 11px;
    color: #64748b;
    background: rgba(255, 255, 255, 0.04);
    padding: 2px 6px;
    border-radius: 4px;
}

.emp-name {
    font-weight: 600;
    color: #e2e8f0;
}

.text-highlight {
    color: #38bdf8;
    font-size: 14px;
}

/* Slip Dialog - Sepidar Style adapted to Dark mode */
.slip-dialog-card {
    background: #0f172a;
    border: 1px solid rgba(56, 189, 248, 0.15);
    border-radius: 16px;
    font-family: 'Vazirmatn', sans-serif;
}

.slip-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 20px 24px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.06);
    background: #1e293b;
}

.slip-title {
    text-align: center;
}

.slip-title h3 {
    font-size: 18px;
    font-weight: 800;
    color: #e2e8f0;
    margin: 0;
}

.slip-period {
    font-size: 12px;
    color: #94a3b8;
}

.slip-brand {
    display: flex;
    align-items: center;
}

.brand-text {
    font-weight: 800;
    font-size: 16px;
    letter-spacing: 1px;
    color: #38bdf8;
}

.slip-body {
    padding: 24px;
}

.emp-header-info {
    display: flex;
    flex-wrap: wrap;
    border: 1px solid rgba(255, 255, 255, 0.06);
    border-radius: 8px;
    padding: 12px 16px;
    margin-bottom: 24px;
    background: #1aa5cf08;
}

.info-cell {
    width: 25%;
    display: flex;
    flex-direction: column;
    gap: 4px;
}

.info-cell .label {
    font-size: 11px;
    color: #64748b;
}

.info-cell .val {
    font-size: 13px;
    font-weight: 600;
    color: #e2e8f0;
}

.slip-panel {
    border: 1px solid rgba(255, 255, 255, 0.05);
    border-radius: 10px;
    overflow: hidden;
    height: 100%;
    display: flex;
    flex-direction: column;
}

.panel-title {
    padding: 12px 16px;
    font-weight: 700;
    font-size: 14px;
    background: rgba(255, 255, 255, 0.02);
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
    display: flex;
    align-items: center;
}

.panel-list {
    padding: 8px 0;
    flex: 1;
}

.list-item {
    display: flex;
    justify-content: space-between;
    padding: 8px 16px;
    font-size: 13px;
    color: #cbd5e1;
}

.list-item span:last-child {
    font-family: inherit;
    letter-spacing: 0.5px;
}

.list-item.filler {
    height: 35px;
    /* keep columns even */
}

.panel-total {
    padding: 14px 16px;
    display: flex;
    justify-content: space-between;
    background: rgba(255, 255, 255, 0.015);
    border-top: 1px solid rgba(255, 255, 255, 0.05);
    font-size: 14px;
}

.net-pay-box {
    margin-top: 24px;
    background: rgba(56, 189, 248, 0.08);
    border: 1px dashed rgba(56, 189, 248, 0.3);
    border-radius: 12px;
    padding: 16px 24px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.net-pay-box .net-label {
    font-size: 16px;
    color: #38bdf8;
    font-weight: 700;
}

.net-pay-box .net-val {
    font-size: 24px;
    font-weight: 800;
    color: #e2e8f0;
    font-family: inherit;
    letter-spacing: 1px;
}

.slip-actions {
    padding: 16px 24px;
    border-top: 1px solid rgba(255, 255, 255, 0.06);
    display: flex;
    justify-content: flex-end;
    gap: 12px;
    background: rgba(255, 255, 255, 0.01);
}
</style>
