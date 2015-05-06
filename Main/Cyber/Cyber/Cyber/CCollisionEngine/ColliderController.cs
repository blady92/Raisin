using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyber.CItems;
using Cyber.GraphicsEngine;

namespace Cyber.CollisionEngine
{
    class ColliderController
    {
        private Collider samantha;
        private Collider terminal;
        private List<Collider> enemies;
        private List<Collider> walls;
        private CollidedWith collideState;

        enum CollidedWith
        {
            terminal,
            wall,
            spy,
            flyer,
            tank
        }

        #region ACCESSORS
        public Collider Samantha
        {
            get { return samantha; }
            set { samantha = value; }
        }

        public List<Collider> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }

        public List<Collider> Walls
        {
            get { return walls; }
            set { walls = value; }
        }
        #endregion

        public bool WallCollision()
        {
            foreach (Collider wall in walls)
                if (wall.AABB.Intersects(Samantha.AABB))
                    return true;
            return false;
        }

        public bool EnemyCollision()
        {
            foreach (Collider enemy in enemies)
                if (enemy.AABB.Intersects(Samantha.AABB))
                {
                    collideState = CollidedWith.tank;
                    return true;
                }
            return false;
        }
    }
}
