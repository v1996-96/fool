using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PodkidnoiDurakGame.Core;

namespace PodkidnoiDurakGame
{
    class CardSprite : Sprite
    {
        public override Vector2 direction { set; get; }

        public event Action<CardSprite> OnClick;
        public event Action<CardSprite> OnHover;


        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex)
            :base(textureImage, position, frameSize, currentFrame, zIndex){}
        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, int frameOffset)
            : base(textureImage, position, frameSize, currentFrame, zIndex, frameOffset) { }
        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, float scale)
            : base(textureImage, position, frameSize, currentFrame, zIndex, scale) { }

        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame,
            float zIndex, int frameOffset, float rotation, Vector2 rotationOrigin, float scale)
            : base(textureImage, position, frameSize, currentFrame, zIndex, frameOffset, rotation, rotationOrigin, scale) { }

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
