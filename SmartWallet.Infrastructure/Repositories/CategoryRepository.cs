using Microsoft.EntityFrameworkCore;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Infrastructure.Data;

namespace SmartWallet.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(string userId)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.ApplicationUserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetByTransactionTypeAsync(
        TransactionType transactionType,
        string userId)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.ApplicationUserId == userId &&
                        c.TransactionType == transactionType)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id, string userId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.ApplicationUserId == userId);
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    public async Task AddRangeAsync(IEnumerable<Category> categories)
    {
        await _context.Categories.AddRangeAsync(categories);
    }

    public async Task<bool> ExistsByNameAsync(string name, string userId)
    {
        return await _context.Categories
            .AnyAsync(category =>
                category.ApplicationUserId == userId &&
                category.Name == name);
    }

    public async Task<bool> ExistsByNameAsync(string name, string userId, int ignoreId)
    {
        return await _context.Categories
            .AnyAsync(category =>
                category.ApplicationUserId == userId &&
                category.Name == name &&
                category.Id != ignoreId);
    }

    public async Task<bool> HasTransactionsAsync(int categoryId, string userId)
    {
        return await _context.FinancialTransactions
            .AnyAsync(t =>
                t.CategoryId == categoryId &&
                t.ApplicationUserId == userId);
    }

    public Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);

        return Task.CompletedTask;
    }
}
