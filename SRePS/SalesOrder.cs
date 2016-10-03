using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace SRePS
{
    public class SalesOrder
    {
        private List<SalesOrderInfo> salesOrderList = new List<SalesOrderInfo>();

        public SalesOrder()
        {
            loadSalesOrders();
        }

        public List<SalesOrderInfo> getSoList
        {
            get
            {
                return salesOrderList;
            }
            set
            {
                salesOrderList = value;
            }
        }
    
        public string SaveSalesOrders()
        {
            string notification = "";

            string output = "<?xml version=\"1.0\"?>\n<salesorderid>\n";
            foreach (SalesOrderInfo so in MainScreen.salesOrderList)
            {
                output += "<salesorderid salesorderid=\"" + so.id + "\">\n";
                output += "<user>"+so.user+"</user>\n<date>"+so.date+"</date>\n";
                int itemid = 0;

                foreach(Items item in so.items)
                {
                    itemid++;
                    output += "<item item=\"" + itemid.ToString() + "\">\n";
                    output += "<item>" + item.item_name + "</item>\n";
                    //output += "<price>" + MainScreen.stockItemsList.(itemid.ToString()).ToString();
                    output += "<quantity>" + item.item_quantity + "</quantity>\n";
                    output += "</item>\n";
                }

                foreach(Items item in so.items)
                {
                    StreamWriter writer;
                    FileStream fs = new FileStream(@"items.txt", FileMode.Open, FileAccess.Write);
                    // File.SetAttributes("salesorder.xml", FileAttributes.Normal);
                    writer = new StreamWriter(fs);

                    foreach (StockItems s in MainScreen.stockItemsList)
                    {
                        if (s.item_name == item.item_name)
                        {
                            if(s.item_stock - Convert.ToInt32(item.item_quantity) < 0)
                            {
                                //ALERT USER NOT THAT THERE IS NOT ENOUGH STOCK FOR CURRENT SALE ORDER
                                notification += "NOT ENOUGH " + s.item_name + " IN STOCK FOR SALE. Current stock level for " + s.item_name + ": " + s.item_stock + "\n";
                                writer.Dispose();
                                return notification;
                            }

                            s.item_stock -= Convert.ToInt32(item.item_quantity);
                            writer.WriteLine(s.item_name + " " + s.item_price + " " + s.item_stock + " " + s.item_stock_threshold);

                            if (s.item_stock < s.item_stock_threshold)
                            {
                                //ALERT USER WITH NOTIFICATION THAT STOCK IS LOW
                                notification += "LOW STOCK FOR " + s.item_name + ". Current stock level for " + s.item_name + ": " + s.item_stock + "\n";
                            }
                            
                        }
                        else
                        {
                            writer.WriteLine(s.item_name + " " + s.item_price + " " + s.item_stock + " " + s.item_stock_threshold);
                        }
                        
                    }
                    writer.Dispose();
                }
                output += "<total>" + so.total + "</total>\n";
                output += "</salesorderid>\n";   
            }
            output += "</salesorderid>";
            return notification; 
        }

        public int GetCurrentId()
        {
            
            int id = 0;
            foreach(SalesOrderInfo s in salesOrderList)
            {
                int s_id = Convert.ToInt32(s.id);
                if (s_id > id)
                    id = s_id;
            }
            return id+1;
        }

        public List<SalesOrderInfo> loadSalesOrders()
        {
            salesOrderList = new List<SalesOrderInfo>();
            XElement rootElement = XElement.Load("salesorder.xml");
            GetSalesOrders(0, rootElement);
            return salesOrderList;
        }

        private void GetSalesOrders(int indentLevel, XElement element)
        {
            if (element.Attribute("salesorderid") != null)
            {
                SalesOrderInfo salesorder = new SalesOrderInfo();
                salesorder.id = (string)element.Attribute("salesorderid");
                salesorder.user = (string)element.Element("user");
                salesorder.date = (string)element.Element("date");
                salesorder.total = (double)element.Element("total");

                if (element.Element("item") != null)
                {
                    GetItems(0, element, salesorder);
                }

                salesOrderList.Add(salesorder);
            }

            foreach (XElement childElement in element.Elements())
            {
                GetSalesOrders(indentLevel + 1, childElement);
            }
            return;
        }

        private void GetItems(int indentLevel, XElement element, SalesOrderInfo salesOrder)
        {
            if (element.Attribute("item") != null)
            {
                string itemName = (string)element.Element("item");
                double quantity = (double)element.Element("quantity");

                salesOrder.AddItem(itemName, quantity);
            }

            foreach (XElement childElement in element.Elements())
            {
                GetItems(indentLevel + 1, childElement, salesOrder);
            }
            return;
        }

    }

    public class SalesOrderInfo
    {
        public string id;
        public string user;
        public string date;
        public double total;
        public List<Items> items = new List<Items>();

        public void AddItem(string item, double quantity)
        {
            Items _item = new Items(item, quantity);
            items.Add(_item);
        }
        
    }

    public class Items
    {
        public string item_name;
        public double item_quantity;

        public Items(string item, double quantity)
        {
            item_name = item;
            item_quantity = quantity;
        }
    }
    
}
