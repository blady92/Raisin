using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber2O
{
    public class Game2 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Vector3 testLocation = new Vector3(0,0,0);
        Vector3 test2Location = new Vector3(0, 0, 0);
        private Matrix testMatrixLocation, testMatrixLocation2;
        Matrix view = Matrix.CreateLookAt(new Vector3(60, 60, 50), new Vector3(0, 0, 5), Vector3.UnitZ);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
        private bool drawOnce = true;
        private ModelTest test, test2;
        private BoundingBox a, b;
            

        public Game2()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            test = new ModelTest("Assets/3D/shipX");
            test.LoadContent(this.Content);
        }

        protected override void UnloadContent()
        {}

        protected override void Update(GameTime gameTime)
        {
            testLocation += new Vector3(0, 0.0f, 0);
            test.CreateAABB(test.Model);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Black);
            Matrix testMatrix = Matrix.CreateTranslation(testLocation);    //model view
            testMatrixLocation = Matrix.CreateTranslation(testLocation);   //box view
            DrawModel(test.Model, testMatrix, view, projection);

            Vector3[] AABBVertices = test.AABB.GetCorners();

            VertexPositionColor[] AABBVPC = new VertexPositionColor[AABBVertices.Length];

            for (int i = 0; i < AABBVertices.Length; i++)
            {
                AABBVPC[i] = new VertexPositionColor(AABBVertices[i], Color.White);
            }

            short[] bBoxIndices = {
                0, 1, 1, 2, 2, 3, 3, 0, // Front edges
                4, 5, 5, 6, 6, 7, 7, 4, // Back edges
                0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
            };

            BasicEffect boxEffect = new BasicEffect(this.GraphicsDevice);
            boxEffect.World = testMatrix;
            boxEffect.View = view;
            boxEffect.Projection = projection;
            boxEffect.TextureEnabled = false;
            foreach (EffectPass pass in boxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList, AABBVPC, 0, 8,
                    bBoxIndices, 0, 12);
            }
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}