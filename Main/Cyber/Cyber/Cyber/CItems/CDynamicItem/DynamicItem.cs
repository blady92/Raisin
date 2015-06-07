using Cyber.CItems.CDynamicItem;
using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Cyber.CItems
{
    public class DynamicItem : _3DObjects
    {
        private string pathToModel;
        public string animationClipName { get; set; }
        private SkinningAnimation skinnedModel;
        protected Vector3 position;
        private Collider colliderInternal;
        private Collider colliderExternal;
        private float rotation;
        public DynamicItemType Type { get; set; }
        public string ID { get; set; }


        public DynamicItem(string path, string animationClipNamePassed, Vector3 position)
        {
            this.skinnedModel = new SkinningAnimation();
            this.animationClipName = animationClipNamePassed;
            this.colliderExternal = new Collider();
            this.colliderInternal = new Collider();
            this.pathToModel = path;
            this.position = position;
        }

        public DynamicItem(string path, Vector3 position)
        {
            rotation = 0;
            skinnedModel = new SkinningAnimation();
            this.colliderExternal = new Collider();
            this.colliderInternal = new Collider();
            this.pathToModel = path;
            this.position = position;
        }

        public DynamicItem(string path)
        {
            rotation = 0;
            skinnedModel = new SkinningAnimation();
            this.colliderExternal = new Collider();
            this.colliderInternal = new Collider();
            this.pathToModel = path;
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

        #endregion
        
        
        public void LoadItem(ContentManager thecContentManager)
        {
            skinnedModel.LoadContent_SkinnedModel(thecContentManager, pathToModel, animationClipName);
        }

        public void DrawItem(GameTime gameTime, GraphicsDevice device, Matrix world, Matrix view, Matrix projection)
        {
            skinnedModel.DrawSkinnedModelWithSkinnedEffect(gameTime, device, world, view, projection);
        }

        public void DrawItem(GraphicsDevice device, Matrix world, Matrix view, Matrix projection, Effect celShader)
        {
            skinnedModel.DrawSkinnedModelWithShader(device, world, view, projection, celShader);
        }

        public void MoveItem(Vector3 vec)
        {
            position += vec;
        }

        public void FixColliderExternal(Vector3 resize, Vector3 move)
        {
            colliderExternal.SetBoudings(skinnedModel.CurrentModel);
            colliderExternal.CreateColliderBoudingBox();
            colliderExternal.BoudingBoxResizeOnce(resize.X, resize.Y, resize.Z);
            colliderExternal.MoveBoundingBox(move);
            colliderExternal.RecreateCage(position);
        }

        public void FixColliderInternal(Vector3 resize, Vector3 move)
        {
            colliderInternal.SetBoudings(skinnedModel.CurrentModel);
            colliderInternal.CreateColliderBoudingBox();
            colliderInternal.BoudingBoxResizeOnce(resize.X, resize.Y, resize.Z);
            colliderInternal.MoveBoundingBox(move);
            colliderInternal.RecreateCage(position);
        }
    }
}
