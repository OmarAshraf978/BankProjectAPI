using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bank.Shared.DTO.IdentityDto;
using Bank.Shared.ResultPattern;

namespace Bank.ServiceAbstraction.Services_Abstraction
{
    #region AuthenticationService
    public interface IAuthenticationService
    {
        public Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto);
        public Task<Result<UserDto>> LoginAsync(LoginDto loginDto);
        public Task<Result<UserExistsDto>> UserExistsAsync(string email);
        public Task<Result<UserDto>> GetUserByEmailAsync(string email);
        public Task<Result<LogOutDto>> LogOutAsync(string email);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Result<UserDto>> GetUserByEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User is null)
                return Error.NotFound("User.NotFound", $"User With Email: {email} Is Not Found");
            if (string.IsNullOrEmpty(User.RefreshToken))
                return Error.Forbidden("User.Forbidden", $"Account With Email: {email} was Logged Out");
            return new UserDto
                (User.Email!,
                User.FullName,
                await GenerateTokenAsync(User),
                User.RefreshToken);
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User is null)
                return Error.InvalidCredentials("User.InvalidCredentials");
            var PasswordValid = await _userManager.CheckPasswordAsync(User, loginDto.Password);
            if (!PasswordValid)
                return Error.InvalidCredentials("User.InvalidCredentials");
            var RefreshToken = await GenerateRefreshTokenAsync();
            User.RefreshToken = RefreshToken;
            User.LastLoginAt = DateTimeOffset.UtcNow;
            await _userManager.UpdateAsync(User);
            return new UserDto
                (User.Email!,
                User.FullName,
                await GenerateTokenAsync(User),
                RefreshToken);
        }

        public async Task<Result<LogOutDto>> LogOutAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User is null)
                return Error.NotFound("User.NotFound", $"User With Email: {email} Is Not Found");
            User.RefreshToken = null;
            await _userManager.UpdateAsync(User);
            return new LogOutDto(User.FullName, email, "This Account Logged Out Successfully");
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            var User = new ApplicationUser()
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName,
                Address = new Address()
                {
                    FirstName = registerDto.Address.FirstName,
                    LastName = registerDto.Address.LastName,
                    BuildingNumber = registerDto.Address.BuildingNumber,
                    Street = registerDto.Address.Street,
                    City = registerDto.Address.City,
                    Country = registerDto.Address.Country
                },
                NationalId = registerDto.NationalId,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                LastLoginAt = DateTimeOffset.UtcNow
            };
            var IdentityResult = await _userManager.CreateAsync(User, registerDto.Password);
            if (!IdentityResult.Succeeded)
                return Result<UserDto>.Fail(IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList());
            var refreshToken = await GenerateRefreshTokenAsync();
            User.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(User);
            return new UserDto(User.Email, User.FullName, await GenerateTokenAsync(User), refreshToken);
        }

        public async Task<Result<UserExistsDto>> UserExistsAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User is not null && !string.IsNullOrEmpty(User.RefreshToken))
                return new UserExistsDto(User.FullName, email, "This Account Is In The System");
            else if (User is not null && string.IsNullOrEmpty(User.RefreshToken))
                return new UserExistsDto(User.FullName, email, "This Account Is Logged out");
            return Error.NotFound("User.NotFound", $"User With Email: {email} Is Not Found");
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName!)
            };
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var SecretKey = _configuration["JwtOptions:SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));
            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: Claims,
                signingCredentials: Creds
            );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        private Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var token = Convert.ToBase64String(randomNumber);
            return Task.FromResult(token);
        }
    }
    #endregion
}
