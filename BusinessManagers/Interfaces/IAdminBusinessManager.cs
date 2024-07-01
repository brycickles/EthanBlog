using EthanBlog.Models.AdminViewModels;
using System.Security.Claims;

namespace EthanBlog.BusinessManagers.Interfaces
{
    public interface IAdminBusinessManager
    {
        Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrincipal);
    }
}
