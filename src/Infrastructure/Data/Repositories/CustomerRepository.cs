// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Customer Repository
//  Repository สำหรับจัดการข้อมูลลูกค้า
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore;
using LiveXShopPro.Core.Entities;
using LiveXShopPro.Core.Interfaces;
using LiveXShopPro.Infrastructure.Data.Context;

namespace LiveXShopPro.Infrastructure.Data.Repositories;

/// <summary>
/// Repository สำหรับจัดการข้อมูลลูกค้า
/// </summary>
public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    /// <summary>
    /// สร้าง CustomerRepository
    /// </summary>
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// ค้นหาลูกค้าจาก Facebook ID
    /// </summary>
    public async Task<Customer?> GetByFacebookIdAsync(string facebookId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.FacebookId == facebookId && !c.IsDeleted);
    }

    /// <summary>
    /// ค้นหาลูกค้าจาก TikTok ID
    /// </summary>
    public async Task<Customer?> GetByTikTokIdAsync(string tiktokId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.TikTokId == tiktokId && !c.IsDeleted);
    }

    /// <summary>
    /// ค้นหาลูกค้าจาก LINE ID
    /// </summary>
    public async Task<Customer?> GetByLineIdAsync(string lineId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.LineId == lineId && !c.IsDeleted);
    }

    /// <summary>
    /// ค้นหาลูกค้าจากเบอร์โทรศัพท์
    /// </summary>
    public async Task<Customer?> GetByPhoneAsync(string phone)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Phone == phone && !c.IsDeleted);
    }

    /// <summary>
    /// ค้นหาลูกค้าที่ซ้ำกัน (ชื่อคล้ายกัน + ข้อมูลบางส่วนตรงกัน)
    /// </summary>
    public async Task<IEnumerable<Customer>> FindDuplicatesAsync(string name, string? phone = null, string? address = null)
    {
        var query = _dbSet.Where(c => !c.IsDeleted);

        // ค้นหาชื่อที่คล้ายกัน
        if (!string.IsNullOrEmpty(name))
        {
            var nameLower = name.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(nameLower) ||
                                    nameLower.Contains(c.Name.ToLower()));
        }

        // ถ้ามีเบอร์โทร ให้เช็คด้วย
        if (!string.IsNullOrEmpty(phone))
        {
            var normalizedPhone = NormalizePhone(phone);
            query = query.Where(c => c.Phone != null &&
                                    c.Phone.Replace("-", "").Replace(" ", "") == normalizedPhone);
        }

        return await query.Take(20).ToListAsync();
    }

    /// <summary>
    /// ค้นหาลูกค้าทั่วไป (ชื่อ, เบอร์โทร, หรือ Social ID)
    /// </summary>
    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm, int skip = 0, int take = 20)
    {
        var termLower = searchTerm.ToLower();

        return await _dbSet
            .Where(c => !c.IsDeleted)
            .Where(c => c.Name.ToLower().Contains(termLower) ||
                       (c.Phone != null && c.Phone.Contains(searchTerm)) ||
                       (c.FacebookId != null && c.FacebookId.ToLower().Contains(termLower)) ||
                       (c.TikTokId != null && c.TikTokId.ToLower().Contains(termLower)) ||
                       (c.LineId != null && c.LineId.ToLower().Contains(termLower)))
            .OrderByDescending(c => c.TotalOrders)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงลูกค้าพร้อมออเดอร์ล่าสุด
    /// </summary>
    public async Task<Customer?> GetWithRecentOrdersAsync(Guid customerId, int orderCount = 5)
    {
        return await _dbSet
            .Include(c => c.Orders
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .Take(orderCount))
            .FirstOrDefaultAsync(c => c.Id == customerId && !c.IsDeleted);
    }

    /// <summary>
    /// อัพเดทสถิติลูกค้า
    /// </summary>
    public async Task UpdateStatisticsAsync(Guid customerId)
    {
        var customer = await _dbSet
            .Include(c => c.Orders.Where(o => !o.IsDeleted))
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null) return;

        customer.TotalOrders = customer.Orders.Count;
        customer.TotalSpent = customer.Orders.Sum(o => o.TotalAmount);
        customer.LastOrderAt = customer.Orders
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefault()?.CreatedAt;

        _dbSet.Update(customer);
    }

    /// <summary>
    /// ดึงลูกค้า Top โดยยอดซื้อ
    /// </summary>
    public async Task<IEnumerable<Customer>> GetTopCustomersAsync(int count = 10)
    {
        return await _dbSet
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.TotalSpent)
            .Take(count)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงลูกค้าใหม่ในช่วงเวลา
    /// </summary>
    public async Task<IEnumerable<Customer>> GetNewCustomersAsync(DateTime from, DateTime to)
    {
        return await _dbSet
            .Where(c => !c.IsDeleted && c.CreatedAt >= from && c.CreatedAt <= to)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Normalize เบอร์โทรศัพท์
    /// </summary>
    private static string NormalizePhone(string phone)
    {
        return phone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
    }
}
