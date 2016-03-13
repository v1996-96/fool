using PodkidnoiDurakGame.Core.CardDefinitions;
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
                
                return _instance
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
        public event Action<Card> OnPlayerThrow;
        public event Action OnWaitingPlayersTurn;
        public event Action OnPlayerPass;
        public event Action OnPlayerGetAll;

        // Enemy events (handled by player)
        public event Action<Card> OnEnemyThrow;
        public event Action OnWaitingEnemyTurn;
        public event Action OnEnemyPass;
        public event Action OnEnemyGetAll;

        // Game events
        public event Action OnGameStarted;
        public event Action OnGameStopped;
        public event Action OnGameBlocked;
        public event Action OnGameUnBlocked;
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
        }


        public void StopGame()
        {

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

                    if (card.CardSuit == Trump)
                        card.IsTrump = true;
                    else
                        card.IsTrump = false;

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

            if (WhoseParty == null ||
                WhoseParty == PlayerType.Player)
            {
                HandOutCardsToPlayer();
                HandOutCardsToEnemy();
            }

            if (WhoseParty == PlayerType.Enemy)
            {
                HandOutCardsToEnemy();
                HandOutCardsToPlayer();
            }
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

    }
}
