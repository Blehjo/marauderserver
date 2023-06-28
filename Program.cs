using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<MarauderContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MarauderDb")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var marauderContext = scope.ServiceProvider.GetRequiredService<MarauderContext>();
        marauderContext.Database.EnsureDeleted();
        marauderContext.Database.EnsureCreated();
        //marauderContext.Seed();
    }
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

