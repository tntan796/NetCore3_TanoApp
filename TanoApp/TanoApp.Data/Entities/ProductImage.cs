using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TanoApp.Data.Entities
{
    [Table("ProductImages")]
    public class ProductImage
    {
        [StringLength(255)]
        public int ProductId { set; get; }
        [StringLength(255)]
        public string Path { set; get; }
        [StringLength(255)]
        public string Caption { set; get; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
