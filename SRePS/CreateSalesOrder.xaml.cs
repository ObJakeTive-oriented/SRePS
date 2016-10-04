﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Text.RegularExpressions;

namespace SRePS
{
    public sealed partial class CreateSalesOrder : Page
    {
        SalesOrder salesOrder = new SalesOrder();
        SalesOrderInfo current_so = new SalesOrderInfo();
        List<Items> _itemsList = new List<Items>();
        List<StockItems> _stockList = new List<StockItems>();
        ErrorLogging errorObject = new ErrorLogging();
        UserLogging userLog = new UserLogging();
        Backup _backup = new Backup();
        RetrieveItems getitems = new RetrieveItems();

        public double running_total = 0;

        public CreateSalesOrder()
        {
            this.InitializeComponent();
            textBlock1.Text = "Signed in as: " + Globals.currentUser;
            int id = salesOrder.GetCurrentId();
            current_so.id = id.ToString();
            textbox_id.Text = id.ToString();
            dropdown_item.Items.Add("");

            LoginParsing loginparsing = new LoginParsing();

            _stockList = getitems.getList();
            foreach (StockItems i in _stockList)
            {
                dropdown_item.Items.Add(i.item_name);
            }

        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private string findItemPrice(string itemName)
        {
            double price = 0;
            foreach (StockItems i in _stockList)
            {
                if (i.item_name == itemName)
                {
                    price = i.item_price;
                    break;
                }
            }
            return price.ToString();
        }

        private void button_additem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string item_name = (string)dropdown_item.SelectedItem;
                string quantity = textbox_quantity.Text;
                int i = getitems.ReturnStockLevel(item_name) - Convert.ToInt32(quantity);
                if (i >= 0) //if stock level will be zero or above after sale, go ahead with it
                {
                    string item_price = findItemPrice(item_name);
                    getitems.UpdateStock(item_name, Convert.ToInt32(quantity));
                    current_so.AddItem(item_name, Convert.ToDouble(quantity));
                    running_total += (Convert.ToDouble(quantity) * Convert.ToDouble(item_price));
                    int item_quantity;
                    if (int.TryParse(quantity, out item_quantity))
                    {
                        textbox_listitems.Text += item_name + "\n";
                        textbox_listunitprice.Text += "$" + item_price + "\n";
                        textbox_listquantity.Text += item_quantity.ToString() + "\n";
                        textbox_total.Text = "$" + running_total.ToString();
                        string ul = "Added item: " + item_name;
                        userLog.Log(ul);
                    }
                    if(getitems.ReturnStockLevel(item_name) < getitems.ReturnThresh(item_name))
                    {
                        textboxNotification.Text =  item_name + " is below threshold\nIt's current stock: " + getitems.ReturnStockLevel(item_name);
                    }
                    else
                    {
                        textbox_error.Text = "Please enter a number into the quantity field!";
                        return;
                    }
                    dropdown_item.SelectedItem = "";
                    textbox_quantity.Text = "";
                    textbox_error.Text = "";
                }
                else
                {
                    textboxNotification.Text = "Cannot add " + item_name  + " as stock will be negative.\nIt's current stock: " + getitems.ReturnStockLevel(item_name);
                }

            }
            catch
            {
                string error = "Error in CreateSalesOrder.caml.cs - button_additem_Click";
                errorObject.Log(error);
            }
            finally
            {
                //what do I do in here?
            }
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            string date = DateTime.Now.ToString();

            current_so.user = Globals.currentUser;
            current_so.date = date;
            current_so.total = running_total;
            MainScreen.salesOrderList.Add(current_so);
            salesOrder.SaveSalesOrders();
            string ul = "Added sales order #? TODO work out how to add sales number here";
            userLog.Log(ul);
        }

        private void button_backup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _backup.MakeBackup();
                backupText.Text = "Successful backup";
                string ul = "Prompted backup that was successful";
                userLog.Log(ul);
            }
            catch
            {
                string er = "Unsuccesful backup";
                backupText.Text = er;
                errorObject.Log(er);
                string ul = "Prompted backup that was unsuccessful";
                userLog.Log(ul);
            }
        }

        private void button_returnToMenu_Click(object sender, RoutedEventArgs e)
        {
            string ul = "Returned to main screen";
            userLog.Log(ul);
            Frame.Navigate(typeof(MainScreen));
        }

        private void dropdown_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
