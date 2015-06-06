using System;
using Cyber.CItems.CStaticItem;
using Cyber.CollisionEngine;
using Microsoft.Xna.Framework;

namespace Cyber.CStageParsing
{
    public class GateHolder
    {
        public GateHolder(Gate gate)
        {
            FirstItem = new GateBounding(gate.Height, gate.Width);
            SecondItem = new Gate(gate.Height, gate.Width);
            Orientation = GetOrientation(gate);
            IsActive = true;
        }
        public Orientation Orientation { get; set; }
        public StageObject FirstItem { get; set; }
        public StageObject SecondItem { get; set; }
        public Collider Collider { get; set; }
        public bool IsActive { get; private set; }

        public void SetUpCollider(StaticItem template)
        {
            Collider = new Collider();
            Collider.SetBoudings(template.SkinnedModel.CurrentModel);
            Collider.CreateColliderBoudingBox();
            Pair<int, int> firstPosition = FirstItem.GetBlock();
            Pair<int, int> secondPosition = SecondItem.GetBlock();
            Collider.BoudingBoxResizeOnce(secondPosition.X - firstPosition.X, secondPosition.Y - firstPosition.Y, 1);
            Collider.MoveBoundingBox(new Vector3(firstPosition.X, firstPosition.Y, 0.0f));
            Collider.RecreateCage(new Vector3(firstPosition.X, firstPosition.Y, 0.0f));
            if (Orientation.Equals(Orientation.Vertical))
            {
                Collider.Position = new Vector3(firstPosition.X*19.5f, (firstPosition.Y - 2)*19.5f, 0.0f);
            }
            else
            {
                Collider.Position = new Vector3((firstPosition.X - 2) * 19.5f, firstPosition.Y * 19.5f, 0.0f);
            }
        }

        private Orientation GetOrientation(GenerableStructureImplementation gate)
        {
            Orientation orientation = Orientation.Horizontal;
            bool itemSet = false;
            int lasti = 0, lastj = 0;
            for (var i = 0; i < gate.Height; i++)
            {
                for (var j = 0; j < gate.Width; j++)
                {
                    if (gate.Structure[j, i] != true) continue;
                    if (!itemSet)
                    {
                        FirstItem.Structure[j, i] = true;
                        try
                        {
                            if (gate.Structure[j, i + 1])
                            {
                                orientation = Orientation.Vertical;
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                    lasti = i;
                    lastj = j;
                    itemSet = true;
                }
            }
            SecondItem.Structure[lastj, lasti] = true;
            return orientation;
        }

        public void Open()
        {
            IsActive = false;
            Collider = null;
        }
    }

    public enum Orientation
    {
        Horizontal, Vertical
    }
}