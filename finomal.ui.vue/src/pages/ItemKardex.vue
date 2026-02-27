<template>
    <div class="inventory-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="cyan-lighten-1" class="mr-2"
                        size="28">mdi-clipboard-text-history-outline</v-icon> کاردکس کالا</h1>
                <p class="page-subtitle">گزارش ریز ورود و خروج هر کالا و مانده در انبار مشخص</p>
            </div>
            <div class="header-actions">
                <v-btn color="grey-darken-3" prepend-icon="mdi-printer" elevation="0" class="action-btn border"
                    rounded="lg">چاپ کاردکس</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="filters px-4 py-4 d-flex gap-4 border-b">
                <v-autocomplete v-model="selectedProduct" :items="products" label="انتخاب کالا" variant="solo-filled"
                    density="comfortable" hide-details bg-color="rgba(30, 41, 59, 1)"
                    prepend-inner-icon="mdi-package-variant" class="flex-grow-1" rounded="lg"></v-autocomplete>
                <v-select v-model="selectedWarehouse" :items="['انبار مرکزی', 'انبار ضایعات', 'همه انبارها']"
                    label="انتخاب انبار" variant="solo-filled" density="comfortable" hide-details
                    bg-color="rgba(30, 41, 59, 1)" style="width: 250px" rounded="lg"></v-select>
                <v-btn color="info" height="48" rounded="lg" class="px-6" elevation="2">مشاهده</v-btn>
            </div>

            <div class="table-container p-0">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>ردیف</th>
                            <th>تاریخ</th>
                            <th>نوع سند</th>
                            <th>شماره سند</th>
                            <th>شرح تفصیلی</th>
                            <th class="text-success text-center px-1">وارده</th>
                            <th class="text-error text-center px-1">صادره</th>
                            <th class="text-info text-center px-1">موجودی</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(item, index) in kardex" :key="index" class="table-row">
                            <td class="text-grey text-center px-2">{{ index + 1 }}</td>
                            <td class="mono-number">{{ item.date }}</td>
                            <td><v-chip size="x-small" :color="item.typeColor" variant="tonal">{{ item.type }}</v-chip>
                            </td>
                            <td class="mono-number font-weight-bold">{{ item.docNo }}</td>
                            <td class="text-grey-lighten-2 text-caption w-25">{{ item.desc }}</td>
                            <td class="text-success text-center mono-number">{{ item.in || '-' }}</td>
                            <td class="text-error text-center mono-number">{{ item.out || '-' }}</td>
                            <td class="text-info text-center mono-number font-weight-black">{{ item.balance }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </v-card>
    </div>
</template>

<script setup>
import { ref } from 'vue';

const selectedProduct = ref('لپ‌تاپ لنوو V15');
const selectedWarehouse = ref('انبار مرکزی');

const products = ['لپ‌تاپ لنوو V15', 'کابل شبکه CAT6 - حلقه', 'سرور HP DL380 G10'];

const kardex = ref([
    { date: '----/--/--', type: 'انتقال از دوره قبل', typeColor: 'grey', docNo: '-', desc: 'موجودی اولیه سال مالی جدید', in: '10', out: '', balance: '10' },
    { date: '۱۴۰۳/۰۱/۱۵', type: 'رسید خرید', typeColor: 'success', docNo: 'REQ-403-101', desc: 'خرید از شرکت رایانه‌پرداز فاکتور ۹۳۱', in: '30', out: '', balance: '40' },
    { date: '۱۴۰۳/۰۲/۰۲', type: 'حواله فروش', typeColor: 'error', docNo: 'ISS-403-24', desc: 'فروش به شرکت زاگرس', in: '', out: '12', balance: '28' },
    { date: '۱۴۰۳/۰۵/۱۱', type: 'حواله مصرف داخلی', typeColor: 'error', docNo: 'ISS-403-88', desc: 'تخصیص به پرسنل جدید واحد IT', in: '', out: '3', balance: '25' },
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

.px-1 {
    padding-left: 8px !important;
    padding-right: 8px !important;
}

.px-2 {
    padding-left: 16px !important;
    padding-right: 16px !important;
}
</style>
