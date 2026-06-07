<template>
  <div class="page-root">

    <!-- ===== 1. Page Header ===== -->
    <header class="page-header">
      <div class="header-right">
        <div class="header-icon-wrap">
          <svg class="glow-icon" width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="var(--cyan)" stroke-width="2">
            <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z" />
            <polyline points="14 2 14 8 20 8" />
            <line x1="12" y1="18" x2="12" y2="12" />
            <line x1="9" y1="15" x2="15" y2="15" />
          </svg>
        </div>
        <div>
          <h1 class="page-title">{{ editingDocumentId ? 'ویرایش سند حسابداری' : 'ثبت سند حسابداری جدید' }}</h1>
          <p class="page-sub">سیستم یکپارچه مالی و اداری فینومال</p>
        </div>
      </div>
      <div class="header-left">
        <button class="btn-preview-ledger-top" @click="openCurrentLedgerModal">
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
            <circle cx="12" cy="12" r="3" />
          </svg>
          پیش‌نمایش سند رسمی
        </button>
      </div>
    </header>

    <!-- ===== 2. Hero Statistics Dashboard ===== -->
    <section class="hero-stats-grid mb-4">
      <div class="stat-card glass-card">
        <div class="stat-icon-wrap icon-purple">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <rect x="3" y="3" width="18" height="18" rx="2" />
            <path d="M21 12H3M12 3v18" />
          </svg>
        </div>
        <div class="stat-info">
          <span class="stat-label">کل اسناد آرشیو</span>
          <span class="stat-value">{{ totalSavedDocumentsCount }} سند</span>
        </div>
        <div class="stat-mini-details">
          <span class="pill-draft font-fa">{{ totalDraftsCount }} پیش‌نویس</span>
          <span class="pill-final font-fa">{{ totalFinalizedCount }} نهایی</span>
        </div>
      </div>

      <div class="stat-card glass-card">
        <div class="stat-icon-wrap icon-emerald">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <line x1="12" y1="1" x2="12" y2="23" />
            <path d="M17 5H9.5a3.5 3.5 0 000 7h5a3.5 3.5 0 010 7H6" />
          </svg>
        </div>
        <div class="stat-info">
          <span class="stat-label">گردش کل بدهکار</span>
          <span class="stat-value text-emerald font-fa">{{ formatNum(totalDebitVolume) }}</span>
        </div>
        <div class="stat-caption">حجم گردش اسناد آرشیو شده</div>
      </div>

      <div class="stat-card glass-card">
        <div class="stat-icon-wrap icon-cyan">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="12" cy="12" r="10" />
            <polyline points="12 6 12 12 16 14" />
          </svg>
        </div>
        <div class="stat-info">
          <span class="stat-label">وضعیت سیستم جاری</span>
          <span class="stat-value text-cyan">{{ editingDocumentId ? 'در حال ویرایش' : 'سند جدید' }}</span>
        </div>
        <div class="stat-caption">{{ editingDocumentId ? 'کد سند: ' + document.accountingCode : 'کد خودکار: ACC-XXX' }}</div>
      </div>

      <div class="stat-card glass-card">
        <div class="stat-icon-wrap icon-amber">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <polygon points="12 2 2 22 22 22" />
            <line x1="12" y1="9" x2="12" y2="13" />
            <line x1="12" y1="17" x2="12.01" y2="17" />
          </svg>
        </div>
        <div class="stat-info">
          <span class="stat-label">صحت تعادل اسناد</span>
          <span class="stat-value text-amber font-fa">۱۰۰٪ متوازن</span>
        </div>
        <div class="stat-caption">تمام اسناد نهایی کاملا تراز هستند</div>
      </div>
    </section>

    <!-- ===== 3. Interactive Balance Scale Section ===== -->
    <section class="glass-card mb-4 balance-panel-wrapper">
      <div class="balance-content-row">
        <!-- Scale Graphics -->
        <div class="scale-visual-wrap">
          <svg class="balance-scale-svg" width="160" height="100" viewBox="0 0 200 120">
            <!-- Pillar Base & Stand -->
            <path d="M 70 110 L 130 110 Z" stroke="#334155" stroke-width="6" stroke-linecap="round" />
            <path d="M 100 50 L 100 110 Z" stroke="#1e293b" stroke-width="8" stroke-linecap="round" />
            <circle cx="100" cy="50" r="6" fill="var(--cyan)" />

            <!-- Rotating Beam -->
            <g class="scale-beam" :style="{ transform: 'rotate(' + rotationAngle + 'deg)' }">
              <line x1="40" y1="50" x2="160" y2="50" stroke="#475569" stroke-width="5" stroke-linecap="round" />
              <!-- Pivot point -->
              <circle cx="100" cy="50" r="3" fill="#0f172a" />
              
              <!-- Left Hanger cords (Debit) -->
              <g class="scale-pan-left" :style="{ transform: 'rotate(' + (-rotationAngle) + 'deg)', transformOrigin: '40px 50px' }">
                <line x1="40" y1="50" x2="25" y2="85" stroke="#475569" stroke-width="1.5" />
                <line x1="40" y1="50" x2="55" y2="85" stroke="#475569" stroke-width="1.5" />
                <path d="M 20 85 Q 40 100 60 85 Z" fill="var(--green)" opacity="0.8" />
                <circle cx="40" cy="72" r="5" fill="var(--green)" opacity="0.3" v-if="totalDebit > 0" />
              </g>

              <!-- Right Hanger cords (Credit) -->
              <g class="scale-pan-right" :style="{ transform: 'rotate(' + (-rotationAngle) + 'deg)', transformOrigin: '160px 50px' }">
                <line x1="160" y1="50" x2="145" y2="85" stroke="#475569" stroke-width="1.5" />
                <line x1="160" y1="50" x2="175" y2="85" stroke="#475569" stroke-width="1.5" />
                <path d="M 140 85 Q 160 100 180 85 Z" fill="var(--red)" opacity="0.8" />
                <circle cx="160" cy="72" r="5" fill="var(--red)" opacity="0.3" v-if="totalCredit > 0" />
              </g>
            </g>
          </svg>
        </div>

        <!-- Scale Data & Alerts -->
        <div class="scale-data-wrap">
          <div class="scale-data-header">
            <span class="scale-title">موازنه لحظه‌ای سند جاری</span>
            <div class="balance-indicator-badge"
              :class="isBalanced ? 'bal-ok' : totalDebit === 0 && totalCredit === 0 ? 'bal-neutral' : 'bal-err'">
              <span class="bal-dot"></span>
              <span>{{ isBalanced ? 'سند متوازن است' : totalDebit === 0 && totalCredit === 0 ? 'در انتظار ردیف مالی' : 'سند ناتراز است' }}</span>
            </div>
          </div>

          <div class="scale-bars-grid">
            <div class="scale-bar-item">
              <div class="scale-bar-label">
                <span>جمع بدهکار (صفحه راست)</span>
                <span class="text-emerald font-fa font-weight-bold">{{ formatNum(totalDebit) }} ریال</span>
              </div>
              <div class="progress-track">
                <div class="progress-bar fill-emerald" :style="{ width: getPercentage(totalDebit) + '%' }"></div>
              </div>
            </div>

            <div class="scale-bar-item">
              <div class="scale-bar-label">
                <span>جمع بستانکار (صفحه چپ)</span>
                <span class="text-rose font-fa font-weight-bold">{{ formatNum(totalCredit) }} ریال</span>
              </div>
              <div class="progress-track">
                <div class="progress-bar fill-rose" :style="{ width: getPercentage(totalCredit) + '%' }"></div>
              </div>
            </div>
          </div>

          <!-- Difference Warning -->
          <div v-if="!isBalanced && (totalDebit > 0 || totalCredit > 0)" class="scale-diff-alert animate-shake">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <polygon points="12 2 2 22 22 22" />
              <line x1="12" y1="9" x2="12" y2="13" />
              <line x1="12" y1="17" x2="12.01" y2="17" />
            </svg>
            <span class="font-fa">اختلاف عدم موازنه: <strong>{{ formatNum(Math.abs(totalDebit - totalCredit)) }} ریال</strong></span>
            <span class="diff-desc">({{ totalDebit > totalCredit ? 'ستون بدهکار سنگین‌تر است' : 'ستون بستانکار سنگین‌تر است' }})</span>
          </div>
        </div>
      </div>
    </section>

    <!-- ===== 4. Main Form Card (اطلاعات اصلی) ===== -->
    <section class="glass-card mb-4 primary-form-card">
      <div class="card-section-title">
        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <circle cx="12" cy="12" r="10" />
          <line x1="12" y1="8" x2="12" y2="12" />
          <line x1="12" y1="16" x2="12.01" y2="16" />
        </svg>
        اطلاعات اصلی سند مالی
      </div>

      <div class="form-grid">
        <div class="field-wrap span-2">
          <label class="field-label">شرح کلی سند</label>
          <v-text-field v-model="document.description" variant="outlined" density="compact"
            placeholder="مثال: خرید ملزومات اداری و پرداخت هزینه‌های جاری شعبه مرکزی..." class="dark-field-premium" hide-details rounded="lg" />
        </div>

        <div class="field-wrap">
          <label class="field-label">تاریخ صدور</label>
          <v-text-field v-model="document.date" type="date" variant="outlined" density="compact" class="dark-field-premium"
            hide-details rounded="lg" />
        </div>

        <div class="field-wrap">
          <label class="field-label">کد حسابداری سند</label>
          <div class="code-row">
            <label class="toggle-wrap">
              <input type="checkbox" v-model="isManualCode" class="toggle-input" />
              <span class="toggle-track"><span class="toggle-thumb"></span></span>
              <span class="toggle-label">دستی</span>
            </label>
            <v-text-field v-model="document.accountingCode" variant="outlined" density="compact"
              :disabled="!isManualCode" class="dark-field-premium flex-1" :class="{ 'disabled-field': !isManualCode }" hide-details rounded="lg" />
          </div>
        </div>
      </div>
    </section>

    <!-- ===== 5. Smart Voucher Items Table ===== -->
    <section class="glass-card mb-4 table-card">
      <div class="table-toolbar">
        <div class="card-section-title mb-0">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <rect x="3" y="3" width="18" height="18" rx="2" />
            <path d="M3 9h18M3 15h18M9 3v18" />
          </svg>
          آرایش ردیف‌های مالی سند
          <span v-if="document.items.length > 0" class="row-count font-fa">{{ document.items.length }} ردیف</span>
        </div>
        <div class="table-toolbar-actions">
          <span class="keyboard-tip">راهنما: Enter در آخرین سلول = سطر جدید</span>
          <button class="btn-add-row-premium" @click="addRow">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <line x1="12" y1="5" x2="12" y2="19" />
              <line x1="5" y1="12" x2="19" y2="12" />
            </svg>
            افزودن ردیف مالی
          </button>
        </div>
      </div>

      <!-- Empty State -->
      <div v-if="document.items.length === 0" class="empty-state-premium">
        <div class="empty-icon-wrap-premium">
          <svg width="34" height="34" viewBox="0 0 24 24" fill="none" stroke="var(--cyan)" stroke-width="1.5">
            <rect x="3" y="3" width="18" height="18" rx="2" />
            <path d="M3 9h18M9 3v18" />
            <path d="M15 15l-3-3m0 0l-3-3m3 3l3-3m-3 3l-3 3" stroke-dasharray="2 2" />
          </svg>
        </div>
        <div class="empty-title-premium">ردیفی برای سند حسابداری ثبت نشده است</div>
        <div class="empty-body-premium">برای تعریف تراکنش‌های مالی، روی کلید «افزودن ردیف مالی» کلیک کنید.</div>
        <button class="btn-add-row-empty mt-3" @click="addRow">
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <line x1="12" y1="5" x2="12" y2="19" />
            <line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          ایجاد اولین ردیف
        </button>
      </div>

      <!-- Table with internal scroll -->
      <div v-else class="table-scroll-wrap-premium">
        <table class="doc-table-premium">
          <thead>
            <tr>
              <th class="th-num">#</th>
              <th>حساب معین</th>
              <th>حساب تفصیل</th>
              <th>شرح ردیف</th>
              <th class="th-unit">واحد</th>
              <th class="th-amount-head text-emerald">بدهکار (ریال)</th>
              <th class="th-amount-head text-rose">بستانکار (ریال)</th>
              <th class="th-action">عملیات سطر</th>
            </tr>
          </thead>
          <tbody>
            <transition-group name="row-anim">
              <tr v-for="(item, index) in document.items" :key="item.itemId" class="doc-row-premium"
                :class="{ 'row-debit-active': item.debit > 0, 'row-credit-active': item.credit > 0 }">
                <td class="td-num font-fa">{{ index + 1 }}</td>
                <td>
                  <v-select v-model="item.accountId" :items="accounts" item-title="title" item-value="id"
                    variant="outlined" density="compact" class="dark-field-table tf" placeholder="انتخاب حساب..." hide-details rounded="lg" />
                </td>
                <td>
                  <v-select v-model="item.detailId" :items="details" item-title="title" item-value="id" variant="outlined"
                    density="compact" class="dark-field-table tf" placeholder="انتخاب تفصیل..." hide-details rounded="lg" />
                </td>
                <td>
                  <v-text-field v-model="item.description" variant="outlined" density="compact" class="dark-field-table tf-desc-input"
                    placeholder="شرح ردیف را بنویسید..." hide-details rounded="lg" />
                </td>
                <td>
                  <v-select v-model="item.unit" :items="units" item-title="title" item-value="id" variant="outlined"
                    density="compact" class="dark-field-table tf-sm" hide-details rounded="lg" />
                </td>
                
                <!-- DEBIT FIELD WITH MAGIC AUTO BALANCE -->
                <td class="td-debit-cell">
                  <v-text-field v-model.number="item.debit" type="number" variant="outlined" density="compact"
                    class="dark-field-table tf-num debit-field" placeholder="0" hide-details rounded="lg" 
                    @input="item.credit = 0" @keydown="handleKeyDown($event, index, 'debit')">
                    <template #append-inner>
                      <button v-if="!item.debit && !item.credit && totalDebit !== totalCredit" 
                        class="magic-balance-btn" title="تراز خودکار ردیف جاری" @click.stop="fillAutoBalance(index, 'debit')">
                        <svg class="magic-sparkle" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                          <path d="M12 2v4M12 18v4M4.93 4.93l2.83 2.83M16.24 16.24l2.83 2.83M2 12h4M18 12h4M4.93 19.07l2.83-2.83M16.24 7.76l2.83-2.83" />
                        </svg>
                      </button>
                    </template>
                  </v-text-field>
                </td>

                <!-- CREDIT FIELD WITH MAGIC AUTO BALANCE -->
                <td class="td-credit-cell">
                  <v-text-field v-model.number="item.credit" type="number" variant="outlined" density="compact"
                    class="dark-field-table tf-num credit-field" placeholder="0" hide-details rounded="lg" 
                    @input="item.debit = 0" @keydown="handleKeyDown($event, index, 'credit')">
                    <template #append-inner>
                      <button v-if="!item.debit && !item.credit && totalDebit !== totalCredit" 
                        class="magic-balance-btn" title="تراز خودکار ردیف جاری" @click.stop="fillAutoBalance(index, 'credit')">
                        <svg class="magic-sparkle" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                          <path d="M12 2v4M12 18v4M4.93 4.93l2.83 2.83M16.24 16.24l2.83 2.83M2 12h4M18 12h4M4.93 19.07l2.83-2.83M16.24 7.76l2.83-2.83" />
                        </svg>
                      </button>
                    </template>
                  </v-text-field>
                </td>

                <!-- ROW OPERATIONS -->
                <td class="td-action">
                  <div class="row-util-actions">
                    <!-- Swap columns -->
                    <button class="btn-row-action swap-act" @click="swapDebitCredit(index)" title="جابه‌جایی بدهکار / بستانکار">
                      <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                        <path d="M7 16V4M7 4L3 8M7 4l4 4M17 8v12M17 20l-4-4M17 20l4-4" />
                      </svg>
                    </button>
                    <!-- Duplicate -->
                    <button class="btn-row-action duplicate-act" @click="duplicateRow(index)" title="تکثیر این ردیف">
                      <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                        <rect x="9" y="9" width="13" height="13" rx="2" ry="2" />
                        <path d="M5 15H4a2 2 0 01-2-2V4a2 2 0 012-2h9a2 2 0 012 2v1" />
                      </svg>
                    </button>
                    <!-- Delete -->
                    <button class="btn-row-action delete-act" @click="removeRow(item)" title="حذف ردیف مالی">
                      <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                        <polyline points="3 6 5 6 21 6" />
                        <path d="M19 6l-1 14H6L5 6" />
                        <path d="M10 11v6M14 11v6" />
                      </svg>
                    </button>
                  </div>
                </td>
              </tr>
            </transition-group>
          </tbody>
        </table>
      </div>

      <!-- Totals Footer -->
      <div v-if="document.items.length > 0" class="totals-footer-premium">
        <span class="totals-label-premium">جمع کل تراز سند</span>
        <div class="totals-chips-premium">
          <div class="total-chip-premium chip-debit-p">
            <span class="chip-label-p">بدهکار</span>
            <span class="chip-value-p font-fa">{{ formatNum(totalDebit) }} <span class="val-unit">ریال</span></span>
          </div>
          <div class="total-sep-p">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#334155" stroke-width="2">
              <path d="M5 12h14M12 5v14" />
            </svg>
          </div>
          <div class="total-chip-premium chip-credit-p">
            <span class="chip-label-p">بستانکار</span>
            <span class="chip-value-p font-fa">{{ formatNum(totalCredit) }} <span class="val-unit">ریال</span></span>
          </div>
          
          <div v-if="!isBalanced" class="total-chip-premium chip-diff-p">
            <span class="chip-label-p">اختلاف عدم موازنه</span>
            <span class="chip-value-p font-fa">{{ formatNum(Math.abs(totalDebit - totalCredit)) }} <span class="val-unit">ریال</span></span>
          </div>
          
          <div v-else class="total-balanced-p animate-glow">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
              <polyline points="20 6 9 17 4 12" />
            </svg>
            سند کاملاً متوازن است
          </div>
        </div>
      </div>
    </section>

    <!-- ===== 6. Action Bar ===== -->
    <div class="action-bar-premium">
      <button class="btn-submit-premium" :disabled="!isBalanced || totalDebit === 0" @click="handleSubmit">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <path d="M19 21H5a2 2 0 01-2-2V5a2 2 0 012-2h11l5 5v14z" />
          <polyline points="17 21 17 13 7 13 7 21" />
          <polyline points="7 3 7 8 15 8" />
        </svg>
        <span>{{ editingDocumentId ? 'ذخیره اصلاحات سند' : 'ثبت و تایید نهایی سند' }}</span>
      </button>
      
      <button v-if="editingDocumentId" class="btn-cancel-premium" @click="cancelEdit">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <line x1="18" y1="6" x2="6" y2="18" />
          <line x1="6" y1="6" x2="18" y2="18" />
        </svg>
        <span>انصراف از ویرایش</span>
      </button>
    </div>

    <!-- ===== 7. Section Divider ===== -->
    <div class="section-sep">
      <div class="sep-line"></div>
      <div class="sep-label">
        <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <line x1="8" y1="6" x2="21" y2="6" />
          <line x1="8" y1="12" x2="21" y2="12" />
          <line x1="8" y1="18" x2="21" y2="18" />
          <line x1="3" y1="6" x2="3.01" y2="6" />
          <line x1="3" y1="12" x2="3.01" y2="12" />
          <line x1="3" y1="18" x2="3.01" y2="18" />
        </svg>
        آرشیو اسناد حسابداری صادر شده
      </div>
      <div class="sep-line"></div>
    </div>

    <!-- ===== 8. Saved Documents List ===== -->
    <section class="glass-card archive-card">
      <div class="list-toolbar">
        <div class="card-section-title mb-0">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <path d="M21.44 11.05l-9.19 9.19a6 6 0 01-8.49-8.49l9.19-9.19a4 4 0 015.66 5.66l-9.2 9.19a2 2 0 01-2.83-2.83l8.49-8.48" />
          </svg>
          دفتر اسناد مالی (ثبت شده)
        </div>
        <span class="doc-count-badge-premium font-fa">{{ savedDocuments.length }} سند مالی صادر شده</span>
      </div>

      <!-- Empty archive -->
      <div v-if="savedDocuments.length === 0" class="empty-state-premium py-5">
        <div class="empty-icon-wrap-premium">
          <svg width="34" height="34" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <path d="M22 12h-4l-3 9L9 3l-3 9H2" />
          </svg>
        </div>
        <div class="empty-title-premium">آرشیو اسناد خالی است</div>
        <div class="empty-body-premium">تاکنون هیچ سند حسابداری در سیستم صادر یا ذخیره نشده است.</div>
      </div>

      <!-- Archive table -->
      <div v-else class="table-scroll-wrap-premium">
        <table class="doc-table-premium">
          <thead>
            <tr>
              <th class="th-code">کد رسمی سند</th>
              <th class="th-date">تاریخ صدور</th>
              <th>شرح و پیگیری سند</th>
              <th class="th-amount-head text-emerald">گردش کل بدهکار (ریال)</th>
              <th class="th-status">وضعیت ثبتی</th>
              <th class="th-action-archive">عملیات آرشیو</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="doc in savedDocuments" :key="doc.documentId" class="doc-row-premium">
              <td>
                <span class="code-chip-premium font-fa">{{ doc.accountingCode }}</span>
              </td>
              <td class="td-date font-fa">{{ formatDate(doc.date) }}</td>
              <td class="td-desc-archive" :title="doc.description">{{ doc.description || 'بدون شرح کلی' }}</td>
              <td class="td-amount-archive font-fa text-emerald">
                <span>{{ formatNum(doc.items.reduce((a, b) => a + Number(b.debit || 0), 0)) }}</span>
              </td>
              <td>
                <span class="status-pill-premium" :class="doc.isFinalized ? 'pill-final-active' : 'pill-draft-active'">
                  <span class="pill-dot-premium"></span>
                  {{ doc.isFinalized ? 'ثبت قطعی (نهایی)' : 'پیش‌نویس موقت' }}
                </span>
              </td>
              <td>
                <div class="row-actions-archive">
                  <!-- View Official Ledger Modal -->
                  <button class="btn-arch-act act-view" title="مشاهده و چاپ سند رسمی حسابداری" @click="openLedgerModal(doc)">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                      <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
                      <circle cx="12" cy="12" r="3" />
                    </svg>
                  </button>

                  <template v-if="!doc.isFinalized">
                    <!-- Edit -->
                    <button class="btn-arch-act act-edit" title="ویرایش و اصلاح جزئیات" @click="editDocument(doc)">
                      <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" />
                        <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z" />
                      </svg>
                    </button>
                    <!-- Finalize -->
                    <button class="btn-arch-act act-final" title="تایید نهایی و قفل سند" @click="finalizeDocument(doc.documentId)">
                      <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                        <polyline points="20 6 9 17 4 12" />
                      </svg>
                    </button>
                    <!-- Delete -->
                    <button class="btn-arch-act act-del" title="حذف دائم سند" @click="deleteDocument(doc.documentId)">
                      <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <polyline points="3 6 5 6 21 6" />
                        <path d="M19 6l-1 14H6L5 6" />
                      </svg>
                    </button>
                  </template>
                  
                  <span v-else class="locked-indicator" title="سند نهایی شده و قفل است">
                    <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                      <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                      <path d="M7 11V7a5 5 0 0110 0v4" />
                    </svg>
                  </span>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>

    <!-- ===== 9. Official Persian Ledger Viewer Modal ===== -->
    <v-dialog v-model="showLedgerModal" max-width="850px" transition="dialog-bottom-transition">
      <div class="ledger-modal-wrapper glass-card">
        <div class="ledger-modal-header no-print">
          <div class="ledger-modal-title">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="var(--cyan)" stroke-width="2.5">
              <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z" />
              <polyline points="14 2 14 8 20 8" />
            </svg>
            برگ رسمی سند حسابداری فینومال
          </div>
          <div class="ledger-modal-actions">
            <button class="btn-ledger-print" @click="printLedger">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                <polyline points="6 9 6 2 18 2 18 9" />
                <path d="M6 18H4a2 2 0 01-2-2v-5a2 2 0 012-2h16a2 2 0 012 2v5a2 2 0 01-2 2h-2" />
                <rect x="6" y="14" width="12" height="8" />
              </svg>
              چاپ برگ سند (PDF)
            </button>
            <button class="btn-ledger-close" @click="closeLedgerModal">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                <line x1="18" y1="6" x2="6" y2="18" />
                <line x1="6" y1="6" x2="18" y2="18" />
              </svg>
            </button>
          </div>
        </div>

        <!-- Printable Document Area -->
        <div class="print-ledger-container" id="printable-ledger">
          <div class="print-header">
            <div class="print-header-right">
              <h2 class="print-company-name">شرکت توسعه فناوری مالی فینومال</h2>
              <h3 class="print-doc-title">سند حسابداری (دفتر معین و تفصیل)</h3>
            </div>
            <div class="print-header-center">
              <div class="print-logo-dummy">Finomal ERP</div>
            </div>
            <div class="print-header-left font-fa">
              <div><strong>شماره سند:</strong> {{ selectedLedgerDoc?.accountingCode || 'AUTO-DRAFT' }}</div>
              <div><strong>تاریخ سند:</strong> {{ formatDate(selectedLedgerDoc?.date) }}</div>
              <div><strong>وضعیت سند:</strong> {{ selectedLedgerDoc?.isFinalized ? 'ثبت قطعی' : 'پیش‌نویس موقت' }}</div>
            </div>
          </div>

          <div class="print-main-meta">
            <div><strong>شرح کلی سند:</strong> {{ selectedLedgerDoc?.description || 'شرحی درج نشده است' }}</div>
          </div>

          <table class="print-ledger-table">
            <thead>
              <tr>
                <th style="width: 40px; text-align: center;">ردیف</th>
                <th style="width: 130px;">کد و عنوان معین</th>
                <th style="width: 130px;">کد و عنوان تفصیل</th>
                <th>شرح جزء آرتیکل مالی</th>
                <th style="width: 120px; text-align: left;">بدهکار (ریال)</th>
                <th style="width: 120px; text-align: left;">بستانکار (ریال)</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, idx) in selectedLedgerDoc?.items" :key="idx">
                <td style="text-align: center;" class="font-fa">{{ idx + 1 }}</td>
                <td>{{ getAccountTitle(item.accountId) }}</td>
                <td>{{ getDetailTitle(item.detailId) }}</td>
                <td>{{ item.description || 'ثبت سند حسابداری' }}</td>
                <td style="text-align: left;" class="font-fa font-weight-bold">
                  {{ item.debit > 0 ? formatNum(item.debit) : '—' }}
                </td>
                <td style="text-align: left;" class="font-fa font-weight-bold">
                  {{ item.credit > 0 ? formatNum(item.credit) : '—' }}
                </td>
              </tr>
              <!-- Totals row -->
              <tr class="print-totals-row">
                <td colspan="4" style="text-align: right; font-weight: bold;">جمع کل برگ سند حسابداری</td>
                <td style="text-align: left;" class="font-fa font-weight-black">
                  {{ formatNum(selectedLedgerDoc?.items.reduce((a, b) => a + Number(b.debit || 0), 0)) }}
                </td>
                <td style="text-align: left;" class="font-fa font-weight-black">
                  {{ formatNum(selectedLedgerDoc?.items.reduce((a, b) => a + Number(b.credit || 0), 0)) }}
                </td>
              </tr>
            </tbody>
          </table>

          <div class="print-signatures">
            <div class="sig-box">
              <span>تهیه‌کننده (امضاء)</span>
              <span class="sig-space"></span>
            </div>
            <div class="sig-box">
              <span>تاییدکننده معین</span>
              <span class="sig-space"></span>
            </div>
            <div class="sig-box">
              <span>مدیر امور مالی</span>
              <span class="sig-space"></span>
            </div>
            <div class="sig-box">
              <span>مدیر عامل</span>
              <span class="sig-space"></span>
            </div>
          </div>
        </div>
      </div>
    </v-dialog>

    <!-- ===== 10. Premium Toast Alert ===== -->
    <transition name="toast">
      <div v-if="toast.show" class="toast-premium" :class="'toast-' + toast.type">
        <div class="toast-content">
          <div class="toast-icon">
            <svg v-if="toast.type === 'success'" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
              <polyline points="20 6 9 17 4 12" />
            </svg>
            <svg v-else-if="toast.type === 'error'" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
              <line x1="18" y1="6" x2="6" y2="18" />
              <line x1="6" y1="6" x2="18" y2="18" />
            </svg>
            <svg v-else width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
              <circle cx="12" cy="12" r="10" />
              <line x1="12" y1="8" x2="12" y2="12" />
              <line x1="12" y1="16" x2="12.01" y2="16" />
            </svg>
          </div>
          <div class="toast-text font-fa">{{ toast.text }}</div>
        </div>
        <div class="toast-timer-bar" :style="{ animationDuration: '3000ms' }"></div>
      </div>
    </transition>

  </div>
</template>

<script setup>
import { ref, computed, nextTick } from 'vue'

// --- Document Structure State ---
const document = ref({
  documentId: null,
  description: '',
  date: new Date().toISOString().split('T')[0],
  accountingCode: 'AUTO-001',
  items: []
})

// --- Archive Database (Mock) ---
const savedDocuments = ref([
  {
    documentId: 'doc-1',
    accountingCode: 'ACC-001',
    date: new Date('2024-01-15'),
    description: 'خرید تجهیزات اداری و ملزومات واحد فنی شعبه مرکزی',
    isFinalized: true,
    items: [
      { itemId: 101, accountId: 201, detailId: 2, description: 'خرید میز و صندلی پرسنل', unit: 1, debit: 5000000, credit: 0 },
      { itemId: 102, accountId: 101, detailId: 1, description: 'پرداخت از تنخواه آقای رضایی', unit: 1, debit: 0, credit: 5000000 }
    ]
  },
  {
    documentId: 'doc-2',
    accountingCode: 'ACC-002',
    date: new Date('2024-01-20'),
    description: 'دریافت از مشتری بابت تسویه فاکتور فروش شماره ۱۰۲',
    isFinalized: false,
    items: [
      { itemId: 103, accountId: 101, detailId: 1, description: 'واریز نقدی به صندوق شرکت', unit: 1, debit: 12000000, credit: 0 },
      { itemId: 104, accountId: 301, detailId: 2, description: 'فروش خدمات فنی مهندسی', unit: 1, debit: 0, credit: 12000000 }
    ]
  }
])

const isManualCode = ref(false)
const editingDocumentId = ref(null)
const toast = ref({ show: false, text: '', type: 'success' })

// --- Ledger Modal state ---
const showLedgerModal = ref(false)
const selectedLedgerDoc = ref(null)

// --- Master Data ---
const accounts = [
  { id: 101, title: '۱۰۱۰۱ - صندوق ریالی شرکت' },
  { id: 201, title: '۲Ax۰۱ - هزینه‌های اداری و تجهیزات' },
  { id: 301, title: '۳Ax۰۱ - درآمدهای ناخالص عملیاتی' },
]
const details = [
  { id: 1, title: 'آقای رضایی (تنخواه گردان مرکزی)' },
  { id: 2, title: 'شرکت نمونه توسعه ارتباطات ایرانیان' },
]
const units = [
  { id: 1, title: 'ریال' },
  { id: 2, title: 'تومان' },
]

// --- Computed Statistics (Dashboard) ---
const totalSavedDocumentsCount = computed(() => savedDocuments.value.length)
const totalDraftsCount = computed(() => savedDocuments.value.filter(d => !d.isFinalized).length)
const totalFinalizedCount = computed(() => savedDocuments.value.filter(d => d.isFinalized).length)
const totalDebitVolume = computed(() => {
  return savedDocuments.value.reduce((total, doc) => {
    return total + doc.items.reduce((sum, item) => sum + Number(item.debit || 0), 0)
  }, 0)
})

// --- Document Totals ---
const totalDebit = computed(() => document.value.items.reduce((a, b) => a + Number(b.debit || 0), 0))
const totalCredit = computed(() => document.value.items.reduce((a, b) => a + Number(b.credit || 0), 0))
const isBalanced = computed(() => totalDebit.value === totalCredit.value && totalDebit.value > 0)

// --- Interactive Balance Scale Angle ---
const rotationAngle = computed(() => {
  if (totalDebit.value === 0 && totalCredit.value === 0) return 0
  const diff = totalDebit.value - totalCredit.value
  const maxVal = Math.max(totalDebit.value, totalCredit.value)
  // Maps ratio (-1 to 1) to rotation degrees (-14 to +14 deg)
  const angle = (diff / maxVal) * 14
  return Math.min(14, Math.max(-14, angle))
})

function getPercentage(val) {
  const maxVal = Math.max(totalDebit.value, totalCredit.value)
  if (maxVal === 0) return 0
  return (val / maxVal) * 100
}

// --- Helper Functions to get strings inside Printable Ledger ---
function getAccountTitle(id) {
  const found = accounts.find(a => a.id === id)
  return found ? found.title : '—'
}

function getDetailTitle(id) {
  const found = details.find(d => d.id === id)
  return found ? found.title : '—'
}

// --- Methods ---

function addRow() {
  document.value.items.push({
    itemId: Date.now() + Math.random(),
    accountId: null,
    detailId: null,
    description: document.value.description ? `${document.value.description}` : '',
    unit: 1,
    debit: 0,
    credit: 0
  })
}

function removeRow(item) {
  document.value.items = document.value.items.filter(i => i.itemId !== item.itemId)
}

function duplicateRow(idx) {
  const source = document.value.items[idx]
  if (!source) return
  document.value.items.splice(idx + 1, 0, {
    ...source,
    itemId: Date.now() + Math.random(),
    debit: source.debit,
    credit: source.credit
  })
  showToast('ردیف مالی با موفقیت تکثیر شد', 'success')
}

function swapDebitCredit(idx) {
  const item = document.value.items[idx]
  if (!item) return
  const temp = item.debit
  item.debit = item.credit
  item.credit = temp
}

function fillAutoBalance(idx, type) {
  const diff = Math.abs(totalDebit.value - totalCredit.value)
  if (diff === 0) return
  if (type === 'debit') {
    document.value.items[idx].debit = diff
    document.value.items[idx].credit = 0
  } else {
    document.value.items[idx].credit = diff
    document.value.items[idx].debit = 0
  }
  showToast('موازنه خودکار سطر با موفقیت انجام شد', 'info')
}

// Keyboard Navigation support
function handleKeyDown(event, index, field) {
  if (event.key === 'Enter') {
    event.preventDefault()
    // If we're at the last row, auto-create a new row on pressing Enter
    if (index === document.value.items.length - 1) {
      addRow()
      // Wait for DOM render and focus on first input if possible
      nextTick(() => {
        const rows = window.document.querySelectorAll('.doc-row-premium')
        if (rows.length > 0) {
          const nextRowInputs = rows[rows.length - 1].querySelectorAll('input')
          if (nextRowInputs.length > 0) {
            nextRowInputs[0].focus()
          }
        }
      })
    }
  }
}

async function handleSubmit() {
  if (!isBalanced.value) return
  
  if (editingDocumentId.value) {
    const idx = savedDocuments.value.findIndex(d => d.documentId === editingDocumentId.value)
    if (idx !== -1) {
      savedDocuments.value[idx] = {
        ...savedDocuments.value[idx],
        description: document.value.description,
        date: new Date(document.value.date),
        items: [...document.value.items]
      }
    }
    showToast('تغییرات سند حسابداری با موفقیت ذخیره شد', 'success')
  } else {
    savedDocuments.value.push({
      documentId: `doc-${Date.now()}`,
      accountingCode: isManualCode.value ? document.value.accountingCode : `ACC-${String(savedDocuments.value.length + 1).padStart(3, '0')}`,
      date: new Date(document.value.date),
      description: document.value.description,
      isFinalized: false,
      items: [...document.value.items]
    })
    showToast('سند حسابداری جدید با موفقیت ثبت و صادر گردید', 'success')
  }
  createNewDocument()
}

function editDocument(doc) {
  document.value = {
    documentId: doc.documentId,
    description: doc.description,
    date: doc.date instanceof Date ? doc.date.toISOString().split('T')[0] : doc.date,
    accountingCode: doc.accountingCode,
    items: doc.items.map(i => ({ ...i, itemId: Date.now() + Math.random() }))
  }
  editingDocumentId.value = doc.documentId
  isManualCode.value = true
  
  // Smooth scroll back to form
  window.scrollTo({ top: 0, behavior: 'smooth' })
  showToast('در حال ویرایش اطلاعات سند...', 'info')
}

function cancelEdit() {
  createNewDocument()
  showToast('عملیات ویرایش لغو شد', 'info')
}

function deleteDocument(docId) {
  savedDocuments.value = savedDocuments.value.filter(d => d.documentId !== docId)
  showToast('سند حسابداری از آرشیو حذف گردید', 'error')
}

function finalizeDocument(docId) {
  const doc = savedDocuments.value.find(d => d.documentId === docId)
  if (doc) doc.isFinalized = true
  showToast('سند حسابداری نهایی شد و تغییر در آن قفل گردید', 'info')
}

function createNewDocument() {
  document.value = {
    documentId: null,
    description: '',
    date: new Date().toISOString().split('T')[0],
    accountingCode: 'AUTO-001',
    items: []
  }
  editingDocumentId.value = null
  isManualCode.value = false
}

function openLedgerModal(doc) {
  selectedLedgerDoc.value = doc
  showLedgerModal.value = true
}

function openCurrentLedgerModal() {
  // Preview current doc even if not saved
  selectedLedgerDoc.value = {
    accountingCode: isManualCode.value ? document.value.accountingCode : 'ACC-DRAFT',
    date: document.value.date,
    description: document.value.description,
    isFinalized: false,
    items: [...document.value.items]
  }
  showLedgerModal.value = true
}

function closeLedgerModal() {
  showLedgerModal.value = false
  selectedLedgerDoc.value = null
}

function printLedger() {
  window.print()
}

function showToast(text, type) {
  toast.value = { show: true, text, type }
  setTimeout(() => { toast.value.show = false }, 3000)
}

function formatNum(val) {
  return Number(val || 0).toLocaleString('fa-IR')
}

function formatDate(d) {
  if (!d) return '—'
  return new Date(d).toLocaleDateString('fa-IR')
}
</script>

<style scoped>
/* ── Theme Custom System Colors ── */
.page-root {
  padding: 30px 24px 60px;
  background: var(--bg);
  font-family: var(--font);
  font-size: 13.5px;
  direction: rtl;
  min-height: 100%;
  color: var(--text);
  overflow-y: auto;
}

/* ── Typography and number overrides ── */
.font-fa {
  font-family: 'Vazirmatn', sans-serif !important;
  font-feature-settings: "ss01", "ss02"; /* Arabic/Persian glyphs */
}

/* ── Header ── */
.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 26px;
  gap: 20px;
  flex-wrap: wrap;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 16px;
}

.header-icon-wrap {
  width: 48px;
  height: 48px;
  border-radius: 16px;
  background: rgba(56, 189, 248, 0.06);
  border: 1px solid rgba(56, 189, 248, 0.16);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  box-shadow: 0 4px 15px rgba(56, 189, 248, 0.05);
  transition: transform 0.3s;
}

.header-icon-wrap:hover {
  transform: rotate(5deg) scale(1.05);
}

.glow-icon {
  filter: drop-shadow(0 0 4px rgba(56, 189, 248, 0.3));
}

.page-title {
  font-size: 19px;
  font-weight: 850;
  color: #f1f5f9;
  margin: 0;
  line-height: 1.4;
  letter-spacing: -0.3px;
}

.page-sub {
  font-size: 12px;
  color: #475569;
  margin: 4px 0 0;
  font-weight: 500;
}

.btn-preview-ledger-top {
  display: flex;
  align-items: center;
  gap: 8px;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.08);
  color: #94a3b8;
  padding: 8px 16px;
  border-radius: 10px;
  font-weight: 600;
  font-size: 12px;
  cursor: pointer;
  transition: all 0.25s;
}

.btn-preview-ledger-top:hover {
  background: rgba(56, 189, 248, 0.08);
  border-color: rgba(56, 189, 248, 0.3);
  color: var(--cyan);
  box-shadow: 0 4px 12px rgba(56, 189, 248, 0.06);
}

/* ── Glass Card base ── */
.glass-card {
  background: rgba(13, 17, 23, 0.45);
  backdrop-filter: blur(14px);
  -webkit-backdrop-filter: blur(14px);
  border: 1px solid rgba(255, 255, 255, 0.045);
  border-radius: 18px;
  padding: 22px 24px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.glass-card:hover {
  border-color: rgba(255, 255, 255, 0.07);
}

/* ── Section Title ── */
.card-section-title {
  display: flex;
  align-items: center;
  gap: 9px;
  font-size: 12px;
  font-weight: 800;
  color: #64748b;
  letter-spacing: 0.5px;
  text-transform: uppercase;
  margin-bottom: 20px;
}

.card-section-title svg {
  color: var(--cyan);
  filter: drop-shadow(0 0 2px rgba(56, 189, 248, 0.25));
}

.mb-0 {
  margin-bottom: 0 !important;
}

.mb-4 {
  margin-bottom: 18px;
}

/* ── 2. Hero Stats Dashboard ── */
.hero-stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 16px;
}

.stat-card {
  display: flex;
  flex-direction: column;
  position: relative;
  overflow: hidden;
  padding: 16px 20px;
}

.stat-card::after {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 4px;
  height: 100%;
}

.stat-card:nth-child(1)::after { background: var(--purple); }
.stat-card:nth-child(2)::after { background: var(--green); }
.stat-card:nth-child(3)::after { background: var(--cyan); }
.stat-card:nth-child(4)::after { background: var(--amber); }

.stat-icon-wrap {
  position: absolute;
  top: 14px;
  left: 16px;
  width: 36px;
  height: 36px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 0.75;
}

.icon-purple { background: rgba(167, 139, 250, 0.08); color: var(--purple); }
.icon-emerald { background: rgba(52, 211, 153, 0.08); color: var(--green); }
.icon-cyan { background: rgba(56, 189, 248, 0.08); color: var(--cyan); }
.icon-amber { background: rgba(251, 191, 36, 0.08); color: var(--amber); }

.stat-info {
  margin-bottom: 6px;
}

.stat-label {
  font-size: 11px;
  font-weight: 700;
  color: #475569;
  display: block;
}

.stat-value {
  font-size: 18px;
  font-weight: 900;
  color: #e2e8f0;
  display: block;
  margin-top: 4px;
}

.text-emerald { color: #34d399 !important; }
.text-rose { color: #f87171 !important; }
.text-cyan { color: #38bdf8 !important; }
.text-amber { color: #fbbf24 !important; }

.stat-caption {
  font-size: 10px;
  color: #334155;
  margin-top: auto;
}

.stat-mini-details {
  display: flex;
  gap: 6px;
  margin-top: auto;
}

.pill-draft, .pill-final {
  font-size: 9px;
  font-weight: 700;
  padding: 1px 6px;
  border-radius: 4px;
}

.pill-draft { background: rgba(251, 191, 36, 0.08); color: var(--amber); border: 1px solid rgba(251, 191, 36, 0.15); }
.pill-final { background: rgba(52, 211, 153, 0.08); color: var(--green); border: 1px solid rgba(52, 211, 153, 0.15); }

/* ── 3. Interactive Balance Scale ── */
.balance-panel-wrapper {
  background: linear-gradient(135deg, rgba(13, 17, 23, 0.5) 0%, rgba(20, 26, 38, 0.4) 100%);
  border: 1px solid rgba(56, 189, 248, 0.08);
}

.balance-content-row {
  display: flex;
  align-items: center;
  gap: 24px;
  flex-wrap: wrap;
}

.scale-visual-wrap {
  flex-shrink: 0;
  width: 160px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.balance-scale-svg {
  filter: drop-shadow(0 4px 15px rgba(0, 0, 0, 0.4));
}

.scale-beam {
  transform-origin: 100px 50px;
  transition: transform 0.8s cubic-bezier(0.25, 0.8, 0.25, 1);
}

.scale-pan-left, .scale-pan-right {
  transition: transform 0.8s cubic-bezier(0.25, 0.8, 0.25, 1);
}

.scale-data-wrap {
  flex: 1;
  min-width: 250px;
}

.scale-data-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 12px;
  flex-wrap: wrap;
  gap: 10px;
}

.scale-title {
  font-size: 13.5px;
  font-weight: 800;
  color: #94a3b8;
}

/* Balance Indicators */
.balance-indicator-badge {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 4px 12px;
  border-radius: 99px;
  font-size: 11px;
  font-weight: 800;
  border: 1px solid;
  transition: all 0.3s;
}

.bal-ok {
  background: rgba(52, 211, 153, 0.08);
  border-color: rgba(52, 211, 153, 0.25);
  color: var(--green);
  box-shadow: 0 0 10px rgba(52, 211, 153, 0.06);
}

.bal-err {
  background: rgba(248, 113, 113, 0.08);
  border-color: rgba(248, 113, 113, 0.25);
  color: var(--red);
  box-shadow: 0 0 10px rgba(248, 113, 113, 0.06);
}

.bal-neutral {
  background: rgba(71, 85, 105, 0.1);
  border-color: rgba(71, 85, 105, 0.2);
  color: var(--muted);
}

.bal-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: currentColor;
  animation: pulse 1.6s infinite;
}

@keyframes pulse {
  0%, 100% { opacity: 1; transform: scale(1); }
  50% { opacity: 0.4; transform: scale(0.8); }
}

/* Dual Progress bars */
.scale-bars-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
  margin-bottom: 12px;
}

@media (max-width: 600px) {
  .scale-bars-grid {
    grid-template-columns: 1fr;
    gap: 8px;
  }
}

.scale-bar-item {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.scale-bar-label {
  display: flex;
  justify-content: space-between;
  font-size: 11px;
  color: #475569;
}

.progress-track {
  height: 6px;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.04);
  border-radius: 99px;
  overflow: hidden;
}

.progress-bar {
  height: 100%;
  width: 0;
  border-radius: 99px;
  transition: width 0.6s cubic-bezier(0.25, 0.8, 0.25, 1);
}

.fill-emerald {
  background: linear-gradient(90deg, #10b981, #34d399);
  box-shadow: 0 0 6px rgba(52, 211, 153, 0.3);
}

.fill-rose {
  background: linear-gradient(90deg, #f43f5e, #fb7171);
  box-shadow: 0 0 6px rgba(248, 113, 113, 0.3);
}

.scale-diff-alert {
  display: flex;
  align-items: center;
  gap: 8px;
  background: rgba(251, 191, 36, 0.06);
  border: 1px solid rgba(251, 191, 36, 0.16);
  border-radius: 10px;
  padding: 8px 14px;
  font-size: 11px;
  color: var(--amber);
}

.scale-diff-alert strong {
  font-weight: 900;
}

.diff-desc {
  font-size: 10px;
  color: #b45309;
  opacity: 0.8;
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-3px); }
  75% { transform: translateX(3px); }
}

.animate-shake {
  animation: shake 0.4s ease-in-out;
}

/* ── 4. Primary Form Card ── */
.primary-form-card {
  background: rgba(13, 17, 23, 0.45);
}

.form-grid {
  display: grid;
  grid-template-columns: 2fr 1fr 1.2fr;
  gap: 16px;
}

.span-2 {
  grid-column: span 1; /* reset on full grid if 3 columns */
}

@media (max-width: 900px) {
  .form-grid {
    grid-template-columns: 1fr 1fr;
  }
}

@media (max-width: 550px) {
  .form-grid {
    grid-template-columns: 1fr;
  }
}

.field-label {
  display: block;
  font-size: 11px;
  font-weight: 800;
  color: #475569;
  margin-bottom: 6px;
}

/* Custom code toggler */
.code-row {
  display: flex;
  align-items: center;
  gap: 12px;
}

.toggle-wrap {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  user-select: none;
}

.toggle-input {
  display: none;
}

.toggle-track {
  width: 34px;
  height: 19px;
  border-radius: 99px;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.08);
  position: relative;
  transition: all 0.3s;
}

.toggle-input:checked + .toggle-track {
  background: rgba(56, 189, 248, 0.2);
  border-color: var(--cyan);
}

.toggle-thumb {
  position: absolute;
  width: 13px;
  height: 13px;
  border-radius: 50%;
  background: #334155;
  top: 2px;
  right: 2px;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.toggle-input:checked + .toggle-track .toggle-thumb {
  right: calc(100% - 15px);
  background: var(--cyan);
  box-shadow: 0 0 6px rgba(56, 189, 248, 0.5);
}

.toggle-label {
  font-size: 11px;
  color: #475569;
  font-weight: 700;
}

/* ── Premium Input Overrides ── */
:deep(.dark-field-premium .v-field),
:deep(.dark-field-table .v-field) {
  background: rgba(0, 0, 0, 0.4) !important;
  font-family: var(--font) !important;
  font-size: 13px !important;
  transition: all 0.25s !important;
}

:deep(.dark-field-premium .v-field:hover),
:deep(.dark-field-table .v-field:hover) {
  background: rgba(0, 0, 0, 0.55) !important;
}

:deep(.dark-field-premium .v-field__outline__start),
:deep(.dark-field-premium .v-field__outline__end),
:deep(.dark-field-premium .v-field__outline__notch),
:deep(.dark-field-table .v-field__outline__start),
:deep(.dark-field-table .v-field__outline__end),
:deep(.dark-field-table .v-field__outline__notch) {
  border-color: rgba(255, 255, 255, 0.05) !important;
}

/* Glow focus state */
:deep(.dark-field-premium .v-field--focused .v-field__outline__start),
:deep(.dark-field-premium .v-field--focused .v-field__outline__end),
:deep(.dark-field-premium .v-field--focused .v-field__outline__notch) {
  border-color: rgba(56, 189, 248, 0.5) !important;
  box-shadow: 0 0 8px rgba(56, 189, 248, 0.1) !important;
}

:deep(.dark-field-premium input),
:deep(.dark-field-premium .v-select__selection-text),
:deep(.dark-field-table input),
:deep(.dark-field-table .v-select__selection-text) {
  color: #f1f5f9 !important;
  font-family: var(--font) !important;
}

.disabled-field {
  opacity: 0.5;
  cursor: not-allowed;
}

/* ── 5. Smart Table ── */
.table-card {
  background: rgba(13, 17, 23, 0.45);
}

.table-toolbar-actions {
  display: flex;
  align-items: center;
  gap: 14px;
}

.keyboard-tip {
  font-size: 10px;
  color: #334155;
  font-weight: 500;
}

@media (max-width: 600px) {
  .table-toolbar {
    flex-direction: column;
    align-items: flex-start;
    gap: 12px;
  }
  .table-toolbar-actions {
    width: 100%;
    justify-content: space-between;
  }
}

.row-count {
  background: rgba(56, 189, 248, 0.08);
  color: var(--cyan);
  font-size: 11px;
  padding: 3px 10px;
  border-radius: 99px;
  font-weight: 800;
  border: 1px solid rgba(56, 189, 248, 0.15);
  margin-right: 8px;
}

.btn-add-row-premium {
  display: flex;
  align-items: center;
  gap: 6px;
  background: linear-gradient(135deg, rgba(3, 105, 161, 0.15), rgba(14, 165, 233, 0.15));
  border: 1px solid rgba(56, 189, 248, 0.25);
  color: var(--cyan);
  border-radius: 10px;
  padding: 8px 16px;
  font-size: 12px;
  font-weight: 850;
  cursor: pointer;
  box-shadow: 0 4px 12px rgba(56, 189, 248, 0.04);
  transition: all 0.25s;
}

.btn-add-row-premium:hover {
  background: linear-gradient(135deg, rgba(3, 105, 161, 0.25), rgba(14, 165, 233, 0.25));
  transform: translateY(-1px);
  box-shadow: 0 4px 15px rgba(56, 189, 248, 0.08);
}

/* Empty State Premium */
.empty-state-premium {
  text-align: center;
  padding: 45px 20px;
}

.empty-icon-wrap-premium {
  width: 60px;
  height: 60px;
  border-radius: 18px;
  background: rgba(56, 189, 248, 0.03);
  border: 1px solid rgba(56, 189, 248, 0.08);
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 16px;
  color: var(--cyan);
  filter: drop-shadow(0 4px 8px rgba(0, 0, 0, 0.2));
}

.empty-title-premium {
  font-size: 13.5px;
  font-weight: 800;
  color: #94a3b8;
}

.empty-body-premium {
  font-size: 11.5px;
  color: #475569;
  margin-top: 6px;
}

.btn-add-row-empty {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: var(--cyan);
  color: #020617;
  border: none;
  font-weight: 800;
  font-size: 12px;
  padding: 8px 18px;
  border-radius: 10px;
  cursor: pointer;
  transition: all 0.2s;
  box-shadow: 0 4px 12px rgba(56, 189, 248, 0.2);
}

.btn-add-row-empty:hover {
  opacity: 0.9;
  transform: scale(1.02);
}

/* Table Scroll Wrapper */
.table-scroll-wrap-premium {
  overflow-x: auto;
  border-radius: 12px;
  border: 1px solid rgba(255, 255, 255, 0.03);
  scrollbar-width: thin;
  scrollbar-color: rgba(255, 255, 255, 0.05) transparent;
}

.table-scroll-wrap-premium::-webkit-scrollbar {
  width: 5px;
  height: 5px;
}

.table-scroll-wrap-premium::-webkit-scrollbar-thumb {
  background: rgba(255, 255, 255, 0.07);
  border-radius: 99px;
}

/* Table Design */
.doc-table-premium {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
  text-align: right;
}

.doc-table-premium thead tr {
  background: rgba(7, 10, 16, 0.85);
}

.doc-table-premium th {
  padding: 12px 14px;
  font-size: 11px;
  font-weight: 850;
  color: #475569;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  white-space: nowrap;
}

.th-num { width: 45px; text-align: center; }
.th-unit { width: 100px; }
.th-amount-head { min-width: 140px; }
.th-action { width: 110px; text-align: center; }

.doc-table-premium td {
  padding: 8px 10px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.025);
  vertical-align: middle;
}

.doc-row-premium {
  transition: all 0.2s;
}

.doc-row-premium:hover {
  background: rgba(255, 255, 255, 0.012);
}

/* Focus color row hints */
.row-debit-active td:nth-child(6) {
  background: rgba(52, 211, 153, 0.02);
}
.row-credit-active td:nth-child(7) {
  background: rgba(248, 113, 113, 0.02);
}

.td-num {
  text-align: center;
  color: #334155;
  font-weight: 700;
  font-size: 12px;
}

.tf { min-width: 155px; }
.tf-sm { min-width: 85px; }
.tf-num { min-width: 125px; }
.tf-desc-input { min-width: 200px; }

/* Focus outlines on debit/credit cells */
:deep(.debit-field .v-field--focused .v-field__outline__start),
:deep(.debit-field .v-field--focused .v-field__outline__end),
:deep(.debit-field .v-field--focused .v-field__outline__notch) {
  border-color: rgba(52, 211, 153, 0.5) !important;
  box-shadow: 0 0 6px rgba(52, 211, 153, 0.15) !important;
}

:deep(.credit-field .v-field--focused .v-field__outline__start),
:deep(.credit-field .v-field--focused .v-field__outline__end),
:deep(.credit-field .v-field--focused .v-field__outline__notch) {
  border-color: rgba(248, 113, 113, 0.5) !important;
  box-shadow: 0 0 6px rgba(248, 113, 113, 0.15) !important;
}

/* Magic balance button */
.magic-balance-btn {
  background: transparent;
  border: none;
  color: var(--cyan);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 4px;
  border-radius: 4px;
  transition: all 0.2s;
  opacity: 0.7;
}

.magic-balance-btn:hover {
  opacity: 1;
  background: rgba(56, 189, 248, 0.12);
  transform: scale(1.1);
}

.magic-sparkle {
  filter: drop-shadow(0 0 2px rgba(56, 189, 248, 0.4));
  animation: spin-pulse 3s infinite linear;
}

@keyframes spin-pulse {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

/* Row utils actions */
.row-util-actions {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 4px;
}

.btn-row-action {
  width: 26px;
  height: 26px;
  border-radius: 6px;
  border: 1px solid transparent;
  background: transparent;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s;
}

.swap-act { color: #60a5fa; }
.swap-act:hover { background: rgba(96, 165, 250, 0.1); border-color: rgba(96, 165, 250, 0.2); }

.duplicate-act { color: #c084fc; }
.duplicate-act:hover { background: rgba(192, 132, 252, 0.1); border-color: rgba(192, 132, 252, 0.2); }

.delete-act { color: #f87171; }
.delete-act:hover { background: rgba(248, 113, 113, 0.1); border-color: rgba(248, 113, 113, 0.2); }

/* Row transitions */
.row-anim-enter-active, .row-anim-leave-active {
  transition: all 0.35s ease;
}
.row-anim-enter-from {
  opacity: 0;
  transform: translateY(-8px);
}
.row-anim-leave-to {
  opacity: 0;
  transform: translateY(8px);
}

/* Totals Footer Premium */
.totals-footer-premium {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  padding: 16px 8px 4px;
  border-top: 1px solid rgba(255, 255, 255, 0.03);
  flex-wrap: wrap;
}

.totals-label-premium {
  font-size: 12.5px;
  font-weight: 850;
  color: #475569;
}

.totals-chips-premium {
  display: flex;
  align-items: center;
  gap: 10px;
  flex-wrap: wrap;
}

.total-chip-premium {
  display: flex;
  flex-direction: column;
  padding: 5px 16px;
  border-radius: 12px;
  border: 1px solid;
  min-width: 130px;
}

.chip-debit-p {
  background: rgba(52, 211, 153, 0.04);
  border-color: rgba(52, 211, 153, 0.15);
  color: var(--green);
}

.chip-credit-p {
  background: rgba(248, 113, 113, 0.04);
  border-color: rgba(248, 113, 113, 0.15);
  color: var(--red);
}

.chip-diff-p {
  background: rgba(251, 191, 36, 0.04);
  border-color: rgba(251, 191, 36, 0.15);
  color: var(--amber);
}

.chip-label-p {
  font-size: 9.5px;
  font-weight: 700;
  opacity: 0.7;
  margin-bottom: 2px;
}

.chip-value-p {
  font-size: 14px;
  font-weight: 900;
  font-variant-numeric: tabular-nums;
}

.val-unit {
  font-size: 9px;
  font-weight: 500;
}

.total-sep-p {
  display: flex;
  align-items: center;
}

.total-balanced-p {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 12px;
  font-weight: 800;
  color: var(--green);
  background: rgba(52, 211, 153, 0.08);
  border: 1px solid rgba(52, 211, 153, 0.2);
  padding: 6px 16px;
  border-radius: 99px;
  box-shadow: 0 0 12px rgba(52, 211, 153, 0.06);
}

@keyframes glow {
  0%, 100% { box-shadow: 0 0 8px rgba(52, 211, 153, 0.04); }
  50% { box-shadow: 0 0 16px rgba(52, 211, 153, 0.15); }
}

.animate-glow {
  animation: glow 3s infinite ease-in-out;
}

/* ── 6. Action Bar Premium ── */
.action-bar-premium {
  display: flex;
  align-items: center;
  gap: 12px;
  margin: 20px 0;
}

.btn-submit-premium {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: linear-gradient(135deg, #0284c7, #0ea5e9);
  color: #ffffff;
  border: none;
  font-weight: 850;
  font-size: 13px;
  padding: 10px 24px;
  border-radius: 12px;
  cursor: pointer;
  box-shadow: 0 6px 20px rgba(14, 165, 233, 0.2);
  transition: all 0.25s;
}

.btn-submit-premium:hover:not(:disabled) {
  transform: translateY(-1.5px);
  box-shadow: 0 8px 25px rgba(14, 165, 233, 0.28);
  filter: brightness(1.05);
}

.btn-submit-premium:disabled {
  background: #1e293b;
  color: #475569;
  box-shadow: none;
  cursor: not-allowed;
  opacity: 0.6;
}

.btn-cancel-premium {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.08);
  color: #94a3b8;
  font-weight: 700;
  font-size: 13px;
  padding: 10px 20px;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.25s;
}

.btn-cancel-premium:hover {
  background: rgba(255, 255, 255, 0.06);
  color: #f1f5f9;
}

/* ── 7. Section Sep ── */
.section-sep {
  display: flex;
  align-items: center;
  gap: 14px;
  margin: 32px 0 16px;
}

.sep-line {
  flex: 1;
  height: 1px;
  background: linear-gradient(to left, transparent, rgba(255, 255, 255, 0.05), transparent);
}

.sep-label {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 12px;
  font-weight: 850;
  color: #334155;
  white-space: nowrap;
}

/* ── 8. Saved Documents List ── */
.archive-card {
  background: rgba(13, 17, 23, 0.45);
}

.doc-count-badge-premium {
  font-size: 11px;
  color: #475569;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.05);
  padding: 4px 12px;
  border-radius: 99px;
  font-weight: 700;
}

.th-code { width: 110px; }
.th-date { width: 100px; }
.th-status { width: 140px; }
.th-action-archive { width: 130px; text-align: center; }

.code-chip-premium {
  font-size: 11px;
  font-weight: 850;
  color: var(--cyan);
  background: rgba(56, 189, 248, 0.06);
  border: 1px solid rgba(56, 189, 248, 0.15);
  padding: 3px 10px;
  border-radius: 6px;
  white-space: nowrap;
}

.td-date {
  color: #64748b;
  font-size: 12px;
  white-space: nowrap;
}

.td-desc-archive {
  color: #94a3b8;
  max-width: 250px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.td-amount-archive {
  font-weight: 850;
  font-size: 13.5px;
}

/* Status pills */
.status-pill-premium {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 3px 10px;
  border-radius: 99px;
  font-size: 11px;
  font-weight: 850;
  border: 1px solid;
}

.pill-final-active {
  background: rgba(52, 211, 153, 0.06);
  border-color: rgba(52, 211, 153, 0.18);
  color: var(--green);
}

.pill-draft-active {
  background: rgba(251, 191, 36, 0.06);
  border-color: rgba(251, 191, 36, 0.18);
  color: var(--amber);
}

.pill-dot-premium {
  width: 5px;
  height: 5px;
  border-radius: 50%;
  background: currentColor;
}

/* Row actions archive */
.row-actions-archive {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 4px;
}

.btn-arch-act {
  width: 26px;
  height: 26px;
  border-radius: 6px;
  border: 1px solid transparent;
  background: transparent;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s;
}

.act-view { color: #94a3b8; }
.act-view:hover { background: rgba(255, 255, 255, 0.05); border-color: rgba(255, 255, 255, 0.12); }

.act-edit { color: var(--cyan); }
.act-edit:hover { background: rgba(56, 189, 248, 0.08); border-color: rgba(56, 189, 248, 0.15); }

.act-final { color: var(--green); }
.act-final:hover { background: rgba(52, 211, 153, 0.08); border-color: rgba(52, 211, 153, 0.15); }

.act-del { color: var(--red); }
.act-del:hover { background: rgba(248, 113, 113, 0.08); border-color: rgba(248, 113, 113, 0.15); }

.locked-indicator {
  color: #334155;
  display: inline-flex;
  padding: 4px;
}

/* ── 9. Official Persian Ledger Viewer Modal ── */
.ledger-modal-wrapper {
  background: #111827;
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 18px;
  overflow: hidden;
  padding: 0 !important;
}

.ledger-modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 24px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  background: #0b0f17;
}

.ledger-modal-title {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13.5px;
  font-weight: 850;
  color: #f1f5f9;
}

.ledger-modal-actions {
  display: flex;
  align-items: center;
  gap: 10px;
}

.btn-ledger-print {
  display: flex;
  align-items: center;
  gap: 6px;
  background: var(--cyan);
  color: #020617;
  border: none;
  font-weight: 850;
  font-size: 12px;
  padding: 6px 14px;
  border-radius: 8px;
  cursor: pointer;
  transition: opacity 0.2s;
}

.btn-ledger-print:hover { opacity: 0.9; }

.btn-ledger-close {
  background: transparent;
  border: none;
  color: #475569;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: color 0.2s;
}

.btn-ledger-close:hover { color: #f1f5f9; }

/* Printable Ledger View Style */
.print-ledger-container {
  padding: 30px;
  background: #ffffff;
  color: #0f172a;
  min-height: 500px;
  direction: rtl;
}

.print-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 2px double #1e293b;
  padding-bottom: 16px;
  margin-bottom: 16px;
}

.print-company-name {
  font-size: 16px;
  font-weight: 900;
  color: #000000;
  margin: 0;
}

.print-doc-title {
  font-size: 13px;
  font-weight: 700;
  color: #475569;
  margin: 6px 0 0;
}

.print-logo-dummy {
  font-size: 18px;
  font-weight: 900;
  font-style: italic;
  color: #1e3a8a;
  border: 1px solid #1e3a8a;
  padding: 4px 12px;
  border-radius: 6px;
}

.print-header-left {
  font-size: 11.5px;
  line-height: 1.6;
}

.print-main-meta {
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  padding: 10px 14px;
  font-size: 12px;
  margin-bottom: 16px;
}

/* Ledger table inside print container */
.print-ledger-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 12px;
  margin-bottom: 26px;
}

.print-ledger-table th {
  background: #f1f5f9;
  border: 1px solid #cbd5e1;
  padding: 8px;
  font-weight: 800;
  color: #0f172a;
}

.print-ledger-table td {
  border: 1px solid #cbd5e1;
  padding: 8px;
  color: #334155;
}

.print-totals-row td {
  background: #f8fafc;
  border-top: 2px double #000;
  color: #000000;
}

.print-signatures {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
  margin-top: 40px;
  border-top: 1px dashed #cbd5e1;
  padding-top: 16px;
}

.sig-box {
  display: flex;
  flex-direction: column;
  align-items: center;
  font-size: 11px;
  font-weight: 700;
  color: #475569;
}

.sig-space {
  height: 50px;
}

/* ── 10. Premium Toast Alert ── */
.toast-premium {
  position: fixed;
  bottom: 24px;
  left: 24px;
  display: flex;
  flex-direction: column;
  border-radius: 12px;
  font-size: 13.5px;
  font-weight: 800;
  z-index: 99999;
  backdrop-filter: blur(14px);
  -webkit-backdrop-filter: blur(14px);
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.45);
  overflow: hidden;
  border: 1px solid;
  min-width: 250px;
}

.toast-content {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 14px 18px;
}

.toast-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 24px;
  height: 24px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.1);
}

.toast-success {
  background: rgba(16, 185, 129, 0.12);
  border-color: rgba(52, 211, 153, 0.25);
  color: var(--green);
}

.toast-error {
  background: rgba(239, 68, 68, 0.12);
  border-color: rgba(248, 113, 113, 0.25);
  color: var(--red);
}

.toast-info {
  background: rgba(14, 165, 233, 0.12);
  border-color: rgba(56, 189, 248, 0.25);
  color: var(--cyan);
}

.toast-timer-bar {
  height: 3px;
  width: 100%;
  background: currentColor;
  opacity: 0.6;
  align-self: flex-start;
  animation: timer-shrink linear forwards;
}

@keyframes timer-shrink {
  from { width: 100%; }
  to { width: 0%; }
}

.toast-enter-active,
.toast-leave-active {
  transition: all 0.35s cubic-bezier(0.175, 0.885, 0.32, 1.275);
}

.toast-enter-from {
  opacity: 0;
  transform: translateY(20px) scale(0.9);
}
.toast-leave-to {
  opacity: 0;
  transform: translateY(-20px) scale(0.9);
}

/* ── Print Media Query (Failsafe) ── */
@media print {
  body {
    background: #ffffff !important;
    color: #000000 !important;
  }
  .no-print,
  header,
  section,
  div.action-bar-premium,
  div.section-sep,
  .v-overlay-container,
  .v-dialog {
    display: none !important;
    visibility: hidden !important;
  }
  
  .v-overlay-active {
    display: block !important;
    position: static !important;
  }

  .ledger-modal-wrapper {
    border: none !important;
    background: transparent !important;
    box-shadow: none !important;
  }

  .print-ledger-container {
    display: block !important;
    visibility: visible !important;
    position: absolute;
    left: 0;
    top: 0;
    width: 100%;
    margin: 0;
    padding: 0;
  }
}
</style>
