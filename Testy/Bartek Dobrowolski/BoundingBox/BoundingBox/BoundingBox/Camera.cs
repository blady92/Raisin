using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BoundingBox
{
    public class Camera : DrawableGameComponent, ICamera
    {
        public Camera(Game game) : base(game)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            View = Matrix.CreateLookAt(new Vector3(200, 100, 200), new Vector3(0, 0, 0), Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio,
                1.0f, 400.0f);

            base.Update(gameTime);
        }

        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }
    }
}
