<template>
  <div class="account-review-page">
    <div class="layout-container">
      <!-- Sidebar Filters -->
      <aside class="filter-panel">
        <AccountFilterPanel @filter-changed="onFilterChanged" />
      </aside>

      <!-- Main Content -->
      <section class="main-content">
        <!-- Summary Dashboard -->
        <AccountSummary :summary="summary" />

        <!-- Transaction Grid -->
        <AccountTransactionGrid
          :transactions="transactions"
          :loading="loading"
          @row-clicked="onRowClicked"
        />
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, onMounted } from 'vue';
import AccountFilterPanel from '@/components/accounting/AccountFilterPanel.vue';
import AccountSummary from '@/components/accounting/AccountSummary.vue';
import AccountTransactionGrid from '@/components/accounting/AccountTransactionGrid.vue';
import { fetchAccountSummary, fetchAccountTransactions } from '@/services/accountingService';

// Reactive state
const selectedAccountId = ref<string>('');
const filters = ref<any>({});
const summary = ref<any>(null);
const transactions = ref<Array<any>>([]);
const loading = ref<boolean>(false);

function loadData() {
  if (!selectedAccountId.value) return;
  loading.value = true;
  // Fetch summary
  fetchAccountSummary(selectedAccountId.value, filters.value)
    .then((data) => {
      summary.value = data;
    })
    .catch(console.error);
  // Fetch transactions
  fetchAccountTransactions(selectedAccountId.value, filters.value)
    .then((data) => {
      transactions.value = data;
    })
    .catch(console.error)
    .finally(() => {
      loading.value = false;
    });
}

function onFilterChanged(newFilters: any) {
  filters.value = newFilters;
  loadData();
}

function onRowClicked(row: any) {
  // Open detailed document view – placeholder implementation
  const docUrl = `/documents/${row.documentId}`;
  window.open(docUrl, '_blank');
}

// Watch for account selection changes (could be emitted from filter panel)
watch(selectedAccountId, loadData);

onMounted(() => {
  // Initial load could be empty until user selects an account
});
</script>

<style scoped>
.account-review-page {
  padding: 1rem;
  min-height: 100vh;
  background: var(--background-gradient, linear-gradient(135deg, #f0f4ff, #e6e9ff));
}
.layout-container {
  display: flex;
  gap: 1rem;
}
.filter-panel {
  flex: 0 0 280px;
  background: rgba(255, 255, 255, 0.2);
  backdrop-filter: blur(12px);
  border-radius: 12px;
  padding: 1rem;
}
.main-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
@media (max-width: 960px) {
  .layout-container {
    flex-direction: column;
  }
  .filter-panel {
    flex: none;
    width: 100%;
  }
}
</style>
