using Microsoft.Extensions.Configuration;

namespace BCP.WebApplicationNCTest
{
    public class CustomConfigurationProvider : ConfigurationProvider, IConfigurationSource
    {
        public override void Load()
        {
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CustomConfigurationProvider();
        }
    }
}
