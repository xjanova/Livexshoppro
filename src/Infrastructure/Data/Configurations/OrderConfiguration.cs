// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Order Entity Configuration
//  EF Core Configuration สำหรับตาราง Orders
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LiveXShopPro.Core.Entities;

namespace LiveXShopPro.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration สำหรับ Order Entity
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // ตั้งชื่อตาราง
        builder.ToTable("Orders");

        // Primary Key
        builder.HasKey(o => o.Id);

        // Properties
        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.CustomerPhone)
            .HasMaxLength(20);

        // ที่อยู่จัดส่ง
        builder.Property(o => o.ShippingAddress).HasMaxLength(500);
        builder.Property(o => o.ShippingSubDistrict).HasMaxLength(100);
        builder.Property(o => o.ShippingDistrict).HasMaxLength(100);
        builder.Property(o => o.ShippingProvince).HasMaxLength(100);
        builder.Property(o => o.ShippingPostalCode).HasMaxLength(10);

        // ยอดเงิน
        builder.Property(o => o.SubTotal).HasPrecision(18, 2);
        builder.Property(o => o.Discount).HasPrecision(18, 2);
        builder.Property(o => o.ShippingFee).HasPrecision(18, 2);
        builder.Property(o => o.Total).HasPrecision(18, 2);

        builder.Property(o => o.DiscountCode).HasMaxLength(50);

        // หมายเหตุ
        builder.Property(o => o.CustomerNote).HasMaxLength(500);
        builder.Property(o => o.AdminNote).HasMaxLength(1000);
        builder.Property(o => o.CancellationReason).HasMaxLength(500);

        // Indexes
        builder.HasIndex(o => o.OrderNumber).IsUnique();
        builder.HasIndex(o => o.CustomerId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.PaymentStatus);
        builder.HasIndex(o => o.ShippingStatus);
        builder.HasIndex(o => o.CreatedAt);
        builder.HasIndex(o => o.LiveSessionId);
        builder.HasIndex(o => o.IsDeleted);

        // Relationships
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Payments)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.Shipment)
            .WithOne(s => s.Order)
            .HasForeignKey<Shipment>(s => s.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.LiveSession)
            .WithMany(l => l.Orders)
            .HasForeignKey(o => o.LiveSessionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

/// <summary>
/// Configuration สำหรับ OrderItem Entity
/// </summary>
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.ProductName)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(i => i.ProductSKU).HasMaxLength(50);
        builder.Property(i => i.VariantName).HasMaxLength(200);

        builder.Property(i => i.UnitPrice).HasPrecision(18, 2);
        builder.Property(i => i.Discount).HasPrecision(18, 2);
        builder.Property(i => i.Total).HasPrecision(18, 2);

        builder.HasIndex(i => i.OrderId);
        builder.HasIndex(i => i.ProductId);

        builder.HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Variant)
            .WithMany()
            .HasForeignKey(i => i.VariantId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
