using PodkidnoiDurakGame.Core.ArtificialIntelligence;
using PodkidnoiDurakGame.Core.CardDefinitions;
using PodkidnoiDurakGame.Core.GameDefinitions;
using PodkidnoiDurakGame.Core.PlayerDefinitions;
using PodkidnoiDurakGame.GameDesk;
using PodkidnoiDurakGame.UI;
using PodkidnoiDurakGame.UI.ElementDefenitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PodkidnoiDurakGame.Core
{
    class GameGateway
    {
        #region Singleton pattern
        private static GameGateway _instance;
        public static GameGateway Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameGateway();

                return _instance;
            }
        }
        private GameGateway() { }
        #endregion

        private GameDesktop gameDesktop = GameDesktop.Instance;
        public GameDesktop GameDesktop { get { return gameDesktop; } }

        private IPlayer _player;
        private IPlayer _enemy;
        public SpriteManager SpriteManager { get; set; }


        public void StartGame()
        {
            _player = new Player();
            _enemy = new AINormal();

            _player.OnThrow += (card) => gameDesktop.ThrowCard(PlayerType.Player, card);
            _player.OnPass += () => gameDesktop.Pass(PlayerType.Player);
            _player.OnGetAll += () => gameDesktop.GetAll(PlayerType.Player);
            _enemy.OnThrow += (card) => gameDesktop.ThrowCard(PlayerType.Enemy, card);
            _enemy.OnPass += () => gameDesktop.Pass(PlayerType.Enemy);
            _enemy.OnGetAll += () => gameDesktop.GetAll(PlayerType.Enemy);

            GameDesktop.OnDeckCreated += () => SpriteManager.ResetCardsOnWindow(GameDesktop.GetGameData().Deck); ;

            GameDesktop.OnCardsHandOut += () => { 
                RenewWindowHandler(); 
            };
            GameDesktop.OnThrowCard += (a, b) => { 
                RenewWindowHandler(); 
            };
            GameDesktop.OnPass += (a) => { 
                RenewWindowHandler(); 
            };
            GameDesktop.OnGetAll += (a) => { 
                RenewWindowHandler(); 
            };
            GameDesktop.OnWhoseTurnChanged += (playerType) =>
            {
                _player.GamePackage = gameDesktop.GetGameData();
                _enemy.GamePackage = gameDesktop.GetGameData();

                if (playerType == PlayerType.Player) _player.TakeTheBaton();

                if (playerType == PlayerType.Enemy) _enemy.TakeTheBaton();
            };


            GameDesktop.OnActionRefused += (gameAction, gameError, message) =>
            {
                //MessageBox.Show(message);
            };
            GameDesktop.OnGameError += (gameError, message) =>
            {
                //MessageBox.Show(message);
            };


            // Connect UI and player instance
            SpriteManager.OnPlayerCardThrow += _player.Throw;
            SpriteManager.OnGameButtonClicked += (btnType) =>
            {
                switch (btnType)
                {
                    case ButtonType.Pass:
                    case ButtonType.PassHovered:
                        _player.Pass();
                        break;
                    case ButtonType.GetAll:
                    case ButtonType.GetAllHovered:
                        _player.GetAll();
                        break;
                    default:
                        break;
                }
            };


            // There we start game in game desktop
            GameDesktop.StartGame();
        }




        private void RenewWindowHandler()
        {
            _player.GamePackage = gameDesktop.GetGameData();
            _enemy.GamePackage = gameDesktop.GetGameData();
            SpriteManager.RenewWindowPackage(GameDesktop.GetGameData());
        }
        private void RenewWindowHandler(PlayerType playerType, Card card)
        {
            _player.GamePackage = gameDesktop.GetGameData();
            _enemy.GamePackage = gameDesktop.GetGameData();
            SpriteManager.RenewWindowPackage(GameDesktop.GetGameData());
        }
        private void RenewWindowHandler(PlayerType playerType)
        {
            _player.GamePackage = gameDesktop.GetGameData();
            _enemy.GamePackage = gameDesktop.GetGameData();
            SpriteManager.RenewWindowPackage(GameDesktop.GetGameData());
        }
    }
}
