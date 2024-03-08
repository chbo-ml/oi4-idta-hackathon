using hackathon.Database;
using hackathon.Import;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddControllers((configure) =>
{
});

builder.Services.AddDbContext<HackathonContext>(options =>
{
    options.UseSqlite();
});


builder.Services.AddScoped<ImportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();


// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<HackathonContext>();

    if (dbContext != null)
    {
        dbContext.Database.Migrate();
    }
}