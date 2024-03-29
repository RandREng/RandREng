using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Configuration;
using MimeKit;
using RandREng.Utility.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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
            public string SmtpFrom { get; set; }
            public string ImapServer { get; set; }
            public int ImapPort { get; set; } = 993;
            public bool ImapUseSsl { get; set; }
            public string ImapAccount { get; set; }
            public string ImapPassword { get; set; }
            public string ImapTo { get; set; }
        }

        private IConfiguration configuration { get; set; }
        private Settings settings { get; set; }

//        private readonly ITestOutputHelper output;

        public MailerTests(ITestOutputHelper output)
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddUserSecrets<Settings>()
                .Build();

//            this.output = output;
            settings = configuration.GetSection("MailSettings").Get<Settings>();

        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MailerDefaults()
        {
            Mailer mailer = new(settings.SmtpServer);

            Assert.Equal(25, mailer.Port);
            Assert.Equal(settings.SmtpServer, mailer.SMTPServer);
            Assert.False(mailer.UseSSL);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MailerOverrides()
        {
            Mailer mailer = new(settings.SmtpServer, 100, true);

            Assert.Equal(100, mailer.Port);
            Assert.Equal(settings.SmtpServer, mailer.SMTPServer);
            Assert.True(mailer.UseSSL);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void MailerOverrides2()
        {
            Mailer mailer = new(settings.SmtpServer, useSSL: settings.SmtpUseSsl);

            Assert.Equal(25, mailer.Port);
            Assert.Equal(settings.SmtpServer, mailer.SMTPServer);
            Assert.True(mailer.UseSSL);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task SendMail1()
        {
            Mailer mailer = new(settings.SmtpServer, settings.SmtpPort, useSSL: settings.SmtpUseSsl);

            string subject = $"Test{Guid.NewGuid()}";
            string body = "Test Body";
            bool highpriority = false;
            string attachment = null;
            string from = settings.SmtpFrom;
            string friendly = "fred";
            string account = settings.SmtpAccount;
            string password = settings.SmtpPassword;

            List<string> to = new() { settings.ImapTo };

            (bool ok, string errors) result = await mailer.SendMailAsync(subject, body, highpriority, attachment, to, from, friendly, account, password);

            Assert.True(result.ok);
            Assert.True(string.IsNullOrEmpty(result.errors));

            MimeMessage message = await GetMail(subject);

            Assert.NotNull(message);
            Assert.Equal(body, message.TextBody.Trim());

        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task SendMail2()
        {
            string subject = $"Test{Guid.NewGuid()}";
            string body = "Test Body";
            bool highpriority = false;
            string attachment = null;
            string from = settings.SmtpFrom;
            string friendly = "fred";
            string account = settings.SmtpAccount;
            string password = settings.SmtpPassword;

            List<string> to = new() { settings.ImapTo };

            Mailer mailer = new(settings.SmtpServer, settings.SmtpPort, useSSL: settings.SmtpUseSsl);
            mailer.ToAddresses = to;
            mailer.FromAddress = from;
            mailer.FromFriendlyName = friendly;
            mailer.AuthAccount = account;
            mailer.AuthPasswrd = password;

            (bool ok, string errors) result = await mailer.SendMailAsync(subject, body, highpriority, attachment);

            Assert.True(result.ok);
            Assert.True(string.IsNullOrEmpty(result.errors));

            MimeMessage message = await GetMail(subject);

            Assert.NotNull(message);
            Assert.Equal(body, message.TextBody.Trim());

        }
        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetMailTest()
        {
            MimeMessage message = await GetMail("");
        }

        private async Task<MimeMessage> GetMail(string subject, int maxDelay = 20)
        {
            MimeMessage message = null;
            using (ImapClient client = new())
            {
                await client.ConnectAsync(settings.ImapServer, settings.ImapPort, settings.ImapUseSsl);
                await client.AuthenticateAsync(settings.ImapAccount, settings.ImapPassword);
                DateTime start = DateTime.Now;

                while ((null == message) && ((DateTime.Now - start).TotalSeconds < maxDelay))
                {

                    try
                    {

                        // The Inbox folder is always available on all IMAP servers...
                        IMailFolder inbox = client.Inbox;
                        await inbox.OpenAsync(FolderAccess.ReadWrite);

                        for (int i = 0; i < inbox.Count; i++)
                        {

                            MimeKit.MimeMessage temp = await inbox.GetMessageAsync(i);
                            if (temp.Subject == subject)
                            {
                                await inbox.SetFlagsAsync(i, MessageFlags.Deleted, true);
                                await inbox.ExpungeAsync();
                                message = temp;
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return message;
        }
    }
}
