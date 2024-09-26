using CMMSAPIs.BS;
using CMMSAPIs.BS.Audits;
using CMMSAPIs.BS.Calibration;
using CMMSAPIs.BS.Cleaning;
using CMMSAPIs.BS.DSM;
using CMMSAPIs.BS.EM;
using CMMSAPIs.BS.Facility;
using CMMSAPIs.BS.FileUpload;
using CMMSAPIs.BS.Grievance;
using CMMSAPIs.BS.Incident_Reports;
using CMMSAPIs.BS.Inventory;
using CMMSAPIs.BS.JC;
using CMMSAPIs.BS.Jobs;
using CMMSAPIs.BS.Masters;
using CMMSAPIs.BS.MISEvaluation;
using CMMSAPIs.BS.MISMasters;
using CMMSAPIs.BS.MoM;
using CMMSAPIs.BS.Permits;
using CMMSAPIs.BS.PM;
using CMMSAPIs.BS.SM;
using CMMSAPIs.BS.Users;
using CMMSAPIs.BS.Utils;
using CMMSAPIs.BS.WC;
using CMMSAPIs.Cleaning;
using CMMSAPIs.Helper;
using CMMSAPIs.Middlewares;
using CMMSAPIs.Models.Mails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

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
            //services.AddAuthentication(authOption =>
            //{
            //    authOption.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    authOption.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(jwtOption =>
            //{
            //    var key = Configuration.GetValue<string>("JwtConfig:Key");
            //    var keyBytes = Encoding.ASCII.GetBytes(key);
            //    jwtOption.SaveToken = true;
            //    jwtOption.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            //        ValidateLifetime = true,
            //        ValidateAudience = false,
            //        ValidateIssuer = false,
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
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
            services.Configure<CMMailSettings>(Configuration.GetSection("MailSettings"));
            // services.AddTransient<IMailService, MailService>();
            services.AddScoped<IFileUploadBS, FileUploadBS>();
            services.AddScoped<ICheckListBS, CheckListBS>();
            services.AddScoped<IPMScheduleViewBS, PMScheduleViewBS>();
            services.AddScoped<IAuditPlanBS, AuditPlanBS>();
            services.AddScoped<ICalibrationBS, CalibrationBS>();
            services.AddScoped<IAuditScheduleViewBS, AuditScheduleViewBS>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IMRSBS, MRSBS>();
            services.AddScoped<IGOBS, GOBS>();
            services.AddScoped<IReOrderBS, ReOrderBS>();
            services.AddScoped<CleaningBS>();
            services.AddScoped<ISMReportsBS, ReportsBS>();
            services.AddScoped<IRequestOrderBS, RequestOrderBS>();
            services.AddScoped<IEMBS, EMBS>();
            services.AddScoped<IDSMBS, DSMBS>();
            services.AddScoped<IMISMasterBS, MISMasterBS>();
            services.AddScoped<IGrievanceBS, GrievanceBS>();
            services.AddScoped<IMoMBS, MoMBS>();
            services.AddScoped<AttendeceBS, _AttendeceBS>();
            services.AddScoped<TrainingCourseBS, Traningbs>();
            services.AddScoped<VegBS, vegetaion>();
            services.AddScoped<IMISEvaluationBS, MISEvaluationBS>();
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

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseMyMiddleware();

            // Use FileServer to serve static files
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Upload")),
                RequestPath = "/Upload",
                EnableDirectoryBrowsing = true  // Enable this if you want directory browsing
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
