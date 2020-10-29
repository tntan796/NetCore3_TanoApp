using System;
using System.ComponentModel.DataAnnotations;
using TanoApp.Data.Enums;

namespace TanoApp.Application.ViewModels.Common
{
    public class FeedBackViewModel
    {
        public int Id { set; get; }
        [StringLength(255)]
        [Required]
        public string Name { set; get; }
        [StringLength(255)]
        public string Email { set; get; }
        [StringLength(500)]
        public string Message { set; get; }
        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
    }
}
