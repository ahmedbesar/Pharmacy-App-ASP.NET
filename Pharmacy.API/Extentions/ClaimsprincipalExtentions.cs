using System.Security.Claims;

namespace Pharmacy.API.Extentions
{
    public static class ClaimsprincipalExtentions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }
    }
}
