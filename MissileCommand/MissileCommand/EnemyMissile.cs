using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MissileCommand
{
    class EnemyMissile
    {
        public Rectangle position;
        public Boolean isHit;
        public List<Rectangle> trail;
        public Vector2 velocity;
        public EnemyMissile(Rectangle pos, Vector2 vel)
        {
            position = pos;
            velocity = vel;
            isHit = false;
            trail = new List<Rectangle>();
        }
        public void Move()
        {
            position.X += (int)velocity.X;
            position.Y += (int)velocity.Y;
            trail.Add(new Rectangle((int)position.X, (int)position.Y, 2, 2));
        }


    }
}
