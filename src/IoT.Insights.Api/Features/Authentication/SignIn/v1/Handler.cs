using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentResults;
using IoT.Insights.Api.Infrastructure.Authentication;
using Mediator;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IoT.Insights.Api.Features.Authentication.SignIn.v1;

internal class Handler(IOptions<AuthenticationOptions> options) : ICommandHandler<SignInCommand, Result<SignInResponse>>
{
    private readonly AuthenticationOptions _authenticationOptions = options.Value;

    public ValueTask<Result<SignInResponse>> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        if (!AreCredentialsValid(command))
            return ValueTask.FromResult(
                Result.Fail<SignInResponse>("Invalid username or password")
            );

        var token = GenerateJwt(command);
        var response = new SignInResponse(token);

        return ValueTask.FromResult(
            Result.Ok(response)
        );
    }

    private static bool AreCredentialsValid(SignInCommand command)
    {
        return command is { Username: "admin", Password: "admin" };
    }

    private string GenerateJwt(SignInCommand command)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationOptions.Secret!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, command.Username)
        };

        var token = new JwtSecurityToken(_authenticationOptions.Issuer,
            _authenticationOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}