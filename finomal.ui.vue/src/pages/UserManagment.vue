<template>
  <div class="page-root" dir="rtl">

    <!-- ══════════════════════════════════════ HEADER ══════════════════════════════════════ -->
    <header class="page-header">
      <div class="header-brand">
        <div class="brand-icon">
          <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
            <path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2" />
            <circle cx="9" cy="7" r="4" />
            <path d="M23 21v-2a4 4 0 00-3-3.87M16 3.13a4 4 0 010 7.75" />
          </svg>
        </div>
        <div>
          <h1 class="page-title">مدیریت کاربران</h1>
          <p class="page-sub">سیستم فینومال · {{ usersList.length }} کاربر ثبت‌شده</p>
        </div>
      </div>

      <div class="header-left">
        <!-- وضعیت اتصال -->
        <div class="conn-badge" :class="connBadgeClass">
          <span class="conn-dot"></span>
          {{ connBadgeLabel }}
        </div>
        <button class="btn-primary" @click="startAddNewUser" :disabled="!isConnected">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <line x1="12" y1="5" x2="12" y2="19" />
            <line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          کاربر جدید
        </button>
      </div>
    </header>

    <!-- ══════════════════════════════════════ ERROR BARS ══════════════════════════════════════ -->

    <!-- خطای اتصال -->
    <transition name="bar">
      <div v-if="connectionError" class="error-bar error-bar-conn">
        <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <path d="M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0z" />
          <line x1="12" y1="9" x2="12" y2="13" />
          <line x1="12" y1="17" x2="12.01" y2="17" />
        </svg>
        <span>{{ connectionError }}</span>
        <button class="btn-retry" @click="retryConnect" :disabled="isConnecting">
          <span v-if="isConnecting" class="btn-spinner btn-spinner-sm"></span>
          {{ isConnecting ? 'در حال اتصال...' : 'تلاش مجدد' }}
        </button>
      </div>
    </transition>

    <!-- خطای عملیات (خارج از فرم) -->
    <transition name="bar">
      <div v-if="operationError && !showUserForm" class="error-bar error-bar-op">
        <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <circle cx="12" cy="12" r="10" />
          <line x1="12" y1="8" x2="12" y2="12" />
          <line x1="12" y1="16" x2="12.01" y2="16" />
        </svg>
        <span>{{ operationError }}</span>
        <button class="bar-dismiss" @click="clearOperationError">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <line x1="18" y1="6" x2="6" y2="18" />
            <line x1="6" y1="6" x2="18" y2="18" />
          </svg>
        </button>
      </div>
    </transition>

    <!-- ══════════════════════════════════════ STATS ══════════════════════════════════════ -->
    <div class="stats-row">
      <div class="stat-card">
        <div class="stat-icon stat-icon-green">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M22 11.08V12a10 10 0 11-5.93-9.14" />
            <polyline points="22 4 12 14.01 9 11.01" />
          </svg>
        </div>
        <div>
          <div class="stat-value">{{ activeCount }}</div>
          <div class="stat-label">کاربر فعال</div>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon stat-icon-red">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="12" cy="12" r="10" />
            <line x1="4.93" y1="4.93" x2="19.07" y2="19.07" />
          </svg>
        </div>
        <div>
          <div class="stat-value">{{ usersList.length - activeCount }}</div>
          <div class="stat-label">غیرفعال</div>
        </div>
      </div>
      <div class="stat-card">
        <div class="stat-icon stat-icon-orange">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
          </svg>
        </div>
        <div>
          <div class="stat-value">{{ adminCount }}</div>
          <div class="stat-label">مدیر سیستم</div>
        </div>
      </div>
    </div>

    <!-- Loading bar -->
    <div v-if="isConnecting" class="loading-bar">
      <div class="loading-fill"></div>
    </div>

    <!-- ══════════════════════════════════════ FORM DRAWER ══════════════════════════════════════ -->
    <transition name="drawer">
      <section v-if="showUserForm" class="form-drawer">

        <div class="drawer-header">
          <div class="drawer-title">
            <div class="drawer-title-icon">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                <path v-if="editingUserId" d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" />
                <path v-if="editingUserId" d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z" />
                <path v-else d="M16 21v-2a4 4 0 00-4-4H6a4 4 0 00-4 4v2" />
                <circle v-if="!editingUserId" cx="9" cy="7" r="4" />
                <line v-if="!editingUserId" x1="19" y1="8" x2="19" y2="14" />
                <line v-if="!editingUserId" x1="16" y1="11" x2="22" y2="11" />
              </svg>
            </div>
            {{ editingUserId ? 'ویرایش اطلاعات کاربر' : 'ثبت کاربر جدید' }}
          </div>
          <button class="btn-close" @click="cancelUserForm" :disabled="isSaving">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <line x1="18" y1="6" x2="6" y2="18" />
              <line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>

        <!-- خطای عملیات داخل فرم -->
        <transition name="bar">
          <div v-if="operationError && showUserForm" class="form-error-banner">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <circle cx="12" cy="12" r="10" />
              <line x1="12" y1="8" x2="12" y2="12" />
              <line x1="12" y1="16" x2="12.01" y2="16" />
            </svg>
            {{ operationError }}
          </div>
        </transition>

        <div class="form-grid">

          <!-- نام -->
          <div class="field-wrap" :class="{ 'field-error': formErrors.firstName || serverFieldErrors.firstName }">
            <label class="field-label">نام <span class="required">*</span></label>
            <div class="input-wrap">
              <svg class="input-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                stroke-width="2">
                <path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2" />
                <circle cx="12" cy="7" r="4" />
              </svg>
              <input v-model="form.firstName" class="field-input" placeholder="نام"
                @input="clearFieldError('firstName')" />
            </div>
            <span class="error-msg">{{ formErrors.firstName || serverFieldErrors.firstName }}</span>
          </div>

          <!-- نام خانوادگی -->
          <div class="field-wrap" :class="{ 'field-error': formErrors.lastName || serverFieldErrors.lastName }">
            <label class="field-label">نام خانوادگی <span class="required">*</span></label>
            <div class="input-wrap">
              <svg class="input-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                stroke-width="2">
                <path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2" />
                <circle cx="12" cy="7" r="4" />
              </svg>
              <input v-model="form.lastName" class="field-input" placeholder="نام خانوادگی"
                @input="clearFieldError('lastName')" />
            </div>
            <span class="error-msg">{{ formErrors.lastName || serverFieldErrors.lastName }}</span>
          </div>

          <!-- نام کاربری -->
          <div class="field-wrap" :class="{ 'field-error': formErrors.userName || serverFieldErrors.userName }">
            <label class="field-label">نام کاربری <span class="required">*</span></label>
            <div class="input-wrap">
              <svg class="input-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                stroke-width="2">
                <path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2" />
                <circle cx="12" cy="7" r="4" />
              </svg>
              <input v-model="form.userName" class="field-input" placeholder="username" :disabled="!!editingUserId"
                dir="ltr" @input="clearFieldError('userName')" />
            </div>
            <span class="error-msg">{{ formErrors.userName || serverFieldErrors.userName }}</span>
          </div>

          <!-- ایمیل -->
          <div class="field-wrap" :class="{ 'field-error': serverFieldErrors.email }">
            <label class="field-label">ایمیل</label>
            <div class="input-wrap">
              <svg class="input-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                stroke-width="2">
                <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z" />
                <polyline points="22,6 12,13 2,6" />
              </svg>
              <input v-model="form.email" type="email" class="field-input" placeholder="example@company.com" dir="ltr"
                @input="clearFieldError('email')" />
            </div>
            <span class="error-msg">{{ serverFieldErrors.email }}</span>
          </div>

          <!-- موبایل -->
          <div class="field-wrap" :class="{ 'field-error': serverFieldErrors.phoneNumber }">
            <label class="field-label">شماره موبایل</label>
            <div class="input-wrap">
              <svg class="input-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                stroke-width="2">
                <rect x="5" y="2" width="14" height="20" rx="2" ry="2" />
                <line x1="12" y1="18" x2="12.01" y2="18" />
              </svg>
              <input v-model="form.phoneNumber" class="field-input" placeholder="0912xxxxxxx" dir="ltr"
                @input="clearFieldError('phoneNumber')" />
            </div>
            <span class="error-msg">{{ serverFieldErrors.phoneNumber }}</span>
          </div>

          <!-- نقش -->
          <div class="field-wrap" :class="{ 'field-error': formErrors.roleId || serverFieldErrors.roleId }">
            <label class="field-label">نقش / سطح دسترسی <span class="required">*</span></label>
            <div class="input-wrap select-wrap">
              <svg class="input-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                stroke-width="2">
                <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
              </svg>
              <select v-model="form.roleId" class="field-input field-select" @change="clearFieldError('roleId')">
                <option value="" disabled>انتخاب نقش...</option>
                <option v-for="r in rolesList" :key="r.id" :value="r.id">
                  {{ r.displayName || r.name }}
                </option>
              </select>
              <svg class="select-arrow" width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                stroke-width="2.5">
                <polyline points="6 9 12 15 18 9" />
              </svg>
            </div>
            <span class="error-msg">{{ formErrors.roleId || serverFieldErrors.roleId }}</span>
          </div>

          <!-- رمز عبور (ایجاد) -->
          <div v-if="!editingUserId" class="field-wrap" :class="{ 'field-error': formErrors.password }">
            <label class="field-label">رمز عبور <span class="required">*</span></label>
            <div class="input-wrap">
              <svg class="input-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                stroke-width="2">
                <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                <path d="M7 11V7a5 5 0 0110 0v4" />
              </svg>
              <input v-model="form.password" :type="showPass ? 'text' : 'password'" class="field-input"
                placeholder="حداقل ۸ کاراکتر" dir="ltr" @input="clearFieldError('password')" />
              <button class="pass-toggle" type="button" @click="showPass = !showPass">
                <svg v-if="!showPass" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                  stroke-width="2">
                  <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
                  <circle cx="12" cy="12" r="3" />
                </svg>
                <svg v-else width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                  stroke-width="2">
                  <path
                    d="M17.94 17.94A10.07 10.07 0 0112 20c-7 0-11-8-11-8a18.45 18.45 0 015.06-5.94M9.9 4.24A9.12 9.12 0 0112 4c7 0 11 8 11 8a18.5 18.5 0 01-2.16 3.19" />
                  <line x1="1" y1="1" x2="23" y2="23" />
                </svg>
              </button>
            </div>
            <div v-if="form.password" class="pass-strength">
              <div class="strength-bar">
                <div class="strength-fill" :class="passwordStrength.class" :style="{ width: passwordStrength.pct }">
                </div>
              </div>
              <span class="strength-label" :class="passwordStrength.class">{{ passwordStrength.label }}</span>
            </div>
            <span class="error-msg">{{ formErrors.password }}</span>
          </div>

          <!-- ریست رمز (ویرایش) -->
          <div v-if="editingUserId" class="field-wrap">
            <label class="field-label">
              ریست رمز عبور
              <span class="field-optional">(اختیاری)</span>
            </label>
            <div class="input-wrap">
              <svg class="input-icon" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                stroke-width="2">
                <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                <path d="M7 11V7a5 5 0 0110 0v4" />
              </svg>
              <input v-model="form.newPassword" :type="showPass ? 'text' : 'password'" class="field-input"
                placeholder="خالی = بدون تغییر" dir="ltr" />
              <button class="pass-toggle" type="button" @click="showPass = !showPass">
                <svg v-if="!showPass" width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                  stroke-width="2">
                  <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
                  <circle cx="12" cy="12" r="3" />
                </svg>
                <svg v-else width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor"
                  stroke-width="2">
                  <path
                    d="M17.94 17.94A10.07 10.07 0 0112 20c-7 0-11-8-11-8a18.45 18.45 0 015.06-5.94M9.9 4.24A9.12 9.12 0 0112 4c7 0 11 8 11 8a18.5 18.5 0 01-2.16 3.19" />
                  <line x1="1" y1="1" x2="23" y2="23" />
                </svg>
              </button>
            </div>
          </div>

        </div>

        <div class="form-actions">
          <button class="btn-ghost" @click="cancelUserForm" :disabled="isSaving">انصراف</button>
          <button class="btn-primary" @click="saveUser" :disabled="isSaving">
            <span v-if="isSaving" class="btn-spinner"></span>
            <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <path d="M19 21H5a2 2 0 01-2-2V5a2 2 0 012-2h11l5 5v14z" />
              <polyline points="17 21 17 13 7 13 7 21" />
              <polyline points="7 3 7 8 15 8" />
            </svg>
            {{ isSaving ? 'در حال ذخیره...' : (editingUserId ? 'ذخیره تغییرات' : 'ثبت کاربر') }}
          </button>
        </div>

      </section>
    </transition>

    <!-- ══════════════════════════════════════ USER LIST ══════════════════════════════════════ -->
    <section class="list-card">

      <div class="list-toolbar">
        <div class="search-box">
          <svg class="search-icon" width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor"
            stroke-width="2">
            <circle cx="11" cy="11" r="8" />
            <line x1="21" y1="21" x2="16.65" y2="16.65" />
          </svg>
          <input v-model="searchQuery" class="search-input" placeholder="جستجو بر اساس نام، نام کاربری یا ایمیل..." />
          <button v-if="searchQuery" class="search-clear" @click="searchQuery = ''">
            <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <line x1="18" y1="6" x2="6" y2="18" />
              <line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>
        <div class="filter-row">
          <button v-for="f in computedRoleFilters" :key="f.value" class="filter-pill"
            :class="{ active: activeFilter === f.value }"
            @click="activeFilter = activeFilter === f.value && f.value !== 'all' ? 'all' : f.value">
            {{ f.label }}
          </button>
        </div>
      </div>

      <!-- Empty -->
      <div v-if="filteredUsers.length === 0 && !isConnecting" class="empty-state">
        <div class="empty-icon">
          <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.2">
            <circle cx="11" cy="11" r="8" />
            <line x1="21" y1="21" x2="16.65" y2="16.65" />
          </svg>
        </div>
        <div class="empty-title">{{ searchQuery ? 'نتیجه‌ای یافت نشد' : 'هنوز کاربری ثبت نشده' }}</div>
        <div class="empty-sub">{{ searchQuery ? `برای «${searchQuery}» موردی پیدا نشد` : 'با کلیک روی «کاربر جدید» شروع کنید' }}</div>
      </div>

      <!-- Table -->
      <div v-else class="table-wrap">
        <table class="user-table">
          <thead>
            <tr>
              <th class="th-idx">#</th>
              <th>کاربر</th>
              <th>ایمیل</th>
              <th>موبایل</th>
              <th>نقش</th>
              <th>وضعیت</th>
              <th class="th-act">عملیات</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(user, i) in filteredUsers" :key="user.id" class="user-row"
              :class="{ 'row-inactive': !user.isActive }">
              <td class="td-idx">{{ i + 1 }}</td>
              <td>
                <div class="user-cell">
                  <div class="user-avatar" :class="`avatar-${primaryRole(user)}`">
                    {{ getInitials(user.fullName || user.userName) }}
                  </div>
                  <div>
                    <div class="user-name">{{ user.fullName || '—' }}</div>
                    <div class="user-uname">@{{ user.userName }}</div>
                  </div>
                </div>
              </td>
              <td class="td-muted">{{ user.email || '—' }}</td>
              <td class="td-muted" dir="ltr">{{ user.phoneNumber || '—' }}</td>
              <td>
                <span class="role-pill" :class="`role-${primaryRole(user)}`">
                  {{ roleDisplayLabel(user) }}
                </span>
              </td>
              <td>
                <!-- وضعیت با اسپینر هنگام toggle -->
                <button class="status-toggle" :class="user.isActive ? 'st-active' : 'st-inactive'"
                  :disabled="!isConnected || togglingId === user.id" @click="onToggleStatus(user)"
                  :title="user.isActive ? 'کلیک برای غیرفعال کردن' : 'کلیک برای فعال کردن'">
                  <span v-if="togglingId === user.id" class="btn-spinner btn-spinner-sm"></span>
                  <span v-else class="st-dot"></span>
                  {{ user.isActive ? 'فعال' : 'غیرفعال' }}
                </button>
              </td>
              <td class="td-act">
                <div class="row-actions">
                  <button class="action-btn ab-edit" title="ویرایش" @click="startEditUser(user)"
                    :disabled="!isConnected">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" />
                      <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z" />
                    </svg>
                  </button>
                  <button class="action-btn ab-del" title="حذف" @click="confirmDelete(user)" :disabled="!isConnected">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <polyline points="3 6 5 6 21 6" />
                      <path d="M19 6l-1 14H6L5 6" />
                      <path d="M10 11v6M14 11v6" />
                    </svg>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-if="filteredUsers.length > 0" class="list-footer">
        نمایش {{ filteredUsers.length }} از {{ usersList.length }} کاربر
      </div>
    </section>

    <!-- ══════════════════════════════════════ DELETE CONFIRM ══════════════════════════════════════ -->
    <transition name="overlay">
      <div v-if="deleteTarget" class="modal-overlay" @click.self="deleteTarget = null">
        <div class="confirm-modal">
          <div class="confirm-icon">
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <polyline points="3 6 5 6 21 6" />
              <path d="M19 6l-1 14H6L5 6" />
              <path d="M10 11v6M14 11v6" />
            </svg>
          </div>
          <h3 class="confirm-title">حذف کاربر</h3>
          <p class="confirm-desc">
            آیا از حذف <strong>{{ deleteTarget.fullName || deleteTarget.userName }}</strong>
            اطمینان دارید؟ این عمل قابل بازگشت نیست.
          </p>

          <!-- خطای حذف داخل مودال -->
          <transition name="bar">
            <div v-if="deleteError" class="modal-error">
              <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <circle cx="12" cy="12" r="10" />
                <line x1="12" y1="8" x2="12" y2="12" />
                <line x1="12" y1="16" x2="12.01" y2="16" />
              </svg>
              {{ deleteError }}
            </div>
          </transition>

          <div class="confirm-actions">
            <button class="btn-ghost" @click="cancelDelete" :disabled="isDeleting">انصراف</button>
            <button class="btn-danger" @click="doDelete" :disabled="isDeleting">
              <span v-if="isDeleting" class="btn-spinner"></span>
              {{ isDeleting ? 'در حال حذف...' : 'بله، حذف کن' }}
            </button>
          </div>
        </div>
      </div>
    </transition>

    <!-- ══════════════════════════════════════ TOAST ══════════════════════════════════════ -->
    <transition name="toast">
      <div v-if="toast.show" class="toast" :class="`toast-${toast.type}`">
        <div class="toast-icon">
          <svg v-if="toast.type === 'success'" width="14" height="14" viewBox="0 0 24 24" fill="none"
            stroke="currentColor" stroke-width="2.5">
            <polyline points="20 6 9 17 4 12" />
          </svg>
          <svg v-else-if="toast.type === 'error'" width="14" height="14" viewBox="0 0 24 24" fill="none"
            stroke="currentColor" stroke-width="2.5">
            <line x1="18" y1="6" x2="6" y2="18" />
            <line x1="6" y1="6" x2="18" y2="18" />
          </svg>
          <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <circle cx="12" cy="12" r="10" />
            <line x1="12" y1="8" x2="12" y2="12" />
            <line x1="12" y1="16" x2="12.01" y2="16" />
          </svg>
        </div>
        {{ toast.text }}
      </div>
    </transition>

  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import { useUserManagementHub } from '@/composables/UseUserManagementHub'

// ══════════════════════════════════════════════════════════
//  Composable
// ══════════════════════════════════════════════════════════
const {
  isConnected,
  isConnecting,
  connectionError,
  usersList,
  rolesList,
  hubError,
  operationError,
  clearOperationError,
  connect,
  createUser,
  updateUser,
  toggleUserStatus,
  deleteUser,
  resetPassword,
} = useUserManagementHub()

// ══════════════════════════════════════════════════════════
//  UI State
// ══════════════════════════════════════════════════════════
const showUserForm = ref(false)
const editingUserId = ref(null)
const showPass = ref(false)
const isSaving = ref(false)
const isDeleting = ref(false)
const togglingId = ref(null)     // id کاربری که toggle در حال اجراست
const searchQuery = ref('')
const activeFilter = ref('all')
const deleteTarget = ref(null)
const deleteError = ref(null)     // خطای خاص مودال حذف
const formErrors = ref({})
const serverFieldErrors = ref({})       // خطاهای فیلد از سرور
const toast = ref({ show: false, text: '', type: 'success' })

const emptyForm = () => ({
  firstName: '', lastName: '', userName: '',
  email: '', phoneNumber: '', roleId: '',
  password: '', newPassword: '',
})
const form = ref(emptyForm())

// ══════════════════════════════════════════════════════════
//  Computed
// ══════════════════════════════════════════════════════════
const connBadgeClass = computed(() => {
  if (isConnected.value) return 'conn-ok'
  if (isConnecting.value) return 'conn-wait'
  return 'conn-off'
})
const connBadgeLabel = computed(() => {
  if (isConnected.value) return 'متصل'
  if (isConnecting.value) return 'در حال اتصال...'
  return 'قطع'
})

const activeCount = computed(() => usersList.value.filter(u => u.isActive).length)
const adminCount = computed(() =>
  usersList.value.filter(u => u.roles?.some(r => r.toLowerCase().includes('admin'))).length
)

const computedRoleFilters = computed(() => [
  { value: 'all', label: 'همه' },
  ...rolesList.value.map(r => ({
    value: r.name.toLowerCase(),
    label: r.displayName || r.name,
  }))
])

const filteredUsers = computed(() => {
  let list = usersList.value
  if (activeFilter.value !== 'all')
    list = list.filter(u => u.roles?.some(r => r.toLowerCase() === activeFilter.value))
  if (!searchQuery.value.trim()) return list
  const q = searchQuery.value.toLowerCase().trim()
  return list.filter(u =>
    (u.fullName || '').toLowerCase().includes(q) ||
    (u.userName || '').toLowerCase().includes(q) ||
    (u.email || '').toLowerCase().includes(q) ||
    (u.phoneNumber || '').includes(q)
  )
})

const passwordStrength = computed(() => {
  const p = form.value.password
  if (!p) return { class: '', label: '', pct: '0%' }
  let s = 0
  if (p.length >= 8) s++
  if (/[A-Z]/.test(p)) s++
  if (/[0-9]/.test(p)) s++
  if (/[^A-Za-z0-9]/.test(p)) s++
  return [
    { class: 'str-weak', label: 'ضعیف', pct: '25%' },
    { class: 'str-fair', label: 'متوسط', pct: '50%' },
    { class: 'str-good', label: 'خوب', pct: '75%' },
    { class: 'str-strong', label: 'قوی', pct: '100%' },
  ][Math.max(0, s - 1)]
})

// ══════════════════════════════════════════════════════════
//  Watchers
// ══════════════════════════════════════════════════════════

// hubError — خطای broadcast از سرور (مستقل از عملیات جاری)
watch(hubError, (msg) => {
  if (msg) showToast(msg, 'error', 7000)
})

// operationError — فقط وقتی فرم بسته است toast نمایش بده
// (وقتی فرم باز است، banner داخل فرم نمایش می‌دهد)
watch(operationError, (err) => {
  if (err && !showUserForm.value && !deleteTarget.value)
    showToast(err, 'error', 5000)
})

// ══════════════════════════════════════════════════════════
//  Helpers
// ══════════════════════════════════════════════════════════
const getInitials = (name) => {
  if (!name) return '?'
  const p = name.trim().split(' ')
  return p.length >= 2 ? p[0][0] + p[1][0] : name[0]
}

const primaryRole = (user) => user.roles?.[0]?.toLowerCase() || 'viewer'

const roleDisplayLabel = (user) => {
  const name = user.roles?.[0]
  if (!name) return '—'
  return rolesList.value.find(r => r.name.toLowerCase() === name.toLowerCase())
    ?.displayName || name
}

const roleIdFromUser = (user) => {
  const name = user.roles?.[0]
  return name
    ? (rolesList.value.find(r => r.name.toLowerCase() === name.toLowerCase())?.id ?? '')
    : ''
}

function showToast(text, type = 'success', duration = 4000) {
  toast.value = { show: true, text, type }
  setTimeout(() => { toast.value.show = false }, duration)
}

// ══════════════════════════════════════════════════════════
//  Form — خطا
// ══════════════════════════════════════════════════════════
function clearFieldError(field) {
  delete formErrors.value[field]
  delete serverFieldErrors.value[field]
}

function clearAllErrors() {
  formErrors.value = {}
  serverFieldErrors.value = {}
  clearOperationError()
}

/** تشخیص خطاهای فیلد از پیام سرور و نگاشت به فیلد مربوطه */
function mapServerErrorToField(msg) {
  if (!msg) return
  const lower = msg.toLowerCase()
  if (lower.includes('ایمیل') || lower.includes('email'))
    serverFieldErrors.value.email = msg
  if (lower.includes('نام کاربری') || lower.includes('username'))
    serverFieldErrors.value.userName = msg
  if (lower.includes('موبایل') || lower.includes('phone'))
    serverFieldErrors.value.phoneNumber = msg
  if (lower.includes('رمز') || lower.includes('password'))
    serverFieldErrors.value.password = msg
  if (lower.includes('نقش') || lower.includes('role'))
    serverFieldErrors.value.roleId = msg
}

function validateForm() {
  clearAllErrors()
  const e = {}
  if (!form.value.firstName?.trim()) e.firstName = 'نام الزامی است'
  if (!form.value.lastName?.trim()) e.lastName = 'نام خانوادگی الزامی است'
  if (!form.value.userName?.trim()) e.userName = 'نام کاربری الزامی است'
  if (!form.value.roleId) e.roleId = 'انتخاب نقش الزامی است'
  if (!editingUserId.value) {
    if (!form.value.password?.trim()) e.password = 'رمز عبور الزامی است'
    else if (form.value.password.length < 8) e.password = 'رمز عبور باید حداقل ۸ کاراکتر باشد'
  }
  formErrors.value = e
  return Object.keys(e).length === 0
}

// ══════════════════════════════════════════════════════════
//  Form — باز/بسته
// ══════════════════════════════════════════════════════════
function startAddNewUser() {
  form.value = emptyForm()
  editingUserId.value = null
  showPass.value = false
  clearAllErrors()
  showUserForm.value = true
}

function startEditUser(user) {
  const parts = (user.fullName || '').trim().split(' ')
  form.value = {
    firstName: parts[0] || '',
    lastName: parts.slice(1).join(' ') || '',
    userName: user.userName || '',
    email: user.email || '',
    phoneNumber: user.phoneNumber || '',
    roleId: roleIdFromUser(user),
    password: '',
    newPassword: '',
  }
  editingUserId.value = user.id
  showPass.value = false
  clearAllErrors()
  showUserForm.value = true
}

function cancelUserForm() {
  if (isSaving.value) return
  showUserForm.value = false
  editingUserId.value = null
  clearAllErrors()
}

// ══════════════════════════════════════════════════════════
//  Save
// ══════════════════════════════════════════════════════════
async function saveUser() {
  if (!validateForm()) {
    showToast('لطفاً موارد الزامی را پر کنید', 'warn', 3000)
    return
  }
  isSaving.value = true

  try {
    if (editingUserId.value) {
      // ── UpdateUser ──────────────────────────────────────────────────────────
      await updateUser({
        userId: editingUserId.value,
        firstName: form.value.firstName.trim(),
        lastName: form.value.lastName.trim(),
        email: form.value.email?.trim() || null,
        phoneNumber: form.value.phoneNumber?.trim() || null,
        roleId: form.value.roleId,
      })

      // ریست رمز اختیاری
      if (form.value.newPassword?.trim()) {
        await resetPassword({
          userId: editingUserId.value,
          newPassword: form.value.newPassword,
        })
        showToast('رمز عبور نیز تغییر یافت', 'success', 4000)
      }

      showToast('اطلاعات کاربر به‌روزرسانی شد', 'success')

    } else {
      // ── CreateUser ──────────────────────────────────────────────────────────
      await createUser({
        firstName: form.value.firstName.trim(),
        lastName: form.value.lastName.trim(),
        userName: form.value.userName.trim(),
        email: form.value.email?.trim() || null,
        phoneNumber: form.value.phoneNumber?.trim() || null,
        password: form.value.password,
        roleId: form.value.roleId,
      })
      showToast('کاربر جدید با موفقیت ایجاد شد', 'success')
    }

    cancelUserForm()

  } catch (err) {
    // خطاهای فیلدی از پیام سرور شناسایی و نگاشت می‌شوند
    mapServerErrorToField(operationError.value)
    // اگر خطای فیلدی شناسایی نشد، banner داخل فرم نمایش می‌دهد
  } finally {
    isSaving.value = false
  }
}

// ══════════════════════════════════════════════════════════
//  Delete
// ══════════════════════════════════════════════════════════
function confirmDelete(user) {
  deleteTarget.value = user
  deleteError.value = null
}

function cancelDelete() {
  if (isDeleting.value) return
  deleteTarget.value = null
  deleteError.value = null
}

async function doDelete() {
  if (!deleteTarget.value) return
  isDeleting.value = true
  deleteError.value = null

  try {
    await deleteUser(deleteTarget.value.id)
    showToast(`${deleteTarget.value.fullName || deleteTarget.value.userName} حذف شد`, 'info', 4000)
    deleteTarget.value = null
  } catch (err) {
    // خطا داخل مودال نمایش داده می‌شود — نه toast
    deleteError.value = operationError.value || 'خطا در حذف کاربر. لطفاً دوباره تلاش کنید.'
  } finally {
    isDeleting.value = false
  }
}

// ══════════════════════════════════════════════════════════
//  Toggle Status
// ══════════════════════════════════════════════════════════
async function onToggleStatus(user) {
  if (togglingId.value) return
  togglingId.value = user.id

  try {
    await toggleUserStatus(user.id, !user.isActive)
    // hub event مستقیماً لیست را آپدیت می‌کند — toast اختیاری
    showToast(`کاربر ${!user.isActive ? 'فعال' : 'غیرفعال'} شد`, 'info', 3000)
  } catch {
    showToast(operationError.value || 'خطا در تغییر وضعیت کاربر', 'error')
  } finally {
    togglingId.value = null
  }
}

// ══════════════════════════════════════════════════════════
//  Reconnect
// ══════════════════════════════════════════════════════════
async function retryConnect() {
  try {
    await connect(() => sessionStorage.getItem('jwt') ?? '')
    showToast('اتصال مجدد با موفقیت انجام شد', 'success')
  } catch {
    showToast(connectionError.value || 'اتصال مجدد ناموفق بود', 'error', 6000)
  }
}

// ══════════════════════════════════════════════════════════
//  Init
// ══════════════════════════════════════════════════════════
onMounted(async () => {
  try {
    await connect(() => sessionStorage.getItem('jwt') ?? '')
  } catch {
    showToast(
      connectionError.value || 'اتصال به سرور مدیریت کاربران ممکن نبود.',
      'error', 8000
    )
  }
})
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Vazirmatn:wght@300;400;500;600;700;800&display=swap');

:root {
  --bg: #07090f;
  --surface: #0d1117;
  --surface2: #111827;
  --border: rgba(255, 255, 255, 0.07);
  --border2: rgba(255, 255, 255, 0.04);
  --text: #e2e8f0;
  --muted: #475569;
  --muted2: #334155;
  --green: #34d399;
  --red: #f87171;
  --blue: #60a5fa;
  --orange: #fb923c;
  --purple: #a78bfa;
  --cyan: #38bdf8;
  --amber: #fbbf24;
  --font: 'Vazirmatn', sans-serif;
  --radius: 14px;
}

.page-root {
  min-height: 100vh;
  background: var(--bg);
  color: var(--text);
  font-family: var(--font);
  direction: rtl;
  padding: 28px 24px;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

/* ── Header ── */
.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 20px 24px;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius);
}

.header-brand {
  display: flex;
  align-items: center;
  gap: 14px;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 12px;
}

.brand-icon {
  width: 44px;
  height: 44px;
  background: linear-gradient(135deg, rgba(56, 189, 248, .15), rgba(56, 189, 248, .05));
  border: 1px solid rgba(56, 189, 248, .2);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--cyan);
}

.page-title {
  font-size: 17px;
  font-weight: 800;
  color: var(--text);
  margin: 0;
}

.page-sub {
  font-size: 11.5px;
  color: var(--muted);
  margin: 2px 0 0;
}

/* connection badge */
.conn-badge {
  display: flex;
  align-items: center;
  gap: 6px;
  padding: 5px 12px;
  border-radius: 99px;
  border: 1px solid;
  font-size: 11px;
  font-weight: 700;
}

.conn-ok {
  background: rgba(52, 211, 153, .08);
  border-color: rgba(52, 211, 153, .2);
  color: var(--green);
}

.conn-wait {
  background: rgba(251, 191, 36, .08);
  border-color: rgba(251, 191, 36, .2);
  color: var(--amber);
}

.conn-off {
  background: rgba(248, 113, 113, .08);
  border-color: rgba(248, 113, 113, .2);
  color: var(--red);
}

.conn-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: currentColor;
}

.conn-ok .conn-dot,
.conn-wait .conn-dot {
  animation: pulse-dot 2s infinite;
}

.conn-wait .conn-dot {
  animation-duration: .8s;
}

@keyframes pulse-dot {

  0%,
  100% {
    opacity: 1
  }

  50% {
    opacity: .2
  }
}

/* ── Error bars ── */
.error-bar {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 11px 16px;
  border-radius: 10px;
  font-size: 12.5px;
  font-weight: 600;
}

.error-bar span {
  flex: 1;
}

.error-bar-conn {
  background: rgba(248, 113, 113, .08);
  border: 1px solid rgba(248, 113, 113, .2);
  color: var(--red);
}

.error-bar-op {
  background: rgba(251, 191, 36, .07);
  border: 1px solid rgba(251, 191, 36, .2);
  color: var(--amber);
}

.btn-retry {
  display: flex;
  align-items: center;
  gap: 6px;
  background: rgba(248, 113, 113, .12);
  border: 1px solid rgba(248, 113, 113, .25);
  color: var(--red);
  border-radius: 7px;
  padding: 5px 12px;
  font-size: 11.5px;
  font-weight: 700;
  cursor: pointer;
  font-family: var(--font);
  transition: all .15s;
  white-space: nowrap;
  flex-shrink: 0;
}

.btn-retry:hover:not(:disabled) {
  background: rgba(248, 113, 113, .2);
}

.btn-retry:disabled {
  opacity: .5;
  cursor: not-allowed;
}

.bar-dismiss {
  background: rgba(255, 255, 255, .05);
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 4px;
  color: var(--muted);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all .15s;
  flex-shrink: 0;
}

.bar-dismiss:hover {
  background: rgba(248, 113, 113, .1);
  color: var(--red);
}

/* ── Form error banner ── */
.form-error-banner {
  display: flex;
  align-items: center;
  gap: 8px;
  background: rgba(248, 113, 113, .07);
  border: 1px solid rgba(248, 113, 113, .18);
  border-radius: 9px;
  padding: 10px 14px;
  margin-bottom: 16px;
  font-size: 12px;
  color: var(--red);
  line-height: 1.5;
}

.form-error-banner svg {
  flex-shrink: 0;
}

/* ── Modal error ── */
.modal-error {
  display: flex;
  align-items: center;
  gap: 8px;
  background: rgba(248, 113, 113, .07);
  border: 1px solid rgba(248, 113, 113, .18);
  border-radius: 9px;
  padding: 10px 14px;
  width: 100%;
  font-size: 12px;
  color: var(--red);
  text-align: right;
}

.modal-error svg {
  flex-shrink: 0;
}

/* ── Loading ── */
.loading-bar {
  height: 2px;
  background: rgba(56, 189, 248, .1);
  border-radius: 99px;
  overflow: hidden;
}

.loading-fill {
  height: 100%;
  background: var(--cyan);
  animation: sweep 1.4s ease-in-out infinite;
}

@keyframes sweep {
  0% {
    width: 0%;
    margin-left: 0%
  }

  50% {
    width: 60%;
    margin-left: 20%
  }

  100% {
    width: 0%;
    margin-left: 100%
  }
}

/* ── Stats ── */
.stats-row {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 12px;
}

.stat-card {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  padding: 16px 18px;
  display: flex;
  align-items: center;
  gap: 14px;
}

.stat-icon {
  width: 38px;
  height: 38px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.stat-icon-green {
  background: rgba(52, 211, 153, .1);
  color: var(--green);
  border: 1px solid rgba(52, 211, 153, .2);
}

.stat-icon-red {
  background: rgba(248, 113, 113, .1);
  color: var(--red);
  border: 1px solid rgba(248, 113, 113, .2);
}

.stat-icon-orange {
  background: rgba(251, 146, 60, .1);
  color: var(--orange);
  border: 1px solid rgba(251, 146, 60, .2);
}

.stat-value {
  font-size: 22px;
  font-weight: 800;
  line-height: 1;
}

.stat-label {
  font-size: 11px;
  color: var(--muted);
  margin-top: 3px;
}

/* ── Buttons ── */
.btn-primary {
  display: flex;
  align-items: center;
  gap: 7px;
  background: linear-gradient(135deg, #059669, #10b981);
  color: white;
  border: none;
  border-radius: 10px;
  padding: 10px 18px;
  font-size: 12.5px;
  font-weight: 700;
  cursor: pointer;
  font-family: var(--font);
  box-shadow: 0 4px 16px rgba(16, 185, 129, .2);
  transition: all .2s;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 6px 22px rgba(16, 185, 129, .3);
}

.btn-primary:disabled {
  opacity: .5;
  cursor: not-allowed;
}

.btn-ghost {
  display: flex;
  align-items: center;
  gap: 7px;
  background: transparent;
  color: var(--muted);
  border: 1px solid var(--border);
  border-radius: 10px;
  padding: 9px 18px;
  font-size: 12.5px;
  font-weight: 600;
  cursor: pointer;
  font-family: var(--font);
  transition: all .2s;
}

.btn-ghost:hover:not(:disabled) {
  border-color: rgba(255, 255, 255, .15);
  color: var(--text);
}

.btn-ghost:disabled {
  opacity: .4;
  cursor: not-allowed;
}

.btn-danger {
  display: flex;
  align-items: center;
  gap: 7px;
  background: linear-gradient(135deg, #dc2626, #ef4444);
  color: white;
  border: none;
  border-radius: 10px;
  padding: 10px 18px;
  font-size: 12.5px;
  font-weight: 700;
  cursor: pointer;
  font-family: var(--font);
  transition: all .2s;
}

.btn-danger:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 6px 22px rgba(239, 68, 68, .3);
}

.btn-danger:disabled {
  opacity: .5;
  cursor: not-allowed;
}

.btn-close {
  width: 30px;
  height: 30px;
  background: rgba(255, 255, 255, .05);
  border: 1px solid var(--border);
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--muted);
  cursor: pointer;
  transition: all .2s;
}

.btn-close:hover:not(:disabled) {
  background: rgba(248, 113, 113, .1);
  color: var(--red);
  border-color: rgba(248, 113, 113, .2);
}

.btn-close:disabled {
  opacity: .4;
  cursor: not-allowed;
}

.btn-spinner {
  width: 14px;
  height: 14px;
  border: 2px solid rgba(255, 255, 255, .3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin .7s linear infinite;
  flex-shrink: 0;
}

.btn-spinner-sm {
  width: 10px;
  height: 10px;
  border-width: 1.5px;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

/* ── Form Drawer ── */
.form-drawer {
  background: var(--surface);
  border: 1px solid var(--border);
  border-top: 2px solid var(--green);
  border-radius: var(--radius);
  padding: 24px;
}

.drawer-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 20px;
}

.drawer-title {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13.5px;
  font-weight: 700;
  color: var(--text);
}

.drawer-title-icon {
  width: 26px;
  height: 26px;
  background: rgba(52, 211, 153, .1);
  border: 1px solid rgba(52, 211, 153, .2);
  border-radius: 7px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--green);
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.field-wrap {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.field-label {
  font-size: 11.5px;
  font-weight: 600;
  color: var(--muted);
}

.required {
  color: var(--red);
  margin-right: 2px;
}

.field-optional {
  color: var(--muted2);
  font-size: 10.5px;
  font-weight: 400;
}

.input-wrap {
  position: relative;
  display: flex;
  align-items: center;
}

.input-icon {
  position: absolute;
  right: 11px;
  color: var(--muted2);
  pointer-events: none;
  z-index: 1;
}

.field-input {
  width: 100%;
  background: var(--surface2);
  border: 1px solid var(--border);
  border-radius: 10px;
  color: var(--text);
  font-size: 12.5px;
  font-family: var(--font);
  padding: 9px 34px 9px 12px;
  outline: none;
  transition: border-color .2s, box-shadow .2s;
  -webkit-appearance: none;
}

.field-input:focus {
  border-color: rgba(56, 189, 248, .4);
  box-shadow: 0 0 0 3px rgba(56, 189, 248, .08);
}

.field-input:disabled {
  opacity: .4;
  cursor: not-allowed;
}

.field-error .field-input {
  border-color: rgba(248, 113, 113, .4);
}

.field-error .field-input:focus {
  box-shadow: 0 0 0 3px rgba(248, 113, 113, .08);
}

.error-msg {
  font-size: 11px;
  color: var(--red);
  min-height: 15px;
}

.select-wrap {
  position: relative;
}

.field-select {
  appearance: none;
  cursor: pointer;
  padding-left: 30px;
}

.select-arrow {
  position: absolute;
  left: 10px;
  color: var(--muted);
  pointer-events: none;
}

.pass-toggle {
  position: absolute;
  left: 10px;
  background: none;
  border: none;
  color: var(--muted);
  cursor: pointer;
  padding: 0;
  transition: color .15s;
}

.pass-toggle:hover {
  color: var(--cyan);
}

.pass-strength {
  display: flex;
  align-items: center;
  gap: 8px;
}

.strength-bar {
  flex: 1;
  height: 3px;
  background: rgba(255, 255, 255, .06);
  border-radius: 99px;
  overflow: hidden;
}

.strength-fill {
  height: 100%;
  border-radius: 99px;
  transition: width .3s, background .3s;
}

.strength-label {
  font-size: 10.5px;
  font-weight: 600;
}

.str-weak .strength-fill {
  background: var(--red);
}

.str-weak {
  color: var(--red);
}

.str-fair .strength-fill {
  background: var(--amber);
}

.str-fair {
  color: var(--amber);
}

.str-good .strength-fill {
  background: var(--cyan);
}

.str-good {
  color: var(--cyan);
}

.str-strong .strength-fill {
  background: var(--green);
}

.str-strong {
  color: var(--green);
}

.form-actions {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 24px;
  padding-top: 20px;
  border-top: 1px solid var(--border2);
}

/* ── List Card ── */
.list-card {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  overflow: hidden;
}

.list-toolbar {
  padding: 16px 20px;
  border-bottom: 1px solid var(--border2);
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.search-box {
  position: relative;
  display: flex;
  align-items: center;
}

.search-icon {
  position: absolute;
  right: 13px;
  color: var(--muted);
  pointer-events: none;
  z-index: 1;
  transition: color .2s;
}

.search-input {
  width: 100%;
  background: var(--surface2);
  border: 1px solid var(--border);
  border-radius: 10px;
  color: var(--text);
  font-size: 12.5px;
  font-family: var(--font);
  padding: 10px 40px;
  outline: none;
  transition: border-color .2s, box-shadow .2s;
}

.search-input::placeholder {
  color: var(--muted2);
}

.search-input:focus {
  border-color: rgba(56, 189, 248, .35);
  box-shadow: 0 0 0 3px rgba(56, 189, 248, .07);
}

.search-box:focus-within .search-icon {
  color: var(--cyan);
}

.search-clear {
  position: absolute;
  left: 12px;
  background: rgba(255, 255, 255, .06);
  border: none;
  border-radius: 5px;
  padding: 3px;
  color: var(--muted);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all .15s;
}

.search-clear:hover {
  background: rgba(248, 113, 113, .1);
  color: var(--red);
}

.filter-row {
  display: flex;
  gap: 7px;
  flex-wrap: wrap;
}

.filter-pill {
  padding: 4px 12px;
  border-radius: 99px;
  font-size: 11px;
  font-weight: 600;
  font-family: var(--font);
  border: 1px solid var(--border);
  background: transparent;
  color: var(--muted);
  cursor: pointer;
  transition: all .15s;
}

.filter-pill:hover {
  border-color: rgba(255, 255, 255, .15);
  color: var(--text);
}

.filter-pill.active {
  background: rgba(56, 189, 248, .1);
  border-color: rgba(56, 189, 248, .3);
  color: var(--cyan);
}

.table-wrap {
  overflow-x: auto;
}

.user-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 12.5px;
  font-family: var(--font);
}

.user-table thead tr {
  background: rgba(0, 0, 0, .3);
}

.user-table th {
  padding: 11px 16px;
  text-align: right;
  font-size: 10.5px;
  font-weight: 700;
  color: var(--muted2);
  letter-spacing: .4px;
  white-space: nowrap;
  border-bottom: 1px solid var(--border);
}

.th-idx {
  width: 36px;
  text-align: center;
}

.th-act {
  width: 80px;
  text-align: center;
}

.user-table td {
  padding: 12px 16px;
  border-bottom: 1px solid var(--border2);
  vertical-align: middle;
}

.user-row {
  transition: background .15s;
}

.user-row:hover td {
  background: rgba(255, 255, 255, .015);
}

.user-row:last-child td {
  border-bottom: none;
}

.row-inactive {
  opacity: .55;
}

.td-idx {
  text-align: center;
  color: var(--muted2);
  font-size: 11px;
}

.td-muted {
  color: var(--muted);
  font-size: 12px;
}

.td-act {
  text-align: center;
}

.user-cell {
  display: flex;
  align-items: center;
  gap: 10px;
}

.user-avatar {
  width: 34px;
  height: 34px;
  border-radius: 9px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 11px;
  font-weight: 800;
  flex-shrink: 0;
}

.avatar-admin {
  background: rgba(249, 115, 22, .12);
  color: var(--orange);
  border: 1px solid rgba(249, 115, 22, .2);
}

.avatar-accountant {
  background: rgba(96, 165, 250, .12);
  color: var(--blue);
  border: 1px solid rgba(96, 165, 250, .2);
}

.avatar-viewer {
  background: rgba(107, 114, 128, .12);
  color: #94a3b8;
  border: 1px solid rgba(107, 114, 128, .2);
}

.avatar-operator {
  background: rgba(167, 139, 250, .12);
  color: var(--purple);
  border: 1px solid rgba(167, 139, 250, .2);
}

.user-name {
  font-size: 12.5px;
  font-weight: 600;
  color: var(--text);
}

.user-uname {
  font-size: 11px;
  color: var(--muted);
  margin-top: 1px;
}

.role-pill {
  padding: 3px 10px;
  border-radius: 99px;
  font-size: 10.5px;
  font-weight: 700;
  border: 1px solid;
  white-space: nowrap;
}

.role-admin {
  background: rgba(249, 115, 22, .1);
  border-color: rgba(249, 115, 22, .25);
  color: var(--orange);
}

.role-accountant {
  background: rgba(96, 165, 250, .1);
  border-color: rgba(96, 165, 250, .25);
  color: var(--blue);
}

.role-viewer {
  background: rgba(107, 114, 128, .1);
  border-color: rgba(107, 114, 128, .25);
  color: #94a3b8;
}

.role-operator {
  background: rgba(167, 139, 250, .1);
  border-color: rgba(167, 139, 250, .25);
  color: var(--purple);
}

.status-toggle {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 4px 10px;
  border-radius: 99px;
  border: 1px solid;
  font-size: 11px;
  font-weight: 700;
  font-family: var(--font);
  cursor: pointer;
  transition: all .15s;
  min-width: 72px;
  justify-content: center;
}

.status-toggle:disabled {
  opacity: .4;
  cursor: not-allowed;
}

.st-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  flex-shrink: 0;
}

.st-active {
  background: rgba(52, 211, 153, .07);
  border-color: rgba(52, 211, 153, .25);
  color: var(--green);
}

.st-active .st-dot {
  background: var(--green);
}

.st-active:hover:not(:disabled) {
  background: rgba(52, 211, 153, .14);
}

.st-inactive {
  background: rgba(248, 113, 113, .07);
  border-color: rgba(248, 113, 113, .25);
  color: var(--red);
}

.st-inactive .st-dot {
  background: var(--red);
}

.st-inactive:hover:not(:disabled) {
  background: rgba(248, 113, 113, .14);
}

.row-actions {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 5px;
}

.action-btn {
  width: 30px;
  height: 30px;
  border-radius: 8px;
  border: 1px solid var(--border);
  background: transparent;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all .15s;
}

.action-btn:disabled {
  opacity: .3;
  cursor: not-allowed;
}

.ab-edit {
  color: var(--blue);
}

.ab-edit:hover:not(:disabled) {
  background: rgba(96, 165, 250, .1);
  border-color: rgba(96, 165, 250, .25);
}

.ab-del {
  color: var(--muted);
}

.ab-del:hover:not(:disabled) {
  background: rgba(248, 113, 113, .1);
  border-color: rgba(248, 113, 113, .25);
  color: var(--red);
}

.list-footer {
  padding: 12px 20px;
  border-top: 1px solid var(--border2);
  font-size: 11px;
  color: var(--muted2);
  text-align: left;
}

/* ── Empty ── */
.empty-state {
  padding: 56px 24px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
}

.empty-icon {
  width: 64px;
  height: 64px;
  background: rgba(255, 255, 255, .03);
  border: 1px solid var(--border);
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--muted2);
  margin-bottom: 6px;
}

.empty-title {
  font-size: 14px;
  font-weight: 700;
  color: var(--text);
}

.empty-sub {
  font-size: 12px;
  color: var(--muted);
}

/* ── Modal ── */
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, .65);
  backdrop-filter: blur(6px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 200;
}

.confirm-modal {
  background: #0d1117;
  border: 1px solid rgba(248, 113, 113, .2);
  border-radius: 18px;
  padding: 32px 28px;
  max-width: 400px;
  width: 90%;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  text-align: center;
}

.confirm-icon {
  width: 56px;
  height: 56px;
  background: rgba(248, 113, 113, .1);
  border: 1px solid rgba(248, 113, 113, .2);
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--red);
  margin-bottom: 4px;
}

.confirm-title {
  font-size: 16px;
  font-weight: 800;
  color: var(--text);
}

.confirm-desc {
  font-size: 12.5px;
  color: var(--muted);
  line-height: 1.7;
}

.confirm-desc strong {
  color: var(--text);
}

.confirm-actions {
  display: flex;
  gap: 10px;
  margin-top: 8px;
}

/* ── Toast ── */
.toast {
  position: fixed;
  bottom: 24px;
  left: 24px;
  display: flex;
  align-items: center;
  gap: 9px;
  padding: 11px 16px;
  border-radius: 11px;
  border: 1px solid;
  font-size: 12.5px;
  font-weight: 700;
  font-family: var(--font);
  z-index: 9999;
  backdrop-filter: blur(16px);
  box-shadow: 0 8px 28px rgba(0, 0, 0, .4);
  max-width: 380px;
}

.toast-icon {
  width: 22px;
  height: 22px;
  border-radius: 6px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(255, 255, 255, .07);
  flex-shrink: 0;
}

.toast-success {
  background: rgba(52, 211, 153, .1);
  border-color: rgba(52, 211, 153, .2);
  color: var(--green);
}

.toast-error {
  background: rgba(248, 113, 113, .1);
  border-color: rgba(248, 113, 113, .2);
  color: var(--red);
}

.toast-info {
  background: rgba(56, 189, 248, .1);
  border-color: rgba(56, 189, 248, .2);
  color: var(--cyan);
}

.toast-warn {
  background: rgba(251, 191, 36, .1);
  border-color: rgba(251, 191, 36, .2);
  color: var(--amber);
}

/* ── Transitions ── */
.drawer-enter-active,
.drawer-leave-active {
  transition: all .3s ease;
}

.drawer-enter-from,
.drawer-leave-to {
  opacity: 0;
  transform: translateY(-10px);
  max-height: 0;
}

.overlay-enter-active,
.overlay-leave-active {
  transition: opacity .25s;
}

.overlay-enter-from,
.overlay-leave-to {
  opacity: 0;
}

.toast-enter-active,
.toast-leave-active {
  transition: all .3s;
}

.toast-enter-from,
.toast-leave-to {
  opacity: 0;
  transform: translateY(12px);
}

.bar-enter-active,
.bar-leave-active {
  transition: all .25s ease;
}

.bar-enter-from,
.bar-leave-to {
  opacity: 0;
  transform: translateY(-6px);
  max-height: 0;
}

* {
  scrollbar-width: thin;
  scrollbar-color: rgba(255, 255, 255, .06) transparent;
}

::-webkit-scrollbar {
  width: 4px;
  height: 4px;
}

::-webkit-scrollbar-thumb {
  background: rgba(255, 255, 255, .07);
  border-radius: 2px;
}
</style>
