using AutoMapper;
using CostsSettler.Auth.Models;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Models;

namespace CostsSettler.Domain.Profiles;

/// <summary>
/// Domain AutoMapper profiles.
/// </summary>
public class AutoMapperProfiles : Profile
{
    /// <summary>
    /// Creates AutoMapperProfiles instance.
    /// </summary>
    public AutoMapperProfiles()
    {
        CreateMap<User, UserForListDto>();
        CreateMap<KcUser, User>();

        CreateMap<Circumstance, CircumstanceForListDto>();
        CreateMap<Circumstance, CircumstanceForReturnDto>();

        CreateMap<Charge, ChargeForListDto>()
            .ForMember(
                dst => dst.CircumstanceDescription, 
                opt => opt.MapFrom(src => src.Circumstance.Description))
            .ForMember(
                dst => dst.CircumstanceStatus,
                opt => opt.MapFrom(src => src.Circumstance.CircumstanceStatus));
    }
}
