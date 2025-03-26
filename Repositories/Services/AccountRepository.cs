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
using TestToken.Helpers;
using System.Net;
using TestToken.DTO.PasswordSettingsDto;
using Microsoft.EntityFrameworkCore;

namespace TestToken.Repositories.Services
{
    public class AccountRepository : GenericRepository<ApplicationUser>, IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly EmailTemplateService _emailTemplateService;
        private readonly ILogger<AccountRepository> _logger;
        public AccountRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ITokenService tokenService, IMapper mapper,
            IEmailService emailService,EmailTemplateService emailTemplateService,RoleManager<IdentityRole> roleManager, ILogger<AccountRepository> logger) : base(context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleManager = roleManager;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _logger = logger;
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
            if (user.RefreshTokens!.Any(t => t.IsActive))
            {
                var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                refreshToken = activeToken.Token;
                refreshTokenExpiration = activeToken.ExpiresOn;
            }
            else
            {
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                refreshToken = newRefreshToken.Token;
                refreshTokenExpiration = newRefreshToken.ExpiresOn;
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user); //save to db
            }
            return new ResponseDto
            {
                Message = "User login successfully ",
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
            if (await _userManager.FindByEmailAsync(register.Email) is not null 
                || await _userManager.FindByNameAsync(register.UserName) is not null)
                return new ResponseDto { Message = "Email or UserName already exits!!" };
            var otp = GenerateOTP.GenerateeOTP();
            var otpExpiry = DateTime.UtcNow.AddDays(1);
            var user =_mapper.Map<ApplicationUser>(register);
            user.OTP = otp;
            user.OTPExpiry = otpExpiry;
            user.IsConfirmed = false;
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
            //  var token = _tokenService.GenerateToken(user);
            await _emailService.sendEmailAsync(user.Email!, "OTP Email verfication", $"Hi {register.UserName} , " +
                $"use this code below to verify your account {otp}");
            return new ResponseDto
            {
                Message = "OTP sent to your email. Please verify to complete registration",
                StatusCode = 200,
                IsSucceeded = true,
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
        public async Task<ResponseDto> ChangePasswordAsync(ChangePasswordDto passwordDto)
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
        public async Task<ResponseDto> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string subject = "Password Reset Request";
            string message = $"Please use the following token to reset your password: {token}";
             await _emailService.sendEmailAsync(email, subject, message);
            return new ResponseDto
            {
                Message = "Password reset link sent successfully!",
                IsSucceeded = true,
                StatusCode = 200
            };
        }
        public async Task<ResponseDto> ResetPasswordAsync(ResetPasswordDto passDto)
        {
            var user = await _userManager.FindByEmailAsync(passDto.Email);
            if (user == null)
                return new ResponseDto
                {
                    Message = "User not found!!",
                    IsSucceeded = false,
                    StatusCode = 400
                };

            
            var result = await _userManager.ResetPasswordAsync(user, passDto.Token, passDto.NewPassword);
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
            if (user == null)
            {
                return new ResponseDto
                {
                    Message = "Invalid Email!!",
                    IsSucceeded = false,
                    StatusCode = 400,
                };
            }
            var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            if (activeToken is not null)
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
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            //    return new ResponseDto
            //    {
            //        IsSucceeded = true,
            //        StatusCode = 200,
            //        model = new
            //        {
            //            IsAuthenticated = true,
            //            Token = token,
            //            RefreshToken = refreshToken,
            //            UserName = user.UserName,
            //            Email = user.Email,
            //        }
            //    };
            //  var user = await _userManager.Users
            //                               .Include(u => u.RefreshTokens)
            //                               .FirstOrDefaultAsync(u => u.Email == email);
            //  if (user == null)
            //  {
            //      _logger.LogWarning($"Refresh token generation attempted for non-existent email: {email}");
            //      return new ResponseDto
            //      {
            //          Message = "Invalid Email!!",
            //          IsSucceeded = false,
            //          StatusCode = 400,
            //      };
            //  }
            ////  var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            //  if (user.RefreshTokens?.Any(t=>t.IsActive) == true)
            //  {
            //      _logger.LogInformation($"Active token already exists for user: {email}");
            //      return new ResponseDto
            //      {
            //          Message = "Token still active",
            //          IsSucceeded = false,
            //          StatusCode = 400,
            //      };
            //  }
            //  var token = _tokenService.GenerateToken(user);
            //  var refreshToken = _tokenService.GenerateRefreshToken();
            //  user.RefreshTokens.Add(refreshToken);
            // var updatedResult =  await _userManager.UpdateAsync(user);
            //  if (!updatedResult.Succeeded)
            //  {
            //      _logger.LogError($"Failed to update user with new refresh token: {email}");
            //      return new ResponseDto
            //      {
            //          Message = "Token generation failed",
            //          IsSucceeded = false,
            //          StatusCode = 500
            //      };
            //  }
            //    await _context.SaveChangesAsync();
            return new ResponseDto
            {
                Message = "Token generated successfully", 
                IsSucceeded = true,
                StatusCode = 200,
                model = new
                {
                    IsAuthenticated = true,
                    Token = token,
                    RefreshToken = refreshToken.Token, 
                    UserName = user.UserName,
                    Email = user.Email,
                }
            };
        }
        
        public async Task<bool> RevokeRefreshTokenAsync(string email)
        {
            var user = await _userManager.Users
            .Include(u => u.RefreshTokens) 
            .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return false;
           
            var activeToken = user.RefreshTokens?.FirstOrDefault(t=>t.IsActive);

            if(activeToken is null) return false;

            activeToken.RevokedOn = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return false;
       //     await _context.SaveChangesAsync();
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
            if(user.RefreshTokens?.Any()==true)
                user.RefreshTokens.Clear();
            return new ResponseDto
            {
                Message = "User Logged out successfully1",
                IsSucceeded = true,
                StatusCode = 200
            };
        }
       public  async Task<ResponseDto> SendOtpAsync(string email)
        {
            var user = await _userManager.FindByIdAsync(email);
            if(user is null)
                return new ResponseDto
                {
                    Message = "User not found!",
                    IsSucceeded = false,
                    StatusCode = 404
                };
            var otp = GenerateOTP.GenerateeOTP();
            var otpExpiry = DateTime.UtcNow.AddMinutes(15);
            user.OTP = otp;
            user.OTPExpiry = otpExpiry;
            await _userManager.UpdateAsync(user);
            return new ResponseDto
            {
                Message = "New OTP sent!",
                IsSucceeded = true,
                StatusCode = 200
            };

        }
       public async Task<ResponseDto> VerifyOtpRequest(VerifyOtpRequest verifyOtp)
        {
            var user = await _userManager.FindByEmailAsync(verifyOtp.Email);
            if (user is null)
            return new ResponseDto
            {
                Message = "User not found!",
                IsSucceeded = false,
                StatusCode = 404
            };

            if(user.IsConfirmed)
                return new ResponseDto
                {
                    Message = "User already verified!",
                    IsSucceeded = false
                };
            if (user.OTP != verifyOtp.OTP || user.OTPExpiry < DateTime.UtcNow)
                return new ResponseDto
                {
                    Message = "Invalid or expired OTP!",
                    IsSucceeded = false
                };
            user.IsConfirmed = true;
            user.OTP = null;
            user.OTPExpiry = null;
            await _userManager.UpdateAsync(user);
            //var emailBodey = _emailTemplateService.RenderWelcomeEmail(user.UserName!, user.Email!, "Customer");
            //await _emailService.sendEmailAsync(
            //    user.Email!,
            //    emailBodey,
            //    "Welcome to E-Commerce account!"
            //    );
            return new ResponseDto
            {
                Message = "User verified successfully!",
                IsSucceeded = true,
                IsConfirmed = true,
                StatusCode = 200
            };

        }
    }

}









