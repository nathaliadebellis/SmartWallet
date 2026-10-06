using System.Threading.Tasks;

namespace SmartWallet.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
