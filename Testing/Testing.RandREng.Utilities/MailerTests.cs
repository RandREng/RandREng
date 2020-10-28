using Microsoft.Extensions.Configuration;
using RandREng.Utility.Mail;
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
        private Settings settings { get; set; }

        public MailerTests()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddUserSecrets<Settings>()
                .Build();

            settings = configuration.GetSection("MailSettings").Get<Settings>();

        }

        [Fact]
        public void MailerDefaults()
        {
            Mailer mailer = new Mailer(settings.SmtpServer);

            Assert.Equal(25, mailer.Port);
            Assert.Equal(settings.SmtpServer, mailer.SMTPServer);
            Assert.False(mailer.UseSSL);
        }

        [Fact]
        public void MailerOverrides()
        {
            Mailer mailer = new Mailer(settings.SmtpServer, 100, true);

            Assert.Equal(100, mailer.Port);
            Assert.Equal(settings.SmtpServer, mailer.SMTPServer);
            Assert.True(mailer.UseSSL);
        }
    }
}
