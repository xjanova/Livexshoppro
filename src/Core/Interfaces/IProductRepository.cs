// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Product Repository Interface
//  Interface สำหรับจัดการข้อมูลสินค้า
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using LiveXShopPro.Core.Entities;

namespace LiveXShopPro.Core.Interfaces;

/// <summary>
/// Repository Interface สำหรับสินค้า
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    /// <summary>
    /// ค้นหาสินค้าจาก SKU
    /// </summary>
    Task<Product?> GetBySKUAsync(string sku, CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาสินค้าจาก Barcode
    /// </summary>
    Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาสินค้าจาก Live Code (CF1, CF2, ...)
    /// </summary>
    Task<Product?> GetByLiveCodeAsync(string liveCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาสินค้าตามชื่อ (Partial Match)
    /// </summary>
    Task<IEnumerable<Product>> SearchAsync(
        string searchTerm,
        int take = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงสินค้าพร้อม Variants
    /// </summary>
    Task<Product?> GetWithVariantsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงสินค้าพร้อมรูปภาพ
    /// </summary>
    Task<Product?> GetWithImagesAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงสินค้าตามหมวดหมู่
    /// </summary>
    Task<IEnumerable<Product>> GetByCategoryAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงสินค้าที่ Active และพร้อมขาย
    /// </summary>
    Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงสินค้าที่สต็อกต่ำ
    /// </summary>
    Task<IEnumerable<Product>> GetLowStockProductsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// อัพเดทสต็อก
    /// </summary>
    Task UpdateStockAsync(
        Guid productId,
        int quantityChange,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// จองสต็อก (เมื่อสร้างออเดอร์)
    /// </summary>
    Task ReserveStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ปล่อยสต็อกที่จอง (เมื่อยกเลิกออเดอร์)
    /// </summary>
    Task ReleaseStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default);
}
