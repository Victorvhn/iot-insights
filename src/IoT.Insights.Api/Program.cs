using IoT.Insights.Api.Infrastructure.ApiClients;
using IoT.Insights.Api.Infrastructure.Authentication;
using IoT.Insights.Api.Infrastructure.Documentation;
using IoT.Insights.Api.Infrastructure.Routing;
using IoT.Insights.Api.Infrastructure.TcpClients;
using IoT.Insights.Api.Infrastructure.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddFluentValidation(typeof(IoT.Insights.Api.Program));
builder.Services.AddApiVersions();
builder.Services.AddEndpoints(typeof(IoT.Insights.Api.Program).Assembly);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddMediator();
builder.Services.AddApiClients(builder.Configuration);
builder.Services.AddTcpClients();

var app = builder.Build();

app.MapEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerWithVersions();
app.UseHttpsRedirection();

app.Run();

namespace IoT.Insights.Api
{
    /// <summary>
    ///     The entry point for the application.
    /// </summary>
    public class Program; // For testing purposes
}