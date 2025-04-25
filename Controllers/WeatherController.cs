using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspAPIMultiAuth.Controllers {
    [ApiController]
    [Route("[controller]")]
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

    public class WeatherForecast {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
