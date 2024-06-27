using Microsoft.AspNetCore.Mvc;

namespace EthanBlog.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
