using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber2O
{
    public class ModelTest
    {
        private string fieldToAsset;
        private Model model;
        private BoundingBox aabb;
        private short[] indexData; // The index array used to render the AABB.
        private VertexPositionColorTexture[] aabbVertices; // The AABB vertex array (used for rendering).
        private BasicEffect renderer; // The basic effect used to render the AABB.
        Vector3[] vertexData;

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        public BoundingBox AABB
        {
            get { return aabb; }
            set { aabb = value; }
        }
        public ModelTest(string assetName)
        {
            fieldToAsset = assetName;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            model = theContentManager.Load<Model>(fieldToAsset);
            CreateAABB(model);
        }

        public void Update(Model model)
        {
            CreateAABB(model);
        }
        //Dat loading vertices from generic models...
        //No kurwa, wreszcie działa dla dowolnego modelu
        //XNA - suck my cock, I did my job well.
        public void CreateAABB(Model model)
        {
            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);
            Vector3 modelMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 modelMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            float minX, minY, minZ;
            float maxX, maxY, maxZ;
            minX = minY = minZ = maxX = maxY = maxZ = 0;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    vertexData = new Vector3[part.VertexBuffer.VertexCount]; 
                    part.VertexBuffer.GetData<Vector3>(vertexData);
                    Vector3 vertPosition;
                    for (int i = 0; i < part.VertexBuffer.VertexCount; i++)
                    {
                        vertPosition = new Vector3(vertexData[i].X, vertexData[i].Y, vertexData[i].Z);
                        vertPosition = vertexData[i];
                        
                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                    //Debug.WriteLine("Wierzchołków jest: "+part.VertexBuffer.VertexCount);
                }
                Matrix meshTransform = Matrix.CreateTranslation(new Vector3(0, 0, -2));
                meshMin = Vector3.Transform(meshMin, meshTransform);
                meshMax = Vector3.Transform(meshMax, meshTransform);
                modelMin = Vector3.Min(modelMin, meshMin);
                modelMax = Vector3.Max(modelMax, meshMax);
            }
            //Vector3 minimum = new Vector3(minX, minY, minZ-5);
            //Vector3 maximum = new Vector3(maxX, maxY, maxZ);
            aabb = new BoundingBox(modelMin, modelMax);
        }
    }
}
