using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using PopGmail.ConsoleApp.Models;
using System;
using System.IO;

namespace PopGmail.ConsoleApp.Controllers
{
    public interface IGmailable
    {
        void FetchAttachment();
    }

    public class GmailController : IGmailable
    {
        private EmailAccount _emailAccount;

        public GmailController(EmailAccount emailAccount)
        {
            _emailAccount = emailAccount;
        }

        public void FetchAttachment()
        {
            if (_emailAccount == null)
            {
                Utils.ConsoleWriteError("FIND NOT FOUND EMAIL ACCOUNT.");
                return;
            }

            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_emailAccount.HostOrIPAddress, _emailAccount.Port, _emailAccount.isSSL);
                client.Authenticate(_emailAccount.User, _emailAccount.Password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);

                DateTime today = DateTime.Now;
                DateTime scopeDate = today.AddDays(-1);

                // Found message in scope date and not read.
                var query = SearchQuery.DeliveredAfter(scopeDate).And(SearchQuery.NotSeen);

                foreach (var uid in inbox.Search(query))
                {
                    var message = inbox.GetMessage(uid);
                    var senderAddress = string.Empty;

                    foreach (var mailbox in message.From.Mailboxes)
                    {
                        senderAddress = mailbox.Address;
                    }

                    Console.WriteLine(string.Format("SENDER{0}{0}: {1}{0}{2}", Utils.Tab, senderAddress, message.Subject));

                    if (senderAddress == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(string.Format("ERROR{0}{0}: FIND NOT FOUND SENDER", Utils.Tab));
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }

                    foreach (MimeEntity attachment in message.Attachments)
                    {
                        var senderPath = Utils.InitPath(senderAddress);
                        Console.WriteLine(string.Format("ATTACHMENT{0}: {1}", Utils.Tab, attachment.ContentType.Name));

                        var attachmentFileName = Path.Combine(senderPath, attachment.ContentType.Name);
                        var conAttachmentFileName = string.Format("WRITE{0}{0}: {1}", Utils.Tab, attachmentFileName);

                        Console.WriteLine(conAttachmentFileName);
                        Logger.WriteLog(conAttachmentFileName);

                        try
                        {
                            using (var stream = File.Create(attachmentFileName))
                            {
                                if (attachment is MessagePart)
                                {
                                    var rfc822 = (MessagePart)attachment;

                                    rfc822.Message.WriteTo(stream);
                                }
                                else
                                {
                                    var part = (MimePart)attachment;

                                    part.Content.DecodeTo(stream);
                                }
                            }

                            Console.WriteLine(string.Format("WRTIE{0}{0}: OK", Utils.Tab));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("WRTIE{0}{0}: ERROR | {1}", Utils.Tab, ex.Message));
                        }
                    }

                    // The message have been read.
                    inbox.AddFlags(uid, MessageFlags.Seen, true);
                }

                client.Disconnect(true);
            }
        }
    }
}