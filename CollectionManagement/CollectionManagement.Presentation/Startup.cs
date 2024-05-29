﻿using AspNetCoreRateLimit;
using CollectionManagement.Domain.Entities;
using CollectionManagement.Domain.Enums;
using CollectionManagement.Shared.Security;
using Microsoft.AspNetCore.Identity;

namespace CollectionManagement.Presentation;

public static class Startup
{
  private const string _defaultCorsPolicyName = "localhost";

  public static void AddDIServices(this WebApplicationBuilder builder)
  {
    #region Default DI Services
    builder.Services.AddControllersWithViews();
    //builder.Services.AddControllers(
    //  // Agar "Accept" "application/json" request kelmasa 406 Not Acceptable qaytaradi
    //  options => options.ReturnHttpNotAcceptable = true
    //  )
    //  .AddXmlSerializerFormatters();

    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen(options =>
    //{
    //    options.SwaggerDoc("v2", new OpenApiInfo { Title = "ICollection.swagger", Version = "v2" });
    //});
    //builder.Services.AddSwaggerGen(option =>
    //{
    //  option.SwaggerDoc("v1", new OpenApiInfo { Title = "Furniture API", Version = "v1" });
    //  option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //  {
    //    In = ParameterLocation.Header,
    //    Description = "Please enter a valid token",
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.Http,
    //    BearerFormat = "JWT",
    //    Scheme = "Bearer"
    //  });
    //  option.AddSecurityRequirement(new OpenApiSecurityRequirement
    //        {
    //            {
    //                new OpenApiSecurityScheme
    //                {
    //                    Reference = new OpenApiReference
    //                    {
    //                        Type=ReferenceType.SecurityScheme,
    //                        Id="Bearer"
    //                    }
    //                },
    //                new string[]{}
    //            }
    //        });

    //var filePath = Path.Combine(AppContext.BaseDirectory, "Furniture_API.xml");
    //if (File.Exists(filePath))
    //{
    //  option.IncludeXmlComments(filePath);
    //}

    // this is way to represent xml comments in SwaggerUI
    //
    //var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    //option.IncludeXmlComments(xmlCommentsFullPath);

    //});

    #endregion

    #region DBContext

    //builder.Services.AddDbContext<AppDbContext>(ServiceLifetime.Scoped);
    //builder.Services.AddDbContext<AppDbContext>(options =>
    //{
    //  options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
    //});
    //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    #endregion

    #region Identity

    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
      options.Password.RequireDigit = true;
      options.Password.RequiredLength = 6;
      options.Password.RequireNonAlphanumeric = false;
      options.Password.RequireUppercase = false;

      options.User.RequireUniqueEmail = true;
      options.SignIn.RequireConfirmedEmail = true;
    }).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

    //add role manager to DI

    #endregion

    #region Custom DI Services
    //builder.Services.AddHttpContextAccessor();
    builder.Services.ConfigureDatabase(builder.Configuration);
    builder.Services.ConfigureWeb(builder.Configuration);
    builder.Services.ConfigureServices(builder.Environment);
    #endregion

    #region CORS Policy for all origins
    builder.Services.AddCors(options =>
    {
      options.AddPolicy(_defaultCorsPolicyName, builder =>
      {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
      });
    });
    #endregion

    #region JWT
    //var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
    //var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>() ?? "key";
    //builder.Services.AddAuthentication(options =>
    //{
    //  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //})
    //    .AddJwtBearer(options =>
    //    {
    //      options.TokenValidationParameters = new()
    //      {
    //        ValidateIssuer = true,
    //        ValidIssuer = jwtIssuer,
    //        ValidateAudience = true,
    //        ValidAudience = jwtIssuer,
    //        ValidateLifetime = true,
    //        ValidateIssuerSigningKey = true,
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
    //        ClockSkew = TimeSpan.Zero,
    //        RoleClaimType = ClaimTypes.Role
    //      };
    //    });
    #endregion

    #region Add rate limiting to DI services
    builder.Services.Configure<IpRateLimitOptions>
        (builder.Configuration.GetSection("IpRateLimit"));
    builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
    builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    builder.Services.AddMemoryCache();
    builder.Services.AddHttpContextAccessor();
    #endregion

    #region API Versioning
    //builder.Services.AddApiVersioning(options =>
    //{
    //  options.ReportApiVersions = true;
    //  options.DefaultApiVersion = new ApiVersion(1, 0);
    //  options.AssumeDefaultVersionWhenUnspecified = true;
    //  options.ApiVersionReader = new UrlSegmentApiVersionReader();
    //});
    #endregion

    #region Redis
    //builder.Services.Configure<ConfigurationOptions>(builder.Configuration.GetSection("RedisCacheOptions"));
    //builder.Services.AddStackExchangeRedisCache(setupAction =>
    //{
    //  setupAction.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
    //});
    #endregion
  }

  public static void AddMiddleware(this WebApplication app)
  {
    app.UseIpRateLimiting();

    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      //app.UseSwagger();
      //app.UseSwaggerUI();
    }

    if (app.Environment.IsProduction())
    {
      app.UseExceptionHandler("/error");
      app.UseHsts();
    }
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors(_defaultCorsPolicyName);

    app.UseMiddleware<TokenRedirectMiddleware>();
    app.UseMiddleware<ExceptionHandlerMiddleware>();

    app.UseStatusCodePages(async context =>
    {
      if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
      {
        context.HttpContext.Response.Redirect("../accounts/login");
      }
    });

    app.UseStaticFiles();

    // i don't know. but i watch in pluralsight course
    // Authorization must be after Authentication !!!
    //
    //builder.Services.AddAuthorization(options =>
    //{
    //    options.AddPolicy("MustBeFromAntwerp", policy =>
    //    {
    //        policy.RequireAuthenticatedUser();
    //        policy.RequireClaim("city", "Antwerp");
    //    });
    //});
    // after this add to [Authorize(Policy = "MustBeFromAntwerp")]

    app.UseAuthentication();
    // disabled for development
    // app.UseMiddleware<JwtValidation>();
    app.UseAuthorization();
    //var versionSet = app.NewApiVersionSet()
    //                    .HasApiVersion(new ApiVersion(1, 0))
    //                    .Build();
    //app.MapControllers().WithApiVersionSet(versionSet);

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.SeedRolesToDatabase().Wait();
  }

  private static async Task SeedRolesToDatabase(this WebApplication app)
  {
    var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User", "SuperAdmin" };
    foreach (var role in roles)
    {
      if (!roleManager.RoleExistsAsync(role).Result)
      {
        var result = roleManager.CreateAsync(new IdentityRole(role)).Result;
      }
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var admin = new User
    {
      UserName = "Buzurgmexr Sultonaliyev",
      Email = "buzurgmexrubon@gmail.com",
      //Image = "",       // string
      BirthDate = new DateTime(2001, 9, 23),
      Role = RoleConstants.SuperAdmin,
      Status = StatusType.Active,
      CreatedAt = DateTime.Now,
    };

    var adminPassword = "Adm1nj0nm1san$";
    var user = await userManager.FindByNameAsync(admin.UserName);
    if (user == null)
    {
      var createAdmin = await userManager.CreateAsync(admin, adminPassword);
      if (createAdmin.Succeeded)
      {
        await userManager.AddToRoleAsync(admin, "SuperAdmin");
      }
    }
  }
}

