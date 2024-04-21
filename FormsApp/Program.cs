using FormsApp.Models.ContextClasses;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<MyContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")).UseLazyLoadingProxies());// Bu ifade bizim SQL ile baðlantýmýzýn tetiklenebilmesi için Middleware'e eklenmiþtir.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
