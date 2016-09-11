using System;
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

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace SRePS
{
    public sealed partial class CreateSalesOrder : Page
    {
        //List<SalesOrderInfo> LoadedSalesOrders = new List<SalesOrderInfo>();
        SalesOrder salesOrder = new SalesOrder();
        SalesOrderInfo current_so = new SalesOrderInfo();
        List<Items> _itemsList = new List<Items>();
        List<StockItems> _stockList = new List<StockItems>();
        ErrorLogging errorObject = new ErrorLogging();
        Backup _backup = new Backup();

        public double running_total = 0;

        public CreateSalesOrder()
        {
            this.InitializeComponent();
            int id = salesOrder.GetCurrentId();
            current_so.id = id.ToString();
            textbox_id.Text = id.ToString();
            dropdown_user.Items.Add("");
            dropdown_item.Items.Add("");

            LoginParsing loginparsing = new LoginParsing();
            string[] userNames = loginparsing.userNames;
            foreach(string s in userNames)
            {
                dropdown_user.Items.Add(s);
            }

            RetrieveItems getitems = new RetrieveItems();
            _stockList = getitems.getList();
            foreach(StockItems i in _stockList)
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
            foreach(StockItems i in _stockList)
            {
                if(i.item_name == itemName)
                {
                    price = i.item_price;
                    break;
                }
            }
            return price.ToString();
        }

        private void button_additem_Click(object sender, RoutedEventArgs e)
        {
            try {
            string item_name = (string)dropdown_item.SelectedItem;
            string quantity = textbox_quantity.Text;
            string item_price = findItemPrice(item_name);

            current_so.AddItem(item_name, Convert.ToDouble(quantity));
            running_total += (Convert.ToDouble(quantity) * Convert.ToDouble(item_price));
            int item_quantity;
            if (int.TryParse(quantity, out item_quantity))
            {
                textbox_listitems.Text += item_name + "\n";
                textbox_listunitprice.Text += "$" + item_price + "\n";
                textbox_listquantity.Text += item_quantity.ToString() + "\n";
                textbox_total.Text = "$" + running_total.ToString();
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
            catch
            {
                string error = "Error in CreateSalesOrder.caml.cs - button_additem_Click";
                errorObject.LogError(error);
            }
            finally
            {
                //what do I do in here?
            }
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            
            string user = (string)dropdown_user.SelectedItem;
            string date = DateTime.Now.ToString();

            current_so.user = user;
            current_so.date = date;
            current_so.total = running_total;
            MainScreen.salesOrderList.Add(current_so);
            salesOrder.SaveSalesOrders();
        }

        private void button_backup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _backup.MakeBackup();
                backupText.Text = "Successful backup";
            }
            catch
            {
                string er = "Unsuccesful backup";
                backupText.Text = er;
                errorObject.LogError(er);
            }
        }

        private void button_returnToMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainScreen));
        }
    }
}
