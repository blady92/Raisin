#region File Description
//-----------------------------------------------------------------------------
// SkinningSample.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkinnedModel;
#endregion

namespace SkinningSample
{
    /// <summary>
    /// Sample game showing how to display skinned character animation.
    /// </summary>
    public class SkinningSampleGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;

        KeyboardState currentKeyboardState = new KeyboardState();
        GamePadState currentGamePadState = new GamePadState();

        //Model currentModel;
        Model oxygenGenerator;

        Effect myEffect;
        Effect testEffect;
        AnimationPlayer animationPlayer;
        Texture2D texture; 

        float cameraArc = 0;
        float cameraRotation = 0;
        float cameraDistance = 500;

        #endregion

        #region Initialization


        public SkinningSampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if WINDOWS_PHONE
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            
            graphics.IsFullScreen = true;            
#endif
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load the model.
            oxygenGenerator = Content.Load<Model>("dude");

            //Load the effect.
            myEffect = Content.Load<Effect>("SkinnedModel");
            texture = Content.Load<Texture2D>("grungie");
            testEffect = Content.Load<Effect>("TestEffect");

            // <-- ANIMACJA -->

            // Look up our custom skinning information.
          SkinningData skinningData = oxygenGenerator.Tag as SkinningData;

if (skinningData == null)
                throw new InvalidOperationException
("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

           AnimationClip clip = skinningData.AnimationClips["Take 001"];
animationPlayer.StartClip(clip);
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the game to run logic.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            UpdateCamera(gameTime);
            
           animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            device.Clear(Color.CornflowerBlue);

          Matrix[] bones = animationPlayer.GetSkinTransforms();

            // Compute camera matrices.
            // Zbudowanie macierzy WORLD
            Matrix[] transforms = new Matrix[oxygenGenerator.Bones.Count];
            oxygenGenerator.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix world = transforms[oxygenGenerator.Meshes[0].ParentBone.Index];
            
            // Zbudowanie macierzy VIEW
            Matrix view = Matrix.CreateTranslation(0, -40, 0) * 
                          Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation)) *
                          Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                          Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance), 
                                              new Vector3(0, 30, 100), Vector3.Up);

            // Zbudowanie macierzy PROJECTION
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    device.Viewport.AspectRatio,
                                                                    1,
                                                                    10000);


       
            
            // Render the skinned mesh.
            foreach (ModelMesh mesh in oxygenGenerator.Meshes)
            {
                /*
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.LightingEnabled = true;
                  //  effect.Alpha = 0.5f;
                    // <-- �wiat�o og�lne -->
                    effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
                    effect.DiffuseColor = new Vector3(0.65f, 0.65f, 0.65f);
                    effect.EmissiveColor = new Vector3(0.0f, 0.0f, 0.25f);
                   
                } */
                

             
                // <-- W�ASNY EFEKT .FX -->
               /*
                foreach(ModelMeshPart meshpart in mesh.MeshParts)
                {
                    meshpart.Effect = myEffect;
                    myEffect.Parameters["Bones"].SetValue(bones);
                    myEffect.Parameters["View"].SetValue(view);
                    myEffect.Parameters["Projection"].SetValue(projection);
                    myEffect.Parameters["Tekstura"].SetValue(texture);   
                }  
               */
                
                
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
        
            base.Draw(gameTime);
        }

        
        #endregion

        #region Handle Input


        /// <summary>
        /// Handles input for quitting the game.
        /// </summary>
        private void HandleInput()
        {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // Check for exit.
            if (currentKeyboardState.IsKeyDown(Keys.Escape) ||
                currentGamePadState.Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }
        }


        /// <summary>
        /// Handles camera input.
        /// </summary>
        private void UpdateCamera(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check for input to rotate the camera up and down around the model.
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

            // Limit the arc movement.
            if (cameraArc > 90.0f)
                cameraArc = 90.0f;
            else if (cameraArc < -90.0f)
                cameraArc = -90.0f;

            // Check for input to rotate the camera around the model.
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

            // Check for input to zoom camera in and out.
            if (currentKeyboardState.IsKeyDown(Keys.Z))
                cameraDistance += time * 0.25f;

            if (currentKeyboardState.IsKeyDown(Keys.X))
                cameraDistance -= time * 0.25f;

            cameraDistance += currentGamePadState.Triggers.Left * time * 0.5f;
            cameraDistance -= currentGamePadState.Triggers.Right * time * 0.5f;

            // Limit the camera distance.
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


    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (SkinningSampleGame game = new SkinningSampleGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}
