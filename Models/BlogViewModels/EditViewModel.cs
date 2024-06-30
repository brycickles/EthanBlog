using EthanBlog.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EthanBlog.Models.BlogViewModels
{
    public class EditViewModel
    {
        [Display(Name = "Header Image")]
        public IFormFile BlogHeaderImage { get; set; }
        public Blog Blog { get; set; }
    }
}
