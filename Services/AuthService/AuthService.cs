using Back.Models;
using FirebaseAdmin.Auth;

namespace Back.Services.AuthService;

public class AuthService : IAuthInterface
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(string email, string password)
    {
        var userArgs = new UserRecordArgs
        {
            Email = email,
            Password = password
        };

        var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

        return userRecord.Uid;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var requestPayload = new
        {
            email,
            password,
            returnSecureToken = true
        };

        var tokenUri = Environment.GetEnvironmentVariable("FIREBASE_TOKEN_URI");

        HttpResponseMessage response;

        response = await _httpClient.PostAsJsonAsync(tokenUri, requestPayload);

        var responseContent = await response.Content.ReadFromJsonAsync<FirebaseUser>();

        return responseContent.idToken;
    }

    public async Task<UserRecord> GetUserAsync(string uid)
    {
        var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
        return userRecord;
    }
}