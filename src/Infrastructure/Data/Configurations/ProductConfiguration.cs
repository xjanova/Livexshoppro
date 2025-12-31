// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Product Entity Configuration
//  EF Core Configuration สำหรับตาราง Products
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LiveXShopPro.Core.Entities;

namespace LiveXShopPro.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration สำหรับ Product Entity
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // ตั้งชื่อตาราง
        builder.ToTable("Products");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.SKU)
            .HasMaxLength(50);

        builder.Property(p => p.Barcode)
            .HasMaxLength(50);

        builder.Property(p => p.LiveCode)
            .HasMaxLength(20);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        // ราคา
        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Property(p => p.Cost)
            .HasPrecision(18, 2);

        builder.Property(p => p.CompareAtPrice)
            .HasPrecision(18, 2);

        // น้ำหนัก/ขนาด
        builder.Property(p => p.Weight).HasPrecision(10, 2);
        builder.Property(p => p.Width).HasPrecision(10, 2);
        builder.Property(p => p.Height).HasPrecision(10, 2);
        builder.Property(p => p.Length).HasPrecision(10, 2);

        // รูปภาพ
        builder.Property(p => p.MainImagePath).HasMaxLength(500);

        // Indexes
        builder.HasIndex(p => p.SKU).IsUnique();
        builder.HasIndex(p => p.Barcode);
        builder.HasIndex(p => p.LiveCode);
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => p.IsDeleted);

        // Relationships
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.Variants)
            .WithOne(v => v.Product)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Images)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

/// <summary>
/// Configuration สำหรับ ProductVariant Entity
/// </summary>
public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.SKU).HasMaxLength(50);
        builder.Property(v => v.Barcode).HasMaxLength(50);
        builder.Property(v => v.Price).HasPrecision(18, 2);

        builder.HasIndex(v => v.ProductId);
    }
}

/// <summary>
/// Configuration สำหรับ ProductImage Entity
/// </summary>
public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.ImagePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(i => i.ProductId);
    }
}

/// <summary>
/// Configuration สำหรับ Category Entity
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Self-referencing relationship
        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
