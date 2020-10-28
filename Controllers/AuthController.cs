using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthTest.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            // get access token
            var serverClient = new HttpClient();
            serverClient.DefaultRequestHeaders.Accept.Add
                (new MediaTypeWithQualityHeaderValue("application/text"));

            Dictionary<string, string> param =
                new Dictionary<string, string>();
            param.Add("client_id", "client1");
            param.Add("client_secret", "client1_secret_code");
            param.Add("grant_type", "password");
            param.Add("username", "user1");
            param.Add("password", "password1");
            param.Add("scope", "employeesWebApi roles");

            var content = new FormUrlEncodedContent(param);
            var serverResponse = serverClient.PostAsync
                ("https://localhost:6001/connect/token", content).Result;
            string jsonData = serverResponse.Content.ReadAsStringAsync().Result;

            var accessToken = JsonConvert.DeserializeObject<Token>(jsonData).access_token;

            // call web api
            return Ok(accessToken);
        }
    }
}
