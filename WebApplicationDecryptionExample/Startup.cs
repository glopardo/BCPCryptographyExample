using System;
using System.Text;
using BCP.Security;
using BCP.WebApplicationNCTest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplicationDecryptionExample
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddCustomProvider()
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

            _configuration = builder.Build();

            var connStr1 = _configuration.GetConnectionString("FirstDatabase");
            var connStr2 = _configuration.GetConnectionString("SecondDatabase");
            var connStr3 = _configuration.GetConnectionString("ThirdDatabase");

            var decryptedConnStr1 = Encoding.UTF8.GetString(CriptographyUtils.Decrypt(Convert.FromBase64String(connStr1), _configuration["CertificateThumbPrint"]));
            var decryptedConnStr2 = Encoding.UTF8.GetString(CriptographyUtils.Decrypt(Convert.FromBase64String(connStr2), _configuration["CertificateThumbPrint"]));
            var decryptedConnStr3 = Encoding.UTF8.GetString(CriptographyUtils.Decrypt(Convert.FromBase64String(connStr3), _configuration["CertificateThumbPrint"]));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync(
                    "<html><body>" +
                    $"<b>Connection string 1 encrypted</b>: {_configuration.GetConnectionString("FirstDatabase")}<br>" +
                    $"<b>Connection string 2 encrypted</b>: {_configuration.GetConnectionString("SecondDatabase")}<br>" +
                    $"<b>Connection string 3 encrypted</b>: {_configuration.GetConnectionString("ThirdDatabase")}<br><br>" +
                    $"<b>Connection string 1 decrypted</b>: {Encoding.UTF8.GetString(CriptographyUtils.Decrypt(Convert.FromBase64String(_configuration.GetConnectionString("FirstDatabase")), _configuration["CertificateThumbPrint"]))}<br>" +
                    $"<b>Connection string 2 decrypted</b>: {Encoding.UTF8.GetString(CriptographyUtils.Decrypt(Convert.FromBase64String(_configuration.GetConnectionString("SecondDatabase")), _configuration["CertificateThumbPrint"]))}<br>" +
                    $"<b>Connection string 3 decrypted</b>: {Encoding.UTF8.GetString(CriptographyUtils.Decrypt(Convert.FromBase64String(_configuration.GetConnectionString("ThirdDatabase")), _configuration["CertificateThumbPrint"]))}" +
                    "</body></html>");
            });
        }
    }
}
