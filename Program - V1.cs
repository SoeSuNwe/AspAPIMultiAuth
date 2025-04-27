//using AspAPIMultiAuth.dto;
//using Microsoft.AspNetCore.Authentication.JwtBearer; 
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);

//// Bind JwtSettings
//var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//{
//    options.Authority = jwtSettings.Authority;
//    options.RequireHttpsMetadata = jwtSettings.RequireHttpsMetadata;

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = jwtSettings.ValidIssuer,
//        ValidateAudience = true,
//        ValidAudiences = jwtSettings.ValidAudiences,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true
//    };
//})
//    .AddNegotiate(); // Enable Windows Authentication (Negotiate)

//builder.Services.AddAuthorization();

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Windows Auth API",
//        Version = "v1"
//    });
//});
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        // Allow CORS from both HTTP and HTTPS for localhost
//        policy.WithOrigins("http://localhost:5184", "https://localhost:7184") // Add both HTTP and HTTPS
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});
//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("https://localhost:7184/swagger/v1/swagger.json", "Windows Auth API v1");
//        c.RoutePrefix = string.Empty;
//    });
//}
//app.UseCors();
//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();
//app.Run();
