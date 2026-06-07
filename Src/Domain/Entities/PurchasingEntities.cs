namespace fastinventoryPurchase.Src.Domain.Entities;

public class Supplier
{
    public string SupplierCen { get; private set; }
    public string CompanyCen { get; private set; }
    public string Name { get; private set; }

    public Supplier(string companyCen, string name)
    {
        SupplierCen = Guid.NewGuid().ToString("N");
        CompanyCen = companyCen;
        Name = name;
    }
}

public class PurchaseOrder
{
    public string OrderCen { get; private set; }
    public string CompanyCen { get; private set; }
    public string WarehouseCen { get; private set; }
    public string SupplierCen { get; private set; }
    public short Status { get; private set; } // 0: Created, 1: Confirmed
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ConfirmedAt { get; private set; }

    private readonly List<PurchaseOrderItem> _items = new();
    public IReadOnlyCollection<PurchaseOrderItem> Items => _items.AsReadOnly();

    public PurchaseOrder(string companyCen, string warehouseCen, string supplierCen)
    {
        OrderCen = Guid.NewGuid().ToString("N");
        CompanyCen = companyCen;
        WarehouseCen = warehouseCen;
        SupplierCen = supplierCen;
        Status = 0;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void AddItem(string productCen, decimal quantity)
    {
        _items.Add(new PurchaseOrderItem(productCen, quantity));
    }

    public void Confirm()
    {
        if (Status == 1) throw new InvalidOperationException("Order already confirmed");
        Status = 1;
        ConfirmedAt = DateTimeOffset.UtcNow;
    }
}

public class PurchaseOrderItem
{
    public string ItemCen { get; private set; }
    public string ProductCen { get; private set; }
    public decimal Quantity { get; private set; }

    public PurchaseOrderItem(string productCen, decimal quantity)
    {
        ItemCen = Guid.NewGuid().ToString("N");
        ProductCen = productCen;
        Quantity = quantity;
    }
}
