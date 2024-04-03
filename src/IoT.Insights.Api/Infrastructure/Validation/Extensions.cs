using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace IoT.Insights.Api.Infrastructure.Validation;

internal static class Extensions
{
    public static void AddFluentValidation(this IServiceCollection services, Type programType)
    {
        ArgumentNullException.ThrowIfNull(programType);

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining(programType, ServiceLifetime.Singleton);
    }
}