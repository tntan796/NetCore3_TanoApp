using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.Entities
{
    [Table("Footers")]
    public class Footer: DomainEntity<string>
    {
        [Required]
        public string Content { set; get; }
    }
}
