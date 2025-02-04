﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Cyber
{
    public class BillboardSystem
    {
        // Vertex buffer and index buffer, particle
        // and index arrays
        VertexBuffer verts;
        IndexBuffer ints;
        VertexPositionTexture[] particles;
        public Vector3 positions { get; set; }
        public Vector3[] particlePositions { get; set; }
        int[] indices;

        // Billboard settings
        int nBillboards;
        Vector2 billboardSize;
        Texture2D texture;

        // GraphicsDevice and Effect
        GraphicsDevice graphicsDevice;
        Effect effect;

        public bool EnsureOcclusion = true;

        public enum BillboardMode { Cylindrical, Spherical };
        public BillboardMode Mode = BillboardMode.Spherical;

        public BillboardSystem(GraphicsDevice graphicsDevice,
            ContentManager content, Texture2D texture,
            Vector2 billboardSize, Vector3 position)
        {
            //this.nBillboards = particlePositions.Length;
            this.nBillboards = 1;
            this.billboardSize = billboardSize;
            this.graphicsDevice = graphicsDevice;
            this.texture = texture;
            Vector3[] particlePositions = { position };

            effect = content.Load<Effect>("Assets/ShadersFX/Billboard");

            generateParticles(particlePositions);
        }

        public void generateParticles(Vector3[] particlePositions)
        {
            particles = new VertexPositionTexture[nBillboards * 4];
            indices = new int[nBillboards * 6];

            int x = 0;

            for (int i = 0; i < nBillboards * 4; i += 4)
            {
                Vector3 pos = particlePositions[i / 4];

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
            verts = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture),
                nBillboards * 4, BufferUsage.WriteOnly);
            verts.SetData<VertexPositionTexture>(particles);

            // Create and set the index buffer
            ints = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits,
                nBillboards * 6, BufferUsage.WriteOnly);
            ints.SetData<int>(indices);
        }

        void setEffectParameters(Matrix View, Matrix Projection, Vector3 Up, Vector3 Right)
        {
            effect.Parameters["ParticleTexture"].SetValue(texture);
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["Size"].SetValue(billboardSize / 2f);
            effect.Parameters["Up"].SetValue(Mode == BillboardMode.Spherical ? Up : Vector3.Up);
            effect.Parameters["Side"].SetValue(Right);
        }

        public void Draw(GraphicsDevice device, Matrix View, Matrix Projection, float cameraRotation, 
            Vector3 translation, float resizeX, float resizeY, float resizeZ)
        {
            Matrix rotation = Matrix.CreateFromYawPitchRoll(0, -80, 0); ;
            rotation *= Matrix.CreateScale(resizeX, resizeY, resizeZ) 
                * Matrix.CreateRotationZ(MathHelper.ToRadians(-cameraRotation))
                * Matrix.CreateTranslation(translation);

            Vector3 up = Vector3.Transform(Vector3.Up, rotation);
            Vector3 right = Vector3.Cross(Vector3.Transform(Vector3.Forward, rotation), up);

            // Set the vertex and index buffer to the graphics card
            graphicsDevice.SetVertexBuffer(verts);
            graphicsDevice.Indices = ints;

            graphicsDevice.BlendState = BlendState.AlphaBlend;

            setEffectParameters(View, Projection, up, right);

            if (EnsureOcclusion)
            {
                drawOpaquePixels();
                drawTransparentPixels();
            }
            else
            {
                graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                effect.Parameters["AlphaTest"].SetValue(false);
                drawBillboards();
            }

            // Reset render states
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Un-set the vertex and index buffer
            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;
        }

        void drawOpaquePixels()
        {
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            effect.Parameters["AlphaTest"].SetValue(true);
            effect.Parameters["AlphaTestGreater"].SetValue(true);

            drawBillboards();
        }

        void drawTransparentPixels()
        {
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            effect.Parameters["AlphaTest"].SetValue(true);
            effect.Parameters["AlphaTestGreater"].SetValue(false);

            drawBillboards();
        }

        void drawBillboards()
        {
            effect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                4 * nBillboards, 0, nBillboards * 2);
        }
    }
}
