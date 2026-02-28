using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared.DTO.IdentityDto
{
    public record LogOutDto
    (
        string FullName,
        string Email,
        string Description
    );
}
