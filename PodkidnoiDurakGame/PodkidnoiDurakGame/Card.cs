using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame
{
    class Card : Sprite
    {
        public override Vector2 direction { set; get; }

        public event Action<Card> OnClick;
        public event Action<Card> OnHover;


        public Card(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex)
            :base(textureImage, position, frameSize, currentFrame, zIndex){}

        public Card(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, float rotation, Vector2 rotationOrigin)
            : base(textureImage, position, frameSize, currentFrame, zIndex, rotation, rotationOrigin) { }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            MouseState mouseState = Mouse.GetState();
            if (rectangle.Contains(mouseState.Position) &&
                OnHover != null)
            {
                OnHover(this);

                if (mouseState.LeftButton == ButtonState.Pressed &&
                    OnClick != null)
                    OnClick(this);
            }

            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
