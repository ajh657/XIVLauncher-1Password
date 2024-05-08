using System;
using System.Threading.Tasks;
using CliWrap;

namespace XIVLauncherOnePassword;

public static class ProgramCheck
{
    public static async Task<bool> ProgramExists(string program)
    {
        try
        {
            var variables = Util.GetEnviromentVariables();
            await Cli.Wrap(program).WithEnvironmentVariables(variables).WithValidation(CommandResultValidation.None).ExecuteAsync();
            
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
