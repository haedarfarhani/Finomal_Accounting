<template>
  <div class="page-root font-fa">

    <!-- ===== 1. Page Header ===== -->
    <header class="page-header no-print">
      <div class="header-right">
        <div class="header-icon-wrap">
          <svg class="glow-icon" width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="var(--cyan)" stroke-width="2">
            <path d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5" />
          </svg>
        </div>
        <div>
          <h1 class="page-title">تعریف سرفصل‌ها (کدینگ حسابداری)</h1>
          <p class="page-sub">ساختار درختی طبقه‌بندی مالی سرفصل‌های گروه، کل، معین و تفصیلی فینومال</p>
        </div>
      </div>
      <div class="header-left">
        <button class="btn-print-coding" @click="printCodingTree">
          <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <polyline points="6 9 6 2 18 2 18 9" />
            <path d="M6 18H4a2 2 0 01-2-2v-5a2 2 0 012-2h16a2 2 0 012 2v5a2 2 0 01-2 2h-2" />
            <rect x="6" y="14" width="12" height="8" />
          </svg>
          چاپ درختواره کدینگ
        </button>
        <v-menu transition="slide-y-transition" location="bottom end">
          <template v-slot:activator="{ props }">
            <button class="btn-add-account-top" v-bind="props">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" style="margin-left: 6px;">
                <line x1="12" y1="5" x2="12" y2="19" />
                <line x1="5" y1="12" x2="19" y2="12" />
              </svg>
              تعریف سرفصل جدید...
            </button>
          </template>
          <v-list bg-color="#080c14" border class="mt-1 dark-menu-list" rounded="lg" style="border-color: rgba(56, 189, 248, 0.25) !important;">
            <v-list-item link @click="openCreateDialogForLevel(1)">
              <template v-slot:prepend>
                <span class="lvl-badge-sm lvl-1" style="margin-left: 8px;">۱</span>
              </template>
              <v-list-item-title class="font-fa text-body-2 text-grey-lighten-2">تعریف گروه حساب جدید</v-list-item-title>
            </v-list-item>
            <v-list-item link @click="openCreateDialogForLevel(2)">
              <template v-slot:prepend>
                <span class="lvl-badge-sm lvl-2" style="margin-left: 8px;">۲</span>
              </template>
              <v-list-item-title class="font-fa text-body-2 text-grey-lighten-2">تعریف حساب کل جدید</v-list-item-title>
            </v-list-item>
            <v-list-item link @click="openCreateDialogForLevel(3)">
              <template v-slot:prepend>
                <span class="lvl-badge-sm lvl-3" style="margin-left: 8px;">۳</span>
              </template>
              <v-list-item-title class="font-fa text-body-2 text-grey-lighten-2">تعریف حساب معین جدید</v-list-item-title>
            </v-list-item>
            <v-list-item link @click="openCreateDialogForLevel(4)">
              <template v-slot:prepend>
                <span class="lvl-badge-sm lvl-4" style="margin-left: 8px;">۴</span>
              </template>
              <v-list-item-title class="font-fa text-body-2 text-grey-lighten-2">تعریف حساب تفصیلی جدید</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </div>
    </header>

    <!-- ===== 2. Two-Column Layout (Sidebar filters + Tree Grid) ===== -->
    <div class="coding-layout-grid mt-4 no-print">

      <!-- Right Column: Sidebar quick filter -->
      <aside class="sidebar-filters glass-card">
        <div class="sidebar-title">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <polygon points="22 3 2 3 10 12.46 10 19 14 21 14 12.46 22 3" />
          </svg>
          فیلتر سریع گروه‌ها
        </div>
        <div class="sidebar-menu">
          <button class="sidebar-menu-item" :class="{ 'active-menu-item': activeSidebarFilter === 'all' }" @click="selectSidebarFilter('all')">
            <span class="menu-dot all-dot"></span>
            <span>همه سرفصل‌ها</span>
            <span class="menu-count font-fa">{{ accounts.length }}</span>
          </button>
          <button v-for="grp in groupAccounts" :key="grp.id" class="sidebar-menu-item"
            :class="{ 'active-menu-item': activeSidebarFilter === grp.id }" @click="selectSidebarFilter(grp.id)">
            <span class="menu-dot" :style="{ backgroundColor: getSidebarDotColor(grp.code) }"></span>
            <span class="menu-text" :title="grp.title">{{ grp.title }}</span>
            <span class="menu-count font-fa">{{ getChildCountRecursive(grp.id) }}</span>
          </button>
        </div>

        <div class="coding-info-widget mt-4">
          <div class="widget-title">راهنمای ساختار کدینگ</div>
          <div class="widget-levels">
            <div class="widget-level-row">
              <span class="lvl-badge-sm lvl-1">۱</span>
              <span>گروه حساب (۱ رقم - مانند ۱)</span>
            </div>
            <div class="widget-level-row">
              <span class="lvl-badge-sm lvl-2">۲</span>
              <span>حساب کل (۲ رقم - مانند ۱۱)</span>
            </div>
            <div class="widget-level-row">
              <span class="lvl-badge-sm lvl-3">۳</span>
              <span>حساب معین (۴ رقم - مانند ۱۱۰۱)</span>
            </div>
            <div class="widget-level-row">
              <span class="lvl-badge-sm lvl-4">۴</span>
              <span>حساب تفصیلی (مانند ۱۱۰۱-۰۱)</span>
            </div>
          </div>
        </div>
      </aside>

      <!-- Left Column: Search & Interactive Tree Grid -->
      <main class="main-tree-area">
        <section class="glass-card table-card">
          <!-- Table Toolbar search -->
          <div class="table-toolbar mb-4" style="display: flex; gap: 1rem; align-items: center; flex-wrap: wrap;">
            <div class="search-input-wrap" style="flex: 1; min-width: 250px;">
              <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" placeholder="جستجوی کد یا عنوان حساب..."
                variant="outlined" density="compact" hide-details rounded="lg" class="dark-field-search" clearable />
            </div>
            <div class="level-filter-wrap">
              <v-btn-toggle v-model="activeLevelFilter" color="cyan" mandatory density="compact" class="dark-btn-toggle rounded-lg" style="background: rgba(15, 23, 42, 0.6); border: 1px solid rgba(255, 255, 255, 0.1);">
                <v-btn value="all" class="font-fa">همه سطوح</v-btn>
                <v-btn :value="1" class="font-fa">گروه</v-btn>
                <v-btn :value="2" class="font-fa">کل</v-btn>
                <v-btn :value="3" class="font-fa">معین</v-btn>
                <v-btn :value="4" class="font-fa">تفصیلی</v-btn>
              </v-btn-toggle>
            </div>
            <div class="toolbar-actions">
              <button class="btn-expand-all" @click="expandAll(true)" title="باز کردن تمام شاخه‌ها">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                  <polyline points="15 3 21 3 21 9M9 21 3 21 3 15M21 3l-7 7M3 21l7-7" />
                </svg>
                گسترش همه
              </button>
              <button class="btn-expand-all" @click="expandAll(false)" title="بستن تمام شاخه‌ها">
                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                  <polyline points="4 14 10 14 10 20M20 10 14 10 14 4M14 10l7-7M10 14l-7 7" />
                </svg>
                جمع کردن همه
              </button>
            </div>
          </div>

          <!-- Tree Table representation -->
          <div class="table-scroll-wrap">
            <table class="coding-tree-table">
              <thead>
                <tr>
                  <th class="th-code">کد ساختاری حساب</th>
                  <th>عنوان سر‌فصل حسابداری</th>
                  <th class="th-level">سطح حساب</th>
                  <th class="th-nature">ماهیت حساب</th>
                  <th class="th-actions">عملیات سرفصل</th>
                </tr>
              </thead>
              <tbody>
                <tr v-if="filteredAccounts.length === 0" class="no-records-row">
                  <td colspan="5">
                    <div class="empty-state-table">
                      <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
                        <circle cx="11" cy="11" r="8" />
                        <line x1="21" y1="21" x2="16.65" y2="16.65" />
                      </svg>
                      <div>حسابی مطابق با فیلتر یا جستجوی شما یافت نشد.</div>
                    </div>
                  </td>
                </tr>
                <tr v-for="act in filteredAccounts" :key="act.id" class="tree-row"
                  :class="[getRowClass(act), { 'highlight-match': isSearchedAndMatched(act), 'clickable-row': hasChildren(act.id) }]"
                  @click="hasChildren(act.id) ? toggleExpand(act) : null">

                  <!-- Structural Code column -->
                  <td class="td-code font-fa">
                    <!-- Connective hierarchy lines -->
                    <div class="tree-connector" v-if="act.level > 1" :style="{ '--indent-level': act.level - 1 }"></div>

                    <span class="code-badge" :style="{ paddingRight: (act.level - 1) * 22 + 'px' }">
                      <span class="code-text" :class="'lvl-color-' + act.level">{{ act.code }}</span>
                    </span>
                  </td>

                  <!-- Account Title Column with Collapse Toggle -->
                  <td>
                    <div class="title-cell-wrap" :style="{ paddingRight: (act.level - 1) * 22 + 'px' }">
                      <!-- Toggle button for expanding/collapsing children (Only shown if children exist) -->
                      <button v-if="hasChildren(act.id)" class="btn-toggle-expand" @click.stop="toggleExpand(act)"
                        :class="{ 'expanded-rotated': act.expanded }">
                        <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
                          <polyline points="15 18 9 12 15 6" />
                        </svg>
                      </button>
                      <span v-else class="bullet-spacer"></span>

                      <!-- Folder/File Icon -->
                      <span class="node-icon">
                        <svg v-if="act.level === 1" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                          <path d="M22 19a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2h5l2 3h9a2 2 0 012 2z" />
                        </svg>
                        <svg v-else-if="act.level === 2" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                          <path d="M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2zM22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z" />
                        </svg>
                        <svg v-else-if="act.level === 3" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                          <rect x="3" y="3" width="18" height="18" rx="2" ry="2" />
                          <line x1="9" y1="3" x2="9" y2="21" />
                        </svg>
                        <svg v-else width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                          <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z" />
                          <polyline points="14 2 14 8 20 8" />
                        </svg>
                      </span>

                      <!-- Account Title Text -->
                      <span class="account-title" :class="{ 'font-weight-black': act.level === 1 }">{{ act.title }}</span>
                    </div>
                  </td>

                  <!-- Level Chip Column -->
                  <td>
                    <span class="lvl-badge" :class="'lvl-' + act.level">
                      {{ getLevelLabel(act.level) }}
                    </span>
                  </td>

                  <!-- Nature Column -->
                  <td>
                    <span class="nature-badge" :class="'nature-' + act.nature">
                      {{ getNatureLabel(act.nature) }}
                    </span>
                  </td>

                  <!-- Direct Actions Column -->
                  <td>
                    <div class="row-actions">
                      <!-- Add direct child button (Only allowed for levels 1, 2, and 3) -->
                      <button v-if="act.level < 4" class="btn-node-act act-add-child" title="افزودن زیرمجموعه مستقیم" @click.stop="openCreateDialog(act)">
                        <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                          <line x1="12" y1="5" x2="12" y2="19" />
                          <line x1="5" y1="12" x2="19" y2="12" />
                        </svg>
                      </button>

                      <!-- Edit -->
                      <button class="btn-node-act act-edit" title="ویرایش حساب" @click.stop="openEditDialog(act)">
                        <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                          <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" />
                          <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z" />
                        </svg>
                      </button>

                      <!-- Delete -->
                      <button class="btn-node-act act-del" title="حذف حساب" @click.stop="handleDeleteAccount(act.id)" :disabled="act.isSystem">
                        <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                          <polyline points="3 6 5 6 21 6" />
                          <path d="M19 6l-1 14H6L5 6" />
                        </svg>
                      </button>
                    </div>
                  </td>

                </tr>
              </tbody>
            </table>
          </div>
        </section>
      </main>

    </div>

    <!-- ===== 3. Dynamic Dialog for Create/Edit Account (VDialog) ===== -->
    <v-dialog v-model="dialogOpen" max-width="500px" transition="dialog-bottom-transition">
      <div class="account-dialog-wrapper glass-card">
        <div class="dialog-header">
          <div class="dialog-title">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="var(--cyan)" stroke-width="2.5">
              <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
            </svg>
            {{ isEditingMode ? 'اصلاح و ویرایش سرفصل حساب' : 'تعریف سرفصل جدید حسابداری' }}
          </div>
          <button class="btn-close-dialog" @click="dialogOpen = false">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <line x1="18" y1="6" x2="6" y2="18" />
              <line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>

        <div class="dialog-body mt-4">
          <!-- Parent Account Selection (If editing or creating group, this is managed carefully) -->
          <div class="field-wrap mb-4">
            <label class="field-label">انتخاب حساب والد (سرفصل بالاتر)</label>
            <v-select v-model="form.parentId" :items="parentOptions" item-title="title" item-value="id"
              variant="outlined" density="compact" class="dark-field" placeholder="بدون والد (تعریف به عنوان سطح ۱ - گروه)"
              hide-details rounded="lg" :disabled="isEditingMode" @update:modelValue="onParentChange" clearable />
          </div>

          <div class="dialog-fields-row mb-4">
            <!-- Account Level (Automatic based on parent) -->
            <div class="field-wrap">
              <label class="field-label">سطح حسابداری حاصله</label>
              <div class="static-badge-wrap">
                <span class="lvl-badge lvl-dialog" :class="'lvl-' + form.level">
                  {{ getLevelLabel(form.level) }} (سطح {{ form.level }})
                </span>
              </div>
            </div>

            <!-- Account Nature -->
            <div class="field-wrap">
              <label class="field-label">ماهیت مالی سرفصل</label>
              <v-select v-model="form.nature" :items="natureOptions" item-title="title" item-value="id"
                variant="outlined" density="compact" class="dark-field" hide-details rounded="lg" />
            </div>
          </div>

          <!-- Account Title Name -->
          <div class="field-wrap mb-4">
            <label class="field-label">عنوان رسمی سرفصل حساب</label>
            <v-text-field v-model="form.title" variant="outlined" density="compact"
              placeholder="مثال: بانک ملت شعبه تجریش..." class="dark-field" hide-details rounded="lg" />
          </div>

          <!-- Account Code (With Autogeneration Notice) -->
          <div class="field-wrap mb-4">
            <label class="field-label">کد معین ساختاری حساب (کدینگ)</label>
            <v-text-field v-model="form.code" variant="outlined" density="compact"
              placeholder="مثال: 1102-01" class="dark-field code-input font-fa" hide-details rounded="lg" />
            <div class="code-generation-tip font-fa" v-if="!isEditingMode && form.parentId">
              💡 پیشنهاد سیستم بر اساس ساختار والد: <strong>{{ suggestedCode }}</strong>
            </div>
          </div>

          <!-- Account Description -->
          <div class="field-wrap mb-4">
            <label class="field-label">توضیحات و شرح عملیاتی حساب</label>
            <v-textarea v-model="form.description" variant="outlined" density="compact"
              placeholder="یادداشت‌های بیشتر..." class="dark-field" hide-details rounded="lg" rows="2" />
          </div>
        </div>

        <div class="dialog-footer">
          <button class="btn-dialog-cancel" @click="dialogOpen = false">انصراف</button>
          <button class="btn-dialog-submit" @click="saveAccount" :disabled="!form.title || !form.code">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <polyline points="20 6 9 17 4 12" />
            </svg>
            ذخیره و ثبت سرفصل
          </button>
        </div>
      </div>
    </v-dialog>

    <!-- ===== 4. Printable Coding Tree View Area (Print-only) ===== -->
    <div class="print-coding-container" id="printable-coding-area">
      <div class="print-header">
        <div class="print-header-right">
          <h2 class="print-company-name">شرکت توسعه فناوری مالی فینومال</h2>
          <h3 class="print-doc-title">برگ درختواره و ساختار کدینگ رسمی حساب‌ها</h3>
        </div>
        <div class="print-header-center">
          <div class="print-logo-dummy">Finomal Coding</div>
        </div>
        <div class="print-header-left font-fa">
          <div><strong>تاریخ چاپ:</strong> {{ new Date().toLocaleDateString('fa-IR') }}</div>
          <div><strong>تعداد کل سرفصل‌ها:</strong> {{ accounts.length }}</div>
        </div>
      </div>

      <table class="print-coding-table">
        <thead>
          <tr>
            <th style="width: 150px;">کد حساب</th>
            <th>عنوان سرفصل حسابداری</th>
            <th style="width: 100px;">سطح حساب</th>
            <th style="width: 100px;">ماهیت حساب</th>
            <th>توضیحات</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="act in sortedAccounts" :key="act.id" :class="'print-lvl-row-' + act.level">
            <td class="font-fa" :style="{ paddingRight: (act.level - 1) * 16 + 'px', fontWeight: act.level === 1 ? 'bold' : 'normal' }">
              {{ act.code }}
            </td>
            <td :style="{ paddingRight: (act.level - 1) * 16 + 'px', fontWeight: act.level === 1 ? 'bold' : 'normal' }">
              {{ act.title }}
            </td>
            <td>{{ getLevelLabel(act.level) }}</td>
            <td>{{ getNatureLabel(act.nature) }}</td>
            <td>{{ act.description || '—' }}</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- ===== 5. Toast Notification Alert ===== -->
    <transition name="toast">
      <div v-if="toast.show" class="toast-premium" :class="'toast-' + toast.type">
        <div class="toast-content">
          <div class="toast-icon">
            <svg v-if="toast.type === 'success'" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
              <polyline points="20 6 9 17 4 12" />
            </svg>
            <svg v-else width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
              <circle cx="12" cy="12" r="10" />
              <line x1="12" y1="8" x2="12" y2="12" />
              <line x1="12" y1="16" x2="12.01" y2="16" />
            </svg>
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

// --- Mock Database (Persian Coding Chart of Accounts) ---
// Populated with Level 1 (Groups), Level 2 (Ledger کل), Level 3 (Subsidiary معین)
const accounts = ref([
  // ===== دارایی‌های جاری (Group 1) =====
  { id: 1, code: '1', title: 'دارایی‌های جاری', level: 1, parentId: null, nature: 'debit', description: 'دارایی‌های با نقدشوندگی کمتر از یک سال مالی', expanded: true, isSystem: true },

  // Ledger 11
  { id: 11, code: '11', title: 'موجودی نقد و بانک', level: 2, parentId: 1, nature: 'debit', description: 'وجوه نقد و حساب‌های بانکی جاری', expanded: true, isSystem: false },
  { id: 1101, code: '1101', title: 'بانک ها', level: 3, parentId: 11, nature: 'debit', description: 'حساب‌های جاری بانکی شرکت', expanded: true, isSystem: false },
  { id: 1102, code: '1102', title: 'صندوق', level: 3, parentId: 11, nature: 'debit', description: 'وجوه نقد داخل صندوق شرکت', expanded: true, isSystem: false },
  { id: 1103, code: '1103', title: 'تنخواه گردان', level: 3, parentId: 11, nature: 'debit', description: 'وجوه در اختیار تنخواه گردان‌های شعب', expanded: true, isSystem: false },

  // Level 4 (Detailed accounts matching the image)
  { id: 110101, code: '1101-01', title: 'بانک ملت شعبه مرکزی', level: 4, parentId: 1101, nature: 'debit', description: 'حساب جاری ملت', expanded: true, isSystem: false },
  { id: 110102, code: '1101-02', title: 'بانک ملی شعبه بازار', level: 4, parentId: 1101, nature: 'debit', description: 'حساب جاری ملی', expanded: true, isSystem: false },
  { id: 110201, code: '1102-01', title: 'صندوق مرکزی', level: 4, parentId: 1102, nature: 'debit', description: 'صندوق اصلی شرکت', expanded: true, isSystem: false },
  { id: 110301, code: '1103-01', title: 'تنخواه گردان دفتر مرکزی', level: 4, parentId: 1103, nature: 'debit', description: 'تنخواه مدیر اجرایی', expanded: true, isSystem: false },

  // Ledger 12
  { id: 12, code: '12', title: 'سرمایه گذاری های کوتاه مدت', level: 2, parentId: 1, nature: 'debit', description: 'سرمایه‌گذاری‌های سریع‌المعامله', expanded: true, isSystem: false },
  { id: 1201, code: '1201', title: 'سپرده های بانکی', level: 3, parentId: 12, nature: 'debit', description: 'سپرده‌های کوتاه‌مدت بانکی', expanded: false, isSystem: false },
  { id: 1202, code: '1202', title: 'سهام شرکت ها', level: 3, parentId: 12, nature: 'debit', description: 'سهام بورسی خریداری شده', expanded: false, isSystem: false },

  // Ledger 14 (Mapped to 13 in image, but using 14 to fit existing structure)
  { id: 14, code: '14', title: 'حساب ها و اسناد دریافتنی تجاری', level: 2, parentId: 1, nature: 'debit', description: 'مطالبات حاصل از فروش کالا یا ارائه خدمات تجاری', expanded: true, isSystem: false },
  { id: 1401, code: '1401', title: 'حساب های دریافتنی تجاری', level: 3, parentId: 14, nature: 'debit', description: 'مطالبات از مشتریان', expanded: false, isSystem: false },
  { id: 1402, code: '1402', title: 'اسناد دریافتنی تجاری', level: 3, parentId: 14, nature: 'debit', description: 'چک‌های دریافتی از مشتریان', expanded: false, isSystem: false },

  // Ledger 18 (Mapped to 14 in image, but using 18 to fit existing structure)
  { id: 18, code: '18', title: 'پیش پرداخت ها و سفارشات', level: 2, parentId: 1, nature: 'debit', description: 'پیش‌پرداخت خرید اقلام تجاری و سفارش‌های خارجی', expanded: true, isSystem: false },
  { id: 1801, code: '1801', title: 'پیش پرداخت ها', level: 3, parentId: 18, nature: 'debit', description: 'پیش‌پرداخت خرید کالا و خدمات', expanded: false, isSystem: false },
  { id: 1802, code: '1802', title: 'سفارشات', level: 3, parentId: 18, nature: 'debit', description: 'سفارش‌های وارداتی در جریان ترخیص کالا', expanded: false, isSystem: false },

  // Ledger 18
  { id: 18, code: '18', title: 'سفارش‌ها و پیش‌پرداخت‌ها', level: 2, parentId: 1, nature: 'debit', description: 'پیش‌پرداخت خرید اقلام تجاری و سفارش‌های خارجی', expanded: false, isSystem: false },
  { id: 1801, code: '1801', title: 'سفارش‌ها', level: 3, parentId: 18, nature: 'debit', description: 'سفارش‌های وارداتی در جریان ترخیص کالا', expanded: false, isSystem: false },
  { id: 1802, code: '1802', title: 'پیش‌پرداخت خرید کالا و خدمات', level: 3, parentId: 18, nature: 'debit', description: 'پیش‌پرداخت خرید اقلام مصرفی و دارایی تجاری ریالی', expanded: false, isSystem: false },
  { id: 1803, code: '1803', title: 'پیش‌پرداخت مالیات عملکرد', level: 3, parentId: 18, nature: 'debit', description: 'علی‌الحساب مالیاتی واریز شده سال جاری', expanded: false, isSystem: false },
  { id: 1804, code: '1804', title: 'پیش‌پرداخت اجاره', level: 3, parentId: 18, nature: 'debit', description: 'اجاره‌های پیش‌پرداخت شده ساختمان‌ها', expanded: false, isSystem: false },
  { id: 1805, code: '1805', title: 'پیش‌پرداخت بیمه', level: 3, parentId: 18, nature: 'debit', description: 'پیش‌پرداخت حق بیمه دارایی‌ها و کارکنان', expanded: false, isSystem: false },
  { id: 1811, code: '1811', title: 'سایر پیش‌پرداخت‌ها', level: 3, parentId: 18, nature: 'debit', description: 'پیش‌پرداخت‌های متفرقه عملیاتی', expanded: false, isSystem: false },

  // Ledger 19
  { id: 19, code: '19', title: 'دارایی‌های نگهداری‌شده برای فروش', level: 2, parentId: 1, nature: 'debit', description: 'دارایی‌های ثابت خارج از رده جهت واگذاری', expanded: false, isSystem: false },
  { id: 1901, code: '1901', title: 'وسایل نقلیه‌ی فرسوده برای فروش', level: 3, parentId: 19, nature: 'debit', description: 'ماشین‌آلات ترابری فرسوده آماده فروش', expanded: false, isSystem: false },
  { id: 1902, code: '1902', title: 'اثاثیه و منصوبات خارج از رده برای فروش', level: 3, parentId: 19, nature: 'debit', description: 'کامپیوترها و مبلمان خارج از رده آماده فروش', expanded: false, isSystem: false },


  // ===== دارایی‌های غیرجاری (Group 2) =====
  { id: 2, code: '2', title: 'دارایی‌های غیرجاری', level: 1, parentId: null, nature: 'debit', description: 'دارایی‌های بلندمدت با عمر مفید بیش از یک سال مالی', expanded: false, isSystem: true },

  // Ledger 21
  { id: 21, code: '21', title: 'حساب‌ها و اسناد دریافتنی بلندمدت', level: 2, parentId: 2, nature: 'debit', description: 'مطالبات بلندمدت غیرتجاری', expanded: false, isSystem: false },
  { id: 2101, code: '2101', title: 'شرکت‌ها، موسسه‌ها و اشخاص', level: 3, parentId: 21, nature: 'debit', description: 'مطالبات بلندمدت از شرکت‌های همکار و اشخاص حقیقی', expanded: false, isSystem: false },
  { id: 2102, code: '2102', title: 'اسناد دریافتنی بلندمدت نزد صندوق', level: 3, parentId: 21, nature: 'debit', description: 'چک‌ها و سفته‌های بلندمدت موجود در شرکت', expanded: false, isSystem: false },
  { id: 2103, code: '2103', title: 'اسناد دریافتنی بلندمدت نزد بانک‌ها', level: 3, parentId: 21, nature: 'debit', description: 'اسناد تجاری بلندمدت تودیع شده نزد بانک', expanded: false, isSystem: false },
  { id: 2199, code: '2199', title: 'ذخیره‌ی مطالبات مشکوک‌الوصول بلندمدت', level: 3, parentId: 21, nature: 'credit', description: 'ذخیره مطالبات بلندمدت غیرتجاری', expanded: false, isSystem: false },

  // Ledger 22
  { id: 22, code: '22', title: 'سرمایه‌گذاری بلندمدت', level: 2, parentId: 2, nature: 'debit', description: 'سرمایه‌گذاری‌های با اهداف بلندمدت و استراتژیک', expanded: false, isSystem: false },
  { id: 2201, code: '2201', title: 'سرمایه‌گذاری در سهام شرکت‌ها', level: 3, parentId: 22, nature: 'debit', description: 'مشارکت بلندمدت در سرمایه سایر شرکت‌ها', expanded: false, isSystem: false },
  { id: 2299, code: '2299', title: 'ذخیره‌ی کاهش ارزش سرمایه‌گذاری‌های بلندمدت', level: 3, parentId: 22, nature: 'credit', description: 'ذخیره افت ارزش سهام بلندمدت مشارکتی', expanded: false, isSystem: false },

  // Ledger 23
  { id: 23, code: '23', title: 'سرمایه‌گذاری در املاک', level: 2, parentId: 2, nature: 'debit', description: 'املاک و اراضی خریداری شده جهت افزایش ارزش و سرمایه‌گذاری', expanded: false, isSystem: false },
  { id: 2301, code: '2301', title: 'سرمایه‌گذاری در املاک مسکونی', level: 3, parentId: 23, nature: 'debit', description: 'آپارتمان‌های مسکونی ملکی غیرعملیاتی', expanded: false, isSystem: false },
  { id: 2302, code: '2302', title: 'سرمایه‌گذاری در املاک تجاری', level: 3, parentId: 23, nature: 'debit', description: 'مغازه‌ها و غرفه‌های ملکی غیرعملیاتی', expanded: false, isSystem: false },
  { id: 2303, code: '2303', title: 'سرمایه‌گذاری در املاک اداری', level: 3, parentId: 23, nature: 'debit', description: 'ساختمان‌های اداری ملکی غیرعملیاتی', expanded: false, isSystem: false },

  // Ledger 24
  { id: 24, code: '24', title: 'دارایی‌های نامشهود', level: 2, parentId: 2, nature: 'debit', description: 'دارایی‌های فاقد ماهیت فیزیکی ولی دارای ارزش مالی بالا', expanded: false, isSystem: false },
  { id: 2401, code: '2401', title: 'نرم‌افزارهای مالی', level: 3, parentId: 24, nature: 'debit', description: 'حق استفاده و پروانه‌های نرم‌افزارهای ERP فینومال', expanded: false, isSystem: false },
  { id: 2402, code: '2402', title: 'دانش فنی', level: 3, parentId: 24, nature: 'debit', description: 'امتیاز اختراع و فرمول‌های ساخت محصولات', expanded: false, isSystem: false },
  { id: 2403, code: '2403', title: 'سرقفلی', level: 3, parentId: 24, nature: 'debit', description: 'حق سرقفلی و ارزش برند شرکت', expanded: false, isSystem: false },

  // Ledger 25
  { id: 25, code: '25', title: 'دارایی‌های ثابت مشهود', level: 2, parentId: 2, nature: 'debit', description: 'دارایی‌های فیزیکی مولد و عملیاتی شرکت', expanded: false, isSystem: false },
  { id: 2501, code: '2501', title: 'زمین', level: 3, parentId: 25, nature: 'debit', description: 'اراضی ملکی تحت مالکیت شرکت', expanded: false, isSystem: false },
  { id: 2502, code: '2502', title: 'ساختمان', level: 3, parentId: 25, nature: 'debit', description: 'ساختمان‌های مرکزی و کارگاه‌های ملکی', expanded: false, isSystem: false },
  { id: 2503, code: '2503', title: 'تاسیسات', level: 3, parentId: 25, nature: 'debit', description: 'تاسیسات سرمایش، گرمایش و تجهیزات کارگاه', expanded: false, isSystem: false },
  { id: 2506, code: '2506', title: 'وسایل نقلیه', level: 3, parentId: 25, nature: 'debit', description: 'ماشین‌آلات ترابری و سواری اداری', expanded: false, isSystem: false },
  { id: 2508, code: '2508', title: 'اثاثیه و منصوبات', level: 3, parentId: 25, nature: 'debit', description: 'مبلمان، کامپیوترها و سیستم‌های اداری پرسنل', expanded: false, isSystem: false },
  { id: 2511, code: '2511', title: 'دارایی‌های در جریان ساخت', level: 3, parentId: 25, nature: 'debit', description: 'پروژه‌های ساختمانی نیمه‌کاره در دست ساخت', expanded: false, isSystem: false },
  { id: 2522, code: '2522', title: 'استهلاک انباشته‌ی ساختمان', level: 3, parentId: 25, nature: 'credit', description: 'ذخیره استهلاک تجمیعی ساختمان', expanded: false, isSystem: false },
  { id: 2523, code: '2523', title: 'استهلاک انباشته‌ی تاسیسات', level: 3, parentId: 25, nature: 'credit', description: 'ذخیره استهلاک تجمیعی ماشین‌آلات و تاسیسات', expanded: false, isSystem: false },
  { id: 2526, code: '2526', title: 'استهلاک انباشته‌ی وسایل نقلیه', level: 3, parentId: 25, nature: 'credit', description: 'ذخیره استهلاک تجمیعی اتومبیل‌های اداری', expanded: false, isSystem: false },
  { id: 2528, code: '2528', title: 'استهلاک انباشته‌ی اثاثیه و منصوبات', level: 3, parentId: 25, nature: 'credit', description: 'ذخیره استهلاک تجمیعی کامپیوترها و مبلمان', expanded: false, isSystem: false },

  // Ledger 26
  { id: 26, code: '26', title: 'سایر دارایی‌ها', level: 2, parentId: 2, nature: 'debit', description: 'دارایی‌های غیرجاری متفرقه و سپرده‌های بلندمدت', expanded: false, isSystem: false },
  { id: 2601, code: '2601', title: 'حق‌الامتیاز آب', level: 3, parentId: 26, nature: 'debit', description: 'امتیاز کنتور آب اداری', expanded: false, isSystem: false },
  { id: 2602, code: '2602', title: 'حق‌الامتیاز برق', level: 3, parentId: 26, nature: 'debit', description: 'امتیاز کنتور برق سه فاز صنعتی اداری', expanded: false, isSystem: false },
  { id: 2603, code: '2603', title: 'حق‌الامتیاز تلفن', level: 3, parentId: 26, nature: 'debit', description: 'خطوط تلفن ثابت شرکت', expanded: false, isSystem: false },
  { id: 2604, code: '2604', title: 'حق‌الامتیاز گاز', level: 3, parentId: 26, nature: 'debit', description: 'امتیاز کنتور گاز محل شرکت', expanded: false, isSystem: false },
  { id: 2606, code: '2606', title: 'هزینه‌های انتقالی به دوره‌های آتی', level: 3, parentId: 26, nature: 'debit', description: 'مخارج تاسیس و قبل از بهره‌برداری غیرجاری', expanded: false, isSystem: false },

  // Ledger 29
  { id: 29, code: '29', title: 'تراز افتتاحیه و اختتامیه', level: 2, parentId: 2, nature: 'both', description: 'حساب‌های رابط و واسطه بستن و باز کردن سال مالی', expanded: false, isSystem: true },
  { id: 2901, code: '2901', title: 'تراز افتتاحیه و اختتامیه', level: 3, parentId: 29, nature: 'both', description: 'تراکت واسطه‌ای سال مالی', expanded: false, isSystem: true },


  // ===== بدهی‌های جاری (Group 3) =====
  { id: 3, code: '3', title: 'بدهی‌های جاری', level: 1, parentId: null, nature: 'credit', description: 'تعهدات مالی کوتاه‌مدت با سررسید زیر یک سال مالی', expanded: false, isSystem: true },

  // Ledger 31
  { id: 31, code: '31', title: 'اسناد پرداختنی کوتاه‌مدت', level: 2, parentId: 3, nature: 'credit', description: 'چک‌های صادر شده عهده حساب‌های جاری شرکت', expanded: false, isSystem: false },
  { id: 3101, code: '3101', title: 'اسناد پرداختنی ریالی', level: 3, parentId: 31, nature: 'credit', description: 'چک‌های جاری ریالی صادر شده در وجه بستانکاران', expanded: false, isSystem: false },

  // Ledger 32
  { id: 32, code: '32', title: 'حساب‌های پرداختنی تجاری', level: 2, parentId: 3, nature: 'credit', description: 'بدهی بابت خرید مواد اولیه و خدمات عملیاتی', expanded: false, isSystem: false },
  { id: 3201, code: '3201', title: 'شرکت‌ها، موسسه‌ها و سازمان‌ها', level: 3, parentId: 32, nature: 'credit', description: 'بستانکاران تجاری حقوقی ملزومات', expanded: false, isSystem: false },
  { id: 3202, code: '3202', title: 'حساب‌های پرداختنی تجاری (اشخاص حقیقی)', level: 3, parentId: 32, nature: 'credit', description: 'بستانکاران تجاری حقیقی', expanded: false, isSystem: false },

  // Ledger 33
  { id: 33, code: '33', title: 'سایر حساب‌های پرداختنی', level: 2, parentId: 3, nature: 'credit', description: 'بدهی‌های غیرتجاری و جاری شرکت به افراد و نهادها', expanded: false, isSystem: false },
  { id: 3301, code: '3301', title: 'سایر حساب‌های پرداختنی تجاری شرکت‌ها، موسسه‌ها و سازمان‌ها', level: 3, parentId: 33, nature: 'credit', description: 'بستانکاران غیرتجاری حقوقی', expanded: false, isSystem: false },
  { id: 3302, code: '3302', title: 'سایر حساب‌های پرداختنی (اشخاص حقیقی)', level: 3, parentId: 33, nature: 'credit', description: 'بستانکاران غیرتجاری حقیقی', expanded: false, isSystem: false },
  { id: 3303, code: '3303', title: 'حقوق و دستمزد پرداختنی', level: 3, parentId: 33, nature: 'credit', description: 'حقوق کارکنان تسویه نشده دوره مالی', expanded: false, isSystem: false },
  { id: 3304, code: '3304', title: 'سازمان تامین اجتماعی (حق بیمه‌ی کارکنان)', level: 3, parentId: 33, nature: 'credit', description: 'حق بیمه لیست پرسنل پرداختنی به بیمه', expanded: false, isSystem: false },
  { id: 3305, code: '3305', title: 'اداره‌ی امور مالیاتی (مالیات حقوق)', level: 3, parentId: 33, nature: 'credit', description: 'مالیات مکسوره حقوق کارکنان پرداختنی به دارایی', expanded: false, isSystem: false },
  { id: 3306, code: '3306', title: 'اداره‌ی امور مالیاتی (مالیات تکلیفی)', level: 3, parentId: 33, nature: 'credit', description: 'مالیات تکلیفی مکسوره قراردادها پرداختنی', expanded: false, isSystem: false },
  { id: 3307, code: '3307', title: 'اداره‌ی امور مالیاتی (مالیات ارزش افزوده)', level: 3, parentId: 33, nature: 'credit', description: 'بدهی مالیات ارزش افزوده حاصل از فروش مابه التفاوت', expanded: false, isSystem: false },
  { id: 3310, code: '3310', title: 'جاری شریکان یا سهامداران', level: 3, parentId: 33, nature: 'both', description: 'حساب جاری شرکا بابت واریز و برداشت نقدی', expanded: false, isSystem: false },
  { id: 3311, code: '3311', title: 'سپرده‌های دریافتی از شرکت‌ها و موسسه‌ها', level: 3, parentId: 33, nature: 'credit', description: 'ضمانت و سپرده‌های دریافتی شرکت‌های پیمانکاری', expanded: false, isSystem: false },
  { id: 3312, code: '3312', title: 'سپرده‌های دریافتی از اشخاص حقیقی', level: 3, parentId: 33, nature: 'credit', description: 'ضمانت و سپرده‌های مأخوذه از پرسنل یا متفرقه', expanded: false, isSystem: false },
  { id: 3313, code: '3313', title: 'ذخیره‌ی هزینه‌های تحقق‌یافته‌ی پرداخت‌نشده', level: 3, parentId: 33, nature: 'credit', description: 'ذخیره قبوض و هزینه‌های سال جاری پرداخت نشده', expanded: false, isSystem: false },
  { id: 3321, code: '3321', title: 'رند حقوق', level: 3, parentId: 33, nature: 'both', description: 'کسورات ناچیز رندکردن فیش‌های حقوقی پرسنل', expanded: false, isSystem: true },

  // Ledger 34
  { id: 34, code: '34', title: 'مالیات پرداختنی سال جاری', level: 2, parentId: 3, nature: 'credit', description: 'ذخیره مالیات عملکرد تشخیص داده شده', expanded: false, isSystem: false },
  { id: 3401, code: '3401', title: 'مالیات پرداختنی سال جاری', level: 3, parentId: 34, nature: 'credit', description: 'ذخیره مالیات ابرازی اظهارنامه عملکرد', expanded: false, isSystem: false },
  { id: 3402, code: '3402', title: 'مالیات متمم سال‌های قبل', level: 3, parentId: 34, nature: 'credit', description: 'مابه‌التفاوت مالیات برگه‌های تشخیص قطعی سنوات گذشته', expanded: false, isSystem: false },

  // Ledger 35
  { id: 35, code: '35', title: 'سود سهام پرداختنی', level: 2, parentId: 3, nature: 'credit', description: 'سود مصوب تقسیمی مجامع عمومی به صاحبان سهام', expanded: false, isSystem: false },
  { id: 3501, code: '3501', title: 'سود سهام مصوب مجمع عمومی سال جاری', level: 3, parentId: 35, nature: 'credit', description: 'سود مصوب پرداختنی به سهامداران', expanded: false, isSystem: false },
  { id: 3502, code: '3502', title: 'سود سهام پرداختنی معوقه سنوات قبل', level: 3, parentId: 35, nature: 'credit', description: 'سود سهام معوق پرداخت نشده سهامداران', expanded: false, isSystem: false },

  // Ledger 36
  { id: 36, code: '36', title: 'تسهیلات مالی دریافتی کوتاه‌مدت', level: 2, parentId: 3, nature: 'credit', description: 'تسهیلات اخذ شده از بانک‌ها و نهادهای مالی با بازپرداخت زیر یک سال', expanded: false, isSystem: false },
  { id: 3601, code: '3601', title: 'تسهیلات مالی دریافتی از بانک‌ها', level: 3, parentId: 36, nature: 'credit', description: 'وام‌های بانکی دریافتی ریالی جاری کوتاه‌مدت', expanded: false, isSystem: false },
  { id: 3602, code: '3602', title: 'تسهیلات مالی دریافتی از اشخاص', level: 3, parentId: 36, nature: 'credit', description: 'قرض‌الحسنه‌ها و استقراض‌های کوتاه‌مدت از شرکا یا اشخاص', expanded: false, isSystem: false },

  // Ledger 38
  { id: 38, code: '38', title: 'پیش‌دریافت‌ها و سپرده‌های جاری', level: 2, parentId: 3, nature: 'credit', description: 'پیش‌دریافت از مشتریان بابت قراردادها', expanded: false, isSystem: false },
  { id: 3801, code: '3801', title: 'پیش‌دریافت از شرکت‌ها، موسسه‌ها و سازمان‌ها', level: 3, parentId: 38, nature: 'credit', description: 'پیش‌دریافت‌های پیمانی از خریداران حقوقی', expanded: false, isSystem: false },
  { id: 3802, code: '3802', title: 'پیش‌دریافت از اشخاص حقیقی', level: 3, parentId: 38, nature: 'credit', description: 'پیش‌دریافت‌های تجاری از مشتریان حقیقی', expanded: false, isSystem: false },

  // Ledger 39
  { id: 39, code: '39', title: 'بدهی‌های مرتبط با دارایی‌های نگهداری‌شده برای فروش', level: 2, parentId: 3, nature: 'credit', description: 'تعهدات مستقیم متصل به دارایی‌های آماده فروش', expanded: false, isSystem: false },
  { id: 3901, code: '3901', title: 'حق‌الزحمه‌ی پرداختنی کارشناس رسمی', level: 3, parentId: 39, nature: 'credit', description: 'حق کارشناسی رسمی دادگستری بابت ارزش‌گذاری دارایی‌های فروشی', expanded: false, isSystem: false },


  // ===== بدهی‌های بلندمدت (Group 4) =====
  { id: 4, code: '4', title: 'بدهی‌های بلندمدت', level: 1, parentId: null, nature: 'credit', description: 'بدهی‌های بلندمدت شرکت با سررسید بیش از یک سال مالی', expanded: false, isSystem: true },

  // Ledger 41
  { id: 41, code: '41', title: 'اسناد پرداختنی بلندمدت', level: 2, parentId: 4, nature: 'credit', description: 'چک‌های بلندمدت صادر شده در سنوات مالی آتی', expanded: false, isSystem: false },
  { id: 4101, code: '4101', title: 'اسناد پرداختنی سررسید سال‌های آتی', level: 3, parentId: 41, nature: 'credit', description: 'اسناد تجاری پرداختنی بلندمدت سررسیددار', expanded: false, isSystem: false },

  // Ledger 42
  { id: 42, code: '42', title: 'حساب‌های پرداختنی تجاری بلندمدت', level: 2, parentId: 4, nature: 'credit', description: 'بدهی‌های بلندمدت به تامین‌کنندگان ماشین‌آلات خط تولید', expanded: false, isSystem: false },
  { id: 4201, code: '4201', title: 'شرکت‌ها، موسسه‌ها و سازمان‌ها', level: 3, parentId: 42, nature: 'credit', description: 'حقوقی‌های بستانکار تجاری بلندمدت', expanded: false, isSystem: false },
  { id: 4202, code: '4202', title: 'اشخاص حقیقی', level: 3, parentId: 42, nature: 'credit', description: 'حقیقی‌های بستانکار تجاری بلندمدت', expanded: false, isSystem: false },

  // Ledger 45
  { id: 45, code: '45', title: 'تسهیلات مالی دریافتی بلندمدت', level: 2, parentId: 4, nature: 'credit', description: 'وام‌های اخذ شده از سیستم بانکی با بازپرداخت چندساله', expanded: false, isSystem: false },
  { id: 4501, code: '4501', title: 'تسهیلات مالی دریافتی بلندمدت از بانک‌ها', level: 3, parentId: 45, nature: 'credit', description: 'تسهیلات اقساطی بلندمدت دریافتی از بانک‌ها', expanded: false, isSystem: false },

  // Ledger 46
  { id: 46, code: '46', title: 'ذخیره‌ی مزایای پایان خدمت', level: 2, parentId: 4, nature: 'credit', description: 'ذخیره سنوات و حق خدمت کارکنان قابل پرداخت در پایان استخدام', expanded: false, isSystem: false },
  { id: 4601, code: '4601', title: 'ذخیره‌ی مزایای پایان خدمت کارکنان', level: 3, parentId: 46, nature: 'credit', description: 'حق سنوات پایان خدمت پرسنل', expanded: false, isSystem: false },


  // ===== حقوق صاحبان سهام (Group 5) =====
  { id: 5, code: '5', title: 'حقوق صاحبان سهام', level: 1, parentId: null, nature: 'credit', description: 'منافع مالکانه سهامداران در سرمایه خالص شرکت', expanded: false, isSystem: true },

  // Ledger 51
  { id: 51, code: '51', title: 'سرمایه ثبتی', level: 2, parentId: 5, nature: 'credit', description: 'سرمایه اسمی اولیه ثبت شده در اداره ثبت شرکت‌ها', expanded: false, isSystem: false },
  { id: 5101, code: '5101', title: 'سرمایه ثبتی پرداخت شده', level: 3, parentId: 51, nature: 'credit', description: 'سرمایه سهم‌الشرکه شرکای اصلی فینومال', expanded: false, isSystem: false },

  // Ledger 52
  { id: 52, code: '52', title: 'افزایش سرمایه‌ی در جریان', level: 2, parentId: 5, nature: 'credit', description: 'مراحل ثبت افزایش سرمایه رسمی مصوب مجمع', expanded: false, isSystem: false },
  { id: 5201, code: '5201', title: 'افزایش سرمایه از محل مطالبات سهامداران', level: 3, parentId: 52, nature: 'credit', description: 'تهاتر بدهی جاری شرکا با آورده سهام در جریان', expanded: false, isSystem: false },
  { id: 5202, code: '5202', title: 'افزایش سرمایه از محل آورده‌ی نقدی سهامداران', level: 3, parentId: 52, nature: 'credit', description: 'آورده‌های نقدی واریز شده به حساب افزایش سرمایه بانکی', expanded: false, isSystem: false },

  // Ledger 53
  { id: 53, code: '53', title: 'اندوخته‌ی صرف سهام', level: 2, parentId: 5, nature: 'credit', description: 'اندوخته‌های مابه‌التفاوت ارزش اسمی و ارزش بازاری واگذاری سهام', expanded: false, isSystem: false },
  { id: 5301, code: '5301', title: 'اندوخته‌ی صرف سهام افزایش سرمایه‌ی جاری', level: 3, parentId: 53, nature: 'credit', description: 'صرف سهام حاصل از افزایش سرمایه سال جاری', expanded: false, isSystem: false },
  { id: 5302, code: '5302', title: 'اندوخته‌ی صرف سهام افزایش سرمایه‌ی سنوات قبل', level: 3, parentId: 53, nature: 'credit', description: 'صرف سهام تجمیع شده قبلی', expanded: false, isSystem: false },

  // Ledger 54
  { id: 54, code: '54', title: 'اندوخته‌ی قانونی', level: 2, parentId: 5, nature: 'credit', description: 'اندوخته الزامی موضوع ماده ۱۴۰ قانون تجارت (یک بیستم سود خالص)', expanded: false, isSystem: false },
  { id: 5401, code: '5401', title: 'اندوخته‌ی قانونی مصوب', level: 3, parentId: 54, nature: 'credit', description: 'کل اندوخته قانونی انباشته شده', expanded: false, isSystem: false },

  // Ledger 55
  { id: 55, code: '55', title: 'سایر اندوخته‌ها', level: 2, parentId: 5, nature: 'credit', description: 'اندوخته‌های احتیاطی و اختیاری مصوب مجامع شرکت', expanded: false, isSystem: false },
  { id: 5501, code: '5501', title: 'اندوخته‌ی طرح توسعه', level: 3, parentId: 55, nature: 'credit', description: 'اندوخته مخصوص اجرای پروژه‌های عمرانی شرکت', expanded: false, isSystem: false },

  // Ledger 56
  { id: 56, code: '56', title: 'مازاد تجدید ارزیابی دارایی‌ها', level: 2, parentId: 5, nature: 'credit', description: 'مازاد حاصل از ارزیابی دارایی‌ها به ارزش روز کارشناسی', expanded: false, isSystem: false },
  { id: 5601, code: '5601', title: 'ساختمان‌های اداری', level: 3, parentId: 56, nature: 'credit', description: 'مازاد ارزیابی شده ارزش دفتری املاک و آپارتمان‌ها', expanded: false, isSystem: false },

  // Ledger 57
  { id: 57, code: '57', title: 'تفاوت تسعیر ارز عملیات خارجی', level: 2, parentId: 5, nature: 'credit', description: 'سود و زیان تسعیر انباشته تسویه ارز پیمان‌های برون مرزی', expanded: false, isSystem: false },
  { id: 5701, code: '5701', title: 'تفاوت تسعیر ارز پیمان خارجی کشور عراق', level: 3, parentId: 57, nature: 'credit', description: 'تسعیر ارز درآمد و هزینه پروژه‌های مستقر در عراق', expanded: false, isSystem: false },
  { id: 5702, code: '5702', title: 'تفاوت تسعیر ارز عملیات پیمانکاری در دست اجرای بین‌الملل', level: 3, parentId: 57, nature: 'credit', description: 'تسعیر ارز پروژه‌های پیمانی در دست تکمیل خارجی', expanded: false, isSystem: false },

  // Ledger 58
  { id: 58, code: '58', title: 'سود انباشته', level: 2, parentId: 5, nature: 'credit', description: 'سود یا زیان‌های انباشته تقسیم‌نشده سال‌های گذشته', expanded: false, isSystem: false },
  { id: 5801, code: '5801', title: 'سود و زیان انباشته سنواتی', level: 3, parentId: 58, nature: 'credit', description: 'کل سود انباشته باقیمانده انتهای دوره', expanded: false, isSystem: false },
  { id: 5802, code: '5802', title: 'تعدیلات سنواتی طی دوره', level: 3, parentId: 58, nature: 'both', description: 'اصلاحات ناشی از اشتباهات حسابداری یا تغییر روش مالی سنوات پیشین', expanded: false, isSystem: false },

  // Ledger 59 (User listed 'حساب سود و زیان جاری 5903', meaning Ledger 59 exists)
  { id: 59, code: '59', title: 'سود و زیان سال جاری', level: 2, parentId: 5, nature: 'both', description: 'حساب عملکرد سود و زیان جاری سال مالی جاری شرکت', expanded: false, isSystem: true },
  { id: 5903, code: '5903', title: 'حساب سود و زیان جاری', level: 3, parentId: 59, nature: 'both', description: 'سود و زیان جاری دوره قبل از بستن در پایان سال', expanded: false, isSystem: true },


  // ===== درآمدها (Group 6) =====
  { id: 6, code: '6', title: 'درآمدها', level: 1, parentId: null, nature: 'credit', description: 'کل درآمدهای عملیاتی و غیرعملیاتی شرکت فینومال', expanded: false, isSystem: true },

  // Ledger 61
  { id: 61, code: '61', title: 'درآمد خدمات', level: 2, parentId: 6, nature: 'credit', description: 'درآمدهای حاصل از فروش خدمات نرم‌افزاری و مشاوره‌ای', expanded: false, isSystem: false },
  { id: 6101, code: '6101', title: 'درآمد ارائهی خدمات', level: 3, parentId: 61, nature: 'credit', description: 'درآمد کل فروش لایسنس و اشتراک‌های خدمات ابری', expanded: false, isSystem: false },
  { id: 6103, code: '6103', title: 'درآمد خدمات از پروژه‌های جانبی', level: 3, parentId: 61, nature: 'credit', description: 'درآمد حاصل از پشتیبانی و استقرار ERP', expanded: false, isSystem: false },

  // Ledger 62
  { id: 62, code: '62', title: 'سایر درآمدهای عملیاتی', level: 2, parentId: 6, nature: 'credit', description: 'سایر منابع درآمدی متصل به عملیات اصلی شرکت', expanded: false, isSystem: false },
  { id: 6201, code: '6201', title: 'سایر درآمد خدمات متفرقه', level: 3, parentId: 62, nature: 'credit', description: 'درآمدهای خرد خدمات نصب و آموزش', expanded: false, isSystem: false },
  { id: 6203, code: '6203', title: 'سایر درآمدهای عملیاتی عمومی', level: 3, parentId: 62, nature: 'credit', description: 'درآمد فروش کالاهای مازاد یا ضایعات تجاری', expanded: false, isSystem: false },


  // ===== هزینه‌ها (Group 7) =====
  { id: 7, code: '7', title: 'هزینه‌ها', level: 1, parentId: null, nature: 'debit', description: 'هزینه‌های جاری، اداری، حقوق و عملیاتی شرکت فینومال', expanded: false, isSystem: true },

  // Ledger 71
  { id: 71, code: '71', title: 'هزینه‌ی حقوق و دستمزد', level: 2, parentId: 7, nature: 'debit', description: 'مخارج نیروی انسانی و پرسنلی اداری', expanded: false, isSystem: false },
  { id: 7101, code: '7101', title: 'حقوق پایه', level: 3, parentId: 71, nature: 'debit', description: 'حقوق مبنای ماهیانه پرسنل بر اساس قانون کار', expanded: false, isSystem: false },
  { id: 7102, code: '7102', title: 'اضافه کاری', level: 3, parentId: 71, nature: 'debit', description: 'هزینه ساعات کار مازاد پرسنل', expanded: false, isSystem: false },
  { id: 7103, code: '7103', title: 'خواربار و مسکن', level: 3, parentId: 71, nature: 'debit', description: 'حق مسکن و بن خواربار ماهیانه پرسنل', expanded: false, isSystem: false },
  { id: 7104, code: '7104', title: 'حق اولاد', level: 3, parentId: 71, nature: 'debit', description: 'حق عائله‌مندی مصوب پرسنل دارای فرزند', expanded: false, isSystem: false },
  { id: 7105, code: '7105', title: 'کمکهزینهی اقلام مصرفی خانوار', level: 3, parentId: 71, nature: 'debit', description: 'کمک‌هزینه‌های نقدی معیشتی پرسنل', expanded: false, isSystem: false },
  { id: 7106, code: '7106', title: 'نوبت کاری و شبکاری', level: 3, parentId: 71, nature: 'debit', description: 'فوق‌العاده نوبت‌کاری و شب‌کاری پرسنل عملیاتی نوبت‌کار', expanded: false, isSystem: false },
  { id: 7107, code: '7107', title: '%23 حق بیمهی سهم کارفرما', level: 3, parentId: 71, nature: 'debit', description: 'حق بیمه کارفرما واریز شده به تامین اجتماعی', expanded: false, isSystem: false },
  { id: 7108, code: '7108', title: 'عیدی و پاداش', level: 3, parentId: 71, nature: 'debit', description: 'عیدی پایان سال و پاداش کارایی پرسنل', expanded: false, isSystem: false },
  { id: 7109, code: '7109', title: 'حقالجذب', level: 3, parentId: 71, nature: 'debit', description: 'فوق‌العاده جذب و تخصص پرسنل کلیدی', expanded: false, isSystem: false },
  { id: 7110, code: '7110', title: 'بازخرید مرخصی استفادهنشده', level: 3, parentId: 71, nature: 'debit', description: 'حق بازخرید روزهای مرخصی ذخیره شده پرسنل', expanded: false, isSystem: false },
  { id: 7111, code: '7111', title: 'مزایای پایانخدمت کارکنان', level: 3, parentId: 71, nature: 'debit', description: 'حق سنوات پایان خدمت هزینه شده دوره جاری پرسنل', expanded: false, isSystem: false },
  { id: 7112, code: '7112', title: 'کمکهای غیرنقدی', level: 3, parentId: 71, nature: 'debit', description: 'سبدهای کالا و هدیه‌های غیرنقدی توزیعی', expanded: false, isSystem: false },

  // Ledger 72
  { id: 72, code: '72', title: 'هزینه‌های عملیاتی', level: 2, parentId: 7, nature: 'debit', description: 'مخارج روزمره و اداری پشتیبانی عملیات شرکت', expanded: false, isSystem: false },
  { id: 7202, code: '7202', title: 'تعمیر و نگهداری ساختمان و تاسیسات', level: 3, parentId: 72, nature: 'debit', description: 'هزینه‌های تعمیراتی بنا و تاسیسات اداری', expanded: false, isSystem: false },
  { id: 7203, code: '7203', title: 'تعمیر و نگهداری ماشینآلات و تجهیزات', level: 3, parentId: 72, nature: 'debit', description: 'هزینه نگهداری سرورها و ابزارهای سخت‌افزاری', expanded: false, isSystem: false },
  { id: 7204, code: '7204', title: 'تعمیر و نگهداری وسائط نقلیه', level: 3, parentId: 72, nature: 'debit', description: 'هزینه‌های سوخت و تعمیر ماشین‌های ترابری شرکت', expanded: false, isSystem: false },
  { id: 7205, code: '7205', title: 'تعمیر و نگهداری اثاثیه و منصوبات', level: 3, parentId: 72, nature: 'debit', description: 'سرویس ادواری کولرها، مبلمان و وسایل اداری', expanded: false, isSystem: false },
  { id: 7206, code: '7206', title: 'سوخت و انرژی', level: 3, parentId: 72, nature: 'debit', description: 'بنزین و گازوئیل وسایل ترابری اداری', expanded: false, isSystem: false },
  { id: 7207, code: '7207', title: 'آب، برق، گاز و تلفن', level: 3, parentId: 72, nature: 'debit', description: 'هزینه قبوض مصارف دفتری دفتر مرکزی', expanded: false, isSystem: false },
  { id: 7208, code: '7208', title: 'ملزومات، نوشتافزار و آگهی', level: 3, parentId: 72, nature: 'debit', description: 'کاغذ، قلم و مخارج چاپ آگهی روزنامه رسمی', expanded: false, isSystem: false },
  { id: 7209, code: '7209', title: 'آبدارخانه', level: 3, parentId: 72, nature: 'debit', description: 'چای، قند و مواد خوراکی پذیرایی دفتر شرکت', expanded: false, isSystem: false },
  { id: 7210, code: '7210', title: 'حملونقل', level: 3, parentId: 72, nature: 'debit', description: 'مخارج ارسال بار و حمل و نقل اقلام اداری شرکت', expanded: false, isSystem: false },
  { id: 7211, code: '7211', title: 'اجارهی مکان', level: 3, parentId: 72, nature: 'debit', description: 'اجاره‌بهای ماهیانه دفتر و انبار شرکت فینومال', expanded: false, isSystem: false },
  { id: 7212, code: '7212', title: 'بیمهی داراییها', level: 3, parentId: 72, nature: 'debit', description: 'حق بیمه آتش‌سوزی و سرقت ساختمان و سرورها', expanded: false, isSystem: false },
  { id: 7213, code: '7213', title: 'استهلاک داراییها', level: 3, parentId: 72, nature: 'debit', description: 'هزینه استهلاک دوره مالی جاری دارایی‌های ثابت', expanded: false, isSystem: false },
  { id: 7214, code: '7214', title: 'ملزومات مصرفی', level: 3, parentId: 72, nature: 'debit', description: 'ابزارها و وسایل مصرفی خرد پرسنلی فنی', expanded: false, isSystem: false },
  { id: 7215, code: '7215', title: 'ایابوذهاب', level: 3, parentId: 72, nature: 'debit', description: 'هزینه تاکسی و رفت‌وآمد پرسنل جهت کارهای اداری شرکت', expanded: false, isSystem: false },
  { id: 7216, code: '7216', title: 'هزینهی غذای کارکنان', level: 3, parentId: 72, nature: 'debit', description: 'هزینه ناهار تهیه شده برای پرسنل اداری', expanded: false, isSystem: false },
  { id: 7217, code: '7217', title: 'هزینهی پیک و پست', level: 3, parentId: 72, nature: 'debit', description: 'مخارج ارسال مراسلات پستی به مشتریان و بانک‌ها', expanded: false, isSystem: false },
  { id: 7218, code: '7218', title: 'پوشاک کارکنان', level: 3, parentId: 72, nature: 'debit', description: 'لباس کار متحدالشکل پرسنل فنی انبار و اداری', expanded: false, isSystem: false },
  { id: 7219, code: '7219', title: 'بهداشت و درمان', level: 3, parentId: 72, nature: 'debit', description: 'جعبه کمک‌های اولیه و معاینات دوره‌ای طب کار پرسنل', expanded: false, isSystem: false },
  { id: 7220, code: '7220', title: 'هزینهی سفر و ماموریت', level: 3, parentId: 72, nature: 'debit', description: 'فوق‌العاده ماموریت و هزینه‌های هتل و بلیط پرسنل', expanded: false, isSystem: false },
  { id: 7221, code: '7221', title: 'کارمزدهای بانکی', level: 3, parentId: 72, nature: 'debit', description: 'کارمزد تراکنش‌ها، حواله‌ها و صدور دسته‌چک‌های بانکی شرکت', expanded: false, isSystem: false },
  { id: 7222, code: '7222', title: 'هزینههای بستهبندی', level: 3, parentId: 72, nature: 'debit', description: 'کارتن‌ها و هالوگرام‌های بسته‌بندی مرسولات ERP', expanded: false, isSystem: false },
  { id: 7226, code: '7226', title: 'حقالمشاوره', level: 3, parentId: 72, nature: 'debit', description: 'حق‌الزحمه مشاوران مالی، حقوقی و فنی پیمانی', expanded: false, isSystem: false },
  { id: 7227, code: '7227', title: 'هزینههای رایانهای', level: 3, parentId: 72, nature: 'debit', description: 'خرید دامنه‌ها، سرورهای مجازی خرد و ملزومات شبکه', expanded: false, isSystem: false },
  { id: 7228, code: '7228', title: 'هزینهی ثبتی، حق تمبر و وکالت', level: 3, parentId: 72, nature: 'debit', description: 'تمبرهای مالیاتی چک‌ها و مخارج تغییرات اساسنامه شرکت', expanded: false, isSystem: false },

  // Ledger 73
  { id: 73, code: '73', title: 'هزینه‌های توزیع و فروش', level: 2, parentId: 7, nature: 'debit', description: 'هزینه‌های مستقیم بازاریابی و فروش کالا و خدمات', expanded: false, isSystem: false },
  { id: 7301, code: '7301', title: 'هزینههای تبلیغات، بازاریابی، کاتالوگ و بروشور', level: 3, parentId: 73, nature: 'debit', description: 'کمپین‌های دیجیتال مارکتینگ و چاپ کاتالوگ فروش', expanded: false, isSystem: false },
  { id: 7302, code: '7302', title: 'هزینهی انبارداری', level: 3, parentId: 73, nature: 'debit', description: 'مخارج نگهداری کالا در انبارهای عمومی', expanded: false, isSystem: false },
  { id: 7303, code: '7303', title: 'کمیسیونهای پرداختی', level: 3, parentId: 73, nature: 'debit', description: 'پورسانت‌های پرداختی به بازاریاب‌ها و نمایندگی‌ها', expanded: false, isSystem: false },
  { id: 7304, code: '7304', title: 'هزینهی حملونقل کالای فروشرفته', level: 3, parentId: 73, nature: 'debit', description: 'مخارج ارسال لایسنس یا محموله‌ها در وجه مشتریان', expanded: false, isSystem: false },
  { id: 7305, code: '7305', title: 'هزینهی مطالبات مشکوکالوصول و سوختشده', level: 3, parentId: 73, nature: 'debit', description: 'هزینه قطعی مطالبات لاوصول تجاری سوخت شده', expanded: false, isSystem: false },
  { id: 7306, code: '7306', title: 'پاداش هیئتمدیره', level: 3, parentId: 73, nature: 'debit', description: 'پاداش سالیانه مصوب مجمع هیئت مدیره شرکت', expanded: false, isSystem: false },

  // Ledger 74
  { id: 74, code: '74', title: 'سایر اقلام عملیاتی', level: 2, parentId: 7, nature: 'both', description: 'سایر سود و زیان‌های متفرقه عملیاتی و تسعیر ارز جاری', expanded: false, isSystem: false },
  { id: 7402, code: '7402', title: 'سود و زیان ناشی از تسعیر داراییها و بدهیهای ارزی عملیاتی', level: 3, parentId: 74, nature: 'both', description: 'تفاوت نرخ تسعیر دارایی و بدهی‌های ارزی در جریان تجارت', expanded: false, isSystem: false },
  { id: 7403, code: '7403', title: 'درآمد اجاره', level: 3, parentId: 74, nature: 'credit', description: 'درآمدهای فرعی حاصل از اجاره دادن املاک دفتری مازاد', expanded: false, isSystem: false },
  { id: 7404, code: '7404', title: 'زیان کاهش ارزش موجودیها', level: 3, parentId: 74, nature: 'debit', description: 'زیان افت قیمت بازاری کالاها زیر بهای تمام شده', expanded: false, isSystem: false },

  // Ledger 75
  { id: 75, code: '75', title: 'هزینه‌های مالی', level: 2, parentId: 7, nature: 'debit', description: 'کارمزدها و سودهای پرداختی بابت تسهیلات بانکی و ضمانت‌ها', expanded: false, isSystem: false },
  { id: 7501, code: '7501', title: 'سود تضمینشده و کارمزد تسهیلات دریافتی', level: 3, parentId: 75, nature: 'debit', description: 'سود بانکی اقساط وام‌های دریافتی', expanded: false, isSystem: false },
  { id: 7502, code: '7502', title: 'هزینهی خرید سفته', level: 3, parentId: 75, nature: 'debit', description: 'مخارج خرید سفته‌های تضمین بانکی شرکت', expanded: false, isSystem: false },
  { id: 7503, code: '7503', title: 'هزینهی خدمات حسابداری و حسابرسی', level: 3, parentId: 75, nature: 'debit', description: 'هزینه‌های حسابرسی مستقل و خدمات دوره‌ای مالی', expanded: false, isSystem: false },
  { id: 7504, code: '7504', title: 'کارمزد صدور ضمانتنامههای بانکی', level: 3, parentId: 75, nature: 'debit', description: 'مخارج تمدید و صدور ضمانت‌های بانکی مناقصات', expanded: false, isSystem: false },
  { id: 7505, code: '7505', title: 'هزینهی رهن املاک', level: 3, parentId: 75, nature: 'debit', description: 'حق‌الزحمه دفاتر ثبت اسناد بابت رهن املاک تضمینی وام', expanded: false, isSystem: false },

  // Ledger 76
  { id: 76, code: '76', title: 'سایر درآمدها و هزینههای غیرعملیاتی', level: 2, parentId: 7, nature: 'both', description: 'سود و زیان متفرقه خارج از فعالیت اصلی شرکت', expanded: false, isSystem: false },
  { id: 7601, code: '7601', title: 'سود و زیان ناشی از فروش داراییهای ثابت مشهود و نامشهود', level: 3, parentId: 76, nature: 'both', description: 'مابه التفاوت ارزش دفتری و ارزش فروش وسایل نقلیه یا رایانه‌ها', expanded: false, isSystem: false },
  { id: 7602, code: '7602', title: 'سود سپردههای بانکی و سایر اوراق بهادار', level: 3, parentId: 76, nature: 'credit', description: 'سود‌های ماهیانه واریزی به حساب‌های سپرده ریالی', expanded: false, isSystem: false },
  { id: 7604, code: '7604', title: 'سود سهام', level: 3, parentId: 76, nature: 'credit', description: 'سود تقسیمی مأخوذه از سهام بلندمدت سایر شرکت‌ها', expanded: false, isSystem: false },
  { id: 7605, code: '7605', title: 'زیان کاهش ارزش سرمایهگذاریهای بلندمدت', level: 3, parentId: 76, nature: 'debit', description: 'هزینه ذخیره افت ارزش سهام‌های بورسی غیرجاری', expanded: false, isSystem: false },
  { id: 7606, code: '7606', title: 'سود ناشی از فروش سرمایهگذاریها', level: 3, parentId: 76, nature: 'credit', description: 'سود قطعی واگذاری سهام بورسی ملکی', expanded: false, isSystem: false },
  { id: 7607, code: '7607', title: 'درآمد و هزینهی ناشی از ارزیابی سرمایهگذاریهای جاری سریعالمعامله به ارزش بازار', level: 3, parentId: 76, nature: 'both', description: 'تعدیلات ارزش بازار جاری سبد بورسی', expanded: false, isSystem: false },
  { id: 7608, code: '7608', title: 'سود و زیان تسعیر داراییها و بدهیهای ارزی غیرمرتبط با عملیات', level: 3, parentId: 76, nature: 'both', description: 'تفاوت تسعیر ارز وجوه بلوکه یا تسهیلات بلندمدت ارزی', expanded: false, isSystem: false },


  // ===== حساب‌های انتظامی (Group 9) =====
  { id: 9, code: '9', title: 'حساب‌های انتظامی و واسطه', level: 1, parentId: null, nature: 'both', description: 'ارقام تضمینی و تعهدات غیر مالی ترازنامه‌ای', expanded: false, isSystem: true },

  // Ledger 91
  { id: 91, code: '91', title: 'حساب‌های انتظامی تعهدات', level: 2, parentId: 9, nature: 'both', description: 'اسناد ضمانتی تودیعی و مأخوذه', expanded: false, isSystem: false },
  { id: 9101, code: '9101', title: 'اسناد تضمینی ما نزد دیگران', level: 3, parentId: 91, nature: 'debit', description: 'چک‌های تضمین پیمان تسلیمی شرکت به کارفرما', expanded: false, isSystem: false },
  { id: 9102, code: '9102', title: 'سفتههای تضمینی ما نزد دیگران', level: 3, parentId: 91, nature: 'debit', description: 'سفته‌های ضمانتی تودیع شده شرکت نزد بانک‌ها یا موسسات', expanded: false, isSystem: false },
  { id: 9103, code: '9103', title: 'ضمانتنامههای بانکی ما نزد دیگران', level: 3, parentId: 91, nature: 'debit', description: 'ضمانتنامه‌های رسمی صادره بانکی شرکت در وجه کارفرمایان', expanded: false, isSystem: false },
  { id: 9104, code: '9104', title: 'اسناد تضمینی دیگران نزد ما', level: 3, parentId: 91, nature: 'debit', description: 'چک‌ها و سفته‌های دریافتی از پرسنل یا شرکت‌های همکار جهت تعهد', expanded: false, isSystem: false },

  // Ledger 92
  { id: 92, code: '92', title: 'طرف حساب‌های انتظامی', level: 2, parentId: 9, nature: 'both', description: 'حساب‌های متقابل و موازنه انتظامی', expanded: false, isSystem: false },
  { id: 9201, code: '9201', title: 'طرف اسناد تضمینی ما نزد دیگران', level: 3, parentId: 92, nature: 'credit', description: 'حساب معکوس اسناد تضمینی تودیعی ما', expanded: false, isSystem: false },
  { id: 9202, code: '9202', title: 'طرف سفتههای تضمینی ما نزد دیگران', level: 3, parentId: 92, nature: 'credit', description: 'حساب معکوس سفته‌های تضمینی تودیعی ما', expanded: false, isSystem: false },
  { id: 9203, code: '9203', title: 'طرف ضمانتنامههای بانکی ما نزد دیگران', level: 3, parentId: 92, nature: 'credit', description: 'حساب معکوس ضمانتنامه‌های بانکی ما نزد بقیه', expanded: false, isSystem: false },
  { id: 9204, code: '9204', title: 'طرف اسناد تضمینی دیگران نزد ما', level: 3, parentId: 92, nature: 'credit', description: 'حساب معکوس اسناد تعهد دیگران نزد شرکت ما', expanded: false, isSystem: false }
])

// --- Operational States ---
const search = ref('')
const activeSidebarFilter = ref('all') // 'all' or groupId
const activeLevelFilter = ref('all') // 'all', 1, 2, 3, 4
const dialogOpen = ref(false)
const isEditingMode = ref(false)
const toast = ref({ show: false, text: '', type: 'success' })
const suggestedCode = ref('')
const creationLevel = ref(null)

// Form State
const form = ref({
  id: null,
  parentId: null,
  title: '',
  code: '',
  level: 1,
  nature: 'debit',
  description: ''
})

// Options Constants
const natureOptions = [
  { id: 'debit', title: 'بدهکار (دارایی/هزینه)' },
  { id: 'credit', title: 'بستانکار (بدهی/سرمایه/درآمد)' },
  { id: 'both', title: 'دوگانه (بدهکار/بستانکار)' }
]

// --- Computed Properties ---

// Sorted flat list of accounts arranged by hierarchical code
const sortedAccounts = computed(() => {
  return [...accounts.value].sort((a, b) => a.code.localeCompare(b.code, 'en'))
})

// Level 1 Accounts (Groups) for the Sidebar quick filter
const groupAccounts = computed(() => {
  return sortedAccounts.value.filter(a => a.level === 1)
})

// Parent dropdown options in form (Levels 1, 2, and 3 are allowed as parents)
const parentOptions = computed(() => {
  return sortedAccounts.value
    .filter(a => {
      if (a.id === form.value.id) return false
      if (creationLevel.value !== null) {
        return a.level === creationLevel.value - 1
      }
      return a.level < 4
    })
    .map(a => ({
      id: a.id,
      title: `${a.code} - ${a.title} (${getLevelLabel(a.level)})`
    }))
})

// High-fidelity search and filter logic
const filteredAccounts = computed(() => {
  return sortedAccounts.value.filter(account => {
    // 0. Level filter check
    if (activeLevelFilter.value !== 'all' && account.level !== activeLevelFilter.value) {
      return false
    }

    // 1. Sidebar filter check
    if (activeSidebarFilter.value !== 'all') {
      let isChildOfActiveFilter = false
      let currentParentId = account.id
      while (currentParentId !== null) {
        if (currentParentId === activeSidebarFilter.value) {
          isChildOfActiveFilter = true
          break
        }
        const parentNode = accounts.value.find(a => a.id === currentParentId)
        currentParentId = parentNode ? parentNode.parentId : null
      }
      if (!isChildOfActiveFilter) return false
    }

    // 2. Search box query check
    if (search.value && search.value.trim() !== '') {
      return matchesSearch(account)
    }

    // 3. Tree collapse status check (only apply if viewing all levels)
    if (activeLevelFilter.value === 'all') {
      let parentId = account.parentId
      while (parentId !== null) {
        const parent = accounts.value.find(a => a.id === parentId)
        if (!parent || !parent.expanded) return false
        parentId = parent.parentId
      }
    }

    return true
  })
})

// --- Methods ---

function selectSidebarFilter(filterVal) {
  activeSidebarFilter.value = filterVal
  // Auto expand all when filtering specifically to avoid blank grids
  if (filterVal !== 'all') {
    expandRecursive(filterVal, true)
  }
}

// Sidebar color markers based on account code ranges
function getSidebarDotColor(code) {
  if (code === '1') return 'var(--cyan)'
  if (code === '2') return '#3b82f6'
  if (code === '3') return 'var(--purple)'
  if (code === '4') return '#ec4899'
  if (code === '5') return 'var(--amber)'
  if (code === '6') return '#10b981'
  if (code === '7') return 'var(--red)'
  return '#94a3b8'
}

function getLevelLabel(level) {
  if (level === 1) return 'گروه حساب'
  if (level === 2) return 'حساب کل'
  if (level === 3) return 'حساب معین'
  if (level === 4) return 'حساب تفصیلی'
  return 'نامشخص'
}

function getNatureLabel(nature) {
  if (nature === 'debit') return 'بدهکار'
  if (nature === 'credit') return 'بستانکار'
  return 'دوگانه'
}

function getRowClass(act) {
  if (act.level === 1) return 'row-level-1'
  if (act.level === 2) return 'row-level-2'
  if (act.level === 3) return 'row-level-3'
  return 'row-level-4'
}

function hasChildren(id) {
  return accounts.value.some(a => a.parentId === id)
}

function getChildCountRecursive(id) {
  let count = 0
  const findChildren = (pid) => {
    const children = accounts.value.filter(a => a.parentId === pid)
    count += children.length
    children.forEach(c => findChildren(c.id))
  }
  findChildren(id)
  return count
}

function toggleExpand(act) {
  act.expanded = !act.expanded
}

function expandAll(state) {
  accounts.value.forEach(a => {
    if (hasChildren(a.id)) {
      a.expanded = state
    }
  })
}

function expandRecursive(id, state) {
  const node = accounts.value.find(a => a.id === id)
  if (node) node.expanded = state
  const children = accounts.value.filter(a => a.parentId === id)
  children.forEach(c => {
    if (hasChildren(c.id)) {
      c.expanded = state
    }
    expandRecursive(c.id, state)
  })
}

// Precise tree search algorithm
function matchesSearch(account) {
  const query = search.value.trim().toLowerCase()
  if (!query) return true

  // Direct match
  const selfMatch = account.code.toLowerCase().includes(query) || account.title.toLowerCase().includes(query)
  if (selfMatch) return true

  // Children match (expand parent to reveal matching children)
  const hasDescendantMatch = (node) => {
    const children = accounts.value.filter(a => a.parentId === node.id)
    for (const child of children) {
      if (child.code.toLowerCase().includes(query) || child.title.toLowerCase().includes(query) || hasDescendantMatch(child)) {
        return true
      }
    }
    return false
  }

  // Ancestor match (show path to descendants)
  const isDescendantOfMatch = (node) => {
    let parentId = node.parentId
    while (parentId !== null) {
      const parent = accounts.value.find(a => a.id === parentId)
      if (parent && (parent.code.toLowerCase().includes(query) || parent.title.toLowerCase().includes(query))) {
        return true
      }
      parentId = parent ? parent.parentId : null
    }
    return false
  }

  return hasDescendantMatch(account) || isDescendantOfMatch(account)
}

function isSearchedAndMatched(act) {
  if (!search.value) return false
  const query = search.value.trim().toLowerCase()
  return act.code.toLowerCase().includes(query) || act.title.toLowerCase().includes(query)
}

// --- CRUD Operations ---

function openCreateDialog(directParentNode = null) {
  isEditingMode.value = false
  const targetLevel = directParentNode ? directParentNode.level + 1 : 1
  creationLevel.value = targetLevel
  form.value = {
    id: null,
    parentId: directParentNode ? directParentNode.id : null,
    title: '',
    code: '',
    level: targetLevel,
    nature: directParentNode ? directParentNode.nature : 'debit',
    description: ''
  }
  calculateAutomaticCoding()
  dialogOpen.value = true
}

function openCreateDialogForLevel(level) {
  isEditingMode.value = false
  creationLevel.value = level
  form.value = {
    id: null,
    parentId: null,
    title: '',
    code: '',
    level: level,
    nature: 'debit',
    description: ''
  }
  
  // Set first available parent option if any, or null
  const options = parentOptions.value
  if (options && options.length > 0) {
    form.value.parentId = options[0].id
    onParentChange(options[0].id)
  } else {
    onParentChange(null)
  }
  
  dialogOpen.value = true
}

function openEditDialog(act) {
  isEditingMode.value = true
  creationLevel.value = null
  form.value = {
    id: act.id,
    parentId: act.parentId,
    title: act.title,
    code: act.code,
    level: act.level,
    nature: act.nature,
    description: act.description
  }
  suggestedCode.value = ''
  dialogOpen.value = true
}

function onParentChange(newParentId) {
  if (!newParentId) {
    form.value.level = 1
    form.value.nature = 'debit'
  } else {
    const parentNode = accounts.value.find(a => a.id === newParentId)
    if (parentNode) {
      form.value.level = parentNode.level + 1
      form.value.nature = parentNode.nature
    }
  }
  calculateAutomaticCoding()
}

// Intelligent Sequential Code Generator (Supporting standard 1-2-4-6 digit Iranian prefix rules)
function calculateAutomaticCoding() {
  const pId = form.value.parentId
  if (!pId) {
    // Level 1: Groups (1, 2, 3...)
    const level1s = accounts.value.filter(a => a.level === 1)
    if (level1s.length === 0) {
      suggestedCode.value = '1'
    } else {
      const codes = level1s.map(a => Number(a.code)).filter(c => !isNaN(c))
      const maxCode = Math.max(...codes)
      suggestedCode.value = String(maxCode + 1)
    }
  } else {
    const parentNode = accounts.value.find(a => a.id === pId)
    if (!parentNode) return

    const siblings = accounts.value.filter(a => a.parentId === pId)

    if (siblings.length === 0) {
      // First child
      if (parentNode.level === 1) {
        // Level 2 (Ledger کل) -> e.g. parent '1' -> suggests '11'
        suggestedCode.value = parentNode.code + '1'
      } else if (parentNode.level === 2) {
        // Level 3 (Subsidiary معین) -> e.g. parent '11' -> suggests '1101'
        suggestedCode.value = parentNode.code + '01'
      } else if (parentNode.level === 3) {
        // Level 4 (Detailed تفصیلی) -> e.g. parent '1101' -> suggests '1101-01'
        suggestedCode.value = parentNode.code + '-01'
      }
    } else {
      // Find sibling with largest code
      const siblingCodes = siblings.map(s => s.code).filter(c => c)
      siblingCodes.sort()
      const lastCode = siblingCodes[siblingCodes.length - 1]

      if (parentNode.level === 1) {
        // Sibling Level 2 -> e.g. last sibling '18' under parent '1' -> next suggests '19'
        const lastDigit = Number(lastCode.slice(1))
        suggestedCode.value = parentNode.code + String(lastDigit + 1)
      } else if (parentNode.level === 2) {
        // Sibling Level 3 -> e.g. last sibling '1103' under parent '11' -> next suggests '1104'
        const lastTwoDigits = Number(lastCode.slice(2))
        suggestedCode.value = parentNode.code + String(lastTwoDigits + 1).padStart(2, '0')
      } else if (parentNode.level === 3) {
        // Sibling Level 4 -> e.g. last sibling '1101-02' under parent '1101' -> next suggests '1101-03'
        const parts = lastCode.split('-')
        const seqStr = parts[parts.length - 1]
        const nextSeq = Number(seqStr) + 1
        suggestedCode.value = parts.slice(0, -1).join('-') + '-' + String(nextSeq).padStart(2, '0')
      }
    }
  }
  form.value.code = suggestedCode.value
}

function saveAccount() {
  if (!form.value.title || !form.value.code) return

  // Code duplication check
  const duplicate = accounts.value.find(a => a.code === form.value.code && a.id !== form.value.id)
  if (duplicate) {
    showToast('خطا: کد ساختاری وارد شده تکراری است!', 'error')
    return
  }

  if (isEditingMode.value) {
    // Edit existing
    const idx = accounts.value.findIndex(a => a.id === form.value.id)
    if (idx !== -1) {
      accounts.value[idx] = {
        ...accounts.value[idx],
        title: form.value.title,
        code: form.value.code,
        nature: form.value.nature,
        description: form.value.description
      }
      showToast('سرفصل حسابداری با موفقیت ویرایش گردید', 'success')
    }
  } else {
    // Add new
    const newId = Date.now() + Math.random()
    accounts.value.push({
      id: newId,
      code: form.value.code,
      title: form.value.title,
      level: form.value.level,
      parentId: form.value.parentId,
      nature: form.value.nature,
      description: form.value.description,
      expanded: true,
      isSystem: false
    })

    // Auto expand parent to show the newly added node
    if (form.value.parentId) {
      const parent = accounts.value.find(a => a.id === form.value.parentId)
      if (parent) parent.expanded = true
    }

    showToast('سرفصل جدید حسابداری با موفقیت به سیستم اضافه شد', 'success')
  }

  dialogOpen.value = false
}

function handleDeleteAccount(id) {
  const account = accounts.value.find(a => a.id === id)
  if (!account) return

  if (account.isSystem) {
    showToast('خطا: حساب‌های سیستمی و پایه قابل حذف نیستند!', 'error')
    return
  }

  // Check for child accounts
  if (hasChildren(id)) {
    showToast('خطا: ابتدا باید تمام زیرمجموعه‌ها (فرزندان) این حساب را حذف کنید!', 'error')
    return
  }

  if (confirm(`آیا از حذف سرفصل «${account.title}» مطمئن هستید؟`)) {
    accounts.value = accounts.value.filter(a => a.id !== id)
    showToast('سرفصل حسابداری با موفقیت حذف گردید', 'success')
  }
}

function printCodingTree() {
  window.print()
}

function showToast(text, type) {
  toast.value = { show: true, text, type }
  setTimeout(() => { toast.value.show = false }, 3000)
}
</script>

<style scoped>
/* ── Layout and General styling ── */
.page-root {
  padding: 30px 24px 60px;
  background: var(--bg);
  color: var(--text);
  min-height: 100%;
}

.font-fa {
  font-family: 'Vazirmatn', sans-serif !important;
  font-feature-settings: "ss01", "ss02";
}

/* ── Page Header ── */
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
}

.page-sub {
  font-size: 12px;
  color: #475569;
  margin: 4px 0 0;
  font-weight: 500;
}

.btn-print-coding {
  display: flex;
  align-items: center;
  gap: 8px;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.08);
  color: #94a3b8;
  padding: 10px 18px;
  border-radius: 12px;
  font-weight: 700;
  font-size: 12.5px;
  cursor: pointer;
  transition: all 0.25s;
}

.btn-print-coding:hover {
  background: rgba(255, 255, 255, 0.06);
  color: #f1f5f9;
}

.btn-add-account-top {
  display: flex;
  align-items: center;
  gap: 8px;
  background: linear-gradient(135deg, #0284c7, #0ea5e9);
  color: #ffffff;
  border: none;
  font-weight: 850;
  font-size: 12.5px;
  padding: 10px 22px;
  border-radius: 12px;
  cursor: pointer;
  box-shadow: 0 6px 20px rgba(14, 165, 233, 0.2);
  transition: all 0.25s;
}

.btn-add-account-top:hover {
  transform: translateY(-1.5px);
  box-shadow: 0 8px 25px rgba(14, 165, 233, 0.28);
  filter: brightness(1.05);
}

/* ── Two Column coding grid layout ── */
.coding-layout-grid {
  display: grid;
  grid-template-columns: 280px 1fr;
  gap: 20px;
  align-items: start;
}

@media (max-width: 950px) {
  .coding-layout-grid {
    grid-template-columns: 1fr;
  }
}

/* ── Glass Card base ── */
.glass-card {
  background: rgba(13, 17, 23, 0.45);
  backdrop-filter: blur(14px);
  -webkit-backdrop-filter: blur(14px);
  border: 1px solid rgba(255, 255, 255, 0.045);
  border-radius: 18px;
  padding: 22px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
}

/* ── Sidebar Filters ── */
.sidebar-title {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 11.5px;
  font-weight: 850;
  color: #475569;
  margin-bottom: 16px;
  text-transform: uppercase;
}

.sidebar-menu {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.sidebar-menu-item {
  display: flex;
  align-items: center;
  width: 100%;
  padding: 10px 14px;
  border-radius: 10px;
  border: 1px solid transparent;
  background: transparent;
  color: #94a3b8;
  font-size: 12.5px;
  font-weight: 700;
  cursor: pointer;
  text-align: right;
  transition: all 0.25s;
}

.sidebar-menu-item:hover {
  background: rgba(255, 255, 255, 0.02);
  color: #f1f5f9;
}

.active-menu-item {
  background: rgba(56, 189, 248, 0.06) !important;
  border-color: rgba(56, 189, 248, 0.15);
  color: var(--cyan) !important;
}

.menu-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  margin-left: 10px;
  flex-shrink: 0;
}

.all-dot {
  background-color: #f1f5f9;
  box-shadow: 0 0 6px #f1f5f9;
}

.menu-text {
  flex: 1;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.menu-count {
  font-size: 10px;
  background: rgba(255, 255, 255, 0.03);
  padding: 1px 6px;
  border-radius: 5px;
  color: #475569;
}

.active-menu-item .menu-count {
  background: rgba(56, 189, 248, 0.12);
  color: var(--cyan);
}

/* Sidebar Info Widget */
.coding-info-widget {
  border-top: 1px solid rgba(255, 255, 255, 0.04);
  padding-top: 16px;
}

.widget-title {
  font-size: 11px;
  font-weight: 850;
  color: #334155;
  margin-bottom: 12px;
}

.widget-levels {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.widget-level-row {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 11px;
  color: #475569;
}

.lvl-badge-sm {
  width: 16px;
  height: 16px;
  border-radius: 4px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 9px;
  font-weight: 900;
  color: #0f172a;
}

/* ── Main Tree Grid area ── */
.table-card {
  background: rgba(13, 17, 23, 0.45);
}

.table-toolbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  flex-wrap: wrap;
}

.search-input-wrap {
  width: 320px;
}

@media (max-width: 600px) {
  .search-input-wrap {
    width: 100%;
  }
}

.toolbar-actions {
  display: flex;
  align-items: center;
  gap: 8px;
}

.btn-expand-all {
  display: flex;
  align-items: center;
  gap: 6px;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.07);
  color: #94a3b8;
  font-size: 11.5px;
  font-weight: 700;
  padding: 6px 12px;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-expand-all:hover {
  background: rgba(255, 255, 255, 0.05);
  color: #f1f5f9;
}

/* Search Box Customization */
:deep(.dark-field-search .v-field) {
  background: rgba(0, 0, 0, 0.4) !important;
  font-family: var(--font) !important;
  font-size: 13px !important;
  border-radius: 10px !important;
}
:deep(.dark-field-search .v-field__outline__start),
:deep(.dark-field-search .v-field__outline__end),
:deep(.dark-field-search .v-field__outline__notch) {
  border-color: rgba(255, 255, 255, 0.05) !important;
}

/* ── Tree Table Structure ── */
.table-scroll-wrap {
  overflow-x: auto;
  border-radius: 12px;
  border: 1px solid rgba(255, 255, 255, 0.03);
}

.coding-tree-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
  text-align: right;
}

.coding-tree-table thead tr {
  background: rgba(7, 10, 16, 0.85);
}

.coding-tree-table th {
  padding: 12px 16px;
  font-size: 11px;
  font-weight: 850;
  color: #475569;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  white-space: nowrap;
}

.th-code { width: 220px; }
.th-level { width: 110px; }
.th-nature { width: 100px; }
.th-actions { width: 120px; text-align: center; }

.coding-tree-table td {
  padding: 10px 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.02);
  vertical-align: middle;
  position: relative;
}

/* Tree Connecting Lines */
.tree-row {
  position: relative;
  transition: all 0.25s;
}

.tree-row:hover {
  background: rgba(255, 255, 255, 0.012) !important;
}

.tree-connector {
  position: absolute;
  top: 0;
  right: calc(var(--indent-level) * 22px - 14px);
  width: 12px;
  height: 50%;
  border-bottom: 2px dashed rgba(255, 255, 255, 0.05);
  border-right: 2px dashed rgba(255, 255, 255, 0.05);
  border-bottom-right-radius: 5px;
  pointer-events: none;
}

/* Row Visual levels */
.row-level-1 { background: rgba(56, 189, 248, 0.015); }
.row-level-1 td { border-bottom: 1px solid rgba(56, 189, 248, 0.06); }
.row-level-2 { background: rgba(255, 255, 255, 0.003); }

/* Highlight matching search nodes */
.highlight-match {
  background: rgba(56, 189, 248, 0.06) !important;
}

.highlight-match .account-title {
  color: var(--cyan) !important;
  text-shadow: 0 0 6px rgba(56, 189, 248, 0.2);
}

/* Code layout cell */
.code-badge {
  display: inline-flex;
}

.code-text {
  font-weight: 800;
  letter-spacing: 0.5px;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.04);
  padding: 2px 8px;
  border-radius: 6px;
  white-space: nowrap;
}

.lvl-color-1 { color: var(--cyan); border-color: rgba(56, 189, 248, 0.15); background: rgba(56, 189, 248, 0.05); }
.lvl-color-2 { color: var(--purple); border-color: rgba(167, 139, 250, 0.15); background: rgba(167, 139, 250, 0.05); }
.lvl-color-3 { color: var(--amber); border-color: rgba(251, 191, 36, 0.15); background: rgba(251, 191, 36, 0.05); }
.lvl-color-4 { color: var(--green); border-color: rgba(52, 211, 153, 0.15); background: rgba(52, 211, 153, 0.05); }

/* Title cell layout with collapses */
.title-cell-wrap {
  display: flex;
  align-items: center;
}

.btn-toggle-expand {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 18px;
  height: 18px;
  border-radius: 50%;
  border: none;
  background: rgba(255, 255, 255, 0.03);
  color: #475569;
  cursor: pointer;
  margin-left: 8px;
  transition: all 0.2s;
}

.btn-toggle-expand:hover {
  background: rgba(255, 255, 255, 0.08);
  color: #f1f5f9;
}

.expanded-rotated {
  transform: rotate(-90deg);
  color: var(--cyan) !important;
}

.clickable-row {
  cursor: pointer;
}
.clickable-row:hover {
  background: rgba(255, 255, 255, 0.02);
}

.bullet-spacer {
  width: 18px;
  display: inline-block;
  margin-left: 8px;
}

.node-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  color: #334155;
  margin-left: 8px;
}

.row-level-1 .node-icon { color: var(--cyan); }
.row-level-2 .node-icon { color: var(--purple); }
.row-level-3 .node-icon { color: var(--amber); }
.row-level-4 .node-icon { color: var(--green); }

.account-title {
  color: #94a3b8;
  font-weight: 600;
  transition: color 0.2s;
}

.tree-row:hover .account-title {
  color: #f1f5f9;
}

/* Chips and badges */
.lvl-badge {
  font-size: 9.5px;
  font-weight: 850;
  padding: 3px 8px;
  border-radius: 5px;
  white-space: nowrap;
}

.lvl-1 { background: rgba(56, 189, 248, 0.07); color: var(--cyan); border: 1px solid rgba(56, 189, 248, 0.15); }
.lvl-2 { background: rgba(167, 139, 250, 0.07); color: var(--purple); border: 1px solid rgba(167, 139, 250, 0.15); }
.lvl-3 { background: rgba(251, 191, 36, 0.07); color: var(--amber); border: 1px solid rgba(251, 191, 36, 0.15); }
.lvl-4 { background: rgba(52, 211, 153, 0.07); color: var(--green); border: 1px solid rgba(52, 211, 153, 0.15); }

.nature-badge {
  font-size: 10px;
  font-weight: 800;
  padding: 2px 8px;
  border-radius: 99px;
  white-space: nowrap;
}

.nature-debit { background: rgba(52, 211, 153, 0.05); color: var(--green); }
.nature-credit { background: rgba(248, 113, 113, 0.05); color: var(--red); }
.nature-both { background: rgba(96, 165, 250, 0.05); color: var(--blue); }

/* Row Action items */
.row-actions {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 4px;
}

.btn-node-act {
  width: 25px;
  height: 25px;
  border-radius: 6px;
  border: 1px solid transparent;
  background: transparent;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s;
}

.act-add-child { color: var(--cyan); }
.act-add-child:hover { background: rgba(56, 189, 248, 0.08); border-color: rgba(56, 189, 248, 0.18); }

.act-edit { color: #60a5fa; }
.act-edit:hover { background: rgba(96, 165, 250, 0.08); border-color: rgba(96, 165, 250, 0.18); }

.act-del { color: #f87171; }
.act-del:hover:not(:disabled) { background: rgba(248, 113, 113, 0.08); border-color: rgba(248, 113, 113, 0.18); }
.act-del:disabled { opacity: 0.2; cursor: not-allowed; }

/* Empty Table state */
.empty-state-table {
  text-align: center;
  padding: 30px;
  color: #334155;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  font-size: 12px;
}

/* ── 3. VDialog for creation ── */
.account-dialog-wrapper {
  background: #111827;
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 18px;
  padding: 0 !important;
  overflow: hidden;
}

.dialog-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 20px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  background: #0b0f17;
}

.dialog-title {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13.5px;
  font-weight: 850;
  color: #f1f5f9;
}

.btn-close-dialog {
  background: transparent;
  border: none;
  color: #475569;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: color 0.2s;
}

.btn-close-dialog:hover { color: #f1f5f9; }

.dialog-body {
  padding: 0 20px;
}

.dialog-fields-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
}

.static-badge-wrap {
  height: 38px;
  display: flex;
  align-items: center;
}

.lvl-dialog {
  padding: 6px 14px;
  border-radius: 8px;
  font-size: 11px;
}

.code-generation-tip {
  font-size: 9.5px;
  color: var(--cyan);
  margin-top: 6px;
}

.code-generation-tip strong {
  font-weight: 900;
  text-decoration: underline;
}

/* Premium VSelect Input overrides */
:deep(.dark-field .v-field) {
  background: rgba(0, 0, 0, 0.4) !important;
  font-family: var(--font) !important;
  font-size: 13px !important;
  transition: all 0.25s !important;
}

:deep(.dark-field .v-field__outline__start),
:deep(.dark-field .v-field__outline__end),
:deep(.dark-field .v-field__outline__notch) {
  border-color: rgba(255, 255, 255, 0.05) !important;
}

:deep(.dark-field .v-field--focused .v-field__outline__start),
:deep(.dark-field .v-field--focused .v-field__outline__end),
:deep(.dark-field .v-field--focused .v-field__outline__notch) {
  border-color: rgba(56, 189, 248, 0.5) !important;
}

:deep(.dark-field input),
:deep(.dark-field .v-select__selection-text) {
  color: #f1f5f9 !important;
  font-family: var(--font) !important;
}

.dialog-footer {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 10px;
  padding: 16px 20px;
  border-top: 1px solid rgba(255, 255, 255, 0.05);
  background: #0b0f17;
  margin-top: 20px;
}

.btn-dialog-cancel {
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.08);
  color: #94a3b8;
  font-weight: 700;
  font-size: 12.5px;
  padding: 8px 18px;
  border-radius: 10px;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-dialog-cancel:hover { background: rgba(255, 255, 255, 0.06); color: #f1f5f9; }

.btn-dialog-submit {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: linear-gradient(135deg, #0284c7, #0ea5e9);
  color: #ffffff;
  border: none;
  font-weight: 850;
  font-size: 12.5px;
  padding: 8px 20px;
  border-radius: 10px;
  cursor: pointer;
  box-shadow: 0 4px 12px rgba(14, 165, 233, 0.15);
  transition: all 0.25s;
}

.btn-dialog-submit:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 6px 18px rgba(14, 165, 233, 0.25);
}

.btn-dialog-submit:disabled {
  background: #1e293b;
  color: #475569;
  box-shadow: none;
  cursor: not-allowed;
  opacity: 0.6;
}

/* ── 4. Printable Coding Tree View Area (Print-only) ── */
.print-coding-container {
  display: none;
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

.print-coding-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 12px;
}

.print-coding-table th {
  background: #f1f5f9;
  border: 1px solid #cbd5e1;
  padding: 8px;
  font-weight: 800;
  color: #0f172a;
}

.print-coding-table td {
  border: 1px solid #cbd5e1;
  padding: 8px;
  color: #334155;
}

.print-lvl-row-1 td { background: #e2e8f0; font-weight: bold; }
.print-lvl-row-2 td { background: #f8fafc; }

/* ── 5. Premium Toast Alert ── */
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

.toast-timer-bar {
  height: 3px;
  width: 100%;
  background: currentColor;
  opacity: 0.6;
  align-self: flex-start;
  animation: timer-shrink 3000ms linear forwards;
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

/* ── Print Media Query ── */
@media print {
  body {
    background: #ffffff !important;
    color: #000000 !important;
  }
  .page-root,
  .no-print,
  header,
  section,
  div.coding-layout-grid,
  .v-overlay-container,
  .v-dialog {
    display: none !important;
    visibility: hidden !important;
  }

  .print-coding-container {
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
