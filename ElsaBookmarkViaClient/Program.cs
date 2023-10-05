using Elsa.Client.Extended.Extensions;
using Elsa.Client.Options;

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