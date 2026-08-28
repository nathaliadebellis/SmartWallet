using System;

namespace SmartWallet.Domain.Exceptions;

/// <summary>
/// Exception used when a requested resource/entity cannot be found.
/// </summary>
public class NotFoundException : DomainException
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
