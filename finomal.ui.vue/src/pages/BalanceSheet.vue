<template>
    <div class="report-page" dir="rtl">
        <!-- Header -->
        <header class="page-header">
            <div class="header-content">
                <h1 class="page-title">
                    <v-icon color="cyan" class="mr-2" size="28">mdi-scale-balance</v-icon>
                    ترازنامه (صورت وضعیت مالی)
                </h1>
                <p class="page-subtitle">گزارش وضعیت دارایی‌ها، بدهی‌ها و حقوق صاحبان سهام در یک تاریخ مشخص</p>
            </div>

            <div class="header-actions">
                <!-- Date simulation selector -->
                <v-btn variant="tonal" color="cyan" rounded="lg" prepend-icon="mdi-calendar">پایان سال ۱۴۰۳</v-btn>
                <v-btn color="cyan" prepend-icon="mdi-printer" class="action-btn text-white" rounded="lg">چاپ</v-btn>
            </div>
        </header>

        <v-card class="content-card mt-4" elevation="0">
            <div class="report-header">
                <div class="company-name text-h6 font-weight-bold text-cyan text-center">شرکت نمونه فینومال</div>
                <div class="report-title text-center text-body-1 mt-1">ترازنامه به تاریخ ۲۹ اسفند ۱۴۰۳</div>
                <div class="report-currency text-caption text-grey text-left">مبالغ به میلیون ریال</div>
            </div>

            <v-row class="px-2 pb-4">
                <!-- دارایی ها (Assets) -->
                <v-col cols="12" md="6" class="asset-col border-right-md border-b-sm">
                    <h3 class="side-title text-success mb-3">دارایی‌ها</h3>
                    <table class="financial-table">
                        <tbody>
                            <tr>
                                <td colspan="2" class="font-weight-bold text-grey-lighten-1">دارایی‌های جاری</td>
                            </tr>
                            <tr>
                                <td class="indent-1">موجودی نقد و بانک</td>
                                <td class="text-left mono-number">۱۵,۲۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">سرمایه‌گذاری‌های کوتاه مدت</td>
                                <td class="text-left mono-number">۴,۵۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">حساب‌ها و اسناد دریافتنی تجاری</td>
                                <td class="text-left mono-number">۲۸,۱۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">پیش‌پرداخت‌ها</td>
                                <td class="text-left mono-number">۲,۳۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">موجودی مواد و کالا</td>
                                <td class="text-left mono-number">۳۵,۶۰۰</td>
                            </tr>
                            <tr class="subtotal-row">
                                <td class="font-weight-bold">جمع دارایی‌های جاری</td>
                                <td class="text-left font-weight-bold mono-number text-success">۸۵,۷۰۰</td>
                            </tr>

                            <tr>
                                <td colspan="2" class="font-weight-bold text-grey-lighten-1 mt-4 d-block">دارایی‌های
                                    غیرجاری</td>
                            </tr>
                            <tr>
                                <td class="indent-1">دارایی‌های ثابت مشهود</td>
                                <td class="text-left mono-number">۱۲۰,۵۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">دارایی‌های نامشهود</td>
                                <td class="text-left mono-number">۱۲,۰۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">سرمایه‌گذاری‌های بلندمدت</td>
                                <td class="text-left mono-number">۱۵,۰۰۰</td>
                            </tr>
                            <tr class="subtotal-row">
                                <td class="font-weight-bold">جمع دارایی‌های غیرجاری</td>
                                <td class="text-left font-weight-bold mono-number text-success">۱۴۷,۵۰۰</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="grand-total-row">
                                <td class="font-weight-bold">جمع کل دارایی‌ها</td>
                                <td class="text-left font-weight-bold mono-number text-cyan text-h6">۲۳۳,۲۰۰</td>
                            </tr>
                        </tfoot>
                    </table>
                </v-col>

                <!-- بدهی ها و حقوق صاحبان سهام (Liabilities & Equity) -->
                <v-col cols="12" md="6" class="liabilities-col">
                    <h3 class="side-title text-error mb-3">بدهی‌ها و حقوق صاحبان سهام</h3>
                    <table class="financial-table">
                        <tbody>
                            <tr>
                                <td colspan="2" class="font-weight-bold text-grey-lighten-1">بدهی‌های جاری</td>
                            </tr>
                            <tr>
                                <td class="indent-1">حساب‌ها و اسناد پرداختنی تجاری</td>
                                <td class="text-left mono-number">۳۲,۴۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">پیش‌دریافت‌ها</td>
                                <td class="text-left mono-number">۸,۱۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">ذخیره مالیات</td>
                                <td class="text-left mono-number">۸,۹۵۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">تسهیلات مالی کوتاه مدت</td>
                                <td class="text-left mono-number">۱۵,۰۰۰</td>
                            </tr>
                            <tr class="subtotal-row">
                                <td class="font-weight-bold">جمع بدهی‌های جاری</td>
                                <td class="text-left font-weight-bold mono-number text-error">۶۴,۴۵۰</td>
                            </tr>

                            <tr>
                                <td colspan="2" class="font-weight-bold text-grey-lighten-1 mt-4 d-block">بدهی‌های
                                    غیرجاری</td>
                            </tr>
                            <tr>
                                <td class="indent-1">تسهیلات مالی بلندمدت</td>
                                <td class="text-left mono-number">۴۵,۰۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">ذخیره مزایای پایان خدمت</td>
                                <td class="text-left mono-number">۱۲,۴۰۰</td>
                            </tr>
                            <tr class="subtotal-row">
                                <td class="font-weight-bold">جمع بدهی‌های غیرجاری</td>
                                <td class="text-left font-weight-bold mono-number text-error">۵۷,۴۰۰</td>
                            </tr>

                            <tr>
                                <td colspan="2" class="font-weight-bold text-grey-lighten-1 mt-6 d-block text-cyan">حقوق
                                    صاحبان سهام</td>
                            </tr>
                            <tr>
                                <td class="indent-1">سرمایه</td>
                                <td class="text-left mono-number">۵۰,۰۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">اندوخته قانونی</td>
                                <td class="text-left mono-number">۵,۰۰۰</td>
                            </tr>
                            <tr>
                                <td class="indent-1">سود (زیان) انباشته</td>
                                <td class="text-left mono-number">۵۶,۳۵۰</td>
                            </tr>
                            <tr class="subtotal-row">
                                <td class="font-weight-bold">جمع حقوق صاحبان سهام</td>
                                <td class="text-left font-weight-bold mono-number text-cyan">۱۱۱,۳۵۰</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="grand-total-row">
                                <td class="font-weight-bold">جمع بدهی‌ها و حقوق صاحبان سهام</td>
                                <td class="text-left font-weight-bold mono-number text-cyan text-h6">۲۳۳,۲۰۰</td>
                            </tr>
                        </tfoot>
                    </table>
                </v-col>
            </v-row>
        </v-card>
    </div>
</template>

<style scoped>
.report-page {
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

.header-actions {
    display: flex;
    align-items: center;
    gap: 12px;
}

.content-card {
    background: rgba(15, 23, 42, 0.6);
    border: 1px solid rgba(255, 255, 255, 0.05);
    border-radius: 16px;
    padding: 24px;
}

.report-header {
    border-bottom: 2px solid rgba(34, 211, 238, 0.2);
    padding-bottom: 16px;
    margin-bottom: 24px;
}

@media (min-width: 960px) {
    .border-right-md {
        border-left: 1px solid rgba(255, 255, 255, 0.05);
        padding-left: 24px !important;
        margin-left: -1px;
    }

    .liabilities-col {
        padding-right: 24px !important;
    }
}

.side-title {
    font-size: 16px;
    text-align: center;
    background: rgba(255, 255, 255, 0.02);
    padding: 8px;
    border-radius: 8px;
}

.financial-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 14px;
}

.financial-table td {
    padding: 10px;
    border-bottom: 1px dashed rgba(255, 255, 255, 0.05);
}

.indent-1 {
    padding-right: 20px !important;
    color: #cbd5e1;
}

.subtotal-row td {
    background: rgba(255, 255, 255, 0.02);
    border-bottom: 1px solid rgba(255, 255, 255, 0.1) !important;
    padding: 12px 10px;
}

tfoot {
    margin-top: auto;
}

.grand-total-row td {
    background: rgba(34, 211, 238, 0.08);
    border-top: 2px solid rgba(34, 211, 238, 0.5);
    padding: 16px 12px;
    height: 60px;
    vertical-align: middle;
}

.mono-number {
    font-family: inherit;
    letter-spacing: 1px;
}
</style>
