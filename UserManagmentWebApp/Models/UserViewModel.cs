﻿namespace UserManagmentWebApp.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AuthenticationCredential AuthenticationCredential { get; set; }
    }
}
