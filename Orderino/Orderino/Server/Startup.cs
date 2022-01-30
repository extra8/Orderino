using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orderino.Infrastructure;
using Orderino.Infrastructure.AzureBlob;
using Orderino.Infrastructure.AzureBlob.Interfaces;
using Orderino.Infrastructure.EntityServices;
using Orderino.Infrastructure.EntityServices.Interfaces;
using Orderino.Infrastructure.Services;
using Orderino.Infrastructure.Services.Interfaces;
using Orderino.Server.Bot;
using Orderino.Shared.Models;

namespace Orderino.Server
{
    public class Startup
    {
        private const string cosmosDbConfigurationSectionKey = "CosmosDbConfig";

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
            services.AddControllers();

            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
            services.AddTransient<IBot, OrderinoBot>();

            RegisterServices(services);
            RegisterRepositories(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IFinalizedOrderService, FinalizedOrderService>();
            services.AddScoped<ILoginInfoService, LoginInfoService>();
            services.AddScoped<IRestaurantStatisticService, RestaurantStatisticService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IFavoritesService, FavoritesService>();
            services.AddScoped<IOrderHistoryService, OrderHistoryService>();
            services.AddScoped<IRestaurantAdministrationService, RestaurantAdministrationService>();

            services.AddScoped<IAzureBlobUploadService, AzureBlobUploadService>();
            services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>();            
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            IConfigurationSection cosmosDbConfigurationSection = Configuration.GetSection(cosmosDbConfigurationSectionKey);
            services.AddSingleton(new Repository<User>(cosmosDbConfigurationSection));
            services.AddSingleton(new Repository<Restaurant>(cosmosDbConfigurationSection));
            services.AddSingleton(new Repository<Order>(cosmosDbConfigurationSection));
            services.AddSingleton(new Repository<FinalizedOrder>(cosmosDbConfigurationSection));
            services.AddSingleton(new Repository<LoginInfo>(cosmosDbConfigurationSection));
            services.AddSingleton(new Repository<RestaurantStatistic>(cosmosDbConfigurationSection));
        }
    }
}
