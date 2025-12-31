// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Product Repository
//  Repository สำหรับจัดการข้อมูลสินค้า
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore;
using LiveXShopPro.Core.Entities;
using LiveXShopPro.Core.Interfaces;
using LiveXShopPro.Infrastructure.Data.Context;

namespace LiveXShopPro.Infrastructure.Data.Repositories;

/// <summary>
/// Repository สำหรับจัดการข้อมูลสินค้า
/// </summary>
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    /// <summary>
    /// สร้าง ProductRepository
    /// </summary>
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// ค้นหาสินค้าจาก SKU
    /// </summary>
    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _dbSet
            .Include(p => p.Variants.Where(v => !v.IsDeleted))
            .FirstOrDefaultAsync(p => p.SKU == sku && !p.IsDeleted);
    }

    /// <summary>
    /// ค้นหาสินค้าจาก Barcode
    /// </summary>
    public async Task<Product?> GetByBarcodeAsync(string barcode)
    {
        // ค้นหาจากสินค้าหลัก
        var product = await _dbSet
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Barcode == barcode && !p.IsDeleted);

        if (product != null) return product;

        // ค้นหาจาก Variant
        var variant = await _context.Set<ProductVariant>()
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.Barcode == barcode && !v.IsDeleted);

        return variant?.Product;
    }

    /// <summary>
    /// ค้นหาสินค้าจาก CF Code (ชื่อย่อ)
    /// </summary>
    public async Task<Product?> GetByCFCodeAsync(string cfCode)
    {
        var codeLower = cfCode.ToLower();
        return await _dbSet
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => (p.CFCode != null && p.CFCode.ToLower() == codeLower) && !p.IsDeleted);
    }

    /// <summary>
    /// ค้นหาสินค้าทั่วไป
    /// </summary>
    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm, int skip = 0, int take = 20)
    {
        var termLower = searchTerm.ToLower();

        return await _dbSet
            .Include(p => p.Variants.Where(v => !v.IsDeleted))
            .Where(p => !p.IsDeleted)
            .Where(p => p.Name.ToLower().Contains(termLower) ||
                       p.SKU.ToLower().Contains(termLower) ||
                       (p.CFCode != null && p.CFCode.ToLower().Contains(termLower)) ||
                       (p.Description != null && p.Description.ToLower().Contains(termLower)))
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงสินค้าตามหมวดหมู่
    /// </summary>
    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category, int skip = 0, int take = 20)
    {
        return await _dbSet
            .Include(p => p.Variants.Where(v => !v.IsDeleted))
            .Where(p => !p.IsDeleted && p.Category == category)
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงสินค้าที่ Active
    /// </summary>
    public async Task<IEnumerable<Product>> GetActiveProductsAsync(int skip = 0, int take = 50)
    {
        return await _dbSet
            .Include(p => p.Variants.Where(v => !v.IsDeleted))
            .Where(p => !p.IsDeleted && p.IsActive)
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงสินค้าที่ Stock ต่ำ
    /// </summary>
    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10)
    {
        return await _dbSet
            .Include(p => p.Variants.Where(v => !v.IsDeleted))
            .Where(p => !p.IsDeleted && p.IsActive && p.Stock <= threshold)
            .OrderBy(p => p.Stock)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงสินค้าที่หมด Stock
    /// </summary>
    public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
    {
        return await _dbSet
            .Include(p => p.Variants.Where(v => !v.IsDeleted))
            .Where(p => !p.IsDeleted && p.IsActive && p.Stock <= 0)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// อัพเดท Stock
    /// </summary>
    public async Task<bool> UpdateStockAsync(Guid productId, int quantity, string reason = "")
    {
        var product = await _dbSet.FirstOrDefaultAsync(p => p.Id == productId && !p.IsDeleted);
        if (product == null) return false;

        product.Stock += quantity;

        // ป้องกัน Stock ติดลบ
        if (product.Stock < 0)
            product.Stock = 0;

        _dbSet.Update(product);
        return true;
    }

    /// <summary>
    /// อัพเดท Stock ของ Variant
    /// </summary>
    public async Task<bool> UpdateVariantStockAsync(Guid variantId, int quantity, string reason = "")
    {
        var variant = await _context.Set<ProductVariant>()
            .FirstOrDefaultAsync(v => v.Id == variantId && !v.IsDeleted);

        if (variant == null) return false;

        variant.Stock += quantity;

        if (variant.Stock < 0)
            variant.Stock = 0;

        _context.Set<ProductVariant>().Update(variant);
        return true;
    }

    /// <summary>
    /// ตัด Stock (ลด)
    /// </summary>
    public async Task<bool> DeductStockAsync(Guid productId, int quantity)
    {
        return await UpdateStockAsync(productId, -quantity, "ตัด Stock จากการขาย");
    }

    /// <summary>
    /// คืน Stock (เพิ่มกลับ)
    /// </summary>
    public async Task<bool> RestoreStockAsync(Guid productId, int quantity)
    {
        return await UpdateStockAsync(productId, quantity, "คืน Stock จากการยกเลิก");
    }

    /// <summary>
    /// ดึงสินค้าขายดี
    /// </summary>
    public async Task<IEnumerable<Product>> GetTopSellingProductsAsync(DateTime from, DateTime to, int count = 10)
    {
        return await _dbSet
            .Include(p => p.Variants)
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.TotalSold)
            .Take(count)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงหมวดหมู่ทั้งหมด
    /// </summary>
    public async Task<IEnumerable<string>> GetAllCategoriesAsync()
    {
        return await _dbSet
            .Where(p => !p.IsDeleted && !string.IsNullOrEmpty(p.Category))
            .Select(p => p.Category!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }
}
