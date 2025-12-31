// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Payment & Shipment Entity Configuration
//  EF Core Configuration สำหรับตาราง Payments, BankSms, Shipments
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LiveXShopPro.Core.Entities;

namespace LiveXShopPro.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration สำหรับ Payment Entity
/// </summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        // ยอดเงิน
        builder.Property(p => p.Amount).HasPrecision(18, 2);

        // ข้อมูลสลิป
        builder.Property(p => p.SlipImagePath).HasMaxLength(500);
        builder.Property(p => p.BankName).HasMaxLength(100);
        builder.Property(p => p.AccountNumber).HasMaxLength(50);
        builder.Property(p => p.AccountName).HasMaxLength(200);
        builder.Property(p => p.ReferenceNo).HasMaxLength(100);
        builder.Property(p => p.TransactionId).HasMaxLength(100);

        // การตรวจสอบ
        builder.Property(p => p.VerifiedBy).HasMaxLength(100);
        builder.Property(p => p.VerificationNote).HasMaxLength(500);
        builder.Property(p => p.VerificationWarnings).HasMaxLength(1000);

        // Indexes
        builder.HasIndex(p => p.OrderId);
        builder.HasIndex(p => p.ReferenceNo);
        builder.HasIndex(p => p.VerificationStatus);

        // Relationships
        builder.HasOne(p => p.BankSms)
            .WithOne(s => s.Payment)
            .HasForeignKey<Payment>(p => p.BankSmsId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

/// <summary>
/// Configuration สำหรับ BankSms Entity
/// </summary>
public class BankSmsConfiguration : IEntityTypeConfiguration<BankSms>
{
    public void Configure(EntityTypeBuilder<BankSms> builder)
    {
        builder.ToTable("BankSms");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Sender)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(s => s.BankName).HasMaxLength(100);
        builder.Property(s => s.TransactionType).HasMaxLength(50);
        builder.Property(s => s.Amount).HasPrecision(18, 2);
        builder.Property(s => s.TransferFrom).HasMaxLength(50);
        builder.Property(s => s.ReferenceNo).HasMaxLength(100);
        builder.Property(s => s.Balance).HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(s => s.ReceivedAt);
        builder.HasIndex(s => s.ReferenceNo);
        builder.HasIndex(s => s.IsMatched);
    }
}

/// <summary>
/// Configuration สำหรับ Shipment Entity
/// </summary>
public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipments");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.TrackingNumber).HasMaxLength(50);
        builder.Property(s => s.ServiceType).HasMaxLength(50);

        // ขนาด/น้ำหนัก
        builder.Property(s => s.Weight).HasPrecision(10, 2);
        builder.Property(s => s.Width).HasPrecision(10, 2);
        builder.Property(s => s.Height).HasPrecision(10, 2);
        builder.Property(s => s.Length).HasPrecision(10, 2);

        // ค่าใช้จ่าย
        builder.Property(s => s.ShippingCost).HasPrecision(18, 2);
        builder.Property(s => s.CodAmount).HasPrecision(18, 2);
        builder.Property(s => s.CodFee).HasPrecision(18, 2);

        builder.Property(s => s.LabelUrl).HasMaxLength(500);
        builder.Property(s => s.TrackingHistory).HasMaxLength(5000);
        builder.Property(s => s.ReceivedBy).HasMaxLength(200);

        // Indexes
        builder.HasIndex(s => s.OrderId).IsUnique();
        builder.HasIndex(s => s.TrackingNumber);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.Carrier);
    }
}
