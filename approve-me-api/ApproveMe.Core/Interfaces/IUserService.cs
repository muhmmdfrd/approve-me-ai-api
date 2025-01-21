using ApproveMe.Core.Dtos;
using ApproveMe.Core.Models;
using Flozacode.Models.Paginations;

namespace ApproveMe.Core.Interfaces;

public interface IUserService : IFlozaPagination<UserViewDto, UserAddDto, UserUpdDto, UserFilter>
{
    Task<int> DeleteAsync(long id, CurrentUser currentUser, bool isHardDelete = false);
    Task<int> UpdateFirebaseTokenAsync(long id, string token);
}