using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.GraphicsEngine
{
    class ModelTest
    {

        private Vector3 position;
        private Model model;
        private string fieldToAsset;
        
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }
        public ModelTest(string assetName)
        {
            position = new Vector3(0, 0, 0);
            this.fieldToAsset = assetName;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            model = theContentManager.Load<Model>(fieldToAsset);
        }

        public void DrawModel(Matrix world, Matrix view, Matrix projection)
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
