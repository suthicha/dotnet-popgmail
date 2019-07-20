using Newtonsoft.Json;
using PopGmail.ConsoleApp.Models;
using System;
using System.IO;

namespace PopGmail.ConsoleApp.Controllers
{
    public class EmailAccountController
    {
        public EmailAccount GetProfile()
        {
            try
            {
                var path = Path.Combine(Utils.AssemblyDirectory, "account.json");
                var jsonContent = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<EmailAccount>(jsonContent);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ERROR | " + ex.Message);
            }
            return null;
        }
    }
}