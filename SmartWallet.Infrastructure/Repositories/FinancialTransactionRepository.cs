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

    public async Task AddAsync(FinancialTransaction transaction)
    {
        await _context.FinancialTransactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FinancialTransaction transaction)
    {
        _context.FinancialTransactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(FinancialTransaction transaction)
    {
        _context.FinancialTransactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }
}