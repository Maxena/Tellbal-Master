using AspNetCoreRateLimit;
using Common;
using Data;
using Data.Repositories;
using Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Contracts;
using Services.Services;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebFramework.Filters;
using WebFramework.Middlewares;
using WebFramework.WS;

namespace Tellbal
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly SiteSettings _siteSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _siteSettings = Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy(
                    "MyPolicy",
                    builder =>
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddLogging();

            services.AddSignalR();

            services.AddResponseCaching();

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
                options.MimeTypes =
                ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] {
                        "image/svg+xml" ,
                        "application/json" ,
                        "application/xml" ,
                        "text/css" ,
                        "text/json" ,
                        "text/plain" ,
                        "text/xml" ,
                        "text/javascript",
                    });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.TokenValidationParameters =
                new TokenValidationParameters
                {
                    LifetimeValidator = (before, expires, token, param) =>
                    {
                        return expires > DateTime.UtcNow;
                    },
                    // bayad validator algorithm ha ro piade konam
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateActor = false,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            Configuration["Tokens:Key"]))
                };

                jwt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;

                        if
                        (
                        !string.IsNullOrEmpty(accessToken)
                        &&
                        path.StartsWithSegments("/hub")
                        )
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers()
                .AddJsonOptions(configure =>
                {
                    configure.JsonSerializerOptions
                    .Converters.Add(new JsonStringEnumConverter());
                });

            services.AddMvc(options =>
            {
                //options.Filters.Add(new AuthorizeFilter()); // add authorization to all actions
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Tellbal api",
                    Description = "راهنمای استفاده از رابط برنامه نویسی نرم افزار تل بال",
                    TermsOfService = new Uri("https://cyberoxi.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "javad zabihi",
                        Email = "jzabihi1980@gmail.com",
                        Url = new Uri("https://cyberoxi.com/jzabihi"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "مجوز استفاده",
                        Url = new Uri("https://cyberoxi.com/license"),
                    }
                });

                c.OperationFilter<SwaggerFileOperationFilter>();

                c.OperationFilter<CategorizeFilter>();

                //c.DocumentFilter<LowerCaseDocumentFilter>();

                c.OrderActionsBy((apiDesc) =>
                {
                    return $"{apiDesc.RelativePath.Split('/')[3]}_{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}_{apiDesc.RelativePath}";
                }
               );

                c.DocInclusionPredicate((name, api) => true);

                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Description =
                    @"JWT Authorization header using the Bearer scheme." +
                    "\r\n\r\n" +
                    "Enter TOKEN in the text input below." +
                    "\r\n\r\n" +
                    "Example: 'a1.b2.c3'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    BearerFormat = "JWT",

                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {securityScheme, new[] { "Bearer" } }
                };

                c.AddSecurityRequirement(securityRequirement);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);

                c.UseInlineDefinitionsForEnums();
            });

            services.AddSingleton<PresenceTracker>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IMemberService, MemberService>();

            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ISellService, SellService>();

            services.AddScoped<IManageService, ManageService>();

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
                //options.UseSqlServer(Configuration.GetConnectionString("JZ"));
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)), x =>
            {

            });

            services.AddControllers()
                .AddJsonOptions(configure =>
                {
                    configure.JsonSerializerOptions
                    .Converters.Add(new JsonStringEnumConverter());
                });


            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddOptions();

            services.AddMemoryCache();

            services.AddInMemoryRateLimiting();

            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(AutoMapperConfiguration));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseIpRateLimiting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tellbal v1");

                c.DocumentTitle = "tellbal API";

                c.DisplayRequestDuration();

                c.DocExpansion(DocExpansion.None);
                //c.EnableTryItOutByDefault();
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();


                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCustomExceptionHandler();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<AppHub>("/hub");

                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }

    }
}
