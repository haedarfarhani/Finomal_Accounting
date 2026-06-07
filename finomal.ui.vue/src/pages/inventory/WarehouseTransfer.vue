<template>
    <div class="inventory-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="amber" class="mr-2" size="28">mdi-swap-horizontal</v-icon> انتقال
                    بین انبارها</h1>
                <p class="page-subtitle">جابجایی کالا بین انبارهای مختلف سازمان</p>
            </div>
            <div class="header-actions">
                <v-btn color="amber" prepend-icon="mdi-plus" elevation="2"
                    class="action-btn text-black font-weight-bold" rounded="lg">ثبت انتقال جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-container p-4">
                <table class="custom-table w-100 mt-2">
                    <thead>
                        <tr>
                            <th>شماره انتقال</th>
                            <th>تاریخ</th>
                            <th>از انبار (مبدا)</th>
                            <th class="text-center px-0"><v-icon color="grey">mdi-arrow-left</v-icon></th>
                            <th>به انبار (مقصد)</th>
                            <th>ثبت کننده</th>
                            <th class="text-center">وضعیت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in transfers" :key="item.id" class="table-row">
                            <td class="mono-number font-weight-bold text-amber">{{ item.transferNo }}</td>
                            <td class="mono-number">{{ item.date }}</td>
                            <td class="text-grey-lighten-1">{{ item.from }}</td>
                            <td class="text-center px-0"><v-icon color="info" size="20">mdi-chevron-double-left</v-icon>
                            </td>
                            <td class="text-grey-lighten-1">{{ item.to }}</td>
                            <td class="text-caption">{{ item.user }}</td>
                            <td class="text-center">
                                <v-chip size="x-small" :color="item.status === 'تایید شده' ? 'success' : 'warning'"
                                    variant="flat" class="font-weight-bold">{{ item.status }}</v-chip>
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
const transfers = ref([
    { id: 1, transferNo: 'TRN-403-010', date: '۱۴۰۳/۱۱/۲۰', from: 'انبار مرکزی', to: 'انبار محصول نهایی', user: 'محمد اکبری', status: 'تایید شده' },
    { id: 2, transferNo: 'TRN-403-011', date: '۱۴۰۳/۱۱/۲۸', from: 'انبار محصول نهایی', to: 'انبار ضایعات', user: 'علی رضایی', status: 'در انتظار رسید مقصد' },
]);
</script>

<style scoped>
.inventory-page {
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
