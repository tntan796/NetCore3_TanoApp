using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("Colors")]
    public class Color: DomainEntity<int>
    {
        [StringLength(255)]
        public string Name { set; get; }
        [StringLength(255)]
        public string Code { set; get; }
    }
}
