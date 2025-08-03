using Gadget_Basket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gadget_Basket.Controllers;

public class ShopController : Controller
{
    private readonly GadgetBasketDbContext _context;
    private readonly UserManager<User> _userManager;

    public ShopController(GadgetBasketDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var categoryProducts = _context.Categories
            .Include(c => c.Products)
            .Select(c => new CategoryProductsViewModel
            {
                Category = c,
                Products = c.Products.Take(4).ToList()
            })
            .ToList();

        return View(categoryProducts);
    }

    public IActionResult ProductList(string? categoryName, string? query, string? sort)
    {
        IQueryable<Product> productsQuery = _context.Products.Include(p => p.Category);

        ViewBag.Categories = _context.Categories.ToList(); // Send category list to view

        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            productsQuery = productsQuery
                .Where(p => p.Category.CategoryName.ToLower() == categoryName.ToLower());
            ViewBag.CategoryName = categoryName;
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            productsQuery = productsQuery
                .Where(p => p.ProductName.Contains(query) || p.ShortDescription.Contains(query) || p.LongDescription.Contains(query));
            ViewBag.SearchQuery = query;
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            ViewBag.Sort = sort;

            switch (sort)
            {
                case "price_asc":
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
                case "discount":
                    productsQuery = productsQuery.Where(p => p.Discount > 0);
                    break;
            }
        }

        var products = productsQuery.ToList();
        return View(products);
    }

    public async Task<IActionResult> ProductDetail(long id)
    {
        var product = _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.ProductID == id);

        var user = await _userManager.GetUserAsync(User);
        int cartQuantity = 0;

        if (user != null)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(c => c.ProductID == id && c.UserId == user.Id);

            if (cartItem != null)
                cartQuantity = cartItem.Quantity;
        }

        ViewBag.CartQuantity = cartQuantity;
        
        return View(product);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Authentication");

        var existingItems = _context.CartItems
            .Where(c => c.UserId == user.Id)
            .ToList();

        if (existingItems.Count >= 5)
        {
            TempData["CartError"] = "You can only add up to 5 items.";
            return RedirectToAction("ProductDetail", new { id = productId });
        }

        var existingCartItem = existingItems.FirstOrDefault(ci => ci.ProductID == productId);

        if (existingCartItem != null)
        {
            if (existingCartItem.Quantity < 5)
                existingCartItem.Quantity++;
        }
        else
        {
            var cartItem = new CartItem
            {
                UserId = user.Id,
                ProductID = productId,
                Quantity = 1
            };
            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("ProductDetail",new { id = productId });
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateCart(int productId, string actionType)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Authentication");

        var cartItem = _context.CartItems
            .FirstOrDefault(c => c.ProductID == productId && c.UserId == user.Id);

        if (cartItem != null)
        {
            if (actionType == "increment" && cartItem.Quantity < 5)
            {
                cartItem.Quantity++;
            }
            else if (actionType == "decrement")
            {
                cartItem.Quantity--;
                if (cartItem.Quantity <= 0)
                    _context.CartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("ProductDetail", new { id = productId });
    }

    public async Task<IActionResult> Cart()
    {
        var user = await _userManager.GetUserAsync(User);
        var items = _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == user.Id)
            .ToList();

        return View(items);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(int cartItemId, string actionType)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Authentication");

        var item = _context.CartItems.FirstOrDefault(c => c.CartItemId == cartItemId && c.UserId == user.Id);
        if (item == null) return NotFound();

        if (actionType == "increment" && item.Quantity < 5)
        {
            item.Quantity++;
        }
        else if (actionType == "decrement")
        {
            item.Quantity--;
            if (item.Quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Authentication");

        var item = _context.CartItems.FirstOrDefault(c => c.CartItemId == cartItemId && c.UserId == user.Id);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Cart");
    }

}