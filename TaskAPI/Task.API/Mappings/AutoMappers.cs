using AutoMapper;
using Task.API.Models;
using Task.API.Models.DTO;

namespace Task.API.Mappings
{
    public class AutoMappers : Profile
    {
        public AutoMappers()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, AddCategoryRequestDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryRequestDTO>().ReverseMap();

            CreateMap<Tag, TagDTO>().ReverseMap();
            CreateMap<Tag, AddTagRequestDTO>().ReverseMap();
            CreateMap<Tag, UpdateTagRequestDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>()
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
    .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag.TagName)));

            CreateMap<Product, AddProductRequestDTO>().ReverseMap();
            CreateMap<Product, UpdateProductRequestDTO>().ReverseMap();




        }
    }
}
