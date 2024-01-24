using AutoMapper;
using Fidenz.Customers.Data.Models;
using Fidenz.Customers.Data.Models.Dto;

namespace Fidenz.Customers.Data.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, EditUserDto>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<EditUserDto, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LoginDto, User>();
            CreateMap<User, UsersByZipCodeDto>();
        }
    }
}
