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

namespace DrawPokerBasic
{
    public class VegasPayoutClass
    {
        public VegasPayoutClass() { }
        public VegasPayoutClass(string pHand, int pPayout)
        {
            Hand = pHand;
            Payout = pPayout;
        }

        public string Hand { get; set; }
        public int Payout { get; set; }

        public override string ToString()
        {
            return Hand;
        }
    }
}
