using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PodkidnoiDurakGame.Core;
using PodkidnoiDurakGame.Core.CardDefinitions;

namespace PodkidnoiDurakGame
{
    class CardSprite : Sprite
    {
        public bool IsAnimated
        {
            get
            {
                return _isAnimated;
            }
        }

        private long UnixTimeNow
        {
            get{
                var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                return (long)timeSpan.TotalMilliseconds;
            }
        }

        public CardUI Card { get; set; }

        private bool _isAnimated;
        private Vector2 _destination;
        private Vector2 _startPosition;
        private long _startTime;
        private long _animTime;

        private bool _previouslyHovered = false;
        private bool _previouslyClicked = false;
        public event Action<CardUI> OnAnimationStart;
        public event Action<CardUI> OnAnimationEnd;
        public event Action<CardUI> OnClick;
        public event Action<CardUI> OnHover;
        public event Action<CardUI> OnUnHover;


        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex)
            :base(textureImage, position, frameSize, currentFrame, zIndex){}
        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, int frameOffset)
            : base(textureImage, position, frameSize, currentFrame, zIndex, frameOffset) { }
        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, float scale)
            : base(textureImage, position, frameSize, currentFrame, zIndex, scale) { }

        public CardSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame,
            float zIndex, int frameOffset, float scale, float rotation, Vector2 rotationOrigin)
            : base(textureImage, position, frameSize, currentFrame, zIndex, frameOffset, scale, rotation, rotationOrigin) { }


        public void Reset(Vector2 position, Texture2D texture, Point currentFrame, float rotation, float zIndex, float resize)
        {
            this.position = position;
            this.textureImage = texture;
            this.rotation = rotation;
            this.zIndex = zIndex;
            this.scale = scale;
            this.currentFrame = currentFrame;
        }
        public void Reset(Texture2D texture, Point currentFrame, float rotation, float zIndex, float resize)
        {
            this.textureImage = texture;
            this.rotation = rotation;
            this.zIndex = zIndex;
            this.scale = scale;
            this.currentFrame = currentFrame;
        }

        public void Animate(Vector2 destination, int speed)
        {
            _isAnimated = true;
            _startPosition = position;
            _destination = destination;
            _animTime = speed;
            _startTime = UnixTimeNow;

            if (OnAnimationStart != null) OnAnimationStart(Card);
        }
        private void ProcessAnimation()
        {
            if (_isAnimated)
            {
                var t = UnixTimeNow - _startTime;
                var c = _destination - _startPosition;
                Vector2 newPos = EaseLinear(t, _startPosition, c, _animTime);

                if (t >= _animTime)
                {
                    _isAnimated = false;
                    position = _destination;
                    if (OnAnimationEnd != null) OnAnimationEnd(Card);
                }
                else
                {
                    position = newPos;
                }
            }
        }
        private Vector2 EaseLinear(long t, Vector2 b, Vector2 c, long d)
        {
            return c * t / d + b;
        }
        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (controlArea.Contains(mouseState.Position))
            {
                _previouslyHovered = true;
                if (OnHover != null) OnHover(Card);

                if (mouseState.LeftButton == ButtonState.Pressed &&
                    !_previouslyClicked)
                {
                    _previouslyClicked = true;
                    if (OnClick != null) OnClick(Card);
                }
                if (mouseState.LeftButton == ButtonState.Released &&
                    _previouslyClicked)
                {
                    _previouslyClicked = false;
                }
            }
            else
            {
                _previouslyClicked = false;
                if (_previouslyHovered)
                {
                    _previouslyHovered = false;
                    if (OnUnHover != null) OnUnHover(Card);
                }
            }

            ProcessAnimation();

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
