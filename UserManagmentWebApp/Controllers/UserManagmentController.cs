using UserManagmentWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using UserManagmentWebApp.Filters;

namespace YourProject.Controllers
{
    public class UserManagmentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UserManagmentController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();

        }



        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var authUrl = _configuration["GatewayUrls:Authentication"];
            var loginData = new { Username = username, Password = password };
            var jsonContent = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{authUrl}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<User>();
                // Oturumu başlatın ve ana sayfaya yönlendirin.
                HttpContext.Session.SetString("Username",user.Name );
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Details", "UserManagment", new { id = user.Id });

            }

            ModelState.AddModelError("LoginFailed", "Kullanıcı adı veya şifre geçersiz.");
            return View();
        }


        [AuthorizeSession]
        public async Task<IActionResult> Index()
        {
            var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
            var users = await _httpClient.GetFromJsonAsync<List<UserViewModel>>($"{userManagmentUrl}");

            return View(users);
        }


        [HttpGet]
        [AuthorizeSession]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
            var user = await _httpClient.GetFromJsonAsync<UserViewModel>($"{userManagmentUrl}/{id}");

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

        // POST: UserManagment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Email,Username,Password")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
                await _httpClient.PostAsJsonAsync($"{userManagmentUrl}/api/UserManagment", userViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }

        // GET: UserManagment/Edit/5
        [AuthorizeSession]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
            var userViewModel = await _httpClient.GetFromJsonAsync<UserViewModel>($"{userManagmentUrl}/{id}");

            if (userViewModel == null)
            {
                return NotFound();
            }
            return View(userViewModel);
        }

        // POST: UserManagment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Email,Username,Password")] UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userManagmentUrl = _configuration["GatewayUrls:UserManagment"];
                await _httpClient.PutAsJsonAsync($"{userManagmentUrl}/api/UserManagment/{id}", userViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }

        // GET: UserManagment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userManagmentUrl = _configuration["GatewayUrls:UserManagment:UserManagment"];
            var user = await _httpClient.GetFromJsonAsync<UserViewModel>($"{userManagmentUrl}/api/UserManagment/{id}");
            if (user == null)
            {
                return NotFound();
            }
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.AuthenticationCredential.Username,
                Password = user.AuthenticationCredential.Password,
                AuthenticationCredential = user.AuthenticationCredential
            };
            return View(userViewModel);
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
