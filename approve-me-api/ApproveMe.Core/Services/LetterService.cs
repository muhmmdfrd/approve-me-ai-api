using ApproveMe.Core.Dtos;
using ApproveMe.Core.Interfaces;
using ApproveMe.Core.Models;
using ApproveMe.Repository.Contexts;
using ApproveMe.Repository.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flozacode.Exceptions;
using Flozacode.Extensions.SortExtension;
using Flozacode.Models.Paginations;
using Flozacode.Repository;
using Microsoft.EntityFrameworkCore;

namespace ApproveMe.Core.Services;

public class LetterService(IFlozaRepo<Letter, AppDbContext> repo, IMapper mapper) : ILetterService
{
    public async Task<Pagination<LetterViewDto>> GetPagedAsync(LetterFilter filter)
    {
        var query = repo.AsQueryable.AsNoTracking();
        var total = query.Count();
        var filtered = total;

        if (!string.IsNullOrEmpty(filter.Keyword))
        {
            query = query.Where(q => q.Title.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase));
            filtered = await query.CountAsync();
        }

        var result = await query
            .ProjectTo<LetterViewDto>(mapper.ConfigurationProvider)
            .SortBy(filter.SortName, filter.SortDir)
            .Skip(filter.Skip)
            .Take(filter.Size)
            .ToListAsync();

        return new Pagination<LetterViewDto>
        {
            Size = filter.Size,
            Data = result,
            Filtered = filtered,
            Total = total,
        };
    }

    public async Task<List<LetterViewDto>> GetListAsync()
    {
        return await repo.AsQueryable
            .AsNoTracking()
            .ProjectTo<LetterViewDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<LetterViewDto> FindAsync(long id)
    {
        var user = await repo.AsQueryable.AsNoTracking().ProjectTo<LetterViewDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(q => q.Id == id);
        if (user == null)
        {
            throw new RecordNotFoundException("Letter not found.");
        }

        return user;
    }

    public Task<int> CreateAsync(LetterAddDto value)
    {
        var entity = mapper.Map<Letter>(value);
        return repo.AddAsync(entity);
    }

    public async Task<int> UpdateAsync(LetterUpdDto value)
    {
        var existing = await repo.AsQueryable.FirstOrDefaultAsync(q => q.Id == value.Id);
        if (existing == null)
        {
            throw new RecordNotFoundException("Letter not found.");
        }

        mapper.Map(value, existing);
        return await repo.UpdateAsync(existing);
    }

    public async Task<int> DeleteAsync(long id)
    {
        var entity = await repo.AsQueryable.FirstOrDefaultAsync(q => q.Id == id);
        if (entity == null)
        {
            throw new RecordNotFoundException("Letter not found.");
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
            throw new RecordNotFoundException("Letter not found.");
        }
        
        entity.ModifiedBy = currentUser.Id;
        entity.ModifiedAt = DateTime.UtcNow;

        return await repo.UpdateAsync(entity);
    }
}