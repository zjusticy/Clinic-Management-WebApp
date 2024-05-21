using ClinicManagementWebApp.Server.Features.User.Services;
using ClinicManagementWebApp.Server.Features.Users.Entity;
using ClinicManagementWebApp.Server.Infrastructure;
using ClinicManagementWebApp.Server.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();

/*// Add services to the container.
builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(System.Net.IPAddress.Any, 5000);
    serverOptions.Listen(System.Net.IPAddress.Any, 7222, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});*/

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
});


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityApiEndpoints<UserModel>().AddRoles<IdentityRole<Guid>>().AddEntityFrameworkStores<AppDbContext>();

/*builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = null;
    options.LogoutPath = null;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});
*/

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", pb => pb
        .RequireClaim("level", "admin"));

    options.AddPolicy("doctor", pb => pb
        .RequireClaim("level", "doctor", "admin"));

    options.AddPolicy("patient", pb => pb
        .RequireClaim("level", "patient", "admin"));
});

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



var app = builder.Build();


app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*app.UseCors(MyAllowSpecificOrigins);*/

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Task seedingAdmin = AdminSeeder.SeedAdmin(app.Services);

app.MapFallbackToFile("/index.html");

app.Run();

public partial class Program { }