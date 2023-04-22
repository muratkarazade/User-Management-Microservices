using AuthenticationService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string _databaseServiceUrl = "http://localhost:7186";

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginModel loginModel)
        {
            User user = await AuthenticateUser(loginModel.Username, loginModel.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        private async Task<User> AuthenticateUser(string username, string password)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(_databaseServiceUrl);
                var response = await client.GetAsync($"api/user/Authenticate?username={username}&password={password}");//("Authenticate")

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(result);
                    return user;
                }

                return null;
            }
        }




    }
}
