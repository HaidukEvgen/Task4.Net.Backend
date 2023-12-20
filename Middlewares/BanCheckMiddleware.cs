using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Task4.Backend.Services;

namespace Task4.Backend.Middlewares
{
    public class BanCheckMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var user = context.User;

            if (user.Identity.IsAuthenticated)
            {
                var username = user.FindFirstValue(JwtRegisteredClaimNames.Name);

                if (!string.IsNullOrEmpty(username) && await userService.IsUserBanned(username))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("You are banned from accessing this resource.");
                    return;
                }
            }

            await next(context);
        }
    }
}
