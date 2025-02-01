using ApproveMe.Core.Dtos;
using ApproveMe.Core.Interfaces;
using ApproveMe.Core.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flozacode.Exceptions;
using Flozacode.Extensions.SortExtension;
using Flozacode.Models.Paginations;
using Flozacode.Repository;
using ApproveMe.Repository.Contexts;
using ApproveMe.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApproveMe.Core.Services;

public class UserService(IFlozaRepo<User, AppDbContext> repo, IMapper mapper) : IUserService
{
    public async Task<Pagination<UserViewDto>> GetPagedAsync(UserFilter filter)
    {
        var query = repo.AsQueryable.AsNoTracking();
        var total = query.Count();
        int filtered;

        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            query = query.Where(q => q.Name.ToLower().Contains(filter.Keyword.ToLower()));
            filtered = await query.CountAsync();
        }
        
        var result = await query
            .ProjectTo<UserViewDto>(mapper.ConfigurationProvider)
            .SortBy(filter.SortName, filter.SortDir)
            .Skip(filter.Skip)
            .Take(filter.Size)
            .ToListAsync();
        
        
        filtered = result.Count;

        return new Pagination<UserViewDto>
        {
            Size = filter.Size,
            Data = result,
            Filtered = filtered,
            Total = total,
        };
    }

    public async Task<List<UserViewDto>> GetListAsync()
    {
        return await repo.AsQueryable
            .AsNoTracking()
            .ProjectTo<UserViewDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<UserViewDto> FindAsync(long id)
    {
        var user = await repo.AsQueryable.AsNoTracking().ProjectTo<UserViewDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(q => q.Id == id);
        if (user == null)
        {
            throw new RecordNotFoundException("User not found.");
        }

        return user;
    }

    public Task<int> CreateAsync(UserAddDto value)
    {
        var entity = mapper.Map<User>(value);
        return repo.AddAsync(entity);
    }

    public async Task<int> UpdateAsync(UserUpdDto value)
    {
        var existing = await repo.AsQueryable.FirstOrDefaultAsync(q => q.Id == value.Id);
        if (existing == null)
        {
            throw new RecordNotFoundException("User not found.");
        }

        mapper.Map(value, existing);
        return await repo.UpdateAsync(existing);
    }

    public async Task<int> DeleteAsync(long id)
    {
        var entity = await repo.AsQueryable.FirstOrDefaultAsync(q => q.Id == id);
        if (entity == null)
        {
            throw new RecordNotFoundException("User not found.");
        }

        return await repo.DeleteAsync(entity);
    }

    public async Task<int> DeleteAsync(long id, CurrentUser currentUser, bool isHardDelete = false)
    {
        if (isHardDelete)
        {
            return await DeleteAsync(id);
        }

        var entity = await repo.AsQueryable.FirstOrDefaultAsync(q => q.Id == id);
        if (entity == null)
        {
            throw new RecordNotFoundException("User not found.");
        }
        
        entity.ModifiedBy = currentUser.Id;
        entity.ModifiedAt = DateTime.UtcNow;

        return await repo.UpdateAsync(entity);
    }

    public async Task<int> UpdateFirebaseTokenAsync(long id, string token)
    {
        var user = await repo.AsQueryable.FirstOrDefaultAsync(q => q.Id == id);
        if (user == null)
        {
            return 0;
        }

        user.ModifiedBy = id;
        user.ModifiedAt = DateTime.UtcNow;
        
        return await repo.UpdateAsync(user);
    }
}