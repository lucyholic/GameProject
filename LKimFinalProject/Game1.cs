/* Program Code: PROG2370 Game Programming
 * 
 * Project name: LKimFinalProject
 * 
 * Purpose: To build a complete game using Monogame framework
 * 
 * Written By: Lucy Kim
 * 
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LKimFinalProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		private const int SCREEN_WIDTH = 1440;
		private const int SCREEN_HEIGHT = 810;

        private SceneBackground bg;
        private StartScene startScene;
        private ActionScene actionScene;
        private EnterNameScene enterNameScene;
        private HelpScene helpScene;
		private HighScoreScene highScoreScene;
		private CreditScene creditScene;

		private SpriteFont scoreFont;

        private Song bgSong;
        private Song actionSong;
        private Song sadSong;

        private enum Menu
        {
            action,
            help,
            highScore,
            credit,
            quit
        }

        /// <summary>
        /// A default constructor for Game
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
			graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
			graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
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

            // Set stage
            Shared.stage = new Vector2(graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);

            IsMouseVisible = true;

            base.Initialize();
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

            // Add scrolling background
            Texture2D bgTex = this.Content.Load<Texture2D>("images/Background");
            bg = new SceneBackground(this, spriteBatch, bgTex, Vector2.Zero);
            this.Components.Add(bg);

            // Retrieve high score list
            Shared.ReadScores();
			Shared.GetHighScore();

            // Add background musics
            MediaPlayer.IsRepeating = true;

            bgSong = this.Content.Load<Song>("sounds/bensound-clearday");
            actionSong = this.Content.Load<Song>("sounds/bensound-littleidea");
            sadSong = this.Content.Load <Song>("sounds/bensound-creepy");

            // Add scenes
            startScene = new StartScene(this, spriteBatch);
            this.Components.Add(startScene);

            enterNameScene = new EnterNameScene(this, spriteBatch);
            this.Components.Add(enterNameScene);

            helpScene = new HelpScene(this, spriteBatch);
            this.Components.Add(helpScene);

			scoreFont = this.Content.Load<SpriteFont>("fonts/nameFont");
			highScoreScene = new HighScoreScene(this, spriteBatch, scoreFont);
			this.Components.Add(highScoreScene);

			creditScene = new CreditScene(this, spriteBatch);
			this.Components.Add(creditScene);

            MediaPlayer.Play(bgSong);

            ReturnToMenu();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here

            KeyboardState ks = Keyboard.GetState();
            int selectedIndex = startScene.Menu.SelectedIndex;

			if (startScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Enter))
                {
                    if (selectedIndex == (int)Menu.action)
                    {
                        startScene.Hide();
                        MediaPlayer.Stop();

                        actionScene = new ActionScene(this, spriteBatch, actionSong, sadSong);
                        this.Components.Add(actionScene);
                        actionScene.Show();
                    }

                    else if (selectedIndex == (int)Menu.help)
                    {
                        startScene.Hide();
                        helpScene.Show();
                    }

                    else if (selectedIndex == (int)Menu.highScore)
                    {
                        startScene.Hide();
                        highScoreScene.Show();
                    }

                    else if (selectedIndex == (int)Menu.credit)
                    {
                        startScene.Hide();
                        creditScene.Show();
                    }

                    else if (selectedIndex == (int)Menu.quit)
                        Exit();
                }
            }

            else
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
					// if previous scene was action scene, stop actionsong and play bgsong
					if (selectedIndex == (int)Menu.action) 
					{
						MediaPlayer.Stop();
						MediaPlayer.Play(bgSong);
					}

                    // if enterNameScene is enabled, write score on score.txt
                    if (enterNameScene.Enabled)
                    {
                        Shared.RecordScores();
                    }

                    // reset level and current score
                    Shared.level = 1;
                    Shared.currentScore = 0;

					ReturnToMenu();
                }

                if (ks.IsKeyDown(Keys.Space))
                {
                    if(Shared.isNextLevel)
                    {
                        actionScene.Hide();
                        MediaPlayer.Stop();

                        // Add a new actionScene
                        actionScene = new ActionScene(this, spriteBatch, actionSong, sadSong);
                        this.Components.Add(actionScene);
                        actionScene.Show();

                        Shared.isNextLevel = false;
                    }

                    if(Shared.isHighScore)
                    {
                        actionScene.Hide();
                        Console.WriteLine(Shared.isHighScore);
                        MediaPlayer.Stop();
                        MediaPlayer.Play(bgSong);

                        // Add a new enterNameScene
                        enterNameScene = new EnterNameScene(this, spriteBatch);
                        this.Components.Add(enterNameScene);

                        enterNameScene.Show();
                        Shared.isHighScore = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// A method that hides all scenes and returns to the menu
        /// </summary>
        private void ReturnToMenu()
        {
            foreach (GameScene item in Components)
            {
                item.Enabled = false;
                item.Visible = false;
            }

            bg.Show();
            startScene.Show();
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
