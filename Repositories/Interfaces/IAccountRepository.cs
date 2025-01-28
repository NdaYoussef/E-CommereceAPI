using TestToken.DTO;
using TestToken.DTO.UserDtos;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;

namespace TestToken.Repositories.Interfaces
{
    public interface IAccountRepository : IGenericRepository<ApplicationUser>
    {
        public Task<ResponseDto> LoginAsync(LoginDto login);
        public Task<ResponseDto> RegisterAsync(RegisterDto register);
        public Task<ResponseDto> UpdateProfile(userDto userDto); 
        public Task<ResponseDto> DeleteAccountAsync (LoginDto account);
        public Task<ResponseDto> ChangePasswordAsync(PasswordSettingsDto passwordDto);
        public Task<ResponseDto> ResetPasswordAsync(PasswordSettingsDto passDto);
        public Task<ResponseDto> GenerateRefreshTokenAsync(string email);
        public Task<bool> RevokeRefreshTokenAsync(string email);





    }
}
