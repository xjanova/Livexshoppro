// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Order Repository Interface
//  Interface สำหรับจัดการข้อมูลออเดอร์
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using LiveXShopPro.Core.Entities;
using LiveXShopPro.Core.Enums;

namespace LiveXShopPro.Core.Interfaces;

/// <summary>
/// Repository Interface สำหรับออเดอร์
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// ค้นหาออเดอร์จากเลขที่ออเดอร์
    /// </summary>
    Task<Order?> GetByOrderNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาออเดอร์จาก Tracking Number
    /// </summary>
    Task<Order?> GetByTrackingNumberAsync(
        string trackingNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงออเดอร์พร้อมรายการสินค้า
    /// </summary>
    Task<Order?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงออเดอร์พร้อมข้อมูลทั้งหมด (Items, Customer, Payments, Shipment)
    /// </summary>
    Task<Order?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงออเดอร์ของลูกค้า
    /// </summary>
    Task<IEnumerable<Order>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงออเดอร์ตามสถานะ
    /// </summary>
    Task<IEnumerable<Order>> GetByStatusAsync(
        OrderStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงออเดอร์ที่รอชำระเงิน
    /// </summary>
    Task<IEnumerable<Order>> GetPendingPaymentOrdersAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงออเดอร์ที่พร้อมแพค
    /// </summary>
    Task<IEnumerable<Order>> GetReadyToPackOrdersAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงออเดอร์ที่พร้อมส่ง
    /// </summary>
    Task<IEnumerable<Order>> GetReadyToShipOrdersAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงออเดอร์จากเซสชันไลฟ์
    /// </summary>
    Task<IEnumerable<Order>> GetByLiveSessionAsync(
        Guid liveSessionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงออเดอร์วันนี้
    /// </summary>
    Task<IEnumerable<Order>> GetTodayOrdersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// สร้างเลขที่ออเดอร์ใหม่
    /// </summary>
    Task<string> GenerateOrderNumberAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// อัพเดทสถานะออเดอร์
    /// </summary>
    Task UpdateStatusAsync(
        Guid orderId,
        OrderStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// อัพเดทสถานะการชำระเงิน
    /// </summary>
    Task UpdatePaymentStatusAsync(
        Guid orderId,
        PaymentStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงยอดขายตามช่วงเวลา
    /// </summary>
    Task<decimal> GetTotalSalesAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงจำนวนออเดอร์ตามช่วงเวลา
    /// </summary>
    Task<int> GetOrderCountAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาออเดอร์ (ชื่อลูกค้า, เบอร์โทร, เลขออเดอร์)
    /// </summary>
    Task<IEnumerable<Order>> SearchAsync(
        string searchTerm,
        int take = 20,
        CancellationToken cancellationToken = default);
}
