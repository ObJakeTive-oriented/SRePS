using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
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
    public static class Globals
    {
        public static string currentUser { get; set; }
    }
    public sealed partial class LoginScreen : Page
    {
        List<UserClass> users = new List<UserClass>();
        public LoginScreen()
        {
            this.InitializeComponent();
            LoginParsing test = new LoginParsing();
            users = test.LoginList();
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
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string a = passwordBox.Password;
                int b;
                if (Int32.TryParse(a, out b))
                {
                    passwordInputTest.Text = b.ToString();
                }
                passwordInputTest.Text = a;

                int countUser = 0;
                int countPass = 0;

                for (int i = 0; i < users.Count; i++)
                {
                    UserClass userCheck = users[i];
                    if (usernameField.Text == userCheck.user)
                    {
                        if (passwordBox.Password == userCheck.password)
                        {
                            //NextPageArguments passedArgs = new NextPageArguments();
                            //passedArgs.user = usernameField.Text;
                            Globals.currentUser = usernameField.Text;
                            Frame.Navigate(typeof(MainScreen));
                        }
                        else
                        {
                            ++countPass;
                        }
                    }

                    else
                    {
                        ++countUser;
                    }
                }

                if (countUser == users.Count)
                {
                    if(countPass == 0)
                    {
                        statusText.Text = "Incorrect username and password";
                    }
                        
                    else
                    {
                        statusText.Text = "Incorrect username";
                    }  
                }
                    
                else if (countPass == 1)
                {
                    statusText.Text = "Incorrect password";
                }     
            }
        }

        public class NextPageArguments
        {
            public string user { get; set; }
        }


        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        

        private void textBlock1_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void textBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
