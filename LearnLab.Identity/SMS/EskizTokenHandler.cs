using LearnLab.Core.Exceptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
namespace LearnLab.Identity.SMS;

public class EskizTokenHandler
{
    private readonly SmsClientOptions _options;
    private string Token { get; set; } = string.Empty;
    public EskizTokenHandler(IOptions<SmsClientOptions> options)
    {
        _options = options.Value;
    }
    public async Task<string> GetToken()
    {
        if (string.IsNullOrEmpty(Token))
        {
            await LogInToEskiz();
        }
        return Token;
    }
    private async Task LogInToEskiz()
    {
        var profile_email = _options.EskizEmail;
        var profile_password = _options.EskizPassword;

        using var httpClient = new HttpClient();

        var stringContent = JsonConvert.SerializeObject(new
        {
            email = profile_email,
            password = profile_password
        });
        var content = new StringContent(stringContent, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(_options.UrlForLoginToEskiz, content);
       
        if (!response.IsSuccessStatusCode)
        {
            Token = string.Empty;
            throw new LearnLabException("Failed to login to Eskiz");
        }
       
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseData = JObject.Parse(responseContent);
        var token = responseData["data"]["token"].ToString();
        Token = token;
    }
    public async Task RefreshToken()
    {
        using var httpClient = new HttpClient();
       
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
       
        var response = await httpClient.PatchAsync(_options.UrlForRefreshEskizToken, null);
       
        if (!response.IsSuccessStatusCode)
        {
            Token = string.Empty;
            throw new LearnLabException("Failed to refresh Eskiz token");
        }
       
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseData = JObject.Parse(responseContent);
        Token = responseData["data"]["token"].ToString();
    }
}
