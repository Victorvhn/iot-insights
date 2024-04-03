using FluentResults;
using Mediator;

namespace IoT.Insights.Api.Features.Metrics.Rainfall.v1;

public class Handler : IRequestHandler<GetRainfallMetricsRequest, Result<GetRainfallMetricsResponse>>
{
    public async ValueTask<Result<GetRainfallMetricsResponse>> Handle(GetRainfallMetricsRequest request, CancellationToken cancellationToken)
    {
        // Get device list
        
        // For each device
            // Get device data
            // Filter device
            // Get rainfall metrics
            
        throw new NotImplementedException();
    }
}