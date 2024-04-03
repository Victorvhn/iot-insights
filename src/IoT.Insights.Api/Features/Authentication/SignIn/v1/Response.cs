namespace IoT.Insights.Api.Features.Authentication.SignIn.v1;

/// <summary>
///     Represents the response of a sign in request.
/// </summary>
/// <param name="Token">The access token for the user.</param>
public record SignInResponse(
    string Token
);