using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace IoT.Insights.Api.Infrastructure.Documentation;

internal static class Extensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = """
                              JWT Authorization header using the Bearer scheme.
                              <br/>Enter 'Bearer' [space] and then your token in the text input below.
                              <br/>Example: 'Bearer 12345random'
                              <br/><br/>
                              """,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);
        });

        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }

    public static void UseSwaggerWithVersions(
        this WebApplication application)
    {
        var apiVersionDescriptionProvider = application.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        application.UseSwagger();
        application.UseSwaggerUI(options =>
        {
            foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
                options.SwaggerEndpoint(
                    $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                    apiVersionDescription.GroupName.ToUpperInvariant()
                );
            options.EnablePersistAuthorization();
        });
    }
}