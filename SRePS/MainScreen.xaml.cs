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
    public sealed partial class MainScreen : Page
    {
        public static List<SalesOrderInfo> salesOrderList = new List<SalesOrderInfo>();
        public static SalesOrderInfo currentSalesOrder = null;
        public static List<StockItems> stockItemsList = new List<StockItems>();

        public MainScreen()
        {
            this.InitializeComponent();
            SalesOrder newSalesOrder = new SalesOrder();
            salesOrderList = newSalesOrder.loadSalesOrders();
            RetrieveItems getStockList = new RetrieveItems();
            stockItemsList = getStockList.getList();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginScreen));
        }

        private void button_editSalesOrder_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditSalesOrder));
        }

        private void button_createSalesOrder_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateSalesOrder));
        }
    }
}