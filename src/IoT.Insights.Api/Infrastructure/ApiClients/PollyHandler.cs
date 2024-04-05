using Polly;

namespace IoT.Insights.Api.Infrastructure.ApiClients;

public class PollyHandler(IAsyncPolicy<HttpResponseMessage> policy) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return policy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
    }
}