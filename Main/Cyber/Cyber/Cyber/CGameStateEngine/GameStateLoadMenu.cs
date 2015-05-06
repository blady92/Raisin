using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        List<SkinningAnimation> modelList = new List<SkinningAnimation>();
        List<string> modelPathList = new List<string>();
        //KeyboardState currentKeyboardState = new KeyboardState();
      
        public void LoadContent(ContentManager theContentManager)
        {
            //Ważna kolejność
            string interiorPath = "Assets/3D/Interior/Interior_";

            modelPathList.Add(interiorPath+"Oxygen_Generator");
            //modelPathList.Add(interiorPath+"Chair");
            //modelPathList.Add(interiorPath+"Wall_Base");
            //modelPathList.Add(interiorPath + "Wall_Base");
            //modelPathList.Add(interiorPath + "Wall_Base");
            //modelPathList.Add(interiorPath + "Wall_Base");
            //modelPathList.Add(interiorPath + "Wall_Base");
            //modelPathList.Add(interiorPath + "Wall_Base");
            for (int i = 0; i < 1; i++) { 
                modelList.Add(new SkinningAnimation());
                modelList[i].LoadContent_StaticModel(theContentManager, modelPathList[i]);
            }
            //modelList[1].LoadContent_StaticModel(theContentManager, "Assets/3D/Interior/Interior_Chair");
            //modelLoader.LoadContent_StaticModel(theContentManager, pathInterior+"");
        }
        public override void Draw(GraphicsDevice device)
        {
            foreach (SkinningAnimation skinningAnimation in modelList)
            {
                skinningAnimation.DrawStaticModelWithBasicEffect(device);
            }
            //modelLoader.DrawStaticModelWithBasicEffect(device);
        }

        public override void Update(GameTime gameTime, KeyboardState currentKeyboardState)
        {
            cameraBehavior.UpdateCamera(gameTime, currentKeyboardState);
        }
    }
}
