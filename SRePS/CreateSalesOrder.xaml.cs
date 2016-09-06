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
        public double running_total = 0;

        public CreateSalesOrder()
        {
            this.InitializeComponent();
            int id = salesOrder.GetCurrentId();
            textbox_id.Text = id.ToString();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_additem_Click(object sender, RoutedEventArgs e)
        {
            string item_name = textbox_item.Text;
            string quantity = textbox_quantity.Text;
            
            current_so.AddItem(item_name, Convert.ToDouble(quantity));
            running_total += Convert.ToDouble(quantity);
            int item_quantity;
            if(int.TryParse(quantity, out item_quantity))
            {
                textbox_listitems.Text += item_name + "\n";
                textbox_listquantity.Text += item_quantity.ToString() + "\n";
            }
            else
            {
                textbox_error.Text = "Please enter a number into the quantity field!";
                return;
            }
            textbox_item.Text = "";
            textbox_quantity.Text = "";
            textbox_error.Text = "";
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            string user = textbox_user.Text;
            string date = textbox_date.Text;

            current_so.user = user;
            current_so.date = date;
            current_so.total = running_total;
            current_so.id = 6.ToString();
            salesOrder.loadSalesOrders().Add(current_so);
            salesOrder.SaveSalesOrders();
        }
        
    }
}
