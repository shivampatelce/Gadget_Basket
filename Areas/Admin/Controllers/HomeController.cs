using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gadget_Basket.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}