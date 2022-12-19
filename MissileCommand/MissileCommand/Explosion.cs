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
        int temp;
        public Explosion(Rectangle pos)
        {
            rect = pos;
            done = false;
            temp = 0;
        }
        public void Explode()
        {
            if (!done)
            {
                if (temp == 0)
                {
                    rect.Width++;
                    rect.Height++;
                    if (rect.Width > 30)
                    {
                        temp = 1;
                    }
                }
                if (temp == 1)
                {
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
