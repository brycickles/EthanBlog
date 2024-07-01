using EthanBlog.Data.Models;
using EthanBlog.Models.AdminViewModels;
using EthanBlog.Models.PostViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EthanBlog.BusinessManagers.Interfaces
{
    public interface IPostBusinessManager
    {
        Models.HomeViewModels.IndexViewModel GetIndexViewModel(string searchString, int? page);
        Task<Post> CreatePost(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> UpdatePost(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);
    }
}
