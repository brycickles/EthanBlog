using EthanBlog.Data.Models;
using EthanBlog.Models.AdminViewModels;
using EthanBlog.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EthanBlog.BusinessManagers.Interfaces
{
    public interface IBlogBusinessManager
    {
        Models.HomeViewModels.IndexViewModel GetIndexViewModel(string searchString, int? page);
        Task<Blog> CreateBlog(CreateViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> UpdateBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);
    }
}
