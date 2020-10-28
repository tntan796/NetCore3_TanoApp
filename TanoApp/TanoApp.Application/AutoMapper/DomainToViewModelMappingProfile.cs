using AutoMapper;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Data.Entities;

namespace TanoApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
        }
    }
}
