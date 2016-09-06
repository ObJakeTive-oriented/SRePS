using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace SRePS
{
    class ErrorLogging
    {
        public void LogError(string errorString)
        {
            string dir = @Directory.GetCurrentDirectory()+"\\ErrorLogging.txt";
            FileStream fs = new FileStream(@"Error.txt", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs);
            try
            {
                writer.WriteLine("Message: " + errorString);
                writer.WriteLine("Date: " + DateTime.Now.ToString());
                writer.WriteLine("-----------------------------------------------------------------------------" + Environment.NewLine);
            }
            finally
            {
                writer.Dispose();
            }
        }
    }
}