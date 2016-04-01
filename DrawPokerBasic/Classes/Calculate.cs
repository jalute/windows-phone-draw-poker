using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace DrawPokerBasic.Classes
{
    public class Calculate
    {
        private Card[] currentDeck;
        private Card[] currentHand = new Card[5];

        //public Stats[] ListofBestHold;
        public List<Stats> ListofBestHold;

        public Calculate(Card[] pCurrentHand, Card[] pCurrentDeck)
        {
            Debug.Assert(pCurrentHand.Length == 5);

            currentDeck = pCurrentDeck;
            currentHand = pCurrentHand;
        }

        #region " Drawing Cards                 "

        public struct Stats
        {
            public Card[] hold_cards;
            public int win_count;
            public float win_average;
            public float win_amount_avg;
            public int win_total;

            public int total_hands;
            public Stats(int arraysize)
            {
                hold_cards = new Card[arraysize];
                total_hands = 0;
                win_count = 0;
                win_average = 0;
                win_amount_avg = 0;
                win_total = 0;
            }
        }


        private void DrawOneCard()
        {
            Card[] possible_hand = new Card[5];
            Stats[] myStats = new Stats[5];
            Cards myCards = new Cards();

            int iDeckCount = currentDeck.Length;
            int iHandCount = currentHand.Length;
            Cards.VegasPayoutHands payback;

            int idx = 0;
            int max_hands = iDeckCount - 5;
            int win_count = 0;
            int win_amount = 0;

            for (int c1 = 0; c1 <= iHandCount - 4; c1++)
            {
                for (int c2 = c1 +1; c2 <= iHandCount - 3; c2++)
                {
                    for (int c3 = c2 + 1; c3 <= iHandCount - 2; c3++)
                    {
                        for (int c4 = c3 + 1; c4 <= iHandCount - 1; c4++)
                        {
                            possible_hand[0] = currentHand[c1];
                            possible_hand[1] = currentHand[c2];
                            possible_hand[2] = currentHand[c3];
                            possible_hand[3] = currentHand[c4];

                            myStats[idx] = new Stats(4);

                            myStats[idx].hold_cards[0] = possible_hand[0];
                            myStats[idx].hold_cards[1] = possible_hand[1];
                            myStats[idx].hold_cards[2] = possible_hand[2];
                            myStats[idx].hold_cards[3] = possible_hand[3];

                            win_count = 0;
                            win_amount = 0;
                            for (int d1 = 5; d1 <= iDeckCount - 1; d1++)
                            {
                                possible_hand[4] = currentDeck[d1];

                                payback = myCards.ProjectPayback(possible_hand);
                                if (payback != Cards.VegasPayoutHands.PaiGow)
                                {
                                    win_count += 1;
                                    win_amount += (int)payback;
                                }
                            }

                            var _with1 = myStats[idx];
                            _with1.win_count = win_count;
                            _with1.win_average = win_count / max_hands;
                            _with1.win_amount_avg = win_amount / win_count;

                            idx = idx + 1;

                        }
                    }
                }
            }

            // add to list
            foreach (Stats h1 in myStats)
            {
                ListofBestHold.Add(h1);
            }

        }


        private void DrawThreeCards()
        {
            Card[] possible_hand = new Card[5];
            Stats[] myStats = new Stats[10];
            Cards myCards = new Cards();
            Cards.VegasPayoutHands payback;

            int iDeckCount = currentDeck.Length;
            int iHandCount = currentHand.Length;

            int max_hands = ((iDeckCount - 5) * (iDeckCount - 6) * (iDeckCount - 7)) / 6;

            bool hasPair;
            int idx = 0;
            int win_count = 0;
            int win_amount = 0;

            for (int c1 = 0; c1 <= iHandCount - 2; c1++)
            {
                for (int c2 = c1 + 1; c2 <= iHandCount - 1; c2++)
                {
                    possible_hand[0] = currentHand[c1];
                    possible_hand[1] = currentHand[c2];
                    myStats[idx] = new Stats(2);

                    myStats[idx].hold_cards[0] = possible_hand[0];
                    myStats[idx].hold_cards[1] = possible_hand[1];

                    win_count = 0;
                    win_amount = 0;
                    hasPair = (possible_hand[0].Value == possible_hand[1].Value);

                    for (short d1 = 5; d1 <= iDeckCount - 3; d1++)
                    {
                        for (short d2 = (short)(d1 + 1); d2 <= iDeckCount - 2; d2++)
                        {
                            for (short d3 = (short)(d2 + 1); d3 <= iDeckCount - 1; d3++)
                            {
                                possible_hand[2] = currentDeck[d1];
                                possible_hand[3] = currentDeck[d2];
                                possible_hand[4] = currentDeck[d3];

                                payback = myCards.ProjectPayback(possible_hand, hasPair);
                                if (payback != Cards.VegasPayoutHands.PaiGow)
                                {
                                    win_count += 1;
                                    win_amount += (int)payback;
                                }
                            }
                        }
                    }

                    myStats[idx].win_count = win_count;
                    myStats[idx].win_average = win_count / max_hands;
                    myStats[idx].win_amount_avg = win_amount / win_count;

                    idx = idx + 1;
                }
            }


            // add to list
            foreach (Stats h1 in myStats)
            {
                ListofBestHold.Add(h1);
            }

        }

        private void DrawTwoCards()
        {
            Card[] possible_hand = new Card[5];
            Stats[] myStats = new Stats[10];
            Cards myCards = new Cards();
            Cards.VegasPayoutHands payback;

            int iDeckCount = currentDeck.Length;
            int iHandCount = currentHand.Length;
            int max_hands = ((iDeckCount - 5) * (iDeckCount - 6)) / 2;
            int idx = 0;
            int win_count = 0;
            int win_amount = 0;

            for (int c1 = 0; c1 <= iHandCount - 3; c1++)
            {
                for (int c2 = c1 + 1; c2 <= iHandCount - 2; c2++)
                {
                    for (int c3 = c2 + 1; c3 <= iHandCount - 1; c3++)
                    {
                        possible_hand[0] = currentHand[c1];
                        possible_hand[1] = currentHand[c2];
                        possible_hand[2] = currentHand[c3];
                        myStats[idx] = new Stats(3);

                        myStats[idx].hold_cards[0] = possible_hand[0];
                        myStats[idx].hold_cards[1] = possible_hand[1];
                        myStats[idx].hold_cards[2] = possible_hand[2];

                        win_count = 0;
                        win_amount = 0;
                        for (short d1 = 5; d1 <= iDeckCount - 2; d1++)
                        {
                            for (short d2 = (short)(d1 + 1); d2 <= iDeckCount - 1; d2++)
                            {
                                possible_hand[3] = currentDeck[d1];
                                possible_hand[4] = currentDeck[d2];

                                payback = myCards.ProjectPayback(possible_hand);
                                if (payback != Cards.VegasPayoutHands.PaiGow)
                                {
                                    win_count += 1;
                                    win_amount += (int)payback;
                                }
                            }
                        }

                        myStats[idx].win_count = win_count;
                        myStats[idx].win_average = win_count / max_hands;
                        myStats[idx].win_amount_avg = win_amount / win_count;

                        idx = idx + 1;
                    }
                }
            }

            // add to list
            foreach (Stats h1 in myStats)
            {
                ListofBestHold.Add(h1);
            }

        }


        // this is might be too slow and REDUNDANT, we might replace it with stats
        private void DrawFourCards()
        {
            Cards myCards = new Cards();

            if (myCards.ProjectPayback(currentHand) != Cards.VegasPayoutHands.PaiGow)
                return;
            else
            {
                Cards.CardValues[] sorted_hand = {
			                                currentHand[0].Value,
			                                currentHand[1].Value,
			                                currentHand[2].Value,
			                                currentHand[3].Value,
			                                currentHand[4].Value
                		};

                    Array.Sort(sorted_hand);
                    if (myCards.HasPair(sorted_hand))
                            return;
            }

            Card[] possible_hand = new Card[5];
            Stats[] myStats = new Stats[5];
            Cards.VegasPayoutHands payback;

            // this is the equation

            // 47   46   45   44 
            // -- * -- * -- * --  
            //  4    3    2    1
            int max_hands = ((currentDeck.Length - 5) * (currentDeck.Length - 6) * (currentDeck.Length - 7) * (currentDeck.Length - 8)) / 24;
            //Dim max_hands2 As Integer = 0
            int idx = 0;            

            for (int c1 = 0; c1 <= currentHand.Length - 1; c1++)
            {
                possible_hand[0] = currentHand[c1];
                myStats[idx] = new Stats(1);

                myStats[idx].hold_cards[0] = possible_hand[0];

                int win_count = 0;
                int win_amount = 0;
                //max_hands2 = 0

                // draw cards
                for (short d1 = 5; d1 <= currentDeck.Length - 4; d1++)
                {
                    for (short d2 = (short)(d1 + 1); d2 <= currentDeck.Length - 3; d2++)
                    {
                        for (short d3 = (short)(d2 + 1); d3 <= currentDeck.Length - 2; d3++)
                        {
                            for (short d4 = (short)(d3 + 1); d4 <= currentDeck.Length - 1; d4++)
                            {
                                possible_hand[1] = currentDeck[d1];
                                possible_hand[2] = currentDeck[d2];
                                possible_hand[3] = currentDeck[d3];
                                possible_hand[4] = currentDeck[d4];

                                payback = myCards.ProjectPayback(possible_hand);
                                if (payback != Cards.VegasPayoutHands.PaiGow)
                                {
                                    win_count += 1;
                                    win_amount += (int)payback;
                                }
                                //max_hands2 += 1
                            }
                        }
                    }
                }

                myStats[idx].win_count = win_count;
                myStats[idx].win_average = win_count / max_hands;
                myStats[idx].win_amount_avg = win_amount / win_count;
                idx = idx + 1;
            }

            // add to list
            foreach (Stats h1 in myStats)
            {
                ListofBestHold.Add(h1);
            }
        }

        /// <summary>
        /// This will give us a rough estimate of values based on the number of high cards they have.
        /// This will avoid running 300,000 scenarios when drawing 4 cards.
        /// </summary>
        private void DrawFourofNothing()
        {
            Cards myCards = new Cards();
            if (myCards.ProjectPayback(currentHand) != Cards.VegasPayoutHands.PaiGow)
            {
                return;
            }
            else
            {
                Cards.CardValues[] sorted_hand = {
			            currentHand[0].Value,
			            currentHand[1].Value,
			            currentHand[2].Value,
			            currentHand[3].Value,
			            currentHand[4].Value
		            };

                Array.Sort(sorted_hand);
                if (myCards.HasPair(sorted_hand))
                    return;
            }

            int highCardCount = 0;
            for (int c1 = 0; c1 <= currentHand.Length - 1; c1++)
            {
                switch (currentHand[c1].Value)
                {
                    case Cards.CardValues.Ace:
                    case Cards.CardValues.King:
                    case Cards.CardValues.Queen:
                    case Cards.CardValues.Jack:
                        highCardCount++;
                        break;
                }
            }

            for (int c1 = 0; c1 <= currentHand.Length - 1; c1++)
            {
                Stats myStats = new Stats(1);
                myStats.hold_cards[0] = currentHand[c1];

                var _with5 = myStats;
                _with5.win_count = 100;

                switch (currentHand[c1].Value)
                {
                    case Cards.CardValues.Ace:
                    case Cards.CardValues.King:
                    case Cards.CardValues.Queen:
                    case Cards.CardValues.Jack:
                        if (highCardCount == 1)
                        {
                            _with5.win_average = (float)0.3329;
                            _with5.win_amount_avg = (float)1.41;
                        }
                        else if (highCardCount == 2)
                        {
                            _with5.win_average = (float)0.3229;
                            _with5.win_amount_avg = (float)1.41;
                        }
                        else if (highCardCount == 3)
                        {
                            _with5.win_average = (float)0.31;
                            _with5.win_amount_avg = (float)1.42;
                        }
                        else
                        {
                            _with5.win_average = (float)0.302;
                            _with5.win_amount_avg = (float)1.43;
                        }

                        break;
                    default:
                        if (highCardCount == 0)
                        {
                            _with5.win_average = (float)0.179;
                            _with5.win_amount_avg = (float)1.79;
                        }
                        else if (highCardCount == 1)
                        {
                            _with5.win_average = (float)0.1674;
                            _with5.win_amount_avg = (float)1.84;
                        }
                        else if (highCardCount == 2)
                        {
                            _with5.win_average = (float)0.1552;
                            _with5.win_amount_avg = (float)1.90;
                        }
                        else if (highCardCount == 3)
                        {
                            _with5.win_average = (float)0.144;
                            _with5.win_amount_avg = (float)1.97;
                        }
                        else if (highCardCount == 4)
                        {
                            _with5.win_average = (float)0.1298;
                            _with5.win_amount_avg = (float)1.99;
                        }
                        break;
                }
                ListofBestHold.Add(myStats);
            }

        }
        #endregion

    }
}
