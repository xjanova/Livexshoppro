// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Order Entity
//  ข้อมูลออเดอร์/คำสั่งซื้อ
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using LiveXShopPro.Core.Enums;

namespace LiveXShopPro.Core.Entities;

/// <summary>
/// Entity สำหรับเก็บข้อมูลออเดอร์
/// สร้างอัตโนมัติจาก CF หรือสร้างเอง
/// </summary>
public class Order : SoftDeleteEntity
{
    #region รหัสออเดอร์

    /// <summary>
    /// เลขที่ออเดอร์ (ORD-20241225-001)
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    #endregion

    #region ข้อมูลลูกค้า

    /// <summary>
    /// ลูกค้า (FK)
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// ชื่อลูกค้า (Snapshot ตอนสร้างออเดอร์)
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// เบอร์โทรลูกค้า (Snapshot)
    /// </summary>
    public string? CustomerPhone { get; set; }

    #endregion

    #region ที่อยู่จัดส่ง

    /// <summary>
    /// ที่อยู่จัดส่ง
    /// </summary>
    public string? ShippingAddress { get; set; }

    /// <summary>
    /// ตำบล/แขวง
    /// </summary>
    public string? ShippingSubDistrict { get; set; }

    /// <summary>
    /// อำเภอ/เขต
    /// </summary>
    public string? ShippingDistrict { get; set; }

    /// <summary>
    /// จังหวัด
    /// </summary>
    public string? ShippingProvince { get; set; }

    /// <summary>
    /// รหัสไปรษณีย์
    /// </summary>
    public string? ShippingPostalCode { get; set; }

    #endregion

    #region ยอดเงิน

    /// <summary>
    /// ยอดรวมสินค้า (ก่อนส่วนลด/ค่าส่ง)
    /// </summary>
    public decimal SubTotal { get; set; }

    /// <summary>
    /// ส่วนลด
    /// </summary>
    public decimal Discount { get; set; } = 0;

    /// <summary>
    /// รหัสส่วนลดที่ใช้
    /// </summary>
    public string? DiscountCode { get; set; }

    /// <summary>
    /// ค่าจัดส่ง
    /// </summary>
    public decimal ShippingFee { get; set; } = 0;

    /// <summary>
    /// ยอดรวมทั้งหมด
    /// </summary>
    public decimal Total { get; set; }

    #endregion

    #region สถานะ

    /// <summary>
    /// สถานะออเดอร์
    /// </summary>
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    /// <summary>
    /// สถานะการชำระเงิน
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

    /// <summary>
    /// สถานะการจัดส่ง
    /// </summary>
    public ShippingStatus ShippingStatus { get; set; } = ShippingStatus.Pending;

    #endregion

    #region แหล่งที่มา

    /// <summary>
    /// แหล่งที่มาของออเดอร์ (Live/POS/Manual)
    /// </summary>
    public OrderSource Source { get; set; } = OrderSource.Live;

    /// <summary>
    /// แพลตฟอร์ม (Facebook/TikTok/LINE)
    /// </summary>
    public Platform? Platform { get; set; }

    /// <summary>
    /// เซสชันไลฟ์ (FK) - ถ้ามาจากไลฟ์
    /// </summary>
    public Guid? LiveSessionId { get; set; }

    #endregion

    #region หมายเหตุ

    /// <summary>
    /// หมายเหตุจากลูกค้า
    /// </summary>
    public string? CustomerNote { get; set; }

    /// <summary>
    /// หมายเหตุจากแอดมิน
    /// </summary>
    public string? AdminNote { get; set; }

    #endregion

    #region Timestamps

    /// <summary>
    /// วันที่ชำระเงิน
    /// </summary>
    public DateTime? PaidAt { get; set; }

    /// <summary>
    /// วันที่จัดส่ง
    /// </summary>
    public DateTime? ShippedAt { get; set; }

    /// <summary>
    /// วันที่เสร็จสิ้น
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// วันที่ยกเลิก
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// เหตุผลในการยกเลิก
    /// </summary>
    public string? CancellationReason { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// ลูกค้า
    /// </summary>
    public virtual Customer? Customer { get; set; }

    /// <summary>
    /// รายการสินค้าในออเดอร์
    /// </summary>
    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    /// <summary>
    /// การชำระเงิน
    /// </summary>
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    /// <summary>
    /// การจัดส่ง
    /// </summary>
    public virtual Shipment? Shipment { get; set; }

    /// <summary>
    /// เซสชันไลฟ์
    /// </summary>
    public virtual LiveSession? LiveSession { get; set; }

    #endregion

    #region Helper Methods

    /// <summary>
    /// สร้างเลขที่ออเดอร์อัตโนมัติ
    /// </summary>
    public static string GenerateOrderNumber(int sequence)
    {
        return $"ORD-{DateTime.Now:yyyyMMdd}-{sequence:D3}";
    }

    /// <summary>
    /// ดึงที่อยู่จัดส่งแบบเต็ม
    /// </summary>
    public string GetFullShippingAddress()
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(ShippingAddress)) parts.Add(ShippingAddress);
        if (!string.IsNullOrWhiteSpace(ShippingSubDistrict)) parts.Add($"ต.{ShippingSubDistrict}");
        if (!string.IsNullOrWhiteSpace(ShippingDistrict)) parts.Add($"อ.{ShippingDistrict}");
        if (!string.IsNullOrWhiteSpace(ShippingProvince)) parts.Add($"จ.{ShippingProvince}");
        if (!string.IsNullOrWhiteSpace(ShippingPostalCode)) parts.Add(ShippingPostalCode);

        return string.Join(" ", parts);
    }

    /// <summary>
    /// คำนวณยอดรวมใหม่จากรายการสินค้า
    /// </summary>
    public void RecalculateTotal()
    {
        SubTotal = Items.Sum(i => i.Total);
        Total = SubTotal - Discount + ShippingFee;
    }

    /// <summary>
    /// เช็คว่าสามารถยกเลิกได้หรือไม่
    /// </summary>
    public bool CanCancel()
    {
        return Status != OrderStatus.Shipped &&
               Status != OrderStatus.InTransit &&
               Status != OrderStatus.Delivered &&
               Status != OrderStatus.Cancelled;
    }

    #endregion
}

/// <summary>
/// รายการสินค้าในออเดอร์
/// </summary>
public class OrderItem : BaseEntity
{
    #region Foreign Keys

    /// <summary>
    /// ออเดอร์ (FK)
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// สินค้า (FK)
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Variant (FK) - ถ้ามี
    /// </summary>
    public Guid? VariantId { get; set; }

    #endregion

    #region ข้อมูลสินค้า (Snapshot)

    /// <summary>
    /// ชื่อสินค้า
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// รหัส SKU
    /// </summary>
    public string? ProductSKU { get; set; }

    /// <summary>
    /// ชื่อ Variant (ถ้ามี)
    /// </summary>
    public string? VariantName { get; set; }

    #endregion

    #region ราคาและจำนวน

    /// <summary>
    /// จำนวน
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// ราคาต่อหน่วย
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// ส่วนลด
    /// </summary>
    public decimal Discount { get; set; } = 0;

    /// <summary>
    /// ยอดรวม = (UnitPrice * Quantity) - Discount
    /// </summary>
    public decimal Total { get; set; }

    #endregion

    #region สถานะการแพค

    /// <summary>
    /// แพคแล้วหรือยัง
    /// </summary>
    public bool IsPacked { get; set; } = false;

    /// <summary>
    /// วันที่แพค
    /// </summary>
    public DateTime? PackedAt { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// ออเดอร์
    /// </summary>
    public virtual Order? Order { get; set; }

    /// <summary>
    /// สินค้า
    /// </summary>
    public virtual Product? Product { get; set; }

    /// <summary>
    /// Variant
    /// </summary>
    public virtual ProductVariant? Variant { get; set; }

    #endregion

    #region Helper Methods

    /// <summary>
    /// คำนวณยอดรวม
    /// </summary>
    public void CalculateTotal()
    {
        Total = (UnitPrice * Quantity) - Discount;
    }

    #endregion
}
