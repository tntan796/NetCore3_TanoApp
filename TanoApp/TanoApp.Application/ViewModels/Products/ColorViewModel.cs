using System.ComponentModel.DataAnnotations;

namespace TanoApp.Application.ViewModels.Products
{
    public class ColorViewModel
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { set; get; }
        [StringLength(255)]
        public string Code { set; get; }
    }
}
