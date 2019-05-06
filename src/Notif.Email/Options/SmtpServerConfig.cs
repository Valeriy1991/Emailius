namespace Notif.Email.Options
{
    public class SmtpServerConfig
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string SenderName { get; set; }

        public string SenderAddress { get; set; }

        public bool IsSenderNameSubmitted => !string.IsNullOrWhiteSpace(SenderName);

        public bool IsSenderAddressSubmitted => !string.IsNullOrWhiteSpace(SenderAddress);
    }
}