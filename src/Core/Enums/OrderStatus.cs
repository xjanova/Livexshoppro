// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Order Status Enum
//  สถานะต่างๆ ของออเดอร์
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

namespace LiveXShopPro.Core.Enums;

/// <summary>
/// สถานะของออเดอร์
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// รอดำเนินการ - เพิ่งสร้างออเดอร์
    /// </summary>
    Pending = 0,

    /// <summary>
    /// ยืนยันแล้ว - ตรวจสอบข้อมูลเรียบร้อย
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// กำลังเตรียมสินค้า - อยู่ระหว่างแพคของ
    /// </summary>
    Processing = 2,

    /// <summary>
    /// แพคเสร็จแล้ว - พร้อมส่ง
    /// </summary>
    Packed = 3,

    /// <summary>
    /// จัดส่งแล้ว - ส่งให้ขนส่งแล้ว
    /// </summary>
    Shipped = 4,

    /// <summary>
    /// กำลังจัดส่ง - อยู่ระหว่างขนส่ง
    /// </summary>
    InTransit = 5,

    /// <summary>
    /// ส่งถึงแล้ว - สำเร็จ
    /// </summary>
    Delivered = 6,

    /// <summary>
    /// ยกเลิก - ออเดอร์ถูกยกเลิก
    /// </summary>
    Cancelled = 7,

    /// <summary>
    /// คืนสินค้า - ลูกค้าส่งคืน
    /// </summary>
    Returned = 8,

    /// <summary>
    /// คืนเงิน - ดำเนินการคืนเงินแล้ว
    /// </summary>
    Refunded = 9
}

/// <summary>
/// สถานะการชำระเงิน
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// ยังไม่ชำระ
    /// </summary>
    Unpaid = 0,

    /// <summary>
    /// รอตรวจสอบ - ส่งสลิปแล้ว รอยืนยัน
    /// </summary>
    Pending = 1,

    /// <summary>
    /// ชำระแล้ว - ยืนยันเรียบร้อย
    /// </summary>
    Paid = 2,

    /// <summary>
    /// ชำระบางส่วน
    /// </summary>
    PartiallyPaid = 3,

    /// <summary>
    /// คืนเงินแล้ว
    /// </summary>
    Refunded = 4,

    /// <summary>
    /// ล้มเหลว - สลิปปลอม/ไม่ถูกต้อง
    /// </summary>
    Failed = 5,

    /// <summary>
    /// เก็บเงินปลายทาง
    /// </summary>
    COD = 6
}

/// <summary>
/// สถานะการจัดส่ง
/// </summary>
public enum ShippingStatus
{
    /// <summary>
    /// รอดำเนินการ
    /// </summary>
    Pending = 0,

    /// <summary>
    /// พร้อมจัดส่ง
    /// </summary>
    ReadyToShip = 1,

    /// <summary>
    /// รับพัสดุแล้ว
    /// </summary>
    PickedUp = 2,

    /// <summary>
    /// กำลังจัดส่ง
    /// </summary>
    InTransit = 3,

    /// <summary>
    /// ถึงศูนย์กระจายสินค้า
    /// </summary>
    AtHub = 4,

    /// <summary>
    /// ออกจัดส่ง
    /// </summary>
    OutForDelivery = 5,

    /// <summary>
    /// ส่งสำเร็จ
    /// </summary>
    Delivered = 6,

    /// <summary>
    /// จัดส่งไม่สำเร็จ
    /// </summary>
    DeliveryFailed = 7,

    /// <summary>
    /// ส่งคืน
    /// </summary>
    Returned = 8
}

/// <summary>
/// สถานะการตรวจสอบสลิป
/// </summary>
public enum SlipVerificationStatus
{
    /// <summary>
    /// รอตรวจสอบ
    /// </summary>
    Pending = 0,

    /// <summary>
    /// กำลังตรวจสอบ
    /// </summary>
    Processing = 1,

    /// <summary>
    /// ผ่านการตรวจสอบ
    /// </summary>
    Verified = 2,

    /// <summary>
    /// น่าสงสัย - ต้องตรวจสอบ Manual
    /// </summary>
    Suspicious = 3,

    /// <summary>
    /// สลิปปลอม
    /// </summary>
    Fake = 4,

    /// <summary>
    /// สลิปซ้ำ
    /// </summary>
    Duplicate = 5,

    /// <summary>
    /// หมดอายุ - สลิปเก่าเกินไป
    /// </summary>
    Expired = 6,

    /// <summary>
    /// ไม่ตรงกับออเดอร์
    /// </summary>
    Mismatched = 7
}

/// <summary>
/// โหมดการตรวจสอบสลิป
/// </summary>
public enum SlipVerificationMode
{
    /// <summary>
    /// อัตโนมัติ - ตรวจสอบด้วย AI/OCR
    /// </summary>
    Auto = 0,

    /// <summary>
    /// Manual - แอดมินตรวจสอบเอง
    /// </summary>
    Manual = 1,

    /// <summary>
    /// ผสม - ตามเงื่อนไขที่กำหนด
    /// </summary>
    Hybrid = 2
}
