using ApproveMe.Core.Interfaces;
using Flozacode.Helpers.StringHelper;
using Flozacode.Repository;
using ApproveMe.Core.Constants;
using ApproveMe.Core.Services.RedisCaching;
using ApproveMe.Repository.Contexts;
using ApproveMe.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApproveMe.Core.Services;

public class UserSessionService(IFlozaRepo<UserSession, AppDbContext> repo) : IUserSessionService
{
    public async Task<string?> CreateAsync(long userId)
    {
        var entity = new UserSession
        {
            UserId = userId,
            Code = FlozaString.GenerateRandomString(10),
            CreatedAt = DateTime.UtcNow,
            IsValid = true,
        };

        var affected = await repo.AddAsync(entity);
        return affected <= 0 ? null : entity.Code;
    }

    public bool CheckCode(string code)
    {
        return repo.AsQueryable.AsNoTracking().Any(q => q.Code == code && q.IsValid);
    }

    public async Task<int?> InvalidateSessionAsync(string code)
    {
        var existing = repo.AsQueryable.FirstOrDefault(q => q.Code == code);
        if (existing == null) return null;
       
        existing.IsValid = false;
       
       return await repo.UpdateAsync(existing);
    }

    public async Task<string?> GetLastSessionAsync(long userId)
    {
        var existing = await repo.AsQueryable.FirstOrDefaultAsync(q => q.UserId == userId && q.IsValid);
        return existing?.Code;
    }
}