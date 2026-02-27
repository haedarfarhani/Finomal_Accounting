<template>
    <div class="sales-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="violet" class="mr-2" size="28">mdi-account-group</v-icon> تعریف
                    مشتریان</h1>
                <p class="page-subtitle">مدیریت اطلاعات مشتریان، اشخاص حقیقی و حقوقی سازمان</p>
            </div>
            <div class="header-actions">
                <v-btn color="violet" prepend-icon="mdi-account-plus" elevation="2"
                    class="action-btn text-white font-weight-bold" rounded="lg">مشتری جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-3 d-flex justify-space-between align-center border-b">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify"
                    placeholder="جستجوی نام، کد اقتصادی یا موبایل..." variant="solo-filled" density="comfortable"
                    hide-details rounded="lg" bg-color="rgba(255, 255, 255, 0.03)"
                    style="max-width: 350px;"></v-text-field>
            </div>
            <div class="table-container">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>کد مشتری</th>
                            <th>نام مشتری</th>
                            <th>نوع شخص</th>
                            <th>شماره تماس</th>
                            <th>کد ملی / شناسه ملی</th>
                            <th>گروه</th>
                            <th class="text-center">وضعیت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in customers" :key="item.id" class="table-row">
                            <td class="mono-number text-violet">{{ item.code }}</td>
                            <td class="font-weight-bold">{{ item.name }}</td>
                            <td><v-chip size="x-small" variant="outlined" color="grey-lighten-1">{{ item.type
                                    }}</v-chip></td>
                            <td class="mono-number text-grey-lighten-1">{{ item.phone }}</td>
                            <td class="mono-number text-grey-lighten-1">{{ item.nationalCode }}</td>
                            <td class="text-grey-lighten-1">{{ item.group }}</td>
                            <td class="text-center"><v-icon :color="item.active ? 'success' : 'grey'" size="18">{{
                                item.active ? 'mdi-check-circle' : 'mdi-close-circle' }}</v-icon></td>
                            <td class="text-center">
                                <v-btn icon="mdi-pencil-outline" size="small" variant="text" color="info"></v-btn>
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
const customers = ref([
    { id: 1, code: 'CUST-1001', name: 'شرکت پتروشیمی زاگرس', type: 'حقوقی', phone: '021-88123456', nationalCode: '10101234567', group: 'مشتریان VIP', active: true },
    { id: 2, code: 'CUST-1002', name: 'فولاد مبارکه اصفهان', type: 'حقوقی', phone: '031-33123456', nationalCode: '10260123456', group: 'مشتریان VIP', active: true },
    { id: 3, code: 'CUST-1003', name: 'رضا کریمی', type: 'حقیقی', phone: '09121234567', nationalCode: '0071234567', group: 'مشتریان عادی', active: false },
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
