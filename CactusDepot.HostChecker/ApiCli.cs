using Newtonsoft.Json;

namespace CactusDepot.HostChecker
{
	internal static class ApiCli
	{
		static HttpClient client = new HttpClient();
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
                //parsing test responces below
                
				//string temp = "{\"status\":\"Healthy\",\"totalDuration\":\"00:00:00.0009768\",\"entries\":{\"mysql\":{\"data\":{},"+
				//	          "\"duration\":\"00:00:00.0008578\",\"status\":\"Healthy\",\"tags\":[]},\"HealthcheckSrv\":{\"data\":{},"+
				//			  "\"description\":\"A healthy result.\",\"duration\":\"00:00:00.0000045\",\"status\":\"Healthy\",\"tags\":[\"basicsrvcheck\"]}}}";

                string temp1 = "{\"status\":\"Healthy\",\"totalDuration\":\"00:00:00.0009768\",\"entries\":\"\"}";
                HostHealthResponse? respTemt = JsonConvert.DeserializeObject<HostHealthResponse>(temp1);
                if ((respTemt is not null) && (!string.IsNullOrEmpty(temp1)))
				{
					HostHealthResponse? resp = JsonConvert.DeserializeObject<HostHealthResponse>(temp1);
					if (resp is not null)
					{
						res = resp.status == "Healthy";
					}
                }
                //parsing below throws exception in json
                /*
                if ((rawResp is not null) && (!string.IsNullOrEmpty(rawResp)))
				{
					HostHealthResponse? resp = JsonConvert.DeserializeObject<HostHealthResponse>(rawResp);
					if (resp is not null)
					{
						res = resp.status == "Healthy";
					}
                }
				*/
            }
            catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return res;
		}
	}
}
