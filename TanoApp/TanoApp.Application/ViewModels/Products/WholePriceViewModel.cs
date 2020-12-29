namespace TanoApp.Application.ViewModels.Products
{
    public class WholePriceViewModel
    {
        public int Id { set; get; }
        public int ProductId { get; set; }

        public int FromQuantity { get; set; }

        public int ToQuantity { get; set; }

        public decimal Price { get; set; }

        public ProductViewModel Product { get; set; }
    }
}
