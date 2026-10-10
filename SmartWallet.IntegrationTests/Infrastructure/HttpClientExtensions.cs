using System.Net;
using System.Text.RegularExpressions;

namespace SmartWallet.IntegrationTests.Infrastructure;

public static partial class HttpClientExtensions
{
    public const string DefaultPassword = "Senha@1234";

    public static string NewEmail() => $"user-{Guid.NewGuid():N}@smartwallet.test";

    /// <summary>
    /// Carrega <paramref name="formUrl"/> para obter o antiforgery token e envia
    /// o formulário para <paramref name="postUrl"/>.
    /// </summary>
    public static async Task<HttpResponseMessage> PostFormAsync(
        this HttpClient client,
        string formUrl,
        string postUrl,
        Dictionary<string, string> fields)
    {
        var token = await client.GetAntiforgeryTokenAsync(formUrl);

        var content = new Dictionary<string, string>(fields)
        {
            ["__RequestVerificationToken"] = token
        };

        return await client.PostAsync(postUrl, new FormUrlEncodedContent(content));
    }

    public static Task<HttpResponseMessage> RegisterAsync(
        this HttpClient client,
        string email,
        string password = DefaultPassword)
    {
        return client.PostFormAsync("/Account/Register", "/Account/Register", new Dictionary<string, string>
        {
            ["Name"] = "Usuário de Teste",
            ["Email"] = email,
            ["Password"] = password,
            ["ConfirmPassword"] = password
        });
    }

    public static Task<HttpResponseMessage> LoginAsync(
        this HttpClient client,
        string email,
        string password = DefaultPassword)
    {
        return client.PostFormAsync("/Account/Login", "/Account/Login", new Dictionary<string, string>
        {
            ["Email"] = email,
            ["Password"] = password
        });
    }

    public static async Task<string> GetDecodedStringAsync(this HttpClient client, string url)
    {
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        return await response.ReadDecodedContentAsync();
    }

    public static async Task<string> ReadDecodedContentAsync(this HttpResponseMessage response)
    {
        // O Razor codifica caracteres acentuados como entidades HTML.
        return WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync());
    }

    private static async Task<string> GetAntiforgeryTokenAsync(this HttpClient client, string formUrl)
    {
        var response = await client.GetAsync(formUrl);

        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();

        var match = AntiforgeryTokenRegex().Match(html);

        if (!match.Success)
            throw new InvalidOperationException($"Antiforgery token não encontrado em '{formUrl}'.");

        return match.Groups[1].Value;
    }

    [GeneratedRegex("name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"")]
    private static partial Regex AntiforgeryTokenRegex();
}
