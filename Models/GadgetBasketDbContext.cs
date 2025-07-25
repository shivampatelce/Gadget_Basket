using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gadget_Basket.Models;

public class GadgetBasketDbContext: IdentityDbContext<User>
{
    public GadgetBasketDbContext(DbContextOptions<GadgetBasketDbContext> options) : base(options) {}
}