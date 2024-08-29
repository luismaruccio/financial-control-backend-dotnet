using FinancialControl.Infra.Data;
using Microsoft.EntityFrameworkCore;
using FinancialControl.IoC;
using FinancialControl.Infra.Bus.Connections;
using FinancialControl.Infra.Bus.Interfaces.Connections;
using FinancialControl.Infra.Bus.Interfaces.Management;
using FinancialControl.Infra.Bus.Management;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FinancialControlContext>(options =>
     options.UseNpgsql(GetDatabaseConnectionString(builder.Configuration),
                      b => b.MigrationsAssembly("FinancialControl.Infra")));

builder.Services.AddSingleton<IRabbitMQConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<RabbitMQConnection>>();
    var factory = new ConnectionFactory()
    {
        Uri = new Uri(GetRabbitMQConnectionString(builder.Configuration)!)
    };
    return new RabbitMQConnection(factory, logger);
});
builder.Services.AddSingleton<IRabbitMQManager, RabbitMQManager>();

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

static string? GetDatabaseConnectionString(IConfiguration configuration)
{
    var envConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
    if (string.IsNullOrEmpty(envConnectionString))
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        return connectionString;
    }
    return envConnectionString;    
}

static string? GetRabbitMQConnectionString(IConfiguration configuration)
{
    var envConnectionString = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING");
    if (string.IsNullOrEmpty(envConnectionString))
    {
        var connectionString = configuration.GetConnectionString("DefaultRabbitMQConnection");
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
