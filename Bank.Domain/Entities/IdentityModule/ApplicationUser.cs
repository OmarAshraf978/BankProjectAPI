using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bank.Domain.Entities.IdentityModule
{
    #region IdentityModule
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public string NationalId { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastLoginAt { get; set; }
        public string? RefreshToken { get; set; } 
        public Address? Address { get; set; } 
    }
    public class Address
    {
        public int Id { get; set; }
        public string BuildingNumber { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
    #endregion
}
