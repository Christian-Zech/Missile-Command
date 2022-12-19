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
        Rectangle window;
        List<Rectangle> markers;
        Site[] missileSites;
        List<Explosion> explosions;

        List<EnemyMissile> eMissiles;
        Airplane airplane;

        KeyboardState oldkb;
        MouseState oldMouse;

        int score;
        SpriteFont scoreFont;

        enum GameState
        {
           menu,
           play,
           end
        }

        GameState gameState;


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
            markers = new List<Rectangle>();
            explosions = new List<Explosion>();
            eMissiles = new List<EnemyMissile>();
            eMissiles.Add(new EnemyMissile(new Rectangle(10, 10, 8, 8), new Vector2(1, 2), false));
            oldkb = Keyboard.GetState();
            oldMouse = Mouse.GetState();
            crosshair = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 7, GraphicsDevice.Viewport.Height / 2, 15, 15);
            ground = new Rectangle(0, GraphicsDevice.Viewport.Height - 15, GraphicsDevice.Viewport.Width, 15);
            missileSites = new Site[3];
            missileSites[0] = new Site(new Rectangle(40, GraphicsDevice.Viewport.Height - 60, 70, 60), true);
            missileSites[1] = new Site(new Rectangle((GraphicsDevice.Viewport.Width / 2) - 35, GraphicsDevice.Viewport.Height - 60, 70, 60), true);
            missileSites[2] = new Site(new Rectangle(GraphicsDevice.Viewport.Width - 110, GraphicsDevice.Viewport.Height - 60, 70, 60), true);
            
            window = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            gameState = GameState.menu;

            score=0;
            base.Initialize();

            airplane = new Airplane(pixel);
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
            scoreFont=Content.Load<SpriteFont>("MenuFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        //comment hi
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public Boolean allEnemiesHit()
        {
            for (int i = 0; i < eMissiles.Count; i++)
            {
                Boolean hitAll = eMissiles[i].isHit;
                if (!eMissiles[i].isHit)
                    return false;
            }
            return true;
        }
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            if (kb.IsKeyDown(Keys.Space) && gameState == GameState.menu)
                gameState++;

            if (gameState == GameState.play)
            {


                // airplane logic

                airplane.Update(window);
                if(airplane.firingMissile)
                {
                    // TODO: add pathing calculations for cities and missile sites (once those things are implemented)
                    eMissiles.Add(new EnemyMissile(new Rectangle(airplane.position.X, airplane.position.Y, 8, 8), new Vector2(1, 3), false));


                    airplane.firingMissile = false;
                }


                //if mouse is the crosshair
                //crosshair.X = mouse.X;
                //crosshair.Y = mouse.Y;

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
                    if (missileSites[0].CheckHit(eMissiles[i].position))
                    {
                        //missile site is hit
                        missileSites[0].Destroy();
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

                //shooting the missiles
                if (kb.IsKeyDown(Keys.Space) && oldkb.IsKeyUp(Keys.Space))
                {
                    if (crosshair.X < GraphicsDevice.Viewport.Width / 3)
                    {
                        if (missileSites[0].missiles.Count != 0 && missileSites[0].missiles[0].velocity.Y == 0)
                        {
                            missileSites[0].missiles[0].Calculate(crosshair, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                            markers.Add(new Rectangle(crosshair.X, crosshair.Y, 10, 10));
                        }
                        
                    }
                    else if (crosshair.X > GraphicsDevice.Viewport.Width / 3 && crosshair.X < 2*GraphicsDevice.Viewport.Width / 3)
                    {
                        if (missileSites[1].missiles.Count != 0 && missileSites[1].missiles[0].velocity.Y == 0)
                        {
                            missileSites[1].missiles[0].Calculate(crosshair, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                            markers.Add(new Rectangle(crosshair.X, crosshair.Y, 10, 10));
                        }
                    }
                    else
                    {
                        if (missileSites[2].missiles.Count != 0 && missileSites[2].missiles[0].velocity.Y == 0)
                        {
                            missileSites[2].missiles[0].Calculate(crosshair, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                            markers.Add(new Rectangle(crosshair.X, crosshair.Y, 10, 10));
                        }
                    }
                }
                for (int i = 0; i < missileSites[0].missiles.Count; i++)
                {
                    if (missileSites[0].missiles[i].velocity.Y != 0)
                    {
                        
                        if (missileSites[0].missiles[i].positionReached())
                        {
                            missileSites[0].missiles[i].Explode();
                        }
                        else
                        {
                            missileSites[0].missiles[i].Move();
                        }
                        
                    }
                    if (missileSites[0].missiles[i].exploded)
                    {
                        missileSites[0].missiles.RemoveAt(i);
                        markers.RemoveAt(0);
                    }
                }

                for (int i = 0; i < missileSites[1].missiles.Count; i++)
                {
                    if (missileSites[1].missiles[i].velocity.Y != 0)
                    {

                        if (missileSites[1].missiles[i].positionReached())
                        {
                            missileSites[1].missiles[i].Explode();
                            
                        }
                        else
                        {
                            missileSites[1].missiles[i].Move();
                        }

                    }
                    if (missileSites[1].missiles[i].exploded)
                    {
                        missileSites[1].missiles.RemoveAt(i);
                        markers.RemoveAt(0);
                    }
                }

                for (int i = 0; i < missileSites[2].missiles.Count; i++)
                {
                    if (missileSites[2].missiles[i].velocity.Y != 0)
                    {

                        if (missileSites[2].missiles[i].positionReached())
                        {
                            missileSites[2].missiles[i].Explode();
                        }
                        else
                        {
                            missileSites[2].missiles[i].Move();
                        }

                    }
                    if (missileSites[2].missiles[i].exploded)
                    {
                        missileSites[2].missiles.RemoveAt(i);
                        markers.RemoveAt(0);
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

            if (gameState == GameState.menu)
                spriteBatch.DrawString(menuFont, "Welcome to Missile Command!\nPress SPACE to start!", new Vector2(150, 150), Color.White);

            if (gameState == GameState.play)
            {
                //ground
                spriteBatch.Draw(pixel, ground, null, Color.Yellow, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                //player missile sites
                spriteBatch.Draw(pixel, missileSites[0].position, null, Color.Yellow, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                spriteBatch.Draw(pixel, missileSites[1].position, null, Color.Yellow, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                spriteBatch.Draw(pixel, missileSites[2].position, null, Color.Yellow, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                for (int i = 0; i < missileSites[0].missiles.Count; i++)
                {
                    spriteBatch.Draw(pixel, missileSites[0].missiles[i].position, Color.White);
                }

                for (int i = 0; i < missileSites[1].missiles.Count; i++)
                {
                    spriteBatch.Draw(pixel, missileSites[1].missiles[i].position, Color.White);
                }

                for (int i = 0; i < missileSites[2].missiles.Count; i++)
                {
                    spriteBatch.Draw(pixel, missileSites[2].missiles[i].position, Color.White);
                }

                for (int i = 0; i < markers.Count; i++)
                {
                    spriteBatch.Draw(crosshairT, markers[i], Color.Red);
                }

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
                //score
                spriteBatch.DrawString(scoreFont,""+score,new Vector2(350,0),Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


