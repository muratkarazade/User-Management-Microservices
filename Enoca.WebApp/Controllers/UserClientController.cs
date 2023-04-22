using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using Enoca.WebApp.Models;

namespace Enoca.WebApp.Controllers
{
    public class UserClientController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UserClientController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();

        }



        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var authUrl = _configuration["GatewayUrls:Authentication"];
                var loginData = new { Username = model.Username, Password = model.Password };
                var jsonContent = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{authUrl}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToAction("Login", "UserClient", new { id = user.Id });
                }
            }

            ModelState.AddModelError("LoginFailed", "Kullanıcı adı veya şifre geçersiz.");
            return View();
        }


        //[AuthorizeSession]
        public async Task<IActionResult> Index()
        {
            var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
            var users = await _httpClient.GetFromJsonAsync<List<User>>($"{userManagmentUrl}");

            return View(users);
        }


        public async Task<IActionResult> Details(int? id)
        {
            // Oturumda UserId bilgisini kontrol edin
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // Oturumda UserId bilgisi yoksa, kullanıcıyı giriş sayfasına yönlendirin
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
            var user = await _httpClient.GetFromJsonAsync<User>($"{userManagmentUrl}/{id}");

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        // GET: UserManagment/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Email,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
                await _httpClient.PostAsJsonAsync($"{userManagmentUrl}", user);

                return RedirectToAction("Login");
            }
            return View(user);
        }

        
        //[AuthorizeSession]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
            var user = await _httpClient.GetFromJsonAsync<User>($"{userManagmentUrl}/{id}");

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [ValidateAntiForgeryToken]
        [Route("{id}")]
        [HttpPost]        
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Email,Username,Password")] User user)
        {
            try
            {
                if (id != user.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
                    await _httpClient.PutAsJsonAsync($"{userManagmentUrl}/{id}", user);
                    return RedirectToAction("Details", "UserClient", new { id = user.Id });
                }
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                return View(user);
            }
        }

        // GET: UserManagment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userManagmentUrl = _configuration["GatewayUrls:UserManagment:UserManagment"];
            var user = await _httpClient.GetFromJsonAsync<User>($"{userManagmentUrl}/api/UserManagment/{id}");
            if (user == null)
            {
                return NotFound();
            }
            var deletedUser = new User
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,

            };
            return View(deletedUser);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
            await _httpClient.DeleteAsync($"{userManagmentUrl}/api/UserManagment/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
