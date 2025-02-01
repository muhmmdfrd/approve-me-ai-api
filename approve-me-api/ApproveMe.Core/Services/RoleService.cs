using ApproveMe.Core.Dtos;
using ApproveMe.Core.Enums;
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

public class RoleService(IFlozaRepo<Role, AppDbContext> repo, IMapper mapper) : IRoleService
{
    public async Task<Pagination<RoleViewDto>> GetPagedAsync(RoleFilter filter)
    {
        var query = repo.AsQueryable.AsNoTracking();
        var total = await query.CountAsync();
        var filtered = total;

        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            query = query.Where(q => q.Name.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase));
            filtered = await query.CountAsync();
        }

        var result = await query
            .ProjectTo<RoleViewDto>(mapper.ConfigurationProvider)
            .SortBy(filter.SortName, filter.SortDir)
            .Skip(filter.Skip)
            .Take(filter.Size)
            .ToListAsync();

        return new Pagination<RoleViewDto>
        {
            Size = filter.Size,
            Data = result,
            Filtered = filtered,
            Total = total,
        };
    }

    public async Task<List<RoleViewDto>> GetListAsync()
    {
        return await repo.AsQueryable
            .AsNoTracking()
            .ProjectTo<RoleViewDto>(mapper.ConfigurationProvider)
            .Where(q => q.DataStatusId == (int)DataStatusEnum.Active)
            .ToListAsync();
    }

    public async Task<RoleViewDto> FindAsync(long id)
    {
        var user = await repo.AsQueryable.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
        if (user == null)
        {
            throw new RecordNotFoundException("Role not found.");
        }
        
        return mapper.Map<RoleViewDto>(user);
    }

    public Task<int> CreateAsync(RoleAddDto value)
    {
        var entity = mapper.Map<Role>(value);
        return repo.AddAsync(entity);
    }

    public async Task<int> UpdateAsync(RoleUpdDto value)
    {
        var existing = await repo.AsQueryable.FirstOrDefaultAsync(q => q.Id == value.Id);
        if (existing == null)
        {
            throw new RecordNotFoundException("Role not found.");
        }

        mapper.Map(value, existing);
        return await repo.UpdateAsync(existing);
    }

    public async Task<int> DeleteAsync(long id)
    {
        var entity = await repo.AsQueryable.FirstOrDefaultAsync(q => q.Id == id);
        if (entity == null)
        {
            throw new RecordNotFoundException("Role not found.");
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
            throw new RecordNotFoundException("Role not found.");
        }
        
        entity.ModifiedBy = currentUser.Id;
        entity.ModifiedAt = DateTime.UtcNow;

        return await repo.UpdateAsync(entity);
    }
}