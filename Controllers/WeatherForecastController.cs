using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AuthTest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            // create a "principal context" - e.g. your domain (could be machine, too)
            using var pc = new PrincipalContext(ContextType.Domain, "helpsystems.com");
            // validate the credentials
            bool isValid = pc.ValidateCredentials("lilit.hakobyan", "");

           

            var uPrincipal = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, "lilit.hakobyan");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //public async Task<IActionResult> CallApi()
        //{
        //    var accessToken = await Microsoft.AspNetCore.Http.HttpContext.GetTokenAsync("access_token");

        //    var client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //    var content = await client.GetStringAsync("https://localhost:5001/identity");

        //    var Json = JArray.Parse(content).ToString();
        //    return Ok(Json);
        //}
    }

  
}
