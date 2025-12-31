// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Application DbContext
//  Entity Framework Core DbContext สำหรับ SQLite
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore;
using LiveXShopPro.Core.Entities;

namespace LiveXShopPro.Infrastructure.Data.Context;

/// <summary>
/// DbContext หลักของแอพพลิเคชัน
/// ใช้ SQLite เป็น Database
/// </summary>
public class AppDbContext : DbContext
{
    #region DbSets

    /// <summary>
    /// ตารางลูกค้า
    /// </summary>
    public DbSet<Customer> Customers => Set<Customer>();

    /// <summary>
    /// ตารางสินค้า
    /// </summary>
    public DbSet<Product> Products => Set<Product>();

    /// <summary>
    /// ตาราง Variants
    /// </summary>
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

    /// <summary>
    /// ตารางรูปสินค้า
    /// </summary>
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    /// <summary>
    /// ตารางหมวดหมู่
    /// </summary>
    public DbSet<Category> Categories => Set<Category>();

    /// <summary>
    /// ตารางออเดอร์
    /// </summary>
    public DbSet<Order> Orders => Set<Order>();

    /// <summary>
    /// ตารางรายการสินค้าในออเดอร์
    /// </summary>
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    /// <summary>
    /// ตารางการชำระเงิน
    /// </summary>
    public DbSet<Payment> Payments => Set<Payment>();

    /// <summary>
    /// ตาราง SMS ธนาคาร
    /// </summary>
    public DbSet<BankSms> BankSmsMessages => Set<BankSms>();

    /// <summary>
    /// ตารางการจัดส่ง
    /// </summary>
    public DbSet<Shipment> Shipments => Set<Shipment>();

    /// <summary>
    /// ตารางเซสชันไลฟ์
    /// </summary>
    public DbSet<LiveSession> LiveSessions => Set<LiveSession>();

    /// <summary>
    /// ตารางข้อความแชท
    /// </summary>
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor สำหรับ Dependency Injection
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    #endregion

    #region Configuration

    /// <summary>
    /// กำหนดค่า Model และ Relationships
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ใช้ Configuration แยกไฟล์
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Global Query Filter สำหรับ Soft Delete
        // ไม่ดึงข้อมูลที่ถูกลบ
        modelBuilder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(e => !e.IsDeleted);
    }

    /// <summary>
    /// Override SaveChanges เพื่ออัพเดท Timestamps อัตโนมัติ
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Override SaveChanges (Sync)
    /// </summary>
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// อัพเดท CreatedAt และ UpdatedAt อัตโนมัติ
    /// </summary>
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // ตั้งค่า CreatedAt เมื่อเพิ่มใหม่
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    // ตั้งค่า UpdatedAt เมื่อแก้ไข
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        // จัดการ Soft Delete
        var softDeleteEntries = ChangeTracker.Entries<SoftDeleteEntity>();

        foreach (var entry in softDeleteEntries)
        {
            if (entry.State == EntityState.Deleted)
            {
                // เปลี่ยนจาก Delete เป็น Update (Soft Delete)
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
            }
        }
    }

    #endregion
}
