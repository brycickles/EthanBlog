using EthanBlog.Data.Models;
using EthanBlog.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EthanBlog.BusinessManagers.Interfaces
{
    public interface IBlogBusinessManager
    {
        Task<Blog> CreateBlog(CreateViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> UpdateBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);
    }
}
