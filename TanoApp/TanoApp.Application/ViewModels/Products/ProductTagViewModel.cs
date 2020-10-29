using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Application.ViewModels.Common;

namespace TanoApp.Application.ViewModels.Products
{
    public class ProductTagViewModel
    {
        public int Id { set; get; }
        public int ProductId { get; set; }
        public string TagId { set; get; }
        public ProductViewModel Product { set; get; }
        public TagViewModel Tag { set; get; }
    }
}
