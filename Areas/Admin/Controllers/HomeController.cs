using Gadget_Basket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gadget_Basket.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class HomeController : Controller
{
    private readonly GadgetBasketDbContext _context;

    public HomeController(GadgetBasketDbContext context)
    {
        _context = context;
    }

    public IActionResult ManageCategories()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult ManageCategories(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("ManageCategories");
        }
        ViewBag.Categories = _context.Categories.ToList();
        return View(category);
    }

    [HttpPost]
    public IActionResult DeleteCategory(int categoryId)
    {
        var category = _context.Categories.Find(categoryId);
        if (category == null)
            return NotFound();

        _context.Categories.Remove(category);
        _context.SaveChanges();
        return RedirectToAction("ManageCategories");
    }

    public IActionResult ManageProducts()
    {
        ViewBag.Products = _context.Products.Include(p => p.Category).ToList();
        ViewBag.CategoryList = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult ManageProducts(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("ManageProducts");
        }

        ViewBag.Products = _context.Products.Include(p => p.Category).ToList();
        ViewBag.CategoryList = _context.Categories.ToList();
        return View(product);
    }

    [HttpPost]
    public IActionResult DeleteProduct(long productId)
    {
        var product = _context.Products.Find(productId);
        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        _context.SaveChanges();
        return RedirectToAction("ManageProducts");
    }
}
