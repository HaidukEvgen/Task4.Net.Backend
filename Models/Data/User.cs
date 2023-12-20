using Microsoft.AspNetCore.Identity;
using Task4.Backend.Infrastructure.Enums;

namespace Task4.Backend.Models.Data
{
    public class User : IdentityUser
    {
        public DateTime LastLogin { get; set; }

        public Status Status { get; set; }
    }
}
