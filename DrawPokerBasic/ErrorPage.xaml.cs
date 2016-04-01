using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;

namespace DrawPokerBasic
{
    public partial class ErrorPage : PhoneApplicationPage
    {
        public static Exception Exception;

        public ErrorPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            ErrorText.Text = Exception.Message;
        }

        private void EMailLink_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Video Poker Basic Error";
            emailComposeTask.Body = Exception.ToString();
            emailComposeTask.To = "feedback@jalute.com";

            emailComposeTask.Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/StartPage.xaml", UriKind.Relative));
        }
       
    }
}