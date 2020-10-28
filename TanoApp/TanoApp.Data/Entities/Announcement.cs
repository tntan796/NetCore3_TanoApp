using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using TanoApp.Data.Enums;
using TanoApp.Data.Interfaces;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("Announcements")]
    public class Announcement : DomainEntity<string>, ISwitchable, IDateTracking
    {
        public Announcement()
        {
            AnnouncementUsers = new List<AnnouncementUser>();
        }
        public Announcement(string title, string content, Guid userId, Status status)
        {
            Title = title;
            Content = content;
            UserId = userId;
            Status = status;
        }
        [Required]
        [StringLength(250)]
        public string Title { set; get; }
        [StringLength(250)]
        public string Content { set; get; }
        [ForeignKey("UserId")]
        public Guid UserId { set; get; }
        public virtual AppUser AppUser { set; get; }
        public virtual ICollection<AnnouncementUser> AnnouncementUsers { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
    }
}
