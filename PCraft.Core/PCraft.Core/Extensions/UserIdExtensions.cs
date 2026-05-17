using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace PCraft.Core.Extensions
{
    public static class UserIdExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(sub, out var id) ? id : null;
        }
    }
}
