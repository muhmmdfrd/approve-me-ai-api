namespace ApproveMe.Core.Interfaces;

public interface IUserSessionService
{
    public Task<string?> CreateAsync(long userId);
    public bool CheckCode(string code);
    public Task<int?> InvalidateSessionAsync(string code);
    public Task<string?> GetLastSessionAsync(long userId);
}