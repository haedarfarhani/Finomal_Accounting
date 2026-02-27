<template>
    <div class="inventory-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="amber" class="mr-2" size="28">mdi-boxes</v-icon> موجودی لحظه‌ای
                    کالا در انبار</h1>
                <p class="page-subtitle">مشاهده لیست موجودی کالاها در تمامی انبارها به تفکیک یا سرجمع</p>
            </div>
            <div class="header-actions">
                <!-- Date simulation selector -->
                <v-btn variant="outlined" color="amber" class="bg-surface" rounded="lg"
                    prepend-icon="mdi-export-variant">خروجی اکسل</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="filters px-4 py-4 d-flex justify-space-between border-b align-center">
                <div class="d-flex gap-4" style="width: 500px">
                    <v-select v-model="selectedWarehouse"
                        :items="['نمایش مجموع همه‌ انبارها', 'انبار مرکزی', 'انبار ضایعات']" variant="solo-filled"
                        density="comfortable" hide-details bg-color="rgba(30, 41, 59, 1)"
                        prepend-inner-icon="mdi-warehouse" class="flex-grow-1" rounded="lg"></v-select>
                </div>
                <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجوی نام، کد..."
                    variant="solo-filled" density="comfortable" hide-details bg-color="rgba(255,255,255,0.03)"
                    style="width: 250px;" rounded="lg"></v-text-field>
            </div>

            <div class="table-container p-0">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>کد کالا</th>
                            <th>نام کالا</th>
                            <th>گروه کالا</th>
                            <th>واحد</th>
                            <th class="text-left">موجودی اولیه دوره</th>
                            <th class="text-left text-success">جمع وارده</th>
                            <th class="text-left text-error">جمع صادره</th>
                            <th class="text-left d-flex align-center justify-end font-weight-bold text-amber">
                                موجودی فعلی (در دسترس)
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in stockList" :key="item.code" class="table-row">
                            <td class="mono-number text-grey-lighten-1">{{ item.code }}</td>
                            <td class="font-weight-bold">{{ item.name }}</td>
                            <td><v-chip size="x-small" variant="outlined" color="grey-lighten-1">{{ item.group
                                    }}</v-chip></td>
                            <td class="text-caption text-grey">{{ item.unit }}</td>
                            <td class="text-left mono-number">{{ item.initBalance }}</td>
                            <td class="text-left text-success mono-number">{{ item.totalIn }}</td>
                            <td class="text-left text-error mono-number">{{ item.totalOut }}</td>
                            <td class="text-left font-weight-black text-amber mono-number">{{ item.currentBalance }}
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

const selectedWarehouse = ref('نمایش مجموع همه‌ انبارها');
const search = ref('');

const stockList = ref([
    { code: 'ITM-1001', name: 'لپ‌تاپ لنوو V15', group: 'تجهیزات IT', unit: 'دستگاه', initBalance: '10', totalIn: '55', totalOut: '40', currentBalance: '25' },
    { code: 'ITM-1002', name: 'کابل شبکه CAT6 - حلقه', group: 'تجهیزات شبکه', unit: 'متر', initBalance: '1000', totalIn: '500', totalOut: '1200', currentBalance: '300' },
    { code: 'ITM-1004', name: 'سرور HP DL380 G10', group: 'سرور و تجهیزات', unit: 'دستگاه', initBalance: '2', totalIn: '0', totalOut: '2', currentBalance: '0' },
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

.gap-4 {
    gap: 16px;
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
