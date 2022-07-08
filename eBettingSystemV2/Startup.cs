using eBettingSystemV2.Controllers;
using eBettingSystemV2.Services;
using eBettingSystemV2.Services.DataBase;
using eBettingSystemV2.Services.Interface;
using eBettingSystemV2.Services.Linq.Interface;
using eBettingSystemV2.Services.Linq.Servisi;
using eBettingSystemV2.Services.Servisi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RezultatiImporter.Services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBettingSystemV2
{
    public class Startup 
    {
        public IConfiguration Configuration { get; }
        public string DefaultConnection { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var _con = Configuration["Test"];
       

        }
        private readonly string _policyName = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //new
            services.AddMemoryCache();


            services.AddCors(opt =>
            {
                opt.AddPolicy(name: _policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                         .WithMethods("GET", "PUT", "DELETE", "POST", "PATCH")
                        ;
                });
            });

            //dodano startup
            services.AddAutoMapper(typeof(ICountryService));
            services.AddAutoMapper(typeof(ITeamService));
            services.AddAutoMapper(typeof(ICompetitionService));



            //DefaultConnection = Configuration.GetConnectionString();
            string connectionString =this.Configuration.GetValue<string>("DefaultConnection");
            //var _ime = Configuration.GetConnectionString(connectionString);

            string dbConn = Configuration.GetSection("ConnectionString").GetSection("DefaultConnection").Value;
            services.AddDbContext<praksa_dbContext>(options =>
            //options.UseNpgsql(Configuration.GetConnectionString(DefaultConnection)));


            options.UseNpgsql(dbConn));
            //Host = my_host; Database = my_db; Username = my_user; Password = my_pw");

            #region AddTransient

           
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<ISportService, SportService>();
            services.AddTransient<ICompetitionService, CompetitionService>();
            services.AddTransient<IEventService,EventService>();
            services.AddTransient<IDemo, DemoServices>();
            services.AddTransient<ICache,CacheService>();
            services.AddTransient<IFetch,FetchService>();
            services.AddTransient<ITimer,TimerService>();
            services.AddTransient<ILogCompetition, LogCompetitionService>();
            services.AddTransient<IFetchCacheInsert, FetchCacheInsertService>();
            services.AddTransient<ICountryNPGSQL, CountryNPGSQL>();
            services.AddTransient<eBettingSystemV2.Services.NPGSQL.Interface.ICountryNPGSQL, eBettingSystemV2.Services.NPGSQL.Service.CountryNPGSQLService>();
            

            //services.AddTransient<IRezultatiService, RezultatiService>();
            //services.AddTransient<IMemoryCache, DemoController>();

            #endregion 

            //services.AddScoped<IService<object, object>, BaseService<object, object, object>>();
            //services.AddScoped<ICRUDService<object, object, object, object>, BaseCRUDService<object, object, object, object, object>>();


            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                //options.DefaultApiVersion = new ApiVersion(1, 0);
                options.DefaultApiVersion = ApiVersion.Default;
                //options.ApiVersionReader = new MediaTypeApiVersionReader("version");
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new MediaTypeApiVersionReader("version"),
                    new MediaTypeApiVersionReader("X-version"));
            });

            //edited
            services.AddControllers().AddNewtonsoftJson();

          


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eBettingSystemV2", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "eBettingSystemV2", Version = "v2" });
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "eBettingSystemV2 v1"));
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "eBettingSystemV2 v2"));

                

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(_policyName);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
