using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Collection.Commands.EditItem;
using CmsRebin.Application.Service.Collection.Commands.EditTable;
using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Application.Service.Collection.Commands.RemoveItem;
using CmsRebin.Application.Service.Collection.Commands.RemoveTable;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Application.Service.Common.SMS;
using CmsRebin.Application.Service.Database.Commands;
using CmsRebin.Application.Service.Database.Queris.GetDB;
using CmsRebin.Application.Service.Database.Queris.UploadDB;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Application.Service.Filed.Commands.EditField;
using CmsRebin.Application.Service.Filed.Commands.RemoveField;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Application.Service.Persons.Commands.EditUser;
using CmsRebin.Application.Service.Persons.Commands.LoginUser;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Application.Service.Persons.Commands.RemoveUser;
using CmsRebin.Application.Service.Persons.Commands.UserSatusChange;
using CmsRebin.Application.Service.Persons.Queries.GetRoles;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using CmsRebin.Persistance.Context;
using Jose;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Endpoint.WebAPI
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
            ////////
            ///

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
            //////////
            
            services.AddScoped<ICreatDB, CreatDBService>();
            services.AddScoped<IUploadBD, UploadDBService>();
            services.AddScoped<IGetDB, GetDBService>();
            services.AddScoped<IEditDB, EditDBService>();
            services.AddScoped<IRemoveDB, RemoveDBService>();

            services.AddScoped<ICreateFiled, CreateFiledService>();
            services.AddScoped<IGetFiledsService, GetFiledsService>();
            services.AddScoped<IEditedField, EditedField>();
            services.AddScoped<IRemoveField, RemoveField>();
            ///////////
            ///

            services.AddScoped<ISMSSender, SMSSender>();
            ////


            services.AddControllers(options =>
            {
                options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                }));
            });




            string contectionstring =/* "Data Source=.; Initial Catalog= swa ; Integrated Security=true ; MultipleActiveResultSets=True;"*/"Data Source=.; Initial Catalog= swa ; Integrated Security=true ; MultipleActiveResultSets=True";

            //string contectionstring = "Data Source= . ; Integrated Security=true ; MultipleActiveResultSets=True;";

            services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(contectionstring));
            ///

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Endpoint.WebAPI", Version = "v1" });

            });



            //////////////////////////////////////14001019- elstic vsearch-youtub
            ///
            var setting = new ConnectionSettings();
            services.AddSingleton<IElasticClient>(new ElasticClient(setting));


            ////////////////////////////////////////

            services.AddResponseCaching();
            services.AddMemoryCache();// insert IMemoryCach to project and use is.


            ///JWT
            //Jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, // tokent be expier.
                    //RequireExpirationTime=true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = "https://localhost:44332",
                    ValidIssuer = "http://localhost:800",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"))
                };
            });

            // Allow to ather app to use these APis.
            services.AddCors(options =>
            {
                options.AddPolicy("RebinCMS", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
                         // builder.AllowAnyOrigin() 
                          .AllowAnyHeader()
                        .AllowAnyMethod()
                         .AllowCredentials()
                        //.AllowCredentials()

                        .Build();
                });



            });
            ///////////

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Endpoint.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //1400-12-1
            app.UseStaticFiles();
            /////
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
            });

            app.UseResponseCaching();

            //ceate middeleware to Token and Aouthentication
            app.UseCors("RebinCMS");
            app.UseAuthentication();

        }
    }
}
