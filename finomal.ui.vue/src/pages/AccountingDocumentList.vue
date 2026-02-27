<template>
    <div class="accounting-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="blue" class="mr-2" size="28">mdi-format-list-bulleted</v-icon>
                    لیست اسناد حسابداری</h1>
                <p class="page-subtitle">مشاهده، جستجو و مدیریت اسناد مالی ثبت شده در سیستم</p>
            </div>
            <div class="header-actions">
                <v-btn color="blue" to="/accounting-document/create" prepend-icon="mdi-plus" elevation="2"
                    class="action-btn text-white font-weight-bold" rounded="lg">سند جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-3 border-b d-flex justify-space-between align-center">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجو شماره سند، شرح..."
                    variant="solo-filled" density="comfortable" hide-details rounded="lg"
                    bg-color="rgba(255, 255, 255, 0.03)" style="max-width: 350px;"></v-text-field>
                <div class="d-flex gap-2">
                    <v-chip color="info" variant="tonal" size="small">کل اسناد: ۳۴۵</v-chip>
                    <v-chip color="success" variant="tonal" size="small">تراز شده: ۳۴۵</v-chip>
                </div>
            </div>
            <div class="table-container">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>شماره سند</th>
                            <th>تاریخ</th>
                            <th>شرح کاربردی سند</th>
                            <th class="text-left">جمع بدهکار (ریال)</th>
                            <th class="text-left">جمع بستانکار (ریال)</th>
                            <th class="text-center">وضعیت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in documents" :key="item.id" class="table-row">
                            <td class="mono-number font-weight-bold text-blue">{{ item.docNo }}</td>
                            <td class="mono-number">{{ item.date }}</td>
                            <td class="font-weight-bold">{{ item.description }}</td>
                            <td class="text-left mono-number text-success">{{ item.debit }}</td>
                            <td class="text-left mono-number text-error">{{ item.credit }}</td>
                            <td class="text-center">
                                <v-chip size="x-small" :color="item.balanced ? 'success' : 'error'" variant="flat"
                                    class="font-weight-bold">{{ item.balanced ? 'تراز' : 'عدم تراز' }}</v-chip>
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
const documents = ref([
    { id: 1, docNo: 'ACC-1001', date: '۱۴۰۳/۱۱/۲۸', description: 'ثبت حقوق و دستمزد بهمن ماه', debit: '۲,۵۰۰,۰۰۰,۰۰۰', credit: '۲,۵۰۰,۰۰۰,۰۰۰', balanced: true },
    { id: 2, docNo: 'ACC-1002', date: '۱۴۰۳/۱۱/۲۹', description: 'خرید ملزومات اداری از تنخواه', debit: '۱۵,۰۰۰,۰۰۰', credit: '۱۵,۰۰۰,۰۰۰', balanced: true },
    { id: 3, docNo: 'ACC-1003', date: '۱۴۰۳/۱۱/۳۰', description: 'فروش محصول به فولاد مبارکه', debit: '۴۵۰,۰۰۰,۰۰۰', credit: '۴۵۰,۰۰۰,۰۰۰', balanced: true },
]);
</script>

<style scoped>
.accounting-page {
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

.gap-2 {
    gap: 8px;
}
</style>
