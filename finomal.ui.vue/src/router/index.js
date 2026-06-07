import { createRouter, createWebHistory } from "vue-router";
import { routes } from "vue-router/auto-routes";

import LoginPage from "../pages/auth/LoginPage.vue";
import RegisterPage from "../pages/auth/RegisterPage.vue";
import UserManagment from "../pages/settings/UserManagment.vue";
import CreatePersonnel from "../pages/hr/CreatePersonnel.vue";
import PersonnelList from "../pages/hr/PersonnelList.vue";
import MonthlyAttendance from "../pages/hr/MonthlyAttendance.vue";
import HomePage from "../pages/home/HomePage.vue";
import MainLayout from "../layouts/MainLayout.vue";
import AccountingDocumentCreate from "../pages/accounting/AccountingDocumentCreate.vue";
import PersonnelContracts from "../pages/hr/PersonnelContracts.vue";
import PayrollCalculation from "../pages/hr/PayrollCalculation.vue";
import PaySlip from "../pages/hr/PaySlip.vue";
import IncomeStatement from "../pages/reports/IncomeStatement.vue";
import BalanceSheet from "../pages/reports/BalanceSheet.vue";
import CashFlow from "../pages/treasury/CashFlow.vue";
import TaxpayersReport from "../pages/reports/TaxpayersReport.vue";
import WarehouseDefinition from "../pages/inventory/WarehouseDefinition.vue";
import ProductDefinition from "../pages/inventory/ProductDefinition.vue";
import WarehouseReceipt from "../pages/inventory/WarehouseReceipt.vue";
import WarehouseIssue from "../pages/inventory/WarehouseIssue.vue";
import WarehouseTransfer from "../pages/inventory/WarehouseTransfer.vue";
import ItemKardex from "../pages/inventory/ItemKardex.vue";
import WarehouseInventory from "../pages/inventory/WarehouseInventory.vue";

import CustomersDefinition from "../pages/sales/CustomersDefinition.vue";
import ProformaInvoice from "../pages/sales/ProformaInvoice.vue";
import SalesInvoice from "../pages/sales/SalesInvoice.vue";
import SalesReturn from "../pages/sales/SalesReturn.vue";
import SalesReports from "../pages/sales/SalesReports.vue";
import SuppliersDefinition from "../pages/purchasing/SuppliersDefinition.vue";
import PurchaseOrder from "../pages/purchasing/PurchaseOrder.vue";
import PurchaseInvoice from "../pages/purchasing/PurchaseInvoice.vue";
import PurchaseReturn from "../pages/purchasing/PurchaseReturn.vue";

import AccountingDocumentList from "../pages/accounting/AccountingDocumentList.vue";
import ChartOfAccounts from "../pages/accounting/ChartOfAccounts.vue";
import FiscalYear from "../pages/accounting/FiscalYear.vue";

import BankCashDefinition from "../pages/treasury/BankCashDefinition.vue";
import CashReceipt from "../pages/treasury/CashReceipt.vue";
import CashPayment from "../pages/treasury/CashPayment.vue";
import ChequeManagement from "../pages/treasury/ChequeManagement.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/login",
      name: "LoginPage",
      component: LoginPage,
      meta: { public: true },
    },
    {
      path: "/register",
      name: "RegisterPage",
      component: RegisterPage,
      meta: { public: true },
    },
    {
      path: "/",
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        {
          path: "",
          name: "HomePage",
          component: HomePage
        },
        {
          path: "accounting",
          children: [
            {
              path: "create",
              name: "AccountingDocumentCreate",
              component: AccountingDocumentCreate,
            },
            { path: "list", name: "AccountingDocumentList", component: AccountingDocumentList },
{ path: "account-review", name: "AccountReview", component: () => import("../pages/accounting/AccountReview.vue") },
          ],
        },
        { path: "/accounting/chart-of-accounts", name: "ChartOfAccounts", component: ChartOfAccounts },
        { path: "/accounting/fiscal-year", name: "FiscalYear", component: FiscalYear },
      ],
    },
    {
      path: "/setting",
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        {
          path: "user-managment",
          name: "UserManagment",
          component: UserManagment,
        },
      ],
    },
    {
      path: "/personnel",
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        {
          path: "",
          name: "PersonnelList",
          component: PersonnelList,
        },
        {
          path: "create-personnel",
          name: "CreatePersonnel",
          component: CreatePersonnel,
        },
        {
          path: "attendance",
          name: "MonthlyAttendance",
          component: MonthlyAttendance,
        },
        {
          path: "contracts",
          name: "PersonnelContracts",
          component: PersonnelContracts,
        },
        {
          path: "payroll",
          name: "PayrollCalculation",
          component: PayrollCalculation,
        },
        {
          path: "payslip",
          name: "PaySlip",
          component: PaySlip,
        },
      ],
    },
    {
      path: "/reports",
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        { path: "income-statement", name: "IncomeStatement", component: IncomeStatement },
        { path: "balance-sheet", name: "BalanceSheet", component: BalanceSheet },
        { path: "cash-flow", name: "CashFlow", component: CashFlow },
        { path: "taxpayers", name: "TaxpayersReport", component: TaxpayersReport },
      ],
    },
    {
      path: "/inventory",
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        { path: "warehouses", name: "WarehouseDefinition", component: WarehouseDefinition },
        { path: "products", name: "ProductDefinition", component: ProductDefinition },
        { path: "receipts", name: "WarehouseReceipt", component: WarehouseReceipt },
        { path: "issues", name: "WarehouseIssue", component: WarehouseIssue },
        { path: "transfers", name: "WarehouseTransfer", component: WarehouseTransfer },
        { path: "kardex", name: "ItemKardex", component: ItemKardex },
        { path: "stock", name: "WarehouseInventory", component: WarehouseInventory },
      ],
    },
    {
      path: "/sales",
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        { path: "customers", name: "CustomersDefinition", component: CustomersDefinition },
        { path: "proforma", name: "ProformaInvoice", component: ProformaInvoice },
        { path: "invoice", name: "SalesInvoice", component: SalesInvoice },
        { path: "returns", name: "SalesReturn", component: SalesReturn },
        { path: "reports", name: "SalesReports", component: SalesReports },
      ],
    },
    {
      path: "/purchase",
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        { path: "suppliers", name: "SuppliersDefinition", component: SuppliersDefinition },
        { path: "orders", name: "PurchaseOrder", component: PurchaseOrder },
        { path: "invoice", name: "PurchaseInvoice", component: PurchaseInvoice },
        { path: "returns", name: "PurchaseReturn", component: PurchaseReturn },
      ],
    },
    {
      path: "/treasury",
      component: MainLayout,
      meta: { requiresAuth: true },
      children: [
        { path: "banks-cash", name: "BankCashDefinition", component: BankCashDefinition },
        { path: "receipts", name: "CashReceipt", component: CashReceipt },
        { path: "payments", name: "CashPayment", component: CashPayment },
        { path: "cheques", name: "ChequeManagement", component: ChequeManagement },
      ],
    },

    ...routes,
  ],
});

// Workaround for https://github.com/vitejs/vite/issues/11804
router.onError((err, to) => {
  if (err?.message?.includes?.("Failed to fetch dynamically imported module")) {
    if (localStorage.getItem("vuetify:dynamic-reload")) {
      console.error("Dynamic import error, reloading page did not fix it", err);
    } else {
      console.log("Reloading page to fix dynamic import error");
      localStorage.setItem("vuetify:dynamic-reload", "true");
      location.assign(to.fullPath);
    }
  } else {
    console.error(err);
  }
});

router.isReady().then(() => {
  localStorage.removeItem("vuetify:dynamic-reload");
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("jwt") || sessionStorage.getItem("jwt");

  const isPublic = to.meta.public === true;

  if (!isPublic && to.meta.requiresAuth && !token) {
    return next({ name: "LoginPage" });
  }

  if (to.name === "LoginPage" && token) {
    return next({ name: "HomePage" });
  }

  next();
});

export default router;
