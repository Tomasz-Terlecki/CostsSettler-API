using AutoMapper;
using CostsSettler.Auth.Models;
using CostsSettler.Domain.Dtos.Circumstance;
using CostsSettler.Domain.Dtos.User;
using CostsSettler.Domain.Models;

namespace CostsSettler.Domain.Profiles;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserForListDto>();
        CreateMap<KcUser, User>();

        CreateMap<Circumstance, CircumstanceForListDto>();
        CreateMap<Circumstance, CircumstanceForReturnDto>();
    }
}
