namespace SmartWallet.Domain.Common;

/// <summary>
/// Classe base para entidades do domínio.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }
}