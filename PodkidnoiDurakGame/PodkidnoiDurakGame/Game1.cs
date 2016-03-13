using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PodkidnoiDurakGame.Core;
using PodkidnoiDurakGame.GameDesk;

namespace PodkidnoiDurakGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteManager spriteManager; // User interface was transmitted there
        GameDesktop gameDesktop = GameDesktop.Instance;

        const string TITLE = "Подкидной дурак v3.0";

        // There we shall write logic to connect WPF, players, desktops, AI and network

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Window.AllowAltF4 = true;
            Window.Title = TITLE;
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }



        #region Monogame gameloop
        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            base.Initialize();
        }
        protected override void LoadContent() {}
        protected override void UnloadContent() {}
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);
        }
        #endregion
    }
}
