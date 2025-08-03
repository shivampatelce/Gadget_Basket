using Gadget_Basket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gadget_Basket.Controllers;

public class CheckoutController : Controller
{
    private readonly GadgetBasketDbContext _context;
    private readonly UserManager<User> _userManager;

    public CheckoutController(GadgetBasketDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Authentication");

        var cartItems = _context.CartItems
            .Where(ci => ci.UserId == user.Id)
            .ToList();

        _context.CartItems.RemoveRange(cartItems);
        _context.SaveChanges();
        
        return RedirectToAction("Confirmation");
    }

    public IActionResult Confirmation()
    {
        return View();
    }
}