using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Authorization;
using marauderserver.Controllers;
using marauderserver.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebSocketSharp;
using WebSocketSharp.Server;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "https://localhost:7225")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});

//builder.Services.AddAutoMapper(typeof(Program));

// configure strongly typed settings object
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// configure DI for application services
builder.Services.AddScoped<ControllerBase, UserController>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
//builder.Services.AddScoped<IUserService, UserService>();

//builder.Services.AddControllers()
//    .AddNewtonsoftJson(options =>
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//);

builder.Services.AddDbContext<MarauderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MarauderDb")));

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MarauderContext>();
        context.Database.EnsureCreated();
        context.Seed();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    //using (var scope = app.Services.CreateScope())
//    //{
//    //    var marauderContext = scope.ServiceProvider.GetRequiredService<MarauderContext>();
//    //    marauderContext.Database.EnsureCreated();
//    //    marauderContext.Seed();
//    //}
//    //app.UseExceptionHandler("/Home/Error");
//    //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    //app.UseHsts();
//}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseWebSockets();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();