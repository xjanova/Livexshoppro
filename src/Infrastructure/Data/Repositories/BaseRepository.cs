// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Base Repository
//  Repository พื้นฐานสำหรับ CRUD Operations
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LiveXShopPro.Core.Entities;
using LiveXShopPro.Core.Interfaces;
using LiveXShopPro.Infrastructure.Data.Context;

namespace LiveXShopPro.Infrastructure.Data.Repositories;

/// <summary>
/// Repository พื้นฐานที่ implement IRepository
/// รองรับ CRUD operations สำหรับทุก Entity
/// </summary>
/// <typeparam name="T">Entity Type ที่สืบทอดจาก BaseEntity</typeparam>
public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// สร้าง BaseRepository
    /// </summary>
    /// <param name="context">Database Context</param>
    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// ดึงข้อมูลทั้งหมด
    /// </summary>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .ToListAsync();
    }

    /// <summary>
    /// ดึงข้อมูลตาม ID
    /// </summary>
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    /// <summary>
    /// ค้นหาข้อมูลตามเงื่อนไข
    /// </summary>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .ToListAsync();
    }

    /// <summary>
    /// ค้นหาข้อมูลแรกที่ตรงเงื่อนไข
    /// </summary>
    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// ตรวจสอบว่ามีข้อมูลที่ตรงเงื่อนไขหรือไม่
    /// </summary>
    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(e => !e.IsDeleted)
            .AnyAsync(predicate);
    }

    /// <summary>
    /// นับจำนวนข้อมูลตามเงื่อนไข
    /// </summary>
    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var query = _dbSet.Where(e => !e.IsDeleted);

        if (predicate != null)
            query = query.Where(predicate);

        return await query.CountAsync();
    }

    /// <summary>
    /// เพิ่มข้อมูลใหม่
    /// </summary>
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// เพิ่มข้อมูลหลายรายการ
    /// </summary>
    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    /// <summary>
    /// อัพเดทข้อมูล
    /// </summary>
    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// ลบข้อมูล (Soft Delete)
    /// </summary>
    public virtual void Delete(T entity)
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    /// <summary>
    /// ลบข้อมูลหลายรายการ (Soft Delete)
    /// </summary>
    public virtual void DeleteRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
        }
        _dbSet.UpdateRange(entities);
    }

    /// <summary>
    /// ลบข้อมูลถาวร (Hard Delete)
    /// </summary>
    public virtual void HardDelete(T entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// กู้คืนข้อมูลที่ลบไป
    /// </summary>
    public virtual void Restore(T entity)
    {
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        _dbSet.Update(entity);
    }
}
