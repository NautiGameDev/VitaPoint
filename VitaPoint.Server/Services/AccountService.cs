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

        public async Task<AuthResult> UpdateCredentials(string userId, UpdateUserDto dto)
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

            //Verify credentials before making changes
            var result = await _signInManager.CheckPasswordSignInAsync(account, dto.CurrentPassword, false);

            if (!result.Succeeded)
            {
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = "Current password is incorrect",
                    ErrorType = AuthErrorType.InvalidCredentials
                };
            }

            //Update credentials using a transaction in case any errors occur during process.
            //Account update process is "all or nothing" to prevent system errors
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (!string.IsNullOrWhiteSpace(dto.NewPassword))
                {
                    var passwordResult = await _userManager.ChangePasswordAsync(account, dto.CurrentPassword, dto.NewPassword);

                    if (!passwordResult.Succeeded)
                    {
                        var firstError = passwordResult.Errors.FirstOrDefault()?.Description ?? "Password update failed.";
                        throw new Exception(firstError);
                    }

                }

                if (account.Email != dto.Email)
                {

                    var emailResult = await _userManager.SetEmailAsync(account, dto.Email);
                    var userNameResult = await _userManager.SetUserNameAsync(account, dto.Email);

                    if (!emailResult.Succeeded || !userNameResult.Succeeded)
                    {
                        var error = emailResult.Errors.Concat(userNameResult.Errors).FirstOrDefault()?.Description;
                        throw new Exception(error ?? "Email/Username update error.");
                    }
                }

                await transaction.CommitAsync();

                return new AuthResult()
                {
                    Success = true,
                    ErrorType = AuthErrorType.None
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new AuthResult()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ErrorType = AuthErrorType.ProcessFailed
                };
            }
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
