

# Notif.Email tests

##### Before run integration tests

1. Download [MailHog](https://github.com/mailhog/MailHog) for your platform
2. Run MailHog on port 25, for example:
```powershell
.\MailHog_windows_386.exe -smtp-bind-addr="0.0.0.0:25"
```
3. Run integration tests