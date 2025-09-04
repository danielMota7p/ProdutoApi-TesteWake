﻿namespace ProductDomain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // BCrypt
        public string Role { get; set; } = "user";
    }
}
