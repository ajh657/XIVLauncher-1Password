using System.Net.Http;
using System.Threading.Tasks;

namespace XIVLauncherOnePassword
{
    internal class Program
    {
        private static async Task Main()
        {
            var opt = OPIntegration.GetAPIKey();

            if (opt != null)
            {
                using var client = new HttpClient();
                await client.GetAsync($"http://localhost:4646/ffxivlauncher/{opt}");
            }
        }
    }
}
