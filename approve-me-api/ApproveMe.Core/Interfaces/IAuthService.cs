using ApproveMe.Core.Dtos;
using ApproveMe.Repository.Entities;

namespace ApproveMe.Core.Interfaces;

public interface IAuthService
{
    public Task<User?> AuthAsync(AuthRequestDto request);
}