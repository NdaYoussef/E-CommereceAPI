using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TestToken.Data;
using TestToken.DTO;
using TestToken.DTO.UserDtos;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace TestToken.Repositories.Services
{
    public class AccountRepository : GenericRepository<ApplicationUser>, IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ITokenService tokenService, IMapper mapper, RoleManager<IdentityRole> roleManager) : base(context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public async Task<ResponseDto> LoginAsync(LoginDto login)
        {
          var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user,login.Password))
            {
                return new ResponseDto
                {
                    Message = "User not found!!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            var token = _tokenService.GenerateToken(user);
            var refreshToken = string.Empty;
            DateTime refreshTokenExpiration;
            if (user.refreshTokens!.Any(t => t.IsActive))
            {
                var activeToken = user.refreshTokens.FirstOrDefault(t => t.IsActive);
                refreshToken = activeToken.Token;
                refreshTokenExpiration = activeToken.ExpiresOn;
            }
            else
            {
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                refreshToken = newRefreshToken.Token;
                refreshTokenExpiration = newRefreshToken.ExpiresOn;
            }
            return new ResponseDto
            {
                Message = " Login successfully ",
                IsSucceeded = true,
                StatusCode = 200,
                model = new
                {
                    IsAuthenticated = true,
                    token = token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiration = refreshTokenExpiration,
                    UserName = user.UserName,
                    Email = user.Email
                }
            };
        }
        public async Task<ResponseDto> RegisterAsync(RegisterDto register)
        {
            if (await _userManager.FindByEmailAsync(register.Email) is not null)
                return new ResponseDto { Message = "Email already exits!!" };
            if (await _userManager.FindByNameAsync(register.Username) is not null)
                return new ResponseDto { Message = "UserName already exists!!" };
            var user =_mapper.Map<ApplicationUser>(register);
            var result = await _userManager.CreateAsync(user,register.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description},";
                return new ResponseDto
                {
                    Message = errors,
                    StatusCode = 400,
                    IsSucceeded = false
                };
            }
            await _userManager.AddToRoleAsync(user, "Customer");
            var token = _tokenService.GenerateToken(user);
            return new ResponseDto
            {
                Message = "User registerd successfully, Thank you for your trust!",
                StatusCode = 200,
                IsSucceeded = true,
                model = new
                {
                    token = token,
                    IsAuthenticated = true,
                    UserName = user.UserName,
                }
            };
        }
        public async Task<ResponseDto> UpdateProfile(userDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
                return new ResponseDto
                {
                    Message = "User not found!!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            _mapper.Map(userDto, user);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {

                    Message = "Failed to update user profile, try agin",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            return new ResponseDto
            {
                Message = "User profile updated successfully.",
                IsSucceeded = true,
                StatusCode = 200
            };
            }
        public async Task<ResponseDto> ChangePasswordAsync(PasswordSettingsDto passwordDto)
        {
            var user = await _userManager.FindByEmailAsync(passwordDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, passwordDto.CurrentPassword))
            {
                return new ResponseDto
                {
                    Message = "User not found!!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {

                    Message = "Failed to change password , try agin",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            return new ResponseDto
            {
                Message = "Password has changed successfully",
                IsSucceeded = true,
                StatusCode = 200
            };
        }
        public async Task<ResponseDto> ResetPasswordAsync(PasswordSettingsDto passDto)
        {
            var user = await _userManager.FindByEmailAsync(passDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, passDto.CurrentPassword))
            {
                return new ResponseDto
                {
                    Message = "User not found!!",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, passDto.NewPassword);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {

                    Message = "Failed to reset password , try agin",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            return new ResponseDto
            {
                Message = "Passwored reset successfully ",
                IsSucceeded = true,
                StatusCode = 200
            };
        }
        public async Task<ResponseDto> DeleteAccountAsync(LoginDto account)
        {
            var user = await _userManager.FindByEmailAsync(account.Email);
            if (user == null)
            {
                return new ResponseDto
                {
                    Message = "User not found !!",
                    IsSucceeded = true,
                    StatusCode = 200
                };
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    Message = "Failed to delete account!! , try again ",
                    IsSucceeded = false,
                    StatusCode = 400
                };
            }
            return new ResponseDto
            {
                Message = "Account deleted successfully ,Your Data Will Be Safely Removed",
                IsSucceeded = true,
                StatusCode = 200
            };
        }
        public async Task<ResponseDto> GenerateRefreshTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid Email!!",
                    IsSucceeded = false,
                    StatusCode = 400,
                };
            }   
            var activeToken = user.refreshTokens.FirstOrDefault(t=>t.IsActive);
            if(activeToken is not null)
            {
                return new ResponseDto
                {
                    Message = "Token still active",
                    IsSucceeded = false,
                    StatusCode = 400,
                };
            }
            var token = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.refreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            return new ResponseDto
            {
                IsSucceeded = true,
                StatusCode = 200,
                model = new
                {
                    IsAuthenticated = true,
                    Token = token,
                    RefreshToken = refreshToken,
                    UserName = user.UserName,
                    Email = user.Email,
                }
            };

        }
        public async Task<bool> RevokeRefreshTokenAsync(string email)
        {
           var user = await _userManager.FindByEmailAsync(email);
            if(user == null) 
                return false;
            
            var activeToken = user.refreshTokens.FirstOrDefault(t=>t.IsActive);
            if(activeToken is null)
                return false;
            activeToken.RevokedOn = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return false;
            return true;
        }

        public async Task<ResponseDto> LogoutAsync(LoginDto logout)
        {
           var user = await _userManager.FindByEmailAsync(logout.Email);
            if(user is null)
            {
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            if(user.refreshTokens?.Any()==true)
                user.refreshTokens.Clear();
            return new ResponseDto
            {
                Message = "User Logged out successfully1",
                IsSucceeded = true,
                StatusCode = 200
            };
        }
    }

}









