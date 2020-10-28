using System.ComponentModel.DataAnnotations;
using TanoApp.Data.Enums;

namespace TanoApp.Application.ViewModels.Common
{
    public class ContactViewModel
    {
        public string Id { set; get; }
        [StringLength(255)]
        [Required]
        public string Name { set; get; }
        [StringLength(50)]
        public string Phone { set; get; }
        [StringLength(255)]
        public string Email { set; get; }
        [StringLength(255)]
        public string Website { set; get; }
        [StringLength(255)]
        public string Address { set; get; }
        public string Other { set; get; }
        public double? Lat { set; get; }
        public double? Long { set; get; }
        public Status Status { set; get; }
    }
}
