using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace DrawPokerBasic
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { LineOne = "Straight Flush", LineTwo = "Even if you have King high Straight Flush, take the payout!" });
            this.Items.Add(new ItemViewModel() { LineOne = "Four of Kind", LineTwo = "This is a no brainer, hold on to them and take the payout!" });
            this.Items.Add(new ItemViewModel() { LineOne = "4 to a Royal Flush", LineTwo = "Go for Royal Flush even if you have a natural Flush or Straight." });
            this.Items.Add(new ItemViewModel() { LineOne = "2 Pair or better", LineTwo = "Keep natural flushes and straights and don't split a pair." });
            this.Items.Add(new ItemViewModel() { LineOne = "4 to a Straight Flush", LineTwo = "You can win with flush, straight, and straight flush." });
            this.Items.Add(new ItemViewModel() { LineOne = "High Pair", LineTwo = "Keep the high pair to at least win your bet back." });
            this.Items.Add(new ItemViewModel() { LineOne = "3 to a Royal Flush", LineTwo = "This includes the 10 if it matches suit." });
            this.Items.Add(new ItemViewModel() { LineOne = "4 to a Flush", LineTwo = "19% chance to hit the Flush." });
            this.Items.Add(new ItemViewModel() { LineOne = "A-K-Q-J mixed suit", LineTwo = "Any card 10 or higher and you win." });
            this.Items.Add(new ItemViewModel() { LineOne = "2 suited high cards", LineTwo = "This is better than holding 3 high cards of mixed suit." });
            this.Items.Add(new ItemViewModel() { LineOne = "3 card Straight Flush", LineTwo = "Must be consecutive numbers of same suit (example: 6-7-8)." });
            this.Items.Add(new ItemViewModel() { LineOne = "K-Q-J mixed suit", LineTwo = "Chance of high pair and a outside Straight draw." });
            this.Items.Add(new ItemViewModel() { LineOne = "2 high cards", LineTwo = "2 high cards are better than holding on to 1 high card." });
            this.Items.Add(new ItemViewModel() { LineOne = "1 high card", LineTwo = "Statistically better than drawing 5 new cards." });
            this.Items.Add(new ItemViewModel() { LineOne = "3 suited cards", LineTwo = "If you have no high cards, this is better than discarding everything." });
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}