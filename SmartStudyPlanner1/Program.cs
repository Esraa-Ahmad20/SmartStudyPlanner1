using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner1.Data;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Controllers + Views
// =======================
builder.Services.AddControllersWithViews();

// =======================
// Database (MySQL)
// =======================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// =======================
// ?? HttpClient (IMPORTANT FIX)
// =======================
builder.Services.AddHttpClient();

// =======================
// Session
// =======================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
});

// =======================
// Http Context Accessor
// =======================
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// =======================
// Exception handling
// =======================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// =======================
// Middleware pipeline
// =======================
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// =======================
// Default Route
// =======================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// =======================
// Controllers routing (??? ?? ???? Attribute Routing ?? /Ai)
// =======================
app.MapControllers();

app.Run();