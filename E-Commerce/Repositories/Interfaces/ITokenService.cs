using TestToken.Models;

namespace TestToken.Repositories.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
        RefreshToken GenerateRefreshToken();
    }
}
