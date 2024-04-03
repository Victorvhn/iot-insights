using FluentAssertions;
using IoT.Insights.Api.Features.Authentication.SignIn.v1;

namespace IoT.Insights.Api.Tests.Features.Authentication.v1;

public class RequestValidatorTests
{
    private readonly SignInRequestValidator _validator = new();

    [Theory]
    [InlineData("", "", false, 2)]
    [InlineData("John Doe", "", false, 1)]
    [InlineData("", "MySup3rSt0ngP@ssw0rd!", false, 1)]
    [InlineData("John Doe", "MySup3rSt0ngP@ssw0rd!", true, 0)]
    public void It_should_validate_sign_in_request(string username, string password, bool isValid, int errorCount)
    {
        var request = new SignInRequest(username, password);

        var result = _validator.Validate(request);

        result
            .IsValid
            .Should()
            .Be(isValid);
        result
            .Errors
            .Should()
            .HaveCount(errorCount);
    }
}