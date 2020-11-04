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

            CreateMap<FunctionViewModel, Function>();
        }
    }
}
