using Cyber.CItems.CDynamicItem;
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
        private Collider colliderInternal;
        private Collider colliderExternal;
        private float rotation;
        private DynamicItemType type;
        public string ID { get; set; }


        public DynamicItem(string path, string pathToAnimationClip, Vector3 position)
        {
            this.pathToAnimationClip = pathToAnimationClip;
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

        public Collider ColliderExternal
        {
            get { return colliderExternal; }
            set { colliderExternal = value; }
        }

        public Collider ColliderInternal
        {
            get { return colliderInternal; }
            set { colliderInternal = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public DynamicItemType Type
        {
            get { return type; }
            set { type = value; }
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
            colliderExternal = new Collider();
            colliderExternal.SetBoudings(skinnedModel.CurrentModel);
            colliderExternal.BoudingBoxResizeOnce(resize.X, resize.Y, resize.Z);
            colliderExternal.MoveBoundingBox(move);
        }
    }
}
