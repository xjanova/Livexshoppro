// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - LiveSession Entity
//  ข้อมูลเซสชันไลฟ์และแชท
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using LiveXShopPro.Core.Enums;

namespace LiveXShopPro.Core.Entities;

/// <summary>
/// Entity สำหรับเก็บข้อมูลเซสชันไลฟ์
/// </summary>
public class LiveSession : BaseEntity
{
    #region ข้อมูลไลฟ์

    /// <summary>
    /// ชื่อ/หัวข้อไลฟ์
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// แพลตฟอร์ม
    /// </summary>
    public Platform Platform { get; set; }

    /// <summary>
    /// URL ของไลฟ์
    /// </summary>
    public string? StreamUrl { get; set; }

    /// <summary>
    /// รายละเอียด/คำอธิบาย
    /// </summary>
    public string? Description { get; set; }

    #endregion

    #region สถิติ

    /// <summary>
    /// จำนวนข้อความทั้งหมด
    /// </summary>
    public int TotalMessages { get; set; } = 0;

    /// <summary>
    /// จำนวน CF ที่ตรวจพบ
    /// </summary>
    public int TotalCF { get; set; } = 0;

    /// <summary>
    /// จำนวนออเดอร์ที่สร้าง
    /// </summary>
    public int TotalOrders { get; set; } = 0;

    /// <summary>
    /// ยอดขายรวม (บาท)
    /// </summary>
    public decimal TotalSales { get; set; } = 0;

    /// <summary>
    /// จำนวนผู้ชมสูงสุด
    /// </summary>
    public int PeakViewers { get; set; } = 0;

    /// <summary>
    /// จำนวนลูกค้าใหม่
    /// </summary>
    public int NewCustomers { get; set; } = 0;

    #endregion

    #region สถานะ

    /// <summary>
    /// สถานะ (Active/Ended/Cancelled)
    /// </summary>
    public LiveSessionStatus Status { get; set; } = LiveSessionStatus.Active;

    #endregion

    #region Timestamps

    /// <summary>
    /// เวลาเริ่มไลฟ์
    /// </summary>
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// เวลาจบไลฟ์
    /// </summary>
    public DateTime? EndedAt { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// ข้อความแชททั้งหมด
    /// </summary>
    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    /// <summary>
    /// ออเดอร์ที่สร้างจากไลฟ์นี้
    /// </summary>
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    #endregion

    #region Helper Methods

    /// <summary>
    /// คำนวณระยะเวลาไลฟ์
    /// </summary>
    public TimeSpan GetDuration()
    {
        var endTime = EndedAt ?? DateTime.UtcNow;
        return endTime - StartedAt;
    }

    /// <summary>
    /// คำนวณออเดอร์ต่อนาที
    /// </summary>
    public double GetOrdersPerMinute()
    {
        var duration = GetDuration();
        if (duration.TotalMinutes == 0) return 0;
        return TotalOrders / duration.TotalMinutes;
    }

    #endregion
}

/// <summary>
/// สถานะของเซสชันไลฟ์
/// </summary>
public enum LiveSessionStatus
{
    /// <summary>
    /// กำลังไลฟ์อยู่
    /// </summary>
    Active = 0,

    /// <summary>
    /// พัก
    /// </summary>
    Paused = 1,

    /// <summary>
    /// จบแล้ว
    /// </summary>
    Ended = 2,

    /// <summary>
    /// ยกเลิก
    /// </summary>
    Cancelled = 3
}

/// <summary>
/// Entity สำหรับเก็บข้อความแชทจากไลฟ์
/// </summary>
public class ChatMessage : BaseEntity
{
    #region Foreign Keys

    /// <summary>
    /// เซสชันไลฟ์ (FK)
    /// </summary>
    public Guid LiveSessionId { get; set; }

    /// <summary>
    /// ลูกค้า (FK) - ถ้า match ได้
    /// </summary>
    public Guid? CustomerId { get; set; }

    #endregion

    #region ข้อมูลผู้ส่ง

    /// <summary>
    /// User ID จากแพลตฟอร์ม
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// ชื่อผู้ใช้
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// URL รูปโปรไฟล์
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// แพลตฟอร์ม
    /// </summary>
    public Platform Platform { get; set; }

    #endregion

    #region ข้อความ

    /// <summary>
    /// ข้อความ
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// เวลาที่ได้รับข้อความ
    /// </summary>
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    #endregion

    #region CF Detection

    /// <summary>
    /// เป็นข้อความ CF หรือไม่
    /// </summary>
    public bool IsCF { get; set; } = false;

    /// <summary>
    /// รายการ CF ที่ตรวจพบ (JSON)
    /// [{"code": "1", "quantity": 2}]
    /// </summary>
    public string? CFProducts { get; set; }

    /// <summary>
    /// สร้างออเดอร์แล้วหรือยัง
    /// </summary>
    public bool ProcessedToOrder { get; set; } = false;

    /// <summary>
    /// ออเดอร์ที่สร้าง (FK)
    /// </summary>
    public Guid? OrderId { get; set; }

    #endregion

    #region Duplicate Detection

    /// <summary>
    /// เป็นข้อความซ้ำหรือไม่
    /// </summary>
    public bool IsDuplicate { get; set; } = false;

    /// <summary>
    /// ซ้ำกับข้อความไหน (FK)
    /// </summary>
    public Guid? DuplicateOfId { get; set; }

    /// <summary>
    /// เหตุผลที่ถือว่าซ้ำ
    /// </summary>
    public string? DuplicateReason { get; set; }

    #endregion

    #region สถานะ

    /// <summary>
    /// ข้ามข้อความนี้ (ไม่ประมวลผล)
    /// </summary>
    public bool IsSkipped { get; set; } = false;

    /// <summary>
    /// เหตุผลที่ข้าม
    /// </summary>
    public string? SkipReason { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// เซสชันไลฟ์
    /// </summary>
    public virtual LiveSession? LiveSession { get; set; }

    /// <summary>
    /// ลูกค้า
    /// </summary>
    public virtual Customer? Customer { get; set; }

    /// <summary>
    /// ออเดอร์ที่สร้าง
    /// </summary>
    public virtual Order? Order { get; set; }

    /// <summary>
    /// ข้อความต้นฉบับ (กรณีซ้ำ)
    /// </summary>
    public virtual ChatMessage? DuplicateOf { get; set; }

    #endregion
}

/// <summary>
/// โมเดลสำหรับ CF ที่ตรวจพบ
/// </summary>
public class CFProduct
{
    /// <summary>
    /// รหัสสินค้า (1, 2, 3, ...)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// จำนวน
    /// </summary>
    public int Quantity { get; set; } = 1;
}
