namespace IoT.Insights.Api.Infrastructure.Authentication;

/// <summary>
///     Represents the options for authentication.
/// </summary>
public record AuthenticationOptions
{
    /// <summary>The secret key used to sign the JWT token.</summary>
    public string? Secret { get; init; }

    /// <summary>The issuer of the JWT token.</summary>
    public string? Issuer { get; init; }

    /// <summary>The audience of the JWT token.</summary>
    public string? Audience { get; init; }

    /// <summary>If issuer validation should be enabled.</summary>
    public bool ValidateIssuer { get; init; } = true;

    /// <summary>If audience validation should be enabled.</summary>
    public bool ValidateAudience { get; init; } = true;

    /// <summary>If lifetime validation should be enabled.</summary>
    public bool ValidateLifetime { get; init; } = true;

    /// <summary>If the token should be validated.</summary>
    public bool ValidateIssuerSigningKey { get; init; } = true;
}