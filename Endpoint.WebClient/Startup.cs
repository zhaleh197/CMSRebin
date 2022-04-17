
using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Persons.Commands.LoginUser;
using Endpoint.WebClient.Models;
using Endpoint.WebClient.Models.Collections;
using Endpoint.WebClient.Models.DBs;
//using Endpoint.WebClient.Models.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsRebin.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Endpoint.WebClient.Models.Fields;
using Endpoint.WebClient.Models.Items;
using Microsoft.AspNetCore.Identity;
using Endpoint.WebClient.Models.Common;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Collection.Commands.RemoveTable;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Application.Service.Collection.Commands.EditTable;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using CmsRebin.Application.Service.Persons.Commands.RemoveUser;
using CmsRebin.Application.Service.Persons.Commands.EditUser;
using CmsRebin.Application.Service.Persons.Commands.UserSatusChange;
using CmsRebin.Application.Service.Persons.Queries.GetRoles;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Collection.Commands.EditItem;
using CmsRebin.Application.Service.Collection.Commands.RemoveItem;
using CmsRebin.Application.Service.Database.Commands;
using CmsRebin.Application.Service.Database.Queris.GetDB;
using CmsRebin.Application.Service.Database.Queris.UploadDB;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Application.Service.Filed.Commands.EditField;
using CmsRebin.Application.Service.Filed.Commands.RemoveField;
using CmsRebin.Application.Service.Filed.Queries.Get;

namespace Endpoint.WebClient
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
            string contectionstring = /* "Data Source=.; Initial Catalog= swa ; Integrated Security=true ; MultipleActiveResultSets=True;"*/"Data Source=.; Initial Catalog= swa ; Integrated Security=true ; MultipleActiveResultSets=True";

            //string contectionstring = "Data Source= . ; Integrated Security=true ; MultipleActiveResultSets=True;";

            services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(contectionstring));
            ///
            services.AddScoped<IGetEverythings, GetEverythings>();

            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddScoped<IUserLoginService, UserLoginService>();
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IDBRepository, DBRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFieldRepository, FieldRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICommonRepository, CommonRepository>();





            /////////////
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddScoped<IPostTable, PostTableService>();
            services.AddScoped<ICreatTable, CreatTableService>();
            services.AddScoped<IRemoveTableService, RemoveTableService>();
            services.AddScoped<IEditTableService, EditTableService>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IGetUsersService, GetUserService>();
            services.AddScoped<IRemoveUserService, RemoveUserService>();
            services.AddScoped<IEditUserService, EditUserService>();
            services.AddScoped<IUserSatusChangeService, UserSatusChangeService>();
            services.AddScoped<IUserLoginService, UserLoginService>();
            services.AddScoped<IGetRolesService, GetRolesService>();
            services.AddScoped<IGetEverythings, GetEverythings>();
            services.AddScoped<IGetItemsService, GetItemsService>();
            services.AddScoped<ICreateItem, CreateItemServie>();
            services.AddScoped<IEditItemService, EditItemService>();
            services.AddScoped<IRemoveItem, RemoveItem>();
            ////////////////
            services.AddScoped<ICreatDB, CreatDBService>();
            services.AddScoped<IUploadBD, UploadDBService>();
            services.AddScoped<IGetDB, GetDBService>();
            services.AddScoped<IEditDB, EditDBService>();
            services.AddScoped<IRemoveDB, RemoveDBService>();
            services.AddScoped<ICreateFiled, CreateFiledService>();
            services.AddScoped<IGetFiledsService, GetFiledsService>();
            services.AddScoped<IEditedField, EditedField>();
            services.AddScoped<IRemoveField, RemoveField>();
            /////////////////////////
            



            services.AddMvc();
            //services.AddControllersWithViews();

            /////////////////////////SAve Token In COoki
            services.AddHttpClient("CMSRebinClient", client=>
            {
                client.BaseAddress = new Uri("https://localhost:44332");
                //client.BaseAddress = new Uri("https://localhost:44332");
            });
           
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(op =>
                {
                    //op.LoginPath = "/Auth/Login"; 
                    //op.LogoutPath = "/Auth/Logout";
                    op.LoginPath = "/Auth/LoginAsync"; 
                    op.LogoutPath = "/Auth/SignOutt";
                    op.AccessDeniedPath= "/Auth/LoginAsync";
                    op.Cookie.Name = "Token.Cooki";//this is just name
                });

            //////////
            ///
            //services.AddAuthorization();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.RequireRole("admin"));
                options.AddPolicy("operator", policy => policy.RequireRole("operator"));
                options.AddPolicy("user", policy => policy.RequireRole("user"));
            });
            //services.AddIdentity<IdentityUser, IdentityRole>()
            //.AddEntityFrameworkStores<DatabaseContext>();

          

            //        services.AddIdentity<Automobile.Models.Account, IdentityRole>(options =>
            //        {
            //            options.User.RequireUniqueEmail = false;
            //        })
            //.AddEntityFrameworkStores<Providers.Database.EFProvider.DataContext>()
            //.AddDefaultTokenProviders();
            /////////////////////////////////////////
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
            }
            
            app.UseStaticFiles();
            app.UseRouting();
            //1400-12-1
            app.UseStaticFiles();
            //
            //////////////////// use Cooki for Token
            app.UseCookiePolicy();
            app.UseAuthentication();
            ////////////////////////
            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                     //pattern: "{controller=Home}/{action=Index}/{id?}");
                     pattern: "{controller=Auth}/{action=Login}/{id?}");
            });

        }
    }
}
