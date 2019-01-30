using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using IdentityServer4.Admin.Controllers.API.Dtos;
using IdentityServer4.Admin.Entities;
using IdentityServer4.Admin.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace IdentityServer4.Admin
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AdminOptions _options;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _hostingEnvironment = env;
            _options = new AdminOptions(_configuration);
            Log.Logger.Information("Configuration: " + _options.Version);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add configuration
            services.AddSingleton(_options);

            // Add MVC
            services.AddMvc()
                .AddMvcOptions(o => o.Filters.Add<HttpGlobalExceptionFilter>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthorization();

            string connectionString = _configuration["ConnectionString"];

            // Add DbContext            
            Action<DbContextOptionsBuilder> dbContextOptionsBuilder;
            if (_hostingEnvironment.IsDevelopment())
            {
                dbContextOptionsBuilder = b => b.UseInMemoryDatabase("IDS4");
            }
            else
            {
                var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
                var provider = _configuration["DatabaseProvider"];
                switch (provider.ToLower())
                {
                    case "mysql":
                    {
                        dbContextOptionsBuilder = b =>
                            b.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                        break;
                    }
                    case "sqlserver":
                    {
                        dbContextOptionsBuilder = b =>
                            b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                        break;
                    }
                    default:
                    {
                        throw new Exception($"Unsupported database provider: {provider}");
                    }
                }
            }

            services.AddDbContext<IDbContext, AdminDbContext>(dbContextOptionsBuilder);

            // Add aspnetcore identity
            IdentityBuilder idBuilder = services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireUppercase = _options.RequireUppercase;
                options.Password.RequireNonAlphanumeric = _options.RequireNonAlphanumeric;
                options.Password.RequireDigit = _options.RequireDigit;
                options.Password.RequiredLength = _options.RequiredLength;
                options.User.RequireUniqueEmail = _options.RequireUniqueEmail;
            }).AddErrorDescriber<CustomIdentityErrorDescriber>();

            idBuilder.AddDefaultTokenProviders();
            idBuilder.AddEntityFrameworkStores<AdminDbContext>();

            // Add ids4
            var builder = services.AddIdentityServer()
                .AddAspNetIdentity<User>();
            if (_hostingEnvironment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var key = Directory.Exists("/ids4admin") ? "/ids4admin/signing_key.rsa" : "signing_key.rsa";
                builder.AddDeveloperSigningCredential(true, key);
            }

            builder.AddConfigurationStore<AdminDbContext>(options =>
                {
                    options.ResolveDbContextOptions = (provider, b) => dbContextOptionsBuilder(b);
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore<AdminDbContext>(options =>
                {
                    options.ResolveDbContextOptions = (provider, b) => dbContextOptionsBuilder(b);
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = _options.EnableTokenCleanup;
                });
            builder.AddProfileService<ProfileService>();

            // Configure AutoMapper
            ConfigureAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            PrePareDatabase(app.ApplicationServices, _hostingEnvironment);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void PrePareDatabase(IServiceProvider serviceProvider, IHostingEnvironment env)
        {
            using (IServiceScope scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                if (scope.ServiceProvider.GetRequiredService<AdminDbContext>().Database.EnsureCreated())
                {
                    Log.Logger.Information("Created database success");
                }
                
                SeedData.AddIdentityResources(scope.ServiceProvider).Wait();
                SeedData.EnsureData(scope.ServiceProvider).Wait();

                if (env.IsDevelopment() || _configuration["seed"] == "true")
                {
                    SeedData.EnsureTestData(scope.ServiceProvider).Wait();
                }
            }
        }

        private void ConfigureAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CreateUserDto, User>();
                cfg.CreateMap<Permission, PermissionDto>();
                cfg.CreateMap<PermissionDto, Permission>();
                cfg.CreateMap<Role, RoleDto>();
                cfg.CreateMap<RoleDto, Role>();
                cfg.CreateMap<User, UserOutputDto>();
            });
        }
    }
}