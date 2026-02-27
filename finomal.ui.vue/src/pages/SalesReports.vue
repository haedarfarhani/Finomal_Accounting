<template>
    <div class="sales-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="violet" class="mr-2" size="28">mdi-chart-areaspline</v-icon>
                    گزارش‌های فروش</h1>
                <p class="page-subtitle">آمار تحلیلی و لیست فروش‌های انجام شده به تفکیک دوره‌ها</p>
            </div>
        </header>

        <v-row class="mt-4 mb-2">
            <v-col cols="12" md="4"><v-card class="stat-card" color="rgba(167, 139, 250, 0.1)" elevation="0">
                    <div class="text-violet">مجموع فروش این ماه: ۲۴۸ میلیون تومان</div>
                </v-card></v-col>
            <v-col cols="12" md="4"><v-card class="stat-card" color="rgba(167, 139, 250, 0.1)" elevation="0">
                    <div class="text-violet">برترین مشتری: پتروشیمی زاگرس</div>
                </v-card></v-col>
            <v-col cols="12" md="4"><v-card class="stat-card" color="rgba(34, 197, 94, 0.1)" elevation="0">
                    <div class="text-success">وصولی این ماه: ۱۶۰ میلیون تومان</div>
                </v-card></v-col>
        </v-row>

        <v-card class="content-card mt-4" elevation="0">
            <div class="p-4 d-flex justify-center align-center" style="min-height: 300px">
                <div class="text-center text-grey">
                    <v-icon size="64" class="mb-4">mdi-finance</v-icon>
                    <p>نمودارها و گزارشات تفصیلی به زودی اضافه خواهند شد...</p>
                </div>
            </div>
        </v-card>
    </div>
</template>

<style scoped>
.sales-page {
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

.stat-card {
    padding: 16px;
    border-radius: 12px;
    font-weight: 700;
    display: flex;
    align-items: center;
    font-size: 16px;
    border: 1px solid rgba(255, 255, 255, 0.05);
}
</style>
