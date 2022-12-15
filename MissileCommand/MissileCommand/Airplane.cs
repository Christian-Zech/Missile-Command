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
        EnemyMissile planeMissile;
        public Rectangle position;
        public Vector2 velocity;
        public Boolean fireMissile;
        public Random rand;

        public Airplane(Rectangle position, Vector2 velocity, Texture2D texture)
        {
            this.position = position;
            this.velocity = velocity;
            fireMissile = false;
            rand = new Random();
            this.texture = texture;
        }


        public void Update()
        {
            position.X += (int)velocity.X;

            if(rand.Next(0, 10) == 5 && !fireMissile)
            {
                fireMissile = true;
                fire();
            }

            if (fireMissile)
                planeMissile.Move();

        }

        public void fire()
        {
            fireMissile = false;
            planeMissile = new EnemyMissile(position, new Vector2(2, 3), false);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Update();
            spriteBatch.Draw(texture, position, Color.White);
            planeMissile.Draw(spriteBatch, texture);
        }

    }
}
