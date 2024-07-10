using System.Reflection;
using Back.Service.UserService;
using FirebaseAdmin;
using System.Text;
using Back.Context;
using Back.Models;
using Back.Service.ProdutoService;
using Back.Service.VendaService;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS"))
});

builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<IProdutoInterface, ProdutoService>();
builder.Services.AddScoped<IVendaInterface, VendaService>();

builder.Services.AddHttpClient<string>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Authentication:TokenUri"]);
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080);
    serverOptions.ListenAnyIP(8081);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = builder.Configuration["Authentication:ValidIssuer"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
    };
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplica as migrações do banco de dados
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Versão 1.0");
        x.InjectStylesheet("/css/swaggerDark.css");
        x.RoutePrefix = string.Empty;
    });
}

app.UseStaticFiles();

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();