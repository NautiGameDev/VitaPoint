using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Data;
using VitaPoint.Server.DTOs.Account;
using VitaPoint.Server.Entities;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Account> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<Account> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountService(UserManager<Account> userManager, ITokenService tokenService, SignInManager<Account> signInManager, ApplicationDbContext context)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._signInManager = signInManager;
            this._context = context;
        }

        public async Task<AuthResult> Login(LoginDto dto)
        {
            //Retrieve account via email
            var account = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (account == null)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = "Account doesn't exist with that email address",
                    ErrorType = AuthErrorType.NotFound
                };
            }

            //Ensure password matches
            var result = await _signInManager.CheckPasswordSignInAsync(account, dto.Password, false);

            if (!result.Succeeded)
            {
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = "Incorrect credentials",
                    ErrorType = AuthErrorType.InvalidCredentials
                };
            }

            //Generate auth token if credentials are correct
            string token = await _tokenService.CreateToken(account);
            string refreshToken = _tokenService.CreateRefreshToken();

            account.RefreshToken = refreshToken;
            account.RefreshExpiration = DateTime.UtcNow.AddHours(24);

            var updatedResult = await _userManager.UpdateAsync(account);

            if (!updatedResult.Succeeded)
            {
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = "Failed to store session details",
                    ErrorType = AuthErrorType.ProcessFailed
                };
            }

            return new AuthResult()
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                ErrorType = AuthErrorType.None
            };
        }

        public async Task<(AuthResult result, string? userId)> Register(RegisterDto dto)
        {
            var account = new Account
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var createdAccount = await _userManager.CreateAsync(account, dto.Password);

            if (!createdAccount.Succeeded)
            {
                return (new AuthResult()
                {
                    Success = false,
                    ErrorMessage = string.Join(", ", createdAccount.Errors.Select(e => e.Description)),
                    ErrorType = AuthErrorType.ProcessFailed
                }, null);
            }

            await _userManager.AddToRoleAsync(account, "User");

            string token = await _tokenService.CreateToken(account);
            string refreshToken = _tokenService.CreateRefreshToken();

            account.RefreshToken = refreshToken;
            account.RefreshExpiration = DateTime.UtcNow.AddHours(24);

            var updatedResult = await _userManager.UpdateAsync(account);

            if (!updatedResult.Succeeded)
            {
                return (new AuthResult()
                {
                    Success = false,
                    ErrorMessage = "Failed to store session details",
                    ErrorType = AuthErrorType.ProcessFailed
                }, null);
            }

            return (new AuthResult()
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                ErrorType = AuthErrorType.None
            }, account.Id);
        }


        //Used for account updating -> Called from PatientController
        //Validate user password first. If a new password was entered in client, update password
        public async Task<AuthResult> ValidatePassword(string userId, string password)
        {
            Account? account = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (account == null)
            {
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = "Account could not be found with that UserId",
                    ErrorType = AuthErrorType.NotFound
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(account, password, false);

            if (!result.Succeeded)
            {
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = "Incorrect Password",
                    ErrorType = AuthErrorType.InvalidCredentials
                };
            }

            return new AuthResult()
            {
                Success = true
            };
        }

        public async Task<AuthResult> UpdatePassword(string userId, string currentPassword, string newPassword)
        {
            Account? account = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (account == null)
            {
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = "Account could not be found with that UserId",
                    ErrorType = AuthErrorType.NotFound
                };
            }
            
            var passwordResult = await _userManager.ChangePasswordAsync(account, currentPassword, newPassword);

            if (!passwordResult.Succeeded)
            {
                var error = passwordResult.Errors.FirstOrDefault()?.Description ?? "Password update failed.";
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = error,
                    ErrorType = AuthErrorType.ProcessFailed
                };
            }
            
            return new AuthResult()
            {
                Success = true,
                ErrorType = AuthErrorType.None
            };
            
        }


        /*
            Uses refreshToken from HTTPOnly cookie and fetches user account with matching refresh token.
            Tests if refresh token stored in the Account table is expired
            If Account exists and refresh token is still valid, a new auth token and refresh token are created
         */
        public async Task<AuthResult> RefreshToken(string refreshToken)
        {
            var account = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (account == null || account.RefreshExpiration <= DateTime.UtcNow)
            {
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = "Invalid or expired refresh token.",
                    ErrorType = AuthErrorType.InvalidCredentials
                };
            }

            string newToken = await _tokenService.CreateToken(account);
            string newRefreshToken = _tokenService.CreateRefreshToken();

            account.RefreshToken = newRefreshToken;
            account.RefreshExpiration = DateTime.UtcNow.AddHours(24);

            var updateResult = await _userManager.UpdateAsync(account);
            if (!updateResult.Succeeded)
            {
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = "Failed to update session.",
                    ErrorType = AuthErrorType.ProcessFailed
                };
            }

            return new AuthResult()
            {
                Success = true,
                Token = newToken,
                RefreshToken = newRefreshToken,
                ErrorType = AuthErrorType.None
            };
        }
    }
}
