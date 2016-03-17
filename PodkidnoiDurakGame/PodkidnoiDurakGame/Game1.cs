using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PodkidnoiDurakGame.Core;
using PodkidnoiDurakGame.Core.ArtificialIntelligence;
using PodkidnoiDurakGame.Core.PlayerDefinitions;
using PodkidnoiDurakGame.GameDesk;

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
            Window.AllowAltF4 = true;
            Window.Title = TITLE;
            Window.AllowUserResizing = true;
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
                spriteManager.RenewWindowPackage(gameGateway.GameDesktop.GetGameData());
            };
            gameGateway.GameDesktop.OnCardsHandOut += () =>
            {
                spriteManager.RenewWindowPackage(gameGateway.GameDesktop.GetGameData());
            };
            spriteManager.OnUserBlocked += () =>
            {
                gameGateway.GameDesktop.TriggerBlockGame();
            };

            // There we start game in game desktop
            gameGateway.GameDesktop.StartGame();

            // There we transmit game packages to players
            gameGateway.GetGamePackages(ref _player);
            gameGateway.GetGamePackages(ref _enemy);
        }



        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            
            // For test purposes
            StartGame();

            base.Initialize();
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
