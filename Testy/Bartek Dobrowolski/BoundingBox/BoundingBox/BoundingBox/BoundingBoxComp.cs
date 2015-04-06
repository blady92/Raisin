using BoundingBox;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoundingBoxTest
{
    class BoundingBoxComp : DrawableGameComponent
    {
        private readonly string modelAssetName;
        private BoundingBoxBufferComp buffer;
        private BasicEffect effect;

        public BoundingBoxComp(Game game, string modelAssetName) : base(game)
        {
            this.modelAssetName = modelAssetName;
        }

        protected override void LoadContent()
        {
            Model model = Game.Content.Load<Model>(modelAssetName);
            base.LoadContent();
        }
    }
}
