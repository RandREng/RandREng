﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RandREng.Utility.Logger;
using RandREng.Utility.AppSettings;
using System.Threading.Tasks;

namespace RandREng.Utility.Mail
{
    public class Mailer
    {
        private readonly ILogger _logger;

        public enum EnEmailAddressStatusCode { Valid, Blank, IncorrectFormat };

        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string AuthAccount { get; set; }
        public string AuthPasswrd { get; set; }

        public string FromAddress { get; set; }
        public string FromFriendlyName { get; set; }

        private List<string> _toAddresses;
        public List<string> ToAddresses
        {
            get
            {
                if (_toAddresses == null)
                {
                    _toAddresses = new List<string>();
                }
                return _toAddresses;
            }

            set
            {
                _toAddresses = value;
            }
        }

        private List<string> _ccAddresses;
        public List<string> CCAddresses
        {
            get
            {
                if (_ccAddresses == null)
                {
                    _ccAddresses = new List<string>();
                }
                return _ccAddresses;
            }

            set
            {
                _ccAddresses = value;
            }
        }

        public bool BodyIsHtml { get; set; }
        private SmtpClient _client;
        public SmtpClient Client
        {
            get
            {
                if (_client == null)
                {
                    if (!string.IsNullOrEmpty(SMTPServer))
                    {
                        _client = new SmtpClient(SMTPServer, Port)
                        {
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            EnableSsl = UseSSL
                        };
                    }
                }
                return _client;
            }

            set
            {
                _client = value;
            }
        }

        public Mailer(ILogger logger)
        {
            SMTPServer = RandREng.Utility.AppSettings.AppSettings.GetAppSetting("SMTPServer", "");
            Port = RandREng.Utility.AppSettings.AppSettings.GetAppSetting("SMTPServerPort", 25);
            UseSSL = RandREng.Utility.AppSettings.AppSettings.GetAppSetting("UseSSL", false);
            AuthAccount = RandREng.Utility.AppSettings.AppSettings.GetAppSetting("AuthAccount", "");
            AuthPasswrd = RandREng.Utility.AppSettings.AppSettings.GetAppSetting("AuthPasswrd", "");

            string ToAddress = RandREng.Utility.AppSettings.AppSettings.GetAppSetting("ToAddress", "");
            if (IsValidEmail(ToAddress))
            {
                _logger.Log(LogLevel.Debug, string.Format("Adding address: {0}", ToAddress));
                ToAddresses.Add(ToAddress);
            }
            FromAddress = RandREng.Utility.AppSettings.AppSettings.GetAppSetting("FromAddress", "");
            FromFriendlyName = RandREng.Utility.AppSettings.AppSettings.GetAppSetting("FromFriendlyName", "");

            _logger = logger;
            BodyIsHtml = false;
        }

        public Mailer(string server, int port = 25, bool useSSL = false) : this(server, port, useSSL, NullLogger.Instance)
        {
        }

        public Mailer(string server, int port, bool useSSL, ILogger logger)
        {
            SMTPServer = server;
            BodyIsHtml = false;
            Port = port;
            UseSSL = useSSL;
            _logger = logger;
        }

        public async Task<(bool, string)> SendMailAsync(string Subject, string Body)
        {
            return await SendMailAsync(Subject, Body, false);
        }

        public async Task<(bool, string)> SendMailAsync(string Subject, string Body, bool HighPriority)
        {
            return await SendMailAsync(Subject, Body, HighPriority, "");
        }

        public async Task<(bool, string)> SendMailAsync(string Subject, string Body, bool HighPriority, string Attachment)
        {
            return await SendMailAsync(Subject, Body, HighPriority, Attachment, ToAddresses, FromAddress, FromFriendlyName);
        }

        public async Task<(bool, string)> SendMailAsync(string Subject, string Body, bool HighPriority, string Attachment, string To, string From, string DisplayName)
        {
            if (string.IsNullOrWhiteSpace(To))
            {
                string error = $"Cannot add an empty To address to email with subject: {Subject}"; 
                _logger.Log(LogLevel.Error, error);
                return (false, error);
            }
            ToAddresses.Clear();  // need to clear any existing addresses that may have been added
            _logger.Log(LogLevel.Debug, string.Format("Adding address: {0}", To));
            ToAddresses.Add(To);
            return await SendMailAsync(Subject, Body, HighPriority, Attachment, ToAddresses, From, DisplayName, AuthAccount, AuthPasswrd);
        }

        public async Task<(bool, string)> SendMailAsync(string Subject, string Body, bool HighPriority, string Attachment, List<string> ToAddresses, string From, string DisplayName)
        {
            return await SendMailAsync(Subject, Body, HighPriority, Attachment, ToAddresses, From, DisplayName, AuthAccount, AuthPasswrd);
        }

        public async Task<(bool, string)> SendMailAsync(string Subject, string Body, bool HighPriority, string Attachment, string To, string CC, string From, string DisplayName, string Account, string Password)
        {
            if (string.IsNullOrWhiteSpace(To))
            {
                _logger.Log(LogLevel.Error, string.Format("Cannot add an empty To address to email with subject: {0}", Subject));
                return (false, "Null to");
            }
            ToAddresses.Clear();  // need to clear any existing addresses that may have been added
            ToAddresses.Add(To);

            CCAddresses.Clear();
            CCAddresses.Add(CC);

            return await SendMailAsync(Subject, Body, HighPriority, Attachment, ToAddresses, From, DisplayName, Account, Password);
        }

        public async Task<(bool, string)> SendMailAsync(string Subject, string Body, bool HighPriority, string Attachment, List<string> ToAddresses, string From, string DisplayName, string Account, string Password)
        {
            string errors = "";
            bool retVal = false;
            try
            {
                if (ToAddresses.Count > 0 && !string.IsNullOrWhiteSpace(From) && !string.IsNullOrWhiteSpace(SMTPServer) &&
                    !string.IsNullOrWhiteSpace(Account) && !string.IsNullOrWhiteSpace(Password))
                {
                    MailMessage message = new();
                    Client.Credentials = new NetworkCredential(Account, Password);

                    try
                    {
                        message.From = new MailAddress(From, DisplayName);
                        message.ReplyToList.Add(message.From);
                    }
                    catch (Exception e)
                    {
                        errors = $"From - {From}";
                        _logger.LogCritical(e);
                        _logger.LogError(errors);
                        return (false, errors);
                    }

                    try
                    {
                        message.ReplyToList.Add(From);
                    }
                    catch (Exception e)
                    {
                        if (!string.IsNullOrEmpty(errors))
                        {
                            errors += Environment.NewLine;
                        }
                        errors += e.Message;
                        _logger.LogCritical(e);
                        _logger.LogError(string.Format("ReplyTo - {0}", From));
                        return (false, errors);
                    }
                    message.Body = Body;
                    message.To.Clear();  // need to clear any existing addresses that may have been added
                    foreach (string ToAddress in ToAddresses)
                    {
                        if (!string.IsNullOrWhiteSpace(ToAddress))
                        {
                            try
                            {
                                message.To.Add(ToAddress);
                            }
                            catch (Exception e)
                            {
                                if (!string.IsNullOrEmpty(errors))
                                {
                                    errors += Environment.NewLine;
                                }
                                errors += e.Message;
                                _logger.LogCritical(e);
                                _logger.LogError(string.Format("CC - {0}", ToAddress));
                                return (false, errors);
                            }
                        }
                        else
                        {
                            _logger.LogError(string.Format("Found an empty ToAddress when sending an email with the subject: '{0}", Subject));
                        }

                    }

                    message.CC.Clear();  // need to clear any existing addresses that may have been added
                    foreach (string CCAddress in CCAddresses)
                    {
                        if (!string.IsNullOrWhiteSpace(CCAddress))
                        {
                            try
                            {
                                message.CC.Add(CCAddress);
                            }
                            catch (Exception e)
                            {
                                if (!string.IsNullOrEmpty(errors))
                                {
                                    errors += Environment.NewLine;
                                }
                                errors += e.Message;
                                _logger.LogCritical(e);
                                _logger.LogError(string.Format("CC - {0}", CCAddress));
                                return (false, errors);
                            }
                        }
                        else
                        {
                            _logger.LogError(string.Format("Found an empty CCAddress when sending an email with the subject: '{0}", Subject));
                        }

                    }

                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        message.Attachments.Add(new Attachment(Attachment, MediaTypeNames.Application.Octet));
                    }

                    message.Subject = Subject;
                    message.IsBodyHtml = BodyIsHtml;
                    message.Priority = HighPriority ? MailPriority.High : MailPriority.Normal;
                    try
                    {
                        await Client.SendMailAsync(message);
                        retVal = true;
                    }
                    catch (Exception ex)
                    {
                        string error = "SendMail - ";
                        foreach (string adr in ToAddresses)
                        {
                            error += adr + " - ";
                        }
                        error += string.Format(" From {0} - DispalyName {1} - Account {2} - Password {3}", From, DisplayName, Account, Password);
                        _logger.LogError(error);
                        if (!string.IsNullOrEmpty(errors))
                        {
                            errors += Environment.NewLine;
                        }
                        errors += ex.Message;
                        _logger.LogCritical(ex);
                    }
                }
                else
                {
                    errors = "SendMail() - ToAddress, FromAddress, or SMTPServer name was empty.";
                    _logger.Log(LogLevel.Error, errors);
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e);
            }
            return (retVal,errors);
        }

        public async Task<(bool, string)> SendMailAsync(MailMessage message, string Account, string Password)
        {
            string error = "";
            bool retVal = false;
            try
            {
                Client.Credentials = new NetworkCredential(Account, Password);

                await Client.SendMailAsync(message);
                retVal = true;
            }
            catch (Exception e)
            {
                _logger.LogError(message.ToString());
                error += e.Message;
                _logger.LogCritical(e);
            }
            return (retVal, error);
        }

        public static bool IsValidEmail(string emailAddress)
        {
            return IsValidEmail(emailAddress, out _);
        }

        public static bool IsValidEmail(string emailAddress, out EnEmailAddressStatusCode statusCode)
        {
            statusCode = EnEmailAddressStatusCode.Valid;

            if (string.IsNullOrEmpty(emailAddress))
            {
                statusCode = EnEmailAddressStatusCode.Blank;
                return false;
            }

            // Use IdnMapping class to convert Unicode domain names.
            emailAddress = Regex.Replace(emailAddress, @"(@)(.+)$", domainMapper);

            if (string.IsNullOrEmpty(emailAddress))
            {
                statusCode = EnEmailAddressStatusCode.IncorrectFormat;
                return false;
            }

            // Return true if emailAddress is in valid e-mail format.
            bool valid = Regex.IsMatch(emailAddress,
                                @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                                RegexOptions.IgnoreCase);

            if (!valid)
            {
                statusCode = EnEmailAddressStatusCode.IncorrectFormat;
            }

            return valid;
        }

        private static string domainMapper(Match match)
        {

            // IdnMapping class with default property values.
            IdnMapping idn = new();

            string domainName = match.Groups[2].Value;

            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                return string.Empty;
            }

            return match.Groups[1].Value + domainName;
        }
    }
}
