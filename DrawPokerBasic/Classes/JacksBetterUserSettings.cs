using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using DrawPokerLibrary;

namespace DrawPokerBasic
{
    /// <summary>
    ///  This is the users settings and statistics for Jacks or Better
    /// </summary>
    /// <remarks>This data is persisted to isolated storage</remarks>
    public class JacksBetterUserSettings
    {

        // user play settings
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
       // private PhoneApplicationService appService = PhoneApplicationService.Current;


        //public int Test
        //{
        //    get { return (int)appService.State["test1"]; }
        //    set { appService.State["test1"] = value; }
        //}

        /// <summary>
        /// Card 1
        /// </summary>
        public Card card1
        {
            get { return (Card)userSettings["card1"]; }
            set { userSettings["card1"] = value; }
        }
        /// <summary>
        /// Card 2
        /// </summary>
        public Card card2
        {
            get { return (Card)userSettings["card2"]; }
            set { userSettings["card2"] = value; }
        }
        /// <summary>
        /// Card 3
        /// </summary>
        public Card card3
        {
            get { return (Card)userSettings["card3"]; }
            set { userSettings["card3"] = value; }
        }
        /// <summary>
        /// Card 4
        /// </summary>
        public Card card4
        {
            get { return (Card)userSettings["card4"]; }
            set { userSettings["card4"] = value; }
        }
        /// <summary>
        /// Card 5
        /// </summary>
        public Card card5
        {
            get { return (Card)userSettings["card5"]; }
            set { userSettings["card5"] = value; }
        }

        public Card[] currentDeck
        {
            get { return (Card[])userSettings["currentDeck"]; }
            set { userSettings["currentDeck"] = value; }
        }

        /// <summary>
        ///  Last Bet made
        /// </summary>
        public int lastBet 
        {
            get { return (int)userSettings["lastBet"]; }
            set { userSettings["lastBet"] = value; }
        }

        /// <summary>
        /// Last multiplier setting
        /// </summary>
        public int multiplier
        {
            get { return (int)userSettings["multiplier"]; }
            set { userSettings["multiplier"] = value; }
        }

        /// <summary>
        /// This is how much money they have to play with
        /// </summary>
        public int betBalance
        {
            get { return (int)userSettings["betBalance"]; }
            set { userSettings["betBalance"] = value; }
        }
        /// <summary>
        /// For the future, currently selected deck they are playing with
        /// </summary>
        public string selectedDeck
        {
            get { return (string)userSettings["selectedDeck"]; }
            set { userSettings["selectedDeck"] = value; }
        }
        /// <summary>
        /// Maximum allowed bet
        /// </summary>
        public int maxBetAllowed
        {
            get { return (int)userSettings["maxBetAllowed"]; }
            set { userSettings["maxBetAllowed"] = value; }
        }
        /// <summary>
        /// Game speed is how fast the cards are displayed to the user.
        /// Not sure why a user wants to play in slow mode.
        /// </summary>
        public int gameSpeed
        {
            get { return (int)userSettings["gameSpeed"]; }
            set { userSettings["gameSpeed"] = value; }
        }
        // statistics

        /// <summary>
        ///  Number of wins in this current session
        /// </summary>
        public int sessionWinCount
        {
            get { return (int)userSettings["sessionWinCount"]; }
            set { userSettings["sessionWinCount"] = value; }
        }
        /// <summary>
        ///  Number of loses in this current session
        /// </summary>
        public int sessionLoseCount
        {
            get { return (int)userSettings["sessionLoseCount"]; }
            set { userSettings["sessionLoseCount"] = value; }
        }
        /// <summary>
        /// Number of hands in this current session
        /// </summary>
        public int sessionHandCount
        {
            get { return (int)userSettings["sessionHandCount"]; }
            set { userSettings["sessionHandCount"] = value; }
        }
        /// <summary>
        /// How much money they started playing with in this session
        /// </summary>
        public int sessionBetBalance
        {
            get { return (int)userSettings["sessionBetBalance"]; }
            set { userSettings["sessionBetBalance"] = value; }
        }
        /// <summary>
        /// Number of hands the player has played
        /// </summary>
        public int handCount
        {
            get { return (int)userSettings["handCount"]; }
            set { userSettings["handCount"] = value; }
        }
        /// <summary>
        /// Number of winning hands
        /// </summary>
        public int winCount
        {
            get { return (int)userSettings["winCount"]; }
            set { userSettings["winCount"] = value; }
        }
        /// <summary>
        /// Number of losing hands
        /// </summary>
        public int loseCount
        {
            get { return (int)userSettings["loseCount"]; }
            set { userSettings["loseCount"] = value; }
        }
        /// <summary>
        /// Most consective winning hands
        /// </summary>
        public int mostWinsInRow
        {
            get { return (int)userSettings["mostWinsInRow"]; }
            set { userSettings["mostWinsInRow"] = value; }
        }
        /// <summary>
        /// Most consective losing hands
        /// </summary>
        public int mostLosesInRow
        {
            get { return (int)userSettings["mostLosesInRow"]; }
            set { userSettings["mostLosesInRow"] = value; }
        }
        /// <summary>
        /// Number of royal flushes obtained
        /// </summary>
        public int royalFlushCount
        {
            get { return (int)userSettings["royalFlushCount"]; }
            set { userSettings["royalFlushCount"] = value; }
        }
        /// <summary>
        /// Number of times the player ran out of money and had to reload their bank
        /// </summary>
        public int balanceReloads
        {
            get { return (int)userSettings["balanceReloads"]; }
            set { userSettings["balanceReloads"] = value; }
        }
        /// <summary>
        /// This will store their largest winning payout
        /// </summary>
        public int largestPayout
        {
            get { return (int)userSettings["largestPayout"]; }
            set { userSettings["largestPayout"] = value; }
        }
        /// <summary>
        /// This is the highest balance player has attained
        /// </summary>
        public int maximumBalance
        {
            get { return (int)userSettings["maximumBalance"]; }
            set { userSettings["maximumBalance"] = value; }
        }

        //-----------------------------
        // button state
        //-----------------------------
        /// <summary>
        /// Is Bet button enabled?
        /// </summary>
        public bool btnBetIsEnabled
        {
            get { return (bool)userSettings["btnBetIsEnabled"]; }
            set { userSettings["btnBetIsEnabled"] = value; }
        }
        /// <summary>
        /// Is Multiplier button enabled?
        /// </summary>
        public bool btnMultiplierIsEnabled
        {
            get { return (bool)userSettings["btnMultiplierIsEnabled"]; }
            set { userSettings["btnMultiplierIsEnabled"] = value; }
        }
        /// <summary>
        /// Is Deal/Draw button enabled?
        /// </summary>
        public bool btnDealIsEnabled
        {
            get { return (bool)userSettings["btnDealIsEnabled"]; }
            set { userSettings["btnDealIsEnabled"] = value; }
        }
        /// <summary>
        /// Deal or Draw?
        /// </summary>
        public string btnDealText
        {
            get { return (string)userSettings["btnDealText"]; }
            set { userSettings["btnDealText"] = value; }
        }

        /// <summary>
        /// Sound toggle for in game noise
        /// </summary>
        public bool isSoundOn
        {
            get { return (bool)userSettings["isSoundOn"]; }
            set { userSettings["isSoundOn"] = value; }
        }

        /// <summary>
        /// hint mode is an aid to assist users on which cards to hold
        /// </summary>
        public bool hintMode
        {
            get { return (bool)userSettings["hintMode"]; }
            set { userSettings["hintMode"] = value; }
        }

        //-----------------------------
        //social networks
        //-----------------------------
        /// <summary>
        /// Facebook user name
        /// </summary>
        /// <remarks>This is for future integration with Facebook</remarks>
        public string facebookName
        {
            get { return (string)userSettings["facebookName"]; }
            set { userSettings["facebookName"] = value; }
        }
        /// <summary>
        /// Twitter account name
        /// </summary>
        /// <remarks>This is for future integration with Twitter</remarks>
        public string twitterAccount
        {
            get { return (string)userSettings["twitterAccount"]; }
            set { userSettings["twitterAccount"] = value; }
        }
        // have they provided feedback
        /// <summary>
        /// Indicator if the user has submitted a feedback, 
        /// set to True if they submitted a feedback.
        /// </summary>
        /// <remarks></remarks>
        public bool feedbackStatus
        {
            get { return (bool)userSettings["feedbackStatus"]; }
            set { userSettings["feedbackStatus"] = value; }
        }
        /// <summary>
        /// Popup will ask if user wants to provide a feedback, if they want to say NO,
        /// set this flag to True
        /// </summary>
        /// <remarks></remarks>
        public bool feedbackIgnore
        {
            get { return (bool)userSettings["feedbackIgnore"]; }
            set { userSettings["feedbackIgnore"] = value; }
        }
        /// <summary>
        /// Indicator if this app will display ads or not
        /// </summary>
        /// <remarks>This flag is currently defaulted to TRUE</remarks>
        public bool showAds
        {
            get { return (bool)userSettings["showAds"]; }
            set { userSettings["showAds"] = value; }
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (userSettings.Contains(Key))
            {
                // If the value has changed
                if (userSettings[Key] != value)
                {
                    // Store the new value
                    userSettings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                userSettings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (userSettings.Contains(Key))
            {
                value = (T)userSettings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Manually save to Isolated Storage
        /// </summary>
        public void SaveToIsolatedStorage()
        {
            userSettings.Save();
        }

        public JacksBetterUserSettings()
        {
            //if (appService.State.ContainsKey("test1") == false)
            //    appService.State.Add("test1", 100);

            // always reset these
            if (userSettings.Contains("sessionWinCount"))
                userSettings["sessionWinCount"] = 0;
            else
                userSettings.Add("sessionWinCount", 0);

            if (userSettings.Contains("sessionLoseCount"))
                userSettings["sessionLoseCount"] = 0;
            else
                userSettings.Add("sessionLoseCount", 0);

            if (userSettings.Contains("sessionHandCount"))
                userSettings["sessionHandCount"] = 0;
            else
                userSettings.Add("sessionHandCount", 0);

            // it is the responsibility of the developer to do a 
            // lookup to see if this user paid for this app
            showAds = true;

            // all of these settings will be reset when loaded
            if (userSettings.Contains("card1") == false)
                userSettings.Add("card1", new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Ten });
            
            if (userSettings.Contains("card2") == false)
                userSettings.Add("card2", new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Jack });
            
            if (userSettings.Contains("card3") == false)
                userSettings.Add("card3", new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Queen });
            
            if (userSettings.Contains("card4") == false)
                userSettings.Add("card4", new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.King });

            if (userSettings.Contains("card5") == false)
                userSettings.Add("card5", new Card { Suit = Cards.Suits.Spade, Value = Cards.CardValues.Ace });


            if (userSettings.Contains("currentDeck") == false)
            {
                Cards c = new Cards();
                Card[] d1 = c.GetNewDeck();
                userSettings.Add("currentDeck", d1);
            }

            if (userSettings.Contains("gameSpeed") == false)
                userSettings.Add("gameSpeed", 2);

            if (userSettings.Contains("lastBet") == false)
                userSettings.Add("lastBet", 1);

            if (userSettings.Contains("multiplier") == false)
                userSettings.Add("multiplier", 1);

            if (userSettings.Contains("selectedDeck") == false)
                userSettings.Add("selectedDeck", "Default");

            if (userSettings.Contains("betBalance") == false)
                userSettings.Add("betBalance", 100);

            if (userSettings.Contains("largestPayout") == false)
                userSettings.Add("largestPayout", 0);

            if (userSettings.Contains("mostWinsInRow") == false)
                userSettings.Add("mostWinsInRow", 0);
          
            if (userSettings.Contains("mostLosesInRow") == false)
                userSettings.Add("mostLosesInRow", 0);
       
            if (userSettings.Contains("handCount") == false)
                userSettings.Add("handCount", 0);
       
            if (userSettings.Contains("winCount") == false)
                userSettings.Add("winCount", 0);
          
            if (userSettings.Contains("loseCount") == false)
                userSettings.Add("loseCount", 0);
          
            if (userSettings.Contains("maxBetAllowed") == false)
                userSettings.Add("maxBetAllowed", 5);
         
            if (userSettings.Contains("royalFlushCount") == false)
                userSettings.Add("royalFlushCount", 0);

            if (userSettings.Contains("maximumBalance") == false)
                userSettings.Add("maximumBalance", 100);

            if (userSettings.Contains("balanceReloads") == false)
                userSettings.Add("balanceReloads", 0);

            if (userSettings.Contains("feedbackStatus") == false)
                userSettings.Add("feedbackStatus", false);

            if (userSettings.Contains("feedbackIgnore") == false)
                userSettings.Add("feedbackIgnore", false);

            if (userSettings.Contains("btnBetIsEnabled") == false)
                userSettings.Add("btnBetIsEnabled", true);

            if (userSettings.Contains("btnMultiplierIsEnabled") == false)
                userSettings.Add("btnMultiplierIsEnabled", true);

            if (userSettings.Contains("btnDealIsEnabled") == false)
                userSettings.Add("btnDealIsEnabled", true);

            if (userSettings.Contains("btnDealText") == false)
                userSettings.Add("btnDealText", "Deal");

            if (userSettings.Contains("isSoundOn") == false)
                userSettings.Add("isSoundOn", true);

            if (userSettings.Contains("hintMode") == false)
                userSettings.Add("hintMode", false);

            if (userSettings.Contains("twitterAccount") == false)
                userSettings.Add("twitterAccount", "");

            if (userSettings.Contains("facebookName") == false)
                userSettings.Add("facebookName", "");

            if (userSettings.Contains("sessionBetBalance"))
                userSettings["sessionBetBalance"] = userSettings["betBalance"];
            else
                userSettings.Add("sessionBetBalance", 100);

        }
    }
}