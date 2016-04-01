using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DrawPokerBasic
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _lineOne;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineOne
        {
            get
            {
                return _lineOne;
            }
            set
            {
                if (value != _lineOne)
                {
                    _lineOne = value;
                    NotifyPropertyChanged("LineOne");
                }
            }
        }

        private string _lineTwo;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineTwo
        {
            get
            {
                return _lineTwo;
            }
            set
            {
                if (value != _lineTwo)
                {
                    _lineTwo = value;
                    NotifyPropertyChanged("LineTwo");
                }
            }
        }

        private string _lineThree;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineThree
        {
            get
            {
                return _lineThree;
            }
            set
            {
                if (value != _lineThree)
                {
                    _lineThree = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
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


    public class StrategyViewModel : INotifyPropertyChanged
    {
        private string _lineOne;
        /// <summary>
        /// Hold strategy
        /// </summary>
        /// <returns></returns>
        public string LineOne
        {
            get
            {
                return _lineOne;
            }
            set
            {
                if (value != _lineOne)
                {
                    _lineOne = value;
                    NotifyPropertyChanged("LineOne");
                }
            }
        }

        private string _lineTwo;
        /// <summary>
        /// Strategy description
        /// </summary>
        /// <returns></returns>
        public string LineTwo
        {
            get
            {
                return _lineTwo;
            }
            set
            {
                if (value != _lineTwo)
                {
                    _lineTwo = value;
                    NotifyPropertyChanged("LineTwo");
                }
            }
        }

        private string _StrategyImage;
        /// <summary>
        /// Displays a image of the strategy hand
        /// </summary>
        /// <returns></returns>
        public string StrategyImage
        {
            get
            {
                return _StrategyImage;
            }
            set
            {
                if (value != _StrategyImage)
                {
                    _StrategyImage = value;
                    NotifyPropertyChanged("StrategyImage");
                }
            }
        }

        private string _CardValue1;
        /// <summary>
        /// Displays a image of the strategy hand
        /// </summary>
        /// <returns></returns>
        public string CardValue1
        {
            get
            {
                return _CardValue1;
            }
            set
            {
                if (value != _CardValue1)
                {
                    _CardValue1 = value;
                    NotifyPropertyChanged("CardValue1");
                }
            }
        }

        public string CardValue2 { get; set; }
        public string CardValue3 { get; set; }
        public string CardValue4 { get; set; }
        public string CardValue5 { get; set; }

        private string _CardSuit1;
        /// <summary>
        /// Displays a image of the strategy hand
        /// </summary>
        /// <returns></returns>
        public string CardSuit1
        {
            get
            {
                return _CardSuit1;
            }
            set
            {
                if (value != _CardSuit1)
                {
                    _CardSuit1 = value;
                    NotifyPropertyChanged("CardSuit1");
                }
            }
        }
        public string CardSuit2 { get; set; }
        public string CardSuit3 { get; set; }
        public string CardSuit4 { get; set; }
        public string CardSuit5 { get; set; }


        private string _CardColor1;
        /// <summary>
        /// Displays a image of the strategy hand
        /// </summary>
        /// <returns></returns>
        public string CardColor1
        {
            get
            {
                return _CardColor1;
            }
            set
            {
                if (value != _CardColor1)
                {
                    _CardColor1 = value;
                    NotifyPropertyChanged("CardColor1");
                }
            }
        }

        public string CardColor2 { get; set; }
        public string CardColor3 { get; set; }
        public string CardColor4 { get; set; }
        public string CardColor5 { get; set; }

        
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