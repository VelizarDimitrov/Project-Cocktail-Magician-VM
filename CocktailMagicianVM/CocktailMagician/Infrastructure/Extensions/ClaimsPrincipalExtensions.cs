using System;
using System.Security.Claims;

namespace CocktailMagician.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier);
            var parsedUserId = int.Parse(userId?.Value);

            return parsedUserId;
        }
    }
}
