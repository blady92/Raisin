using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber2O
{
    public class SpriteAnimation
    {
        public Texture2D Texture { get; set; }
        private int currentFrame;
        private int totalFrames;

        public int Rows
        {
            get; set;
        }

        public int Columns
        {
            get; set;
        }

        public SpriteAnimation(Texture2D texture2D, int rows, int columns)
        {
            this.Texture = texture2D;
            this.Rows = rows;
            this.Columns = columns;
            currentFrame = 0;
            totalFrames = rows*columns;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
        public void Update()
        {
            currentFrame++;
            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
            }
        }
    }
}
