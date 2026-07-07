using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=jobtracker.db"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// 🔥 ONE-TIME CLEANUP (RUNS ON START)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var apps = db.Applications.ToList();

    foreach (var a in apps)
    {
        var status = a.Status.ToString().Trim().ToLower();

        if (status == "applied")
            a.Status = ApplicationStatus.Applied;

        if (status == "interviewing")
            a.Status = ApplicationStatus.Interviewing;

        if (status == "offer")
            a.Status = ApplicationStatus.Offer;

        if (status == "rejected")
            a.Status = ApplicationStatus.Rejected;
    }

    db.SaveChanges();
}

app.Run();