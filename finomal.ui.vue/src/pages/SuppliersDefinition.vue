<template>
    <div class="purchase-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="orange" class="mr-2" size="28">mdi-account-tie</v-icon> تعریف
                    تامین‌کنندگان</h1>
                <p class="page-subtitle">مدیریت فروشندگان کالا و ارائه‌دهندگان خدمات سازمان</p>
            </div>
            <div class="header-actions">
                <v-btn color="orange" prepend-icon="mdi-account-plus" elevation="2"
                    class="action-btn text-black font-weight-bold" rounded="lg">تامین‌کننده جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-3 border-b">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجوی نام یا تلفن..."
                    variant="solo-filled" density="comfortable" hide-details rounded="lg"
                    bg-color="rgba(255, 255, 255, 0.03)" style="max-width: 350px;"></v-text-field>
            </div>
            <div class="table-container">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>کد</th>
                            <th>نام تامین‌کننده</th>
                            <th>تلفن تماس</th>
                            <th>کد اقتصادی</th>
                            <th>دسته‌بندی (گروه)</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in suppliers" :key="item.id" class="table-row">
                            <td class="mono-number text-orange">{{ item.code }}</td>
                            <td class="font-weight-bold">{{ item.name }}</td>
                            <td class="mono-number text-grey-lighten-1">{{ item.phone }}</td>
                            <td class="mono-number text-grey-lighten-1">{{ item.nationalCode }}</td>
                            <td><v-chip size="x-small" variant="outlined" color="grey">{{ item.group }}</v-chip></td>
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
const suppliers = ref([
    { id: 1, code: 'VEN-2001', name: 'خدمات کامپیوتری رایان', phone: '021-66554433', nationalCode: '10101239988', group: 'تجهیزات سخت‌افزار' },
    { id: 2, code: 'VEN-2002', name: 'شرکت توزیع کاغذ آریا', phone: '021-88998877', nationalCode: '10260444333', group: 'ملزومات اداری' },
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
