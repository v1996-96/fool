using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PodkidnoiDurakGame
{
    abstract class Sprite
    {

        Texture2D textureImage;
        float rotation;
        Vector2 rotationOrigin;
        Point currentFrame;             // Represents index of current card 
        protected int frameOffset = 5;  // Represents offset between frames
        protected Point frameSize;      // Represents height and width of the frame
        protected Vector2 position;     // Represents current position of the frame on window
        protected float zIndex = 0;

        public abstract Vector2 direction { set; get; }
        protected Rectangle rectangle
        {
            get
            {
                return new Rectangle(
                    currentFrame.X * frameSize.X + (currentFrame.X + 1) * frameOffset,
                    currentFrame.Y * frameSize.Y + (currentFrame.Y + 1) * frameOffset,
                    frameSize.X, frameSize.Y);
            }
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex)
            : this(textureImage, position, frameSize, currentFrame, zIndex, 0, Vector2.Zero) { }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, float rotation, Vector2 rotationOrigin)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.zIndex = zIndex;
            this.rotation = rotation;
            this.rotationOrigin = rotationOrigin;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                textureImage,
                position,
                rectangle,
                Color.Transparent,
                rotation,
                rotationOrigin,
                1f,
                SpriteEffects.None,
                zIndex
                );
        }

    }
}
