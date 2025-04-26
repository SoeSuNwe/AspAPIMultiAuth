using AspAPIMultiAuth.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AspAPIMultiAuth {
    public class Program {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind JwtSettings
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

            builder.Services.AddAuthentication("CompositeScheme")
                .AddPolicyScheme("CompositeScheme", "Multi-Auth", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var headers = context.Request.Headers;

                        if (headers.ContainsKey("X-API-Key")) return "ApiKeyScheme";
                        if (headers.TryGetValue("Authorization", out var authHeader))
                        {
                            if (authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                                return "JwtScheme";
                            if (authHeader.ToString().StartsWith("Negotiate", StringComparison.OrdinalIgnoreCase))
                                return "WindowsScheme";
                        }

                        if (context.Connection.ClientCertificate != null)
                            return "ClientCertScheme";

                        return "WindowsScheme";
                    };
                })
                .AddJwtBearer("JwtScheme", options =>
                {
                    options.Authority = jwtSettings.Authority;
                    options.RequireHttpsMetadata = jwtSettings.RequireHttpsMetadata;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.ValidIssuer,
                        ValidateAudience = true,
                        ValidAudiences = jwtSettings.ValidAudiences,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                })
                .AddNegotiate();

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Windows Auth API",
                    Version = "v1"
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:5184", "https://localhost:7184") // Add both HTTP and HTTPS
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new UrlSegmentApiVersionReader()
                );
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("https://localhost:7184/swagger/v1/swagger.json", "Windows Auth API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
