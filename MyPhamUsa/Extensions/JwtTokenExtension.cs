using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

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
