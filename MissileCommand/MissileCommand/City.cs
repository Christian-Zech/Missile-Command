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
    class City : Building
    {
        public Boolean alive;
        public Rectangle position;

        public City(Rectangle pos, Boolean a)
        {
            position = pos;
            alive = a;            
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
