using Back.Models;

namespace Back.Service.UserService;

public interface IAuthInterface
{
    Task<string> RegisterAsync(string email, string password);
    Task<string> LoginAsync(string email, string password);
}