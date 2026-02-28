using System;
using System.Collections.Generic;
using System.Linq;
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
    #endregion
}
