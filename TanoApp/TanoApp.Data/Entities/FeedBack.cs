using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TanoApp.Data.Enums;
using TanoApp.Data.Interfaces;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("FeedBacks")]
    public class FeedBack : DomainEntity<int>, ISwitchable, IDateTracking
    {
        public FeedBack() { }
        public FeedBack(int id, string name, string email, string message, Status status)
        {
            Id = id;
            Name = name;
            Email = email;
            Message = message;
            Status = status;
        }
        [Required]
        [StringLength(255)]
        public string Name { set; get; }
        [StringLength(255)]
        public string Email { set; get; }
        [StringLength(255)]
        public string Message { set; get; }
        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
    }
}
