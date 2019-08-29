using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;

namespace MyPhamUsa.Extensions
{
    public static class JwtTokenExtension
    {
        public static string GetGuid(this ClaimsPrincipal user)
        {
            if (user.HasClaim(c => c.Type.Equals(new ClaimsIdentityOptions().UserIdClaimType)))
            {
                return user.Claims.FirstOrDefault(c => c.Type.Equals(new ClaimsIdentityOptions().UserIdClaimType)).Value;
            }
            return null;
        }
    }
}
