using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber2O
{
    public class SpriteAnimationDynamic
    {
        public Texture2D Texture { get; set; }
        private int currentFrame;
        private int totalFrames;
        private Vector2 spritePosition;

        public int Rows
        {
            get; set;
        }

        public int Columns
        {
            get; set;
        }

        public Vector2 SpritePosition { get; set; }
        public SpriteAnimationDynamic(Texture2D texture2D, int rows, int columns, Vector2 position)
        {
            this.Texture = texture2D;
            this.Rows = rows;
            this.Columns = columns;
            this.spritePosition = position;
            currentFrame = 0;
            totalFrames = rows * columns;
        }

        public SpriteAnimationDynamic(Texture2D texture2D, int rows, int columns)
        {
            this.Texture = texture2D;
            this.Rows = rows;
            this.Columns = columns;
            currentFrame = 0;
            totalFrames = rows*columns;
        }

        //For drawing static place postion
        public void Draw(SpriteBatch spriteBatch)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)this.spritePosition.X, (int)this.spritePosition.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }

        //For drawing changeable place position
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

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)spritePosition.X, (int)spritePosition.Y, Texture.Width/Columns, Texture.Height/Rows);
        }
    }
}
