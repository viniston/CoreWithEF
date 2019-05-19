using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ReviewAPIMicroService.Configuration;
using ReviewAPIMicroService.Helpers;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using FluentValidation.AspNetCore;
using AutoMapper;
using BusinessDataAccess.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Common;
using Swashbuckle.AspNetCore.Swagger;

namespace ReviewAPIMicroService
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _environment = environment;
            _configuration = configuration;
        }

        // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
                builder.AddConsole();
            });

            LogManager.LoadConfiguration($"nlog.{_environment.EnvironmentName}.config");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
            EntityFrameworkConfiguration.ConfigureServices(services, _configuration, _environment);
            DiContainerConfiguration.ConfigureServices(services, _configuration);

            //Initiating Seed Data
            services.AddTransient<InitialData>();

            #region Authorization Policies
            services.AddAuthorization(options =>
            {
                const string claimType = "ReviewGroupPolicy";
                options.AddPolicy("IsWAIAdmin", policy => policy.RequireClaim(claimType, _configuration.GetSection("Roles:AdminGroup").Value));
                options.AddPolicy("IsWAIRealTimeReportUser", policy => policy.RequireClaim(claimType, _configuration.GetSection("Roles:RealTimeReportGroup").Value));
                options.AddPolicy("IsWAIHistoricalReportUser", policy => policy.RequireClaim(claimType, _configuration.GetSection("Roles:HistoricalReportGroup").Value));
            });
            #endregion

            // Add MVC Filters
            // Adds FluentValidation for the application.
            services.AddMvc(options =>
                {
                    if (!_environment.IsLocalhost())
                    {
                        var policyBuilder = new AuthorizationPolicyBuilder();
                        policyBuilder.RequireAuthenticatedUser();
                        var policy = policyBuilder.Build();
                        options.Filters.Add(new AuthorizeFilter(policy));
                    }
                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                })
                .AddFluentValidation(fv =>
                    {
                        fv.ImplicitlyValidateChildProperties = true;
                        fv.RunDefaultMvcValidationAfterFluentValidationExecutes = true;
                        fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                    }
                );

            // Disable only the Automatic HTTP 400 Responses
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddAutoMapper();
            services.AddHttpClient();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Review API", Version = "v1" });
            });
            services.ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
            });


            services.AddHttpContextAccessor();
            ConfigureAuthService(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, InitialData seedDbContext)
        {

            if (env.IsLocalhost())
            {
                app.UseDeveloperExceptionPage();

                loggerFactory.AddConsole(_configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //Ensure EF Db is created for Local Development Environments
            if (env.IsLocalhost())
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ReviewDbContext>();
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();
                }
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Review API V1");
            });

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "WaiUserGetByString",
                    template: "{controller}/{action}/{userName?}");
            });

            // Initiating from here
            seedDbContext.SeedData();
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            #region JWT Token
            // prevent from mapping "sub" claim to name identifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "ReviewGroupPolicy",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration.GetSection("JWT:Issuer").Value,
                    ValidAudience = _configuration.GetSection("JWT:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value))
                };
            });
            #endregion
        }

    }
}
