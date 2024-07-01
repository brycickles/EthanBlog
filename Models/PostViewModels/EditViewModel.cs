using EthanBlog.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EthanBlog.Models.PostViewModels
{
    public class EditViewModel
    {
        [Display(Name = "Header Image")]
        public IFormFile HeaderImage { get; set; }
        public Post Post { get; set; }
    }
}
