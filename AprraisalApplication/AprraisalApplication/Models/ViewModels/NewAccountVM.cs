using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Models.ViewModels
{
    public class NewAccountVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"\w+([-+.]\w+)*@ieianchorpensions\.com", ErrorMessage = "You must use your IEI Email account")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}