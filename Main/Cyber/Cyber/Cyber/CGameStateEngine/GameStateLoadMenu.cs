using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cyber.CLogicEngine;


namespace Cyber.CGameStateEngine
{
    class GameStateLoadMenu : GameState
    {
        SkinningAnimation modelLoader = new SkinningAnimation();
        CameraBehavior cameraBehavior = new CameraBehavior();
        //KeyboardState currentKeyboardState = new KeyboardState();
      
        public void LoadContent(ContentManager theContentManager)
        {
            modelLoader.LoadContent_StaticModel(theContentManager, "Assets/3D/Interior/oxygen_generator");
        }
        public override void Draw(GraphicsDevice device)
        {
            modelLoader.DrawStaticModelWithBasicEffect(device);
        }

        public override void Update(GameTime gameTime, KeyboardState currentKeyboardState)
        {
            cameraBehavior.UpdateCamera(gameTime, currentKeyboardState);
        }
    }
}
