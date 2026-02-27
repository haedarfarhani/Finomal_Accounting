<template>
    <div class="inventory-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="success" class="mr-2"
                        size="28">mdi-truck-delivery-outline</v-icon> رسید انبار (ورود کالا)</h1>
                <p class="page-subtitle">ثبت اسناد ورود کالا (خرید داخلی، واردات، برگشت از فروش، و تولید)</p>
            </div>
            <div class="header-actions">
                <v-btn color="success" prepend-icon="mdi-plus" elevation="2" class="action-btn font-weight-bold"
                    rounded="lg">صدور رسید جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-toolbar px-4 py-4 d-flex justify-space-between align-center border-b">
                <div class="d-flex gap-2" style="width: 400px;">
                    <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجو..."
                        variant="solo-filled" density="comfortable" hide-details rounded="lg"
                        bg-color="rgba(255, 255, 255, 0.03)"></v-text-field>
                </div>
                <div class="d-flex gap-2 text-caption">
                    <v-chip color="success" variant="tonal" label size="small">قطعی شده: ۴۵</v-chip>
                    <v-chip color="warning" variant="tonal" label size="small">موقت: ۳</v-chip>
                </div>
            </div>
            <div class="table-container">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>شماره رسید</th>
                            <th>تاریخ</th>
                            <th>انبار مقصد</th>
                            <th>نوع رسید</th>
                            <th>تامین‌کننده / تحویل دهنده</th>
                            <th class="text-left">تعداد اقلام</th>
                            <th class="text-center">وضعیت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in receipts" :key="item.id" class="table-row">
                            <td class="mono-number font-weight-bold text-success">{{ item.receiptNo }}</td>
                            <td class="mono-number">{{ item.date }}</td>
                            <td>{{ item.warehouse }}</td>
                            <td>
                                <span class="type-badge">{{ item.type }}</span>
                            </td>
                            <td class="text-grey-lighten-1">{{ item.supplier }}</td>
                            <td class="text-left mono-number">{{ item.itemsCount }}</td>
                            <td class="text-center">
                                <v-chip size="x-small" :color="item.status === 'قطعی' ? 'success' : 'warning'"
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
const search = ref('');
const receipts = ref([
    { id: 1, receiptNo: 'REQ-403-10045', date: '۱۴۰۳/۱۱/۲۸', warehouse: 'انبار مرکزی', type: 'رسید خرید', supplier: 'شرکت فناوری نوین', itemsCount: '۴۵', status: 'قطعی' },
    { id: 2, receiptNo: 'REQ-403-10046', date: '۱۴۰۳/۱۱/۲۹', warehouse: 'انبار مرکزی', type: 'اول دوره', supplier: 'سند افتتاحیه', itemsCount: '۱۲۰', status: 'قطعی' },
    { id: 3, receiptNo: 'REQ-403-10047', date: '۱۴۰۳/۱۱/۳۰', warehouse: 'انبار ضایعات', type: 'برگشت از فروش', supplier: 'مشتری: دیجی‌کالا', itemsCount: '۲', status: 'موقت (پیش‌نویس)' },
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

.type-badge {
    background: rgba(255, 255, 255, 0.05);
    padding: 4px 8px;
    border-radius: 6px;
    font-size: 11.5px;
    color: #94a3b8;
    border: 1px solid rgba(255, 255, 255, 0.05);
}
</style>
