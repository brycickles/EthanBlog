using EthanBlog.Models.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EthanBlog.BusinessManagers.Interfaces
{
    public interface IHomeBusinessManager
    {
        ActionResult<AuthorViewModel> GetAuthorViewModel(string authorId, string searchString, int? page);
    }
}
