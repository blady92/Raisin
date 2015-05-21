using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dhpoware;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.GraphicsEngine.Bilboarding
{
    class BilboardTestSystem
    {
        public Billboard bilboard { get; set; }
        public Texture2D bilboardTexture { get; set; }
        public List<Vector3> BilboardPositionsList { get; set; }
        public Effect BilboardEffect { get; set; }
        public Vector2 BilboardSize { get; set; }

        private BillboardingTechnique billboardingTechnique;

        //Setup Frame Buffer
        //    graphics.SynchronizeWithVerticalRetrace = false;
        //    graphics.PreferredBackBufferWidth = windowWidth;
        //    graphics.PreferredBackBufferHeight = windowHeight;
        //    graphics.PreferMultiSampling = true;
        //    graphics.ApplyChanges();

        //Ładowanie oraz inicjalizacja
        public void LoadContent(GraphicsDevice device, ContentManager theContentManager)
        {
            #region Potencjalne ustawienia kamer
            //cameraComponent.CurrentBehavior = Camera.Behavior.Orbit;
            //cameraComponent.ClickAndDragMouseRotation = true;
            //cameraComponent.Perspective(90.0f, (float)windowWidth / (float)windowHeight, 0.1f, 3.0f * gridComponent.GridSize);
            //cameraComponent.LookAt(new Vector3(0.0f, gridComponent.GridSize * 0.25f, gridComponent.GridSize * 0.5f), Vector3.Zero, Vector3.Up);
            //cameraComponent.OrbitMinZoom = gridComponent.GridSize * 0.25f;
            //cameraComponent.OrbitMaxZoom = gridComponent.GridSize * 1.5f;
            //cameraComponent.OrbitOffsetDistance = cameraComponent.Position.Length();
#endregion  

            List<Vector3> BilboardPositionsList = new List<Vector3>();
            bilboard = new Billboard(device,
                                      BilboardPositionsList.ToArray(),
                                      1.0f,
                                      1.0f,
                                      0.0f,
                                      0.0f);

            bilboardTexture = theContentManager.Load<Texture2D>("Assets/2D/Bilboard/buttonE");
            BilboardEffect = theContentManager.Load<Effect>("Effects/ShadersFX/Billboard");
            BilboardEffect.CurrentTechnique = BilboardEffect.Techniques["BillboardingWorldYAxisAligned"];
            billboardingTechnique = BillboardingTechnique.WorldYAxisAligned;
        }

        public void Update()
        {
            
        }

        public void DrawBilboard(GameTime gameTime, GraphicsDevice device, Matrix View, Matrix Projection)
        {
            RasterizerState prevRasterizerState = device.RasterizerState;
            BlendState prevBlendState = device.BlendState;

            BilboardEffect.Parameters["world"].SetValue(Matrix.Identity);
            //BilboardEffect.Parameters["view"].SetValue(cameraComponent.ViewMatrix);
            //BilboardEffect.Parameters["projection"].SetValue(cameraComponent.ProjectionMatrix);
            BilboardEffect.Parameters["billboardSize"].SetValue(BilboardSize);
            BilboardEffect.Parameters["colorMap"].SetValue(bilboardTexture);
            //BilboardEffect.Parameters["animationTime"].SetValue(billboardAnimationTime);
            //BilboardEffect.Parameters["animationScaleFactor"].SetValue(BILLBOARD_ANIM_SCALE);
            BilboardEffect.Parameters["alphaTestDirection"].SetValue(1.0f);

            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = RasterizerState.CullNone;

            bilboard.Draw(device, BilboardEffect);


            //Rendering przezroczystych pixeli

            BilboardEffect.Parameters["alphaTestDirection"].SetValue(-1.0f);

            device.BlendState = BlendState.NonPremultiplied;
            device.DepthStencilState = DepthStencilState.DepthRead;

            bilboard.Draw(device, BilboardEffect);

            // Restore original states.
            device.BlendState = prevBlendState;
            device.RasterizerState = prevRasterizerState;
        }

        public void Draw(GameTime gameTime, GraphicsDevice device, Matrix view, Matrix projection) 
        {
            device.Clear(Color.CornflowerBlue);
            device.DepthStencilState = DepthStencilState.Default;
            DrawBilboard(gameTime, device, view, projection);
        }
    }

    enum BillboardingTechnique
    {
        CameraAligned,
        WorldYAxisAligned
    }
}
