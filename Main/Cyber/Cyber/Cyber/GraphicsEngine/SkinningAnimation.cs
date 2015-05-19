using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Cyber.GraphicsEngine
{

    class SkinningAnimation
    {
        #region FIELDS
        Model currentModel;
        Effect myEffect;
        AnimationPlayer animationPlayer;
        Texture2D texture;
        MouseState prevMouseState;

        //float cameraArc = 0;
        //float cameraRotation = 0;
        //float cameraDistance = 500;

        #endregion

        public Model CurrentModel
        {
            get { return currentModel; }
            set { currentModel = value; }
        }

        #region INITIALIZATION

        public SkinningAnimation()
        { }
         
        public void LoadContent_SkinnedModel(ContentManager theContentManager, string pathToModel, string animationClipName)
        {
            this.currentModel = theContentManager.Load<Model>(pathToModel);
         
            // <-- ANIMACJA -->
            SkinningData skinningData = currentModel.Tag as SkinningData;
            if (skinningData == null)
                throw new InvalidOperationException
                ("This Model does not contain a SkinningData tag!");

            //Tworzy animation player i zaczyna dekodować animation clip
            animationPlayer = new AnimationPlayer(skinningData);
            AnimationClip clip = skinningData.AnimationClips[animationClipName];
            animationPlayer.StartClip(clip);
        }

        public void LoadContent_StaticModel(ContentManager theContentManager, string pathToModel)
        {
            currentModel = theContentManager.Load<Model>(pathToModel);
        }

        public void LoadContent_ShaderEffect(ContentManager theContentManager, string pathToEffect)
        {
            myEffect = theContentManager.Load<Effect>(pathToEffect);
        }
        
        public void LoadContent_Texture(ContentManager theContentManager, string pathToTexture)
        {
            texture = theContentManager.Load<Texture2D>(pathToTexture);
        }

        #endregion

        #region UPDATE AND DRAW
        protected void Update(GameTime gameTime)
        {
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            Update(gameTime);
        }


        public void DrawSkinnedModelWithSkinnedEffect(GameTime gameTime, GraphicsDevice device, Matrix world, Matrix view, Matrix projection)
        {

            Matrix[] bones = animationPlayer.GetSkinTransforms();

            //Matrix[] transforms = new Matrix[currentModel.Bones.Count];
            //currentModel.CopyAbsoluteBoneTransformsTo(transforms);

            //Matrix world = transforms[currentModel.Meshes[0].ParentBone.Index];

            //Render zeskinowany mesh
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                // <-- SKINNED EFFECT DLA SZKIELETOWYCH -->
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;

                    // effect.Texture = texture;
                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }
                mesh.Draw();
            }
        }
        public void DrawSkinnedModelWithShader(GameTime gameTime, GraphicsDevice device)
        {
            float cameraArc = 0;
            float cameraRotation = 0;
            float cameraDistance = 500;
            Matrix[] bones = animationPlayer.GetSkinTransforms();

            Matrix[] transforms = new Matrix[currentModel.Bones.Count];
            currentModel.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix world = transforms[currentModel.Meshes[0].ParentBone.Index];

            Matrix view = Matrix.CreateTranslation(0, -40, 0) *
                          Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation)) *
                          Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                          Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                              new Vector3(0, 30, 100), Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1, 100000);

            //Render zeskinowany mesh
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                // <-- WŁASNY EFEKT .FX -->

                foreach (ModelMeshPart meshpart in mesh.MeshParts)
                {
                    meshpart.Effect = myEffect;
                    myEffect.Parameters["Bones"].SetValue(bones);
                    myEffect.Parameters["View"].SetValue(view);
                    myEffect.Parameters["Projection"].SetValue(projection);
                    myEffect.Parameters["Tekstura"].SetValue(texture);
                }  
                mesh.Draw();
            }
        }

         public void DrawStaticModelWithBasicEffect(GraphicsDevice device, Matrix world, Matrix view, Matrix projection)
        {
            //Render zeskinowany mesh
            foreach(ModelMesh mesh in currentModel.Meshes)
            { 
                    //BasicEffect
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;

                    effect.LightingEnabled = true;
                    // <-- Światło ogólne -->
                    effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
                    effect.DiffuseColor = new Vector3(0.65f, 0.65f, 0.65f);
                    effect.EmissiveColor = new Vector3(0.0f, 0.0f, 0.25f);
                }
                    
                mesh.Draw();
            }
        }

         public void DrawStaticModelWithShader(GameTime gameTime, GraphicsDevice device)
        {
            float cameraArc = 0;
            float cameraRotation = 0;
            float cameraDistance = 500;
            Matrix[] transforms = new Matrix[currentModel.Bones.Count];
            currentModel.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix world = transforms[currentModel.Meshes[0].ParentBone.Index];

            Matrix view = Matrix.CreateTranslation(0, -40, 0) *
                          Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation)) *
                          Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                          Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                              new Vector3(0, 30, 100), Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1, 100000);

            //Render zeskinowany mesh
            foreach(ModelMesh mesh in currentModel.Meshes)
            {
                // <-- WŁASNY EFEKT .FX -->

                foreach (ModelMeshPart meshpart in mesh.MeshParts)
                {
                    meshpart.Effect = myEffect;
                  //  myEffect.Parameters["Bones"].SetValue(bones);
                    myEffect.Parameters["View"].SetValue(view);
                    myEffect.Parameters["Projection"].SetValue(projection);
                    myEffect.Parameters["Tekstura"].SetValue(texture);
                }  
                
                    
                mesh.Draw();
            }
        }


         public void UpdateCamera(GraphicsDevice device, GameTime gameTime, KeyboardState currentKeyboardState, MouseState currentMouseState, ref float cameraArc, ref float cameraRotation, ref float cameraDistance)
         {
             float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
             float cameraSpeed = 0.1f;
             

             #region STARE DZIAŁANIE KAMERY - klawiatura
             //float cameraZoomStep = 0.25f;
        //     // Obracanie kamery góra/dół wokół modelu
        //     if (currentKeyboardState.IsKeyDown(Keys.Up) ||
        //         currentKeyboardState.IsKeyDown(Keys.W))
        //     {
        //         cameraArc += time * cameraSpeed;
        //         Debug.WriteLine("In SkinningAnimation: UP KEY PRESSED, or W perhaps");
        //         Debug.WriteLine("cameraArc: ");
        //         Debug.WriteLine(cameraArc);
        //     }

        //     if (currentKeyboardState.IsKeyDown(Keys.Down) ||
        //         currentKeyboardState.IsKeyDown(Keys.S))
        //     {
        //         cameraArc -= time * cameraSpeed;
        //         Debug.WriteLine("In SkinningAnimation: DOWN KEY PRESSED, or W perhaps");
        //         Debug.WriteLine("cameraArc: ");
        //         Debug.WriteLine(cameraArc);
        //     }

        //    // cameraArc += currentGamePadState.ThumbSticks.Right.Y * time * 0.25f;

        //     // Ograniczenie na kąt obrotu.
        //     if (cameraArc > 90.0f)
        //         cameraArc = 90.0f;
        //     else if (cameraArc < -90.0f)
        //         cameraArc = -90.0f;

        //     // Obracanie kamery wokół obrotu.
        //     if (currentKeyboardState.IsKeyDown(Keys.Right) ||
        //         currentKeyboardState.IsKeyDown(Keys.D))
        //     {
        //         cameraRotation += time * cameraSpeed;
        //         Debug.WriteLine("In SA: RIGHT KEY/W, cameraRotation: " + cameraRotation);

        //     }

        //     if (currentKeyboardState.IsKeyDown(Keys.Left) ||
        //         currentKeyboardState.IsKeyDown(Keys.A))
        //     {
        //         cameraRotation -= time * cameraSpeed;
        //         Debug.WriteLine("In SA: LEFT KEY/S, cameraRotation: " + cameraRotation);
        //     }

        //  //   cameraRotation += currentGamePadState.ThumbSticks.Right.X * time * 0.25f;

        //     // Zoom In & Out.
        //     if (currentKeyboardState.IsKeyDown(Keys.Z))
        //         cameraDistance += time * cameraZoomStep;

        //     if (currentKeyboardState.IsKeyDown(Keys.X))
        //         cameraDistance -= time * cameraZoomStep;

        ////     cameraDistance += currentGamePadState.Triggers.Left * time * 0.5f;
        //        //cameraDistance -= currentGamePadState.Triggers.Right * time * 0.5f;

        //     // Ograniczenie dystansu kamery.
        //     if (cameraDistance > 6000.0f)
        //         cameraDistance = 6000.0f;
        //     else if (cameraDistance < 10.0f)
        //         cameraDistance = 10.0f;

        //     if (currentKeyboardState.IsKeyDown(Keys.R))
        //     {
        //         cameraArc = 0;
        //         cameraRotation = 0;
        //         cameraDistance = 100;
        //     }
             #endregion

             #region NOWE DZIAŁANIE KAMERY - mysz
    

             //Debug.Write("Yo, ur mouse x is: ");
             //Debug.WriteLine(currentMouseState.X);

             //Debug.Write("Yo, ur mouse y is: ");
             //Debug.WriteLine(currentMouseState.Y);

             // Ograniczenie na kąt obrotu.
                  if (cameraArc > 37.0f)
                  {
                      cameraArc = 35.0f;
                  } 
                  else if (cameraArc < 33.0f)
                  {
                      cameraArc = 35.0f;
                  }

             if ((currentMouseState.X != prevMouseState.X || currentMouseState.Y != prevMouseState.Y) && (currentMouseState.X < prevMouseState.X) && (currentMouseState.X > 0) && (currentMouseState.X < device.Viewport.Width) && (currentMouseState.LeftButton == ButtonState.Pressed))
             {
                 cameraRotation -= time * cameraSpeed;
                 prevMouseState = currentMouseState;
                 Debug.WriteLine("cr: " + cameraRotation);
             }
             if ((currentMouseState.X != prevMouseState.X || currentMouseState.Y != prevMouseState.Y) && (currentMouseState.X > prevMouseState.X) && (currentMouseState.X > 0) && (currentMouseState.X < device.Viewport.Width) && (currentMouseState.LeftButton == ButtonState.Pressed))
             {
                 cameraRotation += time * cameraSpeed;
                 prevMouseState = currentMouseState;
                 Debug.WriteLine("cr: " + cameraRotation);
             }
             if ((currentMouseState.Y != prevMouseState.Y || currentMouseState.X != prevMouseState.X) && (currentMouseState.Y > prevMouseState.Y) && (currentMouseState.Y > 0) && (currentMouseState.Y < device.Viewport.Height) && (currentMouseState.LeftButton == ButtonState.Pressed))
             {
                 cameraArc += time * cameraSpeed;
                 prevMouseState = currentMouseState;
             }
             if ((currentMouseState.Y != prevMouseState.Y || currentMouseState.X != prevMouseState.X) && (currentMouseState.Y < prevMouseState.Y) && (currentMouseState.Y > 0) && (currentMouseState.Y < device.Viewport.Height) && (currentMouseState.LeftButton == ButtonState.Pressed))
             {
                 cameraArc -= time * cameraSpeed;
                 prevMouseState = currentMouseState;
             }
             if(currentMouseState.RightButton == ButtonState.Pressed)
             {
                  cameraArc = 35.0f;
                  cameraRotation = -360.0f;
                  cameraDistance = 6000;
             }
             #endregion
         }

        public void UpdatePlayer(GameTime gameTime)
         {
             animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity); // rób to -> przekaz do dude i powinno działać
         }
                

        #endregion

    }


    
}
