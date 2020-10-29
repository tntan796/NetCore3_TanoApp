namespace TanoApp.Application.ViewModels.Products
{
    public class WholePriveViewModel
    {
        public int Id { set; get; }
        public int ProductId { get; set; }

        public int FromQuantity { get; set; }

        public int ToQuantity { get; set; }

        public decimal Price { get; set; }

        public ProductViewModel Product { get; set; }
    }
}
