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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DrawPokerLibrary;


namespace DrawPokerBasic
{
    
    public partial class ucCard : UserControl
    {
        /// <summary>
        /// Card 
        /// </summary>
        public Card myCard;

        /// <summary>
        /// Are we holding this card or not?
        /// </summary>
        public bool hold = false;

        public event ClickEventHandler Click;
        public delegate void ClickEventHandler(object sender, RoutedEventArgs e);

        private bool bShowSexyWomen = false;

//        public event EventHandler Click;

        public ucCard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This function displays the correct card output to the screen
        /// </summary>
        /// <param name="pCard">Card that we are going to display</param>
        public void SetCard(Card pCard)
        {
            myCard = pCard;
            bShowSexyWomen = false;
            txtSuiteLarge.Visibility = Visibility.Visible;

            // call animation here
            Cardflip.Begin();            
        }

        public void SetSexyCard(Card pCard)
        {
            myCard = pCard;
            bShowSexyWomen = true;
            txtSuiteLarge.Visibility = Visibility.Collapsed;

            // call animation here
            Cardflip.Begin();
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Click(sender, e);
        }

        private void CardFlip_Complete(object sender, EventArgs e)
        {
            string imagename = "Cards/";

            switch (myCard.Suit)
            {
                case Cards.Suits.Clubs:
                    txtSuitSmall.Text = "♣";     // ascii 5
                    txtSuitSmall.Foreground = new SolidColorBrush(Colors.Black);
                    txtSuiteLarge.Text = "♣";
                    txtSuiteLarge.Foreground = new SolidColorBrush(Colors.Black);
                    txtValue.Foreground = new SolidColorBrush(Colors.Black);
                    imagename += "c";
                    break;
                case Cards.Suits.Diamonds:
                    txtSuitSmall.Text = "♦";     // ascii 4
                    txtSuitSmall.Foreground = new SolidColorBrush(Colors.Red);
                    txtSuiteLarge.Text = "♦";
                    txtSuiteLarge.Foreground = new SolidColorBrush(Colors.Red);
                    txtValue.Foreground = new SolidColorBrush(Colors.Red);
                    imagename += "d";
                    break;
                case Cards.Suits.Hearts:
                    txtSuitSmall.Text = "♥";    // ascii 3
                    txtSuitSmall.Foreground = new SolidColorBrush(Colors.Red);
                    txtSuiteLarge.Text = "♥";
                    txtSuiteLarge.Foreground = new SolidColorBrush(Colors.Red);
                    txtValue.Foreground = new SolidColorBrush(Colors.Red);
                    imagename += "h";
                    break;
                case Cards.Suits.Spade:
                    txtSuitSmall.Text = "♠";    // ascii 6
                    txtSuitSmall.Foreground = new SolidColorBrush(Colors.Black);
                    txtSuiteLarge.Text = "♠";
                    txtSuiteLarge.Foreground = new SolidColorBrush(Colors.Black);
                    txtValue.Foreground = new SolidColorBrush(Colors.Black);
                    imagename += "s";
                    break;
            }

            imagename += Convert.ToInt32(myCard.Value).ToString() + ".jpg";

            switch (Convert.ToInt32(myCard.Value))
            {
                case 11:
                    txtValue.Text = "J";
                    break;
                case 12:
                    txtValue.Text = "Q";
                    break;
                case 13:
                    txtValue.Text = "K";
                    break;
                case 1:
                    txtValue.Text = "A";
                    break;
                default:
                    txtValue.Text = Convert.ToInt32(myCard.Value).ToString();
                    break;
            }

            if (bShowSexyWomen)
            {
                ImageBrush imageBrush = new ImageBrush();
                Uri uri = new Uri(imagename, UriKind.RelativeOrAbsolute);
                imageBrush.ImageSource = new BitmapImage(uri);
                imageBrush.Stretch = Stretch.UniformToFill;

                brdCard.Background = imageBrush;
            }
            // note, not sure if I need to code to reset it back to white.
        }
    }
}
