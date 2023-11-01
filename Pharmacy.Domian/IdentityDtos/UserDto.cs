using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domian.IdentityDtos
{
    public class UserDto
    {
        public string ProfileImage { get; set; }
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(128)]
        [EmailAddress]
        public string Email { get; set; }
        public string Number { get; set; }

        public string Location { get; set; }

    }
}
