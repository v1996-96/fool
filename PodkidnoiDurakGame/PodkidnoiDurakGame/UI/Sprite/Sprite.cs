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

        protected Texture2D textureImage;
        protected float rotation;
        protected Vector2 rotationOrigin;
        protected Point currentFrame;             // Represents index of current card 
        protected int frameOffset = 5;  // Represents offset between frames
        protected Point frameSize;      // Represents height and width of the frame
        protected Vector2 position;     // Represents current position of the frame on window
        protected float zIndex = 0;
        protected float scale = 1f;

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
        protected Rectangle controlArea
        {
            get
            {
                return new Rectangle(
                    (int)Math.Round(position.X), (int)Math.Round(position.Y),
                    (int)Math.Round(frameSize.X * scale), (int)Math.Round(frameSize.Y * scale)
                    );
            }
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }


        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex)
            : this(textureImage, position, frameSize, currentFrame, zIndex, 5, 1f, 0, Vector2.Zero) { }
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, int frameOffset)
            : this(textureImage, position, frameSize, currentFrame, zIndex, frameOffset, 1f, 0, Vector2.Zero) { }
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, float scale)
            : this(textureImage, position, frameSize, currentFrame, zIndex, 5, scale, 0, Vector2.Zero) { }
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, float zIndex, int frameOffset, float scale)
            : this(textureImage, position, frameSize, currentFrame, zIndex, frameOffset, scale, 0, Vector2.Zero) { }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, 
            float zIndex, int frameOffset, float scale, float rotation, Vector2 rotationOrigin)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.zIndex = zIndex;
            this.rotation = rotation;
            this.rotationOrigin = rotationOrigin;
            this.frameOffset = frameOffset;
            this.scale = scale;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                textureImage,
                position,
                rectangle,
                Color.White,
                rotation,
                rotationOrigin,
                scale,
                SpriteEffects.None,
                zIndex
                );
        }

    }
}
