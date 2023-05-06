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
using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinManagers;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.AlertManagers;
using TeamBigData.Utification.AlertServices;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.SQLDataAccess.Abstractions;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Pins;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Alerts;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.SQLDataAccess.LogsDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB.Abstractions;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB.Abstractions;
using ILogger = TeamBigData.Utification.Logging.Abstraction.ILogger;
using TeamBigData.Utification.ErrorResponse;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddDbContext<ILogsDBInserter, LogsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LogsSQLDBConnection")));
builder.Services.AddTransient<ILogger, Logger>();

// Security manager dependencies
builder.Services.AddDbContext<IUsersDBInserter, UsersSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UsersSQLDBConnection")));
builder.Services.AddDbContext<IUsersDBSelecter, UsersSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UsersSQLDBConnection")));
builder.Services.AddTransient<AccountRegisterer>();
builder.Services.AddTransient<AccountAuthentication>();

builder.Services.AddDbContext<IUsersDBUpdater, UsersSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UsersSQLDBConnection")));
builder.Services.AddTransient<RecoveryServices>();

builder.Services.AddDbContext<IUserhashDBInserter, UserhashSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserHashSQLDBConnection")));
builder.Services.AddTransient<UserhashServices>(); 
builder.Services.AddTransient<SecurityManager>();


// Pin dependencies
builder.Services.AddDbContext<IPinDBInserter, PinsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddDbContext<IPinDBSelecter, PinsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddDbContext<IPinDBUpdater, PinsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddTransient<PinService>();
builder.Services.AddTransient<PinManager>();

//Alert dependencies
builder.Services.AddDbContext<IAlertDBInserter, AlertsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddDbContext<IAlertDBSelecter, AlertsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddDbContext<IAlertDBUpdater, AlertsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
//builder.Services.AddDbContext<IDBAlert, AlertsSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddTransient<AlertService>();
builder.Services.AddTransient<AlertManager>();


//File dependencies
builder.Services.AddDbContext<FileSqlDAO>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")));
builder.Services.AddTransient<FileService>();
builder.Services.AddTransient<FileManager>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseMiddleware<CorsMiddleware>(corsTest);

// Add CORS Custom Middleware
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.Add("access-control-allow-credentials", "true");
        context.Response.Headers.Add("access-control-allow-headers", "content-type");
        context.Response.Headers.Add("access-control-allow-methods", "GET, POST, OPTIONS");
        context.Response.Headers.Add("access-control-allow-origin", "*");
        var client = new Microsoft.Extensions.Primitives.StringValues();
        context.Request.Headers.TryGetValue("origin", out client);
        var method = context.Request.Method;
        if(client.Equals("http://localhost:3000") && method.Equals("OPTIONS"))
        {
            context.Response.StatusCode = 200;
        }
        context.Response.Headers.Remove("server");
        context.Response.Headers.Remove("X-Powered-By");

        return Task.FromResult(0);
    });

    await next(context);
});


//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
