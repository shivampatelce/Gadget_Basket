using Gadget_Basket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gadget_Basket.Views.Home;

public class AuthenticationController : Controller
{
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;

    public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public IActionResult Login(string returnURL = "")
    {
        var model = new LoginViewModel { ReturnUrl = returnURL };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid) {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: false);
            
            if (result.Succeeded) {
                if (!string.IsNullOrEmpty(model.ReturnUrl) &&
                    Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
        }
        ModelState.AddModelError("", "Invalid username/password.");
        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = new User { UserName = model.Email };
        var result = await _userManager.CreateAsync(user,
            model.Password);
        if (result.Succeeded) {
            await _signInManager.SignInAsync(user,
                isPersistent: false);
        } else {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        
        return RedirectToAction("Login");
    }
    
    [HttpPost]
    public async Task<IActionResult> LogOut() {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}