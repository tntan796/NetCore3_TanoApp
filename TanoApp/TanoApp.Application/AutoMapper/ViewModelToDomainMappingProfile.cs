using AutoMapper;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Application.ViewModels.System;
using TanoApp.Data.Entities;

namespace TanoApp.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile: Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory> ()
                .ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image, c.HomeFlag,
                c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<FunctionViewModel, Function>().MaxDepth(2);
            CreateMap<ProductViewModel, Product>().MaxDepth(2);
            CreateMap<AppUserViewModel, AppUser>().MaxDepth(2);
            CreateMap<AppRoleViewModel, AppRole>().MaxDepth(2);
            CreateMap<PermissionViewModel, Permission>().MaxDepth(2);
            CreateMap<BillViewModel, Bill>().MaxDepth(2);
            CreateMap<BillDetailViewModel, BillDetail>().MaxDepth(2);
            CreateMap<SizeViewModel, Size>().MaxDepth(2);
            CreateMap<ColorViewModel, Color>().MaxDepth(2);
            CreateMap<ProductQuantityViewModel, ProductQuantity>().MaxDepth(2);
            CreateMap<ProductImageViewModel, ProductImage>().MaxDepth(2);
            CreateMap<WholePriceViewModel, WholePrice>().MaxDepth(2);
        }
    }
}
