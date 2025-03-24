using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using WebApiExample.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();

var app = builder.Build();

app.Map("/", (ILoggerManager _logger) =>
{
    _logger.LogInfo("Here is info message from our values controller.");
    _logger.LogDebug("Here is debug message from our values controller.");
    _logger.LogWarn("Here is warn message from our values controller.");
    _logger.LogError("Here is an error message from our values controller.");
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();