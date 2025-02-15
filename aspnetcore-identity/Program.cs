using aspnetcore_identity.Middlewares;
using aspnetcore_identity.Models;
using aspnetcore_identity.Models.Identity;
using aspnetcore_identity.Models.Settings;
using aspnetcore_identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityDatabaseContext>(p =>
    p.UseSqlServer(connectionString));

builder.Services.AddIdentity<UserIdentity, RoleIdentity>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<IdentityDatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUsersServices, UsersServices>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
