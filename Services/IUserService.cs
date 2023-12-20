using Task4.Backend.Infrastructure.Enums;
using Task4.Backend.Models.Dto;

namespace Task4.Backend.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetAllAsync();

        public Task RegisterAsync(RegisterUserDto registerUserDto);

        public Task<string> LoginAsync(LoginUserDto loginUserDto);

        public Task SetStatusesAsync(List<string> userIds, Status status);

        public Task DeleteUsersAsync(List<string> userIds);

        public Task<bool> IsUserBanned(string username);

    }
}
