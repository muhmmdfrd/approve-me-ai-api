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

public class RoleService : IRoleService
{
    private readonly IFlozaRepo<Role, AppDbContext> _repo;
    private readonly IMapper _mapper;

    public RoleService(IFlozaRepo<Role, AppDbContext> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Pagination<RoleViewDto>> GetPagedAsync(RoleFilter filter)
    {
        var query = _repo.AsQueryable.AsNoTracking();
        var total = query.Count();
        var filtered = total;

        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            query = query.Where(q => q.Name.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase));
            filtered = query.Count();
        }

        var result = await query
            .ProjectTo<RoleViewDto>(_mapper.ConfigurationProvider)
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
        return await _repo.AsQueryable
            .AsNoTracking()
            .ProjectTo<RoleViewDto>(_mapper.ConfigurationProvider)
            .Where(q => q.DataStatusId == (int)DataStatusEnum.Active)
            .ToListAsync();
    }

    public async Task<RoleViewDto> FindAsync(long id)
    {
        var user = await _repo.AsQueryable.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
        if (user == null)
        {
            throw new RecordNotFoundException("Role not found.");
        }
        
        return _mapper.Map<RoleViewDto>(user);
    }

    public Task<int> CreateAsync(RoleAddDto value)
    {
        var entity = _mapper.Map<Role>(value);
        return _repo.AddAsync(entity);
    }

    public Task<int> UpdateAsync(RoleUpdDto value)
    {
        var existing = _repo.AsQueryable.FirstOrDefault(q => q.Id == value.Id);
        if (existing == null)
        {
            throw new RecordNotFoundException("Role not found.");
        }

        _mapper.Map(value, existing);
        return _repo.UpdateAsync(existing);
    }

    public Task<int> DeleteAsync(long id)
    {
        var entity = _repo.AsQueryable.FirstOrDefault(q => q.Id == id);
        if (entity == null)
        {
            throw new RecordNotFoundException("Role not found.");
        }

        return _repo.DeleteAsync(entity);
    }

    public Task<int> DeleteAsync(long id, CurrentUser currentUser, bool isHardDelete = false)
    {
        if (isHardDelete)
        {
            return DeleteAsync(id);
        }

        var entity = _repo.AsQueryable.FirstOrDefault(q => q.Id == id);
        if (entity == null)
        {
            throw new RecordNotFoundException("Role not found.");
        }
        
        entity.ModifiedBy = currentUser.Id;
        entity.ModifiedAt = DateTime.UtcNow;

        return _repo.UpdateAsync(entity);
    }
}