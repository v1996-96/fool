using PodkidnoiDurakGame.Core.GameDefinitions;
using PodkidnoiDurakGame.Core.PlayerDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core.ArtificialIntelligence
{
    class AINormal : IPlayer
    {
        public GamePackage GamePackage { get; set; }

        public event Action<Card> OnThrow;

        public event Action OnPass;

        public event Action OnGetAll;

        public void TakeTheBaton()
        {
            // AI figts back
            if (GamePackage.WhoseParty == PlayerType.Player)
            {
                // Made to be on the safe side
                if (GamePackage.EnemyCards.Count == 0)
                {
                    if (OnGetAll != null) OnGetAll();
                    return;
                }

                List<Card> cardsCanBeThrown = new List<Card> { };
                GamePackage.EnemyCards.ForEach((card) => { 
                    if (ThrowWhenNotOwnParty(card)) cardsCanBeThrown.Add(card); 
                });

                if (cardsCanBeThrown.Count == 0)
                {
                    if (OnGetAll != null) OnGetAll();
                    return;
                }

                cardsCanBeThrown = SortCards(cardsCanBeThrown);

                if (OnThrow != null) OnThrow(cardsCanBeThrown[0]);
            }

            // AI attacks
            if (GamePackage.WhoseParty == PlayerType.Enemy)
            {
                if (GamePackage.EnemyCards.Count == 0)
                {
                    if (OnPass != null) OnPass();
                    return;
                }

                List<Card> cardsCanBeThrown = new List<Card> { };
                GamePackage.EnemyCards.ForEach((card) => { 
                    if (ThrowWhenOwnParty(card)) cardsCanBeThrown.Add(card); 
                });

                if (cardsCanBeThrown.Count == 0)
                {
                    if (OnPass != null) OnPass();
                    return;
                }

                cardsCanBeThrown = SortCards(cardsCanBeThrown);

                if (OnThrow != null) OnThrow(cardsCanBeThrown[0]);
            }
        }

        #region Useful methods
        private List<Card> SortCards(List<Card> cardList)
        {
            for (int i = 0; i < cardList.Count - 1; i++)
            {
                for (int j = i + 1; j < cardList.Count; j++)
                {
                    var firstPriority = GetCardPriority(cardList[i]) * ((int)cardList[i].CardType + 1);
                    var secondPriority = GetCardPriority(cardList[j]) * ((int)cardList[j].CardType + 1);
                    if (firstPriority > secondPriority)
                    {
                        var card = cardList[i];
                        cardList[i] = cardList[j];
                        cardList[j] = card;
                    }
                }
            }

            return cardList;
        }
        private int GetCardPriority(Card card)
        {
            if (card.CardSuit == GamePackage.Trump) return 100000;

            switch (card.CardSuit)
            {
                case CardSuit.Heart: return 10;
                case CardSuit.Diamond: return 100;
                case CardSuit.Club: return 1000;
                case CardSuit.Spade: return 10000;
                default: return 1;
            }
        }

        private bool ThrowWhenOwnParty(Card cardThrown)
        {
            // If there are no cards already thrown we can throw
            if (GamePackage.DescPairs.Count == 0)
                return true;

            // Get list of cards thrown
            List<Card> cards = new List<Card> { };
            foreach (var card in GamePackage.DescPairs)
            {
                cards.Add((Card)card.LowerCard);

                if (card.UpperCard != null)
                    cards.Add((Card)card.UpperCard);
            }

            // Can card be thrown or not...
            bool canThrow = false;
            foreach (var card in cards)
            {
                if (cardThrown.CardType == card.CardType)
                {
                    canThrow = true; break;
                }
            }

            return canThrow;
        }

        private bool ThrowWhenNotOwnParty(Card cardThrown)
        {
            // If there are no cards already thrown we can not throw
            if (GamePackage.DescPairs.Count == 0)
                return false;

            var lastIndex = GamePackage.DescPairs.Count - 1;

            if (GamePackage.DescPairs[lastIndex].LowerCard.CardSuit == GamePackage.Trump &&
                cardThrown.CardSuit != GamePackage.Trump)
                return false;

            if (GamePackage.DescPairs[lastIndex].LowerCard.CardSuit != GamePackage.Trump &&
                cardThrown.CardSuit == GamePackage.Trump)
                return true;

            if ((GamePackage.DescPairs[lastIndex].LowerCard.CardSuit == GamePackage.Trump &&
                cardThrown.CardSuit == GamePackage.Trump) ||
                (GamePackage.DescPairs[lastIndex].LowerCard.CardSuit != GamePackage.Trump &&
                cardThrown.CardSuit != GamePackage.Trump))
                return (cardThrown.CardSuit == GamePackage.DescPairs[lastIndex].LowerCard.CardSuit) &&
                       ((int)GamePackage.DescPairs[lastIndex].LowerCard.CardType < (int)cardThrown.CardType);

            return false;
        }
        #endregion




        public void Throw(Card card)
        {
            // There we do nothing
        }

        public void Pass()
        {
            // There we do nothing
        }

        public void GetAll()
        {
            // There we do nothing
        }
    }
}
