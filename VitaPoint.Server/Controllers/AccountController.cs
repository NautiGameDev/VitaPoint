using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VitaPoint.Server.DTOs.Account;
using VitaPoint.Server.Entities;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Controllers
{    
    public class AccountController : VPBaseController
    {
        private readonly IAccountService _accountService;
        private readonly IPatientService _patientService;
        private readonly SignInManager<Account> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<Account> signInManager, IPatientService patientService)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _patientService = patientService;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            AuthResult result = await _accountService.Login(loginDto);

            if (!result.Success)
            {
                switch (result.ErrorType)
                {
                    case AuthErrorType.NotFound:
                        return NotFound(new { message = result.ErrorMessage });
                    case AuthErrorType.InvalidCredentials:
                        return Unauthorized(new { message = result.ErrorMessage });
                    default:
                        return BadRequest(new { message = result.ErrorMessage });
                }
            }

            SetCookie(result.Token, result.RefreshToken);

            return Ok(new { message = "Login successful" });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {

            //Verify patient exists in the system before proceeding with account creation
            Patient? patient = await _patientService.VerifyPatient(registerDto.Email, registerDto.ActivationCode, registerDto.DOB, registerDto.Zip);

            if (patient == null)
            {
                return Unauthorized(new { message = "Patient cannot be found with those credentials" });
            }

            var (result, userId) = await _accountService.Register(registerDto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            //Update the patient account to reflect account activation
            Patient? updatedPatient = await _patientService.ActivatePatient(patient, userId);
            

            SetCookie(result.Token, result.RefreshToken);       

            return Ok(new { message = "Registration successful." });
        }
                

        /*
            Called when patient dashboard is loaded up.
            If this returns false, the user will be redirected to system login screen.
            Ensures that if the site is accessed via URL extensions leading to a Dashboard page,
                the client app will handle the error gracefully if user isn't logged in
        */
        [HttpGet("check-logged")]
        [Authorize]
        public async Task<IActionResult> CheckLoggedIn()
        {
            return Ok(new { isLogged = true });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await ClearAuthCookie();

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> GetNewToken()
        {
            var refreshToken = Request.Cookies["VPRefresh"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { message = "Refresh token missing." });
            }

            AuthResult result = await _accountService.RefreshToken(refreshToken);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.ErrorMessage });
            }

            SetCookie(result.Token, result.RefreshToken);

            return Ok(new { message = "Token refreshed successfully " });
        }       


        /*
            Cookie options are temporary. This will need to be updated in the future to set-up refresh tokens
        */
        private void SetCookie(string token, string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(24)
            };

            Response.Cookies.Append("VPAuth", token, cookieOptions);
            Response.Cookies.Append("VPRefresh", refreshToken, refreshCookieOptions);
        }

        private async Task ClearAuthCookie()
        {
            await _signInManager.SignOutAsync();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            };

            Response.Cookies.Append("VPAuth", "", cookieOptions);
        }
    }
}
