using System.Transactions;
using ApproveMe.Core.Dtos;
using ApproveMe.Core.Interfaces;
using ApproveMe.Core.Models;
using ApproveMe.Core.Rpc.Interfaces;
using Flozacode.Models.Paginations;
using Textservice;
using UuidExtensions;

namespace ApproveMe.Core.Helpers;

public class UserHelper(IUserService service, IPredictionService predictionService)
{
    public Task<Pagination<UserViewDto>> GetPagedAsync(UserFilter filter)
    {
        return service.GetPagedAsync(filter);
    }
    
    public Task<UserViewDto> FindAsync(long id)
    {
        return service.FindAsync(id);
    }

    public async Task<int> CreateAsync(UserAddDto value, CurrentUser currentUser)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        {
            var now = DateTime.UtcNow;

            value.Password = BCrypt.Net.BCrypt.HashPassword(value.Password);
            value.CreatedBy = currentUser.Id;
            value.CreatedAt = now;
            value.ModifiedBy = currentUser.Id;
            value.ModifiedAt = now;
            value.Code = Uuid7.Guid().ToString();
        
            var result = await service.CreateAsync(value);
            
            transaction.Complete();
            
            return result;
        }
    }

    public Task<int> UpdateAsync(UserUpdDto value, CurrentUser currentUser)
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
    
    public Task<int> UpdateFirebaseTokenAsync(long id, string token)
    {
        return service.UpdateFirebaseTokenAsync(id, token);
    }

    public Task<int> DeleteAsync(long id, CurrentUser currentUser, bool isHardDelete)
    {
        return service.DeleteAsync(id, currentUser, isHardDelete);
    }

    public Task<string> SuggestAsync(TextRequest request)
    {
        return predictionService.SuggestUserName(request);
    }
}