<template>
    <div class="inventory-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="amber" class="mr-2" size="28">mdi-warehouse</v-icon> تعریف انبارها
                </h1>
                <p class="page-subtitle">ایجاد و مدیریت انبارهای فیزیکی و مجازی سازمان</p>
            </div>
            <div class="header-actions">
                <v-btn color="amber" prepend-icon="mdi-plus" elevation="2"
                    class="action-btn text-black font-weight-bold" rounded="lg">انبار جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-3 pb-0">
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجو در انبارها..."
                    variant="solo-filled" density="comfortable" hide-details rounded="lg"
                    bg-color="rgba(255, 255, 255, 0.03)" style="max-width: 300px;"></v-text-field>
            </div>
            <div class="p-4">
                <table class="custom-table w-100 mt-4">
                    <thead>
                        <tr>
                            <th>کد انبار</th>
                            <th>نام انبار</th>
                            <th>نوع انبار</th>
                            <th>مسئول انبار</th>
                            <th>آدرس</th>
                            <th>وضعیت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in warehouses" :key="item.id" class="table-row">
                            <td class="mono-number text-amber">{{ item.code }}</td>
                            <td class="font-weight-bold">{{ item.name }}</td>
                            <td>{{ item.type }}</td>
                            <td>{{ item.manager }}</td>
                            <td class="text-caption text-grey-lighten-1">{{ item.address }}</td>
                            <td><v-chip size="small" :color="item.active ? 'success' : 'grey'" variant="tonal">{{
                                    item.active ? 'فعال' : 'غیرفعال' }}</v-chip></td>
                            <td class="text-center">
                                <v-btn icon="mdi-pencil-outline" size="small" variant="text" color="info"></v-btn>
                                <v-btn icon="mdi-trash-can-outline" size="small" variant="text" color="error"></v-btn>
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
const warehouses = ref([
    { id: 1, code: 'WH-01', name: 'انبار مرکزی (مواد اولیه)', type: 'اصلی', manager: 'محمد اکبری', address: 'تهران، جاده مخصوص', active: true },
    { id: 2, code: 'WH-02', name: 'انبار محصول نهایی', type: 'اصلی', manager: 'علی رضایی', address: 'تهران، شهرک صنعتی', active: true },
    { id: 3, code: 'WH-03', name: 'انبار ضایعات', type: 'مجازی/اسقاط', manager: 'حسین کریمی', address: '-', active: true },
]);
</script>

<style scoped>
.inventory-page {
    font-family: 'Vazirmatn', sans-serif;
    color: #e2e8f0;
    max-width: 1200px;
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
    padding-bottom: 16px;
}

.custom-table {
    border-collapse: collapse;
    text-align: right;
}

.custom-table th {
    padding: 12px 16px;
    border-bottom: 2px solid rgba(255, 255, 255, 0.05);
    color: #94a3b8;
    font-size: 13px;
}

.custom-table td {
    padding: 12px 16px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.02);
    font-size: 13.5px;
}

.table-row:hover td {
    background: rgba(255, 255, 255, 0.02);
}

.mono-number {
    font-family: inherit;
    letter-spacing: 0.5px;
}
</style>
