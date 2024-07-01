using EthanBlog.BusinessManagers.Interfaces;
using EthanBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EthanBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogBusinessManager blogBusinessManager;
        public HomeController(IBlogBusinessManager blogBusinessManager)
        {
            this.blogBusinessManager = blogBusinessManager;
        }

        public IActionResult Index(string searchString, int? page)
        {
            return View(blogBusinessManager.GetIndexViewModel(searchString, page));
        }


    }
}