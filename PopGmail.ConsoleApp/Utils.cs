using System;
using System.IO;
using System.Reflection;

namespace PopGmail.ConsoleApp
{
    static public class Utils
    {
        static public void ConsoleWrite(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
        }

        static public void ConsoleWriteError(string errMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errMessage);
        }

        static public string Tab
        {
            get { return "\t"; }
        }

        static public string LineText(string text, int width)
        {
            var ret = string.Empty;

            for (int i = 0; i < width; i++)
            {
                ret = ret + text;
            }
            return ret;
        }

        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        static public string InitPath(string name)
        {
            string path = Path.Combine(AssemblyDirectory, "inbox", name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}