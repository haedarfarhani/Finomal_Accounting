<template>
    <div class="report-page" dir="rtl">
        <!-- Header -->
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title">
                    <v-icon color="cyan" class="mr-2" size="28">mdi-chart-line</v-icon>
                    صورت سود و زیان
                </h1>
                <p class="page-subtitle">گزارش عملکرد مالی شرکت در دوره زمانی انتخابی</p>
            </div>

            <div class="header-actions">
                <div class="period-selectors">
                    <v-select v-model="period" :items="['سه ماهه اول', 'شش ماهه اول', 'نه ماهه', 'سالانه']"
                        variant="solo-filled" density="comfortable" hide-details bg-color="rgba(30, 41, 59, 0.4)"
                        class="period-select" rounded="lg" label="دوره گزارش"></v-select>
                </div>
                <v-btn color="cyan" prepend-icon="mdi-printer" elevation="2" class="action-btn text-white" rounded="lg">
                    چاپ گزارش
                </v-btn>
                <v-btn variant="outlined" color="cyan" prepend-icon="mdi-export-variant" class="action-btn bg-surface"
                    rounded="lg">
                    خروجی اکسل
                </v-btn>
            </div>
        </header>

        <!-- Main Content -->
        <v-card class="content-card mt-4" elevation="0">
            <div class="report-header">
                <div class="company-name text-h6 font-weight-bold text-cyan text-center">شرکت نمونه فینومال</div>
                <div class="report-title text-center text-body-1 mt-1">صورت سود و زیان دوره {{ period }} منتهی به اسفند
                    ۱۴۰۳</div>
                <div class="report-currency text-caption text-grey text-left">مبالغ به میلیون ریال</div>
            </div>

            <div class="table-container p-6">
                <table class="financial-table">
                    <thead>
                        <tr>
                            <th class="w-75">شرح</th>
                            <th class="text-left">مبلغ (میلیون ریال)</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Revenues -->
                        <tr>
                            <td class="indent-1">فروش ناخالص</td>
                            <td class="text-left mono-number">۱۲۰,۵۰۰</td>
                        </tr>
                        <tr>
                            <td class="indent-1 text-error">(تخفیفات و برگشت از فروش)</td>
                            <td class="text-left text-error mono-number">(۵,۰۰۰)</td>
                        </tr>
                        <tr class="subtotal-row">
                            <td class="font-weight-bold">فروش خالص</td>
                            <td class="text-left font-weight-bold mono-number text-cyan">۱۱۵,۵۰۰</td>
                        </tr>

                        <!-- COGS -->
                        <tr>
                            <td class="indent-1 mt-4 text-error">(بهای تمام شده فروش رفته)</td>
                            <td class="text-left text-error mono-number">(۶۵,۲۰۰)</td>
                        </tr>
                        <tr class="total-row">
                            <td class="font-weight-bold">سود (زیان) ناخالص</td>
                            <td class="text-left font-weight-bold mono-number text-success">۵۰,۳۰۰</td>
                        </tr>

                        <!-- Operating Expenses -->
                        <tr>
                            <td class="indent-1 mt-4 text-error">(هزینه‌های فروش، اداری و عمومی)</td>
                            <td class="text-left text-error mono-number">(۱۵,۴۰۰)</td>
                        </tr>
                        <tr class="total-row">
                            <td class="font-weight-bold">سود (زیان) عملیاتی</td>
                            <td class="text-left font-weight-bold mono-number text-success">۳۴,۹۰۰</td>
                        </tr>

                        <!-- Other Income/Expenses -->
                        <tr>
                            <td class="indent-1 mt-4">خالص سایر درآمدها و هزینه‌های غیرعملیاتی</td>
                            <td class="text-left mono-number">۲,۱۰۰</td>
                        </tr>
                        <tr>
                            <td class="indent-1 text-error">(هزینه‌های مالی)</td>
                            <td class="text-left text-error mono-number">(۱,۲۰۰)</td>
                        </tr>
                        <tr class="total-row">
                            <td class="font-weight-bold">سود (زیان) قبل از کسر مالیات</td>
                            <td class="text-left font-weight-bold mono-number text-success">۳۵,۸۰۰</td>
                        </tr>

                        <!-- Tax -->
                        <tr>
                            <td class="indent-1 mt-4 text-error">(مالیات بر درآمد)</td>
                            <td class="text-left text-error mono-number">(۸,۹۵۰)</td>
                        </tr>

                        <!-- Net Profit -->
                        <tr class="grand-total-row">
                            <td class="font-weight-bold text-h6">سود (زیان) خالص</td>
                            <td class="text-left font-weight-bold text-h6 mono-number text-cyan">۲۶,۸۵۰</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </v-card>
    </div>
</template>

<script setup>
import { ref } from 'vue';
const period = ref('سالانه');
</script>

<style scoped>
.report-page {
    font-family: 'Vazirmatn', sans-serif;
    color: #e2e8f0;
    max-width: 1200px;
    margin: 0 auto;
}

.page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.page-title {
    font-size: 24px;
    font-weight: 800;
    color: #f8fafc;
    display: flex;
    align-items: center;
}

.page-subtitle {
    font-size: 13px;
    color: #94a3b8;
    margin-top: 4px;
}

.header-actions {
    display: flex;
    align-items: center;
    gap: 12px;
}

.period-selectors {
    width: 200px;
}

.period-select :deep(.v-field) {
    border: 1px solid rgba(255, 255, 255, 0.05);
}

.content-card {
    background: rgba(15, 23, 42, 0.6);
    border: 1px solid rgba(255, 255, 255, 0.05);
    border-radius: 16px;
    padding: 24px;
}

.report-header {
    border-bottom: 2px solid rgba(34, 211, 238, 0.2);
    padding-bottom: 16px;
    margin-bottom: 24px;
}

.table-container {
    overflow-x: auto;
}

.financial-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 15px;
}

.financial-table th {
    padding: 12px;
    border-bottom: 2px solid rgba(255, 255, 255, 0.1);
    color: #94a3b8;
    font-weight: 700;
    text-align: right;
}

.financial-table td {
    padding: 12px;
    border-bottom: 1px dashed rgba(255, 255, 255, 0.05);
}

.indent-1 {
    padding-right: 24px !important;
}

.subtotal-row td {
    background: rgba(255, 255, 255, 0.02);
}

.total-row td {
    background: rgba(34, 211, 238, 0.05);
    border-bottom: 2px solid rgba(255, 255, 255, 0.1) !important;
}

.grand-total-row td {
    background: rgba(34, 211, 238, 0.1);
    border-top: 2px solid rgba(34, 211, 238, 0.5);
    border-bottom: 4px double rgba(34, 211, 238, 0.5);
    padding: 16px 12px;
}

.mono-number {
    font-family: inherit;
    letter-spacing: 1px;
}
</style>
