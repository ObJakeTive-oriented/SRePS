﻿using System;
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

        public void SaveSalesOrders()
        {
            string output = "<?xml version=\"1.0\"?>\n<salesorderid>\n";
            foreach (SalesOrderInfo so in MainScreen.salesOrderList)
            {
                output += "<salesorderid salesorderid=\"" + so.id + "\">\n";
                output += "<user>" + so.user + "</user>\n<date>" + so.date + "</date>\n";
                int itemid = 0;
                foreach (Items item in so.items)
                {
                    itemid++;
                    output += "<item item=\"" + itemid.ToString() + "\">\n";
                    output += "<item>" + item.item_name + "</item>\n";
                    output += "<quantity>" + item.item_quantity + "</quantity>\n";
                    output += "</item>\n";
                }
                output += "<total>" + so.total + "</total>\n";
                output += "</salesorderid>\n";
            }
            output += "</salesorderid>";

            StreamWriter writer;
            FileStream fs = new FileStream(@"salesorder.xml", FileMode.Create, FileAccess.ReadWrite);
            File.SetAttributes("salesorder.xml", FileAttributes.Normal);
            writer = new StreamWriter(fs);
            try
            {
                writer.Write(output);
            }
            finally
            {
                writer.Dispose();
            }
        }

        public int GetCurrentId()
        {

            int id = 0;
            foreach (SalesOrderInfo s in salesOrderList)
            {
                int s_id = Convert.ToInt32(s.id);
                if (s_id > id)
                    id = s_id;
            }
            return id + 1;
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

        public void UpdateStockText(RetrieveItems i)
        {
            List<StockItems> _stockList = i.getList();
            StreamWriter writer;
            FileStream fs = new FileStream(@"items.txt", FileMode.Open, FileAccess.Write);
            writer = new StreamWriter(fs);

            foreach (StockItems s in _stockList)
            {
                writer.WriteLine(s.item_name + " " + s.item_price + " " + s.item_stock + " " + s.item_stock_threshold);
            }
            writer.Dispose();
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