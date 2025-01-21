using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Transactions;
using ApproveMe.Core.Constants;
using ApproveMe.Core.Dtos;
using ApproveMe.Core.Interfaces;
using ApproveMe.Core.Services.RedisCaching;
using ApproveMe.Core.Settings;
using Flozacode.Helpers.StringHelper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApproveMe.Core.Helpers;

public class AuthHelper(
    IAuthService service,
    RedisService redisService,
    IOptions<JwtConfigs> jwtConfigs,
    IUserService userService)
{
    private readonly JwtConfigs _jwtConfigs = jwtConfigs.Value;

    public async Task<AuthResponseDto?> AuthAsync(AuthRequestDto request)
    {
        var entity = await service.AuthAsync(request);
        if (entity == null)
        {
            return null;
        }

        var user = await userService.FindAsync(entity.Id);
            
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        {
            var existingSession = await redisService.HashGetAsync(RedisConstant.UserSession, user.Id.ToString());
            if (!string.IsNullOrEmpty(existingSession))
            {
                await redisService.HashDeleteAsync(RedisConstant.UserSession, user.Id.ToString());
            }

            var sessionCode = FlozaString.GenerateRandomString(8);
            var success = await redisService.HashSetAsync(RedisConstant.UserSession, user.Id.ToString(), sessionCode);
            if (!success)
            {
                return null;
            }

            var token = GenerateToken(user, sessionCode);

            transaction.Complete();

            return new AuthResponseDto
            {
                User = user,
                Code = sessionCode,
                Token = token,
            };
        }
    }
    
    public async Task<AuthRevokeResponseDto?> RevokeAsync(AuthRevokeRequestDto request)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        {
            var existingSessionCode = await redisService.HashGetAsync(RedisConstant.UserSession, request.UserId.ToString());
            if (string.IsNullOrEmpty(existingSessionCode))
            {
                return null;
            }
            
            var success = await redisService.HashDeleteAsync(RedisConstant.UserSession, request.UserId.ToString());
            if (success)
            {
                return null;
            }

            var sessionCode = FlozaString.GenerateRandomString(8);
            success = await redisService.HashSetAsync(RedisConstant.UserSession, request.UserId.ToString(), sessionCode);
            if (!success)
            {
                return null;
            }

            var user = await userService.FindAsync(request.UserId);
            if (user == null)
            {
                return null;
            }
            
            var token = GenerateToken(user, sessionCode);
            transaction.Complete();

            return new AuthRevokeResponseDto
            {
                Token = token,
                Code = sessionCode,
            };
        }
    }

    public async Task<bool> LogoutAsync(LogoutRequestDto request)
    {
        var sessions = await redisService.HashGetAllAsync(RedisConstant.UserSession);
        foreach (var session in sessions.Where(session => session.Value == request.Code))
        {
            return await redisService.HashDeleteAsync(RedisConstant.UserSession, session.Key);
        }

        return false;
    }
    
    private string GenerateToken(UserViewDto user, string sessionCode)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfigs.TokenSecret);

        var claims = new List<Claim> 
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Username", user.Username),
            new Claim("Name", user.Name),
            new Claim("sessionCode", sessionCode)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddSeconds(_jwtConfigs.TokenLifeTimes),
            Issuer = _jwtConfigs.Issuer,
            Audience = _jwtConfigs.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}