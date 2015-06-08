using System.Collections.Generic;
using System.Diagnostics;
using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CItems.CStaticItem
{
    public class StaticItem : _3DObjects
    {
        private string pathToModel;
        public SkinningAnimation skinnedModel { get; set; }
        private Collider colliderInternal;
        private Collider colliderExternal;
        public Vector2 moveColliderExternal { get; set; }

        private Vector3 position;
        private float rotation;
        private StaticItemType type;
        public bool DrawBouding { get; set; }
        public bool OnOffBilboard { get; set; }
        public bool DrawID { get; set; }
        public BillboardSystem bilboards { get; set; }
        public Vector3 BilboardHeight { get; set; }
        public BillboardSystem MachineID { get; set; }
        public Vector3 MachineIDHeight { get; set; }
        public ParticleEmitter particles { get; set; }

        public string ID { get; set; }
        public StaticItem(string path)
        {
            rotation = 0;
            moveColliderExternal = new Vector2(0, 0);
            skinnedModel = new SkinningAnimation();
            this.colliderExternal = new Collider();
            this.colliderInternal = new Collider();
            this.pathToModel = path;
        }

        public StaticItem(string path, Vector3 position)
        {
            rotation = 0;
            moveColliderExternal = new Vector2(0, 0);
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
            if (OnOffBilboard)
            {
                bilboards.positions = position;
                bilboards.Draw(device, view, projection, cameraRotation, new Vector3(0, 0, 0), 1, 0.5f, 1);
                bilboards.generateParticles(new Vector3[] { Position + BilboardHeight });
            }
            if (DrawID && !OnOffBilboard)
            {
                MachineID.positions = position;
                MachineID.Draw(device, view, projection, cameraRotation, new Vector3(0, 0, 0), 1, 1, 1);
                MachineID.generateParticles(new Vector3[] { Position + MachineIDHeight });
            }
            skinnedModel.DrawStaticModelWithBasicEffect(device, world, view, projection);
        }

        public void DrawItem(GraphicsDevice device, Matrix world, Matrix view, Matrix projection, Effect celShader)
        {
            skinnedModel.DrawStaticModelWithShader(device, world, view, projection, celShader);
        }

        public void DrawItem(GameTime gameTime, GraphicsDevice device, Matrix world, Matrix view, Matrix projection, float cameraRotation, Effect celShader)
        {
            if (OnOffBilboard)
            {
                bilboards.positions = position;
                bilboards.Draw(device, view, projection, cameraRotation, new Vector3(0, 0, 0), 1, 0.5f, 1);
                bilboards.generateParticles(new Vector3[] { Position + BilboardHeight });
            }
            if (DrawID && !OnOffBilboard)
            {
                MachineID.positions = position;
                MachineID.Draw(device, view, projection, cameraRotation, new Vector3(0, 0, 0), 1, 1, 1);
                MachineID.generateParticles(new Vector3[] { Position + MachineIDHeight });
            }
            skinnedModel.DrawStaticModelWithShader(device, world, view, projection, celShader);
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

        public void ApplyIDBilboard(GraphicsDevice device, ContentManager theContentManager, Vector3 position)
        {
            MachineID = new BillboardSystem(device, theContentManager, theContentManager.Load<Texture2D>("Assets/2D/IDs/" + ID), new Vector2(100), position + new Vector3(0, 0, 30));
        }
    }
}