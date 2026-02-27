<template>
    <div class="page-root" dir="rtl">

        <!-- BG Glows -->
        <div class="bg-glow bg-glow-1" />
        <div class="bg-glow bg-glow-2" />
        <div class="bg-grid" />

        <div class="page-inner">

            <!-- ══════════ HEADER ══════════ -->
            <header class="page-header">
                <div class="brand">
                    <div class="brand-mark">
                        <v-icon size="20" color="#63b3ed">mdi-account-group</v-icon>
                    </div>
                    <div>
                        <div class="brand-name">سیستم فینومال</div>
                        <div class="brand-sub">مدیریت منابع انسانی</div>
                    </div>
                </div>
                <router-link to="/personnel/create-personnel" class="btn btn-primary">
                    <v-icon size="16" class="ml-1">mdi-plus</v-icon>
                    افزودن پرسنل جدید
                </router-link>
            </header>

            <!-- ══════════ STATS ══════════ -->
            <div class="stats-row">
                <div class="stat-card" v-for="stat in stats" :key="stat.label" :style="{ '--stat-color': stat.color }">
                    <div class="stat-icon">
                        <v-icon size="20" :color="stat.color">{{ stat.icon }}</v-icon>
                    </div>
                    <div class="stat-body">
                        <div class="stat-val">{{ stat.val }}</div>
                        <div class="stat-label">{{ stat.label }}</div>
                    </div>
                    <div class="stat-glow" />
                </div>
            </div>

            <!-- ══════════ TOOLBAR ══════════ -->
            <div class="toolbar-card">
                <div class="search-wrap">
                    <v-icon size="16" class="search-icon">mdi-magnify</v-icon>
                    <input v-model="search" class="search-input" placeholder="جستجو بر اساس نام، کد ملی، کد پرسنلی..."
                        dir="rtl" />
                    <button v-if="search" class="search-clear" @click="search = ''">
                        <v-icon size="14">mdi-close</v-icon>
                    </button>
                </div>

                <div class="filters">
                    <div class="filter-select-wrap">
                        <v-icon size="14" class="filter-icon">mdi-office-building</v-icon>
                        <select v-model="filterDept" class="filter-select">
                            <option value="">همه واحدها</option>
                            <option v-for="d in departments" :key="d" :value="d">{{ d }}</option>
                        </select>
                        <v-icon size="12" class="filter-chevron">mdi-chevron-down</v-icon>
                    </div>

                    <div class="filter-select-wrap">
                        <v-icon size="14" class="filter-icon">mdi-shield-check</v-icon>
                        <select v-model="filterStatus" class="filter-select">
                            <option value="">همه وضعیت‌ها</option>
                            <option value="active">فعال</option>
                            <option value="inactive">غیرفعال</option>
                        </select>
                        <v-icon size="12" class="filter-chevron">mdi-chevron-down</v-icon>
                    </div>

                    <button class="btn-icon" title="خروجی Excel" @click="exportExcel">
                        <v-icon size="16">mdi-microsoft-excel</v-icon>
                    </button>
                    <button class="btn-icon" title="چاپ" @click="printList">
                        <v-icon size="16">mdi-printer-outline</v-icon>
                    </button>
                </div>
            </div>

            <!-- ══════════ TABLE ══════════ -->
            <div class="table-card">
                <div class="table-wrap">
                    <table class="ptable">
                        <thead>
                            <tr>
                                <th class="th-check">
                                    <div class="checkbox" :class="{ checked: allChecked }" @click="toggleAll">
                                        <v-icon v-if="allChecked" size="12" color="white">mdi-check</v-icon>
                                    </div>
                                </th>
                                <th @click="sortBy('name')" class="sortable">
                                    نام و نام خانوادگی
                                    <v-icon size="13" class="sort-icon" :class="{ active: sortKey === 'name' }">
                                        {{ sortKey === 'name' && sortDir === 'asc' ? 'mdi-arrow-up' : 'mdi-arrow-down'
                                        }}
                                    </v-icon>
                                </th>
                                <th>کد پرسنلی</th>
                                <th>واحد / دپارتمان</th>
                                <th>عنوان شغلی</th>
                                <th @click="sortBy('hireDate')" class="sortable">
                                    تاریخ استخدام
                                    <v-icon size="13" class="sort-icon" :class="{ active: sortKey === 'hireDate' }">
                                        {{ sortKey === 'hireDate' && sortDir === 'asc' ? 'mdi-arrow-up' :
                                            'mdi-arrow-down' }}
                                    </v-icon>
                                </th>
                                <th>حقوق پایه</th>
                                <th>وضعیت</th>
                                <th class="th-actions">عملیات</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-if="paginatedList.length === 0">
                                <td colspan="9" class="empty-row">
                                    <div class="empty-state">
                                        <v-icon size="48" color="#334155">mdi-account-search</v-icon>
                                        <p>هیچ پرسنلی یافت نشد</p>
                                    </div>
                                </td>
                            </tr>
                            <tr v-for="p in paginatedList" :key="p.id" class="table-row"
                                :class="{ 'row-selected': selected.includes(p.id) }">
                                <td class="td-check">
                                    <div class="checkbox" :class="{ checked: selected.includes(p.id) }"
                                        @click="toggleRow(p.id)">
                                        <v-icon v-if="selected.includes(p.id)" size="12"
                                            color="white">mdi-check</v-icon>
                                    </div>
                                </td>
                                <td>
                                    <div class="person-cell">
                                        <div class="avatar" :style="{ background: avatarColor(p.name) }">
                                            {{ p.name.charAt(0) }}
                                        </div>
                                        <div>
                                            <div class="person-name">{{ p.name }}</div>
                                            <div class="person-sub">{{ p.nationalId }}</div>
                                        </div>
                                    </div>
                                </td>
                                <td><span class="mono">{{ p.code }}</span></td>
                                <td>
                                    <span class="dept-badge">{{ p.dept }}</span>
                                </td>
                                <td class="text-muted">{{ p.jobTitle }}</td>
                                <td class="text-muted mono">{{ p.hireDate }}</td>
                                <td><span class="salary-cell">{{ p.salary }}</span></td>
                                <td>
                                    <span class="status-badge"
                                        :class="p.status === 'active' ? 'status-active' : 'status-inactive'">
                                        <span class="status-dot" />
                                        {{ p.status === 'active' ? 'فعال' : 'غیرفعال' }}
                                    </span>
                                </td>
                                <td>
                                    <div class="actions-cell">
                                        <button class="action-btn action-view" title="مشاهده" @click="viewPersonnel(p)">
                                            <v-icon size="14">mdi-eye-outline</v-icon>
                                        </button>
                                        <button class="action-btn action-edit" title="ویرایش" @click="editPersonnel(p)">
                                            <v-icon size="14">mdi-pencil-outline</v-icon>
                                        </button>
                                        <button class="action-btn action-del" title="حذف" @click="confirmDelete(p)">
                                            <v-icon size="14">mdi-trash-can-outline</v-icon>
                                        </button>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!-- Pagination -->
                <div class="pagination-bar">
                    <div class="pagination-info">
                        نمایش {{ (page - 1) * perPage + 1 }} تا {{ Math.min(page * perPage, filteredList.length) }}
                        از {{ filteredList.length }} پرسنل
                    </div>
                    <div class="pagination-controls">
                        <button class="pg-btn" :disabled="page === 1" @click="page--">
                            <v-icon size="14">mdi-chevron-right</v-icon>
                        </button>
                        <button v-for="p in totalPages" :key="p" class="pg-btn pg-num"
                            :class="{ 'pg-active': p === page }" @click="page = p">{{ toPersian(p) }}</button>
                        <button class="pg-btn" :disabled="page === totalPages" @click="page++">
                            <v-icon size="14">mdi-chevron-left</v-icon>
                        </button>
                    </div>
                    <div class="per-page-wrap">
                        <span>تعداد در صفحه:</span>
                        <select v-model="perPage" class="per-page-select" @change="page = 1">
                            <option :value="5">۵</option>
                            <option :value="10">۱۰</option>
                            <option :value="20">۲۰</option>
                        </select>
                    </div>
                </div>
            </div>

        </div>

        <!-- ══════════ DELETE CONFIRM DIALOG ══════════ -->
        <transition name="dialog-fade">
            <div v-if="deleteDialog.show" class="dialog-overlay" @click.self="deleteDialog.show = false">
                <div class="dialog-box">
                    <div class="dialog-icon">
                        <v-icon size="32" color="#f87171">mdi-alert-circle-outline</v-icon>
                    </div>
                    <div class="dialog-title">حذف پرسنل</div>
                    <div class="dialog-body">
                        آیا از حذف <strong>{{ deleteDialog.person?.name }}</strong> مطمئن هستید؟
                        این عمل قابل بازگشت نیست.
                    </div>
                    <div class="dialog-actions">
                        <button class="btn btn-ghost" @click="deleteDialog.show = false">انصراف</button>
                        <button class="btn btn-danger" @click="doDelete">حذف</button>
                    </div>
                </div>
            </div>
        </transition>

        <!-- Toast -->
        <transition name="toast-anim">
            <div v-if="toast.show" class="toast" :class="`toast-${toast.type}`">
                <v-icon size="16" class="ml-2">{{ toast.type === 'success' ? 'mdi-check-circle' : 'mdi-alert-circle'
                    }}</v-icon>
                {{ toast.text }}
            </div>
        </transition>

    </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

// ── Helpers ──────────────────────────────────────────────────────────────
const persianDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹']
function toPersian(n) {
    return String(n).split('').map(c => persianDigits[+c] ?? c).join('')
}

const AVATAR_COLORS = [
    'linear-gradient(135deg,#63b3ed,#4299e1)',
    'linear-gradient(135deg,#48bb78,#38a169)',
    'linear-gradient(135deg,#ed8936,#dd6b20)',
    'linear-gradient(135deg,#a78bfa,#7c3aed)',
    'linear-gradient(135deg,#f687b3,#d53f8c)',
    'linear-gradient(135deg,#38bdf8,#0284c7)',
    'linear-gradient(135deg,#fbbf24,#d97706)',
]
function avatarColor(name) {
    const idx = name.charCodeAt(0) % AVATAR_COLORS.length
    return AVATAR_COLORS[idx]
}

// ── Sample Data ───────────────────────────────────────────────────────────
const allPersonnel = ref([
    { id: 1, name: 'علی محمدی', nationalId: '0012345678', code: 'P-0001', dept: 'مالی و حسابداری', jobTitle: 'حسابدار ارشد', hireDate: '۱۳۹۸/۰۱/۱۵', salary: '۲۸,۰۰۰,۰۰۰', status: 'active' },
    { id: 2, name: 'فاطمه رضایی', nationalId: '0023456789', code: 'P-0002', dept: 'اداری و منابع انسانی', jobTitle: 'کارشناس منابع انسانی', hireDate: '۱۳۹۹/۰۳/۲۰', salary: '۲۲,۰۰۰,۰۰۰', status: 'active' },
    { id: 3, name: 'محمد کریمی', nationalId: '0034567890', code: 'P-0003', dept: 'فناوری اطلاعات', jobTitle: 'توسعه‌دهنده ارشد', hireDate: '۱۳۹۷/۰۶/۰۱', salary: '۳۵,۰۰۰,۰۰۰', status: 'active' },
    { id: 4, name: 'زهرا احمدی', nationalId: '0045678901', code: 'P-0004', dept: 'فروش و بازاریابی', jobTitle: 'مدیر فروش', hireDate: '۱۴۰۰/۰۲/۱۰', salary: '۳۲,۰۰۰,۰۰۰', status: 'active' },
    { id: 5, name: 'حسن موسوی', nationalId: '0056789012', code: 'P-0005', dept: 'تولید و عملیات', jobTitle: 'سرپرست تولید', hireDate: '۱۳۹۶/۰۸/۰۵', salary: '۲۵,۰۰۰,۰۰۰', status: 'active' },
    { id: 6, name: 'مریم صادقی', nationalId: '0067890123', code: 'P-0006', dept: 'مالی و حسابداری', jobTitle: 'حسابرس داخلی', hireDate: '۱۴۰۱/۰۱/۰۱', salary: '۲۶,۰۰۰,۰۰۰', status: 'active' },
    { id: 7, name: 'رضا جعفری', nationalId: '0078901234', code: 'P-0007', dept: 'انبار و لجستیک', jobTitle: 'مدیر انبار', hireDate: '۱۳۹۸/۱۱/۱۵', salary: '۲۰,۰۰۰,۰۰۰', status: 'inactive' },
    { id: 8, name: 'نرگس حسینی', nationalId: '0089012345', code: 'P-0008', dept: 'اداری و منابع انسانی', jobTitle: 'منشی اداری', hireDate: '۱۴۰۰/۰۵/۱۱', salary: '۱۸,۰۰۰,۰۰۰', status: 'active' },
    { id: 9, name: 'امیر تهرانی', nationalId: '0090123456', code: 'P-0009', dept: 'فناوری اطلاعات', jobTitle: 'مدیر محصول', hireDate: '۱۳۹۹/۰۷/۲۲', salary: '۳۸,۰۰۰,۰۰۰', status: 'active' },
    { id: 10, name: 'سارا نجفی', nationalId: '0001234567', code: 'P-0010', dept: 'فروش و بازاریابی', jobTitle: 'کارشناس بازاریابی', hireDate: '۱۴۰۱/۰۳/۰۸', salary: '۲۱,۰۰۰,۰۰۰', status: 'active' },
    { id: 11, name: 'داود قاسمی', nationalId: '0113456780', code: 'P-0011', dept: 'تولید و عملیات', jobTitle: 'اپراتور ماشین‌آلات', hireDate: '۱۳۹۷/۰۹/۱۴', salary: '۱۹,۰۰۰,۰۰۰', status: 'inactive' },
    { id: 12, name: 'لیلا عباسی', nationalId: '0124567891', code: 'P-0012', dept: 'مالی و حسابداری', jobTitle: 'مدیر مالی', hireDate: '۱۳۹۵/۰۴/۰۱', salary: '۴۵,۰۰۰,۰۰۰', status: 'active' },
])

const departments = ['اداری و منابع انسانی', 'مالی و حسابداری', 'فروش و بازاریابی', 'تولید و عملیات', 'فناوری اطلاعات', 'انبار و لجستیک']

// ── State ────────────────────────────────────────────────────────────────
const search = ref('')
const filterDept = ref('')
const filterStatus = ref('')
const sortKey = ref('name')
const sortDir = ref('asc')
const page = ref(1)
const perPage = ref(10)
const selected = ref([])
const deleteDialog = ref({ show: false, person: null })
const toast = ref({ show: false, text: '', type: 'success' })

// Reset page on filter change
watch([search, filterDept, filterStatus], () => { page.value = 1 })

// ── Computed ─────────────────────────────────────────────────────────────
const stats = computed(() => [
    { label: 'کل پرسنل', val: toPersian(allPersonnel.value.length), icon: 'mdi-account-group', color: '#63b3ed' },
    { label: 'فعال', val: toPersian(allPersonnel.value.filter(p => p.status === 'active').length), icon: 'mdi-account-check', color: '#48bb78' },
    { label: 'غیرفعال', val: toPersian(allPersonnel.value.filter(p => p.status === 'inactive').length), icon: 'mdi-account-off', color: '#f87171' },
    { label: 'واحدها', val: toPersian(departments.length), icon: 'mdi-office-building', color: '#a78bfa' },
])

const filteredList = computed(() => {
    let list = allPersonnel.value
    if (search.value.trim()) {
        const q = search.value.trim().toLowerCase()
        list = list.filter(p =>
            p.name.toLowerCase().includes(q) ||
            p.nationalId.includes(q) ||
            p.code.toLowerCase().includes(q) ||
            p.jobTitle.toLowerCase().includes(q)
        )
    }
    if (filterDept.value) list = list.filter(p => p.dept === filterDept.value)
    if (filterStatus.value) list = list.filter(p => p.status === filterStatus.value)

    // Sort
    list = [...list].sort((a, b) => {
        let va = a[sortKey.value] || '', vb = b[sortKey.value] || ''
        return sortDir.value === 'asc'
            ? va.localeCompare(vb, 'fa')
            : vb.localeCompare(va, 'fa')
    })
    return list
})

const totalPages = computed(() => Math.max(1, Math.ceil(filteredList.value.length / perPage.value)))
const paginatedList = computed(() => {
    const start = (page.value - 1) * perPage.value
    return filteredList.value.slice(start, start + perPage.value)
})

const allChecked = computed(() =>
    paginatedList.value.length > 0 &&
    paginatedList.value.every(p => selected.value.includes(p.id))
)

// ── Actions ──────────────────────────────────────────────────────────────
function sortBy(key) {
    if (sortKey.value === key) sortDir.value = sortDir.value === 'asc' ? 'desc' : 'asc'
    else { sortKey.value = key; sortDir.value = 'asc' }
}

function toggleAll() {
    if (allChecked.value) {
        selected.value = selected.value.filter(id => !paginatedList.value.find(p => p.id === id))
    } else {
        paginatedList.value.forEach(p => { if (!selected.value.includes(p.id)) selected.value.push(p.id) })
    }
}

function toggleRow(id) {
    const idx = selected.value.indexOf(id)
    if (idx >= 0) selected.value.splice(idx, 1)
    else selected.value.push(id)
}

function viewPersonnel(p) {
    showToast(`مشاهده پروفایل ${p.name}`, 'success')
}

function editPersonnel(p) {
    router.push('/personnel/create-personnel')
}

function confirmDelete(p) {
    deleteDialog.value = { show: true, person: p }
}

function doDelete() {
    const p = deleteDialog.value.person
    allPersonnel.value = allPersonnel.value.filter(x => x.id !== p.id)
    deleteDialog.value.show = false
    showToast(`پرسنل ${p.name} حذف شد`, 'error')
}

function exportExcel() {
    showToast('خروجی اکسل آماده دانلود است', 'success')
}

function printList() {
    window.print()
}

let toastTimer
function showToast(text, type = 'success') {
    toast.value = { show: true, text, type }
    clearTimeout(toastTimer)
    toastTimer = setTimeout(() => { toast.value.show = false }, 3000)
}
</script>

<style scoped>
/* ── Root ─────────────────────────────────────────────────────────────── */
.page-root {
    min-height: 100vh;
    background: var(--bg);
    color: var(--text);
    font-family: var(--font);
    direction: rtl;
    position: relative;
    overflow-x: hidden;
}

/* ── BG ─────────────────────────────────────────────────────────────── */
.bg-glow {
    position: fixed;
    border-radius: 50%;
    pointer-events: none;
    z-index: 0;
    filter: blur(90px);
}

.bg-glow-1 {
    width: 700px;
    height: 500px;
    top: -15%;
    right: -5%;
    background: radial-gradient(ellipse, rgba(99, 179, 237, .05), transparent);
}

.bg-glow-2 {
    width: 500px;
    height: 400px;
    bottom: 5%;
    left: -5%;
    background: radial-gradient(ellipse, rgba(72, 187, 120, .04), transparent);
}

.bg-grid {
    position: fixed;
    inset: 0;
    pointer-events: none;
    z-index: 0;
    background:
        repeating-linear-gradient(0deg, transparent, transparent 40px, rgba(255, 255, 255, .007) 40px, rgba(255, 255, 255, .007) 41px),
        repeating-linear-gradient(90deg, transparent, transparent 40px, rgba(255, 255, 255, .007) 40px, rgba(255, 255, 255, .007) 41px);
}

.page-inner {
    position: relative;
    z-index: 1;
    margin: 0 auto;
    padding: 28px 24px 60px;
    display: flex;
    flex-direction: column;
    gap: 18px;
}

/* ── Header ─────────────────────────────────────────────────────────── */
.page-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.brand {
    display: flex;
    align-items: center;
    gap: 12px;
}

.brand-mark {
    width: 42px;
    height: 42px;
    border-radius: 12px;
    background: linear-gradient(135deg, rgba(99, 179, 237, .2), rgba(99, 179, 237, .05));
    border: 1px solid rgba(99, 179, 237, .25);
    display: flex;
    align-items: center;
    justify-content: center;
}

.brand-name {
    font-size: 13px;
    font-weight: 700;
}

.brand-sub {
    font-size: 11px;
    color: var(--muted);
    margin-top: 1px;
}

/* ── Stats ──────────────────────────────────────────────────────────── */
.stats-row {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 14px;
}

.stat-card {
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: var(--radius);
    padding: 18px 20px;
    display: flex;
    align-items: center;
    gap: 14px;
    position: relative;
    overflow: hidden;
    transition: border-color .2s, transform .2s;
}

.stat-card:hover {
    border-color: rgba(255, 255, 255, .12);
    transform: translateY(-1px);
}

.stat-glow {
    position: absolute;
    inset: 0;
    pointer-events: none;
    background: radial-gradient(ellipse at top right, rgba(var(--stat-color-rgb, 99, 179, 237), .04), transparent);
}

.stat-icon {
    width: 44px;
    height: 44px;
    border-radius: 12px;
    flex-shrink: 0;
    background: rgba(255, 255, 255, .04);
    border: 1px solid var(--border);
    display: flex;
    align-items: center;
    justify-content: center;
}

.stat-val {
    font-size: 22px;
    font-weight: 700;
    line-height: 1;
}

.stat-label {
    font-size: 11px;
    color: var(--muted);
    margin-top: 4px;
}

/* ── Toolbar ────────────────────────────────────────────────────────── */
.toolbar-card {
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: var(--radius);
    padding: 14px 20px;
    display: flex;
    align-items: center;
    gap: 12px;
}

.search-wrap {
    flex: 1;
    position: relative;
    display: flex;
    align-items: center;
}

.search-icon {
    position: absolute;
    right: 12px;
    color: var(--muted);
    pointer-events: none;
}

.search-input {
    width: 100%;
    padding: 9px 38px 9px 36px;
    background: rgba(255, 255, 255, .04);
    border: 1px solid var(--border);
    border-radius: 10px;
    color: var(--text);
    font-family: var(--font);
    font-size: 13px;
    outline: none;
    transition: border-color .2s, background .2s;
}

.search-input:focus {
    border-color: rgba(99, 179, 237, .4);
    background: rgba(255, 255, 255, .06);
}

.search-input::placeholder {
    color: var(--muted);
}

.search-clear {
    position: absolute;
    left: 10px;
    background: none;
    border: none;
    cursor: pointer;
    color: var(--muted);
    display: flex;
    align-items: center;
    padding: 2px;
    transition: color .2s;
}

.search-clear:hover {
    color: var(--text);
}

.filters {
    display: flex;
    align-items: center;
    gap: 10px;
}

.filter-select-wrap {
    position: relative;
    display: flex;
    align-items: center;
}

.filter-icon {
    position: absolute;
    right: 10px;
    color: var(--muted);
    pointer-events: none;
    z-index: 1;
}

.filter-chevron {
    position: absolute;
    left: 8px;
    color: var(--muted);
    pointer-events: none;
}

.filter-select {
    appearance: none;
    padding: 8px 30px 8px 28px;
    background: rgba(255, 255, 255, .04);
    border: 1px solid var(--border);
    border-radius: 10px;
    color: var(--text);
    font-family: var(--font);
    font-size: 12px;
    outline: none;
    cursor: pointer;
    transition: border-color .2s;
}

.filter-select:focus {
    border-color: rgba(99, 179, 237, .4);
}

.filter-select option {
    background: #0d1117;
}

.btn-icon {
    width: 38px;
    height: 38px;
    border-radius: 10px;
    background: rgba(255, 255, 255, .04);
    border: 1px solid var(--border);
    color: var(--muted);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all .2s;
}

.btn-icon:hover {
    border-color: rgba(99, 179, 237, .4);
    color: var(--accent);
    background: rgba(99, 179, 237, .06);
}

/* ── Table ──────────────────────────────────────────────────────────── */
.table-card {
    background: var(--surface);
    border: 1px solid var(--border);
    border-radius: var(--radius);
    overflow: hidden;
}

.table-wrap {
    overflow-x: auto;
}

.ptable {
    width: 100%;
    border-collapse: collapse;
    font-size: 13px;
}

.ptable thead tr {
    border-bottom: 1px solid var(--border);
    background: rgba(255, 255, 255, .025);
}

.ptable th {
    padding: 13px 16px;
    text-align: right;
    color: var(--muted);
    font-weight: 600;
    font-size: 11.5px;
    white-space: nowrap;
    user-select: none;
}

.ptable th.sortable {
    cursor: pointer;
}

.ptable th.sortable:hover {
    color: var(--text);
}

.sort-icon {
    vertical-align: middle;
    margin-right: 3px;
    opacity: .3;
    transition: opacity .2s;
}

.sort-icon.active {
    opacity: 1;
    color: var(--accent);
}

.th-check,
.td-check {
    width: 44px;
    padding: 0 12px;
}

.th-actions {
    width: 110px;
}

.table-row {
    border-bottom: 1px solid var(--border);
    transition: background .15s;
}

.table-row:last-child {
    border-bottom: none;
}

.table-row:hover {
    background: rgba(255, 255, 255, .025);
}

.row-selected {
    background: rgba(99, 179, 237, .05) !important;
}

.ptable td {
    padding: 12px 16px;
    vertical-align: middle;
}

/* Checkbox */
.checkbox {
    width: 18px;
    height: 18px;
    border-radius: 5px;
    border: 1.5px solid rgba(255, 255, 255, .15);
    cursor: pointer;
    transition: all .2s;
    display: flex;
    align-items: center;
    justify-content: center;
}

.checkbox:hover {
    border-color: var(--accent);
}

.checkbox.checked {
    background: var(--accent);
    border-color: var(--accent);
}

/* Avatar / Person cell */
.person-cell {
    display: flex;
    align-items: center;
    gap: 10px;
}

.avatar {
    width: 36px;
    height: 36px;
    border-radius: 10px;
    flex-shrink: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 14px;
    font-weight: 700;
    color: white;
}

.person-name {
    font-size: 13px;
    font-weight: 600;
    color: var(--text);
}

.person-sub {
    font-size: 11px;
    color: var(--muted);
    margin-top: 1px;
    font-family: monospace;
    direction: ltr;
    text-align: right;
}

.mono {
    font-family: monospace;
    direction: ltr;
    display: inline-block;
}

.text-muted {
    color: #94a3b8;
}

/* Dept badge */
.dept-badge {
    display: inline-block;
    padding: 3px 10px;
    border-radius: 99px;
    font-size: 11px;
    font-weight: 500;
    background: rgba(99, 179, 237, .1);
    color: #93c5fd;
    border: 1px solid rgba(99, 179, 237, .15);
}

/* Salary */
.salary-cell {
    font-family: monospace;
    color: #34d399;
    font-size: 12px;
}

/* Status badge */
.status-badge {
    display: inline-flex;
    align-items: center;
    gap: 5px;
    padding: 3px 10px;
    border-radius: 99px;
    font-size: 11px;
    font-weight: 600;
}

.status-active {
    background: rgba(52, 211, 153, .1);
    color: #34d399;
    border: 1px solid rgba(52, 211, 153, .2);
}

.status-inactive {
    background: rgba(248, 113, 113, .1);
    color: #f87171;
    border: 1px solid rgba(248, 113, 113, .2);
}

.status-dot {
    width: 6px;
    height: 6px;
    border-radius: 50%;
    background: currentColor;
}

/* Action buttons */
.actions-cell {
    display: flex;
    align-items: center;
    gap: 4px;
}

.action-btn {
    width: 30px;
    height: 30px;
    border-radius: 8px;
    border: 1px solid transparent;
    background: transparent;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all .2s;
}

.action-view:hover {
    background: rgba(99, 179, 237, .1);
    border-color: rgba(99, 179, 237, .2);
    color: #63b3ed;
}

.action-edit:hover {
    background: rgba(251, 191, 36, .1);
    border-color: rgba(251, 191, 36, .2);
    color: #fbbf24;
}

.action-del:hover {
    background: rgba(248, 113, 113, .1);
    border-color: rgba(248, 113, 113, .2);
    color: #f87171;
}

.action-btn .v-icon {
    color: var(--muted);
    transition: color .2s;
}

.action-btn:hover .v-icon {
    color: inherit;
}

/* Empty state */
.empty-row {
    text-align: center;
}

.empty-state {
    padding: 60px 20px;
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 10px;
}

.empty-state p {
    color: var(--muted);
    font-size: 13px;
    margin: 0;
}

/* ── Pagination ──────────────────────────────────────────────────────── */
.pagination-bar {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 14px 20px;
    border-top: 1px solid var(--border);
    flex-wrap: wrap;
    gap: 10px;
}

.pagination-info {
    font-size: 12px;
    color: var(--muted);
}

.pagination-controls {
    display: flex;
    align-items: center;
    gap: 4px;
}

.pg-btn {
    min-width: 32px;
    height: 32px;
    border-radius: 8px;
    border: 1px solid var(--border);
    background: transparent;
    color: var(--muted);
    cursor: pointer;
    font-family: var(--font);
    font-size: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all .2s;
    padding: 0 6px;
}

.pg-btn:hover:not(:disabled) {
    border-color: var(--accent);
    color: var(--accent);
    background: rgba(99, 179, 237, .07);
}

.pg-btn:disabled {
    opacity: .35;
    cursor: default;
}

.pg-active {
    background: var(--accent) !important;
    border-color: var(--accent) !important;
    color: white !important;
}

.per-page-wrap {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 12px;
    color: var(--muted);
}

.per-page-select {
    padding: 5px 10px;
    background: rgba(255, 255, 255, .04);
    border: 1px solid var(--border);
    border-radius: 8px;
    color: var(--text);
    font-family: var(--font);
    font-size: 12px;
    outline: none;
    cursor: pointer;
}

/* ── Buttons ─────────────────────────────────────────────────────────── */
.btn {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 9px 18px;
    border-radius: 10px;
    font-family: var(--font);
    font-size: 13px;
    font-weight: 600;
    cursor: pointer;
    border: none;
    transition: all .22s;
    text-decoration: none;
}

.btn-primary {
    background: linear-gradient(135deg, var(--accent), var(--accent2));
    color: white;
    box-shadow: 0 4px 16px rgba(99, 179, 237, .25);
}

.btn-primary:hover {
    transform: translateY(-1px);
    box-shadow: 0 6px 20px rgba(99, 179, 237, .35);
}

.btn-ghost {
    background: rgba(255, 255, 255, .05);
    color: var(--muted);
    border: 1px solid var(--border);
}

.btn-ghost:hover {
    border-color: rgba(255, 255, 255, .15);
    color: var(--text);
}

.btn-danger {
    background: linear-gradient(135deg, #f87171, #ef4444);
    color: white;
    box-shadow: 0 4px 14px rgba(248, 113, 113, .25);
}

.btn-danger:hover {
    transform: translateY(-1px);
}

/* ── Delete Dialog ───────────────────────────────────────────────────── */
.dialog-overlay {
    position: fixed;
    inset: 0;
    z-index: 100;
    background: rgba(0, 0, 0, .6);
    backdrop-filter: blur(6px);
    display: flex;
    align-items: center;
    justify-content: center;
}

.dialog-box {
    background: #0d1117;
    border: 1px solid rgba(255, 255, 255, .1);
    border-radius: 18px;
    padding: 36px 32px;
    max-width: 380px;
    width: 90%;
    text-align: center;
    box-shadow: 0 30px 80px rgba(0, 0, 0, .6);
}

.dialog-icon {
    margin-bottom: 16px;
}

.dialog-title {
    font-size: 17px;
    font-weight: 700;
    margin-bottom: 10px;
}

.dialog-body {
    font-size: 13px;
    color: #94a3b8;
    line-height: 1.7;
    margin-bottom: 24px;
}

.dialog-body strong {
    color: var(--text);
}

.dialog-actions {
    display: flex;
    justify-content: center;
    gap: 10px;
}

/* ── Toast ───────────────────────────────────────────────────────────── */
.toast {
    position: fixed;
    bottom: 28px;
    left: 50%;
    transform: translateX(-50%);
    padding: 12px 22px;
    border-radius: 12px;
    font-size: 13px;
    font-weight: 600;
    z-index: 200;
    display: flex;
    align-items: center;
    gap: 6px;
    box-shadow: 0 10px 40px rgba(0, 0, 0, .5);
    white-space: nowrap;
}

.toast-success {
    background: rgba(52, 211, 153, .15);
    border: 1px solid rgba(52, 211, 153, .3);
    color: #34d399;
}

.toast-error {
    background: rgba(248, 113, 113, .15);
    border: 1px solid rgba(248, 113, 113, .3);
    color: #f87171;
}

/* ── Transitions ─────────────────────────────────────────────────────── */
.dialog-fade-enter-active,
.dialog-fade-leave-active {
    transition: opacity .2s;
}

.dialog-fade-enter-from,
.dialog-fade-leave-to {
    opacity: 0;
}

.toast-anim-enter-active,
.toast-anim-leave-active {
    transition: all .3s;
}

.toast-anim-enter-from {
    opacity: 0;
    transform: translate(-50%, 16px);
}

.toast-anim-leave-to {
    opacity: 0;
    transform: translate(-50%, 16px);
}

/* ── Responsive ──────────────────────────────────────────────────────── */
@media (max-width: 900px) {
    .stats-row {
        grid-template-columns: repeat(2, 1fr);
    }
}

@media (max-width: 600px) {
    .stats-row {
        grid-template-columns: 1fr 1fr;
    }

    .toolbar-card {
        flex-wrap: wrap;
    }

    .filters {
        flex-wrap: wrap;
    }
}
</style>
