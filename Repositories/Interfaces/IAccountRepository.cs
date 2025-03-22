using TestToken.DTO;
using TestToken.DTO.PasswordSettingsDto;
using TestToken.DTO.UserDtos;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IAccountRepository : IGenericRepository<ApplicationUser>
    {
         Task<ResponseDto> LoginAsync(LoginDto login);
        Task<ResponseDto> LogoutAsync(LoginDto logout);
         Task<ResponseDto> RegisterAsync(RegisterDto register);
        Task<ResponseDto> UpdateProfile(userDto userDto); 
        Task<ResponseDto> DeleteAccountAsync (LoginDto account);
        Task<ResponseDto> ChangePasswordAsync(ChangePasswordDto passwordDto);
        Task<ResponseDto> ForgotPasswordAsync(string email);
        Task<ResponseDto> ResetPasswordAsync(ResetPasswordDto passDto);
        Task<ResponseDto> GenerateRefreshTokenAsync(string email);
        Task<bool> RevokeRefreshTokenAsync(string email);
        Task<ResponseDto> SendOtpAsync(string email);
        Task<ResponseDto> VerifyOtpRequest(VerifyOtpRequest verifyOtp);





    }
}
