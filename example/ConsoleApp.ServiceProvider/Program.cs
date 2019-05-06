using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notif.Abstractions;
using Notif.Email;
using Notif.Email.Factories;
using Notif.Email.Ioc;
using Notif.Email.Options;
using Notif.Email.Smtp;
using Notif.Email.Smtp.Abstract;

namespace ConsoleApp.ServiceProvider
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            using (var host = hostBuilder.Build())
            {
                host.Start();
                await host.StopAsync();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var pathToContentRoot = AppDomain.CurrentDomain.BaseDirectory;

            return new HostBuilder()
                .UseEnvironment(aspNetCoreEnvironment)
                .UseContentRoot(pathToContentRoot)
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configApp.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json", optional: true,
                        reloadOnChange: true);
                    configApp.AddEnvironmentVariables();
                    configApp.AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.Configure<AppSettings>(hostContext.Configuration);

                    // Notif.Email configuration:
                    // 1. Register MailSettingsConfig:
                    services
                        .AddSingleton<MailSettingsConfig>(provider =>
                        {
                            var appSettingsOptions = provider.GetRequiredService<IOptions<AppSettings>>();
                            if (appSettingsOptions?.Value == null)
                                throw new ArgumentNullException(nameof(appSettingsOptions));

                            return appSettingsOptions.Value.MailSettings;
                        });
                    
                    // 2. Add dependencies from Notif.Email:
                    services.AddNotifEmail();
                    // or register all Notif.Email dependencies manually:  
                    //services
                    //    .AddTransient<INotificator<EmailMessage>, EmailNotificator>()
                    //    .AddSingleton<ISmtpClientFactory, SmtpClientWithoutSslFactory>()
                    //    .AddSingleton<IMessageFactory<MailMessageCreateRequest, MailMessage>, MailMessageFactory>();

                    // 3. Add our custom dependencies:
                    services.AddTransient<IMessageFactory<object, EmailMessage>, ExampleEmailMessageFactory>();
                    // That is all.

                    services.AddSingleton<IHostedService, SendEmailHostedService>();
                });
        }
    }
}
