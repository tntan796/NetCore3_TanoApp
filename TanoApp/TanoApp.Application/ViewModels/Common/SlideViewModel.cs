using System.ComponentModel.DataAnnotations;

namespace TanoApp.Application.ViewModels.Common
{
    public class SlideViewModel
    {
        public int Id { get; set; }
        [StringLength(255)]
        [Required]
        public string Name { set; get; }
        [StringLength(255)]
        public string Description { set; get; }
        [StringLength(255)]
        [Required]
        public string Image { set; get; }
        [StringLength(255)]
        public string Url { set; get; }
        public int? DisplayOrder { set; get; }
        public bool Status { set; get; }
        public string Content { set; get; }
        [StringLength(25)]
        [Required]
        public string GroupAlias { get; set; }
    }
}
