using AutoMapper;
using DOTN_DataAccess.Data;
using DOTN_Models;

namespace DOTN_Business.Mapper
{
	public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
