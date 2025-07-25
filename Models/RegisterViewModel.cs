using System.ComponentModel.DataAnnotations;

namespace Gadget_Basket.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Please enter your first name.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    [Display(Name = "First Name")]
    public string Firstname { get; set; }
    
    [Required(ErrorMessage = "Please enter your last name.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    [Display(Name = "Last Name")]
    public string Lastname { get; set; }
    
    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Email Address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Please enter a password.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Please confirm your password.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}