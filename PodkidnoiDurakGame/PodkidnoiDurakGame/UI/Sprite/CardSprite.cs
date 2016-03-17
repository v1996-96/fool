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
        public Card Card { get; set; }

        private Vector2? destination;
        private Vector2 posIncrement;

        public event Action<Card> OnAnimationStart;
        public event Action<Card> OnAnimationEnd;
        public event Action<Card> OnClick;
        public event Action<Card> OnHover;


        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex)
            :base(textureImage, position, frameSize, currentFrame, zIndex){}
        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, int frameOffset)
            : base(textureImage, position, frameSize, currentFrame, zIndex, frameOffset) { }
        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, float scale)
            : base(textureImage, position, frameSize, currentFrame, zIndex, scale) { }

        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame,
            float zIndex, int frameOffset, float scale, float rotation, Vector2 rotationOrigin)
            : base(textureImage, position, frameSize, currentFrame, zIndex, frameOffset, scale, rotation, rotationOrigin) { }

        public void Animate(Vector2 destination, float speed)
        {
            this.destination = destination;
            this.posIncrement = (destination - position)/speed;

            if (OnAnimationStart != null) OnAnimationStart(Card);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (controlArea.Contains(mouseState.Position) &&
                OnHover != null)
                OnHover(Card);

            if (controlArea.Contains(mouseState.Position) && 
                mouseState.LeftButton == ButtonState.Pressed &&
                OnClick != null)
                OnClick(Card);

            if (destination != null)
            {
                if (destination == position)
                {
                    destination = null;

                    if (OnAnimationEnd != null) OnAnimationEnd(Card);
                }
                else
                {
                    destination += posIncrement;
                }
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
