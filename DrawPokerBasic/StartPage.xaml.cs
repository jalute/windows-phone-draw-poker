using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;

namespace DrawPokerBasic
{
    public partial class StartPage : PhoneApplicationPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void btnStrategy_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/JacksOrBetterStrategy.xaml", UriKind.Relative));
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Options.xaml", UriKind.Relative));
        }

        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Stats.xaml", UriKind.Relative));
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
    }
}