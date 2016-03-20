using ApplicationForms;
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

        bool _exitCommand = false;

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
        

        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            base.Initialize();

            gameGateway.OnGameFinished += GameFinishedHandler;

            LoginForm loginForm = new LoginForm();
            loginForm.OnStartWithAI += (nickname) =>
            {
                gameGateway.SpriteManager = spriteManager;
                gameGateway.StartGame();
            };
            loginForm.OnStartWithNetUser += (nickname) => { MessageBox.Show("Comming soon"); };
            loginForm.OnAboutShow += () =>
            {
                About about = new About();
                about.ShowDialog();
            };
            loginForm.OnExit += () => { _exitCommand = true; };

            loginForm.ShowDialog();
        }
        private void GameFinishedHandler(string status)
        {
            ResultWindow result = new ResultWindow(status);
            result.OnStartWithAI += () =>
            {
                gameGateway.SpriteManager = spriteManager;
                gameGateway.StartGame();
            };
            result.OnStartWithNetUser += () => { MessageBox.Show("Comming soon"); };
            result.OnAboutShow += () =>
            {
                About about = new About();
                about.ShowDialog();
            };
            result.OnExit += () => { _exitCommand = true; };
            result.ShowDialog();
        }



        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || _exitCommand)
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
