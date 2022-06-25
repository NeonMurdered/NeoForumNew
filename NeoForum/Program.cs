using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeoForum.Areas.Identity.Data;
using NeoForum.Data;
using NeoForum.Hubs;
//------------------------------------------------//
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("NeoForumDbContextConnection") ?? throw new InvalidOperationException("Connection string 'NeoForumDbContextConnection' not found.");

builder.Services.AddDbContext<NeoForumDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<NeoForumUser>(options => { options.SignIn.RequireConfirmedAccount = false; options.Password.RequireUppercase = false; options.Password.RequireNonAlphanumeric = false; })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NeoForumDbContext>();
//------------------------------------------------//


//Добавление сервисов
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
//------------------------------------------------//

var app = builder.Build();

//Проверка заполненности
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<NeoForumDbContext>();
        var userManager = services.GetRequiredService<UserManager<NeoForumUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await ContextSeed.SeedRolesAsync(userManager, roleManager);
        await ContextSeed.SeedAdminAsync(userManager, roleManager);
        await ContextSeed.SeedTestUserAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
//------------------------------------------------//

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//------------------------------------------------//

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<ChatHub>("/Home/Index");

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHub<CounterHub>("/onlinecount");
});
//Запуск
app.Run();
//------------------------------------------------//