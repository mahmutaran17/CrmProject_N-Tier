 using CrmProject.Business.Abstract;
using CrmProject.Business.Concrete;
using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Concrete;
using CrmProject.DataAccess.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
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
builder.Services.AddScoped<IAppTaskRepository, AppTaskRepository>();
builder.Services.AddScoped<IAppTaskService, AppTaskManager>();
builder.Services.AddScoped<IProjectService, ProjectManager>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IRoleService, RoleManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>(); 
builder.Services.AddScoped<IExpenseService, ExpenseManager>(); 
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>(); 
builder.Services.AddScoped<IIncomeService, IncomeManager>();
builder.Services.AddScoped<ITaskLogRepository, TaskLogRepository>();
builder.Services.AddScoped<ITaskLogService, TaskLogManager>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationManager>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/Index";

        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    });

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
