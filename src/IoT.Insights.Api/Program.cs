using IoT.Insights.Api.Infrastructure.Authentication;
using IoT.Insights.Api.Infrastructure.Documentation;
using IoT.Insights.Api.Infrastructure.Routing;
using IoT.Insights.Api.Infrastructure.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddFluentValidation(typeof(Program));
builder.Services.AddApiVersions();
builder.Services.AddEndpoints(typeof(Program).Assembly);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddMediator();

var app = builder.Build();

app.MapEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerWithVersions();
app.UseHttpsRedirection();

app.Run();

/// <summary>
///     The entry point for the application.
/// </summary>
public partial class Program; // For testing purposes