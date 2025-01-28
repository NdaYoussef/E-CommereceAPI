using AutoMapper;
using TestToken.DTO.UserDtos;
using TestToken.Models;

namespace TestToken.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto,ApplicationUser>().ReverseMap();
            CreateMap<userDto, ApplicationUser>().ReverseMap();

        }

    }
}
