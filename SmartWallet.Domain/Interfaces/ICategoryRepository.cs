using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;

namespace SmartWallet.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();

    Task<IEnumerable<Category>> GetByTransactionTypeAsync(
        TransactionType transactionType);

    Task<Category?> GetByIdAsync(int id);

    Task<bool> ExistsByNameAsync(string name);

    Task<bool> ExistsByNameAsync(string name, int ignoreId);

    Task AddAsync(Category category);

    Task UpdateAsync(Category category);

    Task DeleteAsync(Category category);
}