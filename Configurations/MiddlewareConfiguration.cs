namespace Back.Configurations;

public static class MiddlewareConfiguration
{
    public static void ConfigureMiddlewares(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
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
        
    }
}