using ApproveMe.Core.Interfaces;

namespace ApproveMe.Core.Helpers;

public class UserSessionHelper
{
    private readonly IUserSessionService _service;

    public UserSessionHelper(IUserSessionService service)
    {
        _service = service;
    }

    public Task<string?> CreateAsync(long userId)
    {
        return _service.CreateAsync(userId);
    }

    public bool CheckCode(string code)
    {
        return _service.CheckCode(code);
    }

    public Task<int?> InvalidateSessionAsync(string code)
    {
        return _service.InvalidateSessionAsync(code);
    }

    public Task<string?> GetLastSessionAsync(long userId)
    {
        return _service.GetLastSessionAsync(userId);
    }
}