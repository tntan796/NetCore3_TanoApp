using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("AdvertisementPages")]
    public class AdvertisementPage: DomainEntity<string>
    {
        public AdvertisementPage()
        {
            AdvertisementPositions = new List<AdvertisementPosition>();
        }
        public string Name { set; get; }
        public virtual ICollection<AdvertisementPosition> AdvertisementPositions { set; get; }
    }
}
