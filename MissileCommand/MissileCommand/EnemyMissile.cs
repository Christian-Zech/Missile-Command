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
        public List<Rectangle> splitR;
        public List<Vector2> splitV;
        public Boolean isHit;
        public List<Rectangle> trail;
        public Vector2 velocity;
        public Boolean splitBool;
        public Boolean hasSplit;
        int splitInt;
        Random rnd = new Random();
        public EnemyMissile(Rectangle pos, Vector2 vel, Boolean hit)
        {
            position = pos;
            velocity = vel;
            isHit = hit;
            hasSplit = false;
            splitInt = rnd.Next(2);
            if (splitInt == 0)
            {
                splitBool = true;
                splitInt = rnd.Next(2, 5);
            }
            splitR = new List<Rectangle>();
            trail = new List<Rectangle>();
            splitV = new List<Vector2>();
            Console.WriteLine(splitBool);
        }
        public void Move()
        {
            
            if (splitBool && hasSplit)
            {
                for (int i = 0; i < splitR.Count; i++)
                {
                    Rectangle temp = new Rectangle(splitR[i].X, splitR[i].Y, splitR[i].Width, splitR[i].Height);
                    temp.X += (int)splitV[i].X;
                    temp.Y += (int)splitV[i].Y;
                    splitR[i] = temp;
                    trail.Add(new Rectangle((int)splitR[i].X, (int)splitR[i].Y, 2, 2));
                }
            }
            
            position.X += (int)velocity.X;
            position.Y += (int)velocity.Y;
            trail.Add(new Rectangle((int)position.X, (int)position.Y, 2, 2));
            if (splitBool && !hasSplit && position.Y > 90)
            {
                Split();
            }
        }
        public void Split()
        {
            if (splitBool)
            {
                for (int i = 0; i < splitInt; i++)
                {
                    splitR.Add(position);
                    splitV.Add(new Vector2(rnd.Next(-1, 2), velocity.Y));
                }
                hasSplit = true;
            }
            
        }

    }
}
