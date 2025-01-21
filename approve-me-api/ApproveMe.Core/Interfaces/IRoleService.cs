using ApproveMe.Core.Dtos;
using ApproveMe.Core.Models;
using Flozacode.Models.Paginations;

namespace ApproveMe.Core.Interfaces;

public interface IRoleService : IFlozaPagination<RoleViewDto, RoleAddDto, RoleUpdDto, RoleFilter>
{
    Task<int> DeleteAsync(long id, CurrentUser currentUser, bool isHardDelete = false);
}