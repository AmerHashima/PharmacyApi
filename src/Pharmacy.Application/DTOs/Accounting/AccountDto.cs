namespace Pharmacy.Application.DTOs.Accounting;

public class AccountDto
{
    public Guid Oid { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentNameAr { get; set; }
    public int AccountLevel { get; set; }
    public Guid? AccountTypeId { get; set; }
    public string? AccountTypeName { get; set; }
    public string? AccountTypeNameAr { get; set; }
    public Guid? NatureId { get; set; }
    public string? NatureName { get; set; }
    public string? NatureNameAr { get; set; }
    public Guid? FinalAccountId { get; set; }
    public string? FinalAccountName { get; set; }
    public string? FinalAccountNameAr { get; set; }
    public bool IsLeaf { get; set; }
    public bool IsActive { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateAccountDto
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public Guid? ParentId { get; set; }
    public int AccountLevel { get; set; } = 1;
    public Guid? AccountTypeId { get; set; }
    public Guid? NatureId { get; set; }
    public Guid? FinalAccountId { get; set; }
    public bool IsLeaf { get; set; } = true;
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Payload for creating a child account under a parent and linking it to a customer or stakeholder.
/// Exactly one of <see cref="CustomerId"/> or <see cref="StakeholderId"/> must be set.
/// </summary>
public class CreateChildAccountDto
{
    /// <summary>The parent account under which the child will be created.</summary>
    public Guid ParentAccountId { get; set; }

    /// <summary>Link to a customer (mutually exclusive with StakeholderId).</summary>
    public Guid? CustomerId { get; set; }

    /// <summary>Link to a stakeholder/supplier (mutually exclusive with CustomerId).</summary>
    public Guid? StakeholderId { get; set; }

    /// <summary>Optional override for the child account code. Auto-generated when omitted.</summary>
    public string? AccountCode { get; set; }

    /// <summary>Optional override for the child account name (Arabic). Defaults to the customer/stakeholder name.</summary>
    public string? AccountNameAr { get; set; }

    /// <summary>Optional override for the child account name (English).</summary>
    public string? AccountNameEn { get; set; }
}

public class UpdateAccountDto
{
    public Guid Oid { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public Guid? ParentId { get; set; }
    public int AccountLevel { get; set; }
    public Guid? AccountTypeId { get; set; }
    public Guid? NatureId { get; set; }
    public Guid? FinalAccountId { get; set; }
    public bool IsLeaf { get; set; }
    public bool IsActive { get; set; }
}
