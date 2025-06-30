using System;
using DataManagementTranslation;
using DataManagementTranslation.Infrastructure;
using DataManagementTranslation.Models;
using DataManagementTranslation.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataManagementTranslation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


            // Настройка конфигурации из файла appsettings.json
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configBuilder.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;

            
            builder.Services.AddDbContext<DataManagerDbContext>(x => x.UseSqlServer(config.Database.ConnectionString));

           
            builder.Services.AddControllersWithViews();

            
            builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("Project"));

            builder.Services.AddScoped<ClientRepository>();
           
            builder.Services.AddSession();
            WebApplication app = builder.Build();
            app.UseSession();
         
            app.UseStaticFiles();


            app.UseRouting();

            // Настраиваем маршруты по умолчанию
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataManagerDbContext>();
                db.Database.Migrate();
            }
            await app.RunAsync();
        }
    }
}
