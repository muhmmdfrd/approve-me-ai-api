using AutoMapper;
using ApproveMe.Core.Dtos;
using ApproveMe.Repository.Entities;

namespace ApproveMe.Api.Mappings;

public class GeneralProfile : Profile
{
    private static readonly string[] IgnoredPropertyNames = ["CreatedAt", "CreatedBy", "ModifiedAt", "ModifiedBy", "DataStatusId", "Code"];

    public GeneralProfile()
    {
        ShouldMapProperty = p => !IgnoredPropertyNames.Contains(p.Name);
            
        CreateMap<User, UserViewDto>().ReverseMap();
        CreateMap<UserAddDto, User>();
        CreateMap<UserUpdDto, User>();
        
        CreateMap<Role, RoleViewDto>().ReverseMap();
        CreateMap<RoleAddDto, Role>();
        CreateMap<RoleUpdDto, Role>();
    }
}