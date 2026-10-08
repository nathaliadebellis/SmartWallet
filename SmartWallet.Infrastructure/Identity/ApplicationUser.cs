using Microsoft.AspNetCore.Identity;

namespace SmartWallet.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public const int FullNameMaxLength = 100;

    public string? FullName { get; set; }
}
