using AspAPIMultiAuth.dto; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspAPIMultiAuth.Controllers {
    [ApiController]
    [Route("[controller]")]

    [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme + "," +
    NegotiateDefaults.AuthenticationScheme)]

    public class WeatherController : ControllerBase {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        [Authorize]  
        public IActionResult Get()
        {
            var username = User.Identity?.Name ?? "Unknown User";
            var user = HttpContext.User;

            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray();
           
            return Ok(new
            {
                IsAuthenticated = user.Identity?.IsAuthenticated,
                Message = $"Welcome {username}",
                AuthenticationType = user.Identity?.AuthenticationType,
                Forecast = forecast,
            });
        }
    }
}
