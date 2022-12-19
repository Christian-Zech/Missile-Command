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
        Rectangle window;


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
            this.window = window;

            if (isActive)
            {


                if(position.X < window.Left - position.Width || position.X > window.Right)
                    isActive = false;

                
                // moves plane horizontally
                position.X += (int)velocity.X;



                // randomly fires one missile
                if (rand.Next(0, 200) == 5 && !firingMissile)
                    fire();

            }
            else
            {
                // resets plane at random time if it isn't already active
                if (rand.Next(0, 500) == 5)
                {
                    reset();
                    isActive = true;
                }
                    

            }



        }

        public void reset()
        {
            position = new Rectangle(0, rand.Next(100,250), 50, 20);

            // randomly decides what location to spawn the airplane
            if(rand.Next(0,2) == 0) {
                velocity = new Vector2(-1,0);
                position.X = window.Right - 15;
            }
            else
                velocity = new Vector2(1,0);
            
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
