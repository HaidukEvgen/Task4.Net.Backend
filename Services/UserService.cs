using Task4.Backend.Exceptions;
using Task4.Backend.Infrastructure.Enums;
using Task4.Backend.Models.Data;
using Task4.Backend.Models.Dto;
using Task4.Backend.Repositories;

namespace Task4.Backend.Services
{
    public class UserService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator) : IUserService
    {
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.UserName,
                Email = u.Email,
                LastLogin = u.LastLogin,
                Status = u.Status
            });
        }

        public async Task RegisterAsync(RegisterUserDto registerUserDto)
        {
            User user = new()
            {
                UserName = registerUserDto.Name,
                Email = registerUserDto.Email,
                LastLogin = DateTime.Now
            };

            var result = await userRepository.RegisterAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                throw new RegisterException(result.Errors.FirstOrDefault().Description);
            }
        }

        public async Task<string> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await userRepository.GetByEmailAsync(loginUserDto.Email);

            var isCorrect = await userRepository.IsPasswordCorrectAsync(user, loginUserDto.Password);

            if (user == null || isCorrect == false)
            {
                throw new LoginException("Username or password is incorrect");
            }

            if (await IsUserBanned(user.UserName))
            {
                throw new UserBannedException("You are banned from accessing this resource.");
            }

            user.LastLogin = DateTime.Now;

            await userRepository.SaveChangesAsync();

            var token = jwtTokenGenerator.GenerateToken(user);

            return token;
        }

        public async Task SetStatusesAsync(List<string> userIds, Status status)
        {
            await userRepository.SetStatusesAsync(userIds, status);
        }

        public async Task DeleteUsersAsync(List<string> userIds)
        {
            await userRepository.DeleteUsersAsync(userIds);
        }

        public async Task<bool> IsUserBanned(string username)
        {
            var user = await userRepository.GetByUsernameAsync(username);
            if (user is null)
            {
                throw new UserDeletedException("Your account had been deleted. Cannot access the resource");
            }
            return user.Status == Status.Blocked;

        }
    }
}
