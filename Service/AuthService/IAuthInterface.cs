﻿using Back.Models;
using FirebaseAdmin.Auth;

namespace Back.Service.UserService;

public interface IAuthInterface
{
    Task<string> RegisterAsync(string email, string password);
    Task<string> LoginAsync(string email, string password);
    Task<UserRecord> GetUserAsync(string uid);
}