using EthanBlog.Authorization;
using EthanBlog.BusinessManagers.Interfaces;
using EthanBlog.Data.Models;
using EthanBlog.Models.AdminViewModels;
using EthanBlog.Models.BlogViewModels;
using EthanBlog.Models.HomeViewModels;
using EthanBlog.Service.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PagedList.Core;
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
        private readonly IAuthorizationService authorizationService;
        public BlogBusinessManager(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            IBlogService blogService,
            IWebHostEnvironment webHostEnvironment,
            IAuthorizationService authorizationService)
        {
            this.userManager = userManager;
            this.blogService = blogService;
            this.webHostEnvironment = webHostEnvironment;
            this.authorizationService = authorizationService;
        }

        public Models.HomeViewModels.IndexViewModel GetIndexViewModel(string searchString, int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var blogs = blogService.GetBlogs(searchString ?? string.Empty)
                .Where(blog => blog.Published);

            return new Models.HomeViewModels.IndexViewModel
            { 
                Blogs = new StaticPagedList<Blog>(blogs.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, blogs.Count()),
                SearchString = searchString, 
                PageNumber = pageNumber
            };

        }
        public async Task<Blog> CreateBlog(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
        {
            Blog blog = createViewModel.Blog;

            blog.CreatedOn = DateTime.Now;
            blog.Creator = await userManager.GetUserAsync(claimsPrincipal);
            blog.UpdatedOn = DateTime.Now;
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

        public async Task<ActionResult<EditViewModel>> UpdateBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var blog = blogService.GetBlog(editViewModel.Blog.Id);

            if (blog == null)
            {
                return new NotFoundResult();
            }
            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);

            if (!authorizationResult.Succeeded)
            {
                return DetermineActionResult(claimsPrincipal);
            }

            blog.Published = editViewModel.Blog.Published;
            blog.Title = editViewModel.Blog.Title;
            blog.Content = editViewModel.Blog.Content;
            blog.UpdatedOn = DateTime.Now;

            //TODO: figure out why edit image only works if no image previously exists on post.
            if(editViewModel.BlogHeaderImage != null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

                EnsureFolder(pathToImage);

                using (var fileStram = new FileStream(pathToImage, FileMode.Create))
                {
                    await editViewModel.BlogHeaderImage.CopyToAsync(fileStram);
                }
            }

            return new EditViewModel
            {
                Blog = await blogService.Update(blog)
            };
        }
        public async Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
            {
                return new BadRequestResult();
            }
            var blogId = id.Value;
            var blog = blogService.GetBlog(blogId);
            if (blog == null)
            {
                return new NotFoundResult();
            }
            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);

            if (!authorizationResult.Succeeded)
            {
                return DetermineActionResult(claimsPrincipal);
            }

            return new EditViewModel
            {
                Blog = blog
            };
        }

        private ActionResult DetermineActionResult(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }else
            {
                return new ChallengeResult();
            }
            
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
