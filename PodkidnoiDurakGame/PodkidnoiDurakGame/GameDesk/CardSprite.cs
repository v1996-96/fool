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
            float zIndex, int frameOffset, float scale, float rotation, Vector2 rotationOrigin)
            : base(textureImage, position, frameSize, currentFrame, zIndex, frameOffset, scale, rotation, rotationOrigin) { }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (controlArea.Contains(mouseState.Position) &&
                OnHover != null)
                OnHover(this);

            if (controlArea.Contains(mouseState.Position) && 
                mouseState.LeftButton == ButtonState.Pressed &&
                OnClick != null)
                OnClick(this);


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
