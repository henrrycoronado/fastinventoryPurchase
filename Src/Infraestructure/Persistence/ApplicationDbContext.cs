using Microsoft.EntityFrameworkCore;
using PrismodPurchase.Src.Infraestructure.Persistence.Models;

namespace PrismodPurchase.Src.Infraestructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<SupplierModel> Suppliers { get; set; } = null!;
    public DbSet<PurchaseOrderModel> PurchaseOrders { get; set; } = null!;
    public DbSet<PurchaseOrderItemModel> PurchaseOrderItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("purchasing");

        modelBuilder.Entity<SupplierModel>(e =>
        {
            e.ToTable("suppliers");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.SupplierCen);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.SupplierCen).HasColumnName("supplier_cen");
            e.Property(x => x.CompanyCen).HasColumnName("company_cen");
            e.Property(x => x.Name).HasColumnName("name");
        });

        modelBuilder.Entity<PurchaseOrderModel>(e =>
        {
            e.ToTable("purchase_orders");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.OrderCen);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.OrderCen).HasColumnName("order_cen");
            e.Property(x => x.CompanyCen).HasColumnName("company_cen");
            e.Property(x => x.WarehouseCen).HasColumnName("warehouse_cen");
            e.Property(x => x.SupplierCen).HasColumnName("supplier_cen");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.ConfirmedAt).HasColumnName("confirmed_at");

            e.HasOne(x => x.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(x => x.SupplierCen)
                .HasPrincipalKey(s => s.SupplierCen);
        });

        modelBuilder.Entity<PurchaseOrderItemModel>(e =>
        {
            e.ToTable("purchase_order_items");
            e.HasKey(x => x.Id);
            e.HasAlternateKey(x => x.ItemCen);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.ItemCen).HasColumnName("item_cen");
            e.Property(x => x.OrderCen).HasColumnName("order_cen");
            e.Property(x => x.ProductCen).HasColumnName("product_cen");
            e.Property(x => x.Quantity).HasColumnName("quantity");

            e.HasOne(x => x.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(x => x.OrderCen)
                .HasPrincipalKey(o => o.OrderCen);
        });
    }
}
