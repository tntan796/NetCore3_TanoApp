using System.ComponentModel.DataAnnotations;

namespace TanoApp.Application.ViewModels.Products
{
    public class ProductImageViewModel
    {
        public int Id { set; get; }
        [StringLength(255)]
        public int ProductId { set; get; }
        [StringLength(255)]
        public string Path { set; get; }
        [StringLength(255)]
        public string Caption { set; get; }
        public ProductViewModel Product { get; set; }
    }
}
