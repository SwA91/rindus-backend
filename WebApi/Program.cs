using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Middleware;
using WebApi.Data;
using WebApi.Data.Users;
using WebApi.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
    opt.LogTo(
        Console.WriteLine,
        new[] { DbLoggerCategory.Database.Command.Name },
        LogLevel.Information
    ).EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new UserProfile()));
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddCors(opt => opt.AddPolicy("cors_app",
    _builder => _builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ManagerMiddleware>();

app.UseCors("cors_app");

app.UseAuthorization();

app.MapControllers();

using (IServiceScope environment = app.Services.CreateScope())
{

    IServiceProvider services = environment.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "A migration error has ocurred");
        throw;
    }
}

app.Run();
