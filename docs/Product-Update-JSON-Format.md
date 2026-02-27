# Product Update API - Corrected JSON Format

## ✅ Correct JSON Format:

```json
{
  "oid": "d3d262eb-caa4-46a2-acae-000c87fac07e",
  "drugName": "AMARYL 6 mg tablet",
  "drugNameAr": "اماريل",
  "gtin": "6285088001949",
  "barcode": "",
  "genericName": "GLIMEPIRIDE",
  "productTypeId": "22222222-2222-2222-2222-222222222020",
  "strengthValue": "6.000",
  "strengthUnit": "mg",
  "packageSize": "30",
  "price": 52.35,
  "registrationNumber": "53-23-16",
  "volume": 0,
  "unitOfVolume": "",
  "manufacturer": "",
  "countryOfOrigin": "",
  "minStockLevel": 0,
  "maxStockLevel": 0,
  "isExportable": false,
  "isImportable": true,
  "drugStatus": "1",
  "marketingStatus": "1",
  "legalStatus": "1",
  "vatTypeId": null,
  "packageTypeId": "32439495-d347-415b-950e-76d898251af0",
  "dosageFormId": "f82f16e3-8a42-4927-a7c7-afbff6821a7e",
  "productGroupId": null
}
```

## ❌ Issues Fixed:

### 1. **Empty String GUIDs → null**
```json
// BEFORE (❌):
"vatTypeId": "",
"productGroupId": ""

// AFTER (✅):
"vatTypeId": null,
"productGroupId": null
```

### 2. **Removed Invalid Fields**
```json
// BEFORE (❌):
"packageType": "Pharmacy.Domain.Entities.AppLookupDetail",
"status": 1

// AFTER (✅):
// Removed - not part of UpdateProductDto
```

### 3. **Empty Strings for Optional Fields**
```json
// ACCEPTABLE:
"barcode": "",
"manufacturer": "",

// OR BETTER (null):
"barcode": null,
"manufacturer": null,
```

## 🔧 Changes Made:

### 1. **UpdateProductDto.cs**
- ✅ Added validation attributes
- ✅ Added string length limits
- ✅ Added range validation for numeric fields
- ✅ Added XML documentation

### 2. **NullableGuidConverter.cs**
- ✅ Converts empty strings ("") to null
- ✅ Validates GUID format
- ✅ Handles null values properly

### 3. **Program.cs**
- ✅ Registered JSON converter
- ✅ Configured case-insensitive property names
- ✅ Ignore null values when writing

## 📝 Request Example:

### **PUT** `/api/Product/{id}`

```json
{
  "oid": "d3d262eb-caa4-46a2-acae-000c87fac07e",
  "drugName": "AMARYL 6 mg tablet",
  "drugNameAr": "اماريل",
  "gtin": "6285088001949",
  "genericName": "GLIMEPIRIDE",
  "productTypeId": "22222222-2222-2222-2222-222222222020",
  "strengthValue": "6.000",
  "strengthUnit": "mg",
  "packageSize": "30",
  "price": 52.35,
  "packageTypeId": "32439495-d347-415b-950e-76d898251af0",
  "dosageFormId": "f82f16e3-8a42-4927-a7c7-afbff6821a7e"
}
```

## ✅ Validation Rules:

| Field | Required | Max Length | Range | Format |
|-------|----------|------------|-------|--------|
| `oid` | ✅ Yes | - | - | GUID |
| `drugName` | ✅ Yes | 500 | - | String |
| `drugNameAr` | ❌ No | 500 | - | String |
| `gtin` | ❌ No | 14 | - | 14 digits |
| `price` | ❌ No | - | ≥ 0 | Decimal |
| `volume` | ❌ No | - | ≥ 0 | Decimal |
| `minStockLevel` | ❌ No | - | ≥ 0 | Decimal |
| `maxStockLevel` | ❌ No | - | ≥ 0 | Decimal |

## 🚀 Benefits:

✅ **Empty String Handling** - Automatically converts "" to null for GUIDs  
✅ **Validation** - Server-side validation with clear error messages  
✅ **Type Safety** - Strong typing prevents invalid data  
✅ **Flexible** - Frontend can send "" or null (both work)  
✅ **Case Insensitive** - Property names work regardless of case  

## 🧪 Testing:

```bash
# Test with curl
curl -X PUT "https://localhost:5001/api/Product/d3d262eb-caa4-46a2-acae-000c87fac07e" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d @product-update.json
```

## 📊 Response:

### Success (200 OK):
```json
{
  "success": true,
  "message": "Product updated successfully",
  "data": {
    "oid": "d3d262eb-caa4-46a2-acae-000c87fac07e",
    "drugName": "AMARYL 6 mg tablet",
    ...
  }
}
```

### Validation Error (400 Bad Request):
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "DrugName": ["Drug name is required"],
    "Price": ["Price must be a positive value"]
  }
}
```
