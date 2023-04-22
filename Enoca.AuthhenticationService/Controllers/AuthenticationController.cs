using Enoca.AuthhenticationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Enoca.AuthhenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string _databaseServiceUrl = "http://localhost:7187";

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseModel>> Login(LoginModel loginModel)
        {
            User user = await AuthenticateUser(loginModel.Username, loginModel.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            User userResponse = new User
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Password = user.Password
            };

            return Ok(userResponse);
        }


        private async Task<User> AuthenticateUser(string username, string password)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(_databaseServiceUrl);
                var response = await client.GetAsync($"api/user/Authenticate?username={username}&password={password}");

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
