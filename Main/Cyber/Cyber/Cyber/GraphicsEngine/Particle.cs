using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.GraphicsEngine
{
    class Particle
    {
        public float yMax;
        public float steps;
        public BillboardSystem bilboard { get; set; }
        private Vector3 starterPosition;
        private Vector3 movedPosition;

        public Particle(BillboardSystem bilboard, float yMax, float steps)
        {
            this.bilboard = bilboard;
            this.yMax = yMax;
            this.steps = steps;
            movedPosition = starterPosition = bilboard.positions;
        }
        
        public void Draw(GraphicsDevice device, Matrix view, Matrix projection, float cameraRotation, float scaleX, float scaleY, float scaleZ,
            Vector3 moveParticle)
        {
            //Debug.WriteLine("yMax: " + yMax + " movedposition" + movedPosition);
            movedPosition += moveParticle;
            bilboard.Draw(device, view, projection, cameraRotation, movedPosition, scaleX, scaleY, scaleZ);
        }

        public void Update()
        {
            if (-movedPosition.Z > yMax)
            {
                movedPosition = starterPosition;
            }
            //else
            //{
            //    movedPosition.Z += steps*4;
            //}
        }
    }
}
