# ASP.NET Core API with Multi-Authentication (JWT + Windows Authentication) and API Versioning

This project is an ASP.NET Core Web API that supports:
- **Multi-Authentication** (JWT Bearer tokens & Windows Authentication)
- **API Versioning** (via URL segment and custom headers)
- **Swagger/OpenAPI** with versioned endpoints
- **CORS Policy** for local development (HTTP & HTTPS)
- **JWT Settings** configurable via `appsettings.json`

## Features

- Composite authentication scheme that automatically selects **JWT** or **Windows Authentication** based on the `Authorization` header.
- API Versioning support through URL segments and custom headers (`x-api-version`).
- Fully integrated Swagger UI with version-specific documentation.
- CORS configuration for `http://localhost:5184` and `https://localhost:7184`.
- Secure with HTTPS redirection and authentication/authorization middleware.

## Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) or later
- Visual Studio 2022+ / Visual Studio Code
- Optional: Postman or Swagger UI for API testing

### Configuration

1. **Configure JWT Settings**

Make sure your `appsettings.json` includes a `JwtSettings` section like:

```json
"JwtSettings": {
  "Authority": "https://your-auth-server.com/",
  "RequireHttpsMetadata": true,
  "ValidIssuer": "your-issuer",
  "ValidAudiences": [
    "your-audience-1",
    "your-audience-2"
  ]
}
```

2. **Run the Application**

```bash
dotnet run
```

By default, the API will be available at:
- HTTPS: `https://localhost:7184`
- HTTP: `http://localhost:5184`

3. **Explore Swagger UI**

Navigate to:
```
https://localhost:7184/swagger
```
You will see versioned endpoints (`v1`, etc.).

## Authentication Flow

- If the `Authorization` header starts with `Bearer`, the **JWT Scheme** will be used.
- If it starts with `Negotiate` (or is missing), **Windows Authentication** will be used.

## API Versioning

The API supports two ways to specify version:
- URL segment: `http://localhost:5184/api/v1/weather`
- Custom header: `x-api-version: 1.0`

If no version is specified, it defaults to **v1.0**.

## Technologies Used

- ASP.NET Core
- JWT Authentication
- Windows Authentication (Negotiate)
- ASP.NET Core API Versioning (`Asp.Versioning`)
- Swagger / Swashbuckle
- CORS Policy Configuration

## Folder Structure

```
/dto
    - JwtSettings.cs   // DTO for JWT configuration binding
Program.cs             // Main setup for services and middleware
```

## License

This project is licensed under the [MIT License](LICENSE).