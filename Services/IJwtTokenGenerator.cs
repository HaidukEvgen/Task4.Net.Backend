using Task4.Backend.Models.Data;

namespace Task4.Backend.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
