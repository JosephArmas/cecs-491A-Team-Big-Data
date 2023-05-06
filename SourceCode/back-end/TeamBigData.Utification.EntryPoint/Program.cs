using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Text;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.FileServices;
using TeamBigData.Utification.FileManagers;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.PinManagers;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;
<<<<<<< HEAD
using TeamBigData.Utification.ServiceOfferingsManagers;
using TeamBigData.Utification.ServiceOfferingsServices;
using TeamBigData.Utification.ReputationServices;
using TeamBigData.Utification.DeletionService;
=======
>>>>>>> parent of 7553d278 (Trying to integrate features together and fixing any merging problems)

var builder = WebApplication.CreateBuilder(args);

// adding cors
var corsConfig = new CorsPolicyBuilder();

// to listen on lite-server (front-end)
corsConfig.WithOrigins("http://localhost:3000");

// adding request types here -> using all for now
corsConfig.WithMethods("GET","POST");

corsConfig.AllowAnyHeader();

// allowing cross origin request -> using lite-server
corsConfig.AllowCredentials();

var corsTest = corsConfig.Build();

// JWT Middleware
builder.Services.AddAuthentication(f =>
{
    f.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    f.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(k =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    k.SaveToken = true;
    k.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        //RoleClaimType = builder.Configuration["JWT:Role"],
        IssuerSigningKey = new SymmetricSecurityKey(Key),
        ClockSkew = TimeSpan.Zero
    };
});

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddSingleton<SecurityManager>();

var sqlDAOFactory = new SqlDAOFactory();

// Logging dependencies
builder.Services.AddDbContext<LogsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LogsSQLDBConnection")));
builder.Services.AddTransient<ILogger, Logger>();

// Security manager dependencies
builder.Services.AddDbContext<UsersSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UsersSQLDBConnection")));
builder.Services.AddTransient<AccountRegisterer>();
builder.Services.AddTransient<AccountAuthentication>();

builder.Services.AddDbContext<UsersSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UsersSQLDBConnection")));
builder.Services.AddTransient<RecoveryServices>();

builder.Services.AddDbContext<UserhashSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserHashSQLDBConnection")));
builder.Services.AddTransient<UserhashServices>(); 



// Pin dependencies
builder.Services.AddDbContext<PinsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddTransient<PinService>();
builder.Services.AddTransient<PinManager>();

//File dependencies
builder.Services.AddDbContext<FileSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddTransient<FileService>();
builder.Services.AddTransient<FileManager>();

// Reputation dependencies
builder.Services.AddDbContext<ReportsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddTransient<ReputationService>();
builder.Services.AddTransient<ReputationManager>();

// Service Offering dependencies
builder.Services.AddDbContext<ServicesSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddTransient<ServiceOfferingService>();
builder.Services.AddTransient<ServiceRequestService>();
builder.Services.AddTransient<ServiceOfferingManager>();
builder.Services.AddTransient<ServiceRequestManager>();

// Security Manager
builder.Services.AddTransient<AccDeletionService>();
builder.Services.AddTransient<SecurityManager>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<CorsMiddleware>(corsTest);
app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
