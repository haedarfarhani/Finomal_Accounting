<template>
    <div class="treasury-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="error" class="mr-2" size="28">mdi-cash-minus</v-icon> پرداخت وجه
                </h1>
                <p class="page-subtitle">ثبت اسناد پرداخت نقدی، چک مقطعی و حواله به تامین‌کنندگان و سایرین</p>
            </div>
            <div class="header-actions">
                <v-btn color="error" prepend-icon="mdi-plus" elevation="2" class="action-btn font-weight-bold"
                    rounded="lg">رسید پرداخت جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-3 border-b">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify"
                    placeholder="جستجوی شماره رسید، دریافت‌کننده..." variant="solo-filled" density="comfortable"
                    hide-details rounded="lg" bg-color="rgba(255, 255, 255, 0.03)"
                    style="max-width: 350px;"></v-text-field>
            </div>
            <div class="table-container">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>شماره پرداخت</th>
                            <th>تاریخ پرداخت</th>
                            <th>پرداخت کننده (صندوق/بانک)</th>
                            <th>دریافت کننده (تامین‌کننده/پرسنل)</th>
                            <th class="text-left">مبلغ پرداختی (ریال)</th>
                            <th class="text-center">نوع پرداخت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in payments" :key="item.id" class="table-row">
                            <td class="mono-number font-weight-bold text-error">{{ item.paymentNo }}</td>
                            <td class="mono-number text-grey-lighten-1">{{ item.date }}</td>
                            <td>{{ item.sourceAccount }}</td>
                            <td class="font-weight-bold">{{ item.receiver }}</td>
                            <td class="text-left mono-number text-error">{{ item.amount }}</td>
                            <td class="text-center">
                                <v-chip size="x-small" :color="getTypeColor(item.type)" variant="tonal"
                                    class="font-weight-bold">{{ item.type }}</v-chip>
                            </td>
                            <td class="text-center">
                                <v-btn icon="mdi-eye-outline" size="small" variant="text" color="info"></v-btn>
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
const payments = ref([
    { id: 1, paymentNo: 'PAY-403-51', date: '۱۴۰۳/۱۱/۲۰', sourceAccount: 'بانک سامان - سپرده', receiver: 'خدمات کامپیوتری رایان', amount: '۴۵۰,۰۰۰,۰۰۰', type: 'چک شرکتی' },
    { id: 2, paymentNo: 'PAY-403-52', date: '۱۴۰۳/۱۱/۲۸', sourceAccount: 'صندوق ریالی شرکت', receiver: 'علی رضایی - تنخواه', amount: '۱۵,۰۰۰,۰۰۰', type: 'نقد' },
]);

function getTypeColor(type) {
    if (type === 'نقد') return 'success';
    if (type === 'حواله بانکی') return 'info';
    if (type === 'چک شرکتی') return 'error';
    return 'grey';
}
</script>

<style scoped>
/* Common CSS */
.treasury-page {
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
