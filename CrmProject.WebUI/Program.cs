using CrmProject.Business.Abstract;
using CrmProject.Business.Concrete;
using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Concrete;
using CrmProject.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CrmDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),
b => b.MigrationsAssembly("CrmProject.DataAccess")
));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//business
builder.Services.AddScoped<ITaskService, TaskManager>();
builder.Services.AddScoped<IProjectService, ProjectManager>();
builder.Services.AddScoped<IUserService, UserManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.Run();
