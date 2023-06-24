using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using CMMSAPIs.BS.FileUpload;
using CMMSAPIs.Middlewares;
using CMMSAPIs.BS.Incident_Reports;
using CMMSAPIs.BS.WC;
using CMMSAPIs.BS.Inventory;
using CMMSAPIs.BS.Facility;
using CMMSAPIs.BS.Jobs;
using CMMSAPIs.BS.Permits;
using CMMSAPIs.BS.JC;
using CMMSAPIs.BS.Mails;
using CMMSAPIs.BS.Masters;
using CMMSAPIs.BS.SM;
using CMMSAPIs.BS.Utils;
using CMMSAPIs.BS.Users;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Mails;
using CMMSAPIs.BS.PM;
using CMMSAPIs.BS.Audits;
using CMMSAPIs.BS.MC;
using CMMSAPIs.BS.Calibration;
using CMMSAPIs.BS.EscalationMatrix;
using CMMSAPIs.BS;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Reflection;

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
            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Title", Version = "v1" });

                // Include comments from XML documentation file
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

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
                options.IdleTimeout = TimeSpan.FromMinutes(5);//You can set Time   
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
            services.AddScoped<IPermitBS, PermitBS>();
            services.AddScoped<IFacilityBS, FacilityBs>();
            services.AddScoped<IJobWorkTypeBS, JobWorkTypeBS>();
            services.AddScoped<IPMBS, PMBS>();
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
            services.AddScoped<iEM, EscalationMatrix>();
            services.Configure<CMMailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IFileUploadBS, FileUploadBS>();
            services.AddScoped<ICheckListBS, CheckListBS>();
            services.AddScoped<IPMScheduleViewBS, PMScheduleViewBS>();
            services.AddScoped<IAuditPlanBS, AuditPlanBS>();
            services.AddScoped<IMCBS, MCBS>();
            services.AddScoped<ICalibrationBS, CalibrationBS>();
            services.AddScoped<IMRSBS, MRSBS>();
            services.AddScoped<IGOBS, GOBS>();
            services.AddScoped<IReOrderBS, ReOrderBS>();    
            services.AddScoped<IAuditScheduleViewBS, AuditScheduleViewBS>();
            services.AddScoped<ISMReportsBS, ReportsBS>();
            services.AddScoped<IRequestOrderBS, RequestOrderBS>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHostedService<SchedulerService>();
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
            // Add Swagger middleware
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Title v1");
                // Optional: Add more Swagger endpoints for different versions
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Your API Title v2");
            });
        }
    }
}
