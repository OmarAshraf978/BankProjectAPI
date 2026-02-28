using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared.DTO.IdentityDto
{
    public record RegisterDto
    (
        [EmailAddress]
        string Email,
        string Password,
        string FullName,
        [Phone]
        string PhoneNumber,
        string UserName,
        AddressDto Address,
        string NationalId
    );
}
