using Asp.Versioning;
using AspAPIMultiAuth.dto; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspAPIMultiAuth.Controllers {
    [ApiController]
    [Route("[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]

    public class WeatherController : ControllerBase {
        // For API Version 1
        [HttpGet]
        [MapToApiVersion(1.0)] // only for v1
        public IActionResult GetV1()
        {
            var username = User.Identity?.Name ?? "Unknown User";
            var user = HttpContext.User;

            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = "Sunny"
                }).ToArray();

            return Ok(new
            {
                Version = "v1",
                IsAuthenticated = user.Identity?.IsAuthenticated,
                Message = $"Welcome {username}",
                AuthenticationType = user.Identity?.AuthenticationType,
                Forecast = forecast,
            });
        }

        // For API Version 2
        [HttpGet]
        [MapToApiVersion(2.0)] // only for v2
        public IActionResult GetV2()
        {
            var username = User.Identity?.Name ?? "Unknown User";
            var user = HttpContext.User;

            var forecast = Enumerable.Range(1, 7).Select(index =>
                new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-30, 60),
                    Summary = "Extreme Weather"
                }).ToArray();

            return Ok(new
            {
                Version = "v2",
                IsAuthenticated = user.Identity?.IsAuthenticated,
                Message = $"Hello {username}, welcome to the upgraded API v2!",
                AuthenticationType = user.Identity?.AuthenticationType,
                Forecast = forecast,
            });
        }
    }
}
