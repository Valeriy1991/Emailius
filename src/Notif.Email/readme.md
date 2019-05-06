

# Notif.Email

Email notifications.

## Getting started

1. Install packages:
    1. `Notif.Abstractions`
    2. `Notif.Email`
1. Register dependencies in your IOC-container:
    1. ServiceProvider (.NET Core):
   ```csharp
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
    services
        .AddTransient<INotificator<EmailMessage>, EmailNotificator>()
        .AddSingleton<ISmtpClientFactory, SmtpClientWithoutSslFactory>()
        .AddSingleton<IMessageFactory<MailMessageCreateRequest, MailMessage>, MailMessageFactory>();
   ```