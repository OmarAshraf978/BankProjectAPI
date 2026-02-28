using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared.DTO.IdentityDto
{
    public record UserDto
    (
        string Email,
        string DisplayName,
        string Token,
        string RefreshToken
    );
}
