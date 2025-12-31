// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Customer Repository Interface
//  Interface สำหรับจัดการข้อมูลลูกค้า
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using LiveXShopPro.Core.Entities;
using LiveXShopPro.Core.Enums;

namespace LiveXShopPro.Core.Interfaces;

/// <summary>
/// Repository Interface สำหรับลูกค้า
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    /// <summary>
    /// ค้นหาลูกค้าจากเบอร์โทร
    /// </summary>
    Task<Customer?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาลูกค้าจาก Facebook ID
    /// </summary>
    Task<Customer?> GetByFacebookIdAsync(string facebookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาลูกค้าจาก TikTok ID
    /// </summary>
    Task<Customer?> GetByTikTokIdAsync(string tiktokId, CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาลูกค้าจาก LINE User ID
    /// </summary>
    Task<Customer?> GetByLineUserIdAsync(string lineUserId, CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาลูกค้าตามชื่อ (Partial Match)
    /// </summary>
    Task<IEnumerable<Customer>> SearchByNameAsync(
        string searchTerm,
        int take = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงลูกค้าพร้อมออเดอร์
    /// </summary>
    Task<Customer?> GetWithOrdersAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงลูกค้าตามประเภท
    /// </summary>
    Task<IEnumerable<Customer>> GetByTypeAsync(
        CustomerType type,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// อัพเดทสถิติลูกค้า (ยอดซื้อ, จำนวนออเดอร์)
    /// </summary>
    Task UpdateStatisticsAsync(Guid customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// ค้นหาหรือสร้างลูกค้าใหม่จาก Social ID
    /// </summary>
    Task<Customer> GetOrCreateFromSocialAsync(
        Platform platform,
        string socialId,
        string displayName,
        string? profilePicture = null,
        CancellationToken cancellationToken = default);
}
