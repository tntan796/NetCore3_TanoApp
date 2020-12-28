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

            CreateMap<Function, FunctionViewModel>().MaxDepth(2);
            CreateMap<Product, ProductViewModel>().MaxDepth(2);
            CreateMap<AppUser, AppUserViewModel>().MaxDepth(2);
            CreateMap<AppRole, AppRoleViewModel>().MaxDepth(2);
            CreateMap<Permission, PermissionViewModel>().MaxDepth(2);
            CreateMap<Bill, BillViewModel>().MaxDepth(2);
            CreateMap<BillDetail, BillDetailViewModel>().MaxDepth(2);
            CreateMap<Size, SizeViewModel>().MaxDepth(2);
            CreateMap<Color, ColorViewModel>().MaxDepth(2);
            CreateMap<ProductQuantity, ProductQuantityViewModel>().MaxDepth(2);
            CreateMap<ProductImage, ProductImageViewModel>().MaxDepth(2);
            CreateMap<WholePrice, WholePriceViewModel>().MaxDepth(2);
        }
    }
}
