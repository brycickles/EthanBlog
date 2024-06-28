using EthanBlog.BusinessManagers.Interfaces;
using EthanBlog.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace EthanBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogBusinessManager blogBusinessManager;
        public BlogController(IBlogBusinessManager blogBusinessManager) { 
            this.blogBusinessManager = blogBusinessManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new CreateBlogViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateBlogViewModel createBlogViewModel) {
            await blogBusinessManager.CreateBlog(createBlogViewModel, User);
            return RedirectToAction("Create");
        }
    }
}
