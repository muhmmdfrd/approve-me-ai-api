using System.Transactions;
using ApproveMe.Core.Dtos;
using ApproveMe.Core.Interfaces;
using ApproveMe.Core.Models;
using Flozacode.Models.Paginations;

namespace ApproveMe.Core.Helpers;

public class RoleHelper
{
    private readonly IRoleService _service;

    public RoleHelper(IRoleService service)
    {
        _service = service;
    }

    public Task<Pagination<RoleViewDto>> GetPagedAsync(RoleFilter filter)
    {
        return _service.GetPagedAsync(filter);
    }

    public Task<List<RoleViewDto>> GetListAsync()
    {
        return _service.GetListAsync();
    }

    public Task<RoleViewDto> FindAsync(long id)
    {
        return _service.FindAsync(id);
    }

    public async Task<int> CreateAsync(RoleAddDto value, CurrentUser currentUser)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        {
            var now = DateTime.UtcNow;

            value.CreatedBy = currentUser.Id;
            value.CreatedAt = now;
            value.ModifiedBy = currentUser.Id;
            value.ModifiedAt = now;
        
            var result = await _service.CreateAsync(value);
            transaction.Complete();
            
            return result;
        }
    }

    public Task<int> UpdateAsync(RoleUpdDto value, CurrentUser currentUser)
    {
        value.ModifiedBy = currentUser.Id;
        value.ModifiedAt = DateTime.UtcNow;

        return _service.UpdateAsync(value);
    }

    public Task<int> DeleteAsync(long id, CurrentUser currentUser, bool isHardDelete)
    {
        return _service.DeleteAsync(id, currentUser, isHardDelete);
    }
}