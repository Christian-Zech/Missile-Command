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
    class Explosion
    {
        public Rectangle rect;
        public Boolean done;
        int temp, i;
        public Explosion(Rectangle pos)
        {
            rect = pos;
            done = false;
            temp = 0;
            i = 0;
            Explode();
        }
        public void Explode()
        {
            if (!done)
            {
                i++;
                if (temp == 0)
                {
                    if (i % 2 == 0){
                        rect.X--;
                        rect.Y--;
                    }
                    
                    rect.Width++;
                    rect.Height++;
                    if (rect.Width > 35)
                    {
                        temp = 1;
                    }
                }
                if (temp == 1)
                {
                    if (i % 2 == 0)
                    {
                        rect.X++;
                        rect.Y++;
                    }
                    rect.Width--;
                    rect.Height--;
                    if (rect.Width < 5)
                    {
                        done = true;
                    }
                }
            }
        }
    }
}
