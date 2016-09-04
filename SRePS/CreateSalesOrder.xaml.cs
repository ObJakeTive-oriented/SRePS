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

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace SRePS
{
    public sealed partial class CreateSalesOrder : Page
    {
        SalesOrder salesOrder = new SalesOrder();
        public CreateSalesOrder()
        {
            this.InitializeComponent();
            int id = salesOrder.NewSalesOrderId();
            textbox_id.Text = id.ToString();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_additem_Click(object sender, RoutedEventArgs e)
        {
            string item_name = textbox_item.Text;
            string quantity = textbox_quantity.Text;
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
        }
    }
}
