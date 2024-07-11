using Back.Models;
using FirebaseAdmin.Auth;

namespace Back.Service.UserService;

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
        try
        {
            response = await _httpClient.PostAsJsonAsync(tokenUri, requestPayload);
        }
        catch (HttpRequestException e)
        {
            throw new Exception("An error occurred while sending the request to Firebase Auth.", e);
        }
        
        var responseContent = await response.Content.ReadFromJsonAsync<FirebaseAuthResponse>();
        
        return responseContent.idToken;
    }

    public class FirebaseAuthResponse
    {
        public string localId { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public string idToken { get; set; }
        public bool registered { get; set; }
        public string refreshToken { get; set; }
        public string expiresIn { get; set; }
    }
}