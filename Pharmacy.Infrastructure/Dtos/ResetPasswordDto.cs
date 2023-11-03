using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Dtos
{
    public class ResetPasswordDto
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmedPassword { get; set; }
    }
}
