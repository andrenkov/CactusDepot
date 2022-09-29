using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CactusDepot.Common.Models.Models.Healthcheck
{
    public class HealthcheckSrv : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
                HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var isHealthy = context.GetHashCode() > 0; //true;

            // ...

            if (isHealthy)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));
            }

            return Task.FromResult(
                new HealthCheckResult(
                    context.Registration.FailureStatus, "An unhealthy result."));
        }
    }
}
