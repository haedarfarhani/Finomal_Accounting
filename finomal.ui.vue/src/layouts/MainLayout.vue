<template>
  <div class="layout">
    <AppSidebar />

    <div class="content-wrapper">
      <main class="main-content">
        <router-view />
      </main>

      <AppFooter />
    </div>
  </div>
</template>

<script>
import AppSidebar from "@/components/AppSidebar.vue";
import AppFooter from "@/components/AppFooter.vue";

export default {
  components: {
    AppSidebar,
    AppFooter
  }
};
</script>

<style>
/* ── Override Vuetify global background (بدون scoped) ── */
html,
body,
#app,
.v-application,
.v-application__wrap {
  background: #05070a !important;
  color: #e8edf5 !important;
}

/* حذف padding پیش‌فرض Vuetify */
.v-application__wrap {
  min-height: unset !important;
}
</style>

<style scoped>
.layout {
  display: flex;
  height: 100vh;
  overflow: hidden;
  direction: rtl;
  background: #05070a;
}

.content-wrapper {
  display: flex;
  flex-direction: column;
  flex: 1;
  min-width: 0;
  background: #05070a;
  overflow: hidden;
}

.main-content {
  flex: 1;
  overflow-y: auto;
  overflow-x: hidden;
  padding: 1.5rem 2rem;
  background: #05070a;
}

:deep(.app-footer) {
  flex-shrink: 0;
  height: 38px;
}
</style>