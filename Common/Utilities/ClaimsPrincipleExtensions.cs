using System;
using System.Linq;
using System.Security.Claims;

namespace Common.Utilities
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public static string[] GetRoles(this ClaimsPrincipal user)
        {
            var roles = user.Claims
                 .Where(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)
                 .Select(s => s.Value).ToArray();

            return roles;
        }
    }
}
