using System;
using System.ComponentModel.DataAnnotations;

namespace WebPortal.Models.ViewModel
{
    public class ResetPasswordVM
    {
        [Required]
        public string Email { get; set; }
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
