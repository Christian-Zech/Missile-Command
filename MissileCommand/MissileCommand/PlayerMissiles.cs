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
    class PlayerMissiles
    {
        public Rectangle position;
        public Boolean exploded;
        public Vector2 velocity;
        public Vector2 finalPos; //where it is set to explode
        int temp; //used for explosion
        public PlayerMissiles(Rectangle pos, Boolean exp)
        {
            position = pos;
            exploded = exp;
            temp = 0;
            velocity = new Vector2(0, 0);
        }
        public void Move()
        {
            position.X += (int)velocity.X;
            position.Y += (int)velocity.Y;
        }
        public void Calculate(Rectangle pos, int w, int h)
        {
            int screenW = w;
            int screenH = h;

            finalPos = new Vector2(pos.X, pos.Y) ;
            
            int x = (int)pos.X+pos.Width/2 - position.X+position.Width/2;
            int y = (int)pos.Y+pos.Height/2 - position.Y + position.Height / 2;
            double temp = Math.Atan2(y, x);
            
            
            int dx = (int)pos.X+pos.Width / 2 - position.X + position.Width / 2;
            int dy = (int)pos.Y+pos.Height / 2 - position.Y + position.Height / 2;
            int hyp = (int)Math.Sqrt((double)(dx * dx) + (double)(dy * dy));
            temp = hyp / 5;
            dx = (int)(dx/temp);
            dy = (int)(dy/temp);

            velocity = new Vector2(dx, dy);
            Console.WriteLine(velocity);
        }
        public bool positionReached()
        {
            if (position.Y < finalPos.Y)
            {
                return true;
            }
            return false;
        }
        public void Explode()
        {
            if (!exploded)
            {
                if (temp == 0)
                {
                    position.Width++;
                    position.Height++;
                    if (position.Width > 30)
                    {
                        temp = 1;
                    }
                }
                if (temp == 1)
                {
                    position.Width--;
                    position.Height--;
                    if (position.Width < 5)
                    {
                        exploded = true;
                    }
                }
            }
        }
    }
}
