using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domian.IdentityDtos
{
    public class TokenRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}