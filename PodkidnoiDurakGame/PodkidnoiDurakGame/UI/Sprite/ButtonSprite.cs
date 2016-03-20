using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PodkidnoiDurakGame.UI.ElementDefenitions;

namespace PodkidnoiDurakGame
{
    class ButtonSprite : Sprite
    {
        public ButtonType ButtonType { get; set; }

        private bool _previouslyHovered = false;
        public event Action OnUnHover;
        public event Action OnHover;
        public event Action OnClick;

        public ButtonSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex)
            :base(textureImage, position, frameSize, currentFrame, zIndex){}
        public ButtonSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, int frameOffset)
            : base(textureImage, position, frameSize, currentFrame, zIndex, frameOffset) { }
        public ButtonSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, float scale)
            : base(textureImage, position, frameSize, currentFrame, zIndex, scale) { }

        public ButtonSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame,
            float zIndex, int frameOffset, float scale, float rotation, Vector2 rotationOrigin)
            : base(textureImage, position, frameSize, currentFrame, zIndex, frameOffset, scale, rotation, rotationOrigin) { }

        public void Reset(Point currentFrame)
        {
            base.currentFrame = currentFrame;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (controlArea.Contains(mouseState.Position))
            {
                _previouslyHovered = true;
                if (OnHover != null) OnHover();
            }
            else
            {
                if (_previouslyHovered)
                {
                    _previouslyHovered = false;
                    if (OnUnHover != null) OnUnHover();   
                }
            }

            if (controlArea.Contains(mouseState.Position) &&
                mouseState.LeftButton == ButtonState.Pressed &&
                OnClick != null)
                OnClick();

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
