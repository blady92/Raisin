using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber2O
{
    class Sprite
    {
        public Vector2 Position = new Vector2(0,0);
        private Texture2D sprite;
        private int x;
        private int y;

        public Sprite(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vector2 PositionAccessor
        {
            get { return Position; }
            set { Position = value; }
        }

        public Texture2D SpriteAccessor
        {
            get { return sprite; }
            set { sprite = value; }
        }


        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            sprite = theContentManager.Load<Texture2D>(theAssetName);
        }

        public void DrawByRectangle(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, new Rectangle(x, y, sprite.Width, sprite.Height), Color.White);
            spriteBatch.End();
        }

        public void DrawByVector(SpriteBatch spriteBatch, MouseState mouse)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, new Rectangle(mouse.X, mouse.Y, sprite.Width, sprite.Height), Color.White);
            spriteBatch.End();
        }


        public void DrawByVector(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, new Rectangle((int)Position.X, (int)Position.Y, sprite.Width, sprite.Height), Color.White);
            spriteBatch.End();
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, sprite.Width, sprite.Height);
        }
    }
}
