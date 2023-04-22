namespace DatabaseManagement.Models
{
    public class UserWithCredentials : User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserWithCredentials(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            Surname = user.Surname;
        }
        public User ToUser()
        {
            return new User
            {
                Id = Id,
                Email = Email,
                Name = Name,
                Surname = Surname
            };
        }
        public AuthenticationCredential ToAuthenticationCredential()
        {
            return new AuthenticationCredential
            {
                UserId = Id,
                Username = Username,
                Password = Password
            };
        }
    }
}
