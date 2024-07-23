using FinancialControl.Infra.Data;
using Microsoft.EntityFrameworkCore;
using FinancialControl.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FinancialControlContext>(options =>
     options.UseNpgsql(GetConnectionString(builder.Configuration),
                      b => b.MigrationsAssembly("FinancialControl.Infra")));

builder.Services.AddDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplyMigrations(app);

await app.RunAsync();

static string? GetConnectionString(IConfiguration configuration)
{

    var envConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
    if (string.IsNullOrEmpty(envConnectionString))
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        return connectionString;
    }
    return envConnectionString;
    
}

static void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<FinancialControlContext>();
    dbContext.Database.Migrate();
}
