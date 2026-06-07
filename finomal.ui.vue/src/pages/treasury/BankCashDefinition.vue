<template>
    <div class="treasury-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="emerald" class="mr-2" size="28">mdi-bank-outline</v-icon> تعریف
                    صندوق و حساب‌های بانکی</h1>
                <p class="page-subtitle">مدیریت حساب‌های بانکی سازمان، صندوق‌ها و دستگاه‌های کارتخوان (POS)</p>
            </div>
            <div class="header-actions">
                <v-btn color="emerald" prepend-icon="mdi-plus" elevation="2"
                    class="action-btn text-white font-weight-bold" rounded="lg">حساب/صندوق جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-container p-4">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify"
                    placeholder="جستجو نام بانک، شماره حساب..." variant="solo-filled" density="comfortable" hide-details
                    rounded="lg" bg-color="rgba(255, 255, 255, 0.03)" class="mb-4"
                    style="max-width: 400px;"></v-text-field>

                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>عنوان حساب / صندوق</th>
                            <th>نوع حساب</th>
                            <th>شماره حساب / شبا</th>
                            <th class="text-left">موجودی (ریال)</th>
                            <th class="text-center">وضعیت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in bankAccounts" :key="item.id" class="table-row">
                            <td class="font-weight-bold text-emerald">{{ item.name }}</td>
                            <td><v-chip size="x-small" variant="outlined" color="grey">{{ item.type }}</v-chip></td>
                            <td class="mono-number text-grey-lighten-1">{{ item.accountNo }}</td>
                            <td class="text-left mono-number font-weight-bold">{{ item.balance }}</td>
                            <td class="text-center">
                                <v-icon :color="item.active ? 'success' : 'grey'" size="18">{{ item.active ?
                                    'mdi-check-circle' : 'mdi-close-circle' }}</v-icon>
                            </td>
                            <td class="text-center">
                                <v-btn icon="mdi-history" size="small" variant="text" color="info"
                                    title="مشاهده گردش"></v-btn>
                                <v-btn icon="mdi-pencil-outline" size="small" variant="text" color="grey"></v-btn>
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
const bankAccounts = ref([
    { id: 1, name: 'بانک ملت - جاری مرکزی', type: 'حساب جاری', accountNo: '۴۵۶۲۱۸۹۳۲۰', balance: '۱,۴۵۰,۰۰۰,۰۰۰', active: true },
    { id: 2, name: 'بانک سامان - سپرده', type: 'حساب سپرده', accountNo: 'IR150560000000000123456789', balance: '۵,۰۰۰,۰۰۰,۰۰۰', active: true },
    { id: 3, name: 'صندوق ریالی شرکت', type: 'صندوق', accountNo: '-', balance: '۴۵,۰۰۰,۰۰۰', active: true },
]);
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
