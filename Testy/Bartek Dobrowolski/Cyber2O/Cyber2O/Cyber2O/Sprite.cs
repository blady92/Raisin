using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            sprite = theContentManager.Load<Texture2D>(theAssetName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, Color.White);
        }
    }
}
