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
using Microsoft.Phone.Tasks;

namespace DrawPokerBasic
{
    public partial class JacksOrBetterStrategy : PhoneApplicationPage
    {
       // private JacksBetterUserSettings jbSettings = new JacksBetterUserSettings();

        public JacksOrBetterStrategy()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

           // MessageBox.Show("Test = " + jbSettings.Test.ToString());
        }

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
           // NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void btnVidPokerTrainer_click(object sender, RoutedEventArgs e)
        {
            //Show an Video Poker Trainer, using the default ContentType.
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = "3a40c1b1-cfc3-42cd-8f8e-f7afe24c75ce";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();
        }
    }
}