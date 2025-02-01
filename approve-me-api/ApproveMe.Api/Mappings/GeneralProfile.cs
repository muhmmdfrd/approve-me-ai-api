using AutoMapper;
using ApproveMe.Core.Dtos;
using ApproveMe.Repository.Entities;

namespace ApproveMe.Api.Mappings;

public class GeneralProfile : Profile
{
    private static readonly string[] IgnoredPropertyNames = ["CreatedAt", "CreatedBy", "ModifiedAt", "ModifiedBy", "DataStatusId"];

    public GeneralProfile()
    {
        ShouldMapProperty = p => !IgnoredPropertyNames.Contains(p.Name);
            
        CreateMap<User, UserViewDto>().ReverseMap();
        CreateMap<UserAddDto, User>();
        CreateMap<UserUpdDto, User>().ForMember(d => d.Code, a => a.Ignore());
        
        CreateMap<Role, RoleViewDto>().ReverseMap();
        CreateMap<RoleAddDto, Role>();
        CreateMap<RoleUpdDto, Role>();

        CreateMap<Letter, LetterViewDto>()
            .ForMember(d => d.CreatorName, conf => conf.MapFrom(e => e.Creator.Name))
            .ReverseMap();
        CreateMap<LetterAddDto, Letter>();
        CreateMap<LetterUpdDto, Letter>();
    }
}