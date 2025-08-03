using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gadget_Basket.Models;

public class GadgetBasketDbContext: IdentityDbContext<User>
{
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<CartItem> CartItems { get; set; }
    
    public GadgetBasketDbContext(DbContextOptions<GadgetBasketDbContext> options) : base(options) {}
}