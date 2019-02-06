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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LKimFinalProject
{
    // A class of action scene
    public class ActionScene : GameScene
    {
        #region Constant variables

        private const int INIT_X = 50;
        private const int INIT_Y = 640;
        private const int NUMBER_OF_COINS = 10;
        private const int GAMETIME = 1200;      //counts 20sec for level clear bonus
        private const int ZOMBIE_REGEN = 900;   // initial zombie regen interval is 15 sec

        #endregion

        #region Global variables

        private SpriteBatch spriteBatch;
        private SceneLayer background;
        private Song actionSong;
        private Song sadSong;
        private Map g;
        private Player player;
        private Collisions c;
        private GameString playerScoreString;
        private GameString highScoreString;
        private GameString gameOverString;
        private GameString gameCompleteString;
        private List<Coin> coins;
        private Chest chest;

        private int[,] mapMarkers;
        private int highScore;

        private int timePassed;             //increment every update: bonus = GAMETIME - timePassed
        #endregion

        /// <summary>
        /// A constructor for ActionScene object
        /// </summary>
        /// <param name="game">Game1</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="actionSong">a background music</param>
        /// <param name="sadSong">a mp3 file that will be played when player is dead</param>
        public ActionScene(Game game,
            SpriteBatch spriteBatch,
            Song actionSong, Song sadSong) : base(game)
        {
            
            this.spriteBatch = spriteBatch;
            this.actionSong = actionSong;
            this.sadSong = sadSong;

            mapMarkers = Shared.mapMarkers;
			Shared.GetHighScore();
			highScore = Shared.highScore;
            timePassed = 0;

            Shared.isHighScore = false;
            Shared.isNextLevel = false;

            #region Background and map

            MediaPlayer.Play(actionSong);

            Texture2D bgTex = game.Content.Load<Texture2D>("images/Background");
            background = new SceneLayer(game, spriteBatch, bgTex);
            this.Components.Add(background);

            Texture2D gTex = game.Content.Load<Texture2D>("images/map");
            g = new Map(game, spriteBatch, gTex);
            this.Components.Add(g);

            #endregion

            #region coins and chest

            Texture2D coinTex = game.Content.Load<Texture2D>("images/coin");
            Texture2D chestTex = game.Content.Load<Texture2D>("images/chest");
            coins = new List<Coin>();

            for (int i = 0; i < mapMarkers.GetLength(0); i++)
            {
                for (int j = 0; j < mapMarkers.GetLength(1); j++)
                {
                    if (mapMarkers[i, j] == (int)Shared.GridType.coin)
                    {
                        Coin coin = new Coin(game, spriteBatch, coinTex, i, j);
                        coins.Add(coin);
                        this.Components.Add(coin);
                    }

                    else if (mapMarkers[i, j] == (int)Shared.GridType.chest)
                    {
                        chest = new Chest(game, spriteBatch, chestTex, i, j);
                        this.Components.Add(chest);
                    }
                }
            }

            #endregion

            #region Player and collision

            // Player
            Texture2D playerTex = game.Content.Load<Texture2D>("images/player");
            Vector2 playerPos = new Vector2(INIT_X, INIT_Y);
            SoundEffect jumpSound = game.Content.Load<SoundEffect>("sounds/jump");
            SoundEffect gameOver = game.Content.Load<SoundEffect>("sounds/gameover");
            player = new Player(game, spriteBatch, playerTex, playerPos, jumpSound, gameOver);
            this.Components.Add(player);

            // Collisions
            SoundEffect coinSound = game.Content.Load<SoundEffect>("sounds/clink");
            SoundEffect gameClear = game.Content.Load<SoundEffect>("sounds/gamecomplete");
            c = new Collisions(game, player, g, coins, chest, coinSound, gameClear);
            this.Components.Add(c);

            #endregion

            #region Zombie and collision

            // No zombies in level 1
            if (Shared.level != 1)
                AddZombie();

            #endregion

            #region Strings

            SpriteFont scoreFont = game.Content.Load<SpriteFont>("fonts/scoreFont");
            SpriteFont messageFont = game.Content.Load<SpriteFont>("fonts/messageFont");

            // score string
            playerScoreString = new GameString(game, spriteBatch, scoreFont, Color.White);
            playerScoreString.Message = string.Format("{0:D8}", 0);
            playerScoreString.Position = new Vector2(0, Shared.stage.Y - scoreFont.LineSpacing);
            this.Components.Add(playerScoreString);

            // highscore string
            highScoreString = new GameString(game, spriteBatch, scoreFont, Color.White);
            highScoreString.Message = string.Format("{0:D8}", highScore);
            highScoreString.Position = new Vector2(Shared.stage.X - scoreFont.MeasureString(highScoreString.Message).X,
                Shared.stage.Y - scoreFont.LineSpacing);
            this.Components.Add(highScoreString);

            // gameover string
            gameOverString = new GameString(game, spriteBatch, messageFont, Color.Gray);
            gameOverString.Message = "GAME OVER\nSPACE TO RECORD SCORE";
            gameOverString.Position = new Vector2((Shared.stage.X - messageFont.MeasureString(gameOverString.Message).X) / 2,
                (Shared.stage.Y - messageFont.MeasureString(gameOverString.Message).Y) / 2);
            this.Components.Add(gameOverString);
            gameOverString.Enabled = false;
            gameOverString.Visible = false;

            // level complete string
            gameCompleteString = new GameString(game, spriteBatch, messageFont, Color.Gray);
            gameCompleteString.Message = "LEVEL COMPLETE\nSPACE TO NEXT LEVEL";
            gameCompleteString.Position = new Vector2((Shared.stage.X - messageFont.MeasureString(gameCompleteString.Message).X) / 2,
                (Shared.stage.Y - messageFont.MeasureString(gameCompleteString.Message).Y) / 2);
            this.Components.Add(gameCompleteString);
            gameCompleteString.Enabled = false;
            gameCompleteString.Visible = false;

            #endregion
        }

		/// <summary>
		/// An overriding method that reads any update in game world and makes objects interact accordingly
		/// </summary>
		/// <param name="gameTime">GameTime</param>
		public override void Update(GameTime gameTime)
        {
            timePassed++;

            // get scores
            playerScoreString.Message = string.Format("{0:D8}", player.Score);
            highScoreString.Message = string.Format("{0:D8}", highScore);

            if (player.Score >= highScore)
                highScoreString.Message = playerScoreString.Message;

            // if player clears the stage, display level complete message
            if (player.IsClear && !gameCompleteString.Enabled)
            {
                Shared.isNextLevel = true;

                // Add bonus score
                int bonus = GAMETIME - timePassed;
                if (bonus < 0)
                    bonus = 0;
                player.Score += bonus;

                Shared.currentScore = player.Score;
                Shared.level++;

                gameCompleteString.Enabled = true;
                gameCompleteString.Visible = true;
                this.Enabled = false;
            }

            // if player died, check if the player score hit top5
            if (player.IsDead && !gameOverString.Enabled)
            {
                Shared.currentScore = player.Score;

                MediaPlayer.Stop();
                MediaPlayer.Play(sadSong);
                Shared.index = Shared.SortList(player.Score);

                if (Shared.index == -1)
                {
                    gameOverString.Message = "GAME OVER\nESC TO MAIN MENU";
                }

                else
                {
                    gameOverString.Message = "GAME OVER\nSPACE TO RECORD SCORE";
                    Shared.isHighScore = true;
                }

                gameOverString.Enabled = true;
                gameOverString.Visible = true;
            }

            // Add more Zombies
            // zombie regens more frequently as level gets higher
            if (Shared.level != 1 && timePassed % (ZOMBIE_REGEN / Shared.level) == 0)
                AddZombie();

            base.Update(gameTime);
        }

        /// <summary>
        /// A method that adds zombie and collisionZombie
        /// </summary>
        private void AddZombie()
        {
            Texture2D fallingTex = Game.Content.Load<Texture2D>("images/zombie_falling");
            Texture2D walkingTex = Game.Content.Load<Texture2D>("images/zombie_standing");
            Zombie zombie = new Zombie(Game, spriteBatch, fallingTex, walkingTex);
            this.Components.Add(zombie);

            SoundEffect stepSound = Game.Content.Load<SoundEffect>("sounds/step");
            SoundEffect gameOverSound = Game.Content.Load<SoundEffect>("sounds/gameover");
            CollisionsZombie cz = new CollisionsZombie(Game, zombie, g, player, stepSound, gameOverSound);
            this.Components.Add(cz);
        }
    }
}

