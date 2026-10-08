using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;

namespace SmartWallet.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync(string userId);

    Task<IEnumerable<Category>> GetByTransactionTypeAsync(
        TransactionType transactionType,
        string userId);

    Task<Category?> GetByIdAsync(int id, string userId);

    Task<bool> ExistsByNameAsync(string name, string userId);

    Task<bool> ExistsByNameAsync(string name, string userId, int ignoreId);

    Task<bool> HasTransactionsAsync(int categoryId, string userId);

    Task AddAsync(Category category);

    Task AddRangeAsync(IEnumerable<Category> categories);

    Task UpdateAsync(Category category);

    Task DeleteAsync(Category category);
}
