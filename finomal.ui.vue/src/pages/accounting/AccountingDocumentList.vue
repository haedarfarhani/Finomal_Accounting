<template>
  <div class="accounting-page" dir="rtl">
    <!-- ===== 1. Page Header ===== -->
    <header class="page-header mb-6">
      <div class="header-content">
        <h1 class="page-title">
          <v-icon color="cyan-accent-3" class="ml-2" size="30">mdi-file-document-multiple-outline</v-icon>
          لیست اسناد حسابداری
        </h1>
        <p class="page-subtitle">سیستم جامع مرور، مدیریت، فیلتر پیشرفته، تایید گروهی و گزارش‌گیری اسناد مالی فینومال</p>
      </div>
      <div class="header-actions d-flex gap-3">
        <v-btn color="cyan-darken-2" to="/accounting-document/create" prepend-icon="mdi-plus" elevation="2"
          class="action-btn text-white font-weight-bold px-5" rounded="lg">
          ثبت سند جدید
        </v-btn>
      </div>
    </header>

    <!-- ===== 2. High-Fidelity Stats Panel ===== -->
    <v-row class="mb-6 stats-row">
      <v-col cols="12" sm="6" md="3">
        <v-card class="stat-card glass-card" elevation="0">
          <div class="stat-icon-wrapper bg-blue-glow">
            <v-icon color="blue-lighten-2" size="24">mdi-file-document-outline</v-icon>
          </div>
          <div class="stat-info">
            <p class="stat-title">تعداد کل اسناد</p>
            <h3 class="stat-value font-fa">{{ totalDocumentsCount }} <span class="text-caption text-grey">سند</span></h3>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" sm="6" md="3">
        <v-card class="stat-card glass-card" elevation="0">
          <div class="stat-icon-wrapper bg-amber-glow">
            <v-icon color="amber-lighten-2" size="24">mdi-clock-outline</v-icon>
          </div>
          <div class="stat-info">
            <p class="stat-title">اسناد موقت و معلق</p>
            <h3 class="stat-value font-fa">{{ draftDocumentsCount }} <span class="text-caption text-grey">سند</span></h3>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" sm="6" md="3">
        <v-card class="stat-card glass-card" elevation="0">
          <div class="stat-icon-wrapper bg-success-glow">
            <v-icon color="emerald-lighten-2" size="24">mdi-shield-check-outline</v-icon>
          </div>
          <div class="stat-info">
            <p class="stat-title">اسناد تاییدشده و قطعی</p>
            <h3 class="stat-value font-fa">{{ finalizedDocumentsCount }} <span class="text-caption text-grey">سند</span></h3>
          </div>
        </v-card>
      </v-col>
      <v-col cols="12" sm="6" md="3">
        <v-card class="stat-card glass-card" elevation="0">
          <div class="stat-icon-wrapper bg-cyan-glow">
            <v-icon color="cyan-lighten-2" size="24">mdi-calculator</v-icon>
          </div>
          <div class="stat-info">
            <p class="stat-title">گردش کل مالی اسناد</p>
            <h3 class="stat-value font-fa text-cyan-accent-2" style="font-size: 1.15rem;">{{ formatMoney(totalDebitSum) }} <span class="text-caption text-grey">ریال</span></h3>
          </div>
        </v-card>
      </v-col>
    </v-row>

    <!-- ===== 3. Main Data Card ===== -->
    <v-card class="content-card mb-6" elevation="0">
      <!-- Toolbar containing basic actions -->
      <div class="table-toolbar px-4 py-4 border-b d-flex align-center justify-space-between flex-wrap gap-4">
        <!-- Right side: Search & Advanced toggle -->
        <div class="d-flex align-center flex-grow-1 gap-3" style="max-width: 600px;">
          <div class="search-box-wrap flex-grow-1">
            <v-text-field v-model="searchQuery" prepend-inner-icon="mdi-magnify"
              placeholder="جستجو در شماره، شرح، کد یا نام حساب..." variant="outlined" density="compact" hide-details
              rounded="lg" class="dark-field" clearable @update:model-value="onFiltersChange"></v-text-field>
          </div>
          <v-btn variant="outlined" :color="showAdvancedFilters ? 'cyan' : 'grey-lighten-1'"
            prepend-icon="mdi-filter-variant" class="filter-toggle-btn rounded-lg font-fa text-body-2" @click="showAdvancedFilters = !showAdvancedFilters">
            فیلترهای پیشرفته
            <v-icon end size="16">{{ showAdvancedFilters ? 'mdi-chevron-up' : 'mdi-chevron-down' }}</v-icon>
          </v-btn>
        </div>

        <!-- Left side: Bulk operations & Export -->
        <div class="d-flex align-center gap-3">
          <v-btn-toggle v-if="selectedIds.length > 0" class="bulk-toggle-btn rounded-lg mr-2" density="compact">
            <v-menu transition="slide-y-transition" location="bottom end">
              <template v-slot:activator="{ props }">
                <v-btn color="cyan-accent-4" variant="flat" class="text-white font-weight-bold" v-bind="props" append-icon="mdi-chevron-down">
                  عملیات گروهی ({{ selectedIds.length }})
                </v-btn>
              </template>
              <v-list bg-color="#080c14" class="dark-menu-list font-fa" rounded="lg" border>
                <v-list-item prepend-icon="mdi-check-all" title="تایید گروهی اسناد" @click="bulkAction('approve')"></v-list-item>
                <v-list-item prepend-icon="mdi-lock-outline" title="قطعی کردن گروهی اسناد" @click="bulkAction('finalize')"></v-list-item>
                <v-list-item prepend-icon="mdi-close-circle-outline" title="ابطال گروهی اسناد" base-color="error" @click="bulkAction('cancel')"></v-list-item>
                <v-divider></v-divider>
                <v-list-item prepend-icon="mdi-printer" title="چاپ گروهی اسناد" @click="bulkAction('print')"></v-list-item>
                <v-list-item prepend-icon="mdi-file-pdf-box" title="خروجی PDF گروهی" @click="bulkAction('pdf')"></v-list-item>
              </v-list>
            </v-menu>
          </v-btn-toggle>

          <v-btn variant="outlined" color="grey-lighten-1" prepend-icon="mdi-microsoft-excel" class="rounded-lg text-body-2 font-fa" @click="exportToExcel">
            خروجی Excel
          </v-btn>
          <v-btn variant="outlined" color="grey-lighten-1" prepend-icon="mdi-file-pdf-box" class="rounded-lg text-body-2 font-fa" @click="exportAllToPdf">
            خروجی PDF
          </v-btn>
        </div>
      </div>

      <!-- Advanced Filter Panel (Collapsible) -->
      <v-expand-transition>
        <div v-show="showAdvancedFilters" class="advanced-filters-panel bg-slate-900 border-b px-5 py-5">
          <v-row class="gap-y-4">
            <v-col cols="12" sm="6" md="3">
              <v-select v-model="filters.status" :items="statusOptions" label="وضعیت سند" variant="outlined"
                density="compact" class="dark-field" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-select>
            </v-col>
            <v-col cols="12" sm="6" md="3">
              <v-select v-model="filters.type" :items="typeOptions" label="نوع سند" variant="outlined"
                density="compact" class="dark-field" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-select>
            </v-col>
            <v-col cols="12" sm="6" md="3">
              <v-text-field v-model="filters.creator" label="کاربر ثبت‌کننده" variant="outlined"
                density="compact" class="dark-field" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="3">
              <v-select v-model="filters.fiscalYear" :items="fiscalYearOptions" label="سال مالی" variant="outlined"
                density="compact" class="dark-field" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-select>
            </v-col>

            <v-col cols="12" sm="6" md="3">
              <v-text-field v-model="filters.docNoStart" label="از شماره سند" variant="outlined"
                density="compact" class="dark-field font-fa" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="3">
              <v-text-field v-model="filters.docNoEnd" label="تا شماره سند" variant="outlined"
                density="compact" class="dark-field font-fa" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="3">
              <v-text-field v-model="filters.dateStart" label="تاریخ سند از" variant="outlined"
                placeholder="۱۴۰۳/۰۱/۰۱" density="compact" class="dark-field font-fa" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="3">
              <v-text-field v-model="filters.dateEnd" label="تاریخ سند تا" variant="outlined"
                placeholder="۱۴۰۳/۱۲/۲۹" density="compact" class="dark-field font-fa" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-text-field>
            </v-col>

            <v-col cols="12" sm="6" md="3">
              <v-text-field v-model="filters.project" label="پروژه" variant="outlined"
                density="compact" class="dark-field" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="3">
              <v-text-field v-model="filters.costCenter" label="مرکز هزینه" variant="outlined"
                density="compact" class="dark-field" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="3">
              <v-select v-model="filters.balanced" :items="balancedOptions" label="وضعیت تراز" variant="outlined"
                density="compact" class="dark-field" hide-details rounded="lg" clearable @update:model-value="onFiltersChange"></v-select>
            </v-col>
            <v-col cols="12" sm="6" md="3" class="d-flex align-center justify-end">
              <v-btn variant="text" color="red-lighten-2" prepend-icon="mdi-filter-off-outline" class="font-fa rounded-lg text-body-2" @click="resetFilters">
                حذف فیلترها
              </v-btn>
            </v-col>
          </v-row>
        </div>
      </v-expand-transition>

      <!-- Main Document Tree/Grid Table -->
      <div class="table-responsive">
        <table class="custom-table w-100">
          <thead>
            <tr>
              <th class="th-checkbox text-center" style="width: 50px;">
                <v-checkbox-btn v-model="selectAllChecked" color="cyan" density="compact" @update:model-value="toggleSelectAll"></v-checkbox-btn>
              </th>
              <th class="th-expand" style="width: 40px;"></th>
              <th class="th-code">شماره سند</th>
              <th>تاریخ سند</th>
              <th>تاریخ ثبت</th>
              <th>نوع سند</th>
              <th>شرح کاربردی سند</th>
              <th class="text-left">بدهکار (ریال)</th>
              <th class="text-left">بستانکار (ریال)</th>
              <th class="text-center">وضعیت تراز</th>
              <th class="text-center">وضعیت سند</th>
              <th class="text-center">عملیات</th>
            </tr>
          </thead>
          <tbody>
            <template v-for="doc in paginatedDocuments" :key="doc.id">
              <!-- Document Header Row -->
              <tr class="table-row" :class="{ 'row-expanded': doc.expanded, 'row-selected': isSelected(doc.id) }" @click="toggleExpand(doc)">
                <!-- Checkbox -->
                <td class="text-center" @click.stop>
                  <v-checkbox-btn :model-value="isSelected(doc.id)" color="cyan" density="compact" @update:model-value="toggleSelectRow(doc.id)"></v-checkbox-btn>
                </td>
                
                <!-- Expand button -->
                <td class="text-center">
                  <v-btn icon size="x-small" variant="text" color="grey-lighten-1" class="btn-expand-arrow" :class="{ 'arrow-rotated': doc.expanded }">
                    <v-icon>mdi-chevron-down</v-icon>
                  </v-btn>
                </td>

                <!-- Doc Number -->
                <td class="mono-number font-weight-bold text-cyan-accent-3">{{ doc.docNo }}</td>
                
                <!-- Date -->
                <td class="mono-number text-grey-lighten-1">{{ doc.date }}</td>
                <td class="mono-number text-grey-darken-1" style="font-size: 11px;">{{ doc.createdAt }}</td>
                
                <!-- Type -->
                <td>
                  <span class="type-badge">{{ getDocTypeLabel(doc.type) }}</span>
                </td>

                <!-- Description -->
                <td class="font-weight-medium doc-desc" :title="doc.description">{{ doc.description }}</td>
                
                <!-- Debit -->
                <td class="text-left mono-number text-success font-weight-bold">{{ formatMoney(doc.totalDebit) }}</td>
                
                <!-- Credit -->
                <td class="text-left mono-number text-grey-lighten-2">{{ formatMoney(doc.totalCredit) }}</td>

                <!-- Balanced status -->
                <td class="text-center">
                  <v-tooltip :text="doc.balanced ? 'سند کاملا تراز است' : 'عدم تراز در مبلغ بدهکار و بستانکار!'">
                    <template v-slot:activator="{ props }">
                      <v-chip v-bind="props" size="x-small" :color="doc.balanced ? 'success' : 'error'" variant="tonal" class="font-weight-bold rounded-lg px-2">
                        <v-icon start size="11" class="ml-1">{{ doc.balanced ? 'mdi-check-circle' : 'mdi-alert-circle' }}</v-icon>
                        {{ doc.balanced ? 'تراز' : 'نابالغ' }}
                      </v-chip>
                    </template>
                  </v-tooltip>
                </td>

                <!-- Status Badge -->
                <td class="text-center">
                  <v-chip size="x-small" :color="getStatusColor(doc.status)" variant="flat" class="font-weight-black text-white px-2">
                    {{ getStatusLabel(doc.status) }}
                  </v-chip>
                </td>

                <!-- Row Actions -->
                <td class="text-center" @click.stop>
                  <div class="d-flex align-center justify-center gap-1">
                    <v-btn icon="mdi-eye-outline" size="x-small" variant="text" color="info" title="مشاهده جزئیات کامل" @click="showDocDetailsModal(doc)"></v-btn>
                    
                    <v-btn icon="mdi-pencil-outline" size="x-small" variant="text" color="primary" title="ویرایش سند" :disabled="doc.status === 'finalized' || doc.status === 'cancelled'" @click="editDocument(doc.id)"></v-btn>
                    
                    <v-menu location="left">
                      <template v-slot:activator="{ props }">
                        <v-btn icon="mdi-dots-vertical" size="x-small" variant="text" color="grey" v-bind="props"></v-btn>
                      </template>
                      <v-list bg-color="#080c14" density="compact" class="dark-menu-list font-fa text-body-2" border rounded="lg">
                        <v-list-item prepend-icon="mdi-printer" title="چاپ سند" @click="printDoc(doc)"></v-list-item>
                        <v-list-item prepend-icon="mdi-file-pdf-box" title="خروجی PDF" @click="exportDocToPdf(doc)"></v-list-item>
                        <v-list-item prepend-icon="mdi-content-copy" title="کپی و شبیه‌سازی سند" @click="cloneDoc(doc)"></v-list-item>
                        <v-list-item prepend-icon="mdi-history" title="مشاهده لاگ تغییرات Audit" @click="showAuditLog(doc)"></v-list-item>
                        <v-divider v-if="doc.status !== 'finalized'"></v-divider>
                        <v-list-item v-if="doc.status !== 'finalized' && doc.status !== 'cancelled'" prepend-icon="mdi-close-circle-outline" title="ابطال سند" base-color="warning" @click="cancelDoc(doc)"></v-list-item>
                        <v-list-item v-if="doc.status === 'draft'" prepend-icon="mdi-delete" title="حذف سند" base-color="error" @click="deleteDoc(doc.id)"></v-list-item>
                      </v-list>
                    </v-menu>
                  </div>
                </td>
              </tr>

              <!-- Expanded Details Row (Debit and credit ledger entries) -->
              <tr v-show="doc.expanded" class="expanded-detail-row" @click.stop>
                <td colspan="12" class="expanded-cell-container py-3 px-6">
                  <div class="expanded-details-wrapper glass-card-nested py-3 px-4 rounded-lg">
                    <h4 class="expanded-title mb-3 d-flex align-center justify-space-between">
                      <span>
                        <v-icon size="16" color="cyan" class="ml-1">mdi-arrow-split-vertical</v-icon>
                        آرتیکل‌ها و ردیف‌های تشکیل‌دهنده سند موازنه
                      </span>
                      <span class="text-caption text-grey font-fa">تعداد آرتیکل‌ها: {{ doc.items.length }} ردیف</span>
                    </h4>
                    
                    <table class="detail-items-table w-100">
                      <thead>
                        <tr>
                          <th style="width: 60px;" class="text-center">ردیف</th>
                          <th style="width: 120px;">کد حساب</th>
                          <th>نام سرفصل حسابداری</th>
                          <th>شرح آرتیکل</th>
                          <th class="text-left" style="width: 150px;">بدهکار (ریال)</th>
                          <th class="text-left" style="width: 150px;">بستانکار (ریال)</th>
                          <th>مرکز هزینه</th>
                          <th>پروژه / تفصیلی</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="(entry, index) in doc.items" :key="entry.id" class="detail-row">
                          <td class="text-center mono-number font-fa">{{ index + 1 }}</td>
                          <td class="mono-number font-weight-bold text-cyan-accent-1">{{ entry.accountCode }}</td>
                          <td class="font-weight-medium">{{ entry.accountName }}</td>
                          <td class="text-grey-lighten-1">{{ entry.note || '—' }}</td>
                          <td class="text-left mono-number text-success font-weight-semibold">{{ entry.debit > 0 ? formatMoney(entry.debit) : '—' }}</td>
                          <td class="text-left mono-number text-grey-lighten-2">{{ entry.credit > 0 ? formatMoney(entry.credit) : '—' }}</td>
                          <td class="text-caption text-grey">{{ entry.costCenter || '—' }}</td>
                          <td class="text-caption text-grey">{{ entry.project || '—' }}</td>
                        </tr>
                        <!-- Subtotal Row -->
                        <tr class="subtotal-row font-weight-bold">
                          <td colspan="4" class="text-right py-2">جمع کل موازنه آرتیکل‌ها:</td>
                          <td class="text-left text-success mono-number py-2">{{ formatMoney(doc.totalDebit) }}</td>
                          <td class="text-left text-grey-lighten-2 mono-number py-2">{{ formatMoney(doc.totalCredit) }}</td>
                          <td colspan="2"></td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </td>
              </tr>
            </template>
            
            <tr v-if="filteredDocuments.length === 0" class="no-records-row">
              <td colspan="12">
                <div class="empty-state-table py-12 text-center">
                  <v-icon size="64" color="grey-darken-2" class="mb-4">mdi-file-hidden</v-icon>
                  <h3 class="text-h6 text-grey-lighten-1">سند حسابداری مطابق فیلترهای شما یافت نشد!</h3>
                  <p class="text-body-2 text-grey">لطفاً عبارت جستجو یا فیلترهای پیشرفته را تغییر دهید.</p>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pagination and Record Counter -->
      <div class="table-pagination-footer px-5 py-4 border-t d-flex align-center justify-space-between flex-wrap gap-4 no-print">
        <div class="d-flex align-center gap-3">
          <span class="text-body-2 text-grey-lighten-1">تعداد نمایش در صفحه:</span>
          <v-select v-model="pageSize" :items="[5, 10, 20, 50]" variant="outlined" density="compact"
            class="dark-field font-fa inline-select" style="max-width: 80px;" hide-details rounded="lg" @update:model-value="currentPage = 1"></v-select>
          <span class="text-caption text-grey font-fa">نمایش {{ startIndex + 1 }} تا {{ Math.min(endIndex, filteredDocuments.length) }} از مجموع {{ filteredDocuments.length }} سند</span>
        </div>

        <div class="pagination-buttons font-fa">
          <v-pagination v-model="currentPage" :length="totalPages" total-visible="4" size="small" active-color="cyan" rounded="lg" density="comfortable"></v-pagination>
        </div>
      </div>
    </v-card>

    <!-- ===== 4. Audit Log Dialog (Audit Trail representation) ===== -->
    <v-dialog v-model="auditModal.show" max-width="650px" transition="dialog-bottom-transition">
      <div class="glass-card audit-modal-wrapper">
        <div class="dialog-header px-5 py-4 border-b">
          <div class="dialog-title text-h6 font-weight-bold d-flex align-center">
            <v-icon size="24" color="cyan" class="ml-2">mdi-history</v-icon>
            لاگ تغییرات و تاریخچه ممیزی سند {{ auditModal.doc?.docNo }}
          </div>
          <button class="btn-close-dialog" @click="auditModal.show = false">
            <v-icon>mdi-close</v-icon>
          </button>
        </div>
        <div class="dialog-body px-5 py-5 font-fa text-body-2">
          <div class="audit-timeline">
            <div class="timeline-item d-flex gap-4 mb-4">
              <div class="timeline-badge bg-blue-darken-3 text-white">
                <v-icon size="14">mdi-plus</v-icon>
              </div>
              <div class="timeline-content">
                <div class="font-weight-bold text-white">ایجاد و ثبت اولیه سند</div>
                <div class="text-caption text-grey mt-1">توسط کاربر: <strong>{{ auditModal.doc?.createdBy }}</strong></div>
                <div class="text-caption text-grey mt-0.5">در زمان: {{ auditModal.doc?.createdAt }}</div>
              </div>
            </div>

            <div v-if="auditModal.doc?.updatedBy" class="timeline-item d-flex gap-4 mb-4">
              <div class="timeline-badge bg-amber-darken-3 text-white">
                <v-icon size="14">mdi-pencil</v-icon>
              </div>
              <div class="timeline-content">
                <div class="font-weight-bold text-white">آخرین ویرایش و اصلاح آرتیکل‌ها</div>
                <div class="text-caption text-grey mt-1">توسط کاربر: <strong>{{ auditModal.doc?.updatedBy }}</strong></div>
                <div class="text-caption text-grey mt-0.5">در زمان: {{ auditModal.doc?.updatedAt }}</div>
              </div>
            </div>

            <div v-if="auditModal.doc?.approvedBy" class="timeline-item d-flex gap-4 mb-4">
              <div class="timeline-badge bg-success-darken-3 text-white">
                <v-icon size="14">mdi-shield-check</v-icon>
              </div>
              <div class="timeline-content">
                <div class="font-weight-bold text-white">تایید و امضای رسمی سند مالی</div>
                <div class="text-caption text-grey mt-1">توسط مدیر مالی: <strong>{{ auditModal.doc?.approvedBy }}</strong></div>
                <div class="text-caption text-grey mt-0.5">در زمان: {{ auditModal.doc?.approvedAt || '۱۴۰۳/۱۱/۳۰ ۱۷:۲۰:۰۰' }}</div>
              </div>
            </div>

            <div v-if="auditModal.doc?.status === 'cancelled'" class="timeline-item d-flex gap-4">
              <div class="timeline-badge bg-error-darken-3 text-white">
                <v-icon size="14">mdi-close-circle</v-icon>
              </div>
              <div class="timeline-content">
                <div class="font-weight-bold text-red-lighten-2">ابطال کامل سند مالی</div>
                <div class="text-caption text-grey mt-1">علت ابطال: لغو فاکتور فروش یا اصلاحیه دوره مالی</div>
                <div class="text-caption text-grey mt-0.5">وضعیت آرتیکل‌ها: معلق شده و بدون تاثیر در بیلان دفاتر کل</div>
              </div>
            </div>
          </div>
        </div>
        <div class="dialog-footer px-5 py-4 border-t d-flex justify-end">
          <v-btn variant="tonal" class="rounded-lg font-fa" @click="auditModal.show = false">بستن پنجره</v-btn>
        </div>
      </div>
    </v-dialog>

    <!-- ===== 5. Full Document Details Dialog ===== -->
    <v-dialog v-model="detailsModal.show" max-width="850px" transition="dialog-bottom-transition">
      <div class="glass-card details-modal-wrapper font-fa">
        <div class="dialog-header px-5 py-4 border-b">
          <div class="dialog-title text-h6 font-weight-bold d-flex align-center">
            <v-icon size="24" color="cyan" class="ml-2">mdi-text-box-search-outline</v-icon>
            مشاهده اطلاعات تفصیلی سند {{ detailsModal.doc?.docNo }}
          </div>
          <button class="btn-close-dialog" @click="detailsModal.show = false">
            <v-icon>mdi-close</v-icon>
          </button>
        </div>
        
        <div class="dialog-body px-5 py-5 text-body-2">
          <!-- Summary grid -->
          <v-row class="mb-5 bg-slate-950 p-4 rounded-lg border-1 border-slate-800">
            <v-col cols="6" sm="4" md="3">
              <div class="detail-label text-grey">شماره سند:</div>
              <div class="detail-val font-weight-bold text-white mono-number mt-1">{{ detailsModal.doc?.docNo }}</div>
            </v-col>
            <v-col cols="6" sm="4" md="3">
              <div class="detail-label text-grey">تاریخ سند:</div>
              <div class="detail-val text-white mono-number mt-1">{{ detailsModal.doc?.date }}</div>
            </v-col>
            <v-col cols="6" sm="4" md="3">
              <div class="detail-label text-grey">سال مالی:</div>
              <div class="detail-val text-white mono-number mt-1">۱۴۰۳</div>
            </v-col>
            <v-col cols="6" sm="4" md="3">
              <div class="detail-label text-grey">نوع سند:</div>
              <div class="detail-val text-white mt-1">{{ getDocTypeLabel(detailsModal.doc?.type) }}</div>
            </v-col>
            <v-col cols="12" sm="8" md="6">
              <div class="detail-label text-grey">شرح کلی سند:</div>
              <div class="detail-val text-white mt-1 font-weight-medium">{{ detailsModal.doc?.description }}</div>
            </v-col>
            <v-col cols="6" sm="4" md="3">
              <div class="detail-label text-grey">وضعیت تراز:</div>
              <div class="detail-val mt-1">
                <v-chip size="x-small" :color="detailsModal.doc?.balanced ? 'success' : 'error'" variant="flat">
                  {{ detailsModal.doc?.balanced ? 'تراز شده' : 'عدم تراز' }}
                </v-chip>
              </div>
            </v-col>
            <v-col cols="6" sm="4" md="3">
              <div class="detail-label text-grey">وضعیت سند:</div>
              <div class="detail-val mt-1">
                <v-chip size="x-small" :color="getStatusColor(detailsModal.doc?.status)" variant="flat">
                  {{ getStatusLabel(detailsModal.doc?.status) }}
                </v-chip>
              </div>
            </v-col>
          </v-row>

          <h4 class="text-subtitle-1 text-white mb-3 font-weight-bold">آرتیکل‌های سند حسابداری</h4>
          
          <div class="table-scroll-wrap rounded-lg border-1 border-slate-800">
            <table class="detail-items-table w-100">
              <thead>
                <tr>
                  <th style="width: 50px;" class="text-center">ردیف</th>
                  <th style="width: 100px;">کد حساب</th>
                  <th>حساب معین / تفصیلی</th>
                  <th>شرح آرتیکل</th>
                  <th class="text-left" style="width: 140px;">بدهکار (ریال)</th>
                  <th class="text-left" style="width: 140px;">بستانکار (ریال)</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(entry, index) in detailsModal.doc?.items" :key="entry.id" class="detail-row">
                  <td class="text-center mono-number">{{ index + 1 }}</td>
                  <td class="mono-number text-cyan-accent-1">{{ entry.accountCode }}</td>
                  <td>
                    <div class="font-weight-medium">{{ entry.accountName }}</div>
                    <div class="text-caption text-grey" v-if="entry.project">پروژه: {{ entry.project }} | تفصیلی: {{ entry.costCenter }}</div>
                  </td>
                  <td class="text-grey-lighten-2" style="max-width: 200px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">{{ entry.note }}</td>
                  <td class="text-left text-success mono-number">{{ entry.debit > 0 ? formatMoney(entry.debit) : '—' }}</td>
                  <td class="text-left text-grey-lighten-2 mono-number">{{ entry.credit > 0 ? formatMoney(entry.credit) : '—' }}</td>
                </tr>
                <tr class="subtotal-row font-weight-bold">
                  <td colspan="4" class="text-right py-2">جمع کل آرتیکل‌ها:</td>
                  <td class="text-left text-success mono-number py-2">{{ formatMoney(detailsModal.doc?.totalDebit) }}</td>
                  <td class="text-left text-grey-lighten-2 mono-number py-2">{{ formatMoney(detailsModal.doc?.totalCredit) }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div class="dialog-footer px-5 py-4 border-t d-flex justify-space-between align-center">
          <div class="d-flex gap-2">
            <v-btn size="small" color="blue" prepend-icon="mdi-printer" class="rounded-lg" @click="printDoc(detailsModal.doc)">چاپ سند</v-btn>
            <v-btn size="small" color="red" prepend-icon="mdi-file-pdf-box" class="rounded-lg" @click="exportDocToPdf(detailsModal.doc)">خروجی PDF</v-btn>
          </div>
          <v-btn variant="tonal" class="rounded-lg" @click="detailsModal.show = false">بستن</v-btn>
        </div>
      </div>
    </v-dialog>

    <!-- ===== 6. Premium Toast Notification ===== -->
    <transition name="toast">
      <div v-if="toast.show" class="toast-premium" :class="'toast-' + toast.type">
        <div class="toast-content">
          <div class="toast-icon">
            <v-icon size="16">{{ toast.type === 'success' ? 'mdi-check-bold' : 'mdi-alert' }}</v-icon>
          </div>
          <div class="toast-text font-fa">{{ toast.text }}</div>
        </div>
        <div class="toast-timer-bar"></div>
      </div>
    </transition>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

// --- Mock Database for Accounting Documents ---
const documents = ref([
  {
    id: 1,
    docNo: 'ACC-1001',
    date: '۱۴۰۳/۱۱/۲۸',
    createdAt: '۱۴۰۳/۱۱/۲۸ ۱۰:۱۵:۳۰',
    type: 'payroll',
    description: 'ثبت حقوق و دستمزد بهمن ماه دفتر مرکزی',
    totalDebit: 2500000000,
    totalCredit: 2500000000,
    balanced: true,
    status: 'finalized',
    fiscalYear: '۱۴۰۳',
    createdBy: 'علی رضایی (مدیر مالی)',
    approvedBy: 'حسن حسینی (رئیس حسابداری)',
    approvedAt: '۱۴۰۳/۱۱/۲۸ ۱۶:۳۰:۰۰',
    expanded: false,
    items: [
      { id: 101, accountCode: '7101', accountName: 'هزینه حقوق و دستمزد پرسنل', note: 'حقوق پایه و مزایای بهمن پرسنل دفتر مرکزی', debit: 2500000000, credit: 0, costCenter: 'امور اداری', project: 'دفتر مرکزی' },
      { id: 102, accountCode: '3303', accountName: 'حقوق و دستمزد پرداختنی', note: 'حقوق پرداختنی بهمن ماه پرسنل', debit: 0, credit: 1850000000, costCenter: 'امور اداری', project: 'دفتر مرکزی' },
      { id: 103, accountCode: '3304', accountName: 'سازمان تامین اجتماعی (حق بیمه)', note: 'حق بیمه سهم کارگر و کارفرما بهمن', debit: 0, credit: 500000000, costCenter: 'امور اداری', project: 'دفتر مرکزی' },
      { id: 104, accountCode: '3305', accountName: 'اداره امور مالیاتی (مالیات حقوق)', note: 'مالیات حقوق کسر شده بهمن ماه پرسنل', debit: 0, credit: 1500000000, costCenter: 'امور اداری', project: 'دفتر مرکزی' }
    ]
  },
  {
    id: 2,
    docNo: 'ACC-1002',
    date: '۱۴۰۳/۱۱/۲۹',
    createdAt: '۱۴۰۳/۱۱/۲۹ ۱۱:۲۰:۰۰',
    type: 'payment',
    description: 'پرداخت تنخواه گردان دفتر مرکزی بابت ملزومات اداری و آبدارخانه',
    totalDebit: 150000000,
    totalCredit: 150000000,
    balanced: true,
    status: 'approved',
    fiscalYear: '۱۴۰۳',
    createdBy: 'زهرا مهدوی (حسابدار)',
    approvedBy: 'علی رضایی (مدیر مالی)',
    approvedAt: '۱۴۰۳/۱۱/۲۹ ۱۴:۱۵:۰۰',
    updatedBy: 'زهرا مهدوی',
    updatedAt: '۱۴۰۳/۱۱/۲۹ ۱۲:۰۵:۰۰',
    expanded: false,
    items: [
      { id: 201, accountCode: '8105', accountName: 'هزینه ملزومات اداری', note: 'خرید کاغذ، خودکار و زونکن‌های بایگانی', debit: 85000000, credit: 0, costCenter: 'امور اداری', project: 'پشتیبانی' },
      { id: 202, accountCode: '8112', accountName: 'هزینه پذیرایی و آبدارخانه', note: 'خرید چای، قند و اقلام مصرفی آبدارخانه', debit: 65000000, credit: 0, costCenter: 'امور پشتیبانی', project: 'پشتیبانی' },
      { id: 203, accountCode: '1103', accountName: 'تنخواه گردان دفتر مرکزی', note: 'شارژ و تسویه تنخواه تنخواهدار آقای محمدی', debit: 0, credit: 150000000, costCenter: 'تنخواه‌گردان', project: 'پشتیبانی' }
    ]
  },
  {
    id: 3,
    docNo: 'ACC-1003',
    date: '۱۴۰۳/۱۱/۳۰',
    createdAt: '۱۴۰۳/۱۱/۳۰ ۱۴:۴۵:۰۰',
    type: 'sales',
    description: 'ثبت فاکتور فروش محصول شماره ص-۴۹۲ به شرکت فولاد مبارکه اصفهان',
    totalDebit: 4500000000,
    totalCredit: 4500000000,
    balanced: true,
    status: 'draft',
    fiscalYear: '۱۴۰۳',
    createdBy: 'امیر قاسمی (مسئول فروش)',
    expanded: false,
    items: [
      { id: 301, accountCode: '1401', accountName: 'حساب‌های دریافتنی تجاری مشتریان', note: 'فروش اعتباری کالا به فولاد مبارکه اصفهان', debit: 4500000000, credit: 0, costCenter: 'دپارتمان فروش', project: 'پروژه فولاد' },
      { id: 302, accountCode: '6101', accountName: 'درآمد حاصل از فروش محصول تجاری', note: 'فروش فاکتور ص-۴۹۲', debit: 0, credit: 4128440000, costCenter: 'فروش کالا', project: 'پروژه فولاد' },
      { id: 303, accountCode: '3307', accountName: 'مالیات بر ارزش افزوده پرداختنی', note: '۹٪ ارزش افزوده فاکتور فروش', debit: 0, credit: 371560000, costCenter: 'امور مالیاتی', project: 'پروژه فولاد' }
    ]
  },
  {
    id: 4,
    docNo: 'ACC-1004',
    date: '۱۴۰۳/۱۱/۳۰',
    createdAt: '۱۴۰۳/۱۱/۳۰ ۱۵:۱۰:۰۰',
    type: 'manual',
    description: 'اصلاح ثبت اشتباه هزینه اجاره ساختمان بهمن ماه و انتقال به پیش‌پرداخت',
    totalDebit: 200000000,
    totalCredit: 198000000,
    balanced: false,
    status: 'draft',
    fiscalYear: '۱۴۰۳',
    createdBy: 'علی رضایی (مدیر مالی)',
    expanded: false,
    items: [
      { id: 401, accountCode: '1804', accountName: 'پیش‌پرداخت اجاره ساختمان', note: 'انتقال سهم پیش‌پرداخت اجاره بهمن ماه', debit: 200000000, credit: 0, costCenter: 'امور اداری', project: 'ساختمان مرکزی' },
      { id: 402, accountCode: '8102', accountName: 'هزینه اجاره اداری ساختمان', note: 'اصلاح اشتباه سند شماره ۹۸۷', debit: 0, credit: 198000000, costCenter: 'امور اداری', project: 'ساختمان مرکزی' }
    ]
  },
  {
    id: 5,
    docNo: 'ACC-1005',
    date: '۱۴۰۳/۱۱/۳۰',
    createdAt: '۱۴۰۳/۱۱/۳۰ ۱۵:۳۰:۰۰',
    type: 'purchase',
    description: 'خرید مواد اولیه فاکتور شماره خ-۸۸۲ از پتروشیمی شازند',
    totalDebit: 8500000000,
    totalCredit: 8500000000,
    balanced: true,
    status: 'cancelled',
    fiscalYear: '۱۴۰۳',
    createdBy: 'زهرا مهدوی (حسابدار)',
    expanded: false,
    items: [
      { id: 501, accountCode: '1501', accountName: 'کالا و مواد اولیه در انبار اصلی', note: 'خرید مواد شیمیایی گرید A پتروشیمی', debit: 8500000000, credit: 0, costCenter: 'تامین و انبار', project: 'شازند' },
      { id: 502, accountCode: '3201', accountName: 'حساب‌های پرداختنی تجاری تامین‌کنندگان', note: 'تسویه اعتباری با پتروشیمی شازند', debit: 0, credit: 8500000000, costCenter: 'تامین مالی', project: 'شازند' }
    ]
  }
])

// --- Operational States ---
const searchQuery = ref('')
const showAdvancedFilters = ref(false)
const selectedIds = ref([])
const selectAllChecked = ref(false)
const toast = ref({ show: false, text: '', type: 'success' })

// Filter values
const filters = ref({
  status: null,
  type: null,
  creator: '',
  fiscalYear: '۱۴۰۳',
  docNoStart: '',
  docNoEnd: '',
  dateStart: '',
  dateEnd: '',
  project: '',
  costCenter: '',
  balanced: null
})

// Pagination
const currentPage = ref(1)
const pageSize = ref(10)

// Dialog Modals
const auditModal = ref({ show: false, doc: null })
const detailsModal = ref({ show: false, doc: null })

// --- Options Constants ---
const statusOptions = [
  { value: 'draft', title: 'موقت (چرک‌نویس)' },
  { value: 'approved', title: 'در انتظار تایید / تایید شده' },
  { value: 'finalized', title: 'قطعی و قفل‌شده' },
  { value: 'cancelled', title: 'ابطال شده' }
]

const typeOptions = [
  { value: 'manual', title: 'سند دستی' },
  { value: 'sales', title: 'سند سیستم فروش' },
  { value: 'purchase', title: 'سند سیستم خرید' },
  { value: 'payment', title: 'سند پرداخت صندوق' },
  { value: 'receipt', title: 'سند دریافت صندوق' },
  { value: 'payroll', title: 'سند حقوق و پرسنلی' }
]

const fiscalYearOptions = ['۱۴۰۲', '۱۴۰۳', '۱۴۰۴']
const balancedOptions = [
  { value: 'true', title: 'فقط اسناد تراز شده' },
  { value: 'false', title: 'فقط اسناد دارای مغایرت (نابالغ)' }
]

// --- Computed Properties for Stats ---
const totalDocumentsCount = computed(() => documents.value.length)
const draftDocumentsCount = computed(() => documents.value.filter(d => d.status === 'draft').length)
const finalizedDocumentsCount = computed(() => documents.value.filter(d => d.status === 'finalized').length)
const totalDebitSum = computed(() => documents.value.reduce((acc, d) => acc + d.totalDebit, 0))

// --- High-Fidelity Filtering Logic ---
const filteredDocuments = computed(() => {
  return documents.value.filter(doc => {
    // 1. Text Search (Search globally in document number, description, item account codes or names)
    if (searchQuery.value && searchQuery.value.trim() !== '') {
      const q = searchQuery.value.toLowerCase().trim()
      const matchesDoc = doc.docNo.toLowerCase().includes(q) || doc.description.toLowerCase().includes(q)
      const matchesItems = doc.items.some(item => 
        item.accountCode.toLowerCase().includes(q) || 
        item.accountName.toLowerCase().includes(q) ||
        (item.note && item.note.toLowerCase().includes(q))
      )
      if (!matchesDoc && !matchesItems) return false
    }

    // 2. Status Filter
    if (filters.value.status && doc.status !== filters.value.status) {
      return false
    }

    // 3. Document Type Filter
    if (filters.value.type && doc.type !== filters.value.type) {
      return false
    }

    // 4. Creator filter
    if (filters.value.creator && !doc.createdBy.toLowerCase().includes(filters.value.creator.toLowerCase())) {
      return false
    }

    // 5. Fiscal Year Filter
    if (filters.value.fiscalYear && doc.fiscalYear !== filters.value.fiscalYear) {
      return false
    }

    // 6. Range of Document Numbers
    if (filters.value.docNoStart) {
      const numDoc = Number(doc.docNo.replace(/\D/g, ''))
      const startLimit = Number(filters.value.docNoStart.replace(/\D/g, ''))
      if (numDoc < startLimit) return false
    }
    if (filters.value.docNoEnd) {
      const numDoc = Number(doc.docNo.replace(/\D/g, ''))
      const endLimit = Number(filters.value.docNoEnd.replace(/\D/g, ''))
      if (numDoc > endLimit) return false
    }

    // 7. Date Ranges
    if (filters.value.dateStart && doc.date < filters.value.dateStart) {
      return false
    }
    if (filters.value.dateEnd && doc.date > filters.value.dateEnd) {
      return false
    }

    // 8. Project
    if (filters.value.project) {
      const matchesProject = doc.items.some(item => item.project && item.project.toLowerCase().includes(filters.value.project.toLowerCase()))
      if (!matchesProject) return false
    }

    // 9. Cost Center
    if (filters.value.costCenter) {
      const matchesCC = doc.items.some(item => item.costCenter && item.costCenter.toLowerCase().includes(filters.value.costCenter.toLowerCase()))
      if (!matchesCC) return false
    }

    // 10. Balanced Status
    if (filters.value.balanced) {
      const isTrue = filters.value.balanced === 'true'
      if (doc.balanced !== isTrue) return false
    }

    return true
  })
})

// --- Server-side simulation Pagination ---
const totalPages = computed(() => Math.ceil(filteredDocuments.value.length / pageSize.value))
const startIndex = computed(() => (currentPage.value - 1) * pageSize.value)
const endIndex = computed(() => currentPage.value * pageSize.value)
const paginatedDocuments = computed(() => {
  return filteredDocuments.value.slice(startIndex.value, endIndex.value)
})

// --- Methods & UI Controls ---
function toggleExpand(doc) {
  doc.expanded = !doc.expanded
}

function onFiltersChange() {
  currentPage.value = 1
  selectedIds.value = []
  selectAllChecked.value = false
}

function resetFilters() {
  filters.value = {
    status: null,
    type: null,
    creator: '',
    fiscalYear: '۱۴۰۳',
    docNoStart: '',
    docNoEnd: '',
    dateStart: '',
    dateEnd: '',
    project: '',
    costCenter: '',
    balanced: null
  }
  searchQuery.value = ''
  onFiltersChange()
}

function formatMoney(amount) {
  if (amount === undefined || amount === null) return '۰'
  return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}

function getStatusLabel(status) {
  switch (status) {
    case 'draft': return 'موقت'
    case 'approved': return 'تایید شده'
    case 'finalized': return 'قطعی شده'
    case 'cancelled': return 'ابطال شده'
    default: return 'نامشخص'
  }
}

function getStatusColor(status) {
  switch (status) {
    case 'draft': return 'grey-lighten-1'
    case 'approved': return 'warning'
    case 'finalized': return 'indigo'
    case 'cancelled': return 'error'
    default: return 'grey'
  }
}

function getDocTypeLabel(type) {
  switch (type) {
    case 'manual': return 'سند دستی'
    case 'sales': return 'فاکتور فروش'
    case 'purchase': return 'فاکتور خرید'
    case 'payment': return 'پرداخت خزانه‌داری'
    case 'receipt': return 'دریافت خزانه‌داری'
    case 'payroll': return 'حقوق و پرسنلی'
    default: return 'سایر اسناد'
  }
}

// --- Bulk operations ---
function toggleSelectRow(id) {
  const index = selectedIds.value.indexOf(id)
  if (index > -1) {
    selectedIds.value.splice(index, 1)
  } else {
    selectedIds.value.push(id)
  }
}

function isSelected(id) {
  return selectedIds.value.includes(id)
}

function toggleSelectAll() {
  if (selectAllChecked.value) {
    selectedIds.value = paginatedDocuments.value.map(d => d.id)
  } else {
    selectedIds.value = []
  }
}

function bulkAction(action) {
  if (selectedIds.value.length === 0) return
  
  if (action === 'approve') {
    selectedIds.value.forEach(id => {
      const doc = documents.value.find(d => d.id === id)
      if (doc && doc.status === 'draft') {
        doc.status = 'approved'
        doc.approvedBy = 'سیستم (تایید گروهی)'
      }
    })
    showToast(`تعداد ${selectedIds.value.length} سند با موفقیت تایید و امضا شد.`, 'success')
  } else if (action === 'finalize') {
    let unBalancedCount = 0
    selectedIds.value.forEach(id => {
      const doc = documents.value.find(d => d.id === id)
      if (doc) {
        if (!doc.balanced) {
          unBalancedCount++
        } else {
          doc.status = 'finalized'
        }
      }
    })
    if (unBalancedCount > 0) {
      showToast(`تعداد ${unBalancedCount} سند به علت عدم تراز مالی، قطعی نشدند! مابقی با موفقیت قطعی شدند.`, 'error')
    } else {
      showToast(`تمام ${selectedIds.value.length} سند با موفقیت قطعی و قفل شدند.`, 'success')
    }
  } else if (action === 'cancel') {
    selectedIds.value.forEach(id => {
      const doc = documents.value.find(d => d.id === id)
      if (doc && doc.status !== 'finalized') {
        doc.status = 'cancelled'
      }
    })
    showToast(`عملیات ابطال گروهی با موفقیت اعمال شد.`, 'success')
  } else {
    showToast(`عملیات گروهی "${action}" با موفقیت ارسال شد.`, 'success')
  }
  
  selectedIds.value = []
  selectAllChecked.value = false
}

// --- Individual actions ---
function showAuditLog(doc) {
  auditModal.value = { show: true, doc }
}

function showDocDetailsModal(doc) {
  detailsModal.value = { show: true, doc }
}

function cancelDoc(doc) {
  if (doc.status === 'finalized') {
    showToast('سند قطعی شده غیر قابل ابطال است!', 'error')
    return
  }
  doc.status = 'cancelled'
  showToast(`سند ${doc.docNo} با موفقیت باطل شد.`, 'success')
}

function deleteDoc(id) {
  const index = documents.value.findIndex(d => d.id === id)
  if (index > -1) {
    const doc = documents.value[index]
    if (doc.status !== 'draft') {
      showToast('تنها اسناد در وضعیت موقت قابل حذف هستند!', 'error')
      return
    }
    documents.value.splice(index, 1)
    showToast(`سند ${doc.docNo} با موفقیت حذف گردید.`, 'success')
  }
}

function cloneDoc(doc) {
  const nextNo = 'ACC-' + (1000 + documents.value.length + 1)
  const cloned = {
    ...JSON.parse(JSON.stringify(doc)),
    id: documents.value.length + 1,
    docNo: nextNo,
    date: '۱۴۰۳/۱۲/۰۱',
    createdAt: '۱۴۰۳/۱۲/۰۱ ۰۹:۰۰:۰۰',
    status: 'draft',
    createdBy: 'مدیر مالی (رونوشت)',
    approvedBy: null,
    approvedAt: null,
    updatedBy: null,
    updatedAt: null,
    expanded: false
  }
  documents.value.push(cloned)
  showToast(`سند جدید با شماره ${nextNo} از روی سند قدیمی کپی شد.`, 'success')
}

function printDoc(doc) {
  showToast(`ارسال دستور چاپ برای سند ${doc.docNo}...`, 'success')
}

function exportDocToPdf(doc) {
  showToast(`در حال تولید خروجی PDF برای سند ${doc.docNo}...`, 'success')
}

function exportAllToPdf() {
  showToast('در حال تولید فایل PDF جامع اسناد حسابداری...', 'success')
}

function exportToExcel() {
  showToast('خروجی اکسل با موفقیت دانلود شد.', 'success')
}

function showToast(text, type = 'success') {
  toast.value = { show: true, text, type }
  setTimeout(() => {
    toast.value.show = false
  }, 4000)
}
</script>

<style scoped>
.accounting-page {
  font-family: 'Vazirmatn', sans-serif;
  color: #e2e8f0;
  max-width: 1500px;
  margin: 0 auto;
  padding-bottom: 2rem;
}

/* Page Header */
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

/* Premium glassmorphism cards */
.glass-card {
  background: rgba(30, 41, 59, 0.45) !important;
  backdrop-filter: blur(14px) !important;
  border: 1px solid rgba(255, 255, 255, 0.05) !important;
  border-radius: 16px !important;
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

.border-t {
  border-top: 1px solid rgba(255, 255, 255, 0.05);
}

/* Stats panel widgets */
.stat-card {
  padding: 20px;
  display: flex;
  align-items: center;
  gap: 16px;
  transition: transform 0.22s, border-color 0.22s;
}

.stat-card:hover {
  transform: translateY(-2px);
  border-color: rgba(56, 189, 248, 0.25) !important;
}

.stat-icon-wrapper {
  width: 50px;
  height: 50px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid rgba(255, 255, 255, 0.05);
}

.bg-blue-glow {
  background: rgba(56, 189, 248, 0.12);
}

.bg-amber-glow {
  background: rgba(245, 158, 11, 0.12);
}

.bg-success-glow {
  background: rgba(16, 185, 129, 0.12);
}

.bg-cyan-glow {
  background: rgba(6, 182, 212, 0.12);
}

.stat-title {
  font-size: 12px;
  color: #94a3b8;
  margin-bottom: 4px;
}

.stat-value {
  font-size: 19px;
  font-weight: 800;
  color: #f8fafc;
}

/* Custom Tree Grid Table Layout */
.table-responsive {
  overflow-x: auto;
}

.custom-table {
  border-collapse: collapse;
  text-align: right;
  width: 100%;
}

.custom-table th {
  padding: 15px 16px;
  border-bottom: 2px solid rgba(255, 255, 255, 0.06);
  color: #94a3b8;
  font-size: 12px;
  font-weight: 700;
  background: rgba(255, 255, 255, 0.01);
}

.custom-table td {
  padding: 14px 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.03);
  font-size: 13px;
  vertical-align: middle;
  transition: background 0.15s;
}

.table-row {
  cursor: pointer;
}

.table-row:hover td {
  background: rgba(56, 189, 248, 0.03) !important;
}

.row-expanded td {
  background: rgba(56, 189, 248, 0.015) !important;
  border-bottom: none !important;
}

.row-selected td {
  background: rgba(56, 189, 248, 0.04) !important;
}

.mono-number {
  font-family: inherit;
  letter-spacing: 0.5px;
}

.type-badge {
  font-size: 11px;
  color: #94a3b8;
  background: rgba(255, 255, 255, 0.04);
  padding: 3px 8px;
  border-radius: 6px;
  border: 1px solid rgba(255, 255, 255, 0.03);
}

.doc-desc {
  max-width: 250px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

/* Expand Animation and Nested Table */
.btn-expand-arrow {
  transition: transform 0.22s ease;
}

.arrow-rotated {
  transform: rotate(180deg);
}

.expanded-detail-row td {
  background: rgba(15, 23, 42, 0.7) !important;
}

.glass-card-nested {
  background: rgba(15, 23, 42, 0.8);
  border: 1px solid rgba(56, 189, 248, 0.15);
  box-shadow: inset 0 0 12px rgba(56, 189, 248, 0.05);
}

.expanded-title {
  font-size: 13.5px;
  color: #e2e8f0;
  font-weight: 700;
}

.detail-items-table {
  border-collapse: collapse;
}

.detail-items-table th {
  padding: 10px 12px;
  background: rgba(255, 255, 255, 0.02);
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  color: #94a3b8;
  font-size: 11.5px;
}

.detail-items-table td {
  padding: 10px 12px !important;
  border-bottom: 1px solid rgba(255, 255, 255, 0.02) !important;
  font-size: 12.5px;
}

.subtotal-row td {
  background: rgba(255, 255, 255, 0.015) !important;
  border-top: 1px solid rgba(255, 255, 255, 0.08) !important;
  font-size: 13px !important;
}

/* Premium Dropdowns and Fields */
.dark-field :deep(.v-field) {
  border-radius: 10px;
  background: rgba(15, 23, 42, 0.6);
  border: 1px solid rgba(255, 255, 255, 0.06);
}

.dark-field :deep(.v-field:hover) {
  border-color: rgba(56, 189, 248, 0.3);
}

.dark-menu-list {
  background: #080c14 !important;
  border-color: rgba(255, 255, 255, 0.08) !important;
}

/* Modals */
.audit-modal-wrapper,
.details-modal-wrapper {
  background: rgba(15, 23, 42, 0.93) !important;
  backdrop-filter: blur(20px) !important;
  border: 1px solid rgba(56, 189, 248, 0.2) !important;
  border-radius: 16px !important;
  overflow: hidden;
}

.dialog-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.btn-close-dialog {
  background: transparent;
  border: none;
  color: #94a3b8;
  cursor: pointer;
  width: 28px;
  height: 28px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 6px;
  transition: all 0.2s;
}

.btn-close-dialog:hover {
  background: rgba(239, 68, 68, 0.1);
  color: #ef4444;
}

/* Audit timeline */
.audit-timeline {
  position: relative;
  padding-right: 20px;
  border-right: 1.5px solid rgba(255, 255, 255, 0.05);
}

.timeline-item {
  position: relative;
}

.timeline-badge {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  position: absolute;
  right: -33px;
  top: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 2.5px solid #0f172a;
}

/* Toast Premium styles */
.toast-premium {
  position: fixed;
  bottom: 24px;
  left: 24px;
  background: rgba(8, 12, 20, 0.9);
  backdrop-filter: blur(12px);
  border-radius: 12px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
  overflow: hidden;
  z-index: 9999;
  border: 1px solid rgba(255, 255, 255, 0.08);
}

.toast-content {
  display: flex;
  align-items: center;
  padding: 14px 18px;
  gap: 12px;
}

.toast-icon {
  width: 26px;
  height: 26px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.toast-success {
  border-color: rgba(34, 197, 94, 0.3);
}
.toast-success .toast-icon {
  background: rgba(34, 197, 94, 0.15);
  color: #22c55e;
}

.toast-error {
  border-color: rgba(239, 68, 68, 0.3);
}
.toast-error .toast-icon {
  background: rgba(239, 68, 68, 0.15);
  color: #ef4444;
}

.toast-timer-bar {
  height: 3px;
  background: #0ea5e9;
  width: 100%;
  animation: shrinkWidth 4s linear forwards;
}

@keyframes shrinkWidth {
  from { width: 100%; }
  to { width: 0%; }
}

@media (max-width: 960px) {
  .page-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 16px;
  }
}
</style>
