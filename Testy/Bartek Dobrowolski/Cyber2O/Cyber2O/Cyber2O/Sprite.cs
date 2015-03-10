using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber2O
{
    class Sprite
    {
        public Vector2 Position = new Vector2(0,0);
        private Texture2D sprite;
        private Texture2D spriteHover;

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

        public Texture2D SpriteHoverAccessor
        {
            get { return spriteHover; }
            set { spriteHover = value; }
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            sprite = theContentManager.Load<Texture2D>(theAssetName);
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName, string theAssetHover)
        {
            sprite = theContentManager.Load<Texture2D>(theAssetName);
            spriteHover = theContentManager.Load<Texture2D>(theAssetHover);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, Color.White);
        }
    }
}
