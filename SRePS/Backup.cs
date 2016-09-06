using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SRePS
{
    class Backup
    {
        public void MakeBackup()
        {
            StreamReader reader;
            FileStream salesOrder = new FileStream(@"salesorder.xml", FileMode.Open, FileAccess.Read);

            string temp = "";

            reader = new StreamReader(salesOrder);
            try
            {
                while (!reader.EndOfStream)
                {
                    temp += reader.ReadLine();
                    temp += "\n";
                }
            }

            finally
            {
                reader.Dispose();
            }

            StreamWriter writer;
            FileStream backup = new FileStream(@"backup.xml", FileMode.Create, FileAccess.Write);
            writer = new StreamWriter(backup);

            try
            {
                writer.Write(temp);
            }

            finally
            {
                writer.Dispose();
            }
        }
    }
}
