using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TanoApp.Data.Enums;
using TanoApp.Data.Interfaces;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("Advertisements")]
    public class Advertisement : DomainEntity<int>, ISwitchable, ISortable, IDateTracking
    {
        [StringLength(255)]
        public string Name { set; get; }
        [StringLength(255)]
        public string DescriptionAttribute { set; get; }
        [StringLength(255)]
        public string Image { set; get; }
        public string Url { set; get; }
        [StringLength(20)]
        public string PositionId { set; get; }
        public Status Status { set; get; }
        public int SortOrder { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public virtual AdvertisementPosition AdvertisementPositions { set; get; }
    }
}
