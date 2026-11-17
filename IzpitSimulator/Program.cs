using IzpitSimulator.Data;
using IzpitSimulator.Models;
using IzpitSimulator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<ExamSettings>(builder.Configuration.GetSection("Exam"));

// SQL Server (LocalDB) през Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IQuestionRepository, QuestionRepository>();
builder.Services.AddSingleton<IExamService, ExamService>();

var app = builder.Build();

// Създава базата и таблиците при първо стартиране и ги пълни с въпроси
using (var scope = app.Services.CreateScope())
{
    DbSeeder.Initialize(scope.ServiceProvider.GetRequiredService<AppDbContext>());
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Exam}/{action=Index}/{id?}");

app.Run();
