# 🎯 **ACCOUNTING SETTINGS QUICK REFERENCE**
## **Chart of Accounts Mapping for Pharmacy ERP**

---

## 📊 **COMPLETE ACCOUNT STRUCTURE**

### **1. SALES ACCOUNTS (10 accounts)**

| Property | Purpose | Chart of Account Example | Typical Code |
|----------|---------|--------------------------|--------------|
| `SalesAccountId` | **Taxable sales revenue (15% VAT)** | 4010 - Sales Revenue (Taxable) | 4010 |
| `SalesWithoutVatAccountId` | Sales without VAT documentation | 4015 - Sales (No VAT) | 4015 |
| `ZeroRatedSalesAccountId` | **Zero-rated sales (0% VAT registered)** | 4020 - Zero-Rated Sales | 4020 |
| `ExemptSalesAccountId` | **VAT-exempt sales** | 4030 - Exempt Sales | 4030 |
| `SalesReturnAccountId` | Sales returns contra account | 4110 - Sales Returns & Allowances | 4110 |
| `SalesDiscountAccountId` | **Invoice-level discounts** | 4120 - Sales Discounts | 4120 |
| `DeferredRevenueAccountId` | Unearned/deferred revenue liability | 2410 - Deferred Revenue | 2410 |
| `LoyaltyPointsAccountId` | Loyalty program liability | 2420 - Loyalty Points Liability | 2420 |
| `GiftCardAccountId` | Gift card liability | 2430 - Gift Cards Payable | 2430 |
| `SalesCommissionAccountId` | Sales commission expense | 5210 - Sales Commission | 5210 |

---

### **2. VAT/TAX ACCOUNTS (6 accounts)**

| Property | Purpose | Chart of Account Example | Typical Code |
|----------|---------|--------------------------|--------------|
| `VatAccountId` | General VAT (legacy fallback) | 2310 - VAT Payable | 2310 |
| `VatOutputAccountId` | **VAT output on sales (CRITICAL)** | 2311 - VAT Output (Sales) | 2311 |
| `VatInputAccountId` | VAT input on purchases | 1510 - VAT Input (Purchases) | 1510 |
| `VatSettlementAccountId` | VAT settlement clearing | 2312 - VAT Settlement | 2312 |
| `WithholdingTaxAccountId` | Withholding tax payable | 2320 - WHT Payable | 2320 |
| `VatSuspenseAccountId` | VAT suspense/adjustments | 2315 - VAT Suspense | 2315 |

---

### **3. PURCHASES ACCOUNTS (7 accounts)**

| Property | Purpose | Chart of Account Example | Typical Code |
|----------|---------|--------------------------|--------------|
| `PurchaseAccountId` | Purchase expense | 5010 - Purchases | 5010 |
| `PurchaseWithoutVatAccountId` | Purchases without VAT | 5015 - Purchases (No VAT) | 5015 |
| `PurchaseVatAccountId` | Purchase VAT tracking | 5018 - Purchase VAT | 5018 |
| `PurchaseReturnAccountId` | Purchase returns contra | 5110 - Purchase Returns | 5110 |
| `PurchaseDiscountAccountId` | Purchase discounts received (contra expense) | 5120 - Purchase Discounts | 5120 |
| `PurchaseAccrualAccountId` | Goods received not invoiced | 2510 - Purchase Accrual | 2510 |
| `FreightExpenseAccountId` | Inbound freight/shipping | 5220 - Freight In | 5220 |

---

### **4. INVENTORY ACCOUNTS (9 accounts)**

| Property | Purpose | Chart of Account Example | Typical Code |
|----------|---------|--------------------------|--------------|
| `InventoryAccountId` | **Inventory asset (CRITICAL)** | 1310 - Inventory | 1310 |
| `InventoryAdjustmentAccountId` | Stock count adjustments | 5310 - Inventory Adjustment | 5310 |
| `InventoryLossAccountId` | Stock shrinkage/theft | 5320 - Inventory Loss | 5320 |
| `InventoryGainAccountId` | Stock surplus | 4510 - Inventory Gain | 4510 |
| `DamagedInventoryAccountId` | Damaged stock write-off | 5330 - Damaged Stock | 5330 |
| `ExpiredItemsAccountId` | Expired pharmaceuticals | 5340 - Expired Items | 5340 |
| `StockOpeningAccountId` | Opening stock (year start) | 1311 - Opening Stock | 1311 |
| `StockClosingAccountId` | Closing stock (year end) | 1312 - Closing Stock | 1312 |
| `StockTransferAccountId` | Inter-branch transfer clearing | 1315 - Stock in Transit | 1315 |

---

### **5. COGS ACCOUNT (1 account)**

| Property | Purpose | Chart of Account Example | Typical Code |
|----------|---------|--------------------------|--------------|
| `CogsAccountId` | **Cost of goods sold (CRITICAL)** | 5010 - Cost of Goods Sold | 5010 |

---

### **6. CASH/POS/BANK ACCOUNTS (7 accounts)**

| Property | Purpose | Chart of Account Example | Typical Code |
|----------|---------|--------------------------|--------------|
| `CashAccountId` | **Cash on hand (CRITICAL)** | 1110 - Cash | 1110 |
| `BankAccountId` | **Bank account default (CRITICAL)** | 1120 - Bank Account | 1120 |
| `PosAccountId` | POS terminal clearing | 1115 - POS Terminal | 1115 |
| `PettyCashAccountId` | Petty cash | 1111 - Petty Cash | 1111 |
| `CashDifferenceAccountId` | Cash over/short | 5410 - Cash Over/Short | 5410 |
| `BankFeesAccountId` | Bank charges expense | 5230 - Bank Fees | 5230 |
| `ChequeAccountId` | Cheque clearing | 1125 - Cheques | 1125 |

---

### **7. CUSTOMER/SUPPLIER ACCOUNTS (6 accounts)**

| Property | Purpose | Chart of Account Example | Typical Code |
|----------|---------|--------------------------|--------------|
| `ReceivableAccountId` | **Accounts receivable (CRITICAL)** | 1210 - Accounts Receivable | 1210 |
| `CustomerAdvanceAccountId` | Customer prepayments liability | 2440 - Customer Advances | 2440 |
| `CustomerRefundAccountId` | Customer refund liability | 2450 - Customer Refunds Payable | 2450 |
| `SupplierAdvanceAccountId` | Supplier prepayments asset | 1410 - Supplier Advances | 1410 |
| `SupplierPayableAccountId` | Accounts payable | 2210 - Accounts Payable | 2210 |
| `BadDebtAccountId` | Bad debt expense | 5420 - Bad Debt Expense | 5420 |

---

### **8. EXPENSES ACCOUNTS (5 accounts)**

| Property | Purpose | Chart of Account Example | Typical Code |
|----------|---------|--------------------------|--------------|
| `GeneralExpenseAccountId` | Miscellaneous expenses | 5510 - General Expenses | 5510 |
| `SalaryExpenseAccountId` | Salary and wages | 5610 - Salaries | 5610 |
| `RentExpenseAccountId` | Rent expense | 5710 - Rent | 5710 |
| `ElectricityExpenseAccountId` | Utilities - electricity | 5720 - Electricity | 5720 |
| `InternetExpenseAccountId` | Utilities - internet | 5730 - Internet | 5730 |

---

### **9. SYSTEM ACCOUNTS (3 accounts)**

| Property | Purpose | Chart of Account Example | Typical Code |
|----------|---------|--------------------------|--------------|
| `RoundOffAccountId` | Rounding adjustments | 5810 - Rounding Difference | 5810 |
| `ExchangeRateDifferenceAccountId` | Currency exchange gains/losses | 5820 - Exchange Rate Diff | 5820 |
| `YearEndClosingAccountId` | Year-end closing/retained earnings | 3210 - Retained Earnings | 3210 |

---

## 🔥 **CRITICAL ACCOUNTS FOR SALES POSTING**

### **Absolutely Required (Mandatory)**

```csharp
new AccountingSettings
{
    SalesAccountId = /* 4010 - Taxable Sales */,
    VatOutputAccountId = /* 2311 - VAT Output */,
    CashAccountId = /* 1110 - Cash */,
    BankAccountId = /* 1120 - Bank */,
    ReceivableAccountId = /* 1210 - Accounts Receivable */,
    CogsAccountId = /* 5010 - COGS */,
    InventoryAccountId = /* 1310 - Inventory */
}
```

### **Highly Recommended for Full Features**

```csharp
new AccountingSettings
{
    // ...mandatory accounts above...
    
    ZeroRatedSalesAccountId = /* 4020 - Zero-Rated Sales */,
    ExemptSalesAccountId = /* 4030 - Exempt Sales */,
    SalesDiscountAccountId = /* 4120 - Sales Discounts */
}
```

---

## 📋 **CHART OF ACCOUNTS TEMPLATE**

### **Saudi Arabia Standard (Sample)**

```
┌─ ASSETS (1XXX)
│  ├─ 1110  Cash on Hand
│  ├─ 1111  Petty Cash
│  ├─ 1115  POS Terminal
│  ├─ 1120  Bank Account
│  ├─ 1125  Cheques
│  ├─ 1210  Accounts Receivable
│  ├─ 1310  Inventory
│  ├─ 1311  Opening Stock
│  ├─ 1312  Closing Stock
│  ├─ 1315  Stock in Transit
│  ├─ 1410  Supplier Advances
│  └─ 1510  VAT Input (Purchases)
│
├─ LIABILITIES (2XXX)
│  ├─ 2210  Accounts Payable
│  ├─ 2310  VAT Payable
│  ├─ 2311  VAT Output (Sales)
│  ├─ 2312  VAT Settlement
│  ├─ 2315  VAT Suspense
│  ├─ 2320  WHT Payable
│  ├─ 2410  Deferred Revenue
│  ├─ 2420  Loyalty Points Liability
│  ├─ 2430  Gift Cards Payable
│  ├─ 2440  Customer Advances
│  ├─ 2450  Customer Refunds Payable
│  └─ 2510  Purchase Accrual
│
├─ EQUITY (3XXX)
│  └─ 3210  Retained Earnings
│
├─ REVENUE (4XXX)
│  ├─ 4010  Sales Revenue (Taxable)
│  ├─ 4015  Sales (No VAT)
│  ├─ 4020  Zero-Rated Sales
│  ├─ 4030  Exempt Sales
│  ├─ 4110  Sales Returns & Allowances
│  ├─ 4120  Sales Discounts
│  └─ 4510  Inventory Gain
│
└─ EXPENSES (5XXX)
   ├─ 5010  Cost of Goods Sold
   ├─ 5110  Purchase Returns
   ├─ 5120  Purchase Discounts
   ├─ 5210  Sales Commission
   ├─ 5220  Freight In
   ├─ 5230  Bank Fees
   ├─ 5310  Inventory Adjustment
   ├─ 5320  Inventory Loss
   ├─ 5330  Damaged Stock
   ├─ 5340  Expired Items
   ├─ 5410  Cash Over/Short
   ├─ 5420  Bad Debt Expense
   ├─ 5510  General Expenses
   ├─ 5610  Salaries
   ├─ 5710  Rent
   ├─ 5720  Electricity
   ├─ 5730  Internet
   ├─ 5810  Rounding Difference
   └─ 5820  Exchange Rate Diff
```

---

## 🧮 **ACCOUNT TYPE CLASSIFICATION**

### **Balance Sheet Accounts**

| Type | Accounts | Normal Balance |
|------|----------|----------------|
| **Assets** | Cash, Bank, Receivable, Inventory, Prepayments | **DEBIT** |
| **Liabilities** | Payable, VAT Output, Customer Advances, Gift Cards | **CREDIT** |
| **Equity** | Retained Earnings | **CREDIT** |

### **Income Statement Accounts**

| Type | Accounts | Normal Balance |
|------|----------|----------------|
| **Revenue** | Sales, Returns (contra) | **CREDIT** (DR for returns) |
| **COGS** | COGS, Inventory (contra) | **DEBIT** |
| **Expenses** | Discounts, Commissions, Utilities, Salaries | **DEBIT** |

---

## ✅ **VALIDATION CHECKLIST**

### **Pre-Configuration**
- [ ] Create Chart of Accounts in your accounting system
- [ ] Assign account codes matching your country's standard
- [ ] Set up account hierarchies (parent-child relationships)
- [ ] Define account types (Asset/Liability/Equity/Revenue/Expense)

### **Configuration**
- [ ] Map all 7 **CRITICAL** accounts (marked with CRITICAL above)
- [ ] Map 3 **RECOMMENDED** accounts for mixed VAT support
- [ ] Configure per branch (each branch can have different mappings)
- [ ] Test with sample transactions before go-live

### **Post-Configuration Testing**
- [ ] Post simple cash sale → verify all accounts debited/credited
- [ ] Post mixed VAT invoice → verify category separation
- [ ] Post partial payment → verify receivable + payment split
- [ ] Post return → verify reversals correct
- [ ] Review journal entry balance (Total DR = Total CR)

---

## 🔧 **SQL SCRIPT: INSERT SAMPLE CONFIGURATION**

```sql
-- Sample AccountingSettings for Branch (ADAPT ACCOUNT IDs)
INSERT INTO Accounting.AccountingSettings (
    Oid, BranchId, 
    SalesAccountId, ZeroRatedSalesAccountId, ExemptSalesAccountId,
    VatOutputAccountId, SalesDiscountAccountId,
    CashAccountId, BankAccountId, ReceivableAccountId,
    CogsAccountId, InventoryAccountId,
    CreatedAt
)
VALUES (
    NEWID(),
    '/* YOUR BRANCH ID */',
    '/* 4010 - Sales Revenue (Taxable) */',
    '/* 4020 - Zero-Rated Sales */',
    '/* 4030 - Exempt Sales */',
    '/* 2311 - VAT Output */',
    '/* 4120 - Sales Discounts */',
    '/* 1110 - Cash */',
    '/* 1120 - Bank */',
    '/* 1210 - Accounts Receivable */',
    '/* 5010 - COGS */',
    '/* 1310 - Inventory */',
    GETUTCDATE()
);
```

---

## 📞 **SUPPORT**

For questions about account mapping:
1. Consult your certified accountant
2. Reference your country's Chart of Accounts standard (e.g., SOCPA for Saudi Arabia)
3. Review existing ERP implementations (SAP, Odoo, QuickBooks)

---

**END OF QUICK REFERENCE**
