﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Diagnostics.HealthChecks;
using NATS.Client.Core;

namespace Aspire.NATS.Net;

internal sealed class NatsHealthCheck(INatsConnection connection) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = connection.ConnectionState switch
        {
            NatsConnectionState.Open => HealthCheckResult.Healthy(),
            NatsConnectionState.Connecting or NatsConnectionState.Reconnecting => HealthCheckResult.Degraded(),
            NatsConnectionState.Closed => HealthCheckResult.Unhealthy(),
            _ => new HealthCheckResult(context.Registration.FailureStatus)
        };

        return Task.FromResult(result);
    }
}
