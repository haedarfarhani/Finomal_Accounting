import { createRouter, createWebHistory } from "vue-router";
import { routes } from "vue-router/auto-routes";

import LoginPage from "../pages/LoginPage.vue";
import RegisterPage from "../pages/RegisterPage.vue";
import UserManagment from "../pages/UserManagment.vue";
import CreatePersonnel from "../pages/CreatePersonnel.vue";
import PersonnelList from "../pages/PersonnelList.vue";
import MonthlyAttendance from "../pages/MonthlyAttendance.vue";
import HomePage from "../pages/HomePage.vue";
import MainLayout from "../layouts/MainLayout.vue";
import AccountingDocumentCreate from "../pages/AccountingDocumentCreate.vue";
import PersonnelContracts from "../pages/PersonnelContracts.vue";
import PayrollCalculation from "../pages/PayrollCalculation.vue";
import PaySlip from "../pages/PaySlip.vue";
import IncomeStatement from "../pages/IncomeStatement.vue";
import BalanceSheet from "../pages/BalanceSheet.vue";
import CashFlow from "../pages/CashFlow.vue";
import TaxpayersReport from "../pages/TaxpayersReport.vue";
import WarehouseDefinition from "../pages/WarehouseDefinition.vue";
import ProductDefinition from "../pages/ProductDefinition.vue";
import WarehouseReceipt from "../pages/WarehouseReceipt.vue";
import WarehouseIssue from "../pages/WarehouseIssue.vue";
import WarehouseTransfer from "../pages/WarehouseTransfer.vue";
import ItemKardex from "../pages/ItemKardex.vue";
import WarehouseInventory from "../pages/WarehouseInventory.vue";

import CustomersDefinition from "../pages/CustomersDefinition.vue";
import ProformaInvoice from "../pages/ProformaInvoice.vue";
import SalesInvoice from "../pages/SalesInvoice.vue";
import SalesReturn from "../pages/SalesReturn.vue";
import SalesReports from "../pages/SalesReports.vue";
import SuppliersDefinition from "../pages/SuppliersDefinition.vue";
import PurchaseOrder from "../pages/PurchaseOrder.vue";
import PurchaseInvoice from "../pages/PurchaseInvoice.vue";
import PurchaseReturn from "../pages/PurchaseReturn.vue";

import AccountingDocumentList from "../pages/AccountingDocumentList.vue";
import ChartOfAccounts from "../pages/ChartOfAccounts.vue";
import FiscalYear from "../pages/FiscalYear.vue";

import BankCashDefinition from "../pages/BankCashDefinition.vue";
import CashReceipt from "../pages/CashReceipt.vue";
import CashPayment from "../pages/CashPayment.vue";
import ChequeManagement from "../pages/ChequeManagement.vue";

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
          path: "accounting-document",
          children: [
            {
              path: "create",
              name: "AccountingDocumentCreate",
              component: AccountingDocumentCreate,
            },
            { path: "list", name: "AccountingDocumentList", component: AccountingDocumentList },
          ],
        },
        { path: "chart-of-accounts", name: "ChartOfAccounts", component: ChartOfAccounts },
        { path: "fiscal-year", name: "FiscalYear", component: FiscalYear },
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
