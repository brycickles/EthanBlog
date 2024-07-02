using EthanBlog.Authorization;
using EthanBlog.BusinessManagers.Interfaces;
using EthanBlog.Data.Models;
using EthanBlog.Models.AdminViewModels;
using EthanBlog.Models.PostViewModels;
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
    public class PostBusinessManager : IPostBusinessManager
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAuthorizationService authorizationService;
        public PostBusinessManager(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            IPostService postService,
            IWebHostEnvironment webHostEnvironment,
            IAuthorizationService authorizationService)
        {
            this.userManager = userManager;
            this.postService = postService;
            this.webHostEnvironment = webHostEnvironment;
            this.authorizationService = authorizationService;
        }

        public Models.HomeViewModels.IndexViewModel GetIndexViewModel(string searchString, int? page)
        {
            int pageSize = 20;
            int pageNumber = page ?? 1;
            var posts = postService.GetPosts(searchString ?? string.Empty)
                .Where(post => post.Published);

            return new Models.HomeViewModels.IndexViewModel
            {
                Posts = new StaticPagedList<Post>(posts.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, posts.Count()),
                SearchString = searchString, 
                PageNumber = pageNumber
            };

        }
        public async Task<ActionResult<PostViewModel>> GetPostViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
            {
                return new BadRequestResult();
            }
            var postId = id.Value;
            var post = postService.GetPost(postId);
            if (post == null)
            {
                return new NotFoundResult();
            }

            //check if post has been published. If not published, we want to check if we are the owner of the post. if so, go ahead and allow them to view the post to edit or publish it
            if (!post.Published)
            {
                var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Read);

                if (!authorizationResult.Succeeded)
                {
                    return DetermineActionResult(claimsPrincipal);
                }
            }

            return new PostViewModel
            {
                Post = post
            };
        }
        public async Task<Post> CreatePost(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
        {
            Post post = createViewModel.Post;

            post.CreatedOn = DateTime.Now;
            post.Creator = await userManager.GetUserAsync(claimsPrincipal);
            post.UpdatedOn = DateTime.Now;
            post = await postService.Add(post);

            //build image path 
            string webRootPath = webHostEnvironment.WebRootPath; //points to wwwroot
            string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";

            EnsureFolder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await createViewModel.HeaderImage.CopyToAsync(fileStream);
            }
            return post;
        }

        public async Task<ActionResult<Comment>> CreateComment(PostViewModel postViewModel, ClaimsPrincipal claimsPrincipal) {
            if (postViewModel.Post is null || postViewModel.Post.Id == 0) {
                return new BadRequestResult();
            }
            var post = postService.GetPost(postViewModel.Post.Id);

            if(post is null)
            {
                return new NotFoundResult();
            }

            var comment = postViewModel.Comment;

            comment.Author = await userManager.GetUserAsync(claimsPrincipal);
            comment.Post = post;
            comment.Creation = DateTime.Now;

            if(comment.Parent != null)
            {
                comment.Parent = postService.GetComment(comment.Parent.Id);
            }

            return await postService.Add(comment);
        }
        public async Task<ActionResult<EditViewModel>> UpdatePost(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var post = postService.GetPost(editViewModel.Post.Id);

            if (post == null)
            {
                return new NotFoundResult();
            }
            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

            if (!authorizationResult.Succeeded)
            {
                return DetermineActionResult(claimsPrincipal);
            }

            post.Published = editViewModel.Post.Published;
            post.Title = editViewModel.Post.Title;
            post.Content = editViewModel.Post.Content;
            post.UpdatedOn = DateTime.Now;

            if(editViewModel.HeaderImage != null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";

                EnsureFolder(pathToImage);

                using (var fileStram = new FileStream(pathToImage, FileMode.Create))
                {
                    await editViewModel.HeaderImage.CopyToAsync(fileStram);
                }
            }

            return new EditViewModel
            {
                Post = await postService.Update(post)
            };
        }
        public async Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
            {
                return new BadRequestResult();
            }
            var postId = id.Value;
            var post = postService.GetPost(postId);
            if (post == null)
            {
                return new NotFoundResult();
            }
            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

            if (!authorizationResult.Succeeded)
            {
                return DetermineActionResult(claimsPrincipal);
            }

            return new EditViewModel
            {
                Post = post
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
