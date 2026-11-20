using AutoMapper;
using E_commerce_System.DTOs.ProductDTOs;
using E_commerce_System.Entities;

namespace E_commerce_System.Helper
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetProductResponse>()
                .ForMember(des=>des.ProductName,
                opt=>opt.MapFrom(src=>src.Name))
                
                .ForMember(des=>des.CategoryName,
                    op=>op.MapFrom(src=>src.Category.Name)).ReverseMap();
            //CreateMap<Product, GetProductResponse>()
            //    .ForMember(des=>des.ProductName,
            //    opt=>opt.MapFrom(src=>src.Name))
                
            //    .ForMember(des=>des.CategoryName,
            //        op=>op.MapFrom(src=>src.Category.Name)).ReverseMap();

        }
    }
}
