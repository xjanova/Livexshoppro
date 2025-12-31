// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Repository Interfaces
//  Interface สำหรับ Repository Pattern
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Linq.Expressions;
using LiveXShopPro.Core.Entities;

namespace LiveXShopPro.Core.Interfaces;

/// <summary>
/// Generic Repository Interface
/// ใช้สำหรับ CRUD Operations พื้นฐาน
/// </summary>
/// <typeparam name="T">Entity Type ที่สืบทอดจาก BaseEntity</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    #region Read Operations

    /// <summary>
    /// ดึงข้อมูลตาม Id
    /// </summary>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงข้อมูลทั้งหมด
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงข้อมูลตามเงื่อนไข
    /// </summary>
    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ดึงข้อมูลแรกที่ตรงเงื่อนไข
    /// </summary>
    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// เช็คว่ามีข้อมูลที่ตรงเงื่อนไขหรือไม่
    /// </summary>
    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// นับจำนวนข้อมูล
    /// </summary>
    Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Write Operations

    /// <summary>
    /// เพิ่มข้อมูลใหม่
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// เพิ่มข้อมูลหลายรายการ
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// อัพเดทข้อมูล
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// อัพเดทข้อมูลหลายรายการ
    /// </summary>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// ลบข้อมูล
    /// </summary>
    void Remove(T entity);

    /// <summary>
    /// ลบข้อมูลหลายรายการ
    /// </summary>
    void RemoveRange(IEnumerable<T> entities);

    #endregion
}

/// <summary>
/// Unit of Work Interface
/// จัดการ Transaction และ Save Changes
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// บันทึกการเปลี่ยนแปลงทั้งหมด
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// เริ่ม Transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commit Transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rollback Transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
