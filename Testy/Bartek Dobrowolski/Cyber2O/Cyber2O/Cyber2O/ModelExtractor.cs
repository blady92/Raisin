using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber2O
{
    class ModelExtractor
    {
        private ModelMeshPart mmpModel;
        private Vector3[] arrVectors;
        private VertexPositionColor[] vpcVertices;

        public VertexPositionColor[] VpcVertices
        {
            get { return vpcVertices; }
            set { vpcVertices = value; }
        }

        public ModelExtractor(ModelMeshPart part, Vector3[] vector3s, VertexPositionColor[] vertexPositionColors)
        {
            mmpModel = part;
            arrVectors = vector3s;
            vpcVertices = vertexPositionColors;
        }

        public void ExtractVertices()
        {
            this.mmpModel.VertexBuffer.GetData<Vector3>(this.arrVectors);
            for (int a = 0; a < vpcVertices.Length; a += 2)
            {
                this.vpcVertices[a].Position.X = arrVectors[a].X;
                Debug.WriteLine(arrVectors[a].X);
                this.vpcVertices[a].Position.Y = arrVectors[a].Y;
                Debug.WriteLine(arrVectors[a].Y);
                this.vpcVertices[a].Position.Z = arrVectors[a].Z;
                Debug.WriteLine(arrVectors[a].Z);
            }
        }
    }
}
