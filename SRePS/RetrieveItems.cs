using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SRePS
{
    public class RetrieveItems
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
                while (!reader.EndOfStream)
                {
                    string[] iteminfo = reader.ReadLine().Split(' ');
                    StockItems newitem = new StockItems { item_name = iteminfo[0].ToLower(), item_price = Convert.ToDouble(iteminfo[1]) };
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

        public double ReturnPrice(string name)
        {
            foreach (var item in _itemsList)
            {
                if (name == item.item_name)
                {
                    return item.item_price;
                }
            }
            return 0;
        }
    }
}

        public class StockItems
        {
            public string item_name { get; set; }
            public double item_price { get; set; }
        }