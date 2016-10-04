using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SRePS
{
    public sealed partial class MainScreen : Page
    {
        public static List<SalesOrderInfo> salesOrderList = new List<SalesOrderInfo>();
        public static SalesOrderInfo currentSalesOrder = null;
        public static List<StockItems> stockItemsList = new List<StockItems>();
        UserLogging userLog = new UserLogging();

        public MainScreen()
        {
            this.InitializeComponent();
            textBlock1.Text = "Signed in as: " + Globals.currentUser;
            SalesOrder newSalesOrder = new SalesOrder();
            salesOrderList = newSalesOrder.loadSalesOrders();
            RetrieveItems getStockList = new RetrieveItems();
            stockItemsList = getStockList.getList();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string ul = "Logged out";
            userLog.Log(ul);
            Frame.Navigate(typeof(LoginScreen));
        }

        private void button_editSalesOrder_Click(object sender, RoutedEventArgs e)
        {
            string ul = "Navigated to Edit Sales Order";
            userLog.Log(ul);
            Frame.Navigate(typeof(EditSalesOrder));
        }

        private void button_createSalesOrder_Click(object sender, RoutedEventArgs e)
        {
            string ul = "Navigated to Create Sales Order";
            userLog.Log(ul);
            Frame.Navigate(typeof(CreateSalesOrder));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageToPrint));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageToPrintLowStock));
        }
    }
}
