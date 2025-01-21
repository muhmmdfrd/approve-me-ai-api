using ApproveMe.Core.Dtos;
using ApproveMe.Core.Enums;
using ApproveMe.Core.Interfaces;
using AutoMapper;
using BCrypt.Net;
using Flozacode.Repository;
using ApproveMe.Repository.Contexts;
using ApproveMe.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApproveMe.Core.Services;

public class AuthService(IFlozaRepo<User, AppDbContext> repo) : IAuthService
{
    public async Task<User?> AuthAsync(AuthRequestDto request)
    {
        var user = await repo.AsQueryable
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Username == request.Username);

        if (user == null)
        {
            return null;
        }

        return !BCrypt.Net.BCrypt.Verify(request.Password, user.Password) ? null : user;
    }
}