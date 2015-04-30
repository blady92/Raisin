using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber2O.GameStates
{
    public class GameState
    {
        // Gra może przyjmować kilka stanów
        // 1. Menu główne
        // 2. Pauza w trakcie gry
        // 3. Akcje klikalne z menusów
        // 4. Sama gra
        // Lista dostępnych wywołań statusów to:
            // 1. start
            // 2. load
            // 3. settings
            // 4. exit
            // 5. resume
            // 6. save
            // 7. exitToMenu
            // ... 8. mainMenu
            // ... 9. pauseMenu
        public string StateGame { get; set; }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Draw(GraphicsDevice deviceHere) { }
        //public virtual void Draw(GameTime gameTime) { }

        //public virtual void Draw(GameTime gameTime, GraphicsDevice device) { }
        public virtual void Update(MouseState mouseState) { }
        public virtual void LoadContent(ContentManager theContentManager) { }
    }
}
