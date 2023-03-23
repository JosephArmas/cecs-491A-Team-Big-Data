using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Net.Http.Headers;
using TeamBigData.Utification.Manager;
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
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<SecurityManager>();


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

app.MapControllers();

app.Run();
