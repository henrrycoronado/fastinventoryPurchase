namespace PrismodPurchase.Src.Infraestructure.Persistence.Models;

public class SupplierModel
{
    public long Id { get; set; }
    public string SupplierCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public ICollection<PurchaseOrderModel> PurchaseOrders { get; set; } = new List<PurchaseOrderModel>();
}

public class PurchaseOrderModel
{
    public long Id { get; set; }
    public string OrderCen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public string SupplierCen { get; set; } = string.Empty;
    public short Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ConfirmedAt { get; set; }

    public SupplierModel Supplier { get; set; } = null!;
    public ICollection<PurchaseOrderItemModel> Items { get; set; } = new List<PurchaseOrderItemModel>();
}

public class PurchaseOrderItemModel
{
    public long Id { get; set; }
    public string ItemCen { get; set; } = string.Empty;
    public string OrderCen { get; set; } = string.Empty;
    public string ProductCen { get; set; } = string.Empty;
    public decimal Quantity { get; set; }

    public PurchaseOrderModel Order { get; set; } = null!;
}
