using EthanBlog.Data.Models;
using EthanBlog.Models.BlogViewModels;
using System.Security.Claims;

namespace EthanBlog.BusinessManagers.Interfaces
{
    public interface IBlogBusinessManager
    {
       Task<Blog> CreateBlog(CreateViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);
    }
}
