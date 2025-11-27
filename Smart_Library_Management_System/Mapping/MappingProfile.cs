using AutoMapper;
using Smart_Library_Management_System.DTOs;
using Smart_Library_Management_System.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Smart_Library_Management_System.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Book mappings
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<CreateBookDto, Book>();

            // Loan mappings
            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue));

            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.GetType().Name));

            // Catalog base mapping
            CreateMap<Catalog, CatalogDto>();

            // Catalog details mapping
            CreateMap<Catalog, CatalogDetailsDto>() 
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
        }
    }
}
