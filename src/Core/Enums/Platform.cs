// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Platform Enum
//  แพลตฟอร์มที่รองรับ
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

namespace LiveXShopPro.Core.Enums;

/// <summary>
/// แพลตฟอร์มที่รองรับการดูดแชท
/// </summary>
public enum Platform
{
    /// <summary>
    /// Facebook Live
    /// </summary>
    Facebook = 0,

    /// <summary>
    /// TikTok Live
    /// </summary>
    TikTok = 1,

    /// <summary>
    /// LINE Official Account
    /// </summary>
    Line = 2,

    /// <summary>
    /// Instagram Live
    /// </summary>
    Instagram = 3,

    /// <summary>
    /// YouTube Live
    /// </summary>
    YouTube = 4,

    /// <summary>
    /// Shopee Live
    /// </summary>
    Shopee = 5,

    /// <summary>
    /// Lazada Live
    /// </summary>
    Lazada = 6,

    /// <summary>
    /// อื่นๆ
    /// </summary>
    Other = 99
}

/// <summary>
/// แหล่งที่มาของออเดอร์
/// </summary>
public enum OrderSource
{
    /// <summary>
    /// จากไลฟ์ - ตรวจจับ CF อัตโนมัติ
    /// </summary>
    Live = 0,

    /// <summary>
    /// จาก POS - ขายหน้าร้าน
    /// </summary>
    POS = 1,

    /// <summary>
    /// สร้าง Manual - พิมพ์เอง
    /// </summary>
    Manual = 2,

    /// <summary>
    /// นำเข้าจากไฟล์
    /// </summary>
    Import = 3,

    /// <summary>
    /// จาก API
    /// </summary>
    API = 4
}

/// <summary>
/// ประเภทลูกค้า
/// </summary>
public enum CustomerType
{
    /// <summary>
    /// ลูกค้าปกติ
    /// </summary>
    Normal = 0,

    /// <summary>
    /// ลูกค้า VIP
    /// </summary>
    VIP = 1,

    /// <summary>
    /// ลูกค้าประจำ
    /// </summary>
    Regular = 2,

    /// <summary>
    /// ลูกค้าใหม่
    /// </summary>
    New = 3,

    /// <summary>
    /// ถูกบล็อก - มีปัญหา
    /// </summary>
    Blocked = 99
}

/// <summary>
/// ผู้ให้บริการขนส่ง
/// </summary>
public enum ShippingCarrier
{
    /// <summary>
    /// Kerry Express
    /// </summary>
    Kerry = 0,

    /// <summary>
    /// Flash Express
    /// </summary>
    Flash = 1,

    /// <summary>
    /// J&T Express
    /// </summary>
    JAndT = 2,

    /// <summary>
    /// ไปรษณีย์ไทย
    /// </summary>
    ThaiPost = 3,

    /// <summary>
    /// Shopee Express
    /// </summary>
    ShopeeExpress = 4,

    /// <summary>
    /// Lazada Express
    /// </summary>
    LazadaExpress = 5,

    /// <summary>
    /// DHL
    /// </summary>
    DHL = 6,

    /// <summary>
    /// NinjaVan
    /// </summary>
    NinjaVan = 7,

    /// <summary>
    /// Best Express
    /// </summary>
    BestExpress = 8,

    /// <summary>
    /// SCG Express
    /// </summary>
    SCGExpress = 9,

    /// <summary>
    /// รับเอง/ไม่จัดส่ง
    /// </summary>
    Pickup = 98,

    /// <summary>
    /// อื่นๆ
    /// </summary>
    Other = 99
}

/// <summary>
/// วิธีการชำระเงิน
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// โอนเงิน
    /// </summary>
    Transfer = 0,

    /// <summary>
    /// เงินสด
    /// </summary>
    Cash = 1,

    /// <summary>
    /// เก็บเงินปลายทาง (COD)
    /// </summary>
    COD = 2,

    /// <summary>
    /// บัตรเครดิต/เดบิต
    /// </summary>
    CreditCard = 3,

    /// <summary>
    /// PromptPay
    /// </summary>
    PromptPay = 4,

    /// <summary>
    /// TrueMoney Wallet
    /// </summary>
    TrueMoney = 5,

    /// <summary>
    /// Rabbit LINE Pay
    /// </summary>
    LinePay = 6,

    /// <summary>
    /// อื่นๆ
    /// </summary>
    Other = 99
}
