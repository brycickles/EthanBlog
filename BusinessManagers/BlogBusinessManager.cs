using EthanBlog.BusinessManagers.Interfaces;
using EthanBlog.Data.Models;
using EthanBlog.Models.BlogViewModels;
using EthanBlog.Service.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EthanBlog.BusinessManagers
{
    public class BlogBusinessManager : IBlogBusinessManager {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private readonly IBlogService blogService;
        public BlogBusinessManager(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IBlogService blogService)
        {
            this.userManager = userManager;
            this.blogService = blogService;
        }
        public async Task<Blog> CreateBlog(CreateBlogViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser.FirstName = "Ethan"; 
            appUser.LastName = "LastName";
            Blog blog = createBlogViewModel.Blog;

            blog.CreatedOn = DateTime.Now;
            blog.Creator = appUser;
            //blog.Creator = await userManager.getUserAsync(claimsPrincipal);

            return await blogService.Add(blog);
        }
    }
}
