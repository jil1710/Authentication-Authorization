﻿namespace AuthenticationAndAuthorization.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool IsDisable {get;set;}
        public string Password { get; set; }
    }
}
