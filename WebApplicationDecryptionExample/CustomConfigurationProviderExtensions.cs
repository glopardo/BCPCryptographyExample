using Microsoft.Extensions.Configuration;

namespace BCP.WebApplicationNCTest
{
    public static class CustomConfigurationProviderExtensions
    {
        public static IConfigurationBuilder AddCustomProvider(this IConfigurationBuilder builder)
        {
            return builder.Add(new CustomConfigurationProvider());
        }
    }
}