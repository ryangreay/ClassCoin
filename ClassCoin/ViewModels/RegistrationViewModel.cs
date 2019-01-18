using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassCoin.Models;
using System.ComponentModel.DataAnnotations;

namespace ClassCoin.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class InstructorRegistrationViewModel
    {
        [Required(ErrorMessage = "Required.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Lastname { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Required.")]
        [System.Web.Mvc.Remote("DoesEmailExist", "Home", HttpMethod = "POST", ErrorMessage = "Email already associated with an account.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Institution { get; set; }

        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class StudentRegistrationViewModel
    {
        [Required(ErrorMessage = "Required.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Required. You will use this to login.")]
        [System.Web.Mvc.Remote("DoesUserNameExist", "Home", HttpMethod = "POST", ErrorMessage = "Username already exists.")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [System.Web.Mvc.Remote("DoesEmailExist", "Home", HttpMethod = "POST", ErrorMessage = "Email already associated with an account.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }

    public class SignupViewModel
    {
        public InstructorRegistrationViewModel Instructor { get; set; }
        public StudentRegistrationViewModel Student { get; set; }
    }

    //for students, contact your instructor for your password
    //teachers enter your email and we will send you a link
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}