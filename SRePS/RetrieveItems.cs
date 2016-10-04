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
                string[] iteminfo;
                while (!reader.EndOfStream)
                {
                        iteminfo = reader.ReadLine().Split(' ');
                        StockItems newitem = new StockItems
                        {
                            item_name = iteminfo[0].ToLower(),
                            item_price = Convert.ToDouble(iteminfo[1]),
                            item_stock = Convert.ToInt32(iteminfo[2]),
                            item_stock_threshold = Convert.ToInt32(iteminfo[3])
                        };
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

        public int ReturnStockLevel(string name)
        {
            foreach (var item in _itemsList)
            {
                if (name == item.item_name)
                {
                    return item.item_stock;
                }
            }
            return 0;
        }

        public void UpdateStock(string name, int sold)
        {
            foreach (var item in _itemsList)
            {
                if (name == item.item_name)
                {
                    item.item_stock = item.item_stock - sold;
                    return;
                }
            }
            return;
        }

        public int ReturnThresh(string name)
        {
            foreach (var item in _itemsList)
            {
                if (name == item.item_name)
                {
                    return item.item_stock_threshold;
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
            public int item_stock_threshold { get; set; }
            public int item_stock { get; set; }
        }