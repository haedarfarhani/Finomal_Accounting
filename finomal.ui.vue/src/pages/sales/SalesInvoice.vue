<template>
    <div class="sales-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="violet-lighten-1" class="mr-2"
                        size="28">mdi-receipt-text-check</v-icon> فاکتور فروش</h1>
                <p class="page-subtitle">صدور فاکتور رسمی قطعی، ثبت فروش و کنترل تسویه‌حساب مشتریان</p>
            </div>
            <div class="header-actions">
                <v-btn color="violet" prepend-icon="mdi-plus" elevation="2"
                    class="action-btn text-white font-weight-bold" rounded="lg">صدور فاکتور فروش</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-3 d-flex justify-space-between align-center border-b">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify"
                    placeholder="جستجو شماره فاکتور، مشتری..." variant="solo-filled" density="comfortable" hide-details
                    rounded="lg" bg-color="rgba(255, 255, 255, 0.03)" style="max-width: 350px;"></v-text-field>
                <div class="summary text-caption">
                    <v-chip class="mr-2" color="success" variant="outlined" size="small">تسویه شده: ۷۴</v-chip>
                    <v-chip color="error" variant="outlined" size="small">بدهکار: ۱۲</v-chip>
                </div>
            </div>
            <div class="table-container">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>شماره سریال</th>
                            <th>تاریخ صدور</th>
                            <th>مشتری</th>
                            <th class="text-left">مبلغ کل (ریال)</th>
                            <th class="text-left">مانده (بدهی)</th>
                            <th class="text-center">وضعیت تسویه</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in invoices" :key="item.id" class="table-row">
                            <td class="mono-number font-weight-bold text-violet">{{ item.serialNo }}</td>
                            <td class="mono-number">{{ item.date }}</td>
                            <td class="font-weight-bold">{{ item.customer }}</td>
                            <td class="text-left mono-number text-success">{{ item.totalAmount }}</td>
                            <td class="text-left mono-number text-error">{{ item.balance }}</td>
                            <td class="text-center">
                                <v-chip size="x-small" :color="item.settled ? 'success' : 'error'" variant="flat"
                                    class="font-weight-bold">{{ item.settled ? 'تسویه شده' : 'بدهکار' }}</v-chip>
                            </td>
                            <td class="text-center">
                                <v-btn icon="mdi-cash-plus" size="small" variant="text" color="success"
                                    title="ثبت دریافت وجه"></v-btn>
                                <v-btn icon="mdi-printer-outline" size="small" variant="text" color="info"></v-btn>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </v-card>
    </div>
</template>

<script setup>
import { ref } from 'vue';
const search = ref('');
const invoices = ref([
    { id: 1, serialNo: 'INV-403-5100', date: '۱۴۰۳/۱۱/۲۰', customer: 'شرکت پتروشیمی زاگرس', totalAmount: '۱۶۳,۵۰۰,۰۰۰', balance: '۰', settled: true },
    { id: 2, serialNo: 'INV-403-5101', date: '۱۴۰۳/۱۱/۲۸', customer: 'فولاد مبارکه اصفهان', totalAmount: '۸۵,۰۰۰,۰۰۰', balance: '۳۵,۰۰۰,۰۰۰', settled: false },
]);
</script>

<style scoped>
.sales-page {
    font-family: 'Vazirmatn', sans-serif;
    color: #e2e8f0;
    max-width: 1400px;
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

.content-card {
    background: rgba(15, 23, 42, 0.6);
    border: 1px solid rgba(255, 255, 255, 0.05);
    border-radius: 16px;
    overflow: hidden;
}

.border-b {
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}

.custom-table {
    border-collapse: collapse;
    text-align: right;
}

.custom-table th {
    padding: 14px 16px;
    border-bottom: 2px solid rgba(255, 255, 255, 0.05);
    color: #94a3b8;
    font-size: 12px;
}

.custom-table td {
    padding: 12px 16px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.02);
    font-size: 13px;
    vertical-align: middle;
}

.table-row:hover td {
    background: rgba(255, 255, 255, 0.02);
}

.mono-number {
    font-family: inherit;
    letter-spacing: 0.5px;
}
</style>
