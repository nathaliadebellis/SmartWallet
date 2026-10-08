using Microsoft.Extensions.Logging;
using SmartWallet.Application.Interfaces;

namespace SmartWallet.Infrastructure.Services;

/// <summary>
/// Fallback used when no SMTP server is configured. Nothing is sent; in
/// Development the message body is written to the log so links can be tested.
/// </summary>
public class LoggingEmailSender : IEmailSender
{
    private readonly ILogger<LoggingEmailSender> _logger;
    private readonly bool _logBody;

    public LoggingEmailSender(
        ILogger<LoggingEmailSender> logger,
        bool logBody)
    {
        _logger = logger;
        _logBody = logBody;
    }

    public Task SendAsync(string toEmail, string subject, string htmlBody)
    {
        if (_logBody)
        {
            _logger.LogWarning(
                "SMTP não configurado. E-mail para {To} - {Subject}: {Body}",
                toEmail, subject, htmlBody);
        }
        else
        {
            _logger.LogError(
                "SMTP não configurado (Email:Smtp). E-mail '{Subject}' não foi enviado.",
                subject);
        }

        return Task.CompletedTask;
    }
}
