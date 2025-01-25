using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApproveMe.Api.Attributes;

public class CustomAuthorizationAttribute(params string[] roles) : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (roles.Length == 0)
        {
            return;
        }
        
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
        {
            return;
        }

        var user = context.HttpContext.User;
        if (user.Identity is { IsAuthenticated: false })
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var hasRole = roles.Any(role => user.HasClaim(ClaimTypes.Role, role));
        if (!hasRole)
        {
            context.Result = new ForbidResult();
        }
    }
}