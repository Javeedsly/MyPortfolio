using MyPortfolio.Core.Entities;

namespace MyPortfolio.Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}