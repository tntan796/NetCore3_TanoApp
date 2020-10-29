using System.ComponentModel.DataAnnotations;
using TanoApp.Data.Enums;

namespace TanoApp.Application.ViewModels.System
{
    public class FunctionViewModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { set; get; }
        [Required]
        [StringLength(255)]
        public string URL { set; get; }
        [StringLength(128)]
        public string ParentId { set; get; }
        public string IconCss { get; set; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
    }
}
