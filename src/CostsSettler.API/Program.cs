using CostsSettler.API.Extensions;
using CostsSettler.API.Middlewares;
using CostsSettler.Domain.Profiles;
using CostsSettler.Domain.Queries.Circumstance;
using CostsSettler.Repo;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

builder.Services.AddApplicationServices();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCircumstancesQuery).Assembly));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfiles)));
builder.Services.AddJwtTokenAuthentication(config, env);

builder.Services.AddDbContext<CostsSettlerDbContext>(
    options => options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
    builder =>
    {
        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        builder.MaxBatchSize(1);
    }));

builder.Services.AddSwaggerGen();
builder.Services.AddCors(o =>
    o.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
    }));

var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CostsSettlerExceptionMiddleware>();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

await app.MigrateDatabaseAsync();

app.Run();
