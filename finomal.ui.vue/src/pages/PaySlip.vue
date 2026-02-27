<template>
    <div class="payslip-page" dir="rtl">
        <!-- Header -->
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title">
                    <v-icon color="primary" class="mr-2" size="28">mdi-file-document-outline</v-icon>
                    صدور و چاپ فیش حقوقی
                </h1>
                <p class="page-subtitle">مشاهده تاریخچه حقوق پرسنل و چاپ گروهی/فردی فیش‌های حقوقی</p>
            </div>

            <div class="header-actions">
                <!-- Year/Month Selection -->
                <div class="period-selectors">
                    <v-select v-model="selectedYear" :items="years" variant="solo-filled" density="comfortable"
                        hide-details bg-color="rgba(30, 41, 59, 0.4)" class="period-select" rounded="lg"
                        label="سال"></v-select>
                    <v-select v-model="selectedMonth" :items="months" variant="solo-filled" density="comfortable"
                        hide-details bg-color="rgba(30, 41, 59, 0.4)" class="period-select" rounded="lg"
                        label="ماه"></v-select>
                </div>

                <v-btn color="primary" prepend-icon="mdi-printer" elevation="2" class="print-btn" rounded="lg">
                    چاپ گروهی فیش‌ها
                </v-btn>
            </div>
        </header>

        <!-- Main Content -->
        <v-card class="content-card" elevation="0">
            <div class="table-toolbar">
                <div class="search-box">
                    <v-text-field v-model="search" prepend-inner-icon="mdi-magnify"
                        placeholder="جستجوی کد، نام یا کدملی..." variant="solo-filled" density="comfortable"
                        hide-details rounded="lg" bg-color="rgba(255, 255, 255, 0.03)"></v-text-field>
                </div>
                <div class="toolbar-stats text-body-2 text-grey-lighten-1">
                    تعداد فیش‌های صادر شده این ماه: <strong class="text-white mx-1 mono-number">{{ filteredSlips.length
                        }}</strong> عدد
                </div>
            </div>

            <div class="table-container">
                <table class="payslip-list-table">
                    <thead>
                        <tr>
                            <th class="checkbox-col">
                                <v-checkbox hide-details density="compact" color="primary"></v-checkbox>
                            </th>
                            <th>کد - نام پرسنل</th>
                            <th>سمت</th>
                            <th class="text-left">مجموع ناخالص (ریال)</th>
                            <th class="text-left">کسورات (ریال)</th>
                            <th class="text-left font-weight-bold">خالص پرداختی (ریال)</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in filteredSlips" :key="item.id" class="table-row">
                            <td class="checkbox-col">
                                <v-checkbox hide-details density="compact" color="primary"></v-checkbox>
                            </td>
                            <td>
                                <div class="d-flex align-center gap-3">
                                    <v-avatar size="36" color="surface-variant" variant="tonal">
                                        <span class="text-caption font-weight-bold">{{ item.initials }}</span>
                                    </v-avatar>
                                    <div class="employee-info">
                                        <span class="emp-name text-body-2 font-weight-bold">{{ item.name }}</span>
                                        <span class="emp-code text-caption text-grey">کد: {{ item.code }}</span>
                                    </div>
                                </div>
                            </td>

                            <td class="text-caption text-grey-lighten-1">{{ item.position }}</td>

                            <td class="text-left mono-number">{{ formatCurrency(item.gross) }}</td>
                            <td class="text-left text-error mono-number">{{ formatCurrency(item.deductions) }}</td>
                            <td class="text-left font-weight-bold text-success mono-number">{{ formatCurrency(item.net)
                                }}</td>

                            <td class="text-center action-buttons">
                                <!-- Action: Open Slip -->
                                <v-btn variant="tonal" color="info" size="small" rounded="lg"
                                    prepend-icon="mdi-eye-outline" class="font-weight-bold letter-spacing-0"
                                    @click="openSlip(item)">
                                    مشاهده فیش
                                </v-btn>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <!-- Empty State -->
                <div v-if="filteredSlips.length === 0" class="empty-state py-12 text-center">
                    <v-icon size="64" color="grey-darken-2" class="mb-4">mdi-receipt-text-custom</v-icon>
                    <h3 class="text-h6 text-grey-lighten-1">فیشی یافت نشد!</h3>
                    <p class="text-body-2 text-grey">در اطلاعات ماه و سال انتخابی هیچ فیش صادر شده‌ای وجود ندارد.</p>
                </div>
            </div>
        </v-card>

        <!-- Detailed Payslip Modal (For Print/View) -->
        <v-dialog v-model="slipDialog" max-width="900" transition="dialog-bottom-transition">
            <v-card class="print-dialog-card" elevation="24">
                <!-- Action Bar (Hidden on print) -->
                <div class="print-actions d-print-none">
                    <span class="action-title">پیش‌نمایش فیش حقوقی</span>
                    <div class="d-flex gap-2">
                        <v-btn variant="outlined" color="primary" @click="slipDialog = false"
                            rounded="lg">بازگشت</v-btn>
                        <v-btn variant="flat" color="primary" prepend-icon="mdi-printer" @click="printSlip"
                            rounded="lg">چاپ (Ctrl+P)</v-btn>
                    </div>
                </div>

                <!-- The Printable Area -->
                <div class="printable-slip" id="printable-area" v-if="selectedSlip">
                    <!-- Official Slip Header -->
                    <div class="slip-doc-header">
                        <div class="logo-area">
                            <v-icon size="40" color="#3d4957">mdi-chart-finance</v-icon>
                        </div>
                        <div class="text-center brand-area">
                            <h2 class="company-name">شرکت توسعه فناوری فینومال</h2>
                            <h3 class="doc-title">فیش حقوق و دستمزد</h3>
                            <div class="doc-period">دوره: {{ selectedMonth }} ماه {{ selectedYear }}</div>
                        </div>
                        <div class="date-area">
                            <div class="date-row"><span>تاریخ صدور:</span> <span>۱۴۰۳/۱۱/۳۰</span></div>
                            <div class="date-row"><span>شماره فیش:</span> <span>{{ Math.floor(Math.random() * 899999 +
                                    100000) }}</span></div>
                        </div>
                    </div>

                    <!-- Employee Info Section -->
                    <div class="slip-section-title mt-4">مشخصات پرسنل</div>
                    <div class="slip-info-grid">
                        <div class="info-item"><span>نام و نام خانوادگی:</span> <strong>{{ selectedSlip.name }}</strong>
                        </div>
                        <div class="info-item"><span>کد پرسنلی:</span> <strong>{{ selectedSlip.code }}</strong></div>
                        <div class="info-item"><span>کد ملی:</span> <strong>۰۱۲۳۴۵۶۷۸۹</strong></div>
                        <div class="info-item"><span>سمت:</span> <strong>{{ selectedSlip.position }}</strong></div>
                        <div class="info-item"><span>شماره حساب:</span> <strong>۶۱۰۴۳۳۷۱...</strong></div>
                        <div class="info-item"><span>کارکرد ماه (روز):</span> <strong>۳۰</strong></div>
                    </div>

                    <!-- Salary Details Table -->
                    <div class="slip-tables-container mt-6">
                        <!-- Allowances Table -->
                        <div class="slip-half-table">
                            <div class="slip-table-title text-success">حقوق و مزایا</div>
                            <table class="detail-table">
                                <thead>
                                    <tr>
                                        <th>شرح</th>
                                        <th class="text-left">مبلغ (ریال)</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>حقوق پایه</td>
                                        <td class="text-left">۷۱,۶۶۱,۸۴۰</td>
                                    </tr>
                                    <tr>
                                        <td>حق مسکن</td>
                                        <td class="text-left">۹,۰۰۰,۰۰۰</td>
                                    </tr>
                                    <tr>
                                        <td>بن خواروبار</td>
                                        <td class="text-left">۱۴,۰۰۰,۰۰۰</td>
                                    </tr>
                                    <tr>
                                        <td>حق اولاد</td>
                                        <td class="text-left">۷,۱۶۶,۱۸۴</td>
                                    </tr>
                                    <tr>
                                        <td>اضافه کاری</td>
                                        <td class="text-left">۱۲,۵۰۰,۰۰۰</td>
                                    </tr>
                                    <!-- Empty records to stretch -->
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr class="table-total-row">
                                        <td>جمع مزایا</td>
                                        <td class="text-left font-weight-bold">{{ formatCurrency(selectedSlip.gross) }}
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>

                        <!-- Deductions Table -->
                        <div class="slip-half-table">
                            <div class="slip-table-title text-error">کسورات</div>
                            <table class="detail-table">
                                <thead>
                                    <tr>
                                        <th>شرح</th>
                                        <th class="text-left">مبلغ (ریال)</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>حق بیمه (۷٪ سهم کارگر)</td>
                                        <td class="text-left">۶,۰۴۰,۵۲۰</td>
                                    </tr>
                                    <tr>
                                        <td>مالیات بر درآمد</td>
                                        <td class="text-left">۲,۵۰۰,۰۰۰</td>
                                    </tr>
                                    <tr>
                                        <td>مساعده پرداختی</td>
                                        <td class="text-left">۰</td>
                                    </tr>
                                    <tr>
                                        <td>جریمه / غیبت</td>
                                        <td class="text-left">۰</td>
                                    </tr>
                                    <tr>
                                        <td>سایر کسورات</td>
                                        <td class="text-left">۰</td>
                                    </tr>
                                    <!-- Empty records to stretch -->
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr class="table-total-row">
                                        <td>جمع کسورات</td>
                                        <td class="text-left font-weight-bold">{{
                                            formatCurrency(selectedSlip.deductions) }}</td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                    </div>

                    <!-- Net Pay Summary -->
                    <div class="slip-net-summary mt-6">
                        <div class="net-text">مبلغ خالص پرداختی (ریال)</div>
                        <div class="net-amount">{{ formatCurrency(selectedSlip.net) }}</div>
                    </div>
                    <div class="slip-net-words text-center mt-2 text-caption">
                        مبلغ به حروف: صد و چهار میلیون و دویست و هشتاد و هفت هزار و پانصد و چهار ریال
                    </div>

                    <!-- Signatures -->
                    <div class="slip-signatures mt-10">
                        <div class="sig-box">
                            <span>امضاء مدیر مالی</span>
                        </div>
                        <div class="sig-box">
                            <span>امضاء دریافت کننده (پرسنل)</span>
                        </div>
                    </div>
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

const slipDialog = ref(false);
const selectedSlip = ref(null);

const slipsData = ref([
    { id: 1, code: '1001', name: 'علی احمدی', initials: 'ع.ا', position: 'توسعه‌دهنده فرانت‌اند', gross: 114328024, deductions: 8540520, net: 105787504 },
    { id: 2, code: '1002', name: 'سارا کریمی', initials: 'س.ک', position: 'طراح رابط کاربری', gross: 94661840, deductions: 6040520, net: 88621320 },
    { id: 3, code: '1003', name: 'محمد رضایی', initials: 'م.ر', position: 'مدیر پروژه', gross: 126884384, deductions: 9800000, net: 117084384 },
    { id: 4, code: '1004', name: 'نگین تهرانی', initials: 'ن.ت', position: 'حسابدار', gross: 115000000, deductions: 12500000, net: 102500000 },
]);

const filteredSlips = computed(() => {
    if (!search.value) return slipsData.value;
    return slipsData.value.filter(item =>
        item.name.includes(search.value) ||
        item.code.includes(search.value) ||
        item.position.includes(search.value)
    );
});

function formatCurrency(val) {
    return Number(val).toLocaleString('en-US');
}

function openSlip(item) {
    selectedSlip.value = item;
    slipDialog.value = true;
}

function printSlip() {
    // In a real scenario, this would trigger an actual print
    // Or use a library like html2pdf
    window.print();
}
</script>

<style scoped>
.payslip-page {
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

.print-btn {
    font-weight: 600;
    letter-spacing: 0;
    background: linear-gradient(135deg, #0ea5e9, #2563eb);
}

/* Content Area */
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
    width: 320px;
}

.search-box :deep(.v-field) {
    border-radius: 12px;
    border: 1px solid rgba(255, 255, 255, 0.05);
}

.table-container {
    overflow-x: auto;
}

.payslip-list-table {
    width: 100%;
    border-collapse: collapse;
    text-align: right;
    min-width: 800px;
}

.payslip-list-table th {
    padding: 14px 16px;
    font-size: 12px;
    font-weight: 700;
    color: #94a3b8;
    background: rgba(255, 255, 255, 0.015);
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
    white-space: nowrap;
}

.payslip-list-table td {
    padding: 12px 16px;
    font-size: 13.5px;
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
    background: rgba(255, 255, 255, 0.03);
}

.employee-info {
    display: flex;
    flex-direction: column;
}

.mono-number {
    font-family: inherit;
    letter-spacing: 0.5px;
}

.gap-3 {
    gap: 12px;
}

.letter-spacing-0 {
    letter-spacing: 0 !important;
}

/* Print Dialog Layout */
.print-dialog-card {
    background: #080f1e;
    border: 1px solid rgba(56, 189, 248, 0.2);
    border-radius: 16px;
}

.print-actions {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 16px 24px;
    background: rgba(15, 23, 42, 1);
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.action-title {
    font-weight: 700;
    font-size: 16px;
    color: #e2e8f0;
}

/* Printable Area Styles */
.printable-slip {
    background: white;
    /* Important! Needs to be white for printing */
    color: black;
    padding: 40px;
    font-family: 'Vazirmatn', Arial, sans-serif;
    min-height: 500px;
}

.slip-doc-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 2px solid #1e293b;
    padding-bottom: 16px;
}

.logo-area {
    width: 80px;
}

.brand-area .company-name {
    font-size: 18px;
    font-weight: 800;
    margin: 0 0 4px;
    color: #1e293b;
}

.brand-area .doc-title {
    font-size: 15px;
    font-weight: 600;
    margin: 0 0 4px;
    color: #334155;
}

.brand-area .doc-period {
    font-size: 13px;
    color: #475569;
}

.date-area {
    width: 150px;
    font-size: 12px;
}

.date-row {
    display: flex;
    justify-content: space-between;
    margin-bottom: 4px;
}

.slip-section-title {
    font-size: 13px;
    font-weight: 800;
    background: #f1f5f9;
    padding: 6px 12px;
    border-right: 3px solid #0ea5e9;
    color: #1e293b;
}

.slip-info-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 12px;
    padding: 12px;
    border: 1px solid #e2e8f0;
    font-size: 12px;
}

.slip-info-grid .info-item {
    display: flex;
    gap: 8px;
}

.slip-tables-container {
    display: flex;
    gap: 16px;
}

.slip-half-table {
    flex: 1;
    border: 1px solid #e2e8f0;
}

.slip-table-title {
    padding: 8px 12px;
    font-weight: 800;
    font-size: 13px;
    background: #f8fafc;
    border-bottom: 1px solid #e2e8f0;
    text-align: center;
}

.detail-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 12px;
}

.detail-table th,
.detail-table td {
    padding: 6px 12px;
    border-bottom: 1px dashed #e2e8f0;
}

.detail-table th {
    background: #fcfcfc;
    color: #475569;
    font-weight: 600;
}

.table-total-row td {
    border-top: 1px solid #cbd5e1;
    background: #f8fafc;
    padding: 8px 12px;
    font-size: 13px;
}

.slip-net-summary {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border: 2px solid #0ea5e9;
    background: #f0f9ff;
    border-radius: 8px;
    padding: 12px 24px;
}

.net-text {
    font-size: 16px;
    font-weight: 800;
    color: #0c4a6e;
}

.net-amount {
    font-size: 22px;
    font-weight: 800;
    color: #0c4a6e;
}

.slip-net-words {
    color: #64748b;
}

.slip-signatures {
    display: flex;
    justify-content: space-around;
}

.sig-box {
    width: 200px;
    text-align: center;
    border-top: 1px dashed #cbd5e1;
    padding-top: 8px;
    font-size: 12px;
    color: #475569;
}

/* When actually printing */
@media print {
    body * {
        visibility: hidden;
    }

    .printable-slip,
    .printable-slip * {
        visibility: visible;
    }

    .printable-slip {
        position: absolute;
        left: 0;
        top: 0;
        width: 100%;
        margin: 0;
        padding: 0;
        background: white;
    }

    .d-print-none {
        display: none !important;
    }

    /* Remove Vuetify Dialog backdrop */
    .v-overlay__scrim {
        display: none !important;
    }
}
</style>
