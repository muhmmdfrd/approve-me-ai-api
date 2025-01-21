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
        var filtered = total;

        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            query = query.Where(q => q.Name.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase));
            filtered = query.Count();
        }

        var result = await query
            .ProjectTo<UserViewDto>(mapper.ConfigurationProvider)
            .SortBy(filter.SortName, filter.SortDir)
            .Skip(filter.Skip)
            .Take(filter.Size)
            .ToListAsync();

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

    public Task<int> UpdateAsync(UserUpdDto value)
    {
        var existing = repo.AsQueryable.FirstOrDefault(q => q.Id == value.Id);
        if (existing == null)
        {
            throw new RecordNotFoundException("User not found.");
        }

        mapper.Map(value, existing);
        return repo.UpdateAsync(existing);
    }

    public Task<int> DeleteAsync(long id)
    {
        var entity = repo.AsQueryable.FirstOrDefault(q => q.Id == id);
        if (entity == null)
        {
            throw new RecordNotFoundException("User not found.");
        }

        return repo.DeleteAsync(entity);
    }

    public Task<int> DeleteAsync(long id, CurrentUser currentUser, bool isHardDelete = false)
    {
        if (isHardDelete)
        {
            return DeleteAsync(id);
        }

        var entity = repo.AsQueryable.FirstOrDefault(q => q.Id == id);
        if (entity == null)
        {
            throw new RecordNotFoundException("User not found.");
        }
        
        entity.ModifiedBy = currentUser.Id;
        entity.ModifiedAt = DateTime.UtcNow;

        return repo.UpdateAsync(entity);
    }

    public Task<int> UpdateFirebaseTokenAsync(long id, string token)
    {
        var user = repo.AsQueryable.FirstOrDefault(q => q.Id == id);
        if (user == null)
        {
            return Task.FromResult(0);
        }

        user.ModifiedBy = id;
        user.ModifiedAt = DateTime.UtcNow;
        
        return repo.UpdateAsync(user);
    }
}