using System.Collections;

namespace Amatsucozy.PMS.Email.API;

public static class Extensions
{
    internal static void AddFlatConfigurations<TConfig>(
        this IConfiguration configuration,
        IDictionary arguments
    ) where TConfig : class
    {
        var configType = typeof(TConfig);
        var sectionName = configType.Name;

        foreach (var property in configType.GetProperties())
        {
            configuration[$"{sectionName}:{property.Name}"] = arguments[$"{sectionName}__{property.Name}"]?.ToString();
        }
    }
}