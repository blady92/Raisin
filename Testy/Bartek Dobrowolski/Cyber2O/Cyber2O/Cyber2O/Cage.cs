using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber2O
{
    class Cage
    {
        private BoundingBox aabb; //Sama klatka

        //Startowa pozycja zerowa i później ustawianie tego.
        //Wykorzystywane  do rysowania BoudingBoxa
        private Vector3 position; 
        Vector3[] vertexData;

        //kalkulacja bouding boxa dla całego obieku łącznie z meshami częściowymi w modelu
        private Vector3 meshMin, meshMax, modelMin, modelMax;
        //to przeuswania całego bouding boxa
        private float offsetX, offsetY, offsetZ;
        private float resizeX, resizeY, resizeZ;
        //Do późniejszego wykorzsytania przy skalowaniu bouding boxa z palca
        float minX, minY, minZ;
        float maxX, maxY, maxZ;
        //Na potrzeby kolejności rysowania bouding boxa
        short[] bBoxIndices = {
                0, 1, 1, 2, 2, 3, 3, 0, // Front edges
                4, 5, 5, 6, 6, 7, 7, 4, // Back edges
                0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
            };

        public BoundingBox AABB { get { return aabb; }
            set { aabb = value;  }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public short[] Indices
        {
            get { return bBoxIndices; }
            set { bBoxIndices = value; }
        }

        public Cage()
        {
            position = new Vector3(0, 0, 0);
        }

        //Ustalenie bouding boxa w jednym miejscu
        public void SetBoudings(Model model)
        {
            meshMax = new Vector3(float.MinValue);
            meshMin = new Vector3(float.MaxValue);
            modelMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            modelMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
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
                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }
                Matrix meshTransform = Matrix.CreateTranslation(new Vector3(0, 0, -2));
                meshMin = Vector3.Transform(meshMin, meshTransform);
                meshMax = Vector3.Transform(meshMax, meshTransform);
                modelMin = Vector3.Min(modelMin, meshMin);
                modelMax = Vector3.Max(modelMax, meshMax);
            }
            minX = modelMin.X;
            minY = modelMin.Y;
            minZ = modelMin.Z;
            maxX = modelMax.X;
            maxY = modelMax.Y;
            maxZ = modelMax.Z;
        }

        public void CreateCage()
        {
            aabb = new BoundingBox(modelMin, modelMax); 
        }

        //Przerysowanie BoudingBoxa, stosować do animacji. Pozwala na zmianę położenia boudingBoxa bez jego destrukcji
        public void RecreateCage(Vector3 vec)
        {
            //Przesunięcie oryginalnego położenia
            modelMin += vec;
            modelMax += vec;
            aabb = new BoundingBox(modelMin, modelMax);
        }
        
        //Przesuwa boudingBoxa o określony wektor
        public void MoveBoundingBox(Vector3 vector)
        {
            Matrix meshTransform = Matrix.CreateTranslation(vector);
            modelMin = Vector3.Transform(modelMin, meshTransform);
            modelMax = Vector3.Transform(modelMax, meshTransform);
        }

        //Globalny resize boudingBoxa, do wykorzystania tylko na początku
        public void BoudingBoxResizeOnce(float x, float y, float z)
        {
            modelMin = new Vector3(modelMin.X * x, modelMin.Y * y, modelMin.Z * z);
            modelMax = new Vector3(modelMax.X * x, modelMax.Y * y, modelMax.Z * z);
        }

        //Do wykorzystania w trakcie
        public void BoudingBoxResize(float x, float y, float z)
        {
            Vector3 newModelMin = new Vector3(modelMin.X * x, modelMin.Y * y, modelMin.Z * z);
            Vector3 newModelMax = new Vector3(modelMax.X * x, modelMax.Y * y, modelMax.Z * z);
            aabb = new BoundingBox(newModelMin, newModelMax);
        }

        public void BoudingBoxResize(Vector3 vec)
        {
            Vector3 newModelMin = new Vector3();
        }
        //Skalowanie boxa w górę i od góry w dół
        public void BouidingScaleZ(float factor)
        {
            modelMax = new Vector3(modelMax.X, modelMax.Y, modelMax.Z+factor);
        }

        //Rekonstrukcja BoudingBoxa w zależności od obrotu modelu.
        public void BoudingRotate(float angle)
        {
            
        }

        //Narysowanie obramowania BoudingBoxa
        public void DrawBouding(GraphicsDevice device, Matrix MatrixLocation, Matrix view, Matrix projection)
        {
            VertexPositionColor[] vpc = GetColoredCorners();
            BasicEffect boxEffect = new BasicEffect(device);
            boxEffect.World = MatrixLocation;
            boxEffect.View = view;
            boxEffect.Projection = projection;
            boxEffect.TextureEnabled = false;
            foreach (EffectPass pass in boxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList, vpc, 0, 8, bBoxIndices, 0, 12);
            }
        }

        //BasicEffect wykorzystywany do narysowania BoudingBoxa (w zależności od potrzeby)
        public BasicEffect GetBasicEffect(BasicEffect boxEffect, Matrix Location, Matrix view, Matrix projection)
        {
            boxEffect.World = Location;
            boxEffect.View = view;
            boxEffect.Projection = projection;
            boxEffect.TextureEnabled = false;
            return boxEffect;
        }

        public VertexPositionColor[] GetColoredCorners()
        {
            Vector3[] corners = aabb.GetCorners();
            VertexPositionColor[] vpc = new VertexPositionColor[corners.Length];
            for (int i = 0; i < corners.Length; i++)
            {
                vpc[i] = new VertexPositionColor(corners[i], Color.White);
            }
            return vpc;
        }
    }
}
