using Microsoft.EntityFrameworkCore;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Infrastructure.Data;

namespace SmartWallet.Infrastructure.Repositories;

public class FinancialTransactionRepository : IFinancialTransactionRepository
{
    private readonly ApplicationDbContext _context;

    public FinancialTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FinancialTransaction>> GetAllAsync()
    {
        return await _context.FinancialTransactions
            .Include(t => t.Category)
            .AsNoTracking()
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<FinancialTransaction?> GetByIdAsync(int id)
    {
        return await _context.FinancialTransactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<FinancialTransaction>> GetByUserAsync(string userId)
    {
        return await _context.FinancialTransactions
            .Where(t => t.ApplicationUserId == userId)
            .Include(t => t.Category)
            .AsNoTracking()
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalByUserAndTypeAsync(string userId, Domain.Enums.TransactionType type)
    {
        return await _context.FinancialTransactions
            .Where(t => t.ApplicationUserId == userId && t.Type == type)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
    }

    public async Task<IEnumerable<FinancialTransaction>> GetLatestByUserAsync(string userId, int count)
    {
        return await _context.FinancialTransactions
            .Where(t => t.ApplicationUserId == userId)
            .Include(t => t.Category)
            .AsNoTracking()
            .OrderByDescending(t => t.TransactionDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task AddAsync(FinancialTransaction transaction)
    {
        await _context.FinancialTransactions.AddAsync(transaction);
    }

    public async Task UpdateAsync(FinancialTransaction transaction)
    {
        _context.FinancialTransactions.Update(transaction);
    }

    public async Task DeleteAsync(FinancialTransaction transaction)
    {
        _context.FinancialTransactions.Remove(transaction);
    }
}