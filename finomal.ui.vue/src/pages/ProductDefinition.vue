<template>
    <div class="inventory-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="amber" class="mr-2" size="28">mdi-package-variant-closed</v-icon>
                    تعریف کالا / خدمات</h1>
                <p class="page-subtitle">مدیریت اطلاعات پایه، واحد سنجش، و گروه‌بندی کالاها</p>
            </div>
            <div class="header-actions">
                <v-btn color="amber" prepend-icon="mdi-plus" elevation="2"
                    class="action-btn text-black font-weight-bold" rounded="lg">کالای جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-4 d-flex justify-space-between align-center border-b">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify"
                    placeholder="جستجوی کالا (نام/کد/بارکد)..." variant="solo-filled" density="comfortable" hide-details
                    rounded="lg" bg-color="rgba(255, 255, 255, 0.03)" style="width: 350px;"></v-text-field>
                <div class="d-flex gap-2">
                    <v-btn variant="tonal" color="amber" rounded="lg" prepend-icon="mdi-filter-outline">فیلتر
                        گروه</v-btn>
                </div>
            </div>
            <div class="table-container p-0">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th class="pl-0 text-center" style="width: 50px;">
                                <v-icon color="grey">mdi-image-outline</v-icon>
                            </th>
                            <th>کد کالا</th>
                            <th>نام کالا</th>
                            <th>گروه کالا</th>
                            <th>واحد سنجش</th>
                            <th>نوع</th>
                            <th class="text-left">قیمت پایه (ریال)</th>
                            <th class="text-center">وضعیت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in products" :key="item.id" class="table-row">
                            <td class="text-center px-0">
                                <v-avatar size="32" color="rgba(255,255,255,0.05)" rounded="lg">
                                    <v-icon size="16" color="amber">mdi-package</v-icon>
                                </v-avatar>
                            </td>
                            <td class="mono-number text-grey-lighten-1">{{ item.code }}</td>
                            <td class="font-weight-bold">{{ item.name }}</td>
                            <td><v-chip size="x-small" variant="outlined" color="grey-lighten-1">{{ item.group
                                    }}</v-chip></td>
                            <td>{{ item.unit }}</td>
                            <td>{{ item.type }}</td>
                            <td class="text-left mono-number">{{ item.price }}</td>
                            <td class="text-center"><v-icon :color="item.active ? 'success' : 'grey'" size="18">{{
                                item.active ? 'mdi-check-circle' : 'mdi-close-circle' }}</v-icon></td>
                            <td class="text-center">
                                <v-btn icon="mdi-pencil-outline" size="small" variant="text" color="info"></v-btn>
                                <v-btn icon="mdi-dots-vertical" size="small" variant="text" color="grey"></v-btn>
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
const products = ref([
    { id: 1, code: 'ITM-1001', name: 'لپ‌تاپ لنوو V15', group: 'تجهیزات IT', unit: 'دستگاه', type: 'کالا', price: '۲۵۰,۰۰۰,۰۰۰', active: true },
    { id: 2, code: 'ITM-1002', name: 'کابل شبکه CAT6 - حلقه', group: 'تجهیزات شبکه', unit: 'متر', type: 'کالا', price: '۱۲۰,۰۰۰', active: true },
    { id: 3, code: 'ITM-1003', name: 'پشتیبانی نرم‌افزار (ماهانه)', group: 'خدمات نرم‌افزاری', unit: 'مورد', type: 'خدمات', price: '۵۰,۰۰۰,۰۰۰', active: true },
    { id: 4, code: 'ITM-1004', name: 'سرور HP DL380 G10', group: 'سرور و تجهیزات', unit: 'دستگاه', type: 'کالا', price: '۲,۵۰۰,۰۰۰,۰۰۰', active: false },
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
    white-space: nowrap;
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
