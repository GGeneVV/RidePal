using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RidePal.Data;
using RidePal.Data.Seeder;
using RidePal.Models;
using RidePal.Services;
using RidePal.Services.Contracts;
using RidePal.Services.DTOMappers;
using RidePal.Services.Helpers;
using RidePal.Services.Wrappers;
using RidePal.Services.Wrappers.Contracts;
using RidePal.Web.VMMappers;
using System.Linq;
using System.Text;

namespace RidePal.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddControllersWithViews().AddRazorRuntimeCompilation()
                 .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            var appSettingsSection = this.Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication()
               .AddJwtBearer(cfg =>
               {
                   cfg.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(key),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });

            services.AddIdentity<User, Role>(option =>
            {
                option.SignIn.RequireConfirmedAccount = false;
                option.Password.RequiredLength = 4;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RidePal API", Version = "v1" });
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Identity.Cookie";
            });

            services.AddAutoMapper(typeof(Startup));

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DTOMapperProflie());
                mc.AddProfile(new VMMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddRazorPages();

            services.AddScoped<IAlbumService, AlbumService>();

            services.AddScoped<IArtistService, ArtistService>();

            services.AddScoped<IGenreService, GenreService>();

            services.AddScoped<IPlaylistService, PlaylistService>();

            services.AddScoped<ITrackService, TrackService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ridepal v1");
            });


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });


            // TODO: Toggle seeder
            using var servicescope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = servicescope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (await context.Tracks.CountAsync() <= 0)
            {
                await context.SeedDbAsync();
            }

        }
    }
}
