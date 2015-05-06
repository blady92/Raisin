using System.Collections.Generic;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Cyber.CGameStateEngine
{
    class GameStateLoadMenu : GameState
    {
        float cameraArc = 0;
        float cameraRotation = 0;
        float cameraDistance = 500;


        SkinningAnimation modelLoader = new SkinningAnimation();
        List<SkinningAnimation> modelList = new List<SkinningAnimation>();
        List<string> modelPathList = new List<string>();
      
        public void LoadContent(ContentManager theContentManager)
        {
           // Ważna kolejność
            string interiorPath = "Assets/3D/Interior/Interior_";

            modelPathList.Add(interiorPath+"Oxygen_Generator");
            modelPathList.Add(interiorPath + "Chair");
            modelPathList.Add(interiorPath + "Wall_Base");
            modelPathList.Add(interiorPath + "Wall_Base");
            modelPathList.Add(interiorPath + "Wall_Base");
            modelPathList.Add(interiorPath + "Wall_Base");
            modelPathList.Add(interiorPath + "Wall_Base");
            modelPathList.Add(interiorPath + "Wall_Base");
            for (int i = 0; i < 1; i++) { 
                modelList.Add(new SkinningAnimation());
                modelList[i].LoadContent_StaticModel(theContentManager, modelPathList[i]);
            }

           
            
         
        }
        public override void Draw(GraphicsDevice device)
        {
              Matrix view = Matrix.CreateTranslation(0, -40, 0) *
              Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation)) *
              Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
              Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                  new Vector3(0, 30, 100), Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1, 100000);

            foreach (SkinningAnimation skinningAnimation in modelList)
            {
                skinningAnimation.DrawStaticModelWithBasicEffect(device);
            }

        }

        public override void Update(GameTime gameTime, KeyboardState currentKeyboardState)
        {
            modelList[0].UpdateCamera(gameTime, currentKeyboardState);
        }
    }
}
