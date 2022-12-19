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
    class Site : Building
    {
        public Boolean alive;
        public Rectangle position;
        public List<PlayerMissiles> missiles;

        public Site(Rectangle pos, Boolean a)
        {
            position = pos;
            alive = a;
            missiles = new List<PlayerMissiles>();
            for (int i = 0; i < 10; i++)
            {
                missiles.Add(new PlayerMissiles(new Rectangle(pos.X+pos.Width/2, pos.Y+pos.Height/2, 8, 8), false));
            }
        }
        public override bool CheckHit(Rectangle missile)
        {
            if (position.Intersects(missile))
            {
                return true;
            }
            return false;
        }
        public override void Destroy()
        {
            position.Height /= 3;
        }
    }
}
