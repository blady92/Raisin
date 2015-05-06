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
        public override void Draw(GraphicsDevice device, Matrix world, Matrix view, Matrix projection)
        {
            foreach (SkinningAnimation skinningAnimation in modelList)
            {
                skinningAnimation.DrawStaticModelWithBasicEffect(device, world, view, projection);
            }

        }

        public override void Update(GameTime gameTime, KeyboardState currentKeyboardState, MouseState currentMouseState, ref float cameraArc, ref float cameraRotation, ref float cameraDistance)
        {
            modelList[0].UpdateCamera(gameTime, currentKeyboardState, currentMouseState, ref cameraArc, ref cameraRotation, ref cameraDistance);
        }

        public Matrix[] returnModelTransforms()
        {
            Matrix[] transforms = new Matrix[modelList[0].CurrentModel.Bones.Count];
            modelList[0].CurrentModel.CopyAbsoluteBoneTransformsTo(transforms);
           
            return transforms;
        }
        public int returnModelParentBoneIndex()
        {
            return modelList[0].CurrentModel.Meshes[0].ParentBone.Index;
        }
    }
}
