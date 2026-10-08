using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SmartWallet.Application.Interfaces;

namespace SmartWallet.Infrastructure.Services;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string toEmail, string subject, string htmlBody)
    {
        var section = _configuration.GetSection("Email:Smtp");

        var host = section["Host"]!;
        var port = int.TryParse(section["Port"], out var p) ? p : 587;
        var from = section["From"] ?? section["User"]!;

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = !string.Equals(section["EnableSsl"], "false", StringComparison.OrdinalIgnoreCase),
            Credentials = string.IsNullOrEmpty(section["User"])
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(section["User"], section["Password"])
        };

        using var message = new MailMessage(from, toEmail, subject, htmlBody)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}
