// ═══════════════════════════════════════════════════════════════════════════════
//  Live x Shop Pro - Unit of Work
//  จัดการ Transaction และประสานงาน Repositories
//  พัฒนาโดย Xman Studio
// ═══════════════════════════════════════════════════════════════════════════════

using Microsoft.EntityFrameworkCore.Storage;
using LiveXShopPro.Core.Interfaces;
using LiveXShopPro.Infrastructure.Data.Context;
using LiveXShopPro.Infrastructure.Data.Repositories;

namespace LiveXShopPro.Infrastructure.Data;

/// <summary>
/// Unit of Work สำหรับจัดการ Transaction
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    private ICustomerRepository? _customers;
    private IProductRepository? _products;
    private IOrderRepository? _orders;

    /// <summary>
    /// สร้าง UnitOfWork
    /// </summary>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Customer Repository
    /// </summary>
    public ICustomerRepository Customers
        => _customers ??= new CustomerRepository(_context);

    /// <summary>
    /// Product Repository
    /// </summary>
    public IProductRepository Products
        => _products ??= new ProductRepository(_context);

    /// <summary>
    /// Order Repository
    /// </summary>
    public IOrderRepository Orders
        => _orders ??= new OrderRepository(_context);

    /// <summary>
    /// บันทึกการเปลี่ยนแปลงทั้งหมด
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// เริ่ม Transaction
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commit Transaction
    /// </summary>
    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Rollback Transaction
    /// </summary>
    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
