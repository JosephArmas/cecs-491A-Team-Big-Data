using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.PinManagers;
using TeamBigData.Utification.PinServices;
using TeamBigData.Utification.SQLDataAccess;

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
builder.Services.AddSingleton<SecurityManager>();

// Logging dependencies
builder.Services.AddTransient(logger => { return new Logger(new SqlDAO(builder.Configuration.GetConnectionString("LogsSQLDBConnection"))); });

// Pin dependencies.
builder.Services.AddTransient<SqlDAO>(sqlDAO => { return new SqlDAO(builder.Configuration.GetConnectionString("FeaturesSQLDBConnection")); });
builder.Services.AddTransient<PinService>();
builder.Services.AddTransient<PinManager>();


// Not maintainable
// create a DAO factory to pass connection string as the connection adn output a dao
// dao factory can be in dal
// make a factory a sstatic class that can create ado to pass in the conection string and return the right dao
// not as important to move insto seperate files 
// try to make an object mapper by your self to learn a language better

// Maps Dependencie Injection



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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
