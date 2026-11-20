using AutoMapper;
using E_commerce_System.DTOs.CateroryDTOs;
using E_commerce_System.Entities;

namespace E_commerce_System.Helper
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, GetCategoryResponse>();
        }
    }
}
