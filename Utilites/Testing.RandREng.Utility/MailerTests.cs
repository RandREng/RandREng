using Microsoft.Extensions.Configuration;
using Xunit;

namespace Testing.RandREng.Utilities
{
    public class MailerTests
    {
        public class Settings
        {
            public string SmtpServer { get; set; }
            public int SmtpPort { get; set; } = 25;
            public bool SmtpUseSsl { get; set; }
            public string SmtpAccount { get; set; }
            public string SmtpPassword { get; set; }
            public string ImapServer { get; set; }
            public int ImapPort { get; set; } = 993;
            public bool ImapUseSsl { get; set; }
            public string ImapAccount { get; set; }
            public string ImapPassword { get; set; }
        }

        private IConfiguration configuration { get; set; }

        public MailerTests()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddUserSecrets<Settings>()
                .Build();

            var temp = configuration.GetSection("MailSettings").Get<Settings>();

        }

        [Fact]
        public void Test1()
        {
            string temp = configuration["SmtpServer"];
            string t2 = configuration["SmtpPort"];
        }
    }
}
