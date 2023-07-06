using CostsSettler.API.Extensions;
using CostsSettler.API.Middlewares;
using CostsSettler.Domain.Profiles;
using CostsSettler.Domain.Queries;
using CostsSettler.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCircumstancesQuery).Assembly));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfiles)));

builder.Services.AddJwtTokenAuthentication(config, env);
builder.Services.AddAuthorization(options =>
    options.FallbackPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.MigrateDatabaseAsync();

app.Run();
