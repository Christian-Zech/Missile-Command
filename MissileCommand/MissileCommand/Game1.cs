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
        City[] cities;
        List<PlayerMissiles> movingMissiles;
        List<Explosion> explosions;

        List<EnemyMissile> eMissiles;
        Airplane airplane;

        KeyboardState oldkb;
        MouseState oldMouse;

        int score, round, timer;
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
            timer = 0;
            movingMissiles = new List<PlayerMissiles>();
            markers = new List<Rectangle>();
            explosions = new List<Explosion>();
            eMissiles = new List<EnemyMissile>();
            oldkb = Keyboard.GetState();
            oldMouse = Mouse.GetState();
            crosshair = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 7, GraphicsDevice.Viewport.Height / 2, 15, 15);
            ground = new Rectangle(0, GraphicsDevice.Viewport.Height - 15, GraphicsDevice.Viewport.Width, 15);
            missileSites = new Site[3];
            cities = new City[6];
            missileSites[0] = new Site(new Rectangle(40, GraphicsDevice.Viewport.Height - 60, 70, 60), true);
            missileSites[1] = new Site(new Rectangle((GraphicsDevice.Viewport.Width / 2) - 35, GraphicsDevice.Viewport.Height - 60, 70, 60), true);
            missileSites[2] = new Site(new Rectangle(GraphicsDevice.Viewport.Width - 110, GraphicsDevice.Viewport.Height - 60, 70, 60), true);
            cities[0] = new City(new Rectangle(130, GraphicsDevice.Viewport.Height - 40, 50, 30), true);
            cities[1] = new City(new Rectangle(210, GraphicsDevice.Viewport.Height - 40, 50, 30), true);
            cities[2] = new City(new Rectangle(290, GraphicsDevice.Viewport.Height - 40, 50, 30), true);
            cities[3] = new City(new Rectangle(460, GraphicsDevice.Viewport.Height - 40, 50, 30), true);
            cities[4] = new City(new Rectangle(540, GraphicsDevice.Viewport.Height - 40, 50, 30), true);
            cities[5] = new City(new Rectangle(620, GraphicsDevice.Viewport.Height - 40, 50, 30), true);
            window = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            gameState = GameState.menu;
            round = 1;
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
        public void ResetRound()
        {
            round++;
            Random rnd = new Random();
            while (eMissiles.Count != round)
                eMissiles.Add(new EnemyMissile(new Rectangle(rnd.Next(30, GraphicsDevice.Viewport.Width - 30), 0, 8, 8), new Vector2(rnd.Next(-1, 2), 1), false));
            for (int i = 0; i < 3; i++)
            {
                if (missileSites[i].alive == false)
                {
                    //missileSites[i].unDestroy();
                    missileSites[i].alive = true;
                }
                missileSites[i].Refill();
            }
            for (int i = 0; i < 6; i++)
            {
                cities[i].alive = true;
            }
            airplane.reset();
        }
        public bool Lost()
        {
            
            for (int i = 0; i < 3; i++)
            {
                if (missileSites[i].alive)
                {
                    return false;
                }
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
                

            if (gameState == GameState.menu)
            {

                if (kb.IsKeyDown(Keys.Enter) && oldkb.IsKeyUp(Keys.Enter))
                {
                    ResetRound();
                    gameState = GameState.play;
                }
               
            }

            if (gameState == GameState.play)
            {


                // airplane logic

                airplane.Update(window);
                if(airplane.firingMissile)
                {
                    // TODO: add pathing calculations for cities and missile sites (once those things are implemented)
                    eMissiles.Add(new EnemyMissile(new Rectangle(airplane.position.X, airplane.position.Y, 8, 8), new Vector2(1, 2), false));


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
                if (kb.IsKeyDown(Keys.S) && crosshair.Y < GraphicsDevice.Viewport.Height-crosshair.Height-50)
                {
                    crosshair.Y += 5;
                }
                if (kb.IsKeyDown(Keys.A) && crosshair.X > 0)
                {
                    crosshair.X -= 5;
                }
                if (kb.IsKeyDown(Keys.D) && crosshair.X < GraphicsDevice.Viewport.Width-crosshair.Width)
                {
                    crosshair.X += 5;
                }

                //moving the enemy missiles
                for (int i = 0; i < eMissiles.Count; i++)
                {
                    Console.WriteLine(eMissiles.Count);

                    eMissiles[i].Move();
                    //checking all split-offs if there are any
                    if (eMissiles.Count > 0 && i < eMissiles.Count && eMissiles[i].hasSplit)
                    {
                        for (int x = 0; x < eMissiles[i].splitR.Count; x++)
                        {
                            //cities
                            for (int j = 0; j < 6; j++)
                            {
                                if (eMissiles.Count > 0 && i < eMissiles.Count && eMissiles[i].splitR.Count > 0 && x < eMissiles[i].splitR.Count && eMissiles[i].splitR[x].Intersects(cities[j].position))
                                {
                                    cities[j].alive = false;
                                    eMissiles[i].splitR.RemoveAt(x);
                                    eMissiles[i].splitV.RemoveAt(x);
                                    break;
                                }
                            }
                            //missile sites
                            for (int j = 0; j < 3; j++)
                            {
                                if (eMissiles.Count > 0 && i < eMissiles.Count && eMissiles[i].splitR.Count > 0 && x < eMissiles[i].splitR.Count && eMissiles[i].splitR[x].Intersects(missileSites[j].position))
                                {
                                    missileSites[j].alive = false;
                                    //missileSites[j].Destroy();
                                    eMissiles[i].splitR.RemoveAt(x);
                                    eMissiles[i].splitV.RemoveAt(x);
                                    break;
                                }
                            }
                            
                        }
                    }

                    //checking for collision of buildings
                    if (missileSites[0].CheckHit(eMissiles[i].position) && missileSites[0].alive)
                    {
                        //missile site is hit
                        missileSites[0].alive = false;
                        //missileSites[0].Destroy();
                        eMissiles[i].trail.Clear();
                        eMissiles.RemoveAt(i);
                    }
                    else if (missileSites[1].CheckHit(eMissiles[i].position) && missileSites[1].alive)
                    {
                        //missile site is hit
                        missileSites[1].alive = false;
                        //missileSites[1].Destroy();
                        eMissiles[i].trail.Clear();
                        eMissiles.RemoveAt(i);
                    }
                    else if (missileSites[2].CheckHit(eMissiles[i].position) && missileSites[2].alive)
                    {
                        //missile site is hit
                        missileSites[2].alive = false;
                        //missileSites[2].Destroy();
                        eMissiles[i].trail.Clear();
                        eMissiles.RemoveAt(i);
                    }
                    //cities
                    if (eMissiles.Count > 0)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (eMissiles.Count > 0 && i < eMissiles.Count && cities[j].alive && cities[j].position.Intersects(eMissiles[i].position))
                            {
                                cities[j].alive = false;
                                eMissiles[i].trail.Clear();
                                eMissiles.RemoveAt(i);
                            }
                        }
                    }
                    
                    //out of bounds
                    if (eMissiles.Count > 0)
                    {
                        //if (i < eMissiles.Count && eMissiles[i].position.X < 0 || eMissiles[i].position.X > GraphicsDevice.Viewport.Width)
                        //{
                        //    //too far left or right
                        //    eMissiles[i].trail.Clear();
                        //    eMissiles.RemoveAt(i);
                        //}
                        if (i < eMissiles.Count && eMissiles[i].position.Y > GraphicsDevice.Viewport.Height - 20)
                        {
                            //city is hit
                            eMissiles[i].trail.Clear();
                            eMissiles.RemoveAt(i);

                        }
                    }
                    
                }

                //shooting the missiles
                if (kb.IsKeyDown(Keys.Space) && oldkb.IsKeyUp(Keys.Space))
                {
                    if (crosshair.X < GraphicsDevice.Viewport.Width / 3)
                    {
                        if (missileSites[0].missiles.Count != 0 && missileSites[0].missiles[0].velocity.Y == 0 && missileSites[0].alive)
                        {
                            missileSites[0].missiles[0].Calculate(crosshair, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                            movingMissiles.Add(missileSites[0].missiles[0]);
                            missileSites[0].missiles.RemoveAt(0);
                            markers.Add(new Rectangle(crosshair.X, crosshair.Y, 10, 10));
                        }
                        
                    }
                    else if (crosshair.X > GraphicsDevice.Viewport.Width / 3 && crosshair.X < 2*GraphicsDevice.Viewport.Width / 3 && missileSites[1].alive)
                    {
                        if (missileSites[1].missiles.Count != 0 && missileSites[1].missiles[0].velocity.Y == 0)
                        {
                            missileSites[1].missiles[0].Calculate(crosshair, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                            movingMissiles.Add(missileSites[1].missiles[0]);
                            missileSites[1].missiles.RemoveAt(0);
                            markers.Add(new Rectangle(crosshair.X, crosshair.Y, 10, 10));
                        }
                    }
                    else
                    {
                        if (missileSites[2].missiles.Count != 0 && missileSites[2].missiles[0].velocity.Y == 0 && missileSites[2].alive)
                        {
                            missileSites[2].missiles[0].Calculate(crosshair, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                            movingMissiles.Add(missileSites[2].missiles[0]);
                            missileSites[2].missiles.RemoveAt(0);
                            markers.Add(new Rectangle(crosshair.X, crosshair.Y, 10, 10));
                        }
                    }
                }
                for (int i = 0; i < movingMissiles.Count; i++)
                {
                    if (movingMissiles[i].velocity.Y != 0)
                    {
                        
                        if (movingMissiles[i].positionReached())
                        {
                            explosions.Add(new Explosion(movingMissiles[i].position));
                            movingMissiles.RemoveAt(i);
                            markers.RemoveAt(0);
                        }
                        else
                        {
                            
                            movingMissiles[i].Move();
                        }
                        

                    }
                    
                }
                for (int i = 0; i < explosions.Count; i++)
                {
                    explosions[i].Explode();
                    if (explosions[i].done)
                    {
                        explosions.RemoveAt(i);
                    }
                    for (int j = 0; j < eMissiles.Count; j++)
                    {
                        if (eMissiles.Count > 0 && explosions.Count > 0 && explosions[i].rect.Intersects(eMissiles[j].position))
                        {
                            
                            score += round * 100;
                            eMissiles.RemoveAt(j);
                        }
                        if (eMissiles.Count > 0 && explosions.Count > 0 && j < eMissiles.Count && eMissiles[j].hasSplit)
                        {
                            for (int x = 0; x < eMissiles[j].splitR.Count; x++)
                            {
                                if (eMissiles[j].splitR.Count > 0 && eMissiles[j].splitR[x].Intersects(explosions[i].rect))
                                {
                                    score += round * 100;
                                    eMissiles[j].splitR.RemoveAt(x);
                                    eMissiles[j].splitV.RemoveAt(x);
                                }
                            }
                        }
                    }
                    if (airplane.isActive && explosions.Count > 0 && explosions[i].rect.Intersects(airplane.position))
                    {
                        airplane.isActive = false ;
                    }
                }
                if (Lost())
                {
                    gameState = GameState.end;
                }
                if (eMissiles.Count == 0 && airplane.isActive == false)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (cities[i].alive)
                        {
                            score += 100;
                        }
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        score += missileSites[i].missiles.Count * 10;
                    }
                    ResetRound();
                }
            }
            if (gameState == GameState.end)
            {
                timer = 0;
                if (kb.IsKeyDown(Keys.R))
                {
                    score = 0;
                    round = 1;
                    gameState = GameState.menu;
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
                spriteBatch.DrawString(menuFont, "Welcome to Missile Command!\nUse WASD to move the crosshair around the screen\nPress SPACE to fire a missile to your crosshairs location\nPress ENTER to start!", new Vector2(100, 150), Color.White);

            if (gameState == GameState.play)
            {
                //cities
                for (int i = 0; i < 6; i++)
                {
                    if (cities[i].alive)
                    {
                        spriteBatch.Draw(pixel, cities[i].position, Color.Blue);
                    }
                }

                //ground
                spriteBatch.Draw(pixel, ground, null, Color.Yellow, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                //player missile sites
                if (missileSites[0].alive)
                    spriteBatch.Draw(pixel, missileSites[0].position, Color.Yellow);
                if (missileSites[0].missiles.Count == 0 || missileSites[0].alive == false)
                {
                    spriteBatch.DrawString(menuFont, "OUT", new Vector2(missileSites[0].position.X + 20, missileSites[0].position.Y + 5), Color.Black);
                }
                else if (missileSites[0].missiles.Count < 3)
                {
                    spriteBatch.DrawString(menuFont, "LOW: \n" + missileSites[0].missiles.Count, new Vector2(missileSites[0].position.X + 20, missileSites[0].position.Y + 5), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(menuFont, "" + missileSites[0].missiles.Count, new Vector2(missileSites[0].position.X + 20, missileSites[0].position.Y + 20), Color.Black);
                }

                if (missileSites[1].alive)
                    spriteBatch.Draw(pixel, missileSites[1].position, Color.Yellow);
                if (missileSites[1].missiles.Count == 0 || missileSites[1].alive == false)
                {
                    spriteBatch.DrawString(menuFont, "OUT", new Vector2(missileSites[1].position.X + 20, missileSites[1].position.Y + 5), Color.Black);
                }
                else if (missileSites[1].missiles.Count < 3)
                {
                    spriteBatch.DrawString(menuFont, "LOW:\n" + missileSites[1].missiles.Count, new Vector2(missileSites[1].position.X + 20, missileSites[1].position.Y + 5), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(menuFont, "" + missileSites[1].missiles.Count, new Vector2(missileSites[1].position.X + 20, missileSites[1].position.Y + 20), Color.Black);
                }

                if (missileSites[2].alive)
                    spriteBatch.Draw(pixel, missileSites[2].position, Color.Yellow);
                if (missileSites[2].missiles.Count == 0 || missileSites[2].alive == false)
                {
                    spriteBatch.DrawString(menuFont, "OUT", new Vector2(missileSites[2].position.X + 20, missileSites[2].position.Y + 5), Color.Black);
                }
                else if (missileSites[2].missiles.Count < 3)
                {
                    spriteBatch.DrawString(menuFont, "LOW:\n" + missileSites[2].missiles.Count, new Vector2(missileSites[2].position.X + 20, missileSites[2].position.Y + 5), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(menuFont, "" + missileSites[2].missiles.Count, new Vector2(missileSites[2].position.X + 20, missileSites[2].position.Y + 20), Color.Black);
                }


                for (int i = 0; i < movingMissiles.Count; i++)
                {
                    spriteBatch.Draw(pixel, movingMissiles[i].position, Color.White);
                }
                for (int i = 0; i < explosions.Count; i++)
                {
                    spriteBatch.Draw(pixel, explosions[i].rect, Color.White);
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
                    if (eMissiles[i].hasSplit)
                    {
                        for (int x = 0; x < eMissiles[i].splitR.Count; x++)
                        {
                            spriteBatch.Draw(pixel, eMissiles[i].splitR[x], Color.Red);
                        }
                    }
                }
                spriteBatch.Draw(crosshairT, crosshair, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                airplane.Draw(spriteBatch);
                //score
                spriteBatch.DrawString(scoreFont,""+score,new Vector2(350,0),Color.White);
                spriteBatch.DrawString(menuFont, "Round: " + round, new Vector2(10, 10), Color.White);
            }
            if (gameState == GameState.end)
            {
                spriteBatch.DrawString(menuFont, "You died with a score of " + score + "!\nPress R to go back to the start", new Vector2(150, 100), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


