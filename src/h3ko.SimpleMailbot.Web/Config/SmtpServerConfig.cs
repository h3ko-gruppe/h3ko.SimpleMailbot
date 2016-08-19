namespace h3ko.SimpleMailbot.Web.Config
{
    public class SmtpServerConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseTls { get; set; }
        public string DefaultFrom { get; set; }
        public bool IsEnabled { get; set; }
       
    }
}
