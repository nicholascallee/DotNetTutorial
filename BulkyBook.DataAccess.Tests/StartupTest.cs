using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BulkyBook.DataAccess.Initializer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

namespace BulkyBook.DataAccess.Tests
{
    public class StartupTest
    {
        public IConfiguration Configuration { get; private set; }


        public StartupTest(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }



        public void ConfigureServices(IServiceCollection services)
        {


            //// Connection string retrieval
            //var connectionString = Configuration.GetConnectionString("DefaultConnection");

            //// DbContext registration
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            //// Identity registration, which also registers UserManager and RoleManager and their dependencies
            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>(); // Specifies the EF store for the user and roles

            //// If you encounter issues with logging, you can register the default logging services.
            //services.AddLogging();

            //// Register DBInitializer
            //services.AddScoped<IDBInitializer, DBInitializer>();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

        }


        public void Configure(IServiceProvider serviceProvider)
        {
            // Seed the database
            var initializer = serviceProvider.GetRequiredService<IDBInitializer>();
            initializer.Initialize();
        }
    }

}
