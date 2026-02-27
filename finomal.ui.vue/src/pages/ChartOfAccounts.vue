<template>
    <div class="accounting-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="blue" class="mr-2" size="28">mdi-format-list-numbered</v-icon>
                    تعریف سرفصل‌ها (کدینگ)</h1>
                <p class="page-subtitle">ساختار درختی حساب‌های گروه، کل، معین و تفصیلی</p>
            </div>
            <div class="header-actions">
                <v-btn color="blue" prepend-icon="mdi-plus" elevation="2" class="action-btn text-white font-weight-bold"
                    rounded="lg">حساب جدید</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4 p-4" elevation="0">
            <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجوی کد یا عنوان حساب..."
                variant="solo-filled" density="comfortable" hide-details rounded="lg"
                bg-color="rgba(255, 255, 255, 0.03)" class="mb-4" style="max-width: 400px;"></v-text-field>

            <div class="tree-container">
                <!-- A simple simulation of a tree table -->
                <table class="custom-table w-100">
                    <thead>
                        <tr>
                            <th>کد حساب</th>
                            <th>عنوان حساب</th>
                            <th>سطح حساب</th>
                            <th>ماهیت</th>
                            <th class="text-center">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="table-row group-row">
                            <td class="mono-number font-weight-bold text-blue">۱</td>
                            <td class="font-weight-bold">دارایی‌ها</td>
                            <td><v-chip size="x-small" color="blue">گروه</v-chip></td>
                            <td>بدهکار</td>
                            <td class="text-center"><v-btn icon="mdi-dots-vertical" size="small" variant="text"
                                    color="grey"></v-btn></td>
                        </tr>
                        <tr class="table-row ledger-row">
                            <td class="mono-number pl-6 pr-4">۱۱</td>
                            <td>دارایی‌های جاری</td>
                            <td><v-chip size="x-small" color="success">کل</v-chip></td>
                            <td>بدهکار</td>
                            <td class="text-center"><v-btn icon="mdi-dots-vertical" size="small" variant="text"
                                    color="grey"></v-btn></td>
                        </tr>
                        <tr class="table-row subsidiary-row">
                            <td class="mono-number pr-8 text-grey-lighten-1">۱۱۰۱</td>
                            <td class="text-grey-lighten-1">موجودی نقد و بانک</td>
                            <td><v-chip size="x-small" color="warning" variant="outlined">معین</v-chip></td>
                            <td>بدهکار</td>
                            <td class="text-center"><v-btn icon="mdi-pencil-outline" size="small" variant="text"
                                    color="info"></v-btn></td>
                        </tr>
                        <tr class="table-row subsidiary-row">
                            <td class="mono-number pr-8 text-grey-lighten-1">۱۱۰۲</td>
                            <td class="text-grey-lighten-1">حساب‌های دریافتنی تجاری</td>
                            <td><v-chip size="x-small" color="warning" variant="outlined">معین</v-chip></td>
                            <td>بدهکار</td>
                            <td class="text-center"><v-btn icon="mdi-pencil-outline" size="small" variant="text"
                                    color="info"></v-btn></td>
                        </tr>

                        <tr class="table-row group-row mt-4">
                            <td class="mono-number font-weight-bold text-blue">۲</td>
                            <td class="font-weight-bold">بدهی‌ها</td>
                            <td><v-chip size="x-small" color="blue">گروه</v-chip></td>
                            <td>بستانکار</td>
                            <td class="text-center"><v-btn icon="mdi-dots-vertical" size="small" variant="text"
                                    color="grey"></v-btn></td>
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
    padding: 20px;
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

.group-row td {
    background: rgba(59, 130, 246, 0.05);
}

.ledger-row td {
    background: rgba(255, 255, 255, 0.01);
}

.mono-number {
    font-family: inherit;
    letter-spacing: 0.5px;
}

.pr-4 {
    padding-right: 24px !important;
}

.pr-8 {
    padding-right: 48px !important;
}
</style>
