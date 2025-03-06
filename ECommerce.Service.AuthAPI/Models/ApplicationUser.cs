﻿using Microsoft.AspNetCore.Identity;

namespace ECommerce.Service.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
