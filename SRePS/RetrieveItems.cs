using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SRePS
{
    class RetrieveItems
    {
        private List<StockItems> _itemsList = new List<StockItems>();

        public RetrieveItems()
        {
            StreamReader reader;
            FileStream fs = new FileStream(@"items.txt", FileMode.Open, FileAccess.Read);
           // File.SetAttributes("salesorder.xml", FileAttributes.Normal);
            reader = new StreamReader(fs);
            try
            {
                while(!reader.EndOfStream)
                {
                    string[] iteminfo = reader.ReadLine().Split(' ');
                    StockItems newitem = new StockItems(iteminfo[0], Convert.ToDouble(iteminfo[1]));
                    _itemsList.Add(newitem);
                }
            }
            finally
            {
                reader.Dispose();
            }
            
        }

        public List<StockItems> getList()
        {
            return _itemsList;
        }
    }

    class StockItems
    {
        public string item_name;
        public double item_price;

        public StockItems(string name, double price)
        {
            item_name = name;
            item_price = price;
        }
    }
}
