using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PodkidnoiDurakGame.Core;
using PodkidnoiDurakGame.Core.ArtificialIntelligence;
using PodkidnoiDurakGame.Core.PlayerDefinitions;
using PodkidnoiDurakGame.GameDesk;
using PodkidnoiDurakGame.UI.ElementDefenitions;
using System.Windows;

namespace PodkidnoiDurakGame
{
    public class Game1 : Game
    {
        const string TITLE = "Подкидной дурак v3.0";

        GraphicsDeviceManager graphics;
        SpriteManager spriteManager;

        GameGateway gameGateway = GameGateway.Instance;
        private IPlayer _player;
        private IPlayer _enemy;


        // There we shall write logic to connect WPF, players, desktops, AI and network


        public Game1()
        {
            // Init game window
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 599;

            Window.AllowAltF4 = true;
            Window.Title = TITLE;
            Window.AllowUserResizing = false;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }



        private void StartGame()
        {
            _player = new Player();
            _enemy = new AINormal();

            // There we connect users and game desktop
            gameGateway.InitializeConnections(ref _player, ref _enemy);

            // There we connect game desktop and UI
            gameGateway.GameDesktop.OnDeckCreated += () =>
            {
                spriteManager.ResetCardsOnWindow(gameGateway.GameDesktop.GetGameData().Deck);
            };
            gameGateway.GameDesktop.OnCardsHandOut += () =>
            {
                spriteManager.RenewWindowPackage(gameGateway.GameDesktop.GetGameData());
                gameGateway.GetGamePackages(ref _player);
                gameGateway.GetGamePackages(ref _enemy);
            };


            gameGateway.GameDesktop.OnThrowCard += (playerType, card) =>
            {
                spriteManager.RenewWindowPackage(gameGateway.GameDesktop.GetGameData());
                gameGateway.GetGamePackages(ref _player);
                gameGateway.GetGamePackages(ref _enemy);
            };
            gameGateway.GameDesktop.OnPass += (playerType) =>
            {
                spriteManager.RenewWindowPackage(gameGateway.GameDesktop.GetGameData());
                gameGateway.GetGamePackages(ref _player);
                gameGateway.GetGamePackages(ref _enemy);
            };
            gameGateway.GameDesktop.OnGetAll += (playerType) =>
            {
                spriteManager.RenewWindowPackage(gameGateway.GameDesktop.GetGameData());
                gameGateway.GetGamePackages(ref _player);
                gameGateway.GetGamePackages(ref _enemy);
            };
            gameGateway.GameDesktop.OnWhoseTurnChanged += (playerType) =>
            {
                gameGateway.GetGamePackages(ref _player);
                gameGateway.GetGamePackages(ref _enemy);

                if (playerType == PlayerType.Player)
                    _player.TakeTheBaton();

                if (playerType == PlayerType.Enemy)
                    _enemy.TakeTheBaton();
            };


            gameGateway.GameDesktop.OnActionRefused += (gameAction, gameError, message) =>
            {
                //MessageBox.Show(message);
            };
            gameGateway.GameDesktop.OnGameError += (gameError, message) =>
            {
                MessageBox.Show(message);
            };


            // Connect UI and player instance
            spriteManager.OnPlayerCardThrow += _player.Throw;
            spriteManager.OnGameButtonClicked += (btnType) =>
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
            gameGateway.GameDesktop.StartGame();
        }



        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            base.Initialize();

            StartGame();
        }



        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            base.Draw(gameTime);
        }



        protected override void LoadContent() { }
        protected override void UnloadContent() { }
    }
}
