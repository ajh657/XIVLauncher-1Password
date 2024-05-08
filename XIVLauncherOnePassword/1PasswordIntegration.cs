using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Exceptions;

namespace XIVLauncherOnePassword;

public static partial class OPIntegration
{
    public static async Task<string?> GetAPIKey()
    {
        var variables = Util.GetEnviromentVariables();
        var cmd = Cli.Wrap("op").WithArguments($"read \"op://{variables["OP_PERSONAL_VAULT"]}/Final Fantasy XIV/one-time password?attribute=otp\" -n");
        cmd = cmd.WithEnvironmentVariables(variables);
        
        bool isSuccess;
        var output = new StringBuilder();
        
        if (!await ProgramCheck.ProgramExists("op"))
        {
            return null;
        }
        
        do
        {
            try
            {
                output = new StringBuilder();
                await cmd.WithStandardOutputPipe(PipeTarget.ToStringBuilder(output)).ExecuteAsync();
                isSuccess = true;
            }
            catch (CommandExecutionException)
            {
                await Task.Delay(500);
                isSuccess = false;
            }
        } while (!isSuccess);
        
        var apiKey = output.ToString();
        
        return !IsValidOTP(apiKey) ? null : apiKey;
    }
    
    private static bool IsValidOTP(string secretKey)
    {
        return OTPRegex().IsMatch(secretKey);
    }
    
    [GeneratedRegex(@"^[0-9]{1,6}$")]
    private static partial Regex OTPRegex();
}
