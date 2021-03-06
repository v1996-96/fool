﻿using PodkidnoiDurakGame.Core.CardDefinitions;
using PodkidnoiDurakGame.Core.GameDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core
{
    class GameDesktop
    {

        #region Desktop Settings
        // Fields
        public bool IsBlocked { get; set; }
        public GameState GameState { get; private set; }

        // Variables
        private Random _rnd = new Random();

        // Constants
        const int MaxCardTypeIndex = 9;
        const int MaxCardSuitIndex = 4;
        const int DefaultPlayerCardsCount = 6;
        #endregion



        #region Card lists and settings
        public PlayerType WhoseTurn
        {
            get { return _whoseTurn; }
            private set
            {
                _whoseTurn = value;
                if (OnWhoseTurnChanged != null) OnWhoseTurnChanged(value);
            }
        }
        private PlayerType _whoseTurn;
        public PlayerType WhoseParty
        {
            get
            {
                return _whoseParty;
            }
            private set
            {
                _whoseParty = value;
                if (OnWhosePartyChanged != null) OnWhosePartyChanged(value);
            }
        }
        private PlayerType _whoseParty;
        public CardSuit Trump { get; private set; }
        public List<Card> Deck { get; private set; }
        public List<CardPair> DescPairs { get; private set; }
        public List<Card> PlayerCards { get; private set; }
        public List<Card> EnemyCards { get; private set; }
        #endregion



        #region Game events
        // Player events
        public event Action<PlayerType, Card> OnThrowCard;
        public event Action<PlayerType> OnPass;
        public event Action<PlayerType> OnGetAll;
        public event Action<PlayerType> OnWhosePartyChanged;
        public event Action<PlayerType> OnWhoseTurnChanged;

        // Game events
        public event Action OnGameStarted;
        public event Action OnGameStopped;
        public event Action OnDeckCreated;
        public event Action OnCardsHandOut;
        public event Action<GameResult> OnGameFinished;
        public event Action<GameAction, GameError, string> OnActionRefused;
        public event Action<GameError, string> OnGameError;
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

            GameState = GameState.Ready;

            if (OnGameStarted != null) OnGameStarted();
        }
        public void StopGame()
        {
            PlayerCards = new List<Card> { };
            EnemyCards = new List<Card> { };
            DescPairs = new List<CardPair> { };
            Deck = new List<Card> { };

            GameState = GameState.Finished;

            if (OnGameStopped != null) OnGameStopped();
        }
        public GamePackage GetGameData()
        {
            return new GamePackage { 
                WhoseParty = this.WhoseParty,
                Deck = this.Deck,
                DescPairs = this.DescPairs,
                Trump = this.Trump,
                PlayerCards = this.PlayerCards,
                EnemyCards = this.EnemyCards
            };
        }
        public void SetGameData(GamePackage gamePackage)
        {
            if (Trump != gamePackage.Trump ||
                PlayerCards != gamePackage.PlayerCards)
            {
                if (OnGameError != null) OnGameError(GameError.Error, "Trump suit and player's card list do not match");
                return;
            }

            Deck = gamePackage.Deck;
            EnemyCards = gamePackage.EnemyCards;
            DescPairs = gamePackage.DescPairs;
            WhoseParty = gamePackage.WhoseParty;
        }
        private void CheckGameState()
        {
            // Game result refers only to user on current machine

            // If game is stopped or ready we do nothing
            if (GameState == GameState.Ready ||
                GameState == GameState.Finished)
                return;

            // When there is a draw
            if (PlayerCards.Count == 0 &&
                EnemyCards.Count == 0)
            {
                if (OnGameFinished != null) OnGameFinished(GameResult.Draw);
                GameState = GameState.Finished;
            }

            // When player wins or plays a draw
            if (PlayerCards.Count == 0)
            {
                if (EnemyCards.Count > 1)
                {
                    if (OnGameFinished != null) OnGameFinished(GameResult.Win);
                    GameState = GameState.Finished;
                }
                if (EnemyCards.Count == 1 &&
                    WhoseParty == PlayerType.Player)
                {
                    if (!ThrowWhenNotOwnParty(EnemyCards[0]))
                    {
                        if (OnGameFinished != null) OnGameFinished(GameResult.Win);
                        GameState = GameState.Finished;
                    }
                }
            }


            // When enemy wins or plays a draw
            if (EnemyCards.Count == 0)
            {
                if (PlayerCards.Count > 1)
                {
                    if (OnGameFinished != null) OnGameFinished(GameResult.Loose);
                    GameState = GameState.Finished;
                }
                if (PlayerCards.Count == 1 &&
                    WhoseParty == PlayerType.Enemy)
                {
                    if (!ThrowWhenNotOwnParty(PlayerCards[0]))
                    {
                        if (OnGameFinished != null) OnGameFinished(GameResult.Loose);
                        GameState = GameState.Finished;
                    }
                }
            }
        }
        #endregion



        #region Decide whose turn
        private void DecideWhoseTurn()
        {
            var playerMinTrump = GetMinTrump(PlayerCards);
            var enemyMinTrump = GetMinTrump(EnemyCards);

            if (playerMinTrump == null && enemyMinTrump == null)
            {
                var playerMaxNonTrump = GetMaxNonTrump(PlayerCards);
                var enemyMaxNonTrump = GetMaxNonTrump(EnemyCards);
                if (playerMaxNonTrump == null || enemyMaxNonTrump == null)
                {
                    if (OnGameError != null) OnGameError(GameError.Error, "Can't decide which turn there is");
                    StopGame();
                    return;
                }

                if ((int)playerMaxNonTrump.Value.CardType < (int)enemyMaxNonTrump.Value.CardType)
                    WhoseParty = PlayerType.Enemy;

                if ((int)playerMaxNonTrump.Value.CardType > (int)enemyMaxNonTrump.Value.CardType)
                    WhoseParty = PlayerType.Player;

                if ((int)playerMaxNonTrump.Value.CardType == (int)enemyMaxNonTrump.Value.CardType)
                    WhoseParty = PlayerType.Player;
            }

            if (playerMinTrump != null && enemyMinTrump == null)
                WhoseParty = PlayerType.Player;

            if (playerMinTrump == null && enemyMinTrump != null)
                WhoseParty = PlayerType.Enemy;

            if (playerMinTrump != null && enemyMinTrump != null)
            {
                if ((int)playerMinTrump.Value.CardType < (int)enemyMinTrump.Value.CardType)
                    WhoseParty = PlayerType.Player;

                if ((int)playerMinTrump.Value.CardType > (int)enemyMinTrump.Value.CardType)
                    WhoseParty = PlayerType.Enemy;

                if ((int)playerMinTrump.Value.CardType == (int)enemyMinTrump.Value.CardType)
                    WhoseParty = PlayerType.Player;
            }

            WhoseTurn = WhoseParty;
        }
        private Card? GetMinTrump(List<Card> cardList)
        {
            if (cardList.Count == 0)
                return null;

            Card min = new Card();
            bool found = false;

            foreach (var card in cardList)
            {
                if (card.CardSuit == Trump)
                {
                    min = card;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                foreach (var card in cardList)
                {
                    if ((int)card.CardType < (int)min.CardType)
                    {
                        min = card;
                    }
                }
            }
            else return null;

            return min;
        }
        private Card? GetMaxNonTrump(List<Card> cardList)
        {
            if (cardList.Count == 0)
                return null;

            Card max = cardList[0];
            for (int i = 0; i < cardList.Count; i++)
            {
                if ((int)cardList[i].CardType > (int)max.CardType)
                {
                    max = cardList[i];
                }
            }

            return max;
        }
        #endregion



        #region Work with deck
        private void CreateDeck()
        {
            // Create Deck
            for (int i = 0; i < MaxCardTypeIndex; i++)
            {
                for (int j = 0; j < MaxCardSuitIndex; j++)
                {
                    Card card = new Card();
                    if (Enum.IsDefined(typeof(CardType), i))
                    {
                        card.CardType = (CardType)i;
                    }
                    else
                    {
                        if (OnGameError != null) OnGameError(GameError.FatalError, "Card index not found"); return;
                    }

                    if (Enum.IsDefined(typeof(CardSuit), j))
                    {
                        card.CardSuit = (CardSuit)j;
                    }
                    else
                    {
                        if (OnGameError != null) OnGameError(GameError.FatalError, "Card index not found"); return;
                    }

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

            // The first card suit in the deck is a trump suit
            Trump = Deck[0].CardSuit;

            if (OnDeckCreated != null) OnDeckCreated();
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

            if (OnCardsHandOut != null) OnCardsHandOut();
        }
        private void HandOutCardsToPlayer()
        {
            do
            {
                if (Deck.Count == 0) return;
                if (PlayerCards.Count >= DefaultPlayerCardsCount) break;

                PlayerCards.Add(Deck[Deck.Count - 1]);
                Deck.RemoveAt(Deck.Count - 1);
            } while (Deck.Count > 0);
        }
        private void HandOutCardsToEnemy()
        {
            do
            {
                if (Deck.Count == 0) return;
                if (EnemyCards.Count >= DefaultPlayerCardsCount) break;

                EnemyCards.Add(Deck[Deck.Count - 1]);
                Deck.RemoveAt(Deck.Count - 1);
            } while (Deck.Count > 0);
        }
        #endregion



        #region Cards sorting
        private List<Card> SortCards(List<Card> cardList)
        {
            for (int i = 0; i < cardList.Count-1; i++)
            {
                for (int j = i+1; j < cardList.Count; j++)
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
            if (card.CardSuit == Trump) return 100000;

            switch (card.CardSuit)
            {
                case CardSuit.Heart: return 10;
                case CardSuit.Diamond: return 100;
                case CardSuit.Club: return 1000;
                case CardSuit.Spade: return 10000;
                default: return 1;
            }
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

                if (IsBlocked) return;
                switch (GameState)
                {
                    case GameState.Ready: GameState = GameState.Playing; break;
                    case GameState.Finished: return;
                    case GameState.Playing: break;
                    default: return;
                }

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
                        WhoseTurn = PlayerType.Enemy;
                        CheckGameState();
                        return;
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
                        WhoseTurn = PlayerType.Player;
                        CheckGameState();
                        return;
                    }
                }

                if (player == PlayerType.Player && WhoseParty == PlayerType.Enemy)
                {
                    if (!PlayerCards.Contains(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You has no such card");
                        return;
                    }

                    if (!ThrowWhenNotOwnParty(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.ActionRefused, "You can't throw that card");
                        return;
                    }
                    else
                    {
                        PlayerCards.Remove(card);
                        DescPairs[DescPairs.Count - 1].UpperCard = card;

                        if (OnThrowCard != null) OnThrowCard(player, card);
                        WhoseTurn = PlayerType.Enemy;
                        CheckGameState();
                        return;
                    }
                }
                if (player == PlayerType.Enemy && WhoseParty == PlayerType.Player)
                {
                    if (!EnemyCards.Contains(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.Warning, "You has no such card");
                        return;
                    }

                    if (!ThrowWhenNotOwnParty(card))
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Throw, GameError.ActionRefused, "You can't throw that card");
                        return;
                    }
                    else
                    {
                        EnemyCards.Remove(card);
                        DescPairs[DescPairs.Count - 1].UpperCard = card;

                        if (OnThrowCard != null) OnThrowCard(player, card);
                        WhoseTurn = PlayerType.Player;
                        CheckGameState();
                        return;
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
                    return (cardThrown.CardSuit == DescPairs[lastIndex].LowerCard.CardSuit) &&
                       ((int)DescPairs[lastIndex].LowerCard.CardType < (int)cardThrown.CardType);

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

                if (IsBlocked) return;
                switch (GameState)
                {
                    case GameState.Ready: GameState = GameState.Playing; break;
                    case GameState.Finished: return;
                    case GameState.Playing: break;
                    default: return;
                }

                if (DescPairs.Count == 0)
                {
                    if (OnActionRefused != null) OnActionRefused(GameAction.Pass, GameError.Warning, "You cant't say pass");
                    return;
                }

                if (WhoseParty == PlayerType.Player)
                {
                    if (player != PlayerType.Player)
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Pass, GameError.Warning, "You cant't say pass");
                        return;
                    }

                    DescPairs.Clear();
                    WhoseParty = PlayerType.Enemy;

                    if (OnPass != null) OnPass(player);
                    HandOutCards();
                    WhoseTurn = PlayerType.Enemy;
                    CheckGameState();
                    return;
                }

                if (WhoseParty == PlayerType.Enemy)
                {
                    if (player != PlayerType.Enemy)
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.Pass, GameError.Warning, "You cant't say pass");
                        return;
                    }

                    DescPairs.Clear();
                    WhoseParty = PlayerType.Player;

                    if (OnPass != null) OnPass(player);
                    HandOutCards();
                    WhoseTurn = PlayerType.Player;
                    CheckGameState();
                    return;
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

                if (IsBlocked) return;
                switch (GameState)
                {
                    case GameState.Ready: GameState = GameState.Playing; break;
                    case GameState.Finished: return;
                    case GameState.Playing: break;
                    default: return;
                }

                if (DescPairs.Count == 0)
                {
                    if (OnActionRefused != null) OnActionRefused(GameAction.Pass, GameError.Warning, "You cant't say get all");
                    return;
                }

                if (WhoseParty == PlayerType.Player)
                {
                    if (player != PlayerType.Enemy)
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.GetAll, GameError.Warning, "You cant't say get all");
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
                    HandOutCards();
                    WhoseTurn = PlayerType.Player;
                    CheckGameState();
                    return;
                }

                if (WhoseParty == PlayerType.Enemy)
                {
                    if (player != PlayerType.Player)
                    {
                        if (OnActionRefused != null) OnActionRefused(GameAction.GetAll, GameError.Warning, "You cant't say get all");
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
                    HandOutCards();
                    WhoseTurn = PlayerType.Enemy;
                    CheckGameState();
                    return;
                }
            }
            #endregion

        #endregion
    }
}
