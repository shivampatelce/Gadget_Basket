using System.ComponentModel.DataAnnotations;

namespace Gadget_Basket.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter a email address.")]
    [StringLength(255)]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Please enter a password.")]
    [StringLength(255)]
    public string Password { get; set; }
    
    public string? ReturnUrl { get; set; }

    public bool RememberMe { get; set; }
}