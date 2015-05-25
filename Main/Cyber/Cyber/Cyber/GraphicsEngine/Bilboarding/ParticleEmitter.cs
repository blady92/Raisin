using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.GraphicsEngine
{
    class ParticleEmitter
    {
        private List<Particle> particles;
        private Random frequency;
        private Vector3 dispersion;
        private float lifeSpeed;

        public void LoadContent(GraphicsDevice device, ContentManager theContentManager, string path, 
            int density, 
            int dispersalX,
            int dispersalY,
            int dispersalZ,
            Vector3 startPosition, float yMax, float steps)
        {
            //ścieżka do obrazka, rozmiar, umiejscowienie
            frequency = new Random();
            particles = new List<Particle>();
            for (int i = 0; i < density; i++)
            {
                dispersion = new Vector3(
                    frequency.Next(0, dispersalX + 10),
                    frequency.Next(0, dispersalY + 10),
                    frequency.Next(0, dispersalZ + 10)
                    );
                BillboardSystem bilboard = new BillboardSystem(device, theContentManager,
                    theContentManager.Load<Texture2D>(path),
                    new Vector2(frequency.Next(20, 50)),
                    dispersion + startPosition);
                //float speed = (float) frequency.Next(2, 5)/100+ (float)frequency.NextDouble()/10;
                Particle particle = new Particle(bilboard, yMax, steps);
                particles.Add(particle);
            }
        }

        public void Draw(GraphicsDevice device, Matrix view, Matrix projection, float cameraRotation, Vector3 move)
        {
            Random random = new Random();
            foreach (Particle particle in particles)
            {
                float randomTranslation = (float) random.NextDouble()/1;
                move = new Vector3(0, 0, -randomTranslation);
                //randomTranslation = ;
                //float randomScale = (float) -random.Next(20,40)/10;
                particle.Draw(device, view, projection, cameraRotation, 
                    //(float)random.Next(20, 40) / 10,
                    1, (float)random.Next(1, 8) / 10, (float)-random.Next(1, 8) / 10,
                    //(float)random.Next(20, 40) / 10,
                    move);
            }
        }

        public void Update()
        {
            foreach (Particle particle in particles)
            {
                particle.Update();
            }
        }
    }
}
