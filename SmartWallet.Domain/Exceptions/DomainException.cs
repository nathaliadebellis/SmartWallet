using System;

namespace SmartWallet.Domain.Exceptions;

/// <summary>
/// Base exception type for domain/business rule errors.
/// Throw this (or a derived type) from domain or application services when a business rule is violated.
/// </summary>
public class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string message)
        : base(message)
    {
    }

    public DomainException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
