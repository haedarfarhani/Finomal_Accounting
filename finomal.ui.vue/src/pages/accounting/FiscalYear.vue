<template>
    <div class="accounting-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="blue" class="mr-2" size="28">mdi-calendar-check-outline</v-icon>
                    مدیریت سال مالی</h1>
                <p class="page-subtitle">تعریف دوره‌های مالی، بستن حساب‌ها، و مدیریت مقاطع زمانی حسابداری</p>
            </div>
            <div class="header-actions">
                <v-btn color="blue" prepend-icon="mdi-calendar-plus" elevation="2"
                    class="action-btn text-white font-weight-bold" rounded="lg">سال مالی جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-container p-4">
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>عنوان دوره</th>
                            <th>تاریخ شروع</th>
                            <th>تاریخ پایان</th>
                            <th class="text-center">وضعیت</th>
                            <th class="text-center">تعداد اسناد</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in fiscalYears" :key="item.id" class="table-row"
                            :class="{ 'active-row': item.active }">
                            <td class="font-weight-bold" :class="{ 'text-blue': item.active }">{{ item.title }}</td>
                            <td class="mono-number">{{ item.startDate }}</td>
                            <td class="mono-number">{{ item.endDate }}</td>
                            <td class="text-center">
                                <v-chip size="x-small"
                                    :color="item.closed ? 'error' : (item.active ? 'success' : 'warning')"
                                    variant="flat" class="font-weight-bold">
                                    {{ item.closed ? 'بسته شده' : (item.active ? 'جاری' : 'باز (غیرفعال)') }}
                                </v-chip>
                            </td>
                            <td class="text-center mono-number">{{ item.docsCount }}</td>
                            <td class="text-center">
                                <v-btn icon="mdi-pencil-outline" size="small" variant="text" color="info"></v-btn>
                                <v-btn v-if="!item.closed && !item.active" icon="mdi-check-circle-outline" size="small"
                                    variant="text" color="success" title="انتخاب به عنوان دوره جاری"></v-btn>
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
const fiscalYears = ref([
    { id: 1, title: 'سال مالی ۱۴۰۳', startDate: '۱۴۰۳/۰۱/۰۱', endDate: '۱۴۰۳/۱۲/۳۰', active: true, closed: false, docsCount: '۱,۴۵۲' },
    { id: 2, title: 'سال مالی ۱۴۰۲', startDate: '۱۴۰۲/۰۱/۰۱', endDate: '۱۴۰۲/۱۲/۲۹', active: false, closed: true, docsCount: '۵,۳۴۰' },
    { id: 3, title: 'سال مالی ۱۴۰۱', startDate: '۱۴۰۱/۰۱/۰۱', endDate: '۱۴۰۱/۱۲/۲۹', active: false, closed: true, docsCount: '۴,۸۹۰' },
]);
</script>

<style scoped>
.accounting-page {
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
    padding-bottom: 12px;
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

.active-row td {
    background: rgba(56, 189, 248, 0.05);
    border-bottom: 1px solid rgba(56, 189, 248, 0.1);
}

.mono-number {
    font-family: inherit;
    letter-spacing: 0.5px;
}
</style>
