using EthanBlog.BusinessManagers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EthanBlog.Controllers
{
    [Authorize] //restricts action in the controller to people who are logged in
    public class AdminController : Controller
    {
        private readonly IAdminBusinessManager adminBusinessManager;
        public AdminController(IAdminBusinessManager adminBusinessManager)
        {
            this.adminBusinessManager = adminBusinessManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await adminBusinessManager.GetAdminDashboard(User));
        }
    }
}
