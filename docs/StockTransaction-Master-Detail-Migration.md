# StockTransaction Master-Detail Migration Guide

## ✅ Changes Summary

The `StockTransaction` entity has been refactored from a single-level entity to a **master-detail pattern**:

- **StockTransaction** = Header/Master (transaction metadata only)
- **StockTransactionDetail** = Lines/Details (individual products)

## 📋 What Changed

### **REMOVED from StockTransaction:**
- ❌ `ProductId` → Moved to `StockTransactionDetail`
- ❌ `Quantity` → Moved to `StockTransactionDetail`
- ❌ `UnitCost` → Moved to `StockTransactionDetail`
- ❌ `BatchNumber` → Moved to `StockTransactionDetail`
- ❌ `ExpiryDate` → Moved to `StockTransactionDetail`
- ❌ `Product` navigation → Moved to `StockTransactionDetail`

### **ADDED to StockTransaction:**
- ✅ `Details` collection → `ICollection<StockTransactionDetail>`
- ✅ `Status` → Transaction status (Draft, Approved, etc.)
- ✅ `ApprovedBy` → User who approved
- ✅ `ApprovedDate` → Approval timestamp

### **NEW Entity: StockTransactionDetail**
```csharp
- Oid (PK)
- StockTransactionId (FK)
- ProductId (FK)
- Quantity
- Gtin
- BatchNumber
- ExpiryDate
- SerialNumber
- UnitCost
- TotalCost
- LineNumber
- Notes
```

## 🔧 Migration Steps

### **Step 1: Database Migration**

Create a migration to:
1. Create `StockTransactionDetails` table
2. Remove columns from `StockTransactions`: ProductId, Quantity, UnitCost, BatchNumber, ExpiryDate
3. Add columns to `StockTransactions`: Status, ApprovedBy, ApprovedDate
4. Migrate existing data (see Step 2)

### **Step 2: Data Migration Script**

```sql
-- Migrate existing transaction data to master-detail
INSERT INTO StockTransactionDetails (
    Oid, 
    StockTransactionId, 
    ProductId, 
    Quantity, 
    BatchNumber, 
    ExpiryDate, 
    UnitCost,
    TotalCost,
    LineNumber,
    CreatedAt,
    CreatedBy,
    IsDeleted
)
SELECT 
    NEWID(),
    st.Oid,
    st.ProductId,
    st.Quantity,
    st.BatchNumber,
    st.ExpiryDate,
    st.UnitCost,
    st.Quantity * st.UnitCost,
    1,
    st.CreatedAt,
    st.CreatedBy,
    st.IsDeleted
FROM StockTransactions st
WHERE st.ProductId IS NOT NULL;

-- Drop old columns
ALTER TABLE StockTransactions DROP COLUMN ProductId;
ALTER TABLE StockTransactions DROP COLUMN Quantity;
ALTER TABLE StockTransactions DROP COLUMN UnitCost;
ALTER TABLE StockTransactions DROP COLUMN BatchNumber;
ALTER TABLE StockTransactions DROP COLUMN ExpiryDate;

-- Add new columns
ALTER TABLE StockTransactions ADD Status NVARCHAR(50);
ALTER TABLE StockTransactions ADD ApprovedBy UNIQUEIDENTIFIER;
ALTER TABLE StockTransactions ADD ApprovedDate DATETIME2;
```

### **Step 3: Update Existing Code**

#### **Files That Need Updates:**

1. **CreateSalesInvoiceHandler.cs**
2. **CreateStockTransferHandler.cs**
3. **CreateStockInHandler.cs**
4. **StockTransactionRepository.cs**
5. **StockTransactionProfile.cs** (AutoMapper)
6. **PharmacyDbContext.cs** (Configuration)
7. **PharmacySeeder.cs** (Seed data)

#### **Pattern for Updating:**

**BEFORE (Single Transaction):**
```csharp
var transaction = new StockTransaction
{
    Oid = Guid.NewGuid(),
    ProductId = productId,
    Quantity = 100,
    UnitCost = 5.00m,
    BatchNumber = "BATCH-001",
    ExpiryDate = DateTime.Now.AddYears(2),
    // ... other header fields
};
await _repository.AddAsync(transaction);
```

**AFTER (Master-Detail):**
```csharp
// Create header
var transaction = new StockTransaction
{
    Oid = Guid.NewGuid(),
    // ... header fields only (no product-specific data)
    Status = "Draft"
};

// Create detail line(s)
var detail = new StockTransactionDetail
{
    Oid = Guid.NewGuid(),
    StockTransactionId = transaction.Oid,
    ProductId = productId,
    Quantity = 100,
    UnitCost = 5.00m,
    TotalCost = 500.00m,
    BatchNumber = "BATCH-001",
    ExpiryDate = DateTime.Now.AddYears(2),
    LineNumber = 1
};

transaction.Details.Add(detail);
await _repository.AddAsync(transaction);
```

### **Step 4: Update DTOs**

Create new DTOs that include both header and details:

```csharp
public class StockTransactionWithDetailsDto
{
    // Header fields
    public Guid Oid { get; set; }
    public Guid? FromBranchId { get; set; }
    public Guid? ToBranchId { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? Status { get; set; }
    public decimal? TotalValue { get; set; }
    
    // Detail lines
    public List<StockTransactionDetailDto> Details { get; set; } = new();
}

public class CreateStockTransactionWithDetailsDto
{
    // Header fields
    public Guid? FromBranchId { get; set; }
    public Guid? ToBranchId { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Notes { get; set; }
    
    // Detail lines
    public List<CreateStockTransactionDetailDto> Details { get; set; } = new();
}
```

### **Step 5: Update Handlers**

Update all handlers to work with the master-detail pattern:

```csharp
public async Task<StockTransactionDto> Handle(CreateStockInCommand request, ...)
{
    // 1. Create header
    var transaction = new StockTransaction
    {
        ToBranchId = request.StockIn.BranchId,
        TransactionTypeId = inTypeId,
        TransactionDate = DateTime.UtcNow,
        ReferenceNumber = request.StockIn.ReferenceNumber,
        Status = "Draft"
    };

    // 2. Create detail lines
    int lineNumber = 1;
    foreach (var item in request.StockIn.Items)
    {
        var detail = new StockTransactionDetail
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitCost = item.UnitCost,
            TotalCost = item.Quantity * item.UnitCost,
            BatchNumber = item.BatchNumber,
            ExpiryDate = item.ExpiryDate,
            LineNumber = lineNumber++
        };
        transaction.Details.Add(detail);
    }

    // 3. Calculate total
    transaction.TotalValue = transaction.Details.Sum(d => d.TotalCost ?? 0);

    // 4. Save
    await _repository.AddAsync(transaction);
    
    return _mapper.Map<StockTransactionDto>(transaction);
}
```

### **Step 6: Update Repository Methods**

```csharp
public async Task<StockTransaction?> GetByIdAsync(Guid id, ...)
{
    return await _dbSet
        .Include(t => t.Details)  // Include details collection
            .ThenInclude(d => d.Product)
        .Include(t => t.FromBranch)
        .Include(t => t.ToBranch)
        .Where(t => t.Oid == id && !t.IsDeleted)
        .FirstOrDefaultAsync(cancellationToken);
}
```

### **Step 7: Update AutoMapper Profile**

```csharp
public class StockTransactionProfile : Profile
{
    public StockTransactionProfile()
    {
        CreateMap<StockTransaction, StockTransactionDto>()
            .ForMember(dest => dest.FromBranchName,
                opt => opt.MapFrom(src => src.FromBranch != null ? src.FromBranch.BranchName : null))
            .ForMember(dest => dest.ToBranchName,
                opt => opt.MapFrom(src => src.ToBranch != null ? src.ToBranch.BranchName : null))
            .ForMember(dest => dest.Details,
                opt => opt.MapFrom(src => src.Details));
    }
}
```

## 📝 Benefits of Master-Detail Pattern

✅ **Multiple Products per Transaction** - One transaction can now have many products  
✅ **Better Data Normalization** - Header data not repeated for each product  
✅ **Easier Reporting** - Separate header totals from line items  
✅ **Batch Operations** - Process multiple products in one transaction  
✅ **Audit Trail** - Track changes at both header and line level  
✅ **Scalability** - Handle complex transactions with many items  

## 🚨 Important Notes

1. **Backward Compatibility**: Old code will break - this is a breaking change
2. **Database Migration**: Must migrate existing data before deploying
3. **API Changes**: Existing API contracts will change
4. **Testing**: Thoroughly test all stock transaction operations
5. **Rollback Plan**: Keep backup of old schema and code

## 📊 Example Request

**POST `/api/StockTransaction`** (New Format):
```json
{
  "fromBranchId": "branch-guid-1",
  "toBranchId": "branch-guid-2",
  "transactionTypeId": "transfer-type-guid",
  "referenceNumber": "TRF-2026-001",
  "transactionDate": "2026-02-27T12:00:00Z",
  "notes": "Monthly stock transfer",
  "details": [
    {
      "productId": "product-guid-1",
      "quantity": 100,
      "unitCost": 5.00,
      "batchNumber": "BATCH-001",
      "expiryDate": "2028-02-27",
      "lineNumber": 1
    },
    {
      "productId": "product-guid-2",
      "quantity": 50,
      "unitCost": 10.00,
      "batchNumber": "BATCH-002",
      "expiryDate": "2027-12-31",
      "lineNumber": 2
    }
  ]
}
```

## ✅ Checklist

- [ ] Run database migration script
- [ ] Update all handlers referencing StockTransaction
- [ ] Update repository methods
- [ ] Update AutoMapper profiles
- [ ] Update seed data
- [ ] Update API controllers
- [ ] Update DTOs
- [ ] Run unit tests
- [ ] Run integration tests
- [ ] Update API documentation
- [ ] Update frontend code (if applicable)

---

This is a major refactoring. Take it step by step and test thoroughly!
