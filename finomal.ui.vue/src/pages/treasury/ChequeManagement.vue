<template>
    <div class="treasury-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="emerald" class="mr-2"
                        size="28">mdi-ticket-confirmation-outline</v-icon> مدیریت چک‌ها</h1>
                <p class="page-subtitle">پیگیری چک‌های دریافتی و پرداختی سازمان، واگذاری به بانک، وصول و برگشت چک</p>
            </div>
        </header>

        <!-- Stat Cards -->
        <v-row class="mt-4 mb-2">
            <v-col cols="12" md="3"><v-card class="stat-card" color="rgba(16, 185, 129, 0.1)" elevation="0">
                    <div class="text-emerald">اسناد در جریان: ۴</div>
                </v-card></v-col>
            <v-col cols="12" md="3"><v-card class="stat-card" color="rgba(59, 130, 246, 0.1)" elevation="0">
                    <div class="text-blue">واگذار شده به بانک: ۲</div>
                </v-card></v-col>
            <v-col cols="12" md="3"><v-card class="stat-card" color="rgba(239, 68, 68, 0.1)" elevation="0">
                    <div class="text-error">برگشت خورده: ۱</div>
                </v-card></v-col>
        </v-row>

        <!-- Tabs inside the card -->
        <v-card class="content-card mt-4" elevation="0">
            <v-tabs v-model="tab" color="emerald" align-tabs="start" class="border-b" bg-color="transparent">
                <v-tab value="received" class="font-weight-bold">چک‌های دریافتی (اسناد دریافتنی)</v-tab>
                <v-tab value="paid" class="font-weight-bold text-grey">چک‌های پرداختی (اسناد پرداختنی)</v-tab>
            </v-tabs>

            <v-window v-model="tab">
                <v-window-item value="received">
                    <div class="table-container p-4">
                        <table class="custom-table w-100 mt-2">
                            <thead>
                                <tr>
                                    <th>شماره چک</th>
                                    <th>تاریخ سررسید</th>
                                    <th>صادرکننده</th>
                                    <th>بانک صادرکننده</th>
                                    <th class="text-left">مبلغ (ریال)</th>
                                    <th class="text-center">وضعیت فعلی</th>
                                    <th class="text-center">تغییر وضعیت</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="item in receivedCheques" :key="item.id" class="table-row">
                                    <td class="mono-number font-weight-bold text-emerald">{{ item.chequeNo }}</td>
                                    <td class="mono-number"
                                        :class="{ 'text-error font-weight-bold': item.status === 'برگشت خورده' }">{{
                                        item.dueDate }}</td>
                                    <td class="font-weight-bold">{{ item.issuer }}</td>
                                    <td class="text-grey-lighten-1">{{ item.bank }}</td>
                                    <td class="text-left mono-number">{{ item.amount }}</td>
                                    <td class="text-center">
                                        <v-chip size="x-small" :color="getStatusColor(item.status)" variant="flat"
                                            class="font-weight-bold">{{ item.status }}</v-chip>
                                    </td>
                                    <td class="text-center">
                                        <v-btn size="small" variant="text" color="info" class="text-caption">عملیات
                                            <v-icon right>mdi-menu-down</v-icon></v-btn>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </v-window-item>

                <v-window-item value="paid">
                    <div class="p-6 text-center text-grey">لیست چک‌های پرداختی در اینجا نمایش داده می‌شود.</div>
                </v-window-item>
            </v-window>
        </v-card>
    </div>
</template>

<script setup>
import { ref } from 'vue';
const tab = ref('received');

const receivedCheques = ref([
    { id: 1, chequeNo: '۶۵۸۹۱۳', dueDate: '۱۴۰۳/۱۲/۰۵', issuer: 'شرکت پتروشیمی زاگرس', bank: 'بانک تجارت شعبه طالقانی', amount: '۱۵۰,۰۰۰,۰۰۰', status: 'موجود در صندوق' },
    { id: 2, chequeNo: '۴۵۲۱۸۸', dueDate: '۱۴۰۳/۱۱/۲۸', issuer: 'فولاد مبارکه', bank: 'بانک ملت شعبه مرکزی', amount: '۴۵,۰۰۰,۰۰۰', status: 'واگذار شده به بانک' },
    { id: 3, chequeNo: '۹۹۸۵۱۲', dueDate: '۱۴۰۳/۱۱/۲۰', issuer: 'رضا کریمی', bank: 'بانک ملی شعبه بازار', amount: '۸,۴۰۰,۰۰۰', status: 'وصول شده' },
    { id: 4, chequeNo: '۱۱۳۳۵۵', dueDate: '۱۴۰۳/۱۱/۱۵', issuer: 'فروشگاه دیجی‌کالا', bank: 'بانک پاسارگاد', amount: '۱۲۰,۰۰۰,۰۰۰', status: 'برگشت خورده' },
]);

function getStatusColor(status) {
    if (status === 'موجود در صندوق') return 'warning';
    if (status === 'واگذار شده به بانک') return 'info';
    if (status === 'وصول شده') return 'success';
    if (status === 'برگشت خورده') return 'error';
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

.stat-card {
    padding: 16px;
    border-radius: 12px;
    font-weight: 700;
    display: flex;
    align-items: center;
    font-size: 15px;
    border: 1px solid rgba(255, 255, 255, 0.05);
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

:deep(.v-tab--selected) {
    color: #34d399 !important;
}
</style>
