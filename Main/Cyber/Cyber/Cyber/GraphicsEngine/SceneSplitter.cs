using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Cyber.CItems.CStaticItem;
using Microsoft.Xna.Framework;

namespace Cyber.GraphicsEngine
{
    class SceneSplitter
    {
        private PointF pointMin;
        private PointF pointMax;

        private PointF defaultDistance;

        public SceneSplitter(StaticItem item, PointF defaultDistance)
        {
            this.defaultDistance = defaultDistance;
            SetSplitterSceneView(item);
        }

        public void SetSplitterSceneView(StaticItem item)
        {
            pointMin = new PointF(item.Position.X - defaultDistance.X, item.Position.Y - defaultDistance.Y);
            pointMax = new PointF(item.Position.X + defaultDistance.X, item.Position.Y + defaultDistance.Y);

        }
        public void SetSplitterSceneView(StaticItem item, float distanceX, float distanceY)
        {
            pointMin = new PointF(item.Position.X - distanceX, item.Position.Y - distanceY);
            pointMax = new PointF(item.Position.X + distanceX, item.Position.Y + distanceY);
        }

        public bool IsItemWithin(StaticItem item)
        {
            return (item.Position.X > pointMin.X &&
                item.Position.Y > pointMin.Y &&
                item.Position.X < pointMax.X &&
                item.Position.Y < pointMax.Y);

        }
    }
}
