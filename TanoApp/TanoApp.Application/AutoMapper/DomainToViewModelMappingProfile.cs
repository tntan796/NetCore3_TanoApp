using AutoMapper;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Application.ViewModels.System;
using TanoApp.Data.Entities;

namespace TanoApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();

            CreateMap<Function, FunctionViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
        }
    }
}
