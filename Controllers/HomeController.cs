using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Gadget_Basket.Models;

namespace Gadget_Basket.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly GadgetBasketDbContext _context;

    public HomeController(GadgetBasketDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.Categories = _context.Categories.ToList();
        
        var featuredProducts = _context.Products
            .Where(p => p.Discount != null && p.Discount < p.Price)
            .OrderByDescending(p => p.Price - p.Discount)
            .Take(4)
            .ToList();

        ViewBag.FeaturedProducts = featuredProducts;
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}