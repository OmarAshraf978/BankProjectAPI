using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bank.ServiceAbstraction.Services_Abstraction;
using Bank.Shared.DTO.IdentityDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var Result = await _authenticationService.RegisterAsync(registerDto);
            return HandleResult(Result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var Result = await _authenticationService.LoginAsync(loginDto);
            return HandleResult(Result);
        }

        [HttpGet("CheckEmail")]
        public async Task<ActionResult<UserExistsDto>> CheckEmail(string email)
        {
            var Result = await _authenticationService.UserExistsAsync(email);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet("GetUserByEmail")]
        public async Task<ActionResult<UserDto>> GetUserByEmail()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email)!;
            var Result = await _authenticationService.GetUserByEmailAsync(Email);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<ActionResult<LogOutDto>> LogOut(string email)
        {
            var Result = await _authenticationService.LogOutAsync(email);
            return HandleResult(Result);
        }
    }
}
