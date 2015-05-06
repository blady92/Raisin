using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.GraphicsEngine
{

    class SkinningAnimation
    {
        #region FIELDS
        Model currentModel;
        Effect myEffect;
        AnimationPlayer animationPlayer;
        Texture2D texture;

        float cameraArc = 0;
        float cameraRotation = 0;
        float cameraDistance = 500;

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

        public void DrawSkinnedModelWithSkinnedEffect(GameTime gameTime, GraphicsDevice device, Matrix view, Matrix projection)
        {
            Matrix[] bones = animationPlayer.GetSkinTransforms();

            Matrix[] transforms = new Matrix[currentModel.Bones.Count];
            currentModel.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix world = transforms[currentModel.Meshes[0].ParentBone.Index];

            //Render zeskinowany mesh
            foreach(ModelMesh mesh in currentModel.Meshes)
            { 
                    // <-- SKINNED EFFECT DLA SZKIELETOWYCH -->
                    foreach (SkinnedEffect effect in mesh.Effects)
                    {
                        effect.SetBoneTransforms(bones);

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

        public void DrawSkinnedModelWithSkinnedEffect(GameTime gameTime, GraphicsDevice device)
        {
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
                // <-- SKINNED EFFECT DLA SZKIELETOWYCH -->
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

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

         public void DrawStaticModelWithBasicEffect(GraphicsDevice device)
        {
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
                    
                mesh.Draw();
            }
        }

         public void DrawStaticModelWithShader(GameTime gameTime, GraphicsDevice device)
        {

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

         

                

        #endregion

    }


    
}
