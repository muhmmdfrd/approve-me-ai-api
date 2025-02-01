using System.Transactions;
using ApproveMe.Core.Dtos;
using ApproveMe.Core.Interfaces;
using ApproveMe.Core.Models;
using Flozacode.Models.Paginations;

namespace ApproveMe.Core.Helpers;

public class LetterHelper(ILetterService service)
{
    public Task<Pagination<LetterViewDto>> GetPagedAsync(LetterFilter filter)
    {
        return service.GetPagedAsync(filter);
    }

    public Task<LetterViewDto> FindAsync(long id)
    {
        return service.FindAsync(id);
    }

    public async Task<int> CreateAsync(LetterAddDto value, CurrentUser currentUser)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        {
            var now = DateTime.UtcNow;

            value.CreatedBy = currentUser.Id;
            value.CreatedAt = now;
            value.ModifiedBy = currentUser.Id;
            value.ModifiedAt = now;
        
            var result = await service.CreateAsync(value);
            
            transaction.Complete();
            
            return result;
        }
    }

    public Task<int> UpdateAsync(LetterUpdDto value, CurrentUser currentUser)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        {
            value.ModifiedBy = currentUser.Id;
            value.ModifiedAt = DateTime.UtcNow;
            
            var result = service.UpdateAsync(value);
            transaction.Complete();
            return result;
        }
    }
    
    public Task<int> DeleteAsync(long id, CurrentUser currentUser, bool isHardDelete)
    {
        return service.DeleteAsync(id, currentUser, isHardDelete);
    }
}