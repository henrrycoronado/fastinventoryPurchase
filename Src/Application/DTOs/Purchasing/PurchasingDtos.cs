namespace fastinventoryPurchase.Src.Application.DTOs.Purchasing;

public class SupplierDto
{
    public string SupplierCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class CreateSupplierDto
{
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class UpdateSupplierDto
{
    public string Name { get; set; } = string.Empty;
}

public class CreatePurchaseOrderItemDto
{
    public string ProductCen { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class CreatePurchaseOrderDto
{
    public string SupplierCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
}

public class PurchaseOrderSummaryDto
{
    public string OrderCen { get; set; } = string.Empty;
    public int Status { get; set; }
}

public class PurchaseOrderListDto
{
    public string OrderCen { get; set; } = string.Empty;
    public int Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ConfirmedAt { get; set; }
    public string SupplierCen { get; set; } = string.Empty;
    public int ItemCount { get; set; }
}

public class PurchaseOrderDetailItemDto
{
    public string ProductCen { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class PurchaseOrderDetailDto
{
    public string OrderCen { get; set; } = string.Empty;
    public int Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ConfirmedAt { get; set; }
    public string SupplierCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public List<PurchaseOrderDetailItemDto> Items { get; set; } = new();
}

public class PurchaseOrderConfirmationDto
{
    public string OrderCen { get; set; } = string.Empty;
    public int Status { get; set; }
    public DateTimeOffset ConfirmedAt { get; set; }
}