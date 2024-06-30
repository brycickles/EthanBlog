using EthanBlog.BusinessManagers.Interfaces;
using EthanBlog.Data.Models;
using EthanBlog.Models.AdminViewModels;
using EthanBlog.Service.Interfaces;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace EthanBlog.BusinessManagers
{
    public class AdminBusinessManager : IAdminBusinessManager
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private IBlogService blogService;
        public AdminBusinessManager(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            IBlogService blogService)
        {
            this.userManager = userManager;
            this.blogService = blogService;
        }

        //method to get back our blogs
        public async Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
            return new IndexViewModel {
                Blogs = blogService.GetBlogs(applicationUser)
            };

        }
    }
}
