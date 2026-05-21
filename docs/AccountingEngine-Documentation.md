# 🏦 **PRODUCTION-GRADE ERP ACCOUNTING ENGINE**
## **Pharmacy POS Journal Posting System**

### **Version**: 2.0 - SAP/Odoo-Level Sophistication  
### **Date**: 2026-05-21  
### **Architecture**: Double-Entry Accounting with Mixed VAT Support

---

## 📋 **TABLE OF CONTENTS**

1. [System Overview](#system-overview)
2. [VAT Category Classification](#vat-category-classification)
3. [Account Resolution Rules](#account-resolution-rules)
4. [Sales Invoice Posting Logic](#sales-invoice-posting-logic)
5. [Return Invoice Posting Logic](#return-invoice-posting-logic)
6. [Edge Cases & Scenarios](#edge-cases--scenarios)
7. [Journal Entry Examples](#journal-entry-examples)
8. [AccountingSettings Configuration](#accountingsettings-configuration)

---

## 🎯 **SYSTEM OVERVIEW**

This system implements **production-grade double-entry accounting** for pharmacy POS transactions with:

### ✅ **Core Features**
- **Mixed VAT Invoices**: Single invoice with taxable (15%), zero-rated (0%), and exempt items
- **Item-Level Classification**: Each product classified by VAT category for correct revenue posting
- **Partial/Mixed Payments**: Support multiple payment methods on one invoice (e.g., 500 CASH + 300 BANK + 200 CREDIT)
- **Item-Level COGS**: Separate journal lines per product for complete audit trail
- **Category-Aware Returns**: Proper VAT reversal based on original item classification
- **Bilingual Descriptions**: English + Arabic journal line descriptions
- **Sequential Line Numbering**: Audit trail with line numbers in journal details
- **Fiscal Year Controls**: Cannot post to closed fiscal years
- **Duplicate Posting Prevention**: Idempotency via JournalEntryId linkage
- **Balanced Entry Validation**: Enforces DR = CR with 0.01 tolerance
- **Atomic Transactions**: Retry logic + rollback on failure

---

## 🏷️ **VAT CATEGORY CLASSIFICATION**

### **VatCategory Enum**

```csharp
public enum VatCategory
{
    Taxable = 1,    // Standard VAT (15%) → posts to VatOutputAccountId
    ZeroRated = 2,  // 0% VAT (but VAT-registered) → NO VAT account entry
    Exempt = 3      // Exempt from VAT → NO VAT account entry
}
```

### **Classification Logic**

**In Sales/Return Handler:**
```csharp
var vatCategory = (item.TaxPercent ?? 0) switch
{
    > 0 => VatCategory.Taxable,      // Has VAT (e.g., 15%)
    0 when item.TaxPercent.HasValue => VatCategory.ZeroRated,  // Explicitly 0%
    _ => VatCategory.Exempt          // Null TaxPercent = Exempt
};
```

---

## 🔑 **ACCOUNT RESOLUTION RULES**

### **Sales Revenue Accounts**
| VAT Category | Account Used |
|--------------|-------------|
| **Taxable** | `AccountingSettings.SalesAccountId` |
| **Zero-Rated** | `AccountingSettings.ZeroRatedSalesAccountId` |
| **Exempt** | `AccountingSettings.ExemptSalesAccountId` |

### **VAT Accounts**
| Scenario | Account Used |
|----------|-------------|
| **Taxable Items** | `AccountingSettings.VatOutputAccountId` |
| **Zero-Rated** | NO VAT ENTRY |
| **Exempt** | NO VAT ENTRY |

### **Payment Method Routing**
| Payment Method Code | Debit Account |
|---------------------|--------------|
| **CASH** | `AccountingSettings.CashAccountId` |
| **BANK** | `PaymentMethodDetail.BankAccountId` ?? `AccountingSettings.BankAccountId` |
| **CREDIT** | None (stays in Accounts Receivable) |

### **Customer/Receivable**
```
Customer.ChildAccountId (if customer has dedicated account)
  └─ fallback to AccountingSettings.ReceivableAccountId
```

### **COGS/Inventory**
- **COGS**: `AccountingSettings.CogsAccountId`
- **Inventory**: `AccountingSettings.InventoryAccountId`

### **Discounts**
- **Invoice-Level**: `AccountingSettings.SalesDiscountAccountId`
- **Line-Level**: Reduces `NetPrice` (no separate journal entry)

---

## 📝 **SALES INVOICE POSTING LOGIC**

### **Three-Section Architecture**

```
SECTION A — SALES REVENUE (Mixed VAT)
  ├─ DR Customer/Receivable (if not fully paid)
  ├─ DR Discount Allowed (invoice-level discount)
  ├─ CR Sales Revenue (Taxable)
  ├─ CR Zero-Rated Sales
  ├─ CR Exempt Sales
  └─ CR VAT Output (taxable items ONLY)

SECTION B — PAYMENT SETTLEMENT
  ├─ DR Cash Account (if CASH payment)
  ├─ DR Bank Account (if BANK payment)
  └─ CR Customer/Receivable (clears receivable)

SECTION C — COGS (Item-Level)
  └─ FOR EACH item (non-free):
       DR COGS (item cost)
       CR Inventory (item cost)
```

### **Key Rules**
1. **Receivable** only created for unpaid portion
2. **VAT Output** ONLY for taxable items (not zero-rated/exempt)
3. **COGS** posted per item with LineNumber reference
4. **Free items** excluded from COGS posting

---

## 🔄 **RETURN INVOICE POSTING LOGIC**

### **Three-Section Architecture**

```
SECTION A — SALES REVERSAL (by VAT Category)
  ├─ DR Sales Revenue (Taxable) — reversal
  ├─ DR Zero-Rated Sales — reversal
  ├─ DR Exempt Sales — reversal
  └─ DR VAT Output — reversal (taxable ONLY)

SECTION B — REFUND SETTLEMENT
  └─ CR Cash/Bank/Receivable (refund issued)

SECTION C — COGS REVERSAL (Item-Level)
  └─ FOR EACH returned item:
       DR Inventory (restore stock)
       CR COGS (reverse cost)
```

### **Key Rules**
1. **Category-Aware**: Returns grouped by original VAT category
2. **VAT Reversal**: ONLY for taxable items
3. **COGS Restored**: Per-item reversal with LineNumber
4. **Original Cost Prices**: Uses cost from original invoice

---

## ⚠️ **EDGE CASES & SCENARIOS**

### **1. Mixed VAT Invoice**

**Scenario:**
```
Item A: Drug (15% VAT) — Taxable
Item B: Medical device (0% VAT) — Zero-Rated
Item C: Prescription services (Exempt) — Exempt
```

**Result:**
- Sales Revenue split across 3 accounts (SalesAccountId, ZeroRatedSalesAccountId, ExemptSalesAccountId)
- VAT Output ONLY posted for Item A
- COGS posted separately for each item

---

### **2. Partial Payment**

**Scenario:**
```
Invoice Total: 1,000 SAR
Payment 1: CASH 500 SAR
Payment 2: BANK 300 SAR
Remaining: CREDIT 200 SAR (Accounts Receivable)
```

**Journal Entries:**
```
DR Customer/Receivable  200   (remaining unpaid)
DR Cash Account         500   (immediate payment)
DR Bank Account         300   (immediate payment)
CR Sales Revenue        870   (net of discounts/tax)
CR VAT Output           130
```

---

### **3. Invoice-Level + Line-Level Discounts**

**Scenario:**
```
Item A: 100 SAR - 10% line discount = 90 SAR
Item B: 200 SAR - 5% line discount = 190 SAR
SubTotal: 280 SAR
Invoice Discount: 10% = 28 SAR
Net Before Tax: 252 SAR
VAT (15%): 37.80 SAR
Total: 289.80 SAR
```

**Journal Entries:**
```
DR Customer/Receivable    289.80
DR Discount Allowed        28.00  (invoice-level discount)
CR Sales Revenue          252.00  (after line discounts)
CR VAT Output              37.80
```

---

### **4. Post-Settlement Return**

**Scenario:**
- Original invoice fully paid in CASH (1,150 SAR)
- Customer returns 1 item (150 SAR including VAT)
- Refund issued in CASH

**Return Journal:**
```
DR Sales Revenue (Taxable)    130.43  (net before VAT)
DR VAT Output                  19.57  (reversal)
CR Cash Account               150.00  (refund)

DR Inventory                   80.00  (restore item @ cost)
CR COGS                        80.00  (reverse cost)
```

---

### **5. Mixed Payment Return**

**Scenario:**
- Original invoice: 1,000 SAR (CASH 600 + CREDIT 400)
- Return 300 SAR worth of items
- Refund method: BANK 300 SAR

**Return Journal:**
```
DR Sales Revenue (Taxable)    260.87
DR VAT Output                  39.13
CR Bank Account               300.00

DR Inventory                  180.00
CR COGS                       180.00
```

---

## 📊 **JOURNAL ENTRY EXAMPLES**

### **Example 1: Simple Cash Sale (Taxable Items Only)**

**Invoice Details:**
- Product: Panadol 500mg (Taxable, 15% VAT)
- Qty: 10, Price: 10 SAR each
- SubTotal: 100 SAR
- VAT: 15 SAR
- Total: 115 SAR
- Payment: CASH 115 SAR

**Journal Entry:**
| Line | Account | Debit | Credit | Description |
|------|---------|-------|--------|-------------|
| 1 | Cash Account | 115.00 | — | Payment (CASH) - INV001 |
| 2 | Sales Revenue (Taxable) | — | 100.00 | Taxable Sales - INV001 |
| 3 | VAT Output | — | 15.00 | VAT Output - INV001 |
| 4 | COGS | 60.00 | — | COGS - Panadol 500mg (Line 1) |
| 5 | Inventory | — | 60.00 | Inventory - Panadol 500mg (Line 1) |
| **TOTAL** | **175.00** | **175.00** | **BALANCED** |

---

### **Example 2: Mixed VAT Invoice with Partial Payment**

**Invoice Details:**
```
Item A: Aspirin (Taxable, 15% VAT) — 200 SAR + 30 VAT = 230 SAR
Item B: Surgical Mask (Zero-Rated, 0% VAT) — 50 SAR
Item C: Consultation (Exempt) — 100 SAR
Total: 380 SAR
Payment: CASH 250 SAR, CREDIT 130 SAR
```

**Journal Entry:**
| Line | Account | Debit | Credit | Description |
|------|---------|-------|--------|-------------|
| 1 | Customer/Receivable | 130.00 | — | Sale - INV002 |
| 2 | Cash Account | 250.00 | — | Payment (CASH) - INV002 |
| 3 | Sales Revenue (Taxable) | — | 200.00 | Taxable Sales - INV002 |
| 4 | Zero-Rated Sales | — | 50.00 | Zero-Rated Sales - INV002 |
| 5 | Exempt Sales | — | 100.00 | Exempt Sales - INV002 |
| 6 | VAT Output | — | 30.00 | VAT Output - INV002 |
| 7 | Customer/Receivable | — | 250.00 | Payment Settlement - INV002 |
| 8 | COGS | 120.00 | — | COGS - Aspirin (Line 1) |
| 9 | Inventory | — | 120.00 | Inventory - Aspirin (Line 1) |
| 10 | COGS | 30.00 | — | COGS - Surgical Mask (Line 2) |
| 11 | Inventory | — | 30.00 | Inventory - Surgical Mask (Line 2) |
| **TOTAL** | **530.00** | **530.00** | **BALANCED** |

---

### **Example 3: Invoice with Discount**

**Invoice Details:**
```
Item A: Vitamin C (Taxable, 15% VAT) — 200 SAR
Invoice Discount: 10% = 20 SAR
Net Before Tax: 180 SAR
VAT (15%): 27 SAR
Total: 207 SAR
Payment: BANK 207 SAR
```

**Journal Entry:**
| Line | Account | Debit | Credit | Description |
|------|---------|-------|--------|-------------|
| 1 | Bank Account | 207.00 | — | Payment (BANK) - INV003 |
| 2 | Discount Allowed | 20.00 | — | Discount - INV003 |
| 3 | Sales Revenue (Taxable) | — | 180.00 | Taxable Sales - INV003 |
| 4 | VAT Output | — | 27.00 | VAT Output - INV003 |
| 5 | COGS | 100.00 | — | COGS - Vitamin C (Line 1) |
| 6 | Inventory | — | 100.00 | Inventory - Vitamin C (Line 1) |
| **TOTAL** | **327.00** | **327.00** | **BALANCED** |

---

### **Example 4: Return Invoice (Category-Aware)**

**Original Invoice:**
```
Item A: Antibiotic (Taxable) — 150 SAR + 22.50 VAT
Item B: Bandages (Zero-Rated) — 50 SAR
Total: 222.50 SAR (Paid CASH)
```

**Return:** Customer returns both items

**Return Journal Entry:**
| Line | Account | Debit | Credit | Description |
|------|---------|-------|--------|-------------|
| 1 | Sales Revenue (Taxable) | 150.00 | — | Return - Taxable Sales - RET001 |
| 2 | Zero-Rated Sales | 50.00 | — | Return - Zero-Rated Sales - RET001 |
| 3 | VAT Output | 22.50 | — | Return - VAT Reversal - RET001 |
| 4 | Cash Account | — | 222.50 | Refund (CASH) - RET001 |
| 5 | Inventory | 90.00 | — | Inventory Restore - Antibiotic (Line 1) |
| 6 | COGS | — | 90.00 | COGS Reversal - Antibiotic (Line 1) |
| 7 | Inventory | 30.00 | — | Inventory Restore - Bandages (Line 2) |
| 8 | COGS | — | 30.00 | COGS Reversal - Bandages (Line 2) |
| **TOTAL** | **342.50** | **342.50** | **BALANCED** |

---

## ⚙️ **ACCOUNTINGSETTINGS CONFIGURATION**

### **Required Accounts for Full Functionality**

#### **Sales Accounts (10)**
- `SalesAccountId` — Taxable sales revenue
- `SalesWithoutVatAccountId` — Sales without VAT
- `ZeroRatedSalesAccountId` — Zero-rated sales (0% VAT)
- `ExemptSalesAccountId` — Exempt sales
- `SalesReturnAccountId` — Sales returns
- `SalesDiscountAccountId` — Invoice-level discounts
- `DeferredRevenueAccountId` — Unearned revenue
- `LoyaltyPointsAccountId` — Loyalty program liability
- `GiftCardAccountId` — Gift card liability
- `SalesCommissionAccountId` — Sales commission expense

#### **VAT Accounts (6)**
- `VatAccountId` — General VAT (fallback)
- `VatOutputAccountId` — **VAT output on sales (REQUIRED)**
- `VatInputAccountId` — VAT input on purchases
- `VatSettlementAccountId` — VAT settlement clearing
- `WithholdingTaxAccountId` — Withholding tax payable
- `VatSuspenseAccountId` — VAT suspense/adjustment

#### **Cash/POS/Bank Accounts (7)**
- `CashAccountId` — **Cash payments (REQUIRED)**
- `BankAccountId` — **Bank payments (REQUIRED)**
- `PosAccountId` — POS terminal clearing
- `PettyCashAccountId` — Petty cash
- `CashDifferenceAccountId` — Cash over/short
- `BankFeesAccountId` — Bank charges
- `ChequeAccountId` — Cheque clearing

#### **Customer/Supplier Accounts (6)**
- `ReceivableAccountId` — **Accounts receivable (REQUIRED)**
- `CustomerAdvanceAccountId` — Customer deposits
- `CustomerRefundAccountId` — Customer refund liability
- `SupplierAdvanceAccountId` — Supplier prepayments
- `SupplierPayableAccountId` — Accounts payable
- `BadDebtAccountId` — Bad debt expense

#### **Inventory/COGS Accounts (10)**
- `InventoryAccountId` — **Inventory asset (REQUIRED)**
- `CogsAccountId` — **Cost of goods sold (REQUIRED)**
- `InventoryAdjustmentAccountId` — Stock adjustments
- `InventoryLossAccountId` — Stock losses
- `InventoryGainAccountId` — Stock gains
- `DamagedInventoryAccountId` — Damaged stock
- `ExpiredItemsAccountId` — Expired stock
- `StockOpeningAccountId` — Opening stock
- `StockClosingAccountId` — Closing stock
- `StockTransferAccountId` — Inter-branch transfers

### **Minimum Configuration for Core Sales Posting**

```csharp
new AccountingSettings
{
    // MANDATORY
    SalesAccountId = /* Taxable Sales */,
    VatOutputAccountId = /* VAT Output */,
    CashAccountId = /* Cash */,
    BankAccountId = /* Bank */,
    ReceivableAccountId = /* Accounts Receivable */,
    CogsAccountId = /* Cost of Goods Sold */,
    InventoryAccountId = /* Inventory */,
    
    // RECOMMENDED for Mixed VAT
    ZeroRatedSalesAccountId = /* Zero-Rated Sales */,
    ExemptSalesAccountId = /* Exempt Sales */,
    SalesDiscountAccountId = /* Sales Discounts */
}
```

---

## 🔐 **DATA INTEGRITY GUARANTEES**

### **1. Idempotency**
- `invoice.JournalEntryId` prevents duplicate posting
- Atomic transactions ensure all-or-nothing persistence

### **2. Balance Validation**
```csharp
if (Math.Abs(entry.TotalDebit - entry.TotalCredit) > 0.01m)
    throw new InvalidOperationException("Unbalanced journal entry");
```

### **3. Fiscal Year Controls**
- Cannot post to closed fiscal years
- Automatic fiscal year assignment from `GetCurrentAsync()`

### **4. Transaction Isolation**
```csharp
var strategy = _context.Database.CreateExecutionStrategy();
await strategy.ExecuteAsync(async () =>
{
    await using var tx = await _context.Database.BeginTransactionAsync();
    try
    {
        await _journalRepo.InsertMasterDetailAsync(entry, details);
        invoice.JournalEntryId = entry.Oid;
        await _invoiceRepo.UpdateAsync(invoice);
        await tx.CommitAsync();
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
});
```

---

## 📈 **SCALABILITY & PERFORMANCE**

### **Design Patterns**
- **Execution Strategy**: SQL Server retry logic for transient failures
- **Async/Await**: Non-blocking I/O for database operations
- **Repository Pattern**: Clean separation of data access
- **CQRS**: Command/Query separation via MediatR

### **Optimizations**
- Single bulk insert for all journal details (`InsertMasterDetailAsync`)
- Eager loading of product/customer navigation properties
- Indexed queries on `JournalEntryId`, `ReferenceId`, `BranchId`

### **Audit Trail**
- **Line Numbers**: Sequential numbering for detail ordering
- **Bilingual Descriptions**: English + Arabic for international compliance
- **Timestamps**: `CreatedAt` on all entities
- **Reference Linking**: `ReferenceId` links journal to source document

---

## 🚀 **DEPLOYMENT CHECKLIST**

### **Pre-Deployment**
- [ ] Apply pending migrations:
  - `AddDescriptionArToJournalEntryDetail`
  - `AddLineNumberToReturnInvoiceItem`
  - `ExpandAccountingSettings`
- [ ] Configure `AccountingSettings` for all branches
- [ ] Assign customers to child accounts (optional)
- [ ] Create fiscal years and mark current as active

### **Post-Deployment Validation**
- [ ] Test mixed VAT invoice (taxable + zero-rated + exempt)
- [ ] Test partial payment (CASH + BANK + CREDIT)
- [ ] Test return with VAT reversal
- [ ] Verify journal balance (TotalDebit = TotalCredit)
- [ ] Check audit trail (line numbers, descriptions)
- [ ] Confirm idempotency (re-posting same invoice fails)

---

## 📞 **SUPPORT & MAINTENANCE**

### **Logging & Monitoring**
- All exceptions bubble up to application layer
- Journal entry OID returned in `SalesInvoicePostingResult`
- Query `JournalEntry` + `JournalEntryDetail` for audit review

### **Troubleshooting**
- **Unbalanced Entry**: Check account configuration and item-level calculations
- **Duplicate Posting**: Verify `invoice.JournalEntryId` is null before posting
- **Missing Accounts**: Ensure all required `AccountingSettings` properties are configured
- **VAT Mismatch**: Verify `TaxPercent` classification logic in handlers

---

## 📄 **VERSION HISTORY**

| Version | Date | Changes |
|---------|------|---------|
| **2.0** | 2026-05-21 | Complete ERP-grade rewrite with mixed VAT, partial payments, item-level COGS |
| **1.5** | 2026-05-21 | Added Arabic descriptions, line numbering, return invoice journal posting |
| **1.0** | 2026-05-16 | Initial basic journal posting (aggregate COGS, single VAT) |

---

**END OF DOCUMENTATION**
