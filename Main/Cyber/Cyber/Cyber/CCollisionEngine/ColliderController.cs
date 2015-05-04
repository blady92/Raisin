using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyber.GraphicsEngine;

namespace Cyber.CollisionEngine
{
    class ColliderController
    {
        private Collider samantha;
        private List<Collider> enemies;
        private List<Collider> walls;

        enum CollideWith
        {
            walls,
            enemy,
            terminal
        }

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
                    return true;
            return false;
        }
    }
}
