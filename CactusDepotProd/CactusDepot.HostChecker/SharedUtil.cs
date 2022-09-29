namespace CactusDepot.Shared
{
    public static class SharedUtil
    {
        public static void WriteLogToConsole(string eventType, string msg)
        {
            Console.WriteLine($"{DateTime.Now.ToLocalTime()} [{eventType}]: {msg}");
        }

    }
}
