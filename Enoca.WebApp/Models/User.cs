using System.Text.Json.Serialization;

namespace Enoca.WebApp.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
