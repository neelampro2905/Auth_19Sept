using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SampleAPIRepository;

namespace SampleCoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();
            try
            {
                BuildWebHost(args).Run();
            }
            catch (Exception exception) 
            {
                // This will not be executed
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var webHostBuilder = CreateWebHostBuilder(args);
            // ConfigureKestrel(webHostBuilder);

            var webHost = webHostBuilder.Build();
            UpdateDatabase(webHost.Services);

            return webHost;
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbc = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbc.Database.Migrate();
                }
            }
            catch (Exception exception)
            {
                return;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        // .ConfigureLogging(x => x.AddApplicationInsights())
        .ConfigureAppConfiguration((ctx, builder) =>
        {
            //var keyVaultEndpoint = builder.Build()["KeyVaultEndPoint"];

            //        // If flag set to false, make sure you provide a connection string in appsetting.json for DatabaseConnectionString
            //        // var useAzureKeyVault = Convert.ToBoolean(builder.Build()["UseAzureKeyVaultFlag"]);

            //if (!string.IsNullOrEmpty(keyVaultEndpoint)
            //    && Uri.IsWellFormedUriString(keyVaultEndpoint, UriKind.Absolute)
            //    && useAzureKeyVault)
            //{
            //    //var azureServiceTokenProvider = new AzureServiceTokenProvider();
            //    //var keyVaultClient = new KeyVaultClient(
            //    //    new KeyVaultClient.AuthenticationCallback(
            //    //        azureServiceTokenProvider.KeyVaultTokenCallback));
            //    //builder.AddAzureKeyVault(
            //    //    keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
            //}
        })
        .UseStartup<Startup>();



        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
