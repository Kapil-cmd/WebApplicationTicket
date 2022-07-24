using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels.Users
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Username is required"), Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required"), Display(Name = "Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class UserRegister
    {
        [Required(ErrorMessage = "FirstName is required"), MaxLength(25), Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required"), MaxLength(25), Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Address is required"), MaxLength(15), Display(Name = "Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email is required"), MaxLength(15), Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Age")]
        public int Age { get; set; }
        [Display(Name = "DateOfBirth")]
        [Range(1, 100)]
        public DateTime DateOfBirth { get; set; }
        public long PhoneNumber { get; set; }
        [Required(ErrorMessage = "UserName is required"), MaxLength(25), Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "FirstName is required"), MaxLength(25), Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required"), MaxLength(25), Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Address is required"), MaxLength(15), Display(Name = "Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email is required"), MaxLength(15), Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Age")]
        public int Age { get; set; }
        [Display(Name = "DateOfBirth")]
        [Range(1, 100)]
        public DateTime DateOfBirth { get; set; }
        public long PhoneNumber { get; set; }
        [Required(ErrorMessage = "UserName is required"), MaxLength(25), Display(Name = "UserName")]
        public string UserName { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long PhoneNumber { get; set; }
        public string Password { get; set; }

    }
}
