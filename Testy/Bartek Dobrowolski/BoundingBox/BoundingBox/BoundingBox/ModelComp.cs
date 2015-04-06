using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoundingBox
{
    class ModelComp : DrawableGameComponent
    {
        private readonly string assetName;
        private Model model;
        private Microsoft.Xna.Framework.BoundingBox aabb;

        //Przygotowanie modelu do załadowania
        public ModelComp(Game game, string assetPath) : base(game)
        {
            assetName = assetPath;
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>(assetName);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            ICamera cameraService = (ICamera)Game.Services.GetService(typeof(ICamera));
            model.Draw(Matrix.Identity, cameraService.View, cameraService.Projection);
            base.Draw(gameTime);
        }
    }
}
