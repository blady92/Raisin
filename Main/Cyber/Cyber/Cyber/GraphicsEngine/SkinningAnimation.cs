using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber.GraphicsEngine
{
    // UWAGA!!!!!
    // Nie jest to finalnie skończona klasa, bowiem należy:
    // 1. Połączyć funkcje Draw, Update oraz LoadContent z główną grą
    // 2. Ogarnąć logikę wczytywania modeli z wyróżnieniem na modele SkinningData oraz statyczne - obecnie trzeba komentować zbędne linijki...
    // 3. Przenieść stąd sterowanie kamerą do osobnej klasy

    class SkinningAnimation
    {
        #region FIELDS
        GraphicsDeviceManager graphics;
        
        KeyboardState currentKeyboardState = new KeyboardState();
        GamePadState currentGamePadState = new GamePadState();

        Model currentModel;
        Effect myEffect;
        AnimationPlayer animationPlayer;
        Texture2D texture;

        float cameraArc = 0;
        float cameraRotation = 0;
        float cameraDistance = 500;

        private string fieldToModel;
        private string fieldToShader;
        private string fieldToTexture;

        #endregion

        #region INITIALIZATION

        protected void LoadContent(ContentManager theContentManager)
        {
            currentModel = theContentManager.Load<Model>(fieldToModel);
            myEffect = theContentManager.Load<Effect>(fieldToShader);
            texture = theContentManager.Load<Texture2D>(fieldToTexture);

            // <-- ANIMACJA -->
            SkinningData skinningData = currentModel.Tag as SkinningData;
            if (skinningData == null)
                throw new InvalidOperationException
                ("This Model does not contain a SkinningData tag!");

            //Tworzy animation player i zaczyna dekodować animation clip
            animationPlayer = new AnimationPlayer(skinningData);
            AnimationClip clip = skinningData.AnimationClips["Take 001"];
            animationPlayer.StartClip(clip);
        }
        #endregion

        #region UPDATE AND DRAW
        protected void Update(GameTime gameTime)
        {
            UpdateCamera(gameTime);
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
           Update(gameTime);
        }

        protected void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;
            device.Clear(Color.CornflowerBlue);

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

                // <-- WŁASNY EFEKT .FX -->
                
                 foreach(ModelMeshPart meshpart in mesh.MeshParts)
                 {
                     meshpart.Effect = myEffect;
                     myEffect.Parameters["Bones"].SetValue(bones);
                     myEffect.Parameters["View"].SetValue(view);
                     myEffect.Parameters["Projection"].SetValue(projection);
                     myEffect.Parameters["Tekstura"].SetValue(texture);   
                 }  
                

                
                    // <-- SKINNED EFFECT DLA SZKIELETOWYCH -->
                    foreach (SkinnedEffect effect in mesh.Effects)
                    {
                        effect.SetBoneTransforms(bones);

                        effect.View = view;
                        effect.Projection = projection;

                       effect.Texture = texture;
                        effect.EnableDefaultLighting();

                       effect.SpecularColor = new Vector3(0.25f);
                       effect.SpecularPower = 16;
                    }
                    mesh.Draw();
            }
            Draw(gameTime);
        }

        #endregion

        
        #region Handle Input


       //Camera Input Handler
        private void UpdateCamera(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Obracanie kamery góra/dół wokół modelu
            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
                currentKeyboardState.IsKeyDown(Keys.W))
            {
                cameraArc += time * 0.1f;
            }
            
            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
                currentKeyboardState.IsKeyDown(Keys.S))
            {
                cameraArc -= time * 0.1f;
            }

            cameraArc += currentGamePadState.ThumbSticks.Right.Y * time * 0.25f;

            // Ograniczenie na kąt obrotu.
            if (cameraArc > 90.0f)
                cameraArc = 90.0f;
            else if (cameraArc < -90.0f)
                cameraArc = -90.0f;

            // Obracanie kamery wokół obrotu.
            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
                currentKeyboardState.IsKeyDown(Keys.D))
            {
                cameraRotation += time * 0.1f;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
                currentKeyboardState.IsKeyDown(Keys.A))
            {
                cameraRotation -= time * 0.1f;
            }

            cameraRotation += currentGamePadState.ThumbSticks.Right.X * time * 0.25f;

            // Zoom In & Out.
            if (currentKeyboardState.IsKeyDown(Keys.Z))
                cameraDistance += time * 0.25f;

            if (currentKeyboardState.IsKeyDown(Keys.X))
                cameraDistance -= time * 0.25f;

            cameraDistance += currentGamePadState.Triggers.Left * time * 0.5f;
            cameraDistance -= currentGamePadState.Triggers.Right * time * 0.5f;

            // Ograniczenie dystansu kamery.
            if (cameraDistance > 700.0f)
                cameraDistance = 700.0f;
            else if (cameraDistance < 10.0f)
                cameraDistance = 10.0f;
            
            if (currentGamePadState.Buttons.RightStick == ButtonState.Pressed ||
                currentKeyboardState.IsKeyDown(Keys.R))
            {
                cameraArc = 0;
                cameraRotation = 0;
                cameraDistance = 100;
            }
        }


        #endregion
    }


    
}
