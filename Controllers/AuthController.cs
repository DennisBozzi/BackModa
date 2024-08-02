using Back.Models;
using Back.Models.Dto;
using Back.Service.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthInterface _authInterface;

    public AuthController(IAuthInterface authInterface)
    {
        _authInterface = authInterface;
    }

    [HttpPost("Register")]
    public async Task<string> Register(EmailPasswordDto esDto)
    {
        return await _authInterface.RegisterAsync(esDto.email, esDto.password);
    }
    
    [HttpPost("Login")]
    public async Task<string> Login(EmailPasswordDto esDto)
    {
        return await _authInterface.LoginAsync(esDto.email, esDto.password);
    }

    [Authorize]
    [HttpGet("Teste")]
    public async Task<bool> Teste()
    {
        return true;
    }
}