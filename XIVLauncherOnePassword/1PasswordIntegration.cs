using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace XIVLauncherOnePassword
{
    public static partial class OPIntegration
    {
        public static string? GetAPIKey()
        {
            var vaultID = Environment.GetEnvironmentVariable("OP_PERSONAL_VAULT");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "op",
                    Arguments = $"read \"op://{vaultID}/Final Fantasy XIV/one-time password?attribute=otp",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            try
            {
                process.Start();
            }
            catch (Win32Exception)
            {
                return null;
            }

            var apiKey = process.StandardOutput.ReadLine();
            if (apiKey == null)
            {
                return null;
            }

            if (!IsValidOTP(apiKey))
            {
                return null;
            }

            return apiKey;
        }

        private static bool IsValidOTP(string secretKey)
        {
            return OTPRegex().IsMatch(secretKey);
        }

        [GeneratedRegex(@"^[0-9]{1,6}$")]
        private static partial Regex OTPRegex();
    }
}
