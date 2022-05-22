using System;

namespace ObservabilityDemo.Api.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public User(string name, string password, string email)
        {
            Id = Guid.NewGuid().ToString("N");
            Name = name;
            Password = password;
            Email = email;
        }
    }
}