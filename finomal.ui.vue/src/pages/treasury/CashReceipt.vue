<template>
    <div class="treasury-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="emerald" class="mr-2" size="28">mdi-cash-plus</v-icon> دریافت وجه
                </h1>
                <p class="page-subtitle">ثبت اسناد دریافت نقدی، چک، فیش بانکی و کارتخوان از مشتریان</p>
            </div>
            <div class="header-actions">
                <v-btn color="emerald" prepend-icon="mdi-plus" elevation="2"
                    class="action-btn text-white font-weight-bold" rounded="lg">رسید دریافت جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-3 border-b">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify"
                    placeholder="جستجوی شماره رسید، پرداخت‌کننده..." variant="solo-filled" density="comfortable"
                    hide-details rounded="lg" bg-color="rgba(255, 255, 255, 0.03)"
                    style="max-width: 350px;"></v-text-field>
            </div>
            <div class="table-container">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>شماره رسید</th>
                            <th>تاریخ دریافت</th>
                            <th>دریافت کننده (صندوق/بانک)</th>
                            <th>پرداخت کننده (شخص/مشتری)</th>
                            <th class="text-left">مبلغ (ریال)</th>
                            <th class="text-center">نوع دریافت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in receipts" :key="item.id" class="table-row">
                            <td class="mono-number font-weight-bold text-emerald">{{ item.receiptNo }}</td>
                            <td class="mono-number text-grey-lighten-1">{{ item.date }}</td>
                            <td>{{ item.targetAccount }}</td>
                            <td class="font-weight-bold">{{ item.payer }}</td>
                            <td class="text-left mono-number text-success">{{ item.amount }}</td>
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
const receipts = ref([
    { id: 1, receiptNo: 'REC-403-101', date: '۱۴۰۳/۱۱/۲۸', targetAccount: 'بانک ملت - جاری مرکزی', payer: 'شرکت پتروشیمی زاگرس', amount: '۱۶۳,۵۰۰,۰۰۰', type: 'حواله بانکی' },
    { id: 2, receiptNo: 'REC-403-102', date: '۱۴۰۳/۱۱/۲۹', targetAccount: 'صندوق ریالی شرکت', payer: 'رضا کریمی', amount: '۸,۴۰۰,۰۰۰', type: 'نقد' },
]);

function getTypeColor(type) {
    if (type === 'نقد') return 'success';
    if (type === 'حواله بانکی') return 'info';
    if (type === 'چک') return 'warning';
    return 'grey';
}
</script>

<style scoped>
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
