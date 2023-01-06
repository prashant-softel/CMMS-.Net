using CMMSAPIs.BS.Jobs;
using CMMSAPIs.BS.Mails;
using CMMSAPIs.BS.Masters;
using CMMSAPIs.BS.SM;
using CMMSAPIs.BS.Utils;
using CMMSAPIs.BS.Users;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Mails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.BS.Permits;
using CMMSAPIs.BS.JC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CMMSAPIs.BS.Authentication;
using System.Configuration;
using System.Text;
using CMMSAPIs.BS.FileUpload;
using CMMSAPIs.Middlewares;
using Microsoft.AspNetCore.Http;
using CMMSAPIs.BS.Incident_Reports;
using CMMSAPIs.BS.WC;
using CMMSAPIs.BS.Inventory;

namespace CMMSAPIs
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
            services.AddAuthentication(authOption =>
            {
                authOption.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOption.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtOption =>
            {
                var key = Configuration.GetValue<string>("JwtConfig:Key");
                var keyBytes = Encoding.ASCII.GetBytes(key);
                jwtOption.SaveToken = true;
                jwtOption.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(1);//You can set Time   
            });
            services.AddControllers();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
          
            services.AddHttpClient();
            //Enable CORS
            services.AddCors(c =>
            c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddScoped<DatabaseProvider>();
            services.AddScoped<ICMMSBS, CMMSBS>();
            services.AddScoped<IJobBS, JobBS>();
            services.AddScoped<iLoginBS, LoginBS>();
            services.AddScoped<IRoleAccessBS, RoleAccessBS>();
            services.AddScoped<IUserAccessBS, UserAccessBS>();
            services.AddScoped<ISMMasterBS, SMMasterBS>();
            services.AddScoped<IUtilsBS, UtilsBS>();
            services.AddScoped<IPermitBS, PermitBS>();
            services.AddScoped<IJCBS, JCBS>();
            services.AddScoped<IIncidentReportBS, IncidentReportBS>();
            services.AddScoped<IWCBS, WCBS>();
            services.AddScoped<IInventoryBS, InventoryBS>();
            services.Configure<CMMailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IJwtTokenManagerBS, JwtTokenManagerBS>();
            services.AddScoped<IFileUploadBS, FileUploadBS>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
              
                app.UseHsts();
            }
          //  app.UseHttpsRedirection();
            //app.UseMvc();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseMyMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
