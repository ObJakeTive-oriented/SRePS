using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRePS
{
    class SalesOrder
    {
        private List<SalesOrderInfo> salesOrderList = new List<SalesOrderInfo>();

        private int NumberOfSalesOrders()
        {
            int count = 0;
            foreach(SalesOrderInfo s in salesOrderList)
            {
                count++;
            }
            return count;
        }

        public int NewSalesOrderId()
        {
            int count = NumberOfSalesOrders();
            return (count+1);
        }
        
    }

    public class SalesOrderInfo
    {
        public int id;
        public string date;
        public List<Items> items = new List<Items>();

        public void AddItem(string item, int quantity)
        {
            Items _item = new Items(item, quantity);
            items.Add(_item);
        }
        
    }

    public class Items
    {
        public string item_name;
        public int item_quantity;

        public Items(string item, int quantity)
        {
            item_name = item;
            item_quantity = quantity;
        }
    }
    
}
