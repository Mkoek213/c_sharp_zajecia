var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

// ← here
app.UseStatusCodePagesWithReExecute("/IO/Logowanie");

// now map your controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=IO}/{action=Logowanie}/{id?}");

app.Run();
