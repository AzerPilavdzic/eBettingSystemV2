using eBettingSystemV2.Services;
using eBettingSystemV2.Services.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //dodano startup
            services.AddAutoMapper(typeof(ICountryService));
            services.AddAutoMapper(typeof(ITeamService));



            //DefaultConnection = Configuration.GetConnectionString();
            string connectionString =this.Configuration.GetValue<string>("DefaultConnection");
            //var _ime = Configuration.GetConnectionString(connectionString);

            string dbConn = Configuration.GetSection("ConnectionString").GetSection("DefaultConnection").Value;
            services.AddDbContext<praksa_dbContext>(options =>
            //options.UseNpgsql(Configuration.GetConnectionString(DefaultConnection)));


            options.UseNpgsql(dbConn));
            //Host = my_host; Database = my_db; Username = my_user; Password = my_pw");


            
            //ne radi
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ITeamService, TeamService>();


            //services.AddScoped<IService<object, object>, BaseService<object, object, object>>();
            //services.AddScoped<ICRUDService<object, object, object, object>, BaseCRUDService<object, object, object, object, object>>();





            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eBettingSystemV2", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "eBettingSystemV2 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
