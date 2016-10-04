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
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;

namespace SRePS
{
    public sealed partial class PageToPrint : Page
    {
        private PrintManager printMan;
        private PrintDocument printDoc;
        private IPrintDocumentSource printDocSource;
        UserLogging userLog = new UserLogging();
        List<SalesOrderInfo> salesOrderList = new List<SalesOrderInfo>();

        public PageToPrint()
        {
            this.InitializeComponent();
            string ul = "Entered printing page";
            userLog.Log(ul);
            button_print.Content = "Print";
        }

        #region Register for printing
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Register for PrintTaskRequested event
            printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;

            // Build a PrintDocument and register for callbacks
            printDoc = new PrintDocument();
            printDocSource = printDoc.DocumentSource;
            printDoc.Paginate += Paginate;
            printDoc.GetPreviewPage += GetPreviewPage;
            printDoc.AddPages += AddPages;
             
        }

#endregion

        #region Showing the print dialog

        private async void PrintButtonClick(object sender, RoutedEventArgs e)
        {
            string ul = "Pressed printing button";
            userLog.Log(ul);
            if (PrintManager.IsSupported())
            {
                try
                {
                    // Show print UI
                    await PrintManager.ShowPrintUIAsync();
                }
                catch
                {
                    // Printing cannot proceed at this time
                    ContentDialog noPrintingDialog = new ContentDialog()
                    {
                        Title = "Printing error",
                        Content = "\nSorry, printing can' t proceed at this time.",
                        PrimaryButtonText = "OK"
                    };
                    await noPrintingDialog.ShowAsync();
                }
            }
            else
            {
                // Printing is not supported on this device
                ContentDialog noPrintingDialog = new ContentDialog()
                {
                    Title = "Printing not supported",
                    Content = "\nSorry, printing is not supported on this device.",
                    PrimaryButtonText = "OK"
                };
                await noPrintingDialog.ShowAsync();
            }
        }

        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            // Create the PrintTask.
            // Defines the title and delegate for PrintTaskSourceRequested
            var printTask = args.Request.CreatePrintTask("Print", PrintTaskSourceRequrested);

            // Handle PrintTask.Completed to catch failed print jobs
            printTask.Completed += PrintTaskCompleted;
        }

        private void PrintTaskSourceRequrested(PrintTaskSourceRequestedArgs args)
        {
            // Set the document source.
            args.SetSource(printDocSource);
        }

        #endregion

        #region Print preview

        private void Paginate(object sender, PaginateEventArgs e)
        {
            // As I only want to print one Rectangle, so I set the count to 1
            printDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }

        private void GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            // Provide a UIElement as the print preview.
            printDoc.SetPreviewPage(e.PageNumber, this.ElementGrid);
        }

        #endregion

        #region Add pages to send to the printer

        private void AddPages(object sender, AddPagesEventArgs e)
        {
            printDoc.AddPage(this);

            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }

        #endregion

        #region Print task completed

        private async void PrintTaskCompleted(PrintTask sender, PrintTaskCompletedEventArgs args)
        {
            // Notify the user when the print operation fails.
            if (args.Completion == PrintTaskCompletion.Failed)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    ContentDialog noPrintingDialog = new ContentDialog()
                    {
                        Title = "Printing error",
                        Content = "\nSorry, failed to print.",
                        PrimaryButtonText = "OK"
                    };
                    await noPrintingDialog.ShowAsync();
                });
            }
        }

        #endregion

        private void dateFrom_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs e)
        {
            
            
        }

        private void dateFrom_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (!dateTo.Date.HasValue)
            {
                return;
            }

            GenerateReport(dateFrom.Date.Value, dateTo.Date.Value);
           /* DateTime to = dateTo.Date;
            DateTime from = dateFrom.Date.;

            string dateTime = "10/28/2016 1:52:12 PM";
            DateTime tempDate = Convert.ToDateTime(dateTime);
            if (from > tempDate)
            {
                textbox_listitems.Text = "From is greater than to";
            }
            else
                textbox_listitems.Text = "TO is greater than FROM";*/
        }

        private void dateTo_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            DateTimeOffset from = dateFrom.Date.Value;
            if(!dateFrom.Date.HasValue)
            {
                return;
            }
            GenerateReport(dateFrom.Date.Value, dateTo.Date.Value);
        }

        private void GenerateReport(DateTimeOffset from, DateTimeOffset to)
        {
            textbox_listitems.Text = "";
            textbox_listquantity.Text = "";
            textbox_listunitprice.Text = "";
            outputRevenue.Text = "";
            outputTotalItems.Text = "";

            SalesOrder salesOrder = new SalesOrder();
            salesOrderList = salesOrder.loadSalesOrders();
            List<SalesOrderInfo> tempList = new List<SalesOrderInfo>();
            DateTime temp;
            foreach(SalesOrderInfo so in salesOrderList)
            {
                temp = Convert.ToDateTime(so.date);
                if ((temp >= from) && (temp <= to))
                    tempList.Add(so);
            }
            
            List<Items> items = new List<Items>();
            foreach(SalesOrderInfo info in tempList)
            {
                foreach(Items i in info.items)
                {
                    Items tempItem = findItem(items, i);
                    if(tempItem == null)
                    {
                        items.Add(i);
                    }
                    else
                    {
                        tempItem.item_quantity += i.item_quantity;
                    }
                }
            }
            RetrieveItems stockListClass = new RetrieveItems();
            double revenue = 0;
            double totalItems = 0;
            foreach (Items a in items)
            {
                
                double quantity = a.item_quantity;
                double price = stockListClass.ReturnPrice(a.item_name);
                textbox_listitems.Text += a.item_name + "\n";
                textbox_listunitprice.Text += "$"+price + "\n";
                textbox_listquantity.Text += quantity + "\n";
                totalItems += quantity;
                revenue += (price * quantity);
            }
            outputRevenue.Text = "$"+revenue.ToString();
            outputTotalItems.Text = totalItems.ToString();
        }

        private Items findItem(List<Items> i, Items item)
        {
            foreach(Items x in i)
            {
                if (x.item_name == item.item_name)
                    return x;
            }
            return null;
        }

        private void button_return_Click(object sender, RoutedEventArgs e)
        {
            string ul = "Returnig from printing.";
            userLog.Log(ul);
            printMan.PrintTaskRequested -= PrintTaskRequested;
            printDoc.Paginate -= Paginate;
            printDoc.GetPreviewPage -= GetPreviewPage;
            printDoc.AddPages -= AddPages;
            Frame.Navigate(typeof(MainScreen));
        }
    }
}