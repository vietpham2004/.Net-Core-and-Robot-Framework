using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DotNetCore_RobotFrameWork.Communicate.Email
{
    public static class EmailHelper
    {
        private static ImapClient _client = null;
        public static MimeMessage GetLatestMail(string mailServer, int port, bool ssl, string username, string password)
        {
            return GetAllMails(mailServer, port, ssl, username, password).OrderByDescending(f => f.Date).FirstOrDefault();
        }

        public static bool HasReceivedMail(string mailServer, int port, bool ssl, string username, string password, string sendFrom, string subject, string content, int timeoutInSecond = 30)
        {
            DateTime currDateTime = DateTime.Now;
            DateTime ToDateTime = currDateTime.AddSeconds(timeoutInSecond);
            if (currDateTime <= ToDateTime)
            {
                var result = GetAllMails(mailServer, port, ssl, username, password).Any(f => (f.Sender.Name.Contains(sendFrom) || f.Sender.Address.Contains(sendFrom)) && f.Subject.Contains(subject) && f.TextBody.Contains(content));
                if (result)
                    return true;
                Thread.Sleep(TimeSpan.FromSeconds(10));
                currDateTime = DateTime.Now;
            }
            return false;
        }

        public static IList<MimeMessage> GetAllMails(string mailServer, int port, bool ssl, string username, string password)
        {
            return GetMails(mailServer, port, ssl, username, password, SearchQuery.All);
        }

        public static IList<MimeMessage> GetUnreadMails(string mailServer, int port, bool ssl, string username, string password, string mailBox)
        {
            return GetMails(mailServer, port, ssl, username, password, SearchQuery.NotSeen);
        }

        private static IList<MimeMessage> GetMails(string mailServer, int port, bool ssl, string username, string password, SearchQuery searchQuery)
        {
            Login(mailServer, port, ssl, username, password);

            _client.Inbox.Open(MailKit.FolderAccess.ReadOnly);
            var uids = _client.Inbox.Search(searchQuery);
            IList<MimeMessage> result = new List<MimeMessage>();
            foreach (var uid in uids)
            {
                var message = _client.Inbox.GetMessage(uid);
                result.Add(message);
            }

            return result;
        }

        private static void Login(string mailServer, int port, bool ssl, string login, string password)
        {
            if (_client == null)
                _client = new ImapClient();

            if (ssl)
                _client.Connect(mailServer, port, true);
            else
                _client.Connect(mailServer, port);
            _client.Authenticate(login, password);
        }
    }
}
