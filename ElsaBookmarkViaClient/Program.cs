using Elsa.Client.Extended.Extensions;
using Elsa.Client.Options;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;

namespace ElsaBookmarkViaClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var elsaServerSection = builder.Configuration.GetSection("Elsa").GetSection("Server");

            var serverurl = elsaServerSection.Get<ElsaClientOptions>();
            builder.Services.AddElsaExtendedClient(elsaServerSection.Bind);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            LoggingLevelSwitch lls = new(LogEventLevel.Debug);
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
              .Enrich.WithExceptionDetails()
              .Enrich.FromLogContext()
              .MinimumLevel.ControlledBy(lls)
              .WriteTo.Logger(l => l
                    .WriteTo.Async(c => c.File("Logs/logs.txt",
                        rollingInterval: RollingInterval.Day,
                        fileSizeLimitBytes: 100000000,
                        rollOnFileSizeLimit: true)))
              .CreateLogger();
            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}