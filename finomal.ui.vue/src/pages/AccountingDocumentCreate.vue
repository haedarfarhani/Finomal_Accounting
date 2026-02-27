<template>
  <div class="page-root">

    <!-- ===== Page Header ===== -->
    <header class="page-header">
      <div class="header-right">
        <div class="header-icon-wrap">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#38bdf8" stroke-width="2">
            <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z" />
            <polyline points="14 2 14 8 20 8" />
            <line x1="12" y1="18" x2="12" y2="12" />
            <line x1="9" y1="15" x2="15" y2="15" />
          </svg>
        </div>
        <div>
          <h1 class="page-title">{{ editingDocumentId ? 'ویرایش سند حسابداری' : 'ثبت سند حسابداری جدید' }}</h1>
          <p class="page-sub">سیستم یکپارچه مالی فینومال</p>
        </div>
      </div>
      <div class="header-left">
        <div class="balance-indicator"
          :class="isBalanced ? 'bal-ok' : totalDebit === 0 && totalCredit === 0 ? 'bal-neutral' : 'bal-err'">
          <span class="bal-dot"></span>
          <span>{{ isBalanced ? 'سند متوازن' : totalDebit === 0 && totalCredit === 0 ? 'در انتظار ورود' : 'عدم توازن'
            }}</span>
          <span v-if="!isBalanced && (totalDebit > 0 || totalCredit > 0)" class="bal-diff">
            اختلاف: {{ formatNum(Math.abs(totalDebit - totalCredit)) }}
          </span>
        </div>
      </div>
    </header>

    <!-- ===== Main Form Card ===== -->
    <section class="glass-card mb-4">
      <div class="card-section-title">
        <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <circle cx="12" cy="12" r="10" />
          <line x1="12" y1="8" x2="12" y2="12" />
          <line x1="12" y1="16" x2="12.01" y2="16" />
        </svg>
        اطلاعات اصلی سند
      </div>

      <div class="form-grid">
        <div class="field-wrap span-2">
          <label class="field-label">شرح سند</label>
          <v-text-field v-model="document.description" variant="outlined" density="compact"
            placeholder="توضیحات سند را وارد کنید..." class="dark-field" hide-details rounded="lg" />
        </div>

        <div class="field-wrap">
          <label class="field-label">تاریخ</label>
          <v-text-field v-model="document.date" type="date" variant="outlined" density="compact" class="dark-field"
            hide-details rounded="lg" />
        </div>

        <div class="field-wrap">
          <label class="field-label">کد حسابداری</label>
          <div class="code-row">
            <label class="toggle-wrap">
              <input type="checkbox" v-model="isManualCode" class="toggle-input" />
              <span class="toggle-track"><span class="toggle-thumb"></span></span>
              <span class="toggle-label">دستی</span>
            </label>
            <v-text-field v-model="document.accountingCode" variant="outlined" density="compact"
              :disabled="!isManualCode" class="dark-field flex-1" hide-details rounded="lg" />
          </div>
        </div>
      </div>
    </section>

    <!-- ===== Items Table ===== -->
    <section class="glass-card mb-4">
      <div class="table-toolbar">
        <div class="card-section-title mb-0">
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <rect x="3" y="3" width="18" height="18" rx="2" />
            <path d="M3 9h18M3 15h18M9 3v18" />
          </svg>
          ردیف‌های سند
          <span v-if="document.items.length > 0" class="row-count">{{ document.items.length }} ردیف</span>
        </div>
        <button class="btn-add-row" @click="addRow">
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <line x1="12" y1="5" x2="12" y2="19" />
            <line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          افزودن ردیف
        </button>
      </div>

      <!-- Empty -->
      <div v-if="document.items.length === 0" class="empty-state">
        <div class="empty-icon-wrap">
          <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <rect x="3" y="3" width="18" height="18" rx="2" />
            <path d="M3 9h18M9 3v18" />
            <path d="M15 15l-3-3m0 0l-3-3m3 3l3-3m-3 3l-3 3" stroke-dasharray="2 2" />
          </svg>
        </div>
        <div class="empty-title">ردیفی وجود ندارد</div>
        <div class="empty-body">برای شروع روی «افزودن ردیف» کلیک کنید</div>
      </div>

      <!-- Table with internal scroll -->
      <div v-else class="table-scroll-wrap">
        <table class="doc-table">
          <thead>
            <tr>
              <th class="th-num">#</th>
              <th>حساب</th>
              <th>تفصیل</th>
              <th>شرح ردیف</th>
              <th>واحد</th>
              <th class="th-amount">بدهکار</th>
              <th class="th-amount">بستانکار</th>
              <th class="th-action"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in document.items" :key="item.itemId" class="doc-row"
              :class="{ 'row-debit': item.debit > 0, 'row-credit': item.credit > 0 }">
              <td class="td-num">{{ index + 1 }}</td>
              <td>
                <v-select v-model="item.accountId" :items="accounts" item-title="title" item-value="id"
                  variant="outlined" density="compact" class="dark-field tf" hide-details rounded="lg" />
              </td>
              <td>
                <v-select v-model="item.detailId" :items="details" item-title="title" item-value="id" variant="outlined"
                  density="compact" class="dark-field tf" hide-details rounded="lg" />
              </td>
              <td>
                <v-text-field v-model="item.description" variant="outlined" density="compact" class="dark-field tf"
                  hide-details rounded="lg" />
              </td>
              <td>
                <v-select v-model="item.unit" :items="units" item-title="title" item-value="id" variant="outlined"
                  density="compact" class="dark-field tf tf-sm" hide-details rounded="lg" />
              </td>
              <td>
                <v-text-field v-model.number="item.debit" type="number" variant="outlined" density="compact"
                  class="dark-field tf tf-num debit-field" hide-details rounded="lg" @input="item.credit = 0" />
              </td>
              <td>
                <v-text-field v-model.number="item.credit" type="number" variant="outlined" density="compact"
                  class="dark-field tf tf-num credit-field" hide-details rounded="lg" @input="item.debit = 0" />
              </td>
              <td class="td-action">
                <button class="btn-del-row" @click="removeRow(item)" title="حذف ردیف">
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <polyline points="3 6 5 6 21 6" />
                    <path d="M19 6l-1 14H6L5 6" />
                    <path d="M10 11v6M14 11v6" />
                    <path d="M9 6V4h6v2" />
                  </svg>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Totals -->
      <div v-if="document.items.length > 0" class="totals-footer">
        <span class="totals-label">جمع کل</span>
        <div class="totals-chips">
          <div class="total-chip chip-debit">
            <span class="chip-label">بدهکار</span>
            <span class="chip-value">{{ formatNum(totalDebit) }}</span>
          </div>
          <div class="total-sep">=</div>
          <div class="total-chip chip-credit">
            <span class="chip-label">بستانکار</span>
            <span class="chip-value">{{ formatNum(totalCredit) }}</span>
          </div>
          <div v-if="!isBalanced" class="total-chip chip-diff">
            <span class="chip-label">اختلاف</span>
            <span class="chip-value">{{ formatNum(Math.abs(totalDebit - totalCredit)) }}</span>
          </div>
          <div v-else class="total-balanced">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <polyline points="20 6 9 17 4 12" />
            </svg>
            متوازن
          </div>
        </div>
      </div>
    </section>

    <!-- ===== Actions ===== -->
    <div class="action-bar">
      <button class="btn-submit" :disabled="!isBalanced || totalDebit === 0" @click="handleSubmit">
        <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <path d="M19 21H5a2 2 0 01-2-2V5a2 2 0 012-2h11l5 5v14z" />
          <polyline points="17 21 17 13 7 13 7 21" />
          <polyline points="7 3 7 8 15 8" />
        </svg>
        {{ editingDocumentId ? 'ذخیره تغییرات' : 'ذخیره سند' }}
      </button>
      <button v-if="editingDocumentId" class="btn-cancel" @click="cancelEdit">
        <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <line x1="18" y1="6" x2="6" y2="18" />
          <line x1="6" y1="6" x2="18" y2="18" />
        </svg>
        انصراف از ویرایش
      </button>
    </div>

    <!-- ===== Divider ===== -->
    <div class="section-sep">
      <div class="sep-line"></div>
      <div class="sep-label">
        <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <line x1="8" y1="6" x2="21" y2="6" />
          <line x1="8" y1="12" x2="21" y2="12" />
          <line x1="8" y1="18" x2="21" y2="18" />
          <line x1="3" y1="6" x2="3.01" y2="6" />
          <line x1="3" y1="12" x2="3.01" y2="12" />
          <line x1="3" y1="18" x2="3.01" y2="18" />
        </svg>
        اسناد ثبت‌شده
      </div>
      <div class="sep-line"></div>
    </div>

    <!-- ===== Documents List ===== -->
    <section class="glass-card">
      <div class="list-toolbar">
        <div class="card-section-title mb-0">
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <path
              d="M21.44 11.05l-9.19 9.19a6 6 0 01-8.49-8.49l9.19-9.19a4 4 0 015.66 5.66l-9.2 9.19a2 2 0 01-2.83-2.83l8.49-8.48" />
          </svg>
          آرشیو اسناد
        </div>
        <span class="doc-count-badge">{{ savedDocuments.length }} سند</span>
      </div>

      <div v-if="savedDocuments.length === 0" class="empty-state">
        <div class="empty-icon-wrap">
          <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <path d="M22 12h-4l-3 9L9 3l-3 9H2" />
          </svg>
        </div>
        <div class="empty-title">سندی ثبت نشده</div>
      </div>

      <!-- Scrollable list table -->
      <div v-else class="table-scroll-wrap">
        <table class="doc-table">
          <thead>
            <tr>
              <th>کد سند</th>
              <th>تاریخ</th>
              <th>شرح سند</th>
              <th class="th-amount">جمع بدهکار</th>
              <th>وضعیت</th>
              <th class="th-action">عملیات</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="doc in savedDocuments" :key="doc.documentId" class="doc-row">
              <td>
                <span class="code-chip">{{ doc.accountingCode }}</span>
              </td>
              <td class="td-date">{{ formatDate(doc.date) }}</td>
              <td class="td-desc">{{ doc.description }}</td>
              <td class="th-amount">
                <span class="amount-val">{{formatNum(doc.items.reduce((a, b) => a + Number(b.debit || 0), 0))}}</span>
              </td>
              <td>
                <span class="status-pill" :class="doc.isFinalized ? 'pill-final' : 'pill-draft'">
                  <span class="pill-dot"></span>
                  {{ doc.isFinalized ? 'نهایی' : 'پیش‌نویس' }}
                </span>
              </td>
              <td>
                <div class="row-actions" v-if="!doc.isFinalized">
                  <button class="ract ract-edit" title="ویرایش" @click="editDocument(doc)">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" />
                      <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z" />
                    </svg>
                  </button>
                  <button class="ract ract-final" title="نهایی کردن" @click="finalizeDocument(doc.documentId)">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                      stroke-width="2.5">
                      <polyline points="20 6 9 17 4 12" />
                    </svg>
                  </button>
                  <button class="ract ract-del" title="حذف" @click="deleteDocument(doc.documentId)">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <polyline points="3 6 5 6 21 6" />
                      <path d="M19 6l-1 14H6L5 6" />
                    </svg>
                  </button>
                </div>
                <div class="row-actions" v-else>
                  <button class="ract ract-view" title="مشاهده">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
                      <circle cx="12" cy="12" r="3" />
                    </svg>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>

    <!-- ===== Toast ===== -->
    <transition name="toast">
      <div v-if="toast.show" class="toast" :class="'toast-' + toast.type">
        <svg v-if="toast.type === 'success'" width="15" height="15" viewBox="0 0 24 24" fill="none"
          stroke="currentColor" stroke-width="2.5">
          <polyline points="20 6 9 17 4 12" />
        </svg>
        <svg v-else-if="toast.type === 'error'" width="15" height="15" viewBox="0 0 24 24" fill="none"
          stroke="currentColor" stroke-width="2.5">
          <line x1="18" y1="6" x2="6" y2="18" />
          <line x1="6" y1="6" x2="18" y2="18" />
        </svg>
        <svg v-else width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <circle cx="12" cy="12" r="10" />
          <line x1="12" y1="8" x2="12" y2="12" />
          <line x1="12" y1="16" x2="12.01" y2="16" />
        </svg>
        {{ toast.text }}
      </div>
    </transition>

  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const document = ref({
  documentId: null,
  description: '',
  date: new Date().toISOString().split('T')[0],
  accountingCode: 'AUTO-001',
  items: []
})

const savedDocuments = ref([
  {
    documentId: 'doc-1',
    accountingCode: 'ACC-001',
    date: new Date('2024-01-15'),
    description: 'خرید تجهیزات اداری',
    isFinalized: true,
    items: [{ debit: 5000000, credit: 0 }, { debit: 0, credit: 5000000 }]
  },
  {
    documentId: 'doc-2',
    accountingCode: 'ACC-002',
    date: new Date('2024-01-20'),
    description: 'دریافت از مشتری آقای رضایی',
    isFinalized: false,
    items: [{ debit: 12000000, credit: 0 }, { debit: 0, credit: 12000000 }]
  }
])

const isManualCode = ref(false)
const editingDocumentId = ref(null)
const toast = ref({ show: false, text: '', type: 'success' })

const accounts = [
  { id: 101, title: 'صندوق' },
  { id: 201, title: 'هزینه‌ها' },
  { id: 301, title: 'درآمدها' },
]
const details = [
  { id: 1, title: 'آقای رضایی' },
  { id: 2, title: 'شرکت نمونه' },
]
const units = [
  { id: 1, title: 'ریال' },
  { id: 2, title: 'تومان' },
]

const totalDebit = computed(() => document.value.items.reduce((a, b) => a + Number(b.debit || 0), 0))
const totalCredit = computed(() => document.value.items.reduce((a, b) => a + Number(b.credit || 0), 0))
const isBalanced = computed(() => totalDebit.value === totalCredit.value && totalDebit.value > 0)

function addRow() {
  document.value.items.push({ itemId: Date.now(), accountId: null, detailId: null, description: '', unit: 1, debit: 0, credit: 0 })
}

function removeRow(item) {
  document.value.items = document.value.items.filter(i => i.itemId !== item.itemId)
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
    showToast('سند با موفقیت ویرایش شد', 'success')
  } else {
    savedDocuments.value.push({
      documentId: `doc-${Date.now()}`,
      accountingCode: isManualCode.value ? document.value.accountingCode : `ACC-${String(savedDocuments.value.length + 1).padStart(3, '0')}`,
      date: new Date(document.value.date),
      description: document.value.description,
      isFinalized: false,
      items: [...document.value.items]
    })
    showToast('سند با موفقیت ثبت شد', 'success')
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
}

function cancelEdit() { createNewDocument() }

function deleteDocument(docId) {
  savedDocuments.value = savedDocuments.value.filter(d => d.documentId !== docId)
  showToast('سند حذف شد', 'error')
}

function finalizeDocument(docId) {
  const doc = savedDocuments.value.find(d => d.documentId === docId)
  if (doc) doc.isFinalized = true
  showToast('سند نهایی شد', 'info')
}

function createNewDocument() {
  document.value = { documentId: null, description: '', date: new Date().toISOString().split('T')[0], accountingCode: 'AUTO-001', items: [] }
  editingDocumentId.value = null
  isManualCode.value = false
}

function showToast(text, type) {
  toast.value = { show: true, text, type }
  setTimeout(() => { toast.value.show = false }, 3000)
}

function formatNum(val) { return Number(val || 0).toLocaleString('fa-IR') }
function formatDate(d) { if (!d) return '—'; return new Date(d).toLocaleDateString('fa-IR') }
</script>

<style scoped>


/* ── Root ── */
.page-root {
  padding: 24px 20px 40px;
  background: var(--bg);
  font-family: var(--base);
  font-size: 13px;
  direction: rtl;
  min-height: 100%;
  color: var(--text);
}

/* ── Header ── */
.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 22px;
  gap: 16px;
  flex-wrap: wrap;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 12px;
}

.header-icon-wrap {
  width: 42px;
  height: 42px;
  border-radius: 13px;
  background: rgba(56, 189, 248, 0.08);
  border: 1px solid rgba(56, 189, 248, 0.2);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.page-title {
  font-size: 16px;
  font-weight: 800;
  color: #e2e8f0;
  margin: 0;
  line-height: 1.3;
}

.page-sub {
  font-size: 11px;
  color: #334155;
  margin: 2px 0 0;
}

/* ── Balance Indicator ── */
.balance-indicator {
  display: inline-flex;
  align-items: center;
  gap: 7px;
  padding: 7px 14px;
  border-radius: 10px;
  font-size: 12px;
  font-weight: 700;
  border: 1px solid;
  transition: all 0.3s;
}

.bal-ok {
  background: rgba(74, 222, 128, 0.07);
  border-color: rgba(74, 222, 128, 0.25);
  color: var(--green);
}

.bal-err {
  background: rgba(248, 113, 113, 0.07);
  border-color: rgba(248, 113, 113, 0.25);
  color: var(--red);
}

.bal-neutral {
  background: rgba(71, 85, 105, 0.1);
  border-color: rgba(71, 85, 105, 0.2);
  color: var(--muted);
}

.bal-dot {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  background: currentColor;
  animation: blink 1.8s infinite;
}

.bal-diff {
  font-size: 11px;
  opacity: 0.8;
  padding-right: 8px;
  border-right: 1px solid currentColor;
  opacity: 0.5;
}

@keyframes blink {

  0%,
  100% {
    opacity: 1
  }

  50% {
    opacity: 0.3
  }
}

/* ── Glass Card ── */
.glass-card {
  background: var(--surface);
  border: 1px solid var(--c-border);
  border-radius: 16px;
  padding: 18px 20px;
  box-shadow: 0 2px 20px rgba(0, 0, 0, 0.25);
}

.mb-4 {
  margin-bottom: 14px;
}

/* ── Section Title ── */
.card-section-title {
  display: flex;
  align-items: center;
  gap: 7px;
  font-size: 11px;
  font-weight: 700;
  color: var(--muted);
  letter-spacing: 0.4px;
  text-transform: uppercase;
  margin-bottom: 16px;
}

.card-section-title svg {
  color: var(--cyan);
}

.mb-0 {
  margin-bottom: 0 !important;
}

/* ── Form Grid ── */
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  gap: 14px;
}

.span-2 {
  grid-column: span 2;
}

@media (max-width: 800px) {
  .form-grid {
    grid-template-columns: 1fr 1fr;
  }

  .span-2 {
    grid-column: span 2;
  }
}

@media (max-width: 500px) {
  .form-grid {
    grid-template-columns: 1fr;
  }

  .span-2 {
    grid-column: span 1;
  }
}

.field-label {
  display: block;
  font-size: 11px;
  font-weight: 700;
  color: var(--muted);
  margin-bottom: 5px;
  font-family: var(--v-font-family);
}

/* ── Custom Toggle ── */
.code-row {
  display: flex;
  align-items: center;
  gap: 10px;
}

.flex-1 {
  flex: 1;
  min-width: 0;
}

.toggle-wrap {
  display: flex;
  align-items: center;
  gap: 7px;
  cursor: pointer;
  flex-shrink: 0;
  user-select: none;
}

.toggle-input {
  display: none;
}

.toggle-track {
  width: 32px;
  height: 18px;
  border-radius: 9px;
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid rgba(255, 255, 255, 0.1);
  position: relative;
  transition: background 0.2s;
}

.toggle-input:checked+.toggle-track {
  background: rgba(56, 189, 248, 0.25);
  border-color: var(--cyan);
}

.toggle-thumb {
  position: absolute;
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background: #475569;
  top: 2px;
  right: 2px;
  transition: right 0.2s, background 0.2s;
}

.toggle-input:checked+.toggle-track .toggle-thumb {
  right: auto;
  left: 2px;
  background: var(--cyan);
}

.toggle-label {
  font-size: 11px;
  color: var(--muted);
  font-family: var(--v-font-family);
}

/* ── Vuetify field overrides ── */
:deep(.dark-field .v-field) {
  background: rgba(0, 0, 0, 0.35) !important;
  font-family: var(--v-font-family) !important;
  font-size: 13px !important;
}

:deep(.dark-field .v-field__outline__start),
:deep(.dark-field .v-field__outline__end),
:deep(.dark-field .v-field__outline__notch) {
  border-color: rgba(255, 255, 255, 0.08) !important;
  transition: border-color 0.2s;
}

:deep(.dark-field .v-field--focused .v-field__outline__start),
:deep(.dark-field .v-field--focused .v-field__outline__end),
:deep(.dark-field .v-field--focused .v-field__outline__notch) {
  border-color: var(--border-focus) !important;
}

:deep(.dark-field input),
:deep(.dark-field .v-select__selection-text) {
  color: var(--c-text) !important;
  font-family: var(--v-font-family) !important;
  font-size: 13px !important;
}

:deep(.dark-field .v-field__input) {
  font-family: var(--v-font-family) !important;
}

:deep(.debit-field .v-field--focused .v-field__outline__start),
:deep(.debit-field .v-field--focused .v-field__outline__end),
:deep(.debit-field .v-field--focused .v-field__outline__notch) {
  border-color: rgba(74, 222, 128, 0.4) !important;
}

:deep(.credit-field .v-field--focused .v-field__outline__start),
:deep(.credit-field .v-field--focused .v-field__outline__end),
:deep(.credit-field .v-field--focused .v-field__outline__notch) {
  border-color: rgba(248, 113, 113, 0.4) !important;
}

:deep(.v-overlay__content .v-list) {
  background: #0d1828 !important;
  border: 1px solid rgba(255, 255, 255, 0.08) !important;
  border-radius: 10px !important;
  font-family: var(--v-font-family) !important;
}

:deep(.v-list-item-title) {
  font-family: var(--v-font-family) !important;
  font-size: 13px !important;
  color: var(--text) !important;
}

:deep(.v-list-item:hover) {
  background: rgba(56, 189, 248, 0.07) !important;
}

/* ── Table Toolbar ── */
.table-toolbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 14px;
}

.row-count {
  background: rgba(56, 189, 248, 0.1);
  color: var(--cyan);
  font-size: 10px;
  padding: 2px 8px;
  border-radius: 99px;
  font-weight: 700;
  margin-right: 4px;
  /* RTL: this is actually to the left visually */
}

.btn-add-row {
  display: flex;
  align-items: center;
  gap: 6px;
  background: rgba(56, 189, 248, 0.09);
  border: 1px solid rgba(56, 189, 248, 0.22);
  color: var(--cyan);
  border-radius: 9px;
  padding: 7px 14px;
  font-size: 12px;
  font-weight: 700;
  font-family: var(--v-font-family);
  cursor: pointer;
  transition: background 0.2s, transform 0.15s;
}

.btn-add-row:hover {
  background: rgba(56, 189, 248, 0.16);
  transform: translateY(-1px);
}

/* ── Table ── */
.table-scroll-wrap {
  overflow-x: auto;
  overflow-y: auto;
  max-height: 320px;
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.05);
  scrollbar-width: thin;
  scrollbar-color: rgba(255, 255, 255, 0.07) transparent;
}

.table-scroll-wrap::-webkit-scrollbar {
  width: 4px;
  height: 4px;
}

.table-scroll-wrap::-webkit-scrollbar-thumb {
  background: rgba(255, 255, 255, 0.08);
  border-radius: 2px;
}

.doc-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 12.5px;
  font-family: var(--v-font-family);
}

.doc-table thead {
  position: sticky;
  top: 0;
  z-index: 2;
}

.doc-table thead tr {
  background: #0a1020;
}

.doc-table th {
  padding: 10px 12px;
  text-align: right;
  font-size: 11px;
  font-weight: 700;
  color: #334155;
  letter-spacing: 0.3px;
  white-space: nowrap;
  border-bottom: 1px solid rgba(255, 255, 255, 0.06);
  font-family: var(--v-font-family);
}

.th-num {
  width: 36px;
  text-align: center;
}

.th-amount {
  min-width: 130px;
}

.th-action {
  width: 56px;
  text-align: center;
}

.doc-table td {
  padding: 7px 10px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.028);
  vertical-align: middle;
  font-family: var(--v-font-family);
}

.doc-row {
  transition: background 0.15s;
}

.doc-row:hover td {
  background: rgba(255, 255, 255, 0.013);
}

.doc-row:last-child td {
  border-bottom: none;
}

.row-debit td:nth-child(6) {
  background: rgba(74, 222, 128, 0.03);
}

.row-credit td:nth-child(7) {
  background: rgba(248, 113, 113, 0.03);
}

.td-num {
  text-align: center;
  color: #253347;
  font-size: 11px;
}

.td-date {
  color: var(--muted);
  font-size: 12px;
  white-space: nowrap;
}

.td-desc {
  color: #64748b;
  max-width: 200px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.td-action {
  text-align: center;
}

/* table fields */
.tf {
  min-width: 100px;
}

.tf-sm {
  min-width: 80px;
}

.tf-num {
  min-width: 110px;
}

/* ── Totals Footer ── */
.totals-footer {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 14px 4px 2px;
  flex-wrap: wrap;
}

.totals-label {
  font-size: 11px;
  font-weight: 700;
  color: #253347;
  font-family: var(--v-font-family);
}

.totals-chips {
  display: flex;
  align-items: center;
  gap: 8px;
  flex-wrap: wrap;
}

.total-chip {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 5px 14px;
  border-radius: 99px;
  border: 1px solid;
  font-family: var(--v-font-family);
}

.chip-debit {
  background: rgba(74, 222, 128, 0.07);
  border-color: rgba(74, 222, 128, 0.2);
  color: var(--green);
}

.chip-credit {
  background: rgba(248, 113, 113, 0.07);
  border-color: rgba(248, 113, 113, 0.2);
  color: var(--red);
}

.chip-diff {
  background: rgba(251, 191, 36, 0.07);
  border-color: rgba(251, 191, 36, 0.2);
  color: var(--amber);
}

.chip-label {
  font-size: 10px;
  opacity: 0.7;
}

.chip-value {
  font-size: 13px;
  font-weight: 800;
  font-variant-numeric: tabular-nums;
}

.total-sep {
  color: #1e293b;
  font-size: 16px;
  font-weight: 300;
}

.total-balanced {
  display: flex;
  align-items: center;
  gap: 5px;
  font-size: 11px;
  font-weight: 700;
  color: var(--green);
  background: rgba(74, 222, 128, 0.07);
  border: 1px solid rgba(74, 222, 128, 0.2);
  padding: 5px 12px;
  border-radius: 99px;
  font-family: var(--v-font-family);
}

/* ── Action Bar ── */
.action-bar {
  display: flex;
  align-items: center;
  gap: 10px;
  margin: 14px 0;
}

.btn-submit {
  display: flex;
  align-items: center;
  gap: 8px;
  background: linear-gradient(135deg, #0369a1, #0ea5e9);
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 10px 22px;
  font-size: 13px;
  font-weight: 700;
  font-family: var(--v-font-family);
  cursor: pointer;
  box-shadow: 0 4px 16px rgba(14, 165, 233, 0.22);
  transition: opacity 0.2s, transform 0.15s;
}

.btn-submit:hover:not(:disabled) {
  opacity: 0.88;
  transform: translateY(-1px);
}

.btn-submit:disabled {
  background: #1a2235;
  color: #334155;
  box-shadow: none;
  cursor: not-allowed;
}

.btn-cancel {
  display: flex;
  align-items: center;
  gap: 7px;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.09);
  color: var(--muted);
  border-radius: 10px;
  padding: 10px 18px;
  font-size: 13px;
  font-weight: 600;
  font-family: var(--v-font-family);
  cursor: pointer;
  transition: background 0.2s;
}

.btn-cancel:hover {
  background: rgba(255, 255, 255, 0.06);
}

/* ── Section Sep ── */
.section-sep {
  display: flex;
  align-items: center;
  gap: 12px;
  margin: 20px 0 14px;
}

.sep-line {
  flex: 1;
  height: 1px;
  background: linear-gradient(to left, transparent, rgba(255, 255, 255, 0.05), transparent);
}

.sep-label {
  display: flex;
  align-items: center;
  gap: 6px;
  font-size: 11px;
  font-weight: 700;
  color: #1e3a4a;
  white-space: nowrap;
  font-family: var(--v-font-family);
}

/* ── List Toolbar ── */
.list-toolbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 14px;
}

.doc-count-badge {
  font-size: 11px;
  color: #334155;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.06);
  padding: 3px 10px;
  border-radius: 99px;
  font-family: var(--v-font-family);
}

/* ── List table cells ── */
.code-chip {
  font-size: 11px;
  font-weight: 700;
  color: var(--cyan);
  background: rgba(56, 189, 248, 0.08);
  border: 1px solid rgba(56, 189, 248, 0.15);
  padding: 2px 8px;
  border-radius: 6px;
  font-family: var(--v-font-family);
  white-space: nowrap;
}

.amount-val {
  font-size: 12.5px;
  font-weight: 800;
  color: var(--green);
  font-family: var(--v-font-family);
}

.status-pill {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 3px 10px;
  border-radius: 99px;
  font-size: 11px;
  font-weight: 700;
  border: 1px solid;
  font-family: var(--v-font-family);
}

.pill-final {
  background: rgba(74, 222, 128, 0.07);
  border-color: rgba(74, 222, 128, 0.2);
  color: var(--green);
}

.pill-draft {
  background: rgba(251, 191, 36, 0.07);
  border-color: rgba(251, 191, 36, 0.2);
  color: var(--amber);
}

.pill-dot {
  width: 5px;
  height: 5px;
  border-radius: 50%;
  background: currentColor;
}

/* ── Row Actions ── */
.row-actions {
  display: flex;
  align-items: center;
  gap: 3px;
}

.ract {
  width: 28px;
  height: 28px;
  border-radius: 7px;
  border: 1px solid transparent;
  background: transparent;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: background 0.15s, border-color 0.15s;
}

.ract:hover {
  border-color: currentColor;
}

.ract-edit {
  color: var(--cyan);
}

.ract-edit:hover {
  background: rgba(56, 189, 248, 0.1);
}

.ract-final {
  color: var(--green);
}

.ract-final:hover {
  background: rgba(74, 222, 128, 0.1);
}

.ract-del {
  color: var(--red);
}

.ract-del:hover {
  background: rgba(248, 113, 113, 0.1);
}

.ract-view {
  color: var(--muted);
}

.ract-view:hover {
  background: rgba(255, 255, 255, 0.05);
}

/* ── Delete row btn ── */
.btn-del-row {
  width: 28px;
  height: 28px;
  border-radius: 7px;
  border: 1px solid transparent;
  background: transparent;
  color: #ef4444;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  opacity: 0.4;
  transition: opacity 0.15s, background 0.15s;
}

.doc-row:hover .btn-del-row {
  opacity: 1;
}

.btn-del-row:hover {
  background: rgba(248, 113, 113, 0.1);
  border-color: rgba(248, 113, 113, 0.3);
}

/* ── Empty State ── */
.empty-state {
  text-align: center;
  padding: 36px 20px;
}

.empty-icon-wrap {
  width: 52px;
  height: 52px;
  border-radius: 14px;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.05);
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 12px;
  color: #1e3a4a;
}

.empty-title {
  font-size: 13px;
  font-weight: 700;
  color: #253347;
  font-family: var(--v-font-family);
}

.empty-body {
  font-size: 11px;
  color: #1e3a4a;
  margin-top: 4px;
  font-family: var(--v-font-family);
}

/* ── Toast ── */
.toast {
  position: fixed;
  bottom: 24px;
  left: 24px;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 11px 18px;
  border-radius: 11px;
  font-size: 13px;
  font-weight: 700;
  font-family: var(--v-font-family);
  border: 1px solid;
  z-index: 9999;
  backdrop-filter: blur(12px);
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.4);
}

.toast-success {
  background: rgba(74, 222, 128, 0.12);
  border-color: rgba(74, 222, 128, 0.25);
  color: var(--green);
}

.toast-error {
  background: rgba(248, 113, 113, 0.12);
  border-color: rgba(248, 113, 113, 0.25);
  color: var(--red);
}

.toast-info {
  background: rgba(56, 189, 248, 0.12);
  border-color: rgba(56, 189, 248, 0.25);
  color: var(--cyan);
}

.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s ease;
}

.toast-enter-from,
.toast-leave-to {
  opacity: 0;
  transform: translateY(12px);
}
</style>
