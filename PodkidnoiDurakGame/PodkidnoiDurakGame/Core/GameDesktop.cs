﻿using PodkidnoiDurakGame.Core.CardDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core
{
    class GameDesktop
    {
        #region Singleton pattern
        private static GameDesktop _instance;
        public static GameDesktop Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameDesktop();

                return _instance;
            }
        }
        #endregion



        #region Desktop Settings
        // Fields
        public bool IsBlocked { get; private set; }
        public PlayerType WhoseParty { get; private set; }

        // Variables
        private Random _rnd = new Random();

        // Constants
        const int MaxCardTypeIndex = 9;
        const int MaxCardSuitIndex = 4;
        const int DefaultPlayerCardsCount = 6;
        #endregion



        #region Card lists and settings
        // Card lists
        public CardSuit Trump { get; private set; }
        public List<Card> Deck { get; private set; }
        public List<CardPair> DescPairs { get; private set; }
        public List<Card> PlayerCards { get; private set; }
        public List<Card> EnemyCards { get; private set; }
        #endregion



        #region Game events
        // Player events (handled by enemy)
        public event Action<PlayerType, Card> OnThrowCard;
        public event Action<PlayerType> OnWaitingTurn;
        public event Action<PlayerType> OnPass;
        public event Action<PlayerType> OnGetAll;

        // Game events
        public event Action OnGameStarted;
        public event Action OnGameStopped;
        public event Action OnGameBlocked;
        public event Action OnGameUnBlocked;
        public event Action<GameAction, GameError, string> OnActionRefused;
        #endregion



        #region Game basic actions
        public void StartGame()
        {
            PlayerCards = new List<Card> { };
            EnemyCards = new List<Card> { };
            DescPairs = new List<CardPair> { };
            Deck = new List<Card> { };

            CreateDeck();
            HandOutCards();
            DecideWhoseTurn();

            if (OnGameStarted != null) OnGameStarted();
        }
        public void StopGame()
        {
            PlayerCards = new List<Card> { };
            EnemyCards = new List<Card> { };
            DescPairs = new List<CardPair> { };
            Deck = new List<Card> { };

            if (OnGameStopped != null) OnGameStopped();
        }
        public void BlockGame()
        {
            if (IsBlocked)
            {
                this.IsBlocked = false;
                if (OnGameUnBlocked != null) OnGameUnBlocked();
            }
            else
            {
                this.IsBlocked = true;
                if (OnGameBlocked != null) OnGameBlocked();
            }
        }
        #endregion



        #region Decide whose turn
        private void DecideWhoseTurn()
        {
            WhoseParty = PlayerType.Player;

            // TODO: Write algorithm, which will make that decision
        }
        #endregion



        #region Work with deck
        private void CreateDeck()
        {
            // Get random trump suit
            Trump = (CardSuit)_rnd.Next(0, MaxCardSuitIndex);

            // Create Deck
            for (int i = 0; i < MaxCardTypeIndex; i++)
            {
                for (int j = 0; j < MaxCardSuitIndex; j++)
                {
                    Card card = new Card();
                    if (Enum.IsDefined(typeof(CardType), i))
                        card.CardType = (CardType)i;

                    if (Enum.IsDefined(typeof(CardSuit), j))
                        card.CardSuit = (CardSuit)j;

                    Deck.Add(card);
                }
            }

            // Shuffle cards
            for (int i = 1; i < Deck.Count; i++)
            {
                int pos = _rnd.Next(i + 1);
                var x = Deck[i];
                Deck[i] = Deck[pos];
                Deck[pos] = x;
            }
        }
        #endregion



        #region Hand out cards
        private void HandOutCards()
        {
            if (Deck.Count == 0)
                return;

            if (WhoseParty == PlayerType.Player)
            {
                HandOutCardsToPlayer();
                HandOutCardsToEnemy();
            }

            if (WhoseParty == PlayerType.Enemy)
            {
                HandOutCardsToEnemy();
                HandOutCardsToPlayer();
            }

            PlayerCards = SortCards(PlayerCards);
            EnemyCards = SortCards(EnemyCards);
        }
        private void HandOutCardsToPlayer()
        {
            while (Deck.Count > 0 && PlayerCards.Count < DefaultPlayerCardsCount)
            {
                PlayerCards.Add( Deck[ Deck.Count-1 ] );
                Deck.RemoveAt( Deck.Count-1 );
            }
        }
        private void HandOutCardsToEnemy()
        {
            while (Deck.Count > 0 && EnemyCards.Count < DefaultPlayerCardsCount)
            {
                EnemyCards.Add(Deck[Deck.Count - 1]);
                Deck.RemoveAt(Deck.Count - 1);
            }
        }
        #endregion



        #region Cards sorting
        private List<Card> SortCards(List<Card> cardList)
        {
            for (int i = 0; i < cardList.Count-1; i++)
            {
                for (int j = i+1; j < cardList.Count; j++)
                {
                    var firstPriority = ((cardList[i].CardSuit == Trump) ? 10 : (int)cardList[i].CardSuit + 1) * ((int)cardList[i].CardType +1);
                    var secondPriority = ((cardList[j].CardSuit == Trump) ? 10 : (int)cardList[j].CardSuit + 1) * ((int)cardList[j].CardType +1);
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
        #endregion



        #region Player actions

            #region Throw card
            public void ThrowCard(PlayerType player, Card card)
            {
                // There are 4 possible variations and 2 types of rules
                // 
                // First (1'st type): when that is player's party and player's throw.
                // In this case we shall see for equivalent cards on desc and allow only these, 
                // which are equal by type with cards already thrown.
                //
                // Second (1'st type): when that is enemy's party and enemy's throw.
                // In this case we shall see for equivalent cards on desc and allow only these, 
                // which are equal by type with cards already thrown.
                //
                // Third (2'nd type): when that is player's party and enemy's throw.
                // In this case we shall find out, could the thrown card beat the card, which was 
                // thrown by player. Otherwise we refuse throwing.
                // 
                // Fourth (2'nd type): when that is enemy's party and player's throw.
                // In this case we shall find out, could the thrown card beat the card, which was 
                // thrown by player. Otherwise we refuse throwing.


                if (player == PlayerType.Player && WhoseParty == PlayerType.Player)
                {
                    if (!PlayerCards.Contains(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You has no such card");
                        return;
                    }

                    if (!ThrowWhenOwnParty(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.ActionRefused, "You can't throw that card");
                        return;
                    }
                    else
                    {
                        PlayerCards.Remove(card);
                        DescPairs.Add(new CardPair { LowerCard = card });
                        if (OnThrowCard != null) OnThrowCard(player, card);
                    }
                }
                if (player == PlayerType.Enemy && WhoseParty == PlayerType.Enemy)
                {
                    if (!EnemyCards.Contains(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You has no such card");
                        return;
                    }

                    if (!ThrowWhenOwnParty(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.ActionRefused, "You can't throw that card");
                        return;
                    }
                    else
                    {
                        EnemyCards.Remove(card);
                        DescPairs.Add(new CardPair { LowerCard = card });
                        if (OnThrowCard != null) OnThrowCard(player, card);
                    }
                }

                if (player == PlayerType.Player && WhoseParty == PlayerType.Enemy)
                {
                    if (!PlayerCards.Contains(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You has no such card");
                        return;
                    }

                    if (!ThrowWhenOwnParty(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.ActionRefused, "You can't throw that card");
                        return;
                    }
                    else
                    {
                        PlayerCards.Remove(card);
                        DescPairs[DescPairs.Count - 1].UpperCard = card;
                        if (OnThrowCard != null) OnThrowCard(player, card);
                    }
                }
                if (player == PlayerType.Enemy && WhoseParty == PlayerType.Player)
                {
                    if (!EnemyCards.Contains(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You has no such card");
                        return;
                    }

                    if (!ThrowWhenOwnParty(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.ActionRefused, "You can't throw that card");
                        return;
                    }
                    else
                    {
                        EnemyCards.Remove(card);
                        DescPairs[DescPairs.Count - 1].UpperCard = card;
                        if (OnThrowCard != null) OnThrowCard(player, card);
                    }
                }
            }

            private bool ThrowWhenOwnParty(Card cardThrown)
            {
                // If there are no cards already thrown we can throw
                if (DescPairs.Count == 0)
                    return true;

                // Get list of cards thrown
                List<Card> cards = new List<Card> { };
                foreach (var card in DescPairs)
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
                if (DescPairs.Count == 0)
                    return false;

                var lastIndex = DescPairs.Count - 1;

                if (DescPairs[lastIndex].LowerCard.CardSuit == Trump &&
                    cardThrown.CardSuit != Trump)
                    return false;

                if (DescPairs[lastIndex].LowerCard.CardSuit != Trump &&
                    cardThrown.CardSuit == Trump)
                    return true;

                if ((DescPairs[lastIndex].LowerCard.CardSuit == Trump &&
                    cardThrown.CardSuit == Trump) ||
                    (DescPairs[lastIndex].LowerCard.CardSuit != Trump &&
                    cardThrown.CardSuit != Trump))
                    return (int)DescPairs[lastIndex].LowerCard.CardSuit < (int)cardThrown.CardSuit;

                return false;
            }
            #endregion


            #region Say pass
            public void Pass(PlayerType player)
            {
                // There can be two types of situation
                // First: when that is player's party and player's turn
                // In this case we just clear the DescPairs and hand out remaining cards from deck
                //
                // Second: when that is enemy's party and enemy's turn
                // In this case we make the same thing
                //
                // In other cases we just refuse action

                if (WhoseParty == PlayerType.Player)
                {
                    if (player != PlayerType.Player)
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You cant't say pass");
                        return;
                    }

                    DescPairs.Clear();
                    HandOutCards();

                    if (OnPass != null) OnPass(player);
                }

                if (WhoseParty == PlayerType.Enemy)
                {
                    if (player != PlayerType.Enemy)
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You cant't say pass");
                        return;
                    }

                    DescPairs.Clear();
                    HandOutCards();

                    if (OnPass != null) OnPass(player);
                }
            }
            #endregion


            #region Say get all
            public void GetAll(PlayerType player)
            {
                // There can be two types of situation
                // First: when that is player's party and enemy's turn
                // In this case we extend player's list of cards with list of cards from desc. Thern we clear desc.
                //
                // Second: when that is enemy's party and player's turn
                // In this case we make the same thing
                //
                // In other cases we just refuse action

                if (WhoseParty == PlayerType.Player)
                {
                    if (player != PlayerType.Enemy)
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You cant't say get all");
                        return;
                    }

                    foreach (var cardPair in DescPairs)
                    {
                        EnemyCards.Add(cardPair.LowerCard);
                        if (cardPair.UpperCard != null)
                            EnemyCards.Add((Card)cardPair.UpperCard);
                    }

                    DescPairs.Clear();

                    if (OnGetAll != null) OnGetAll(player);
                }

                if (WhoseParty == PlayerType.Enemy)
                {
                    if (player != PlayerType.Player)
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You cant't say get all");
                        return;
                    }

                    foreach (var cardPair in DescPairs)
                    {
                        PlayerCards.Add(cardPair.LowerCard);
                        if (cardPair.UpperCard != null)
                            PlayerCards.Add((Card)cardPair.UpperCard);
                    }

                    DescPairs.Clear();

                    if (OnGetAll != null) OnGetAll(player);
                }
            }
            #endregion

        #endregion
    }
}
