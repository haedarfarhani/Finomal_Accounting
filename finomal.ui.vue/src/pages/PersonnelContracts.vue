<template>
    <div class="contracts-page" dir="rtl">
        <!-- Header -->
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title">
                    <v-icon color="primary" class="mr-2" size="28">mdi-file-document-edit-outline</v-icon>
                    قراردادهای پرسنل
                </h1>
                <p class="page-subtitle">مدیریت و مشاهده وضعیت قراردادهای کاری کارمندان</p>
            </div>

            <div class="header-actions">
                <v-btn color="primary" prepend-icon="mdi-plus" elevation="2" class="add-btn" rounded="lg">
                    ثبت قرارداد جدید
                </v-btn>
            </div>
        </header>

        <!-- Stats Cards -->
        <v-row class="mb-6 stats-row">
            <v-col cols="12" md="3" v-for="stat in stats" :key="stat.title">
                <v-card class="stat-card" elevation="0">
                    <div class="stat-icon-wrapper" :class="`bg-${stat.color}-lighten-4`">
                        <v-icon :color="stat.color" size="24">{{ stat.icon }}</v-icon>
                    </div>
                    <div class="stat-info">
                        <p class="stat-title">{{ stat.title }}</p>
                        <h3 class="stat-value">{{ stat.value }}</h3>
                    </div>
                </v-card>
            </v-col>
        </v-row>

        <!-- Main Content / Table -->
        <v-card class="content-card" elevation="0">
            <!-- Toolbar -->
            <div class="table-toolbar">
                <div class="search-box">
                    <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجو در قراردادها..."
                        variant="solo-filled" density="comfortable" hide-details rounded="lg"
                        bg-color="rgba(255, 255, 255, 0.03)" color="primary"></v-text-field>
                </div>

                <div class="filter-actions">
                    <v-btn variant="outlined" prepend-icon="mdi-filter-variant" color="grey-lighten-1"
                        class="filter-btn" rounded="lg">
                        فیلترها
                    </v-btn>
                    <v-btn variant="outlined" prepend-icon="mdi-export-variant" color="grey-lighten-1"
                        class="export-btn" rounded="lg">
                        خروجی
                    </v-btn>
                </div>
            </div>

            <!-- Table Section -->
            <div class="table-responsive">
                <table class="custom-table">
                    <thead>
                        <tr>
                            <th>پرسنل</th>
                            <th>شماره قرارداد</th>
                            <th>نوع قرارداد</th>
                            <th>تاریخ شروع</th>
                            <th>تاریخ پایان</th>
                            <th>وضعیت</th>
                            <th class="text-left">عملیات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in filteredContracts" :key="item.id" class="table-row">
                            <!-- Personnel -->
                            <td>
                                <div class="d-flex align-center gap-3">
                                    <v-avatar size="36" color="surface-variant" variant="tonal">
                                        <span class="text-caption font-weight-bold">{{ item.initials }}</span>
                                    </v-avatar>
                                    <div>
                                        <div class="font-weight-bold text-body-2">{{ item.name }}</div>
                                        <div class="text-caption text-grey-lighten-1">{{ item.position }}</div>
                                    </div>
                                </div>
                            </td>

                            <!-- Contract ID -->
                            <td>
                                <span class="mono-text">{{ item.id }}</span>
                            </td>

                            <!-- Type -->
                            <td>{{ item.type }}</td>

                            <!-- Start Date -->
                            <td>{{ item.startDate }}</td>

                            <!-- End Date -->
                            <td>
                                <span :class="{ 'text-error': item.isExpiringSoon }">{{ item.endDate }}</span>
                            </td>

                            <!-- Status -->
                            <td>
                                <v-chip :color="getStatusColor(item.status)" size="small" label class="status-chip">
                                    <v-icon start size="14">{{ getStatusIcon(item.status) }}</v-icon>
                                    {{ item.status }}
                                </v-chip>
                            </td>

                            <!-- Actions -->
                            <td class="text-left">
                                <v-btn icon="mdi-eye-outline" variant="text" size="small" color="info"></v-btn>
                                <v-btn icon="mdi-pencil-outline" variant="text" size="small" color="primary"></v-btn>
                                <v-menu location="start">
                                    <template v-slot:activator="{ props }">
                                        <v-btn icon="mdi-dots-vertical" variant="text" size="small" color="grey"
                                            v-bind="props"></v-btn>
                                    </template>
                                    <v-list bg-color="#1e293b" density="compact">
                                        <v-list-item link prepend-icon="mdi-printer" title="چاپ قرارداد"></v-list-item>
                                        <v-list-item link prepend-icon="mdi-content-copy"
                                            title="تمدید قرارداد"></v-list-item>
                                        <v-divider></v-divider>
                                        <v-list-item link prepend-icon="mdi-delete" base-color="error"
                                            title="حذف"></v-list-item>
                                    </v-list>
                                </v-menu>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <!-- Empty State -->
                <div v-if="filteredContracts.length === 0" class="empty-state py-12 text-center">
                    <v-icon size="64" color="grey-darken-2" class="mb-4">mdi-file-hidden</v-icon>
                    <h3 class="text-h6 text-grey-lighten-1">قراردادی یافت نشد!</h3>
                    <p class="text-body-2 text-grey">لطفا عبارت جستجو را تغییر دهید.</p>
                </div>
            </div>
        </v-card>
    </div>
</template>

<script setup>
import { ref, computed } from 'vue';

const search = ref('');

// Mock Data
const stats = ref([
    { title: 'کل قراردادها', value: '۱۲۸', icon: 'mdi-file-document-multiple', color: 'blue' },
    { title: 'فعال', value: '۱۱۵', icon: 'mdi-check-circle', color: 'success' },
    { title: 'در آستانه انقضا', value: '۸', icon: 'mdi-alert-circle', color: 'warning' },
    { title: 'پایان یافته', value: '۵', icon: 'mdi-close-circle', color: 'error' },
]);

const contracts = ref([
    {
        id: 'CNT-403-001',
        name: 'علی احمدی',
        initials: 'ع.ا',
        position: 'توسعه‌دهنده فرانت‌اند',
        type: 'تمام وقت',
        startDate: '۱۴۰۲/۰۱/۱۵',
        endDate: '۱۴۰۳/۰۱/۱۵',
        status: 'فعال',
        isExpiringSoon: false
    },
    {
        id: 'CNT-403-002',
        name: 'سارا کریمی',
        initials: 'س.ک',
        position: 'طراح رابط کاربری',
        type: 'پاره وقت',
        startDate: '۱۴۰۲/۰۶/۰۱',
        endDate: '۱۴۰۳/۰۶/۰۱',
        status: 'فعال',
        isExpiringSoon: false
    },
    {
        id: 'CNT-403-003',
        name: 'محمد رضایی',
        initials: 'م.ر',
        position: 'مدیر پروژه',
        type: 'پروژه‌ای',
        startDate: '۱۴۰۲/۱۰/۱۵',
        endDate: '۱۴۰۳/۰۲/۱۵',
        status: 'در آستانه انقضا',
        isExpiringSoon: true
    },
    {
        id: 'CNT-403-004',
        name: 'نگین تهرانی',
        initials: 'ن.ت',
        position: 'حسابدار',
        type: 'تمام وقت',
        startDate: '۱۴۰۱/۰۷/۰۱',
        endDate: '۱۴۰۲/۰۷/۰۱',
        status: 'پایان یافته',
        isExpiringSoon: false
    },
]);

const filteredContracts = computed(() => {
    if (!search.value) return contracts.value;
    const s = search.value.toLowerCase();
    return contracts.value.filter(c =>
        c.name.toLowerCase().includes(s) ||
        c.id.toLowerCase().includes(s) ||
        c.position.toLowerCase().includes(s)
    );
});

const getStatusColor = (status) => {
    switch (status) {
        case 'فعال': return 'success';
        case 'در آستانه انقضا': return 'warning';
        case 'پایان یافته': return 'error';
        default: return 'grey';
    }
};

const getStatusIcon = (status) => {
    switch (status) {
        case 'فعال': return 'mdi-check-circle-outline';
        case 'در آستانه انقضا': return 'mdi-clock-alert-outline';
        case 'پایان یافته': return 'mdi-close-circle-outline';
        default: return 'mdi-circle-outline';
    }
};
</script>

<style scoped>
.contracts-page {
    font-family: 'Vazirmatn', sans-serif;
    color: #e2e8f0;
    max-width: 1400px;
    margin: 0 auto;
}

/* Header */
.page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 24px;
}

.page-title {
    font-size: 24px;
    font-weight: 800;
    color: #f8fafc;
    display: flex;
    align-items: center;
    margin-bottom: 4px;
}

.page-subtitle {
    font-size: 13px;
    color: #94a3b8;
}

.add-btn {
    font-weight: 600;
    text-transform: none;
    letter-spacing: 0;
    background: linear-gradient(135deg, #0ea5e9, #2563eb);
}

/* Stats Cards */
.stat-card {
    background: rgba(30, 41, 59, 0.4);
    border: 1px solid rgba(255, 255, 255, 0.05);
    backdrop-filter: blur(10px);
    border-radius: 16px;
    padding: 20px;
    display: flex;
    align-items: center;
    gap: 16px;
    transition: transform 0.2s, box-shadow 0.2s, border-color 0.2s;
}

.stat-card:hover {
    transform: translateY(-2px);
    box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
    border-color: rgba(56, 189, 248, 0.2);
}

.stat-icon-wrapper {
    width: 54px;
    height: 54px;
    border-radius: 14px;
    display: flex;
    align-items: center;
    justify-content: center;
    background: rgba(255, 255, 255, 0.05);
    /* fallback */
}

/* Adjusted specific background colors for stat icons */
.bg-blue-lighten-4 {
    background: rgba(56, 189, 248, 0.15) !important;
}

.bg-success-lighten-4 {
    background: rgba(34, 197, 94, 0.15) !important;
}

.bg-warning-lighten-4 {
    background: rgba(245, 158, 11, 0.15) !important;
}

.bg-error-lighten-4 {
    background: rgba(239, 68, 68, 0.15) !important;
}


.stat-title {
    font-size: 13px;
    color: #94a3b8;
    margin-bottom: 4px;
}

.stat-value {
    font-size: 24px;
    font-weight: 800;
    color: #e2e8f0;
}

/* Content Area */
.content-card {
    background: rgba(15, 23, 42, 0.6);
    border: 1px solid rgba(255, 255, 255, 0.05);
    border-radius: 16px;
    overflow: hidden;
}

/* Toolbar */
.table-toolbar {
    padding: 20px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
    gap: 20px;
}

.search-box {
    width: 350px;
}

.search-box :deep(.v-field) {
    border-radius: 12px;
    border: 1px solid rgba(255, 255, 255, 0.05);
}

.search-box :deep(.v-field:hover) {
    border-color: rgba(56, 189, 248, 0.3);
}

.filter-actions {
    display: flex;
    gap: 12px;
}

/* Table */
.table-responsive {
    overflow-x: auto;
}

.custom-table {
    width: 100%;
    border-collapse: collapse;
    text-align: right;
}

.custom-table th {
    padding: 16px 20px;
    font-size: 12px;
    font-weight: 600;
    color: #94a3b8;
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
    background: rgba(255, 255, 255, 0.01);
    white-space: nowrap;
}

.custom-table td {
    padding: 14px 20px;
    font-size: 13.5px;
    color: #cbd5e1;
    border-bottom: 1px solid rgba(255, 255, 255, 0.03);
    transition: background 0.2s;
    vertical-align: middle;
}

.table-row:hover td {
    background: rgba(255, 255, 255, 0.02);
}

.mono-text {
    font-family: inherit;
    /* Just styling for visual distinction */
    letter-spacing: 1px;
    color: #38bdf8;
    font-weight: 500;
    font-size: 12.5px;
}

.status-chip {
    font-weight: 600;
    letter-spacing: 0;
}

.gap-3 {
    gap: 12px;
}

@media (max-width: 960px) {
    .page-header {
        flex-direction: column;
        align-items: flex-start;
        gap: 16px;
    }

    .table-toolbar {
        flex-direction: column;
        align-items: stretch;
    }

    .search-box {
        width: 100%;
    }
}
</style>
