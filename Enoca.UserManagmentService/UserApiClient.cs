using Enoca.UserManagmentService.Models;

namespace Enoca.UserManagmentService
{
    public class UserApiClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<User>>("api/User");
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<User>($"api/User/{id}");
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User", user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task UpdateUserAsync(User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/User/{user.Id}", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/User/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
