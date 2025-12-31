// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Payment Entity
//  ข้อมูลการชำระเงิน
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using LiveXShopPro.Core.Enums;

namespace LiveXShopPro.Core.Entities;

/// <summary>
/// Entity สำหรับเก็บข้อมูลการชำระเงิน
/// รวมถึงข้อมูลสลิปและการตรวจสอบ
/// </summary>
public class Payment : BaseEntity
{
    #region Foreign Keys

    /// <summary>
    /// ออเดอร์ (FK)
    /// </summary>
    public Guid OrderId { get; set; }

    #endregion

    #region ข้อมูลการชำระ

    /// <summary>
    /// จำนวนเงิน (บาท)
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// วิธีการชำระเงิน
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Transfer;

    /// <summary>
    /// สถานะการชำระ
    /// </summary>
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    #endregion

    #region ข้อมูลสลิป

    /// <summary>
    /// Path รูปสลิป
    /// </summary>
    public string? SlipImagePath { get; set; }

    /// <summary>
    /// ชื่อธนาคาร (จาก OCR)
    /// </summary>
    public string? BankName { get; set; }

    /// <summary>
    /// เลขบัญชีผู้โอน
    /// </summary>
    public string? AccountNumber { get; set; }

    /// <summary>
    /// ชื่อบัญชีผู้โอน
    /// </summary>
    public string? AccountName { get; set; }

    /// <summary>
    /// วันที่โอน (จาก OCR)
    /// </summary>
    public DateTime? TransferDate { get; set; }

    /// <summary>
    /// เวลาที่โอน (จาก OCR)
    /// </summary>
    public TimeSpan? TransferTime { get; set; }

    /// <summary>
    /// เลขอ้างอิง/Reference
    /// </summary>
    public string? ReferenceNo { get; set; }

    /// <summary>
    /// Transaction ID (จากธนาคาร)
    /// </summary>
    public string? TransactionId { get; set; }

    #endregion

    #region การตรวจสอบสลิป

    /// <summary>
    /// สถานะการตรวจสอบ
    /// </summary>
    public SlipVerificationStatus VerificationStatus { get; set; } = SlipVerificationStatus.Pending;

    /// <summary>
    /// โหมดการตรวจสอบที่ใช้
    /// </summary>
    public SlipVerificationMode? VerificationMode { get; set; }

    /// <summary>
    /// คะแนนความมั่นใจ (0-100)
    /// </summary>
    public double? ConfidenceScore { get; set; }

    /// <summary>
    /// ผู้ตรวจสอบ (กรณี Manual)
    /// </summary>
    public string? VerifiedBy { get; set; }

    /// <summary>
    /// วันที่ตรวจสอบ
    /// </summary>
    public DateTime? VerifiedAt { get; set; }

    /// <summary>
    /// หมายเหตุการตรวจสอบ
    /// </summary>
    public string? VerificationNote { get; set; }

    /// <summary>
    /// รายการปัญหาที่พบ (JSON)
    /// </summary>
    public string? VerificationWarnings { get; set; }

    #endregion

    #region SMS Matching

    /// <summary>
    /// ตรงกับ SMS หรือไม่
    /// </summary>
    public bool SmsMatched { get; set; } = false;

    /// <summary>
    /// SMS ที่ Match (FK)
    /// </summary>
    public Guid? BankSmsId { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// ออเดอร์
    /// </summary>
    public virtual Order? Order { get; set; }

    /// <summary>
    /// SMS ที่ Match
    /// </summary>
    public virtual BankSms? BankSms { get; set; }

    #endregion

    #region Helper Methods

    /// <summary>
    /// เช็คว่าผ่านการตรวจสอบหรือไม่
    /// </summary>
    public bool IsVerified => VerificationStatus == SlipVerificationStatus.Verified;

    /// <summary>
    /// เช็คว่าน่าสงสัยหรือไม่
    /// </summary>
    public bool IsSuspicious => VerificationStatus == SlipVerificationStatus.Suspicious;

    /// <summary>
    /// เช็คว่าเป็นสลิปปลอมหรือไม่
    /// </summary>
    public bool IsFake => VerificationStatus == SlipVerificationStatus.Fake;

    #endregion
}

/// <summary>
/// Entity สำหรับเก็บ SMS จากธนาคาร
/// </summary>
public class BankSms : BaseEntity
{
    #region ข้อมูล SMS

    /// <summary>
    /// ผู้ส่ง (KBANK, SCB, ...)
    /// </summary>
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// ข้อความ SMS ดิบ
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// เวลาที่ได้รับ
    /// </summary>
    public DateTime ReceivedAt { get; set; }

    #endregion

    #region ข้อมูลที่ Parse ได้

    /// <summary>
    /// ชื่อธนาคาร
    /// </summary>
    public string? BankName { get; set; }

    /// <summary>
    /// ประเภท (รับโอน/โอนออก)
    /// </summary>
    public string? TransactionType { get; set; }

    /// <summary>
    /// จำนวนเงิน
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// เลขบัญชีผู้โอน (บางส่วน)
    /// </summary>
    public string? TransferFrom { get; set; }

    /// <summary>
    /// วันที่โอน
    /// </summary>
    public DateTime? TransferDate { get; set; }

    /// <summary>
    /// เลขอ้างอิง
    /// </summary>
    public string? ReferenceNo { get; set; }

    /// <summary>
    /// ยอดคงเหลือ
    /// </summary>
    public decimal? Balance { get; set; }

    #endregion

    #region Matching

    /// <summary>
    /// Match กับ Payment แล้วหรือยัง
    /// </summary>
    public bool IsMatched { get; set; } = false;

    /// <summary>
    /// Payment ที่ Match (FK)
    /// </summary>
    public Guid? PaymentId { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// Payment ที่ Match
    /// </summary>
    public virtual Payment? Payment { get; set; }

    #endregion
}
