using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ACRLoginPortal.Models
{
    public class LoginModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression("^([\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4})?$", ErrorMessage = "Enter a valid email.")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string Key { get; set; }
        public bool IsOktaSessionExists { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Error while changing password")]
        public string UserId { get; set; }

        [Display(Name = "Old Password")]
        [Required(ErrorMessage = "Old Password is required.")]
        
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New Password is required.")]
        
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = "Confirm new Password.")]
        
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        public string Key { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression("^([\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4})?$", ErrorMessage = "Enter a valid email.")]
        public string Email { get; set; }

        public string Key { get; set; }
        public string Username { get; set; }
    }

    public class ResetPasswordModel
    {
        [Display(Name = "UserId")]
        [Required(ErrorMessage = "Error while reset password.")]
        public string UserId { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required.")]
        public string ConfirmPassword { get; set; }

        public string Key { get; set; }
    }
}
