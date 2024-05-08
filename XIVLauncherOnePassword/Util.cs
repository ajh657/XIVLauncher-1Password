using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XIVLauncherOnePassword;

public static class Util
{
    public static Dictionary<string, string?> GetEnviromentVariables()
    {
        return Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process)
            .Cast<DictionaryEntry>()
            .Select(x => new KeyValuePair<string, string?>(x.Key.ToString() ?? string.Empty, x.Value?.ToString()))
            .Where(x => x.Value != null)
            .ToDictionary();
    }
}
