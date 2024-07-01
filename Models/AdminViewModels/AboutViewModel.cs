using EthanBlog.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace EthanBlog.Models.AdminViewModels
{
    public class AboutViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Header Image")]
        public IFormFile HeaderImage { get; set; }
        [Display(Name ="Sub-header")]
        public string SubHeader { get; set; }
        public string Content { get; set; }
    }
}
