using PopGmail.ConsoleApp.Controllers;
using System;
using System.Threading;

namespace PopGmail.ConsoleApp
{
    internal class Program
    {
        private static string _title = "PopGmail v.1.0";

        private static void Main(string[] args)
        {
            Console.Title = _title;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(string.Format(@"{0}{0}{0}{0}{0}{0}{1}", Utils.Tab, _title));

            string lineText = Utils.LineText("_", Console.WindowWidth);
            Console.WriteLine(lineText);

            var objEmailAccountCTL = new EmailAccountController();
            var objEmailProfile = objEmailAccountCTL.GetProfile();

            if (objEmailProfile == null)
            {
                Utils.ConsoleWriteError("FIND NOT FOUND YOUR EMAIL SETTINGS.");
            }
            else
            {
                Console.WriteLine(string.Format("HostOrIPAddress{0}: {1}", Utils.Tab, objEmailProfile.HostOrIPAddress));
                Console.WriteLine(string.Format("Port{0}{0}: {1}", Utils.Tab, objEmailProfile.Port));
                Console.WriteLine(string.Format("isSSL{0}{0}: {1}", Utils.Tab, objEmailProfile.isSSL));
                Console.WriteLine(string.Format("EmailAddress{0}: {1}", Utils.Tab, objEmailProfile.User));

                Console.WriteLine("");
                Console.WriteLine(lineText);

                try
                {
                    IGmailable objGmailCTL = new GmailController(objEmailProfile);
                    objGmailCTL.FetchAttachment();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("ERROR{0}{0}: {1}", Utils.Tab, ex.Message));
                    Logger.WriteLog("ERROR " + ex.Message);
                }
            }

            Console.WriteLine(lineText);
            Console.WriteLine("DONE");

            Thread.Sleep(300);
        }
    }
}