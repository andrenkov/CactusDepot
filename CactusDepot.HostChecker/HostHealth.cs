using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CactusDepot.HostChecker
{
    internal class HostHealthEntry
    {
        public string data { get; set; }
        public string description { get; set; }
        public DateTime duration { get; set; }
        public string exception { get; set; }
        public string status { get; set; }
        public List<string> tags { get; set; }

        public HostHealthEntry()
        {
            data = "";
            description = "";
            exception = "";
            status = "";
            tags = new List<string>();
        }
    }

    internal class HostHealthResponse
    {
        public string status { get; set; }
        public DateTime totalDuration { get; set; }
        //public List<HostHealthEntry> entries { get; set; }
        public string entries { get; set; }

        public HostHealthResponse()
        {
            status = "";
            entries = "";
            //entries = new List<HostHealthEntry>();
        }

        /*
    {"status":"Unhealthy",
        "totalDuration":"00:00:05.0019643",
        "entries":
        {
            "mysql":
                {
                  "data":{},
                  "description":"Connect Timeout expired.",
                  "duration":"00:00:05.0018461",
                  "exception":"Connect Timeout expired.",
                  "status":"Unhealthy",
                  "tags":[]
                },
            "HealthcheckSrv":
                {
                "data":{},
                "description":"A healthy result.",
                "duration":"00:00:00.0000054",
                "status":"Healthy",
                "tags":["basicsrvcheck"]}
            }
        }
        */
    }
}
