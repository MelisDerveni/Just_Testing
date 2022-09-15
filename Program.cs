// Additional libraries
using Microsoft.EntityFrameworkCore;
using Just_Testing.Models;
// Creates builder (also part of boilerplate code for web apps)
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddSession();


//  Creates the db connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Adds database connection - must be before app.Build();
builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});




var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
