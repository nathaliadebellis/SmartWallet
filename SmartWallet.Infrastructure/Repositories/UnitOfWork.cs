using System.Threading.Tasks;
using SmartWallet.Application.Interfaces;
using SmartWallet.Infrastructure.Data;

namespace SmartWallet.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
