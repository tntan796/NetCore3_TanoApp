using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("AdvertisementPositions")]
    public class AdvertisementPosition: DomainEntity<string>
    {
        public AdvertisementPosition()
        {
            Advertisements = new List<Advertisement>();
        }
        public string PageId { set; get; }
        [StringLength(255)]
        public string Name { set; get; }
        [ForeignKey("PageId")]
        public virtual AdvertisementPage AdvertisementPages { set; get; }
        public virtual ICollection<Advertisement> Advertisements { set; get; }
    }
}
