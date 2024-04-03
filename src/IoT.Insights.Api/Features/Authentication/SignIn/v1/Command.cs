using FluentResults;
using Mediator;

namespace IoT.Insights.Api.Features.Authentication.SignIn.v1;

/// <summary>
///     Represents a command to sign in a user.
/// </summary>
/// <param name="Username"> The username of the user. </param>
/// <param name="Password"> The password of the user. </param>
public record SignInCommand(
    string Username,
    string Password
) : ICommand<Result<SignInResponse>>;