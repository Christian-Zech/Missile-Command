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
using System.Text;

namespace MissileCommand
{
    abstract class Building
    {
        /*this is all not finished do not expect this to run or serve any purpose right now*/ 
        
        
        public Boolean isHit;
        public Vector2 locations;

        public abstract void Slots();
        public void slot1()
        {
            Vector2 locations = new Vector2(100, 400);
        }

        public abstract void Hit();
        
    }
}
