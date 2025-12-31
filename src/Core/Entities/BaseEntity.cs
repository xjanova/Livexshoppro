// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Base Entity
//  Entity พื้นฐานสำหรับทุก Domain Entity
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

namespace LiveXShopPro.Core.Entities;

/// <summary>
/// Entity พื้นฐานที่ทุก Entity ต้อง inherit
/// มี Id และ Timestamps เป็นมาตรฐาน
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Primary Key - ใช้ GUID เพื่อความปลอดภัยและรองรับ Distributed System
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// วันที่สร้าง Record
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// วันที่แก้ไขล่าสุด
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Entity พื้นฐานพร้อมระบบ Soft Delete
/// ไม่ลบจริง แค่ทำเครื่องหมายว่าลบแล้ว
/// </summary>
public abstract class SoftDeleteEntity : BaseEntity
{
    /// <summary>
    /// สถานะการลบ - true = ถูกลบแล้ว
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// วันที่ลบ
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
