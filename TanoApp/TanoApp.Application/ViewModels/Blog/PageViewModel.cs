using System.ComponentModel.DataAnnotations;
using TanoApp.Data.Enums;

namespace TanoApp.Application.ViewModels.Blog
{
    public class PageViewModel
    {
        public int Id { set; get; }
        [Required]
        [MaxLength(255)]
        public string Name { set; get; }
        [MaxLength(255)]
        [Required]
        public string Alias { set; get; }
        public string Content { set; get; }
        public Status Status { set; get; }
    }
}
