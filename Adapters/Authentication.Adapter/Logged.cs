using Authentication.Adapter.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Authentication.Adapter
{
    public class Logged
    {
        public static UserLogged GetUserLogged(IHttpContextAccessor _accessor)
        {
            return GetUser(_accessor);
        }

        private static UserLogged GetUser(IHttpContextAccessor _accessor)
        {
            var httpContext = _accessor.HttpContext;

            if (httpContext?.User == null)
                return null;

            var userIdentity = httpContext?.User.Identity;
            var identity = userIdentity as ClaimsIdentity;

            if (identity == null)
                return null;

            var claims = identity.Claims.ToList();

            if (claims.Count == 0)
                return null;

            return new UserLogged()
            {
                Id = Guid.Parse(GetClaimValue(claims, "jti")),
                Login = GetClaimValue(claims, "unique_name"),
                Name = GetClaimValue(claims, "userName"),
                Role = GetClaimValue(claims, "role")
            };
        }

        private static string GetClaimValue(IEnumerable<Claim> claims, string type)
        {
            return claims.FirstOrDefault(c => c.Type == type)?.Value;
        }
    }
}
