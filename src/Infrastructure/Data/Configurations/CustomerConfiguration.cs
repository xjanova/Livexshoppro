// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Customer Entity Configuration
//  EF Core Configuration สำหรับตาราง Customers
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LiveXShopPro.Core.Entities;

namespace LiveXShopPro.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration สำหรับ Customer Entity
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // ตั้งชื่อตาราง
        builder.ToTable("Customers");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Nickname)
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(200);

        // ที่อยู่
        builder.Property(c => c.Address).HasMaxLength(500);
        builder.Property(c => c.SubDistrict).HasMaxLength(100);
        builder.Property(c => c.District).HasMaxLength(100);
        builder.Property(c => c.Province).HasMaxLength(100);
        builder.Property(c => c.PostalCode).HasMaxLength(10);

        // Social Media
        builder.Property(c => c.FacebookId).HasMaxLength(100);
        builder.Property(c => c.FacebookName).HasMaxLength(200);
        builder.Property(c => c.TikTokId).HasMaxLength(100);
        builder.Property(c => c.TikTokName).HasMaxLength(200);
        builder.Property(c => c.LineUserId).HasMaxLength(100);
        builder.Property(c => c.LineName).HasMaxLength(200);
        builder.Property(c => c.ProfilePictureUrl).HasMaxLength(500);

        // สถิติ
        builder.Property(c => c.TotalSpent)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(c => c.Phone);
        builder.HasIndex(c => c.FacebookId);
        builder.HasIndex(c => c.TikTokId);
        builder.HasIndex(c => c.LineUserId);
        builder.HasIndex(c => c.CustomerType);
        builder.HasIndex(c => c.IsDeleted);

        // Relationships
        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
