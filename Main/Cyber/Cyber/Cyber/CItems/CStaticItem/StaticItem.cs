using System.Collections.Generic;
using System.Diagnostics;
using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CItems.CStaticItem
{
    class StaticItem : _3DObjects
    {
        private string pathToModel;
        private SkinningAnimation skinnedModel;
        private Collider colliderInternal;
        private Collider colliderExternal;
        private Vector3 position;
        private float rotation;
        private StaticItemType type;
        public bool OnOffBilboard { get; set; }
        public List<BillboardSystem> bilboards { get; set; }
        public ParticleEmitter particles { get; set; }

        public string ID { get; set; }
        public StaticItem(string path)
        {
            rotation = 0;
            skinnedModel = new SkinningAnimation();
            this.colliderExternal = new Collider();
            this.colliderInternal = new Collider();
            this.pathToModel = path;
        }

        public StaticItem(string path, Vector3 position)
        {
            rotation = 0;
            this.skinnedModel = new SkinningAnimation();
            this.colliderExternal = new Collider();
            this.colliderInternal = new Collider();
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

        public StaticItemType Type
        {
            get { return type; }
            set { type = value; }
        }

        #endregion
        
        public void LoadItem(ContentManager theContentManager)
        {
            skinnedModel.LoadContent_StaticModel(theContentManager, pathToModel);
        }

        public void DrawItem(GraphicsDevice device, Matrix world, Matrix view, Matrix projection)
        {
            skinnedModel.DrawStaticModelWithBasicEffect(device, world, view, projection);
        }
        public void DrawItem(GraphicsDevice device, Matrix world, Matrix view, Matrix projection, float cameraRotation)
        {
            skinnedModel.DrawStaticModelWithBasicEffect(device, world, view, projection);
            if (OnOffBilboard)
            {
                foreach (BillboardSystem bilboard in bilboards)
                {
                    bilboard.Draw(device, view, projection, cameraRotation, new Vector3(0, 0, 0), 1, 0.5f, 1);
                }   
            }
        }

        public void DrawItem(GameTime gameTime, GraphicsDevice device)
        {
            skinnedModel.DrawStaticModelWithShader(gameTime, device);
        }
        public void DrawItem(GameTime gameTime, GraphicsDevice device, Matrix world, Matrix view, Matrix projection, float cameraRotation)
        {
            skinnedModel.DrawStaticModelWithShader(gameTime, device);
            if (OnOffBilboard)
            {
                foreach (BillboardSystem bilboard in bilboards)
                {
                    bilboard.Draw(device, view, projection, cameraRotation, new Vector3(0, 0, 0), 1, 0.5f, 1);
                }
            }
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