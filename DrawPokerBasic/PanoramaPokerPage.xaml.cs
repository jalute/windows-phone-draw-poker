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


namespace DrawPokerBasic
{
    public partial class PanoramaPokerPage : PhoneApplicationPage
    {
        private const char vbTab = (char)9;
        private Random rr = new Random();
        private Card[] currentDeck;
        private Card[] currentHand = new Card[5];

        private int _multiplier = 1;
        private int _currentBet = 1;
        private int _winStreak = 0;
        private int _loseStreak = 0;
        private static SoundEffect _sfBeep, _sfWinBeep;

        private const string SUIT_SPADES = "♠";
        private const string SUIT_DIAMONDS = "♦";
        private const string SUIT_HEARTS = "♥";
        private const string SUIT_CLUBS = "♣";

        PhoneApplicationService appService = PhoneApplicationService.Current;
        System.Windows.Threading.DispatcherTimer _splashTimer;

        public PanoramaPokerPage()
        {
            InitializeComponent();

            spSplashScreen.Visibility = Visibility.Visible;
            panoromaMain.Visibility = Visibility.Collapsed;

            _splashTimer = new System.Windows.Threading.DispatcherTimer();
            this.Loaded += new RoutedEventHandler(Splash_Loaded);

            ucCard1.SetCard(App.jbSettings.card1);
            ucCard2.SetCard(App.jbSettings.card2);
            ucCard3.SetCard(App.jbSettings.card3);
            ucCard4.SetCard(App.jbSettings.card4);
            ucCard5.SetCard(App.jbSettings.card5);

            currentHand[0] = App.jbSettings.card1;
            currentHand[1] = App.jbSettings.card2;
            currentHand[2] = App.jbSettings.card3;
            currentHand[3] = App.jbSettings.card4;
            currentHand[4] = App.jbSettings.card5;

            if ((Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
            {
                txtWin.Foreground = new SolidColorBrush(Colors.Red);
            }
            txtWin.Text = string.Empty;
            txtLose.Visibility = System.Windows.Visibility.Collapsed;

            // retrieve saved settings from isolated storage
            _currentBet = App.jbSettings.lastBet;
            _multiplier = App.jbSettings.multiplier;
            if (App.jbSettings.betBalance > 0)
                txtCredits.Text = App.jbSettings.betBalance.ToString();
            else
                txtCredits.Text = "100";
            //btnDeal.Content = App.jbSettings.btnDealText;

            SetBetFields();
            PopulatePayoutList(_currentBet * _multiplier);
            SetMultiplierButton(_multiplier);
            EnableDisableDealButton();
            ResetHoldText();
            //   lblPayout.Visibility = Visibility.Collapsed;
            txtPayout.Visibility = Visibility.Collapsed;
            if ((string)btnDeal.Content == "Deal")
                rectPayout.Visibility = Visibility.Visible;
            else
                rectPayout.Visibility = Visibility.Collapsed;

            // initialize sound
            FrameworkDispatcher.Update();
            var winstream = TitleContainer.OpenStream(@"Sounds/beep-02.wav");
            var beepstream = TitleContainer.OpenStream(@"Sounds/beep-07.wav");

            _sfBeep = SoundEffect.FromStream(beepstream);
            _sfWinBeep = SoundEffect.FromStream(winstream);

            DataContext = App.ViewModel;

            // load learn listbox
            _PokerHands = new ObservableCollection<StrategyViewModel>();
            this.Loaded += new RoutedEventHandler(Learn_Loaded);

            // load strategy grid
            _Strategies = new ObservableCollection<StrategyViewModel>();
            this.Loaded += new RoutedEventHandler(Strategy_Loaded);

            // options
            chkSoundOn.Click += new RoutedEventHandler(chkSoundOn_click);
            chkSoundOn.IsChecked = App.jbSettings.isSoundOn;
        }

        #region "Splash loading routine                "

        private void Splash_Loaded(object sender, RoutedEventArgs e)
        {
            if (_splashTimer != null)
            {
                _splashTimer.Interval = new TimeSpan(0, 0, 3);
                _splashTimer.Tick += new EventHandler(_splashTimer_Tick);
                _splashTimer.Start();
            }
        }

        void _splashTimer_Tick(object sender, EventArgs e)
        {

            _splashTimer.Stop();
            _splashTimer.Tick -= new EventHandler(_splashTimer_Tick);
            _splashTimer = null;

            spSplashScreen.Visibility = Visibility.Collapsed;
            panoromaMain.Visibility = Visibility.Visible;

        }
        #endregion

        #region " Learning loading routines               "
        public ObservableCollection<StrategyViewModel> _PokerHands { get; private set; }
        public bool IsLearnLoaded
        {
            get;
            private set;
        }

        private void Learn_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsLearnLoaded)
            {
                this._PokerHands.Add(new StrategyViewModel()
                {
                    LineOne = "Royal Flush",
                    LineTwo = "Ten, Jack, Queen, King, Ace of the same suit.", 
                    CardValue1 = "10",
                    CardSuit1 = SUIT_SPADES,
                    CardColor1 = "Black",
                    CardValue2 = "J",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "Q",
                    CardSuit3 = SUIT_SPADES,
                    CardColor3 = "Black",
                    CardValue4 = "K",
                    CardSuit4 = SUIT_SPADES,
                    CardColor4 = "Black",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_SPADES,
                    CardColor5 = "Black"
                });

                this._PokerHands.Add(new StrategyViewModel()
                {
                    LineOne = "Straight Flush",
                    LineTwo = "Five suited cards in sequence.",
                    CardValue1 = "4",
                    CardSuit1 = SUIT_DIAMONDS,
                    CardColor1 = "Red",
                    CardValue2 = "5",
                    CardSuit2 = SUIT_DIAMONDS,
                    CardColor2 = "Red",
                    CardValue3 = "6",
                    CardSuit3 = SUIT_DIAMONDS,
                    CardColor3 = "Red",
                    CardValue4 = "7",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "8",
                    CardSuit5 = SUIT_DIAMONDS,
                    CardColor5 = "Red"
                });

                this._PokerHands.Add(new StrategyViewModel()
                {
                    LineOne = "Four of Kind",
                    LineTwo = "Four cards with the same number.",
                    CardValue1 = "A",
                    CardSuit1 = SUIT_CLUBS,
                    CardColor1 = "Black",
                    CardValue2 = "A",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "A",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "A",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "K",
                    CardSuit5 = SUIT_CLUBS,
                    CardColor5 = "Black"
                });

                this._PokerHands.Add(new StrategyViewModel()
                {
                    LineOne = "Full House",
                    LineTwo = "Three cards of one number and two cards of another.",
                    CardValue1 = "J",
                    CardSuit1 = SUIT_CLUBS,
                    CardColor1 = "Black",
                    CardValue2 = "J",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "J",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "4",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "4",
                    CardSuit5 = SUIT_CLUBS,
                    CardColor5 = "Black"
                });

                this._PokerHands.Add(new StrategyViewModel()
                {
                    LineOne = "Flush",
                    LineTwo = "Five cards of the same suit.",
                    CardValue1 = "2",
                    CardSuit1 = SUIT_DIAMONDS,
                    CardColor1 = "Red",
                    CardValue2 = "5",
                    CardSuit2 = SUIT_DIAMONDS,
                    CardColor2 = "Red",
                    CardValue3 = "9",
                    CardSuit3 = SUIT_DIAMONDS,
                    CardColor3 = "Red",
                    CardValue4 = "J",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_DIAMONDS,
                    CardColor5 = "Red"
                });

                this._PokerHands.Add(new StrategyViewModel()
                {
                    LineOne = "Straight",
                    LineTwo = "Five cards in sequence. Ace can be low or high.",
                    CardValue1 = "4",
                    CardSuit1 = SUIT_DIAMONDS,
                    CardColor1 = "Red",
                    CardValue2 = "5",
                    CardSuit2 = SUIT_CLUBS,
                    CardColor2 = "Black",
                    CardValue3 = "6",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "7",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "8",
                    CardSuit5 = SUIT_SPADES,
                    CardColor5 = "Black"
                });

                this._PokerHands.Add(new StrategyViewModel()
                {
                    LineOne = "Three of Kind",
                    LineTwo = "Three cards of the same number.", 
                    CardValue1 = "4",
                    CardSuit1 = SUIT_CLUBS,
                    CardColor1 = "Black",
                    CardValue2 = "4",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "4",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "J",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_CLUBS,
                    CardColor5 = "Black"
                });

                this._PokerHands.Add(new StrategyViewModel()
                {
                    LineOne = "Two Pair",
                    LineTwo = "Two pairs of cards with the same number.",
                    CardValue1 = "2",
                    CardSuit1 = SUIT_CLUBS,
                    CardColor1 = "Black",
                    CardValue2 = "2",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "J",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "4",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "4",
                    CardSuit5 = SUIT_CLUBS,
                    CardColor5 = "Black"
                });

                this._PokerHands.Add(new StrategyViewModel()
                {
                    LineOne = "Pair",
                    LineTwo = "Two cards with the same number.",
                    CardValue1 = "J",
                    CardSuit1 = SUIT_CLUBS,
                    CardColor1 = "Black",
                    CardValue2 = "J",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "2",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "5",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "4",
                    CardSuit5 = SUIT_CLUBS,
                    CardColor5 = "Black"
                });

                this.IsLearnLoaded = true;
            }

            lstLearnHands.ItemsSource = _PokerHands;
        }

        #endregion

        #region " Strategy loading routines               "

        public ObservableCollection<StrategyViewModel> _Strategies { get; private set; }

        public bool IsStrategyLoaded
        {
            get;
            private set;
        }

        private void Strategy_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsStrategyLoaded)
            {
                // Sample data; replace with real data

                // todo: replace this code with Resource data
                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "Straight Flush",
                    LineTwo = "Even if you have King high Straight Flush, take the payout!",
                    CardValue1 = "9",
                    CardSuit1 = SUIT_SPADES,
                    CardColor1 = "Black",
                    CardValue2 = "10",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "J",
                    CardSuit3 = SUIT_SPADES,
                    CardColor3 = "Black",
                    CardValue4 = "Q",
                    CardSuit4 = SUIT_SPADES,
                    CardColor4 = "Black",
                    CardValue5 = "K",
                    CardSuit5 = SUIT_SPADES,
                    CardColor5 = "Black"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "Four of Kind",
                    LineTwo = "This is a no brainer, hold on to them and take the payout!",
                    CardValue1 = "A",
                    CardSuit1 = SUIT_CLUBS,
                    CardColor1 = "Black",
                    CardValue2 = "A",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "A",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "A",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "K",
                    CardSuit5 = SUIT_CLUBS,
                    CardColor5 = "Black"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "4 to a Royal Flush",
                    LineTwo = "Go for Royal Flush even if you have a natural Flush or Straight.",
                    CardValue1 = "2",
                    CardSuit1 = SUIT_SPADES,
                    CardColor1 = "Black",
                    CardValue2 = "10",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "J",
                    CardSuit3 = SUIT_SPADES,
                    CardColor3 = "Black",
                    CardValue4 = "Q",
                    CardSuit4 = SUIT_SPADES,
                    CardColor4 = "Black",
                    CardValue5 = "K",
                    CardSuit5 = SUIT_SPADES,
                    CardColor5 = "Black"
                });


                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "Flushes and Straights",
                    LineTwo = "Keep natural flushes and straights unless you are one card away from a Royal Flush.",
                    CardValue1 = "2",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "3",
                    CardSuit2 = SUIT_HEARTS,
                    CardColor2 = "Red",
                    CardValue3 = "4",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "5",
                    CardSuit4 = SUIT_HEARTS,
                    CardColor4 = "Red",
                    CardValue5 = "J",
                    CardSuit5 = SUIT_HEARTS,
                    CardColor5 = "Red"
                });


                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "2 Pair",
                    LineTwo = "Don't split the 2 pairs. This gives you a chance of winning a Full House.",
                    CardValue1 = "4",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "4",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "K",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "K",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "J",
                    CardSuit5 = SUIT_CLUBS,
                    CardColor5 = "Black"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "4 to a Straight Flush",
                    LineTwo = "Keep the 4 cards with a chance to win with a flush, straight, or straight flush.",
                    CardValue1 = "2",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "3",
                    CardSuit2 = SUIT_HEARTS,
                    CardColor2 = "Red",
                    CardValue3 = "4",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "5",
                    CardSuit4 = SUIT_HEARTS,
                    CardColor4 = "Red",
                    CardValue5 = "5",
                    CardSuit5 = SUIT_SPADES,
                    CardColor5 = "Black"
                });


                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "High Pair",
                    LineTwo = "Keep the high pair to at least win your bet back. Discard all other cards",
                    CardValue1 = "A",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "9",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "8",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "J",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "J",
                    CardSuit5 = SUIT_CLUBS,
                    CardColor5 = "Black"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "3 to a Royal Flush",
                    LineTwo = "This includes the 10 if it matches suit.",
                    CardValue1 = "2",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "10",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "J",
                    CardSuit3 = SUIT_SPADES,
                    CardColor3 = "Black",
                    CardValue4 = "Q",
                    CardSuit4 = SUIT_SPADES,
                    CardColor4 = "Black",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_HEARTS,
                    CardColor5 = "Red"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "4 to a Flush",
                    LineTwo = "19% chance to hit the Flush.",
                    CardValue1 = "2",
                    CardSuit1 = SUIT_SPADES,
                    CardColor1 = "Black",
                    CardValue2 = "4",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "6",
                    CardSuit3 = SUIT_SPADES,
                    CardColor3 = "Black",
                    CardValue4 = "9",
                    CardSuit4 = SUIT_SPADES,
                    CardColor4 = "Black",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_HEARTS,
                    CardColor5 = "Red"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "A-K-Q-J mixed suit",
                    LineTwo = "Any card 10 or higher and you win.",
                    CardValue1 = "4",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "J",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "Q",
                    CardSuit3 = SUIT_CLUBS,
                    CardColor3 = "Black",
                    CardValue4 = "K",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_HEARTS,
                    CardColor5 = "Red"
                });


                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "2 suited high cards",
                    LineTwo = "This is better than holding 3 high cards of mixed suit.",
                    CardValue1 = "J",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "Q",
                    CardSuit2 = SUIT_HEARTS,
                    CardColor2 = "Red",
                    CardValue3 = "2",
                    CardSuit3 = SUIT_CLUBS,
                    CardColor3 = "Black",
                    CardValue4 = "4",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_SPADES,
                    CardColor5 = "Black"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "3 card Straight Flush",
                    LineTwo = "Must be consecutive numbers of same suit.",
                    CardValue1 = "4",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "5",
                    CardSuit2 = SUIT_HEARTS,
                    CardColor2 = "Red",
                    CardValue3 = "6",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "9",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "2",
                    CardSuit5 = SUIT_SPADES,
                    CardColor5 = "Black"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "K-Q-J mixed suit",
                    LineTwo = "Chance of high pair and a outside Straight draw.",
                    CardValue1 = "4",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "J",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "Q",
                    CardSuit3 = SUIT_CLUBS,
                    CardColor3 = "Black",
                    CardValue4 = "K",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_HEARTS,
                    CardColor5 = "Red"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "2 high cards",
                    LineTwo = "2 high cards are better than holding on to 1 high card.",
                    CardValue1 = "4",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "6",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "8",
                    CardSuit3 = SUIT_CLUBS,
                    CardColor3 = "Black",
                    CardValue4 = "J",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_HEARTS,
                    CardColor5 = "Red"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "1 high card",
                    LineTwo = "Statistically better than drawing 5 new cards.",
                    CardValue1 = "4",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "6",
                    CardSuit2 = SUIT_SPADES,
                    CardColor2 = "Black",
                    CardValue3 = "8",
                    CardSuit3 = SUIT_CLUBS,
                    CardColor3 = "Black",
                    CardValue4 = "9",
                    CardSuit4 = SUIT_DIAMONDS,
                    CardColor4 = "Red",
                    CardValue5 = "A",
                    CardSuit5 = SUIT_HEARTS,
                    CardColor5 = "Red"
                });

                this._Strategies.Add(new StrategyViewModel()
                {
                    LineOne = "3 suited cards",
                    LineTwo = "If you have no high cards, this is better than discarding everything.",
                    CardValue1 = "2",
                    CardSuit1 = SUIT_HEARTS,
                    CardColor1 = "Red",
                    CardValue2 = "5",
                    CardSuit2 = SUIT_HEARTS,
                    CardColor2 = "Red",
                    CardValue3 = "7",
                    CardSuit3 = SUIT_HEARTS,
                    CardColor3 = "Red",
                    CardValue4 = "9",
                    CardSuit4 = SUIT_SPADES,
                    CardColor4 = "Black",
                    CardValue5 = "10",
                    CardSuit5 = SUIT_CLUBS,
                    CardColor5 = "Black"
                });

                this.IsStrategyLoaded = true;
            }

            lstStrategy.ItemsSource = _Strategies;
        }

        #endregion

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

            if (lstPayouts.SelectedIndex == -1)
            {
                txtWin.Text = string.Empty;
            }
            else
            {
                txtWin.Text = lstPayouts.SelectedValue.ToString();
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

            if ((_currentBet * _multiplier) <= iCredits) btnDeal.IsEnabled = true;
            else btnDeal.IsEnabled = false;
        }

        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            //TODO: add a check here if phone is busy

            Cards c = new Cards();
            if (btnDeal.Content.Equals("Deal"))
            {
                rectPayout.Visibility = Visibility.Collapsed;
                txtPayout.Visibility = Visibility.Collapsed;
                txtLose.Visibility = Visibility.Collapsed;

                Card[] d1 = c.GetNewDeck();

                // shuffle the cards
                int max = rr.Next(50, 7000);
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

                ucCard1.SetCard(currentHand[0]);
                ucCard2.SetCard(currentHand[1]);
                ucCard3.SetCard(currentHand[2]);
                ucCard4.SetCard(currentHand[3]);
                ucCard5.SetCard(currentHand[4]);

                ucCard1.button1.IsEnabled = true;
                ucCard2.button1.IsEnabled = true;
                ucCard3.button1.IsEnabled = true;
                ucCard4.button1.IsEnabled = true;
                ucCard5.button1.IsEnabled = true;

                // save settings
                App.jbSettings.lastBet = _currentBet;
                App.jbSettings.multiplier = _multiplier;
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

                btnBet.IsEnabled = false;
                btnMultiplier.IsEnabled = false;
                btnDeal.Content = "Draw";
            }
            else if (btnDeal.Content.Equals("Draw"))
            {
                // check current deck if it is tombstoned
                if (currentDeck == null)
                    currentDeck = App.jbSettings.currentDeck;

                int idxCard = 5;
                if (!textBlock1.Text.Equals("HOLD"))
                {
                    currentHand[0] = currentDeck[idxCard];
                    ucCard1.SetCard(currentHand[0]);
                    idxCard += 1;
                }

                if (!textBlock2.Text.Equals("HOLD"))
                {
                    currentHand[1] = currentDeck[idxCard];
                    ucCard2.SetCard(currentHand[1]);
                    idxCard += 1;
                }

                if (!textBlock3.Text.Equals("HOLD"))
                {
                    currentHand[2] = currentDeck[idxCard];
                    ucCard3.SetCard(currentHand[2]);
                    idxCard += 1;
                }

                if (!textBlock4.Text.Equals("HOLD"))
                {
                    currentHand[3] = currentDeck[idxCard];
                    ucCard4.SetCard(currentHand[3]);
                    idxCard += 1;
                }

                if (!textBlock5.Text.Equals("HOLD"))
                {
                    currentHand[4] = currentDeck[idxCard];
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
                    if (_winStreak > App.jbSettings.mostWinsInRow)
                        App.jbSettings.mostWinsInRow = _winStreak;
                }
                else  // loser
                {
                    App.jbSettings.sessionLoseCount += 1;
                    App.jbSettings.loseCount += 1;
                    _loseStreak += 1;
                    _winStreak = 0;
                    if (_loseStreak > App.jbSettings.mostLosesInRow)
                        App.jbSettings.mostLosesInRow = _loseStreak;
                }

                ResetHoldText();

                btnBet.IsEnabled = true;
                btnMultiplier.IsEnabled = true;
                btnDeal.Content = "Deal";
                rectPayout.Visibility = Visibility.Visible;

                if (iCredits == 0)
                {
                    //MessageBox.Show("You have lost all of your credits. Replenish your credits by clicking 'CR 100+' button", "Bad Luck", MessageBoxButton.OK);
                    txtWin.Text = "Credits Gone!";
                    txtLose.Visibility = System.Windows.Visibility.Collapsed;
                    btnDeal.Content = "Add 100";
                    btnDeal.IsEnabled = true;
                    btnBet.IsEnabled = false;
                    btnMultiplier.IsEnabled = false;
                }
                else
                {
                    EnableDisableDealButton();
                    if (lstPayouts.SelectedIndex == -1)
                    {
                        txtLose.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtLose.Visibility = Visibility.Collapsed;
                    }

                }
            }
            else if (btnDeal.Content.Equals("Add 100"))
            {
                txtCredits.Text = "100";
                btnDeal.Content = "Deal";
                txtWin.Text = "";
                txtLose.Visibility = Visibility.Collapsed;
                btnBet.IsEnabled = true;
                btnMultiplier.IsEnabled = true;
                EnableDisableDealButton();

                App.jbSettings.balanceReloads += 1;
                App.jbSettings.betBalance = 100;
            }

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
            if (_multiplier == 1) _multiplier = 2;
            else if (_multiplier == 2) _multiplier = 5;
            else if (_multiplier == 5) _multiplier = 10;
            else if (_multiplier == 10) _multiplier = 25;
            else if (_multiplier == 25) _multiplier = 1;

            SetMultiplierButton(_multiplier);
            EnableDisableDealButton();
            PopulatePayoutList(_currentBet * _multiplier);
            App.jbSettings.multiplier = _multiplier;
            SetBetFields();

            if (App.jbSettings.isSoundOn) _sfBeep.Play();
        }

        private void SetBetFields()
        {
            txtBetAmount.Text = (_currentBet * _multiplier).ToString();
            btnBet.Content = "Bet x" + _currentBet.ToString();
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

        // Handle selection changed on Strategy ListBox
        private void Strategy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (lstStrategy.SelectedIndex == -1)
                return;

            // Reset selected index to -1 (no selection)
            lstStrategy.SelectedIndex = -1;
        }

        private void chkSoundOn_click(object sender, RoutedEventArgs e)
        {
            App.jbSettings.isSoundOn = (bool)chkSoundOn.IsChecked;
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
    }
}