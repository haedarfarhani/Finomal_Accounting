<template>
    <div class="purchase-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="orange" class="mr-2" size="28">mdi-receipt-text-outline</v-icon>
                    فاکتور خرید</h1>
                <p class="page-subtitle">ثبت صورت حساب خریدهای انجام شده سازمان</p>
            </div>
            <div class="header-actions">
                <v-btn color="orange" prepend-icon="mdi-plus" elevation="2"
                    class="action-btn text-black font-weight-bold" rounded="lg">ثبت خرید جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-3 border-b">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجو شماره فاکتور..."
                    variant="solo-filled" density="comfortable" hide-details rounded="lg"
                    bg-color="rgba(255, 255, 255, 0.03)" style="max-width: 350px;"></v-text-field>
            </div>
            <div class="table-container">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>شماره سند خرید</th>
                            <th>شماره فاکتور فروشنده</th>
                            <th>تاریخ</th>
                            <th>تامین‌کننده</th>
                            <th class="text-left">مبلغ کل (ریال)</th>
                            <th class="text-left">بدهی (مانده)</th>
                            <th class="text-center">وضعیت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in invoices" :key="item.id" class="table-row">
                            <td class="mono-number font-weight-bold text-orange">{{ item.docNo }}</td>
                            <td class="mono-number text-grey">{{ item.refNo }}</td>
                            <td class="mono-number">{{ item.date }}</td>
                            <td class="font-weight-bold">{{ item.supplier }}</td>
                            <td class="text-left mono-number text-success">{{ item.totalAmount }}</td>
                            <td class="text-left mono-number text-error">{{ item.balance }}</td>
                            <td class="text-center">
                                <v-chip size="x-small" :color="item.settled ? 'success' : 'warning'" variant="flat"
                                    class="font-weight-bold">{{ item.settled ? 'تسویه شده' : 'بدهکار' }}</v-chip>
                            </td>
                            <td class="text-center">
                                <v-btn icon="mdi-cash-minus" size="small" variant="text" color="error"
                                    title="ثبت پرداخت وجه"></v-btn>
                                <v-btn icon="mdi-printer-outline" size="small" variant="text" color="grey"></v-btn>
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
    { id: 1, docNo: 'PUR-403-1001', refNo: '489512', date: '۱۴۰۳/۱۱/۱۵', supplier: 'خدمات کامپیوتری رایان', totalAmount: '۴۵۰,۰۰۰,۰۰۰', balance: '۰', settled: true },
    { id: 2, docNo: 'PUR-403-1002', refNo: '1004', date: '۱۴۰۳/۱۱/۲۸', supplier: 'شرکت توزیع کاغذ آریا', totalAmount: '۲۵,۰۰۰,۰۰۰', balance: '۲۵,۰۰۰,۰۰۰', settled: false },
]);
</script>

<style scoped>
.purchase-page {
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
