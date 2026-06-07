<template>
  <v-dialog :model-value="modelValue" @update:model-value="$emit('update:model-value', $event)" max-width="400" persistent>
    <v-card class="confirm-dialog-card">
      <v-card-text class="pt-6 text-center">
        <div class="alert-icon-wrapper">
          <v-icon size="36" color="error">mdi-alert-circle-outline</v-icon>
        </div>
        <h3 class="dialog-title mb-2">{{ title }}</h3>
        <p class="dialog-text mx-auto">
          <slot>آیا از انجام این عملیات مطمئن هستید؟</slot>
        </p>
      </v-card-text>
      <div class="d-flex justify-center gap-3 pb-6 actions-wrapper">
        <v-btn class="cancel-btn" variant="text" @click="$emit('update:model-value', false)">انصراف</v-btn>
        <v-btn class="confirm-btn" color="error" @click="$emit('confirm')" :loading="loading">
          {{ confirmText }}
        </v-btn>
      </div>
    </v-card>
  </v-dialog>
</template>

<script setup>
defineProps({
  modelValue: {
    type: Boolean,
    required: true
  },
  title: {
    type: String,
    default: 'حذف آیتم'
  },
  confirmText: {
    type: String,
    default: 'تایید و حذف'
  },
  loading: {
    type: Boolean,
    default: false
  }
});

defineEmits(['update:model-value', 'confirm']);
</script>

<style scoped>
.confirm-dialog-card {
  background: rgba(15, 23, 42, 0.9) !important;
  backdrop-filter: blur(16px) !important;
  border: 1px solid rgba(239, 68, 68, 0.2) !important;
  border-radius: 16px !important;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5) !important;
  overflow: hidden;
  font-family: 'Vazirmatn', sans-serif;
  direction: rtl;
}

.alert-icon-wrapper {
  width: 64px;
  height: 64px;
  border-radius: 50%;
  background: rgba(239, 68, 68, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 16px;
  border: 1px solid rgba(239, 68, 68, 0.2);
}

.dialog-title {
  color: #f8fafc;
  font-size: 18px;
  font-weight: 700;
}

.dialog-text {
  color: #94a3b8;
  font-size: 13.5px;
  line-height: 1.6;
  max-width: 280px;
}

.actions-wrapper {
  gap: 12px;
}

.cancel-btn {
  color: #94a3b8 !important;
  font-weight: 600;
  border-radius: 8px;
}

.confirm-btn {
  font-weight: 600;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(239, 68, 68, 0.2);
}
</style>
