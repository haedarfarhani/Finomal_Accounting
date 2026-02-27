// plugins/vuetify.js
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'
import { createVuetify } from 'vuetify'

// ── پالت رنگی فینومال ──────────────────────────────────────────────────────
const FINOMAL = {
  // پس‌زمینه‌ها
  bg:       '#05070a',   // عمیق‌ترین لایه
  surface1: '#0a0c12',   // کارت‌های اول
  surface2: '#0f1219',   // کارت‌های دوم
  surface3: '#141820',   // input‌ها

  // برند
  primary:  '#63b3ed',   // آبی اصلی
  primary2: '#4299e1',   // آبی تیره‌تر

  // وضعیت
  success:  '#48bb78',
  error:    '#fc8181',
  warning:  '#f6ad55',
  info:     '#63b3ed',

  // متن
  textPrimary:   '#e8edf5',
  textSecondary: '#5a6480',
  textDisabled:  '#3d4560',
}

export default createVuetify({

  // ── آیکون‌ها ──────────────────────────────────────────────────────────────
  icons: {
    defaultSet: 'mdi',
  },

  // ── Theme ─────────────────────────────────────────────────────────────────
  theme: {
    defaultTheme: 'finomalDark',

    themes: {
      finomalDark: {
        dark: true,
        colors: {
          // پس‌زمینه و سطوح
          background:  FINOMAL.bg,
          surface:     FINOMAL.surface1,
          'surface-1': FINOMAL.surface1,
          'surface-2': FINOMAL.surface2,
          'surface-3': FINOMAL.surface3,

          // برند
          primary:             FINOMAL.primary,
          'primary-darken-1':  FINOMAL.primary2,

          // وضعیت
          success: FINOMAL.success,
          error:   FINOMAL.error,
          warning: FINOMAL.warning,
          info:    FINOMAL.info,

          // متن روی سطوح
          'on-background': FINOMAL.textPrimary,
          'on-surface':    FINOMAL.textPrimary,
          'on-primary':    '#ffffff',
          'on-success':    '#ffffff',
          'on-error':      '#ffffff',
        },
      },
    },

    variations: {
      colors:  ['primary', 'success', 'error', 'warning'],
      lighten: 2,
      darken:  2,
    },
  },

  // ── Defaults — تنظیمات پیش‌فرض کامپوننت‌ها ────────────────────────────────
  defaults: {

    // ── Input ها ────────────────────────────────────────────────────────────
    VTextField: {
      variant:     'outlined',
      density:     'compact',
      hideDetails: 'auto',
      color:       'primary',
      bgColor:     FINOMAL.surface3,
    },
    VSelect: {
      variant:     'outlined',
      density:     'compact',
      hideDetails: 'auto',
      color:       'primary',
      bgColor:     FINOMAL.surface3,
    },
    VTextarea: {
      variant:     'outlined',
      density:     'compact',
      hideDetails: 'auto',
      color:       'primary',
      bgColor:     FINOMAL.surface3,
      autoGrow:    false,
    },
    VAutocomplete: {
      variant:     'outlined',
      density:     'compact',
      hideDetails: 'auto',
      color:       'primary',
      bgColor:     FINOMAL.surface3,
    },
    VCombobox: {
      variant:     'outlined',
      density:     'compact',
      hideDetails: 'auto',
      color:       'primary',
    },
    VFileInput: {
      variant:     'outlined',
      density:     'compact',
      hideDetails: 'auto',
      color:       'primary',
    },

    // ── دکمه‌ها ──────────────────────────────────────────────────────────────
    VBtn: {
      variant:   'flat',
      rounded:   'lg',
      elevation: 0,
      style:     'font-family: Vazirmatn, sans-serif; font-weight: 700; letter-spacing: 0;',
    },
    VBtnGroup: {
      rounded:   'lg',
      elevation: 0,
    },

    // ── کارت‌ها ──────────────────────────────────────────────────────────────
    VCard: {
      rounded:   'xl',
      elevation: 0,
      color:     FINOMAL.surface1,
    },
    VCardTitle: {
      style: 'font-family: Vazirmatn, sans-serif; font-weight: 800; font-size: 15px;',
    },
    VCardSubtitle: {
      style: 'font-family: Vazirmatn, sans-serif; font-size: 12px; opacity: 0.6;',
    },

    // ── جدول ─────────────────────────────────────────────────────────────────
    VDataTable: {
      elevation: 0,
      rounded:   'xl',
      hover:     true,
    },

    // ── دیالوگ ───────────────────────────────────────────────────────────────
    VDialog: {
      maxWidth: 540,
      rounded:  'xl',
    },

    // ── List ─────────────────────────────────────────────────────────────────
    VList: {
      bgColor:   FINOMAL.surface2,
      rounded:   'lg',
      elevation: 0,
    },
    VListItem: {
      rounded:   'lg',
      minHeight: 40,
    },

    // ── Chip ─────────────────────────────────────────────────────────────────
    VChip: {
      rounded:   'lg',
      size:      'small',
      elevation: 0,
    },

    // ── Alert ─────────────────────────────────────────────────────────────────
    VAlert: {
      rounded:   'xl',
      elevation: 0,
      variant:   'tonal',
    },

    // ── Checkbox / Switch ──────────────────────────────────────────────────
    VCheckbox: {
      color:       'primary',
      hideDetails: 'auto',
    },
    VSwitch: {
      color:       'primary',
      hideDetails: 'auto',
      inset:       true,
    },
    VRadio: {
      color: 'primary',
    },

    // ── Divider ──────────────────────────────────────────────────────────────
    VDivider: {
      opacity: 1,
    },

    // ── Tooltip ──────────────────────────────────────────────────────────────
    VTooltip: {
      location: 'top',
    },

    // ── Snackbar ─────────────────────────────────────────────────────────────
    VSnackbar: {
      rounded:  'xl',
      location: 'bottom center',
    },

    // ── Menu (dropdown) ───────────────────────────────────────────────────────
    VMenu: {
      rounded:   'xl',
      elevation: 4,
    },

    // ── Pagination ────────────────────────────────────────────────────────────
    VPagination: {
      rounded: 'lg',
      density: 'compact',
      color:   'primary',
    },

    // ── Tabs ─────────────────────────────────────────────────────────────────
    VTab: {
      rounded: 'lg',
      style:   'font-family: Vazirmatn, sans-serif; font-weight: 700; letter-spacing: 0;',
    },

    // ── Badge ─────────────────────────────────────────────────────────────────
    VBadge: {
      color:   'error',
      rounded: 'lg',
    },

    // ── Progress ─────────────────────────────────────────────────────────────
    VProgressLinear: {
      color:   'primary',
      rounded: true,
      height:  4,
    },
    VProgressCircular: {
      color: 'primary',
    },
  },
})