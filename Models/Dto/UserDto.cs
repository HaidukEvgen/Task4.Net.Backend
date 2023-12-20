using Task4.Backend.Infrastructure.Enums;

namespace Task4.Backend.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime LastLogin { get; set; }

        public Status Status { get; set; }
    }
}
