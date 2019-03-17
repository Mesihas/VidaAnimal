using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using VidaAnimal.Models;
using VidaAnimal.Services;

namespace VidaAnimal
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                //  builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // add Identity services
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = false;
                config.Password.RequireDigit = false;
                config.Password.RequiredLength = 4;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<MyDbContext>();

            services.AddDbContext<MyDbContext>();
            services.AddTransient<ISupplierProvider>(f => new SupplierProvider(@"Data Source = DESKTOP-ALE; Initial Catalog = VidaAnimal; Integrated Security=True; MultipleActiveResultSets=true"));
            services.AddTransient<ISalesDataService>(f => new SalesDataService(@"Data Source = DESKTOP-ALE; Initial Catalog = VidaAnimal; Integrated Security=True; MultipleActiveResultSets=true"));

      
      // add My services      
            services.AddScoped<IDataService<Product>, DataService<Product>>();
            services.AddScoped<IDataService<Purchasing>, DataService<Purchasing>>();
            services.AddScoped<IDataService<Sales>, DataService<Sales>>();
            services.AddScoped<IDataService<Stock>, DataService<Stock>>();
            services.AddScoped<IDataService<Supplier>, DataService<Supplier>>();
            services.AddScoped<IDataService<Profile>, DataService<Profile>>();
            services.AddScoped<IDataService<Category>, DataService<Category>>();
            services.AddScoped<IDataService<Client>, DataService<Client>>();
            services.AddScoped<IDataService<SalesDetail>, DataService<SalesDetail>>();

            // 2- add session with options
            services.AddSession();

            //services.AddAuthentication(sharedOptions =>
            //     {
            //         sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //         sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    // sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //});
            // Register the IConfiguration instance which MyOptions binds against.
            services.AddOptions();

            // Load the data from the 'root' of the json file
            services.Configure<MyOptions>(Configuration);

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler(" / Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                           template: "{controller=Home}/{action=Index}/{id?}");
                   // template: "{controller=Sales}/{action=Index}/{id?}");
            });
        }
    }
}
