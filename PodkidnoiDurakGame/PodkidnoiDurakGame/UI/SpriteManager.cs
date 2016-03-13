using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PodkidnoiDurakGame.Core;

using ApplicationForms;

namespace PodkidnoiDurakGame.GameDesk
{
    public class SpriteManager : DrawableGameComponent
    {

        // There we shall write logic for user interface

        SpriteBatch spriteBatch;
        Texture2D spriteList;
        GameDesktop gameDesktop;

        public SpriteManager(Game game)
            :base(game){}
        public SpriteManager(Game game, GameDesktop gameDesktop)
            : base(game) { this.gameDesktop = gameDesktop; }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            spriteList = this.Game.Content.Load<Texture2D>(@"sprite1");

            //card = new CardSprite(
            //    spriteList, 
            //    new Vector2(20, 50), 
            //    new Point(170, 252), 
            //    new Point((int)CardSuit.Diamond, (int)CardType.Jack), 
            //    0,
            //    0.7f);
            //card.OnClick += (sprite) => {
            //    LoginForm login = new LoginForm();
            //    login.ShowDialog();
            //};

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //card.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //card.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
