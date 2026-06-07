<template>
    <div class="sales-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="violet" class="mr-2" size="28">mdi-keyboard-return</v-icon> برگشت
                    از فروش</h1>
                <p class="page-subtitle">ثبت کالاها و خدمات مرجوعی از سوی مشتریان</p>
            </div>
            <div class="header-actions">
                <v-btn color="violet" prepend-icon="mdi-plus" elevation="2"
                    class="action-btn text-white font-weight-bold" rounded="lg">ثبت برگشتی جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-3 border-b">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify"
                    placeholder="جستجوی شماره ارجاع یا مشتری..." variant="solo-filled" density="comfortable"
                    hide-details rounded="lg" bg-color="rgba(255, 255, 255, 0.03)"
                    style="max-width: 350px;"></v-text-field>
            </div>
            <div class="table-container">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>شماره برگشتی</th>
                            <th>شماره فاکتور مبدا</th>
                            <th>تاریخ</th>
                            <th>مشتری</th>
                            <th class="text-left">مبلغ برگشتی (ریال)</th>
                            <th class="text-center">وضعیت انبار</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in returns" :key="item.id" class="table-row">
                            <td class="mono-number font-weight-bold text-violet">{{ item.returnNo }}</td>
                            <td class="mono-number text-grey">{{ item.refInvoice }}</td>
                            <td class="mono-number">{{ item.date }}</td>
                            <td class="font-weight-bold">{{ item.customer }}</td>
                            <td class="text-left mono-number text-error">{{ item.amount }}</td>
                            <td class="text-center">
                                <v-chip size="x-small" :color="item.stockStatus === 'رسید شده' ? 'success' : 'warning'"
                                    variant="flat" class="font-weight-bold">{{ item.stockStatus }}</v-chip>
                            </td>
                            <td class="text-center">
                                <v-btn icon="mdi-eye-outline" size="small" variant="text" color="info"></v-btn>
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
const returns = ref([
    { id: 1, returnNo: 'RET-403-100', refInvoice: 'INV-403-5100', date: '۱۴۰۳/۱۱/۲۵', customer: 'شرکت پتروشیمی زاگرس', amount: '۱۵,۰۰۰,۰۰۰', stockStatus: 'رسید شده' },
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
