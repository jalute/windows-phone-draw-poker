using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using DrawPokerLibrary;
using Microsoft.Advertising.Mobile.UI;


namespace DrawPokerBasic
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor

        private const char vbTab = (char)9;
        private Random rr = new Random();
        private Card[] currentDeck;
        private Card[] currentHand = new Card[5];
        private HandAnalyzer haInstance;

        private int _multiplier = 1;
        private int _currentBet = 1;
        private int _winStreak = 0;
        private int _loseStreak = 0;
        private static SoundEffect _sfBeep, _sfWinBeep;
        private bool bLoading = true;
        private bool _hintMode = false;
        private bool bShowSexyGirls = false;

        //private JacksBetterUserSettings jbSettings = new JacksBetterUserSettings();
        PhoneApplicationService appService = PhoneApplicationService.Current;

        public MainPage()
        {
            InitializeComponent();

            // default cards
            //ucCard1.SetCard(new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Ten });
            //ucCard2.SetCard(new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Jack });
            //ucCard3.SetCard(new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Queen });
            //ucCard4.SetCard(new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.King });
            //ucCard5.SetCard(new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Ace });

            if (App.jbSettings.selectedDeck == "SexyGirls")
                bShowSexyGirls = true;

            if (bShowSexyGirls)
            {
                ucCard1.SetSexyCard(App.jbSettings.card1);
                ucCard2.SetSexyCard(App.jbSettings.card2);
                ucCard3.SetSexyCard(App.jbSettings.card3);
                ucCard4.SetSexyCard(App.jbSettings.card4);
                ucCard5.SetSexyCard(App.jbSettings.card5);
            }
            else
            {
                ucCard1.SetCard(App.jbSettings.card1);
                ucCard2.SetCard(App.jbSettings.card2);
                ucCard3.SetCard(App.jbSettings.card3);
                ucCard4.SetCard(App.jbSettings.card4);
                ucCard5.SetCard(App.jbSettings.card5);
            }

            currentHand[0] = App.jbSettings.card1;
            currentHand[1] = App.jbSettings.card2;
            currentHand[2] = App.jbSettings.card3;
            currentHand[3] = App.jbSettings.card4;
            currentHand[4] = App.jbSettings.card5;

            txtWin.Text = string.Empty;

            // retrieve saved settings from isolated storage
            _currentBet = App.jbSettings.lastBet;
            _hintMode = App.jbSettings.hintMode;

            if (_currentBet > 100) _currentBet = 100;

            //_multiplier = App.jbSettings.multiplier;
            if (App.jbSettings.betBalance > 0)
                txtCredits.Text = App.jbSettings.betBalance.ToString();
            else
                txtCredits.Text = "100";
            //btnDeal.Content = App.jbSettings.btnDealText;

            SetBetFields();
            PopulatePayoutList(_currentBet);
            //SetMultiplierButton(_multiplier);
            EnableDisableDealButton();
            ResetHoldText();
         //   lblPayout.Visibility = Visibility.Collapsed;
            txtPayout.Visibility = Visibility.Collapsed;
            if ((string)btnDeal.Content == "Deal")
                rectPayout.Visibility = Visibility.Visible;
            else
                rectPayout.Visibility = Visibility.Collapsed;

            grdBetAmount.Visibility = System.Windows.Visibility.Collapsed;

            //PopulateStrategy();

            // initialize sound
           // Utils.Beep();

            FrameworkDispatcher.Update();
            var winstream = TitleContainer.OpenStream(@"Sounds/beep-02.wav");
            var beepstream = TitleContainer.OpenStream(@"Sounds/beep-07.wav");

            _sfBeep = SoundEffect.FromStream(beepstream);
            _sfWinBeep = SoundEffect.FromStream(winstream);

            HideHints();

            //_sfBeep = 
            bLoading = false;

            txtStatus.Text = "Change Bet, press Deal";             
            
        }

        /// <summary>
        ///  this displays the payouts in the listbox at the top of the page
        /// </summary>
        /// <param name="pBet"></param>
        private void PopulatePayoutList(int pBet)
        {
            ObservableCollection<VegasPayoutClass> myPayout = new ObservableCollection<VegasPayoutClass>();

            //ObservableCollection<TwoColumnClass> info = new ObservableCollection<TwoColumnClass>();
            string[] s_hand = {"Royal Flush", 
                                  "Straight Flush", 
                                  "Four of Kind", 
                                  "Full House", 
                                  "Flush",
                                  "Straight",
                                  "Three of a Kind",
                                  "Two Pair",
                                  "Jacks or Better"};

            // this is full pay 99.54% return
            int[] i_payout = { 800, 50, 25, 9, 6, 4, 3, 2, 1 };

            for (int i = 0; i < s_hand.Length; i++)
            {
                int total_pay = i_payout[i] * pBet;

                //info.Add(new TwoColumnClass { column1 = s_hand[i], column2 = total_pay.ToString() });
                myPayout.Add(new VegasPayoutClass(s_hand[i], total_pay));
            }

            lstPayouts.DataContext = myPayout;
        }

        /// <summary>
        /// this highlights in the listbox which hand they currently have
        /// </summary>
        /// <param name="hand"></param>
        private void HighlightPayout(Cards.VegasPayoutHands hand)
        {
            switch (hand)
            {
                case Cards.VegasPayoutHands.RoyalFlush:
                    lstPayouts.SelectedIndex = 0;
                    break;
                case Cards.VegasPayoutHands.StraightFlush:
                    lstPayouts.SelectedIndex = 1;
                    break;
                case Cards.VegasPayoutHands.FourOfKind:
                    lstPayouts.SelectedIndex = 2;
                    break;
                case Cards.VegasPayoutHands.FullHouse:
                    lstPayouts.SelectedIndex = 3;
                    break;
                case Cards.VegasPayoutHands.Flush:
                    lstPayouts.SelectedIndex = 4;
                    break;
                case Cards.VegasPayoutHands.Straight:
                    lstPayouts.SelectedIndex = 5;
                    break;
                case Cards.VegasPayoutHands.ThreeOfKind:
                    lstPayouts.SelectedIndex = 6;
                    break;
                case Cards.VegasPayoutHands.TwoPair:
                    lstPayouts.SelectedIndex = 7;
                    break;
                case Cards.VegasPayoutHands.JacksOrBetterPair:
                    lstPayouts.SelectedIndex = 8;
                    break;
                default:
                    lstPayouts.SelectedIndex = -1;
                    break;
            }

            if (lstPayouts.SelectedIndex == -1) txtWin.Text = string.Empty;
            else txtWin.Text = lstPayouts.SelectedValue.ToString();
        }

        private void HideHints()
        {
            lineHint1.Visibility = Visibility.Collapsed;
            lineHint2.Visibility = Visibility.Collapsed;
            lineHint3.Visibility = Visibility.Collapsed;
            lineHint4.Visibility = Visibility.Collapsed;
            lineHint5.Visibility = Visibility.Collapsed;
        }

        private void ShowHints(Card[] currentHand)
        {
            if (_hintMode)
            {
                haInstance = new HandAnalyzer();

                string sAdvice = haInstance.Analyze(currentHand);

                if (haInstance.isPaiGow) return;   // nothing to do

                foreach (Card hc in haInstance.holdCards)
                {
                    for (int idx = 0; idx < 5; idx++)
                    {
                        if (hc.Equals(currentHand[idx]))
                        {
                            if (idx == 0) lineHint1.Visibility = Visibility.Visible;
                            if (idx == 1) lineHint2.Visibility = Visibility.Visible;
                            if (idx == 2) lineHint3.Visibility = Visibility.Visible;
                            if (idx == 3) lineHint4.Visibility = Visibility.Visible;
                            if (idx == 4) lineHint5.Visibility = Visibility.Visible;
                        }
                    }
                }
    
            }

        }


        //private void PopulateStrategy()
        //{
        //    string[] s_strategy = { "4 of Kind or better",
        //                              "4 to Royal Flush",
        //                              "2 Pair or better",
        //                              "4 to Straight Flush",
        //                              "Pay Pair",
        //                              "3 to Royal Flush",
        //                              "4 to a Flush",
        //                              "Low Pair",
        //                              "Open ended straight",
        //                              "AKJQ",
        //                              "2 suited high cards",
        //                              "3 to Straight Flush",
        //                              "KQJ",
        //                              "2 high cards",
        //                              "1 high card"};
            
        //    ObservableCollection<String> os_strategy = new ObservableCollection<string>(new List<String>(s_strategy));
            
        //    lstStrategy.ItemsSource = os_strategy;

        //}

        private void ResetHoldText()
        {
            textBlock1.Text = string.Empty;
            textBlock2.Text = string.Empty;
            textBlock3.Text = string.Empty;
            textBlock4.Text = string.Empty;
            textBlock5.Text = string.Empty;
        }

        /// <summary>
        /// This checks to see if they are betting too much.  If they are 
        /// disable the Deal button.
        /// </summary>
        private void EnableDisableDealButton()
        {
            int iCredits = Convert.ToInt16(txtCredits.Text);

            if (iCredits == 0)
            {
                MessageBox.Show("You have lost all of your credits! Click OK for 100 credits.", "Bad Luck", MessageBoxButton.OK);
                txtCredits.Text = "100";
                btnDeal.Content = "Deal";
                txtWin.Text = "";
                btnBet.IsEnabled = true;
                btnMultiplier.IsEnabled = true;
                txtStatus.Text = "Credits replinished";

                App.jbSettings.balanceReloads += 1;
                App.jbSettings.betBalance = 100;
            }
            else if (bLoading == false && (_currentBet * _multiplier) >= iCredits)
            {
                //btnDeal.IsEnabled = true;
                if (MessageBox.Show("Your credits are low! Do you wish to add 100 credits?", "Low Credits", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {                    
                    iCredits += 100;
                    txtCredits.Text = iCredits.ToString();
                    btnDeal.Content = "Deal";
                    txtWin.Text = "";
                    btnBet.IsEnabled = true;
                    btnMultiplier.IsEnabled = true;
                    txtStatus.Text = "Credits replinished";

                    App.jbSettings.balanceReloads += 1;
                    App.jbSettings.betBalance = iCredits;
                }
            }

            if ((_currentBet * _multiplier) > iCredits)
            {
                btnDeal.IsEnabled = false;
                txtStatus.Text = "Credits low, change bet amount";
            }
            else
                btnDeal.IsEnabled = true;
        }

        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            //TODO: add a check here if phone is busy

            Cards c = new Cards();
            if (btnDeal.Content.Equals("Deal"))
            {
                rectPayout.Visibility = Visibility.Collapsed;
                txtPayout.Visibility = Visibility.Collapsed;

                txtStatus.Text = "Select hold cards and Draw";

                Card[] d1 = c.GetNewDeck();

                // shuffle the cards
                int max = rr.Next(1000, 24000);
                currentDeck = c.VegasShuffle(d1);
                for (int idx = 0; idx < max; idx++)
                {
                    currentDeck = c.ShuffleSplit(currentDeck);
                }
                App.jbSettings.currentDeck = currentDeck;

                ResetHoldText();

                currentHand[0] = currentDeck[0];
                currentHand[1] = currentDeck[1];
                currentHand[2] = currentDeck[2];
                currentHand[3] = currentDeck[3];
                currentHand[4] = currentDeck[4];

                if (bShowSexyGirls)
                {
                    ucCard1.SetSexyCard(currentHand[0]);
                    ucCard2.SetSexyCard(currentHand[1]);
                    ucCard3.SetSexyCard(currentHand[2]);
                    ucCard4.SetSexyCard(currentHand[3]);
                    ucCard5.SetSexyCard(currentHand[4]);
                }
                else
                {
                    ucCard1.SetCard(currentHand[0]);
                    ucCard2.SetCard(currentHand[1]);
                    ucCard3.SetCard(currentHand[2]);
                    ucCard4.SetCard(currentHand[3]);
                    ucCard5.SetCard(currentHand[4]);
                }

                ucCard1.button1.IsEnabled = true;
                ucCard2.button1.IsEnabled = true;
                ucCard3.button1.IsEnabled = true;
                ucCard4.button1.IsEnabled = true;
                ucCard5.button1.IsEnabled = true;

                // save settings
                App.jbSettings.lastBet = _currentBet;
                //App.jbSettings.multiplier = _multiplier;
                App.jbSettings.handCount += 1;
                App.jbSettings.sessionHandCount += 1;
                App.jbSettings.card1 = currentHand[0];
                App.jbSettings.card2 = currentHand[1];
                App.jbSettings.card3 = currentHand[2];
                App.jbSettings.card4 = currentHand[3];
                App.jbSettings.card5 = currentHand[4];

                Cards.VegasPayoutHands pay = c.ProjectPayback(currentHand);

                if (pay != Cards.VegasPayoutHands.PaiGow)
                {
                    if (App.jbSettings.isSoundOn) _sfWinBeep.Play();  //medWinBeep.Play();
                }

                int iPay = (int)pay * (_currentBet * _multiplier); //Convert.ToInt16(txtBetAmount.Text);
                txtPayout.Text = "You Win " + iPay.ToString() + "!";

                HighlightPayout(pay);

                int iCredits = Convert.ToInt16(txtCredits.Text);

                txtCredits.Text = Convert.ToString(iCredits - (_currentBet * _multiplier));

                App.jbSettings.betBalance = Convert.ToInt16(txtCredits.Text);  // this is to prevent cheating
                //App.jbSettings.Test = App.jbSettings.betBalance;

                if (pay == Cards.VegasPayoutHands.RoyalFlush
                 || pay == Cards.VegasPayoutHands.StraightFlush)
                {
                    textBlock1.Text = "HOLD";
                    textBlock2.Text = "HOLD";
                    textBlock3.Text = "HOLD";
                    textBlock4.Text = "HOLD";
                    textBlock5.Text = "HOLD";

                    // don't allow them to change a awesome hand
                    ucCard1.button1.IsEnabled = false;
                    ucCard2.button1.IsEnabled = false;
                    ucCard3.button1.IsEnabled = false;
                    ucCard4.button1.IsEnabled = false;
                    ucCard5.button1.IsEnabled = false;
                }

                ShowHints(currentHand);

                btnBet.IsEnabled = false;
                btnMultiplier.IsEnabled = false;
                btnDeal.Content = "Draw";
            }
            else if (btnDeal.Content.Equals("Draw"))
            {
                HideHints();
                // check current deck if it is tombstoned
                if (currentDeck == null)
                    currentDeck = App.jbSettings.currentDeck;

                int idxCard = 5;
                if (!textBlock1.Text.Equals("HOLD"))
                {
                    currentHand[0] = currentDeck[idxCard];
                    if (bShowSexyGirls)
                        ucCard1.SetSexyCard(currentHand[0]);
                    else
                        ucCard1.SetCard(currentHand[0]);

                    idxCard += 1;
                }

                if (!textBlock2.Text.Equals("HOLD"))
                {
                    currentHand[1] = currentDeck[idxCard];
                    if (bShowSexyGirls)
                        ucCard2.SetSexyCard(currentHand[1]);
                    else
                        ucCard2.SetCard(currentHand[1]);
                    idxCard += 1;
                }

                if (!textBlock3.Text.Equals("HOLD"))
                {
                    currentHand[2] = currentDeck[idxCard];
                    if (bShowSexyGirls)
                        ucCard3.SetSexyCard(currentHand[2]);
                    else
                        ucCard3.SetCard(currentHand[2]);
                    idxCard += 1;
                }

                if (!textBlock4.Text.Equals("HOLD"))
                {
                    currentHand[3] = currentDeck[idxCard];
                    if (bShowSexyGirls)
                        ucCard4.SetSexyCard(currentHand[3]);
                    else
                        ucCard4.SetCard(currentHand[3]);
                    idxCard += 1;
                }

                if (!textBlock5.Text.Equals("HOLD"))
                {
                    currentHand[4] = currentDeck[idxCard];
                    if (bShowSexyGirls)
                        ucCard5.SetSexyCard(currentHand[4]);
                    else
                        ucCard5.SetCard(currentHand[4]);
                    idxCard += 1;
                }
                
                ucCard1.button1.IsEnabled = false;
                ucCard2.button1.IsEnabled = false;
                ucCard3.button1.IsEnabled = false;
                ucCard4.button1.IsEnabled = false;
                ucCard5.button1.IsEnabled = false;

                // save cards to isoloated storage
                App.jbSettings.card1 = currentHand[0];
                App.jbSettings.card2 = currentHand[1];
                App.jbSettings.card3 = currentHand[2];
                App.jbSettings.card4 = currentHand[3];
                App.jbSettings.card5 = currentHand[4];

                Cards.VegasPayoutHands pay = c.ProjectPayback(currentHand);
                if (pay == Cards.VegasPayoutHands.RoyalFlush)
                    App.jbSettings.royalFlushCount += 1;

                int iCredits = Convert.ToInt16(txtCredits.Text);
                int iBet = _currentBet * _multiplier;
                int iPay = (int)pay * iBet;

                HighlightPayout(pay);

                txtPayout.Text = "You Win " + iPay.ToString() + "!";

                iCredits += iPay;
                App.jbSettings.betBalance = iCredits;
                if (iCredits > App.jbSettings.maximumBalance)
                    App.jbSettings.maximumBalance = iCredits;

                //App.jbSettings.Test = iCredits;
                if (iPay > 0)  // winner!
                {
                    txtCredits.Text = Convert.ToString(iCredits);                  
                    txtPayout.Visibility = Visibility.Visible;

                    if (App.jbSettings.isSoundOn) _sfWinBeep.Play();  //medWinBeep.Play();

                    App.jbSettings.sessionWinCount += 1;
                    App.jbSettings.winCount += 1;
                    _winStreak += 1;
                    _loseStreak = 0;
                    txtStatus.Text = "Press Deal to play again";

                    if (_winStreak > App.jbSettings.mostWinsInRow)
                        App.jbSettings.mostWinsInRow = _winStreak;
                }
                else  // loser
                {
                    App.jbSettings.sessionLoseCount += 1;
                    App.jbSettings.loseCount += 1;
                    _loseStreak += 1;
                    _winStreak = 0;
                    txtStatus.Text = "You Lose, try again";

                    if (_loseStreak > App.jbSettings.mostLosesInRow)
                        App.jbSettings.mostLosesInRow = _loseStreak;
                }

                ResetHoldText();

                btnBet.IsEnabled = true;
                btnMultiplier.IsEnabled = true;
                btnDeal.Content = "Deal";
                rectPayout.Visibility = Visibility.Visible;

                EnableDisableDealButton();
            }
            //else if (btnDeal.Content.Equals("Add 100"))
            //{
            //    txtCredits.Text = "100";
            //    btnDeal.Content = "Deal";
            //    txtWin.Text = "";
            //    btnBet.IsEnabled = true;
            //    btnMultiplier.IsEnabled = true;
            //    EnableDisableDealButton();

            //    App.jbSettings.balanceReloads += 1;
            //    App.jbSettings.betBalance = 100;
            //}

            // save button state
            App.jbSettings.btnDealIsEnabled = btnDeal.IsEnabled;
            App.jbSettings.btnBetIsEnabled = btnBet.IsEnabled;
            App.jbSettings.btnMultiplierIsEnabled = btnMultiplier.IsEnabled;
            App.jbSettings.btnDealText = (string)btnDeal.Content;
            App.jbSettings.SaveToIsolatedStorage();
        }

        #region " Bet Click event            "

        private void btnBet_Click(object sender, RoutedEventArgs e)
        {
            //int iBet = Convert.ToInt16(txtBetAmount.Text);
            int iCredits = Convert.ToInt16(txtCredits.Text);
            int maxBet = App.jbSettings.maxBetAllowed;

            if (_currentBet == maxBet) _currentBet = 1;
            else _currentBet += 1;

            EnableDisableDealButton();

            SetBetFields();

            PopulatePayoutList(_currentBet * _multiplier);

            if (App.jbSettings.isSoundOn) _sfBeep.Play();       //medBeep.Play(); 

        }
        #endregion

        #region "Card Click events            "

        //private void ucCard1_ManuiplationCompleted(object sender, ManipulationCompletedEventArgs e)
        //{
        //    if (e.TotalManipulation.Translation.X < -100)
        //    {

        //    }
        //}

        private void ucCard1_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeal.Content.Equals("Deal")) return;

            if (textBlock1.Text.Equals("HOLD"))
                textBlock1.Text = string.Empty;
            else
                textBlock1.Text = "HOLD";
        }

        private void ucCard2_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeal.Content.Equals("Deal")) return;

            if (textBlock2.Text.Equals("HOLD"))
                textBlock2.Text = string.Empty;
            else
                textBlock2.Text = "HOLD";
        }

        private void ucCard3_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeal.Content.Equals("Deal")) return;

            if (textBlock3.Text.Equals("HOLD"))
                textBlock3.Text = string.Empty;
            else
                textBlock3.Text = "HOLD";
        }

        private void ucCard4_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeal.Content.Equals("Deal")) return;

            if (textBlock4.Text.Equals("HOLD"))
                textBlock4.Text = string.Empty;
            else
                textBlock4.Text = "HOLD";
        }

        private void ucCard5_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeal.Content.Equals("Deal")) return;

            if (textBlock5.Text.Equals("HOLD"))
                textBlock5.Text = string.Empty;
            else
                textBlock5.Text = "HOLD";
        }

        #endregion

        #region " Multiplier click event             "

        private void btnMultiplier_Click(object sender, RoutedEventArgs e)
        {
            //if (_multiplier == 1)       _multiplier = 2;
            //else if (_multiplier == 2)  _multiplier = 5;
            //else if (_multiplier == 5)  _multiplier = 10;
            //else if (_multiplier == 10) _multiplier = 25;
            //else if (_multiplier == 25) _multiplier = 1;

            //SetMultiplierButton(_multiplier);
            //EnableDisableDealButton();
            //PopulatePayoutList(_currentBet * _multiplier);
            ////App.jbSettings.multiplier = _multiplier;
            //SetBetFields();

            //if (App.jbSettings.isSoundOn)  _sfBeep.Play(); 
        }

        private void SetBetFields()
        {
            txtBetAmount.Text = (_currentBet * _multiplier).ToString();
            btnBet.Content = "Bet " + _currentBet.ToString();
            App.jbSettings.lastBet = _currentBet;
        }

        /// <summary>
        /// This sets the Multiplier button text
        /// </summary>
        /// <param name="pMultiplier">Multiplier Number (1, 2, 5, 10, 25)</param>
        private void SetMultiplierButton(int pMultiplier)
        {
            //ImageBrush imgChip = new ImageBrush();
            Image content = btnMultiplier.Content as Image;


            switch (pMultiplier)
            {
                case 1:
                    //btnMultiplier.Content = "1x";
                    //lblBet.Text = "Bet (1x)";
                    //lblBet.FontSize = 24;
                    //imgChip.ImageSource = new BitmapImage(new Uri("/DrawPokerBasic;component/Images/PokerChip1.png", UriKind.Relative));
                    content.Source = new BitmapImage(new Uri("/DrawPokerBasic;component/Images/PokerChip1.png", UriKind.Relative));
                    break;
                case 2:
                    //btnMultiplier.Content = "2x";
                    //lblBet.Text = "Bet (2x)";
                    //lblBet.FontSize = 24;
                    content.Source = new BitmapImage(new Uri("/DrawPokerBasic;component/Images/PokerChip2.png", UriKind.Relative));
                    break;
                case 5:
                    //btnMultiplier.Content = "5x";
                    //lblBet.Text = "Bet (5x)";
                    //lblBet.FontSize = 24;
                    content.Source = new BitmapImage(new Uri("/DrawPokerBasic;component/Images/PokerChip5.png", UriKind.Relative));
                    break;
                case 10:
                    //btnMultiplier.Content = "10x";
                    //lblBet.Text = "Bet (10x)";
                    //lblBet.FontSize = 18;
                    content.Source = new BitmapImage(new Uri("/DrawPokerBasic;component/Images/PokerChip10.png", UriKind.Relative));
                    break;
                case 25:
                    //btnMultiplier.Content = "25x";
                    //lblBet.Text = "Bet (25x)";
                    //lblBet.FontSize = 18;
                    content.Source = new BitmapImage(new Uri("/DrawPokerBasic;component/Images/PokerChip25.png", UriKind.Relative));
                    break;
                default:
                    //btnMultiplier.Content = "1x";
                    //lblBet.Text = "Bet (1x)";
                    //lblBet.FontSize = 24;
                    content.Source = new BitmapImage(new Uri("/DrawPokerBasic;component/Images/PokerChip1.png", UriKind.Relative));
                    break;
            }
            // if using background, the image disappears when button is disabled
            //btnMultiplier.Background = imgChip;

        }
        #endregion

        private void abStrategy_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/JacksOrBetterStrategy.xaml",  UriKind.Relative));
        }

        private void lblBet_Tap(object sender, GestureEventArgs e)
        {
            grdBetAmount.Visibility = System.Windows.Visibility.Visible;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            grdBetAmount.Visibility = System.Windows.Visibility.Collapsed;
            _currentBet = Convert.ToInt16(txtBetAmountPop.Text);

            if (txtBetAmount.Text != txtBetAmountPop.Text)
                txtStatus.Text = "Bet changed";
            else
                txtStatus.Text = "";

            txtBetAmount.Text = txtBetAmountPop.Text;
            App.jbSettings.lastBet = _currentBet;
            
            PopulatePayoutList(_currentBet * _multiplier);

            int iCredits = Convert.ToInt16(txtCredits.Text);
            if ((_currentBet * _multiplier) > iCredits)
            {
                btnDeal.IsEnabled = false;
            }
            else
                btnDeal.IsEnabled = true;
        }

        private void SlideBet_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (bLoading) return;

            txtBetAmountPop.Text = Math.Round(e.NewValue).ToString();
        }

        private void hlkBet_Click(object sender, RoutedEventArgs e)
        {
            txtBetAmountPop.Text = txtBetAmount.Text;
            slideBet.Value = Convert.ToDouble(txtBetAmount.Text);
            grdBetAmount.Visibility = System.Windows.Visibility.Visible;

            int iCredits = Convert.ToInt16(txtCredits.Text);

            if (iCredits < 100)
                slideBet.Maximum = iCredits;
            else
                slideBet.Maximum = 100;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // hopefully, this code will eliminate the adcontrol errors that are occuring.

            AdControl ctrl = new AdControl("af90325a-6513-4dc3-bdcd-2f51315690d8", "10028503", false);
            
            ctrl.Height = 80;
            ctrl.Width = 480;
            ctrl.IsEnabled = true;
            ctrl.Visibility = System.Windows.Visibility.Visible;

            adControl1 = ctrl;

            base.OnNavigatedTo(e);
        }
    }
}