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
        public Random rand;

        public Airplane(Rectangle position, Vector2 velocity, Texture2D texture)
        {
            this.position = position;
            this.velocity = velocity;
            firingMissile = false;
            rand = new Random();
            this.texture = texture;
        }


        public void Update()
        {
            // moves plane horizontally
            position.X += (int)velocity.X;


            // randomly fires one missile
            if(rand.Next(0, 100) == 5 && !firingMissile)
                fire();


        }

        public void fire()
        {
            firingMissile = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Update();
            spriteBatch.Draw(texture, position, Color.White);
            
        }

    }
}
