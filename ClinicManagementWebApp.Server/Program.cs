using ClinicManagementWebApp.Server.Features.Appointment.Services;
using ClinicManagementWebApp.Server.Features.Users.Entity;
using ClinicManagementWebApp.Server.Infrastructure;
using ClinicManagementWebApp.Server.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
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
builder.Services.AddScoped<IAppointmentServices, AppointmentServices>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Task seedingAdmin = AdminSeeder.SeedAdmin(app.Services);

app.MapFallbackToFile("/index.html");

app.Run();

