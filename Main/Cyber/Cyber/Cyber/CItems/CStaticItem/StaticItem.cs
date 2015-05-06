using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CItems.CStaticItem
{
    class StaticItem
    {
        private string pathToModel;
        private SkinningAnimation skinnedModel;
        private Vector3 position;
        private Collider collider;

        public StaticItem(string path, Vector3 position)
        {
            this.pathToModel = path;
            this.position = position;
        }

        #region ACCESSORS
        public string PathToModel
        {
            get { return pathToModel; }
            set { pathToModel = value; }
        }

        public SkinningAnimation SkinnedModel
        {
            get { return skinnedModel; }
            set { skinnedModel = value; }
        }

        public Collider Collider
        {
            get { return collider; }
            set { collider = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion
        
        
        public void LoadItem()
        {
            
        }

        public void DrawItem(GameTime gameTime, GraphicsDevice device)
        {
            skinnedModel.DrawStaticModelWithBasicEffect(device);
        }

        public void FixCollider(Vector3 resize, Vector3 move)
        {
            collider = new Collider();
            collider.SetBoudings(skinnedModel.CurrentModel);
            collider.BoudingBoxResizeOnce(resize.X, resize.Y, resize.Z);
            collider.MoveBoundingBox(move);
        }
    }
}