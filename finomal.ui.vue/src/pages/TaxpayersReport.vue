<template>
    <div class="report-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="cyan" class="mr-2" size="28">mdi-shield-check-outline</v-icon>
                    گزارش سامانه مودیان</h1>
                <p class="page-subtitle">مشاهده لیست فاکتورها و صورت‌حساب‌های الکترونیکی ارسال شده به سامانه مالیاتی</p>
            </div>
            <div class="header-actions">
                <v-btn color="cyan" prepend-icon="mdi-cloud-upload" elevation="2" rounded="lg">ارسال گروهی (موارد
                    معلق)</v-btn>
            </div>
        </header>

        <v-row class="mt-4 mb-2">
            <v-col cols="12" md="4"><v-card class="stat-card" color="rgba(34,197,94,0.1)" elevation="0">
                    <div class="text-success"><v-icon>mdi-check-circle</v-icon> ارسال موفق: ۱۵۲</div>
                </v-card></v-col>
            <v-col cols="12" md="4"><v-card class="stat-card border-warning text-warning" color="rgba(245,158,11,0.1)"
                    elevation="0">
                    <div><v-icon>mdi-clock-outline</v-icon> در حال انتظار: ۸</div>
                </v-card></v-col>
            <v-col cols="12" md="4"><v-card class="stat-card text-error" color="rgba(239,68,68,0.1)" elevation="0">
                    <div><v-icon>mdi-alert-circle</v-icon> خطادار: ۲</div>
                </v-card></v-col>
        </v-row>

        <v-card class="content-card mt-4" elevation="0">
            <v-table theme="dark" class="bg-transparent custom-table">
                <thead>
                    <tr>
                        <th>شماره فاکتور</th>
                        <th>مشتری</th>
                        <th>تاریخ صدور</th>
                        <th>مبلغ (ریال)</th>
                        <th>مالیات (ریال)</th>
                        <th class="text-center">وضعیت مودیان</th>
                        <th>شماره منحصر بفرد مالیاتی</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in invoices" :key="item.id">
                        <td class="font-weight-bold text-cyan">{{ item.id }}</td>
                        <td>{{ item.customer }}</td>
                        <td>{{ item.date }}</td>
                        <td class="mono-number">{{ item.amount }}</td>
                        <td class="mono-number text-grey-lighten-1">{{ item.tax }}</td>
                        <td class="text-center">
                            <v-chip size="small" :color="item.statusColor" variant="tonal">{{ item.status }}</v-chip>
                        </td>
                        <td class="mono-number text-caption text-grey">{{ item.taxId }}</td>
                    </tr>
                </tbody>
            </v-table>
        </v-card>
    </div>
</template>

<script setup>
import { ref } from 'vue';

const invoices = ref([
    { id: 'INV-403-1001', customer: 'شرکت پتروشیمی زاگرس', date: '۱۴۰۳/۱۱/۲۸', amount: '۱۵۰,۰۰۰,۰۰۰', tax: '۱۳,۵۰۰,۰۰۰', status: 'ارسال موفق', statusColor: 'success', taxId: 'D12E34F5G6...987' },
    { id: 'INV-403-1002', customer: 'فولاد مبارکه', date: '۱۴۰۳/۱۱/۲۸', amount: '۸۵,۰۰۰,۰۰۰', tax: '۷,۶۵۰,۰۰۰', status: 'ارسال موفق', statusColor: 'success', taxId: 'A87B65C4D3...210' },
    { id: 'INV-403-1003', customer: 'ارتباطات سیار ایران', date: '۱۴۰۳/۱۱/۲۹', amount: '۱۲,۵۰۰,۰۰۰', tax: '۱,۱۲۵,۰۰۰', status: 'در انتظار ارسال', statusColor: 'warning', taxId: '---' },
    { id: 'INV-403-1004', customer: 'فروشگاه دیجی‌کالا', date: '۱۴۰۳/۱۱/۲۹', amount: '۴۳,۲۰۰,۰۰۰', tax: '۳,۸۸۸,۰۰۰', status: 'خطا در اطلاعات خریدار', statusColor: 'error', taxId: '---' },
]);
</script>

<style scoped>
.report-page {
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
    padding: 12px;
}

.stat-card {
    padding: 16px;
    border-radius: 12px;
    font-weight: 700;
    display: flex;
    align-items: center;
    font-size: 16px;
    border: 1px solid rgba(255, 255, 255, 0.05);
}

.custom-table th {
    font-weight: 700 !important;
    color: #94a3b8 !important;
}

.custom-table td {
    padding: 12px 16px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.05) !important;
    font-size: 13px;
}

.mono-number {
    font-family: inherit;
    letter-spacing: 0.5px;
}
</style>
