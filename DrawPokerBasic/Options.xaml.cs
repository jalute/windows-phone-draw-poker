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
using DrawPokerLibrary;

namespace DrawPokerBasic
{
    public partial class Options : PhoneApplicationPage
    {
        bool bLoading = true;

        public Options()
        {
            InitializeComponent();

            try
            {
                chkSoundOn.Click += new RoutedEventHandler(chkSoundOn_click);
                chkHintMode.Click += new RoutedEventHandler(chkHintMode_Checked);
                chkSoundOn.IsChecked = App.jbSettings.isSoundOn;

                chkHintMode.IsChecked = App.jbSettings.hintMode;

                crdDefaultCard.SetCard(new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Ace });
                crdSexyCard.SetSexyCard(new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Ace });

                if (App.jbSettings.selectedDeck == "SexyGirls")
                    radSexyDeck.IsChecked = true;
                else
                    radDefaultDeck.IsChecked = true;
            }
            finally
            {
                bLoading = false;
            }
        }

        private void chkSoundOn_click(object sender, RoutedEventArgs e)
        {
            if (bLoading) return;

            App.jbSettings.isSoundOn = (bool)chkSoundOn.IsChecked;
            App.jbSettings.SaveToIsolatedStorage();
        }

        private void chkHintMode_Checked(object sender, RoutedEventArgs e)
        {
            if (bLoading) return;

            App.jbSettings.hintMode = (bool)chkHintMode.IsChecked;
            App.jbSettings.SaveToIsolatedStorage();
        }


        private void card_click(object sender, RoutedEventArgs e)
        {
            // do nothing
        }

        private void radDefaultDeck_checked(object sender, RoutedEventArgs e)
        {
            if (bLoading) return;

            if ((bool)radDefaultDeck.IsChecked)
            {
                App.jbSettings.selectedDeck = "Default";
                App.jbSettings.SaveToIsolatedStorage();
            }
        }

        private void radSexyDeck_checked(object sender, RoutedEventArgs e)
        {
            if (bLoading) return;

            if ((bool)radSexyDeck.IsChecked)
            {
                App.jbSettings.selectedDeck = "SexyGirls";
                App.jbSettings.SaveToIsolatedStorage();
            }
        }
    }
}