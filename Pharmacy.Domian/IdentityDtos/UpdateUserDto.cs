using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.IdentityDtos
{
    public class UpdateUserDto
    {
        public IFormFile? ProfileImage { get; set; }
        public string? Token { get; set; }

        public string Username { get; set; }

        [StringLength(50)]
        public string Number { get; set; }



        [StringLength(128)]
        public string Email { get; set; }

        [StringLength(256)]
        public string Password { get; set; }

    }
}
