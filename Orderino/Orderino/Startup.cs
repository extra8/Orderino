using Orderino.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orderino.Repository;
using Orderino.Repository.Models;
using Orderino.Utils;
using MudBlazor.Services;

namespace Orderino
{
    public class Startup
    {
        private const string CosmosDbConfigurationSection = "CosmosDbConfig";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }        

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
            services.AddTransient<IBot, OrderinoBot>();

            var cosmosDbConfigurationSection = Configuration.GetSection(CosmosDbConfigurationSection);
            services.AddSingleton(new Repository<User>(cosmosDbConfigurationSection));
            services.AddSingleton(new Repository<Restaurant>(cosmosDbConfigurationSection));
            services.AddSingleton(new Repository<Order>(cosmosDbConfigurationSection));
            services.AddSingleton(new Repository<FinalizedOrder>(cosmosDbConfigurationSection));
            services.AddSingleton(new Repository<LoginInfo>(cosmosDbConfigurationSection));

            services.AddSingleton(new SessionCache());
            services.AddMudServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
