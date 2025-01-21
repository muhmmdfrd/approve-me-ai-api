namespace ApproveMe.Core.Dtos;

public class AuthRequestDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class AuthRevokeRequestDto
{
    public string Code { get; set; } = null!;
    public long UserId { get; set; }
}

public class LogoutRequestDto
{
    public string Code { get; set; } = null!;
}

public class AuthResponseDto
{
    public string Code { get; set; } = null!;
    public string Token { get; set; } = null!;
    public UserViewDto User { get; set; } = null!;
}

public class AuthRevokeResponseDto
{
    public string Code { get; set; } = null!;
    public string Token { get; set; } = null!;
}