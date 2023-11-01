using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domian.IdentityDtos
{
    public class RegisterDto
    {
        
        public IFormFile? ProfileImage { get; set; }
        
        [StringLength(50)]
        public string Number { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(128)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(256)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmedPassword { get; set; }
    }
}