using Microsoft.OpenApi.Models;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Elsa.Activities.UserTask.Extensions;
using Elsa.Providers.Workflows;
using Storage.Net;

namespace ElsaBookmarkTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var elsaSection = builder.Configuration.GetSection("Elsa");
            var elsaDbConn = builder.Configuration.GetConnectionString("ElsaConnection") ?? throw new NotSupportedException("ElsaConnection is empty");

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Elsa services.
            builder.Services
                .AddElsa(elsa => elsa
                    .UseEntityFrameworkPersistence(o => o.UseSqlServer(elsaDbConn), true)
                    .AddUserTaskActivities()
                );

            // Configure Storage for BlobStorageWorkflowProvider with a directory on disk from where to load workflow definition JSON files from the local "Workflows" folder.
            builder.Services.Configure<BlobStorageWorkflowProviderOptions>(options => options.BlobStorageFactory = () => StorageFactory.Blobs.DirectoryFiles(Path.Combine(builder.Environment.ContentRootPath, "Workflows")));

            // Elsa API endpoints.
            builder.Services.AddElsaApiEndpoints();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMvc();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Elsa Workflow", Version = "v1" });
            });

            builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .WithExposedHeaders("Content-Disposition"))
            );

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