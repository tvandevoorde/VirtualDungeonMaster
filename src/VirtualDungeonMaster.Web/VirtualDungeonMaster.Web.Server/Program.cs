using Scalar.AspNetCore;
using VirtualDungeonMaster.Application.Extensions;

namespace VirtualDungeonMaster.Web.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();

            builder.Services.AddVirtualDungeonMasterApp(builder.Configuration);

            WebApplication app = builder.Build();

            app.UseDefaultFiles();
            app.MapStaticAssets();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(opt =>
                {
                    opt.Title = "Virtual Dungeon Master";
                    opt.Theme = ScalarTheme.BluePlanet;
                    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http1);
                });
            }

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
