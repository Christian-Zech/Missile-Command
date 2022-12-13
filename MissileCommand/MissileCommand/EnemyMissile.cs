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
        public Vector2 position;
        public Boolean isHit;
        public float angle;
        public Vector2 velocity;
        public EnemyMissile(Vector2 pos, Vector2 vel, Boolean hit, float ang)
        {
            position = pos;
            velocity = vel;
            isHit = hit;
            angle = ang;
        }
        public void Move()
        {
            
        }
        
    }
}
