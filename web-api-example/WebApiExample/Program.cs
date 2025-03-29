using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning.ApiExplorer;
using AspNetCoreRateLimit;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Scalar.AspNetCore;
using WebApiExample;
using WebApiExample.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
        config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
    }).AddXmlDataContractSerializerFormatters()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        //Не записывает null
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
// Without this code, our API wouldn’t work, and wouldn’t know where to route incoming requests.

builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.ConfigureDataShaper();
builder.Services.AddCustomMediaTypes();
builder.Services.ConfigureLinksForHateoas();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

string[] versions = ["v1", "v2"];

foreach (var version in versions)
{
    builder.Services.AddOpenApi(version, options =>
    {
        // Add the appropriate API version information to the document
        options.AddDocumentTransformer((document, context, _) =>
        {
            var descriptionProvider = context.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            var versionDescription = descriptionProvider.ApiVersionDescriptions.FirstOrDefault(x => x.GroupName == version);
            document.Info.Version = versionDescription?.ApiVersion.ToString();
            return Task.CompletedTask;
        });

        // Indicate if the API is deprecated
        options.AddOperationTransformer((operation, context, _) =>
        {
            var apiDescription = context.Description;
            operation.Deprecated = apiDescription.IsDeprecated();
            return Task.CompletedTask;
        });
    });
}

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseIpRateLimiting();

app.UseCors("CorsPolicy");

// should go after CORS!
app.UseResponseCaching(); //is not that good for validation — it is much better for just expiration
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        // Use [ProducesResponseType((201))] in controllers
        options.AddDocuments(versions);
        
        options.WithTheme(ScalarTheme.DeepSpace)
            .WithLayout(ScalarLayout.Modern)
            .WithSearchHotKey("f")
            .WithTitle("CodeMase API");
    });
}

app.MapControllers();

app.ApplyMigrations(app.Services);

app.Run();