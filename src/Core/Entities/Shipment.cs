// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Shipment Entity
//  ข้อมูลการจัดส่ง
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using LiveXShopPro.Core.Enums;

namespace LiveXShopPro.Core.Entities;

/// <summary>
/// Entity สำหรับเก็บข้อมูลการจัดส่ง
/// </summary>
public class Shipment : BaseEntity
{
    #region Foreign Keys

    /// <summary>
    /// ออเดอร์ (FK) - 1:1 Relationship
    /// </summary>
    public Guid OrderId { get; set; }

    #endregion

    #region ข้อมูลขนส่ง

    /// <summary>
    /// ผู้ให้บริการขนส่ง
    /// </summary>
    public ShippingCarrier Carrier { get; set; }

    /// <summary>
    /// เลข Tracking
    /// </summary>
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// ประเภทบริการ (ปกติ/ด่วน)
    /// </summary>
    public string? ServiceType { get; set; }

    #endregion

    #region ข้อมูลพัสดุ

    /// <summary>
    /// น้ำหนัก (กรัม)
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// ความกว้าง (ซม.)
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// ความสูง (ซม.)
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// ความยาว (ซม.)
    /// </summary>
    public decimal? Length { get; set; }

    #endregion

    #region ค่าใช้จ่าย

    /// <summary>
    /// ค่าจัดส่ง (บาท)
    /// </summary>
    public decimal? ShippingCost { get; set; }

    /// <summary>
    /// ยอด COD (ถ้ามี)
    /// </summary>
    public decimal? CodAmount { get; set; }

    /// <summary>
    /// ค่าธรรมเนียม COD
    /// </summary>
    public decimal? CodFee { get; set; }

    #endregion

    #region สถานะ

    /// <summary>
    /// สถานะการจัดส่ง
    /// </summary>
    public ShippingStatus Status { get; set; } = ShippingStatus.Pending;

    /// <summary>
    /// วันที่คาดว่าจะถึง
    /// </summary>
    public DateTime? EstimatedDeliveryDate { get; set; }

    #endregion

    #region ใบปะหน้า

    /// <summary>
    /// พิมพ์ใบปะหน้าแล้วหรือยัง
    /// </summary>
    public bool LabelPrinted { get; set; } = false;

    /// <summary>
    /// วันที่พิมพ์ใบปะหน้า
    /// </summary>
    public DateTime? LabelPrintedAt { get; set; }

    /// <summary>
    /// URL/Path ของใบปะหน้า
    /// </summary>
    public string? LabelUrl { get; set; }

    #endregion

    #region Tracking History

    /// <summary>
    /// ประวัติการ Track (JSON)
    /// [{"status": "...", "location": "...", "timestamp": "..."}]
    /// </summary>
    public string? TrackingHistory { get; set; }

    /// <summary>
    /// อัพเดทล่าสุดจาก Tracking
    /// </summary>
    public DateTime? LastTrackingUpdate { get; set; }

    #endregion

    #region Timestamps

    /// <summary>
    /// วันที่ส่ง (ส่งให้ขนส่ง)
    /// </summary>
    public DateTime? ShippedAt { get; set; }

    /// <summary>
    /// วันที่ขนส่งมารับพัสดุ
    /// </summary>
    public DateTime? PickedUpAt { get; set; }

    /// <summary>
    /// วันที่ส่งถึง
    /// </summary>
    public DateTime? DeliveredAt { get; set; }

    /// <summary>
    /// ผู้รับ (ชื่อคนที่เซ็นรับ)
    /// </summary>
    public string? ReceivedBy { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// ออเดอร์
    /// </summary>
    public virtual Order? Order { get; set; }

    #endregion

    #region Helper Methods

    /// <summary>
    /// ดึงชื่อขนส่งเป็นภาษาไทย
    /// </summary>
    public string GetCarrierDisplayName()
    {
        return Carrier switch
        {
            ShippingCarrier.Kerry => "Kerry Express",
            ShippingCarrier.Flash => "Flash Express",
            ShippingCarrier.JAndT => "J&T Express",
            ShippingCarrier.ThaiPost => "ไปรษณีย์ไทย",
            ShippingCarrier.ShopeeExpress => "Shopee Express",
            ShippingCarrier.LazadaExpress => "Lazada Express",
            ShippingCarrier.DHL => "DHL",
            ShippingCarrier.NinjaVan => "Ninja Van",
            ShippingCarrier.BestExpress => "Best Express",
            ShippingCarrier.SCGExpress => "SCG Express",
            ShippingCarrier.Pickup => "รับเอง",
            _ => "อื่นๆ"
        };
    }

    /// <summary>
    /// สร้าง URL สำหรับ Track
    /// </summary>
    public string? GetTrackingUrl()
    {
        if (string.IsNullOrEmpty(TrackingNumber)) return null;

        return Carrier switch
        {
            ShippingCarrier.Kerry => $"https://th.kerryexpress.com/th/track/?track={TrackingNumber}",
            ShippingCarrier.Flash => $"https://www.flashexpress.co.th/tracking/?se={TrackingNumber}",
            ShippingCarrier.JAndT => $"https://www.jtexpress.co.th/index/query/gzquery.html?billcode={TrackingNumber}",
            ShippingCarrier.ThaiPost => $"https://track.thailandpost.co.th/?trackNumber={TrackingNumber}",
            _ => null
        };
    }

    #endregion
}
