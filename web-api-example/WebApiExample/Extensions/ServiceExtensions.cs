using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Repository;
using Service;
using Service.Contracts;
using Service.DataShaping;
using Shared.DataTransferObjects;
using WebApiExample.Utility;

namespace WebApiExample.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
        });
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                    b => b.MigrationsAssembly("WebApiExample"))
                .LogTo(Console.WriteLine, LogLevel.Information,
                    DbContextLoggerOptions.SingleLine | DbContextLoggerOptions.LocalTime));
    }

    public static void ConfigureDataShaper(this IServiceCollection services)
    {
        services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
    }

    public static void AddCustomMediaTypes(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            var systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();
            systemTextJsonOutputFormatter?.SupportedMediaTypes
                .Add("application/vnd.codemaze.hateoas+json");

            var xmlOutputFormatter = config.OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?
                .FirstOrDefault();
            xmlOutputFormatter?.SupportedMediaTypes
                .Add("application/vnd.codemaze.hateoas+xml");
        });
    }

    public static void ConfigureLinksForHateoas(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeLinks, EmployeeLinks>();
    }

}