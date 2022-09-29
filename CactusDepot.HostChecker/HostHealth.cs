using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CactusDepot.HostChecker
{
    internal class HostHealthEntry
    {

        public HealthCheckResult? data { get; set; }
        public string description { get { return data is null ? "Unhealthy" : data.Value.Status.ToString(); } }

        public HostHealthEntry()
        {
            data = null;
        }
    }
}
