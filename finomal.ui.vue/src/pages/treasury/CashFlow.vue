<template>
    <div class="report-page" dir="rtl">
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title"><v-icon color="cyan" class="mr-2" size="28">mdi-cash-sync</v-icon> جریان وجه نقد
                </h1>
                <p class="page-subtitle">گزارش ورود و خروج نقدینگی ناشی از عملیات، سرمایه‌گذاری و تامین مالی</p>
            </div>
            <div class="header-actions">
                <!-- Date simulation selector -->
                <v-btn variant="tonal" color="cyan" rounded="lg" prepend-icon="mdi-calendar">پایان سال ۱۴۰۳</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="table-container p-6">
                <table class="financial-table">
                    <thead>
                        <tr>
                            <th class="w-75">شرح</th>
                            <th class="text-left">مبلغ (میلیون ریال)</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="2" class="font-weight-bold text-cyan mt-2 d-block">فعالیت‌های عملیاتی</td>
                        </tr>
                        <tr>
                            <td class="indent-1">وجوه نقد دریافتی از مشتریان</td>
                            <td class="text-left mono-number">۱۱۸,۰۰۰</td>
                        </tr>
                        <tr>
                            <td class="indent-1 text-error">(وجوه نقد پرداختی به تامین کنندگان و کارکنان)</td>
                            <td class="text-left text-error mono-number">(۷۲,۰۰۰)</td>
                        </tr>
                        <tr>
                            <td class="indent-1 text-error">(مالیات بر درآمد پرداختی)</td>
                            <td class="text-left text-error mono-number">(۶,۵۰۰)</td>
                        </tr>
                        <tr class="subtotal-row">
                            <td class="font-weight-bold">جریان خالص وجه نقد ناشی از فعالیت‌های عملیاتی</td>
                            <td class="text-left font-weight-bold mono-number text-success">۳۹,۵۰۰</td>
                        </tr>

                        <tr>
                            <td colspan="2" class="font-weight-bold text-cyan mt-6 d-block">فعالیت‌های سرمایه‌گذاری</td>
                        </tr>
                        <tr>
                            <td class="indent-1 text-error">(خرید دارایی‌های ثابت مشهود)</td>
                            <td class="text-left text-error mono-number">(۲۵,۰۰۰)</td>
                        </tr>
                        <tr>
                            <td class="indent-1">دریافتی بابت فروش دارایی‌ها</td>
                            <td class="text-left mono-number">۲,۵۰۰</td>
                        </tr>
                        <tr class="subtotal-row">
                            <td class="font-weight-bold">جریان خالص وجه نقد ناشی از فعالیت‌های سرمایه‌گذاری</td>
                            <td class="text-left font-weight-bold mono-number text-error">(۲۲,۵۰۰)</td>
                        </tr>

                        <tr>
                            <td colspan="2" class="font-weight-bold text-cyan mt-6 d-block">فعالیت‌های تامین مالی</td>
                        </tr>
                        <tr>
                            <td class="indent-1">دریافت تسهیلات مالی</td>
                            <td class="text-left mono-number">۱۰,۰۰۰</td>
                        </tr>
                        <tr>
                            <td class="indent-1 text-error">(بازپرداخت تسهیلات مالی)</td>
                            <td class="text-left text-error mono-number">(۱۵,۰۰۰)</td>
                        </tr>
                        <tr>
                            <td class="indent-1 text-error">(سود سهام پرداختی)</td>
                            <td class="text-left text-error mono-number">(۵,۰۰۰)</td>
                        </tr>
                        <tr class="subtotal-row">
                            <td class="font-weight-bold">جریان خالص وجه نقد ناشی از فعالیت‌های تامین مالی</td>
                            <td class="text-left font-weight-bold mono-number text-error">(۱۰,۰۰۰)</td>
                        </tr>

                        <tr class="grand-total-row mt-6">
                            <td class="font-weight-bold text-h6 text-white">افزایش (کاهش) خالص در موجودی نقد</td>
                            <td class="text-left font-weight-bold text-h6 mono-number text-success">۷,۰۰۰</td>
                        </tr>
                        <tr>
                            <td class="font-weight-bold pl-4 py-4">موجودی نقد در ابتدای دوره</td>
                            <td class="text-left font-weight-bold mono-number py-4 text-grey-lighten-1">۸,۲۰۰</td>
                        </tr>
                        <tr class="total-row">
                            <td class="font-weight-bold text-cyan text-h6">موجودی نقد در پایان دوره</td>
                            <td class="text-left font-weight-bold mono-number text-cyan text-h6">۱۵,۲۰۰</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </v-card>
    </div>
</template>

<style scoped>
/* Same CSS structure as Income statement */
.report-page {
    font-family: 'Vazirmatn', sans-serif;
    color: #e2e8f0;
    max-width: 1000px;
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
    padding: 24px;
}

.financial-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 15px;
}

.financial-table th {
    padding: 12px;
    border-bottom: 2px solid rgba(255, 255, 255, 0.1);
    color: #94a3b8;
    font-weight: 700;
    text-align: right;
}

.financial-table td {
    padding: 12px;
    border-bottom: 1px dashed rgba(255, 255, 255, 0.05);
}

.indent-1 {
    padding-right: 24px !important;
}

.subtotal-row td {
    background: rgba(255, 255, 255, 0.02);
}

.total-row td {
    background: rgba(34, 211, 238, 0.05);
}

.grand-total-row td {
    background: rgba(34, 211, 238, 0.15);
    border-top: 2px double #22d3ee;
}

.mono-number {
    font-family: inherit;
    letter-spacing: 1px;
}
</style>
