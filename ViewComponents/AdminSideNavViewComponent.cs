using Microsoft.AspNetCore.Mvc;

namespace EthanBlog.ViewComponents
{
    public class AdminSideNavViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Factory.StartNew(() => { return View(); });
        }

    }   
}
