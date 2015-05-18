using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.GraphicsEngine.Bilboarding
{
    internal class BilboardSystem
    {
        // Vertex buffer and index buffer, particle    
        // and index arrays    
        private VertexBuffer verts;
        private IndexBuffer ints;
        private VertexPositionTexture[] particles;
        private int[] indices;
        // Billboard settings    
        private int nBillboards;
        private Vector2 bilboardSize;
        private Texture2D texture;
        // GraphicsDevice and Effect    
        private GraphicsDevice graphicsDevice;
        private Effect effect;

        public BilboardSystem(GraphicsDevice graphicsDevice,
            ContentManager content,
            Texture2D texture, Vector2
                bilboardSize,
            Vector3[] particlePositions)
        {
            this.nBillboards = particlePositions.Length;
            this.bilboardSize = bilboardSize;
            this.graphicsDevice = graphicsDevice;
            this.texture = texture;
            effect = content.Load<Effect>("Assets/ShadersFX/BilboardEffect");

            generateParticles(particlePositions);
        }

        public void generateParticles(Vector3[] particlePositions)
        {
            // Create vertex and index arrays  
            particles = new VertexPositionTexture[nBillboards*4];
            indices = new int[nBillboards*6];
            int x = 0;
            // For each billboard...  
            for (int i = 0; i < nBillboards*4; i += 4)
            {
                Vector3 pos = particlePositions[i/4];
                // Add 4 vertices at the billboard's position     
                particles[i + 0] = new VertexPositionTexture(pos, new Vector2(0, 0));
                particles[i + 1] = new VertexPositionTexture(pos, new Vector2(0, 1));
                particles[i + 2] = new VertexPositionTexture(pos, new Vector2(1, 1));
                particles[i + 3] = new VertexPositionTexture(pos, new Vector2(1, 0));
                // Add 6 indices to form two triangles     
                indices[x++] = i + 0;
                indices[x++] = i + 3;
                indices[x++] = i + 2;
                indices[x++] = i + 2;
                indices[x++] = i + 1;
                indices[x++] = i + 0;
            }
            // Create and set the vertex buffer  
            verts = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), nBillboards * 4, BufferUsage.WriteOnly);  
            verts.SetData<VertexPositionTexture>(particles);
            // Create and set the index buffer  
            ints = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits, nBillboards * 6, BufferUsage.WriteOnly);  
            ints.SetData<int>(indices); 
        }

        public void setEffectParameters(Matrix View, Matrix Projection, Vector3 Up, Vector3 Right)
        {
            effect.Parameters["ParticleTexture"].SetValue(texture);
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["Size"].SetValue(bilboardSize/2f);
            effect.Parameters["Up"].SetValue(Up);
            effect.Parameters["Right"].SetValue(Right);

            effect.CurrentTechnique.Passes[0].Apply();
        }

        public void Draw(Matrix View, Matrix Projection, Vector3 Up, Vector3 Right)
        {
            graphicsDevice.SetVertexBuffer(verts);
            graphicsDevice.Indices = ints;

            setEffectParameters(View, Projection, Up, Right);

            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,0, 4*nBillboards, 0, nBillboards*2);
            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;
        }
    }
}
