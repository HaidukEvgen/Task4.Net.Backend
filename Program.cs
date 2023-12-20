using Task4.Backend.Extensions;
using Task4.Backend.Middlewares;

namespace Task4.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            builder.Services
                .ConfigureMsSqlServer(builder.Configuration)
                .ConfigureBusinessServices()
                .ConfigureIdentity(builder.Configuration)
                .ConfigureJwtAuthentication(builder.Configuration)
                .ConfigureBusinessServices();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();

            var app = builder.Build();

            app.UseMiddleware<GlobalErrorHandler>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<BanCheckMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
