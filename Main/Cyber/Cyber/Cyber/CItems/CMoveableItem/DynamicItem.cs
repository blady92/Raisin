using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CItems
{
    class DynamicItem : _3DObjects
    {
        private string pathToModel;
        private string pathToAnimationClip;
        private SkinningAnimation skinnedModel;
        protected Vector3 position;
        private Collider collider;

        public DynamicItem(string path, string pathToAnimationClip, Vector3 position)
        {
            pathToAnimationClip = pathToAnimationClip;
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
        
        
        public void LoadItem(ContentManager thecContentManager)
        {
            skinnedModel.LoadContent_SkinnedModel(thecContentManager, pathToModel, pathToAnimationClip);
        }

        public void DrawItem(GameTime gameTime, GraphicsDevice device)
        {
            skinnedModel.DrawSkinnedModelWithShader(gameTime, device);
        }

        public void MoveItem(Vector3 vec)
        {
            position += vec;
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
