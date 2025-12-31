// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Product Entity
//  ข้อมูลสินค้า
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

namespace LiveXShopPro.Core.Entities;

/// <summary>
/// Entity สำหรับเก็บข้อมูลสินค้า
/// </summary>
public class Product : SoftDeleteEntity
{
    #region รหัสสินค้า

    /// <summary>
    /// รหัส SKU (Stock Keeping Unit)
    /// </summary>
    public string? SKU { get; set; }

    /// <summary>
    /// Barcode (EAN-13, UPC, etc.)
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// รหัสสำหรับไลฟ์ (CF1, CF2, ...)
    /// ใช้ในการตรวจจับจากแชท
    /// </summary>
    public string? LiveCode { get; set; }

    #endregion

    #region ข้อมูลสินค้า

    /// <summary>
    /// ชื่อสินค้า
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// รายละเอียดสินค้า
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// หมวดหมู่ (FK)
    /// </summary>
    public Guid? CategoryId { get; set; }

    #endregion

    #region ราคา

    /// <summary>
    /// ราคาขาย (บาท)
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// ราคาทุน (บาท)
    /// </summary>
    public decimal Cost { get; set; } = 0;

    /// <summary>
    /// ราคาเปรียบเทียบ/ราคาก่อนลด (บาท)
    /// </summary>
    public decimal? CompareAtPrice { get; set; }

    #endregion

    #region สต็อก

    /// <summary>
    /// จำนวนในสต็อก
    /// </summary>
    public int StockQuantity { get; set; } = 0;

    /// <summary>
    /// จำนวนที่จอง (รอชำระเงิน)
    /// </summary>
    public int ReservedQuantity { get; set; } = 0;

    /// <summary>
    /// ระดับแจ้งเตือน (แจ้งเตือนเมื่อสต็อกต่ำกว่า)
    /// </summary>
    public int ReorderLevel { get; set; } = 5;

    /// <summary>
    /// ติดตามสต็อกหรือไม่
    /// </summary>
    public bool TrackStock { get; set; } = true;

    /// <summary>
    /// อนุญาตให้ขายเกินสต็อกได้หรือไม่
    /// </summary>
    public bool AllowBackorder { get; set; } = false;

    #endregion

    #region รูปภาพ

    /// <summary>
    /// Path รูปหลัก
    /// </summary>
    public string? MainImagePath { get; set; }

    #endregion

    #region ข้อมูลสำหรับขนส่ง

    /// <summary>
    /// น้ำหนัก (กรัม)
    /// </summary>
    public decimal Weight { get; set; } = 0;

    /// <summary>
    /// ความกว้าง (ซม.)
    /// </summary>
    public decimal Width { get; set; } = 0;

    /// <summary>
    /// ความสูง (ซม.)
    /// </summary>
    public decimal Height { get; set; } = 0;

    /// <summary>
    /// ความยาว (ซม.)
    /// </summary>
    public decimal Length { get; set; } = 0;

    #endregion

    #region สถานะ

    /// <summary>
    /// เปิดใช้งาน/พร้อมขาย
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// สินค้าแนะนำ/โปรโมท
    /// </summary>
    public bool IsFeatured { get; set; } = false;

    /// <summary>
    /// ลำดับการแสดงผล
    /// </summary>
    public int SortOrder { get; set; } = 0;

    #endregion

    #region Navigation Properties

    /// <summary>
    /// หมวดหมู่สินค้า
    /// </summary>
    public virtual Category? Category { get; set; }

    /// <summary>
    /// ตัวเลือกสินค้า (ไซส์, สี)
    /// </summary>
    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    /// <summary>
    /// รูปภาพสินค้า
    /// </summary>
    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

    #endregion

    #region Helper Methods

    /// <summary>
    /// จำนวนที่พร้อมขาย = สต็อก - ที่จอง
    /// </summary>
    public int AvailableQuantity => StockQuantity - ReservedQuantity;

    /// <summary>
    /// เช็คว่าสต็อกต่ำหรือไม่
    /// </summary>
    public bool IsLowStock => TrackStock && AvailableQuantity <= ReorderLevel;

    /// <summary>
    /// เช็คว่าหมดสต็อกหรือไม่
    /// </summary>
    public bool IsOutOfStock => TrackStock && AvailableQuantity <= 0 && !AllowBackorder;

    /// <summary>
    /// คำนวณกำไร
    /// </summary>
    public decimal GetProfit() => Price - Cost;

    /// <summary>
    /// คำนวณ % กำไร
    /// </summary>
    public decimal GetProfitMargin()
    {
        if (Cost == 0) return 100;
        return ((Price - Cost) / Cost) * 100;
    }

    #endregion
}

/// <summary>
/// ตัวเลือกสินค้า (เช่น ไซส์, สี)
/// </summary>
public class ProductVariant : BaseEntity
{
    /// <summary>
    /// สินค้าหลัก (FK)
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// ชื่อตัวเลือก (เช่น "ไซส์ M - สีดำ")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// รหัส SKU ของ Variant
    /// </summary>
    public string? SKU { get; set; }

    /// <summary>
    /// Barcode ของ Variant
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// ราคา (ถ้าต่างจากสินค้าหลัก)
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// จำนวนในสต็อก
    /// </summary>
    public int StockQuantity { get; set; } = 0;

    /// <summary>
    /// เปิดใช้งาน
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// สินค้าหลัก
    /// </summary>
    public virtual Product? Product { get; set; }
}

/// <summary>
/// รูปภาพสินค้า
/// </summary>
public class ProductImage : BaseEntity
{
    /// <summary>
    /// สินค้า (FK)
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Path รูปภาพ
    /// </summary>
    public string ImagePath { get; set; } = string.Empty;

    /// <summary>
    /// ลำดับการแสดงผล
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// เป็นรูปหลักหรือไม่
    /// </summary>
    public bool IsMain { get; set; } = false;

    /// <summary>
    /// สินค้า
    /// </summary>
    public virtual Product? Product { get; set; }
}

/// <summary>
/// หมวดหมู่สินค้า
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// ชื่อหมวดหมู่
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// หมวดหมู่หลัก (สำหรับ Sub-category)
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// ลำดับการแสดงผล
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// เปิดใช้งาน
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// หมวดหมู่หลัก
    /// </summary>
    public virtual Category? Parent { get; set; }

    /// <summary>
    /// หมวดหมู่ย่อย
    /// </summary>
    public virtual ICollection<Category> Children { get; set; } = new List<Category>();

    /// <summary>
    /// สินค้าในหมวดหมู่
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
