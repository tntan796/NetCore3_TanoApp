using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("AnnouncementUsers")]
    public class AnnouncementUser: DomainEntity<int>
    {
        public AnnouncementUser()
        {}
        public AnnouncementUser(string announcementId, Guid userId, bool? hasRead)
        {
            AnnouncementId = announcementId;
            UserId = userId;
            HasRead = hasRead;
        }
        [Required]
        public string AnnouncementId { set; get; }
        public Guid UserId { set; get; }
        public bool? HasRead { set; get; }
        [ForeignKey("AnnouncementId")]
        public virtual Announcement Announcement { set; get; }
    }
}
