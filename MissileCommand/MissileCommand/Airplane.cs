using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissileCommand
{
    class Airplane
    {

        public Texture2D texture;
        public Rectangle position;
        public Vector2 velocity;
        public Boolean firingMissile;
        public Boolean isActive;
        public Random rand;

        public Airplane(Texture2D texture)
        {
            position = Rectangle.Empty;
            velocity = Vector2.Zero;
            firingMissile = false;
            isActive = false;
            rand = new Random();
            this.texture = texture;
        }


        public void Update(Rectangle window)
        {

            if (isActive)
            {



                // makes airplane randomly move left and right unless it hits wall
                if (rand.Next(0, 150) == 5 || position.X < window.Left || position.X > window.Right - position.Width)
                    velocity.X *= -1;



                // moves plane horizontally
                position.X += (int)velocity.X;



                // randomly fires one missile
                if (rand.Next(0, 200) == 5 && !firingMissile)
                    fire();

            }
            else
            {

                if (rand.Next(0, 500) == 5)
                {
                    reset();
                    isActive = true;
                }
                    

            }



        }

        public void reset()
        {
            position = new Rectangle(0, 200, 50, 20);
            velocity = new Vector2(3, 0);
            firingMissile = false;
            isActive = false;
        }

        public void fire()
        {
            firingMissile = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(isActive)
                spriteBatch.Draw(texture, position, Color.Red);
            
        }

    }
}
