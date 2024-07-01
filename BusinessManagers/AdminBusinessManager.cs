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
        private IPostService postService;
        public AdminBusinessManager(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            IPostService postService)
        {
            this.userManager = userManager;
            this.postService = postService;
        }

        //method to get back our blogs
        public async Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
            return new IndexViewModel {
                Posts = postService.GetPosts(applicationUser)
            };

        }
    }
}
