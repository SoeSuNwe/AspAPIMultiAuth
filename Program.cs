using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using AspAPIMultiAuth.dto;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// JWT Settings
builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection("JwtSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddAuthentication("CompositeScheme")
                .AddPolicyScheme("CompositeScheme", "Multi-Auth", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var headers = context.Request.Headers; 
                        if (headers.TryGetValue("Authorization", out var authHeader))
                        {
                            if (authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                                return "JwtScheme";
                            if (authHeader.ToString().StartsWith("Negotiate", StringComparison.OrdinalIgnoreCase))
                                return "WindowsScheme";
                        }
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
                .AddNegotiate("WindowsScheme", options => { });

builder.Services.AddAuthorization();

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version")
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        // Allow CORS from both HTTP and HTTPS for localhost
        policy.WithOrigins("http://localhost:5184", "https://localhost:7184") // Add both HTTP and HTTPS
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
    }
});
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
