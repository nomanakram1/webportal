using System;
using Microsoft.AspNetCore.Identity;

namespace WebPortal.Models
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
