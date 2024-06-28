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
    public class BlogBusinessManager : IBlogBusinessManager
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private readonly IBlogService blogService;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BlogBusinessManager(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            IBlogService blogService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.blogService = blogService;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<Blog> CreateBlog(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
        {
            Blog blog = createViewModel.Blog;

            blog.CreatedOn = DateTime.Now;
            blog.Creator = await userManager.GetUserAsync(claimsPrincipal);

            blog = await blogService.Add(blog);

            //build image path 
            string webRootPath = webHostEnvironment.WebRootPath; //points to wwwroot
            string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

            EnsureFolder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await createViewModel.BlogHeaderImage.CopyToAsync(fileStream);
            }
            return blog;
        }

        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }
    }
}
