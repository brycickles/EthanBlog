using EthanBlog.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace EthanBlog.Models.BlogViewModels
{
    public class CreateViewModel
    {
        [Required, Display(Name = "Header Image")]
        public IFormFile BlogHeaderImage { get; set; }
        public Blog Blog { get; set; }
    }
}
