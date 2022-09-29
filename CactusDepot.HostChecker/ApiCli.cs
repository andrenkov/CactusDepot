using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace CactusDepot.HostChecker
{
	internal static class ApiCli
	{
		static readonly HttpClient client = new();
		static async Task<string> GetHealthAsync(string path)
		{
			string health = "";
			HttpResponseMessage response = await client.GetAsync(path);
			if (response.IsSuccessStatusCode)
			{
				health = await response.Content.ReadAsStringAsync();
			}
			return health;
		}

		public static async Task<bool> CheckHosts()
		{
			bool res = false;

			try
			{
				string? rawResp = await GetHealthAsync("http://avlad.no-ip.info:9091/Health");
				
				if ((rawResp is not null) && (!string.IsNullOrEmpty(rawResp)))
				{
					HostHealthEntry? resp = new()
					{
						data = JsonConvert.DeserializeObject<HealthCheckResult>(rawResp)
					};
					if (resp is not null && resp.data is not null)
					{
						res = resp.data.Value.Status == HealthStatus.Healthy;
                    }
				}
				
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return res;
		}
	}
}
