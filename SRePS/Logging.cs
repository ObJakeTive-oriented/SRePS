using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace SRePS
{
    abstract class Logging
    {
        public abstract void Log(string loggingString);
    }

    class ErrorLogging : Logging
    {
        public override void Log(string errorString)
        {
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

    class UserLogging : Logging
    {
        public override void Log(string interactionString)
        {
            FileStream fs = new FileStream(@"UserInteractionLog.txt", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs);
            try
            {
                writer.WriteLine("User: " + Globals.currentUser);
                writer.WriteLine("Message: " + interactionString);
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