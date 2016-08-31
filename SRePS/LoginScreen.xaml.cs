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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SRePS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void usernameField_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string a = usernameField.Text;
                int b;
                if (Int32.TryParse(a, out b))
                {
                    usernameField.Text = b.ToString();
                }
                userInputTest.Text = a;
                if (FocusManager.GetFocusedElement() == usernameField)
                {
                    FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
                }
            }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password == "Password" || passwordBox.Password == "password")
            {
                statusText.Text = "'Password' is not allowed as a password.";
            }
            else
            {
                statusText.Text = string.Empty;
            }
        }

        private void passwordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            //do stuff here to check if password is correct, like this:
            //if (passwordBox.Password == "whatever the password is") {
            //  move onto the main menu page
            //  log event
            //    }

           // below: printing password; this is how we get it
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string a = passwordBox.Password;
                int b;
                if (Int32.TryParse(a, out b))
                {
                    passwordInputTest.Text = b.ToString();
                }
                passwordInputTest.Text = a;
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void passwordBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void textBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }

    }
}
