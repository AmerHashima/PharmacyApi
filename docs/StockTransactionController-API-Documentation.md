# ✅ StockTransactionController - Full CRUD Implementation

## 🎯 Complete Master-Detail Stock Transaction System

A comprehensive REST API for managing stock transactions with master-detail pattern.

---

## 📡 API Endpoints

### **1. GET Stock Transaction by ID**
**GET** `/api/StockTransaction/{id}`

Retrieves transaction header with all detail lines.

**Response:**
```json
{
  "success": true,
  "message": "Stock transaction retrieved successfully",
  "data": {
    "oid": "transaction-guid",
    "transactionTypeId": "type-guid",
    "transactionTypeName": "IN",
    "fromBranchId": null,
    "toBranchId": "branch-1-guid",
    "toBranchName": "Main Pharmacy",
    "referenceNumber": "STK-IN-001",
    "transactionDate": "2026-02-27T12:00:00Z",
    "totalValue": 1500.00,
    "status": "Completed",
    "supplierName": "Global Pharma",
    "details": [
      {
        "oid": "detail-1-guid",
        "productId": "product-1-guid",
        "productName": "Paracetamol 500mg",
        "quantity": 100,
        "unitCost": 5.00,
        "totalCost": 500.00,
        "batchNumber": "BATCH-001",
        "expiryDate": "2028-02-27",
        "lineNumber": 1
      },
      {
        "oid": "detail-2-guid",
        "productId": "product-2-guid",
        "productName": "Aspirin 100mg",
        "quantity": 200,
        "unitCost": 5.00,
        "totalCost": 1000.00,
        "batchNumber": "BATCH-002",
        "lineNumber": 2
      }
    ]
  }
}
```

---

### **2. POST Query Stock Transactions**
**POST** `/api/StockTransaction/query`

Advanced filtering, sorting, and pagination.

**Request:**
```json
{
  "request": {
    "filters": [
      {
        "propertyName": "Status",
        "value": "Completed",
        "operation": 0
      },
      {
        "propertyName": "TransactionDate",
        "value": "2026-02-01",
        "operation": 5,
        "logicalOperator": 0
      },
      {
        "propertyName": "FromBranchId",
        "value": "branch-guid",
        "operation": 0
      }
    ],
    "sort": [
      {
        "sortBy": "TransactionDate",
        "sortDirection": "desc"
      }
    ],
    "pagination": {
      "pageNumber": 1,
      "pageSize": 10,
      "getAll": false
    },
    "columns": []
  }
}
```

**Response:**
```json
{
  "success": true,
  "message": "Stock transaction data retrieved successfully",
  "data": {
    "data": [ /* list of transactions */ ],
    "totalRecords": 25,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 3
  }
}
```

---

### **3. POST Create Transaction with Details** ⭐
**POST** `/api/StockTransaction`

Create transaction header with multiple product lines in one request.

**Request:**
```json
{
  "transactionTypeId": "type-guid",
  "fromBranchId": "branch-1-guid",
  "toBranchId": "branch-2-guid",
  "transactionDate": "2026-02-27T12:00:00Z",
  "referenceNumber": "TRF-2026-001",
  "notificationId": "NOTIF-001",
  "supplierId": null,
  "status": "Draft",
  "notes": "Monthly stock transfer",
  "details": [
    {
      "productId": "product-1-guid",
      "quantity": 100,
      "gtin": "6281086011508",
      "batchNumber": "BATCH-001",
      "expiryDate": "2028-02-27",
      "serialNumber": "SN123456",
      "unitCost": 5.00,
      "totalCost": 500.00,
      "lineNumber": 1,
      "notes": "First item"
    },
    {
      "productId": "product-2-guid",
      "quantity": 50,
      "unitCost": 10.00,
      "totalCost": 500.00,
      "batchNumber": "BATCH-002",
      "expiryDate": "2027-12-31",
      "lineNumber": 2
    }
  ]
}
```

**Response:**
```json
{
  "success": true,
  "message": "Stock transaction created successfully",
  "data": {
    "oid": "new-transaction-guid",
    "referenceNumber": "TRF-2026-001",
    "totalValue": 1000.00,
    "status": "Draft",
    "details": [ /* 2 detail items */ ]
  }
}
```

---

### **4. PUT Update Transaction with Details**
**PUT** `/api/StockTransaction/{id}`

Update transaction header and replace all detail lines.

**Request:** (Same structure as POST)

**Note:** This replaces ALL existing detail lines. To add/remove individual lines, use `/api/StockTransactionDetail` endpoints.

---

### **5. DELETE Stock Transaction**
**DELETE** `/api/StockTransaction/{id}`

Soft delete transaction and all its detail lines.

**Response:**
```json
{
  "success": true,
  "message": "Stock transaction deleted successfully"
}
```

---

### **6. POST Approve Transaction** 🆕
**POST** `/api/StockTransaction/{id}/approve`

Approve a draft transaction (placeholder for future implementation).

---

### **7. POST Cancel Transaction** 🆕
**POST** `/api/StockTransaction/{id}/cancel`

Cancel a transaction (placeholder for future implementation).

---

## 📊 Transaction Types

Common transaction types from `TRANSACTION_TYPE` lookup:

| Type | Code | From Branch | To Branch | Use Case |
|------|------|-------------|-----------|----------|
| **IN** | `IN` | ❌ | ✅ | Receiving from supplier |
| **OUT** | `OUT` | ✅ | ❌ | Dispensing/Sales |
| **TRANSFER** | `TRANSFER` | ✅ | ✅ | Inter-branch transfer |
| **ADJUSTMENT** | `ADJUSTMENT` | ✅ | ❌ | Stock correction |
| **RETURN** | `RETURN` | ❌ | ✅ | Return from customer |

---

## 🔄 Workflow Examples

### **Example 1: Stock Receiving (IN)**

```json
POST /api/StockTransaction
{
  "transactionTypeId": "in-type-guid",
  "toBranchId": "main-pharmacy-guid",
  "supplierId": "supplier-guid",
  "transactionDate": "2026-02-27T10:00:00Z",
  "referenceNumber": "PO-2026-001",
  "status": "Completed",
  "details": [
    {
      "productId": "paracetamol-guid",
      "quantity": 500,
      "unitCost": 3.50,
      "totalCost": 1750.00,
      "batchNumber": "BATCH-001",
      "expiryDate": "2028-02-27",
      "gtin": "6281086011508",
      "lineNumber": 1
    },
    {
      "productId": "aspirin-guid",
      "quantity": 200,
      "unitCost": 8.00,
      "totalCost": 1600.00,
      "batchNumber": "BATCH-002",
      "expiryDate": "2027-12-31",
      "lineNumber": 2
    }
  ]
}
```

**Result:** 
- ✅ Transaction created
- ✅ 2 detail lines created
- ✅ Total value calculated: 3350.00
- ✅ Stock increased at Main Pharmacy (if auto-update enabled)

---

### **Example 2: Stock Transfer**

```json
POST /api/StockTransaction
{
  "transactionTypeId": "transfer-type-guid",
  "fromBranchId": "main-pharmacy-guid",
  "toBranchId": "branch-2-guid",
  "transactionDate": "2026-02-27T14:00:00Z",
  "referenceNumber": "TRF-2026-001",
  "status": "Draft",
  "notes": "Monthly rebalancing",
  "details": [
    {
      "productId": "product-guid",
      "quantity": 50,
      "batchNumber": "BATCH-001",
      "lineNumber": 1
    }
  ]
}
```

**Result:**
- ✅ Transfer transaction created
- ✅ Stock decreased at Main Pharmacy
- ✅ Stock increased at Branch 2

---

### **Example 3: Query Transactions**

```json
POST /api/StockTransaction/query
{
  "request": {
    "filters": [
      {
        "propertyName": "Status",
        "value": "Completed",
        "operation": 0
      },
      {
        "propertyName": "ToBranchId",
        "value": "branch-guid",
        "operation": 0,
        "logicalOperator": 0
      }
    ],
    "sort": [
      {
        "sortBy": "TransactionDate",
        "sortDirection": "desc"
      }
    ],
    "pagination": {
      "pageNumber": 1,
      "pageSize": 20
    }
  }
}
```

---

## 🏗️ Complete Architecture

```
┌──────────────────────────────────────────┐
│      StockTransactionController          │
│  ┌────────────────────────────────────┐  │
│  │ POST   /api/StockTransaction       │  │
│  │ GET    /api/StockTransaction/{id}  │  │
│  │ PUT    /api/StockTransaction/{id}  │  │
│  │ DELETE /api/StockTransaction/{id}  │  │
│  │ POST   /query (filtering/paging)   │  │
│  │ POST   /{id}/approve               │  │
│  │ POST   /{id}/cancel                │  │
│  └────────────────────────────────────┘  │
└──────────────────┬───────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────┐
│           MediatR Commands/Queries        │
│  - CreateStockTransactionWithDetails     │
│  - UpdateStockTransactionWithDetails     │
│  - DeleteStockTransaction                │
│  - GetStockTransactionById               │
│  - GetStockTransactionData (query)       │
└──────────────────┬───────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────┐
│              Handlers                     │
│  - CreateStockTransactionWithDetails     │
│  - UpdateStockTransactionWithDetails     │
│  - DeleteStockTransaction                │
│  - GetStockTransactionById               │
│  - GetStockTransactionData               │
└──────────────────┬───────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────┐
│           Repositories                    │
│  - IStockTransactionRepository           │
│  - IStockTransactionDetailRepository     │
│  - IProductRepository                    │
│  - IBranchRepository                     │
└──────────────────┬───────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────┐
│           Database                        │
│  ┌────────────────────────────────────┐  │
│  │ StockTransactions (Header)         │  │
│  │  - Oid, ReferenceNumber, Date...   │  │
│  └────────────────┬───────────────────┘  │
│                   │ 1:N                   │
│  ┌────────────────▼───────────────────┐  │
│  │ StockTransactionDetails (Lines)    │  │
│  │  - Oid, ProductId, Quantity...     │  │
│  └────────────────────────────────────┘  │
└──────────────────────────────────────────┘
```

---

## 📋 Files Created (15 files)

### **DTOs:**
1. ✅ `CreateStockTransactionWithDetailsDto.cs`
2. ✅ `UpdateStockTransactionWithDetailsDto.cs`
3. ✅ `StockTransactionWithDetailsDto.cs`

### **Commands:**
4. ✅ `CreateStockTransactionWithDetailsCommand.cs`
5. ✅ `UpdateStockTransactionWithDetailsCommand.cs`
6. ✅ `DeleteStockTransactionCommand.cs`

### **Queries:**
7. ✅ `GetStockTransactionByIdQuery.cs`

### **Handlers:**
8. ✅ `CreateStockTransactionWithDetailsHandler.cs`
9. ✅ `UpdateStockTransactionWithDetailsHandler.cs`
10. ✅ `DeleteStockTransactionHandler.cs`
11. ✅ `GetStockTransactionByIdHandler.cs`

### **Mapping:**
12. ✅ Updated `StockTransactionProfile.cs`

### **API:**
13. ✅ `StockTransactionController.cs`

### **Documentation:**
14. ✅ This file

---

## 🧪 Testing Examples

### **Test 1: Create Stock IN Transaction**

```bash
curl -X POST "https://localhost:5001/api/StockTransaction" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
  "transactionTypeId": "in-type-guid",
  "toBranchId": "branch-guid",
  "supplierId": "supplier-guid",
  "transactionDate": "2026-02-27T10:00:00Z",
  "referenceNumber": "STK-IN-001",
  "status": "Completed",
  "details": [
    {
      "productId": "product-guid",
      "quantity": 100,
      "unitCost": 5.00,
      "batchNumber": "BATCH-001",
      "expiryDate": "2028-02-27",
      "lineNumber": 1
    }
  ]
}'
```

### **Test 2: Query Transactions**

```bash
curl -X POST "https://localhost:5001/api/StockTransaction/query" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
  "request": {
    "filters": [
      {
        "propertyName": "Status",
        "value": "Completed",
        "operation": 0
      }
    ],
    "pagination": {
      "pageNumber": 1,
      "pageSize": 10
    }
  }
}'
```

### **Test 3: Get Transaction by ID**

```bash
curl -X GET "https://localhost:5001/api/StockTransaction/{transaction-guid}" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### **Test 4: Update Transaction**

```bash
curl -X PUT "https://localhost:5001/api/StockTransaction/{transaction-guid}" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
  "oid": "transaction-guid",
  "status": "Approved",
  "details": [ /* updated detail lines */ ]
}'
```

### **Test 5: Delete Transaction**

```bash
curl -X DELETE "https://localhost:5001/api/StockTransaction/{transaction-guid}" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## 💡 Key Features

### **✅ Master-Detail Pattern**
- Create header with multiple detail lines in one request
- Automatic total calculation
- Cascading soft delete

### **✅ Validation**
- Required fields validation
- Product existence validation
- Branch existence validation
- At least one detail line required

### **✅ Business Logic**
- Auto-calculates `TotalCost` for each line
- Auto-calculates `TotalValue` for transaction
- Supports multiple products per transaction
- Line numbering for ordering

### **✅ Data Relationships**
```
StockTransaction 1──────N StockTransactionDetail
       │                         │
       │                         │
       ├─────N Branch            ├─────1 Product
       │                         
       └─────1 Stakeholder       
```

### **✅ Status Workflow**
```
Draft → Approved → Completed
  │
  └─────→ Cancelled
```

---

## 📊 Use Cases

| Use Case | Endpoint | Details Count | Status |
|----------|----------|---------------|---------|
| **Receive shipment** | POST | Multiple products | Completed |
| **Transfer between branches** | POST | Multiple products | Draft/Completed |
| **Stock adjustment** | POST | Single/Multiple | Completed |
| **Return to supplier** | POST | Multiple products | Draft |
| **Dispense for sale** | POST (via SalesInvoice) | Multiple | Completed |

---

## 🔧 Additional Endpoints (StockTransactionDetail)

For granular line item management:

- **GET** `/api/StockTransactionDetail/{id}`
- **GET** `/api/StockTransactionDetail/by-transaction/{transactionId}`
- **POST** `/api/StockTransactionDetail` - Add single line
- **PUT** `/api/StockTransactionDetail/{id}` - Update single line
- **DELETE** `/api/StockTransactionDetail/{id}` - Delete single line

---

## 🎯 Benefits

✅ **Complete CRUD** - All operations covered  
✅ **Master-Detail** - Multiple products per transaction  
✅ **Validation** - Comprehensive server-side validation  
✅ **Filtering** - Advanced query with AND/OR logic  
✅ **Pagination** - Handle large result sets  
✅ **Sorting** - Multi-column sorting  
✅ **Audit Trail** - Track who/when created/updated  
✅ **Soft Delete** - Preserve data for audit  
✅ **Type Safe** - Strong typing with DTOs  
✅ **Clean Architecture** - CQRS pattern  
✅ **Scalable** - Ready for enterprise use  

---

## 🚀 Build Status

✅ **Build Successful** - All files compile correctly  
✅ **MediatR Auto-Registration** - Handlers automatically discovered  
✅ **Dependency Injection** - All services registered  
✅ **API Ready** - Endpoints ready for testing  

---

## 📝 Example Frontend Integration

```javascript
// Create stock transaction with details
async function createStockTransaction(transactionData) {
  const response = await fetch('/api/StockTransaction', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(transactionData)
  });
  
  const result = await response.json();
  
  if (result.success) {
    console.log('Transaction created:', result.data.referenceNumber);
    console.log('Total value:', result.data.totalValue);
    console.log('Detail count:', result.data.details.length);
  }
}

// Usage
await createStockTransaction({
  transactionTypeId: 'transfer-guid',
  fromBranchId: 'branch1-guid',
  toBranchId: 'branch2-guid',
  transactionDate: new Date().toISOString(),
  status: 'Draft',
  details: [
    {
      productId: 'product1-guid',
      quantity: 100,
      unitCost: 5.00,
      batchNumber: 'BATCH-001',
      lineNumber: 1
    },
    {
      productId: 'product2-guid',
      quantity: 50,
      unitCost: 10.00,
      batchNumber: 'BATCH-002',
      lineNumber: 2
    }
  ]
});
```

---

## 🎉 Complete!

Your StockTransactionController with full CRUD and master-detail support is ready to use! 🚀

**Features:**
- ✅ Create transaction with multiple products
- ✅ Update transaction and details together
- ✅ Delete transaction (cascades to details)
- ✅ Query with advanced filtering
- ✅ Get by ID with all details
- ✅ Approval workflow placeholders
- ✅ Complete validation
- ✅ AutoMapper integration
- ✅ Clean architecture (CQRS)

Test the endpoints and let me know if you need any adjustments!
