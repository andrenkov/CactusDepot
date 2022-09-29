using CactusDepot.HostChecker;
using CactusDepot.Shared;
using System.Threading;
using System.Timers;


SharedUtil.WriteLogToConsole("HostChecker", "starting...");

System.Timers.Timer timer = new();
timer.Enabled = true;
timer.Interval = 5000;
timer.Elapsed += async (sender, e) => await TimerCallback();

timer.Start();

Console.Write("Press any key to exit... \n");
Console.ReadKey();

async Task TimerCallback()
{
    bool StatusOK;
    timer.Stop();
    try
    {
        StatusOK = await ApiCli.CheckHosts();
        SharedUtil.WriteLogToConsole("HostChecker", string.Format("runing: {0} status {1}", DateTime.Now.ToString(), StatusOK ? "Healthy" : "Unhealthy"));
    }
    finally
    {
        timer.Start();
    }
}








