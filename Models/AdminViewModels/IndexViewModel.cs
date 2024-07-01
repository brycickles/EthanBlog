using EthanBlog.Data.Models;

namespace EthanBlog.Models.AdminViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}
