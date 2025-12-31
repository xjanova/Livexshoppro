// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Order Repository
//  Repository สำหรับจัดการข้อมูลออเดอร์
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore;
using LiveXShopPro.Core.Entities;
using LiveXShopPro.Core.Enums;
using LiveXShopPro.Core.Interfaces;
using LiveXShopPro.Infrastructure.Data.Context;

namespace LiveXShopPro.Infrastructure.Data.Repositories;

/// <summary>
/// Repository สำหรับจัดการข้อมูลออเดอร์
/// </summary>
public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    /// <summary>
    /// สร้าง OrderRepository
    /// </summary>
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// ค้นหาออเดอร์จากเลขที่
    /// </summary>
    public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.Payments)
            .Include(o => o.Shipments)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber && !o.IsDeleted);
    }

    /// <summary>
    /// ดึงออเดอร์พร้อมรายละเอียดทั้งหมด
    /// </summary>
    public async Task<Order?> GetWithDetailsAsync(Guid orderId)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.Items)
                .ThenInclude(i => i.Variant)
            .Include(o => o.Payments)
            .Include(o => o.Shipments)
            .Include(o => o.LiveSession)
            .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);
    }

    /// <summary>
    /// ดึงออเดอร์ตามสถานะ
    /// </summary>
    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, int skip = 0, int take = 50)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .Where(o => !o.IsDeleted && o.Status == status)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงออเดอร์ของลูกค้า
    /// </summary>
    public async Task<IEnumerable<Order>> GetByCustomerAsync(Guid customerId, int skip = 0, int take = 20)
    {
        return await _dbSet
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.Payments)
            .Where(o => !o.IsDeleted && o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงออเดอร์ใน Live Session
    /// </summary>
    public async Task<IEnumerable<Order>> GetByLiveSessionAsync(Guid liveSessionId, int skip = 0, int take = 100)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Where(o => !o.IsDeleted && o.LiveSessionId == liveSessionId)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ค้นหาออเดอร์ทั่วไป
    /// </summary>
    public async Task<IEnumerable<Order>> SearchAsync(string searchTerm, int skip = 0, int take = 20)
    {
        var termLower = searchTerm.ToLower();

        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .Where(o => !o.IsDeleted)
            .Where(o => o.OrderNumber.ToLower().Contains(termLower) ||
                       (o.Customer != null && o.Customer.Name.ToLower().Contains(termLower)) ||
                       (o.Customer != null && o.Customer.Phone != null && o.Customer.Phone.Contains(searchTerm)) ||
                       (o.Notes != null && o.Notes.ToLower().Contains(termLower)))
            .OrderByDescending(o => o.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงออเดอร์ที่รอแพ็ค
    /// </summary>
    public async Task<IEnumerable<Order>> GetPendingPackingAsync(int skip = 0, int take = 50)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Where(o => !o.IsDeleted && o.Status == OrderStatus.Confirmed)
            .OrderBy(o => o.CreatedAt) // FIFO
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงออเดอร์ที่รอส่ง
    /// </summary>
    public async Task<IEnumerable<Order>> GetPendingShippingAsync(int skip = 0, int take = 50)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Shipments)
            .Where(o => !o.IsDeleted && o.Status == OrderStatus.Packed)
            .OrderBy(o => o.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงออเดอร์ในช่วงเวลา
    /// </summary>
    public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime from, DateTime to, int skip = 0, int take = 100)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .Where(o => !o.IsDeleted && o.CreatedAt >= from && o.CreatedAt <= to)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// สร้างเลขที่ออเดอร์ใหม่
    /// </summary>
    public async Task<string> GenerateOrderNumberAsync()
    {
        var today = DateTime.Today;
        var prefix = today.ToString("yyyyMMdd");

        // หาเลขล่าสุดของวันนี้
        var lastOrder = await _dbSet
            .Where(o => o.OrderNumber.StartsWith(prefix))
            .OrderByDescending(o => o.OrderNumber)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastOrder != null)
        {
            var lastNumberStr = lastOrder.OrderNumber.Substring(prefix.Length);
            if (int.TryParse(lastNumberStr, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"{prefix}{nextNumber:D4}";
    }

    /// <summary>
    /// สรุปยอดขายวันนี้
    /// </summary>
    public async Task<OrderSummary> GetTodaySummaryAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var orders = await _dbSet
            .Where(o => !o.IsDeleted && o.CreatedAt >= today && o.CreatedAt < tomorrow)
            .ToListAsync();

        return new OrderSummary
        {
            TotalOrders = orders.Count,
            TotalAmount = orders.Sum(o => o.TotalAmount),
            PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
            ConfirmedOrders = orders.Count(o => o.Status == OrderStatus.Confirmed),
            PackedOrders = orders.Count(o => o.Status == OrderStatus.Packed),
            ShippedOrders = orders.Count(o => o.Status == OrderStatus.Shipped),
            CompletedOrders = orders.Count(o => o.Status == OrderStatus.Completed),
            CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled)
        };
    }

    /// <summary>
    /// สรุปยอดขายตามช่วงเวลา
    /// </summary>
    public async Task<OrderSummary> GetSummaryAsync(DateTime from, DateTime to)
    {
        var orders = await _dbSet
            .Where(o => !o.IsDeleted && o.CreatedAt >= from && o.CreatedAt <= to)
            .ToListAsync();

        return new OrderSummary
        {
            TotalOrders = orders.Count,
            TotalAmount = orders.Sum(o => o.TotalAmount),
            PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
            ConfirmedOrders = orders.Count(o => o.Status == OrderStatus.Confirmed),
            PackedOrders = orders.Count(o => o.Status == OrderStatus.Packed),
            ShippedOrders = orders.Count(o => o.Status == OrderStatus.Shipped),
            CompletedOrders = orders.Count(o => o.Status == OrderStatus.Completed),
            CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled)
        };
    }

    /// <summary>
    /// นับออเดอร์ตามสถานะ
    /// </summary>
    public async Task<Dictionary<OrderStatus, int>> GetCountByStatusAsync()
    {
        return await _dbSet
            .Where(o => !o.IsDeleted)
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count);
    }
}

/// <summary>
/// สรุปข้อมูลออเดอร์
/// </summary>
public class OrderSummary
{
    /// <summary>จำนวนออเดอร์ทั้งหมด</summary>
    public int TotalOrders { get; set; }

    /// <summary>ยอดขายรวม</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>ออเดอร์รอดำเนินการ</summary>
    public int PendingOrders { get; set; }

    /// <summary>ออเดอร์ยืนยันแล้ว</summary>
    public int ConfirmedOrders { get; set; }

    /// <summary>ออเดอร์แพ็คแล้ว</summary>
    public int PackedOrders { get; set; }

    /// <summary>ออเดอร์จัดส่งแล้ว</summary>
    public int ShippedOrders { get; set; }

    /// <summary>ออเดอร์เสร็จสมบูรณ์</summary>
    public int CompletedOrders { get; set; }

    /// <summary>ออเดอร์ยกเลิก</summary>
    public int CancelledOrders { get; set; }
}
