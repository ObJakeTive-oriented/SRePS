using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SRePS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditSalesOrder : Page
    {
        private SalesOrderInfo currentSO = new SalesOrderInfo();
        private string currentMode;

        public EditSalesOrder()
        {
            this.InitializeComponent();
            label_items.IsEnabled = false;
            label_quantity.IsEnabled = false;
            label_titletable.IsEnabled = false;
            label_total.IsEnabled = false;
            label_unitprice.IsEnabled = false;
            button_makechange.IsEnabled = false;

            dropdown_salesorders.Items.Add("");
            foreach(SalesOrderInfo so in MainScreen.salesOrderList)
            {
                dropdown_salesorders.Items.Add(so.id+", "+so.date);
            }
            
           // button_additem.IsEnabled = false;
            //button_additem.Content = "Save Change";
        }

        private void button_edit_Click(object sender, RoutedEventArgs e)
        {
            dropdown_items.Items.Clear();
            button_makechange.IsEnabled = true;
            button_makechange.Content = "Save Change";
            foreach(Items i in currentSO.items)
            {
                dropdown_items.Items.Add(i.item_name);
            }
            currentMode = "edit";
        }

        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            dropdown_items.Items.Clear();
            button_makechange.IsEnabled = true;
            button_makechange.Content = "Add Item";
            foreach (StockItems i in MainScreen.stockItemsList)
            {
                dropdown_items.Items.Add(i.item_name);
            }
            currentMode = "add";
        }

        private SalesOrderInfo getSalesOrder(string id)
        {
            SalesOrderInfo salesorder = new SalesOrderInfo();
            foreach (SalesOrderInfo so in MainScreen.salesOrderList)
            {
                if (so.id == id)
                {
                    salesorder = so;
                    break;
                }
            }
            return salesorder;
        }

        private string findItemPrice(string itemName)
        {
            double price = 0;
            foreach (StockItems i in MainScreen.stockItemsList)
            {
                if (i.item_name == itemName)
                {
                    price = i.item_price;
                    break;
                }
            }

            return price.ToString();
        }
        

        private void refreshTable()
        {
            textbox_listitems.Text = "";
            textbox_listquantity.Text = "";
            textbox_listunitprice.Text = "";
            double runningtotal = 0;

            foreach (Items i in currentSO.items)
            {
                string itemprice = findItemPrice(i.item_name);

                textbox_listitems.Text += i.item_name + "\n";
                textbox_listquantity.Text += i.item_quantity.ToString() + "\n";
                textbox_listunitprice.Text += "$"+itemprice + "\n";
                double lTotal = (i.item_quantity * Convert.ToDouble(itemprice));
                runningtotal += lTotal;
            }
            currentSO.total = runningtotal;
            textbox_total.Text = "$" + runningtotal.ToString();
        }

        private void dropdown_salesorders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = (string)dropdown_salesorders.SelectedItem;
            if (selection == "")
                return;
            string[] input = selection.Split(',');
            currentSO = getSalesOrder(input[0]);

            refreshTable();

            button_edit.IsEnabled = true;
            button_add.IsEnabled = true;
            button_makechange.IsEnabled = false;
            dropdown_items.Items.Add("");
        }

        private void button_makechange_Click(object sender, RoutedEventArgs e)
        {
            string itemName = (string)dropdown_items.SelectedItem;
            string quantity = textbox_quantity.Text;

            switch(currentMode)
            {
                case "edit":
                    foreach(Items i in currentSO.items)
                    {
                        if(i.item_name == itemName)
                        {
                            i.item_quantity = Convert.ToDouble(quantity);
                            break;
                        }
                    }
                    break;
                case "add":
                    Items newItem = new Items(itemName, Convert.ToDouble(quantity));
                    currentSO.items.Add(newItem);
                    break;
            }
            refreshTable();
        }

        private void button_returnToMenu_Click(object sender, RoutedEventArgs e)
        {
            SalesOrder tempSoObj = new SalesOrder();
            tempSoObj.getSoList = MainScreen.salesOrderList;
            tempSoObj.SaveSalesOrders();
            Frame.Navigate(typeof(MainScreen));
        }
    }
}
