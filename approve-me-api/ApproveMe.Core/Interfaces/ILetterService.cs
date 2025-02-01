using ApproveMe.Core.Dtos;
using ApproveMe.Core.Models;
using Flozacode.Models.Paginations;

namespace ApproveMe.Core.Interfaces;

public interface ILetterService : IFlozaPagination<LetterViewDto, LetterAddDto, LetterUpdDto, LetterFilter>
{
    Task<int> DeleteAsync(long id, CurrentUser currentUser, bool isHardDelete = false);
}