using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Elsa.Activities.UserTask.Extensions;
using Elsa.Providers.Workflows;
using Storage.Net;
using Serilog.Core;
using Serilog.Events;
using Serilog;
using Serilog.Exceptions;

namespace ElsaBookmarkTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var elsaDbConn = builder.Configuration.GetConnectionString("ElsaConnection") ?? throw new NotSupportedException("ElsaConnection is empty");

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Elsa services.
            builder.Services
                .AddElsa(elsa => elsa
                    .UseEntityFrameworkPersistence(o => o.UseSqlServer(elsaDbConn), true)
                    .AddUserTaskActivities()
                )
                .AddElsaSwagger()
                .AddElsaApiEndpoints();

            // Configure Storage for BlobStorageWorkflowProvider with a directory on disk from where to load workflow definition JSON files from the local "Workflows" folder.
            builder.Services.Configure<BlobStorageWorkflowProviderOptions>(options => options.BlobStorageFactory = () => StorageFactory.Blobs.DirectoryFiles(Path.Combine(builder.Environment.ContentRootPath, "Workflows")));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMvc();

            builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .WithExposedHeaders("Content-Disposition"))
            );

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

            app.UseCors()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
                    endpoints.MapControllers();
                    endpoints.MapSwagger();
                });
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}