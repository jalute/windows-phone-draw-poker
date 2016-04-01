using System;
using System.Xml.Serialization;
using System.Diagnostics;

namespace DrawPokerBasic
{
    public struct Card : IComparable
    {
        
        /// <summary>
        /// Card suit
        /// </summary>
        public Cards.Suits Suit;
        /// <summary>
        /// Card Value
        /// </summary>
        public Cards.CardValues Value;

        public int CompareTo(object obj)
        {
            Card otherCard = (Card)obj;

            return this.Value.CompareTo(otherCard.Value);
        }

        /// <summary>
        /// Converts the card into a string representation of the card (ie. c7 = 7 of clubs)
        /// </summary>
        /// <remarks>This is only used for debugging purposes</remarks>
        public String DebugOutput()
        {
            string ret = String.Empty;
            
            switch (Suit)
            {
                case Cards.Suits.Clubs:
                    ret = "c";
                    break;
                case Cards.Suits.Diamonds:
                    ret = "d";
                    break;
                case Cards.Suits.Hearts:
                    ret = "h";
                    break;
                case Cards.Suits.Spade:
                    ret = "s";
                    break;
            }

            switch (Convert.ToInt32(Value))
            {
                case 11:
                    ret = ret + "J";
                    break;
                case 12:
                    ret = ret + "Q";
                    break;
                case 13:
                    ret = ret + "K";
                    break;
                case 1:
                    ret = ret + "A";
                    break;
                default:
                    ret = ret + Convert.ToInt32(Value).ToString();
                    break;
            }

            return ret;
        }
    }
    public class Cards
    {
        [Flags()] public enum Suits {Spade, Hearts, Diamonds, Clubs};
        [Flags()] public enum CardValues : byte { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
        
        public enum VegasPayoutHands : short
        {
            PaiGow = 0,
            JacksOrBetterPair = 1,
            TwoPair = 2,
            ThreeOfKind = 3,
            Straight = 4,
            Flush = 6,
            FullHouse = 9,
            FourOfKind = 25,
            StraightFlush = 50,
            RoyalFlush = 800,
        }

        // reference variable are defined once at class level for increased performance
        private Card[] _standard_deck = new Card[52];
        private Random rr = new Random();        
        CardValues[] sorted_hand = new CardValues[5];
        Card[] cut_deck = new Card[52];
        Card[] shuffled_deck = new Card[52];

/// <summary>
/// This creates a new deck of 52 cards with no jokers.
/// </summary>
/// <returns>An array of Card of size 52 in numeric order</returns>
/// <remarks>This deck will need to be shuffled</remarks>
        public Card[] GetNewDeck()
        {
            int idx = 0;
            Card[] standard_deck = new Card[52];

            for (int iValue = 1; iValue < 14; iValue++)
            {
                CardValues cv = (CardValues)iValue;
                for (int iSuit = 0; iSuit < 4; iSuit++)
                {
                    Suits suit = (Suits)iSuit;
                    Card myCard;
                    myCard.Suit = suit;
                    myCard.Value = cv;
                    standard_deck[idx] = myCard;
                    idx += 1;
                }
            }

            return standard_deck;
        }

        /// <summary>
        /// This simulates a standard shuffle where the dealer splits the deck into 2,
        /// then with their thumbs, lift up and slide/merge the two decks together
        /// </summary>
        /// <param name="deck">The deck to shuffle</param>
        /// <param name="NumOfSteps">Number of splits that will be merged together, default is set to 2</param>
        /// <returns>Shuffled Deck</returns>
        public Card[] ShuffleSplit(Card[] deck, int NumOfSteps = 2)
        {
            int sidx = 0;
            int NumOfCards = deck.Length;

            for (int start = 0; start <= NumOfSteps - 1; start++)
            {
                for (int idx = start; idx <= NumOfCards - 1; idx += NumOfSteps)
                {
                    shuffled_deck[sidx] = deck[idx];
                    sidx += 1;
                }
            }

            //Debug.Assert(sidx = 52)
            return shuffled_deck;
        }

        /// <summary>
        /// This simulates if someone cut the deck and the dealer places the bottom half on top.
        /// </summary>
        /// <param name="deck">The deck to shuffle</param>
        /// <returns>Shuffled deck</returns>
        /// ***** THIS HAS A BUG IN IT ! ************
        //public Card[] ShuffleCut(Card[] deck)
        //{
        //    int splitidx = rr.Next(20, 32);
        //    Array.Copy(deck, splitidx, cut_deck, deck.GetLowerBound(0), deck.GetUpperBound(0) - splitidx);
        //    Array.Copy(deck, deck.GetLowerBound(0), cut_deck, deck.GetUpperBound(0) - splitidx, splitidx);

        //    return cut_deck;
        //}


        /// <summary>
        /// This simulates how a vegas shuffle machine works.  It basically pops out
        /// one card at a time from the start deck from a random spot in the middle
        /// to create a new "random" deck.
        /// </summary>
        /// <param name="deck">The deck to shuffle</param>
        /// <returns>Randomly shuffled deck</returns>
        public Card[] VegasShuffle(Card[] deck)
        {
            Card[] shuffled_deck = new Card[52];
            Card[] short_deck = deck;

            int pos = 0;

            for (int idx = deck.Length - 1; idx >= 0; idx += -1)
            {
                if (idx == 0)
                {
                    shuffled_deck[51] = short_deck[0];
                }
                else
                {
                    int z = rr.Next(idx);
                    shuffled_deck[pos] = short_deck[z];
                    //GetRandomCard(short_deck)

                    // remove card and rebuild short deck
                    Card[] tmp = new Card[short_deck.Length-1];     //Where((x, y) => y != z);

                    int tPos = 0;
                    foreach (Card c in short_deck)
                    {
                        if (!short_deck[z].Equals(c))
                        {
                            tmp[tPos] = c;
                            tPos += 1;
                        }

                    }
                    short_deck = tmp;
                }

                pos += 1;
            }

            return shuffled_deck;
        }

        public VegasPayoutHands ProjectPayback(Card[] hand)
        {
            return ProjectPayback(hand, false);
        }

        /// <summary>
        /// This looks at the hand and determines if it won anything
        /// </summary>
        /// <param name="hand">The hand the player is holding</param>
        /// <param name="hasPair">Set to True if we already know we have a pair to avoid checking for flushes and straights</param>
        /// <returns>Payout</returns>
        public VegasPayoutHands ProjectPayback(Card[] hand, bool hasPair)
        {
            if (hand.Length != 5) return VegasPayoutHands.PaiGow;

            // changed code for increased performance
            //CardValues[] sorted_hand = new CardValues[5] { hand[0].Value, hand[1].Value, hand[2].Value, hand[3].Value, hand[4].Value };
            sorted_hand[0] = hand[0].Value;
            sorted_hand[1] = hand[1].Value;
            sorted_hand[2] = hand[2].Value;
            sorted_hand[3] = hand[3].Value;
            sorted_hand[4] = hand[4].Value;

            // sort the cards in order (requirement of function calls below) using insertion sort algorithm
            // Array.Sort is way too slow
            CardValues value;
            int j;
            for (int i = 1; i < 5; i++)
            {
                value = sorted_hand[i];
                j = i - 1;
                bool done = false;
                do
                {
                    if (sorted_hand[j] > value)
                    {
                        sorted_hand[j + 1] = sorted_hand[j];
                        j--;
                        if (j < 0) done = true;
                    }
                    else
                        done = true;
                } while (!done);

                sorted_hand[j + 1] = value;
            }

	        if (HasFourOfKind(sorted_hand)) {
		        return VegasPayoutHands.FourOfKind;
	        } else if (HasFullHouse(sorted_hand)) {
		        return VegasPayoutHands.FullHouse;
	        } else if (!hasPair && HasFlush(hand)) {
		        if (HasStraight(sorted_hand)) {
			        // we need to check if its a royal flush!!
			        if (sorted_hand[0] == CardValues.Ace 
                     && sorted_hand[1] == CardValues.Ten 
                     && sorted_hand[2] == CardValues.Jack 
                     && sorted_hand[3] == CardValues.Queen 
                     && sorted_hand[4] == CardValues.King) {
				        return VegasPayoutHands.RoyalFlush;
			        } else {
				        return VegasPayoutHands.StraightFlush;
			        }
		        } else {
			        return VegasPayoutHands.Flush;
		        }
	        } else if (!hasPair && HasStraight(sorted_hand)) {
		        return VegasPayoutHands.Straight;
	        } else if (HasThreeOfKind(sorted_hand)) {
		        return VegasPayoutHands.ThreeOfKind;
	        } else if (HasTwoPairs(sorted_hand)) {
		        return VegasPayoutHands.TwoPair;
	        } else if (HasHighPair(sorted_hand)) {
		        return VegasPayoutHands.JacksOrBetterPair;
	        }

            // we got nothing
	        return VegasPayoutHands.PaiGow;
        }


#region "Check for poker hand routines       "

        /// <summary>
        /// Check for Jacks or Better hand
        /// </summary>
        /// <param name="cvs">Card values in numeric order</param>
        /// <remarks>The hand values be presorted before calling this function</remarks>
        /// <returns>True if Jacks or better hand</returns>
        private bool HasHighPair(CardValues[] cvs)
        {
            Debug.Assert(cvs.Length >= 2);
            //if (cvs.Length >= 2)
            //{
                for (int idx = 0; idx <= cvs.Length - 2; idx++)
                {
                    // check for pair
                    if (cvs[idx] == cvs[idx + 1])
                    {
                        if (cvs[idx] >= CardValues.Jack | cvs[idx] == CardValues.Ace)
                        {
                            return true;
                        }
                    }
                }
            //}

            return false;
        }

        /// <summary>
        /// this looks for any pair
        /// </summary>
        /// <param name="cvs">Card values in numeric order</param>
        /// <remarks>The values must be presorted before calling this function</remarks>
        /// <returns>True if pair found</returns>
        public bool HasPair(CardValues[] cvs)
        {
            Debug.Assert(cvs.Length >= 2);
            //if (cvs.Length >= 2)
            //{
                for (int idx = 0; idx <= cvs.Length - 2; idx++)
                {
                    // check for pair
                    if (cvs[idx] == cvs[idx + 1])
                    {
                       return true;
                    }
                }
            //}

            return false;
        }

        /// <summary>
        /// This looks for two pairs
        /// </summary>
        /// <param name="cvs">Card values in numeric order</param>
        /// <remarks>The values must be presorted before calling this function</remarks>
        /// <returns>True if two pairs are found</returns>
        private bool HasTwoPairs(CardValues[] cvs)
        {
            Debug.Assert(cvs.Length == 5);

            //if (cvs.Length == 5)
            //{
                // there are only three combinations when sorted
                //             23344, 22344, 22334
                if (cvs[0] == cvs[1] & cvs[2] == cvs[3])    //22334
                {
                    return true;
                   
                }
                else if (cvs[1] == cvs[2] & cvs[3] == cvs[4])   //23344
                {
                    return true;
                   
                }
                else if (cvs[0] == cvs[1] & cvs[3] == cvs[4])   //22344
                {
                    return true;
                }
            //}
            return false;
        }

        /// <summary>
        /// Looks for three of a kind
        /// </summary>
        /// <param name="cvs">Card values in numeric order</param>
        /// <remarks>The values must be presorted before calling this function</remarks>
        /// <returns></returns>
        private bool HasThreeOfKind(CardValues[] cvs)
        {
            Debug.Assert(cvs.Length >= 3);

            //if (cvs.Length >= 3)
            //{
                for (int idx = 0; idx <= cvs.Length - 3; idx++)
                {
                    // check to see if all three cards match
                    if (cvs[idx] == cvs[idx + 1] & cvs[idx] == cvs[idx + 2])
                    {
                        return true;
                    }
                }
            //}

            return false;
        }

        /// <summary>
        /// See if the cards are a straight
        /// </summary>
        /// <param name="cvs">Card values in numeric order</param>
        /// <remarks>The values must be presorted before calling this function</remarks>
        /// <returns>True if straight found</returns>
        private bool HasStraight(CardValues[] cvs)
        {
            // if you have a pair, then you dont have a straight
            //if (HasPair(cvs))
            //    return false;

            Debug.Assert(cvs.Length == 5);

            //if (cvs.Length == 5)
            //{
                if (cvs[0] + 1 == cvs[1] && cvs[1] + 1 == cvs[2] && cvs[2] + 1 == cvs[3] && cvs[3] + 1 == cvs[4])
                {
                    return true;
                }
                else if (cvs[0] == CardValues.Ace)
                {
                    // we need to check for ace high staight
                    if (cvs[1] == CardValues.Ten && cvs[2] == CardValues.Jack && cvs[3] == CardValues.Queen && cvs[4] == CardValues.King)
                    {
                        return true;
                    }
                }
            //}
            return false;
        }

        /// <summary>
        /// This checks for a flush
        /// </summary>
        /// <param name="hand">The poker hand to check</param>
        /// <returns>True if all the cards are the same suit</returns>
        private bool HasFlush(Card[] hand)
        {
            Debug.Assert(hand.Length == 5);

            //if (hand.Length == 5)
            //{
                for (int idx = 0; idx <= hand.Length - 2; idx++)
                {
                    if (hand[idx].Suit != hand[idx + 1].Suit)
                    {
                        return false;   // any bad match and return false
                    }
                }
                // if we get this far, all the suits are the same
                return true;
            //}
            //return false;
        }

        /// <summary>
        /// Look for full house
        /// </summary>
        /// <param name="cvs">Card values in numeric order</param>
        /// <remarks>The values must be presorted before calling this function</remarks>
        /// <returns>True if full house found</returns>
        private bool HasFullHouse(CardValues[] cvs)
        {
            Debug.Assert(cvs.Length == 5);

            //if (cvs.Length == 5)
            //{
                // there are only two combinations because of sort
                // 33344 or 22333
                if (cvs[0] == cvs[1] && cvs[0] == cvs[2] && cvs[3] == cvs[4])
                {
                    // 33344
                    return true;
                }
                else if (cvs[0] == cvs[1] && cvs[2] == cvs[3] && cvs[2] == cvs[4])
                {
                    // 22333
                    return true;
                }
            //}

            return false;
        }

        /// <summary>
        /// Look for four of a kind
        /// </summary>
        /// <param name="cvs">Card values in numeric order</param>
        /// <remarks>The values must be presorted before calling this function</remarks>
        /// <returns>True if four of a kind</returns>
        private bool HasFourOfKind(CardValues[] cvs)
        {
            Debug.Assert(cvs.Length == 5);

            //if (cvs.Length == 5)
            //{
                // there are only two combinations because of sort
                // 34444 or 44445
                if (cvs[1] == cvs[2] && cvs[1] == cvs[3] && cvs[1] == cvs[4])
                {
                    // 34444 (match last 4)
                    return true;
                }
                else if (cvs[0] == cvs[1] && cvs[0] == cvs[2] && cvs[0] == cvs[3])
                {
                    // 44445 (match first 4)
                    return true;
                }
            //}
            return false;
        }

#endregion

    }  // end Cards class
}
