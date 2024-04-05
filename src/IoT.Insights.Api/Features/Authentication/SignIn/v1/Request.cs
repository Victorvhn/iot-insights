namespace IoT.Insights.Api.Features.Authentication.SignIn.v1;

/// <summary>
///     Represents the required data to sign in a user.
/// </summary>
/// <param name="Username"> The username of the user.</param>
/// <param name="Password"> The password of the user.</param>
public record SignInRequest(
    string Username,
    string Password
);