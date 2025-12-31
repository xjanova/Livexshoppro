// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Customer Entity
//  ข้อมูลลูกค้า
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using LiveXShopPro.Core.Enums;

namespace LiveXShopPro.Core.Entities;

/// <summary>
/// Entity สำหรับเก็บข้อมูลลูกค้า
/// สร้างอัตโนมัติจากการ CF ในไลฟ์ หรือเพิ่มเองได้
/// </summary>
public class Customer : SoftDeleteEntity
{
    #region ข้อมูลพื้นฐาน

    /// <summary>
    /// ชื่อลูกค้า
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// ชื่อเล่น/นามแฝง
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// เบอร์โทรศัพท์
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// อีเมล
    /// </summary>
    public string? Email { get; set; }

    #endregion

    #region ที่อยู่จัดส่ง

    /// <summary>
    /// ที่อยู่ (บ้านเลขที่ ซอย ถนน)
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// ตำบล/แขวง
    /// </summary>
    public string? SubDistrict { get; set; }

    /// <summary>
    /// อำเภอ/เขต
    /// </summary>
    public string? District { get; set; }

    /// <summary>
    /// จังหวัด
    /// </summary>
    public string? Province { get; set; }

    /// <summary>
    /// รหัสไปรษณีย์
    /// </summary>
    public string? PostalCode { get; set; }

    #endregion

    #region ข้อมูล Social Media

    /// <summary>
    /// Facebook User ID
    /// </summary>
    public string? FacebookId { get; set; }

    /// <summary>
    /// ชื่อที่แสดงบน Facebook
    /// </summary>
    public string? FacebookName { get; set; }

    /// <summary>
    /// TikTok User ID
    /// </summary>
    public string? TikTokId { get; set; }

    /// <summary>
    /// ชื่อที่แสดงบน TikTok
    /// </summary>
    public string? TikTokName { get; set; }

    /// <summary>
    /// LINE User ID
    /// </summary>
    public string? LineUserId { get; set; }

    /// <summary>
    /// ชื่อที่แสดงบน LINE
    /// </summary>
    public string? LineName { get; set; }

    /// <summary>
    /// URL รูปโปรไฟล์
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    #endregion

    #region สถิติและสถานะ

    /// <summary>
    /// ประเภทลูกค้า (Normal/VIP/Blocked)
    /// </summary>
    public CustomerType CustomerType { get; set; } = CustomerType.New;

    /// <summary>
    /// จำนวนออเดอร์ทั้งหมด
    /// </summary>
    public int TotalOrders { get; set; } = 0;

    /// <summary>
    /// ยอดซื้อสะสมทั้งหมด (บาท)
    /// </summary>
    public decimal TotalSpent { get; set; } = 0;

    /// <summary>
    /// วันที่สั่งซื้อล่าสุด
    /// </summary>
    public DateTime? LastOrderDate { get; set; }

    /// <summary>
    /// หมายเหตุเกี่ยวกับลูกค้า
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// สถานะใช้งาน
    /// </summary>
    public bool IsActive { get; set; } = true;

    #endregion

    #region Navigation Properties

    /// <summary>
    /// รายการออเดอร์ทั้งหมดของลูกค้า
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    #endregion

    #region Helper Methods

    /// <summary>
    /// ดึงที่อยู่แบบเต็ม
    /// </summary>
    public string GetFullAddress()
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(Address)) parts.Add(Address);
        if (!string.IsNullOrWhiteSpace(SubDistrict)) parts.Add($"ต.{SubDistrict}");
        if (!string.IsNullOrWhiteSpace(District)) parts.Add($"อ.{District}");
        if (!string.IsNullOrWhiteSpace(Province)) parts.Add($"จ.{Province}");
        if (!string.IsNullOrWhiteSpace(PostalCode)) parts.Add(PostalCode);

        return string.Join(" ", parts);
    }

    /// <summary>
    /// ดึงชื่อที่แสดง (ใช้ชื่อจาก Social ถ้าไม่มีชื่อหลัก)
    /// </summary>
    public string GetDisplayName()
    {
        if (!string.IsNullOrWhiteSpace(Name)) return Name;
        if (!string.IsNullOrWhiteSpace(FacebookName)) return FacebookName;
        if (!string.IsNullOrWhiteSpace(TikTokName)) return TikTokName;
        if (!string.IsNullOrWhiteSpace(LineName)) return LineName;
        return "ไม่ระบุชื่อ";
    }

    #endregion
}
