using System;

namespace PopGmail.ConsoleApp.Models
{
    public class EmailAccount
    {
        public String HostOrIPAddress { get; set; }
        public Int32 Port { get; set; }
        public Boolean isSSL { get; set; }
        public String User { get; set; }
        public String Password { get; set; }
    }
}