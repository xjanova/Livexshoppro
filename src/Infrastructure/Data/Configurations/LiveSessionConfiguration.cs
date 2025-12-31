// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - LiveSession & ChatMessage Entity Configuration
//  EF Core Configuration สำหรับตาราง LiveSessions, ChatMessages
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LiveXShopPro.Core.Entities;

namespace LiveXShopPro.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration สำหรับ LiveSession Entity
/// </summary>
public class LiveSessionConfiguration : IEntityTypeConfiguration<LiveSession>
{
    public void Configure(EntityTypeBuilder<LiveSession> builder)
    {
        builder.ToTable("LiveSessions");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Title).HasMaxLength(300);
        builder.Property(l => l.StreamUrl).HasMaxLength(500);
        builder.Property(l => l.Description).HasMaxLength(1000);

        // สถิติ
        builder.Property(l => l.TotalSales).HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(l => l.Platform);
        builder.HasIndex(l => l.Status);
        builder.HasIndex(l => l.StartedAt);

        // Relationships
        builder.HasMany(l => l.ChatMessages)
            .WithOne(c => c.LiveSession)
            .HasForeignKey(c => c.LiveSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(l => l.Orders)
            .WithOne(o => o.LiveSession)
            .HasForeignKey(o => o.LiveSessionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

/// <summary>
/// Configuration สำหรับ ChatMessage Entity
/// </summary>
public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");

        builder.HasKey(c => c.Id);

        // ข้อมูลผู้ส่ง
        builder.Property(c => c.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.UserName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.ProfilePictureUrl).HasMaxLength(500);

        // ข้อความ
        builder.Property(c => c.Message)
            .IsRequired()
            .HasMaxLength(2000);

        // CF Detection
        builder.Property(c => c.CFProducts).HasMaxLength(500);
        builder.Property(c => c.DuplicateReason).HasMaxLength(200);
        builder.Property(c => c.SkipReason).HasMaxLength(200);

        // Indexes
        builder.HasIndex(c => c.LiveSessionId);
        builder.HasIndex(c => c.UserId);
        builder.HasIndex(c => c.ReceivedAt);
        builder.HasIndex(c => c.IsCF);
        builder.HasIndex(c => c.IsDuplicate);

        // Relationships
        builder.HasOne(c => c.Customer)
            .WithMany()
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Order)
            .WithMany()
            .HasForeignKey(c => c.OrderId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.DuplicateOf)
            .WithMany()
            .HasForeignKey(c => c.DuplicateOfId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
