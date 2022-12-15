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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 



    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont menuFont;
        Texture2D pixel, crosshairT;

        Rectangle crosshair, ground;
        Rectangle[] missileSites;
        Rectangle window;

        List<EnemyMissile> eMissiles;
        Airplane airplane;

        Boolean isMenu;

        KeyboardState oldkb;
        MouseState oldMouse;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            eMissiles = new List<EnemyMissile>();
            eMissiles.Add(new EnemyMissile(new Rectangle(10, 10, 8, 8), new Vector2(1, 2)));
            oldkb = Keyboard.GetState();
            oldMouse = Mouse.GetState();
            crosshair = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 7, GraphicsDevice.Viewport.Height / 2, 15, 15);
            ground = new Rectangle(0, GraphicsDevice.Viewport.Height - 15, GraphicsDevice.Viewport.Width, 15);
            missileSites = new Rectangle[3];
            missileSites[0] = new Rectangle(40, GraphicsDevice.Viewport.Height - 60, 70, 60);
            missileSites[1] = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 35, GraphicsDevice.Viewport.Height - 60, 70, 60);
            missileSites[2] = new Rectangle(GraphicsDevice.Viewport.Width - 110, GraphicsDevice.Viewport.Height - 60, 70, 60);
            isMenu = true;
            window = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            

            base.Initialize();

            airplane = new Airplane(new Rectangle(100, 200, 50, 50), new Vector2(2, 0), pixel);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            menuFont = this.Content.Load<SpriteFont>("MenuFont");
            pixel = this.Content.Load<Texture2D>("pixel");
            crosshairT = this.Content.Load<Texture2D>("crosshair-img");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            if (kb.IsKeyDown(Keys.Space))
                isMenu = false;

            if (!isMenu)
            {
                //if mouse is the crosshair
                //crosshair.X = mouse.X;
                //crosshair.Y = mouse.Y;




                // plane logic

                airplane.Update(window);
                if(airplane.firingMissile)
                {
                    // TODO: add pathing calculations for cities and missile sites (once those things are implemented)
                    eMissiles.Add(new EnemyMissile(new Rectangle(airplane.position.X, airplane.position.Y, 8, 8), new Vector2(1, 3)));


                    airplane.firingMissile = false;
                }




                //if keyboard moves the crosshair
                if (kb.IsKeyDown(Keys.W) && crosshair.Y > 0)
                {
                    crosshair.Y -= 5;
                }
                if (kb.IsKeyDown(Keys.S) && crosshair.Y < GraphicsDevice.Viewport.Height)
                {
                    crosshair.Y += 5;
                }
                if (kb.IsKeyDown(Keys.A) && crosshair.X > 0)
                {
                    crosshair.X -= 5;
                }
                if (kb.IsKeyDown(Keys.D) && crosshair.X < GraphicsDevice.Viewport.Width)
                {
                    crosshair.X += 5;
                }

                //moving the enemy missiles
                for (int i = 0; i < eMissiles.Count; i++)
                {
                    eMissiles[i].Move();

                    //checking for out of bounds
                    if (eMissiles[i].position.Intersects(missileSites[0]))
                    {
                        //missile site is hit
                        eMissiles[i].trail.Clear();
                        eMissiles.RemoveAt(i);
                    }
                    else if (eMissiles[i].position.X < 0 || eMissiles[i].position.X > GraphicsDevice.Viewport.Width)
                    {
                        //too far left or right
                        eMissiles[i].trail.Clear();
                        eMissiles.RemoveAt(i);
                    }
                    else if (eMissiles[i].position.Y > GraphicsDevice.Viewport.Height - 20)
                    {
                        //city is hit
                        eMissiles[i].trail.Clear();
                        eMissiles.RemoveAt(i);
                        
                    }
                }
            }


            oldkb = kb;
            oldMouse = mouse;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (isMenu)
                spriteBatch.DrawString(menuFont, "Welcome to Missile Command!\nPress SPACE to start!", new Vector2(150, 150), Color.White);

            if (!isMenu)
            {
                //ground
                spriteBatch.Draw(pixel, ground, null, Color.Yellow, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                //player missile sites
                spriteBatch.Draw(pixel, missileSites[0], null, Color.Yellow, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                spriteBatch.Draw(pixel, missileSites[1], null, Color.Yellow, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                spriteBatch.Draw(pixel, missileSites[2], null, Color.Yellow, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                for (int i = 0; i < eMissiles.Count; i++)
                {
                    spriteBatch.Draw(pixel, eMissiles[i].position, Color.Red); //draws the enemy missiles
                    for (int j = 0; j < eMissiles[i].trail.Count; j++)
                    {
                        //draws the missiles trail
                        spriteBatch.Draw(pixel, eMissiles[i].trail[j], Color.White);
                    }
                    
                }
                spriteBatch.Draw(crosshairT, crosshair, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                airplane.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


