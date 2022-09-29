using CactusDepot.HostChecker;
using CactusDepot.Shared;


SharedUtil.WriteLogToConsole("HostChecker", "starting...");

string path = "http://avlad.no-ip.info:9091/Health";
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HOST_NAME", EnvironmentVariableTarget.Process)))
{
    path = Environment.GetEnvironmentVariable("HOST_NAME", EnvironmentVariableTarget.Process);
}

int pullInterval;
int.TryParse(Environment.GetEnvironmentVariable("PULL_INTERVAL", EnvironmentVariableTarget.Process), out pullInterval);

if (pullInterval <= 0)
{
    pullInterval = 5;
}

System.Timers.Timer timer = new();
timer.Enabled = true;
timer.Interval = pullInterval * 1000;//msec
timer.Elapsed += async (sender, e) => await TimerCallback();

timer.Start();

Console.Write("Press any key to exit... \n");
Console.Read();

async Task TimerCallback()
{
    bool StatusOK;
    timer.Stop();
    try
    {
        StatusOK = await ApiCli.CheckHosts(path);
        SharedUtil.WriteLogToConsole("HostChecker", string.Format("runing: {0} status {1}", DateTime.Now.ToString(), StatusOK ? "Healthy" : "Unhealthy"));
    }
    finally
    {
        timer.Start();
    }
}








