using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Persons.Commands.EditUser;
using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Application.Service.Persons.Commands.RemoveUser;
using CmsRebin.Application.Service.Persons.Commands.UserSatusChange;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using CmsRebin.Persistance.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Application.Service.Collection.Commands.RemoveTable;
using CmsRebin.Application.Service.Collection.Commands.EditTable;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Application.Service.Persons.Commands.LoginUser;
using CmsRebin.Application.Service.Persons.Queries.GetRoles;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using CmsRebin.Application.Service.Common.Queries;
//using CmsRebin.Application.Service.Log.Commands;
//using CmsRebin.Application.Service.Log.Queris;

namespace Endpoint.Site
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.RequireRole("admin"));
                options.AddPolicy("operator", policy => policy.RequireRole("operator"));
                options.AddPolicy("user", policy => policy.RequireRole("user"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Authentication/Signin");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
                options.AccessDeniedPath = new PathString("/Authentication/Signin");
            });

            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddScoped<IGetUsersService, GetUserService>();
            services.AddScoped<IPostTable, PostTableService>();
            services.AddScoped<ICreatTable, CreatTableService >();
            services.AddScoped<ICreateFiled, CreateFiledService>();
            services.AddScoped<IRemoveUserService, RemoveUserService>();
            services.AddScoped<IEditUserService, EditUserService>();
            services.AddScoped<IUserSatusChangeService, UserSatusChangeService>();
            services.AddScoped<IGetFiledsService, GetFiledsService>();
            services.AddScoped<IRemoveTableService, RemoveTableService>();
            services.AddScoped<IEditTableService, EditTableService>();
            services.AddScoped<IUserLoginService, UserLoginService>();
            services.AddScoped<IGetRolesService, GetRolesService>();
            services.AddScoped<IGetItemsService, GetItemsService>();
            services.AddScoped<ICreateItem , CreateItemServie> ();
            services.AddScoped<IGetEverythings, GetEverythings>();
            //services.AddScoped<IGetLoggsService, GetLoggsService>();

            string contectionstring = "Data Source= .; Initial Catalog= CRMREBINFinal0710 ; Integrated Security=true ; MultipleActiveResultSets=True;";


            // services.AddEntityFrameworkSqlServer().AddDbContext<DatabaseContext>(op=>op.UseSqlServer(Configuration.GetConnectionString("defultConectionString")));

            services.AddEntityFrameworkSqlServer().AddDbContext<DatabaseContext>(op => op.UseSqlServer(contectionstring));
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            ///JWT
            //Jwt
            object p = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:44332",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"))
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCors", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
                          //builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                        .AllowAnyMethod()
                         .AllowCredentials()
                        //.AllowCredentials()

                        .Build();
                });



            });
            ///////////
            ///


            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<DatabaseContext>()
            //    .AddDefaultTokenProviders();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
         );


            });


            ///jwt
            app.UseCors("EnableCors");
            app.UseAuthentication();
        }

    }
}
