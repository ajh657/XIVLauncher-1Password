using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace XIVLauncherOnePassword;

public class Program
{
    private static async Task Main()
    {
        var opt = await OPIntegration.GetAPIKey();
        
        if (opt != null)
        {
            const string baseAddress = "http://localhost:4646/ffxivlauncher";
            using var client = new HttpClient();
            
            try
            {
                await client.GetAsync(baseAddress);
            }
            catch (HttpRequestException e) when (e.HttpRequestError == HttpRequestError.ConnectionError)
            {
                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XIVLauncher", "XIVLauncher.exe"));
            }
            
            bool serverStarted;
            
            do
            {
                try
                {
                    await client.GetAsync(baseAddress);
                    serverStarted = true;
                }
                catch (HttpRequestException e) when (e.HttpRequestError == HttpRequestError.ConnectionError)
                {
                    serverStarted = false;
                }
            } while (!serverStarted);
            
            await client.GetAsync($"{baseAddress}/{opt}");
        }
    }
}
