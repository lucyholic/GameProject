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

namespace LKimFinalProject
{
    // A class of player
    public class Player : GameObject
    {
        #region constant variables

        // dimension of frame
        private const int FRAME_WIDTH = 104;
        private const int FRAME_HEIGHT = 80;

        // dimension of player
        // MARGIN + PLAYER_WIDTH + MARGIN = width of player frame
        // the frame is wider that the actual character body
        public const int MARGIN = 22;
        public const int PLAYER_WIDTH = 60;
        public const int PLAYER_HEIGHT = 80;
        public const int COLLISION_MARGIN = 1;

        // rows and columns of sheet
        private const int ROW = 8;
        private const int COLUMN = 5;

        // starting index of each animation
        private const int WALKING_INDEX = 0;
        private const int JUMPING_INDEX = 18;
        private const int FALLING_INDEX = 19;
        private const int IDLE_INDEX = 20;
        private const int DELAY = 2;

        private const int SPEED = 3;
        private const int JUMPPOWER = 200;
        private const int JUMPSTEP = 5;

        #endregion

        #region global variables

        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position;
        private SoundEffect jumpSound;
        private SoundEffect gameOverSound;
        private Vector2 dimension;
        private List<Rectangle> frames;
        private int frameIndex = IDLE_INDEX;
        private int delayCounter;

        private Vector2 oldPosition;
        private KeyboardState oldState;

        private bool hitBottom = false;
        private bool hitTop = true;
        private bool isJumping = false;
        private bool isWalking = false;
        private bool wasJumping = false;
        private bool wasWalking = false;
        private bool isGrounded = true;
        private bool isFalling = false;
        private bool isClear = false;
        private bool isDead = false;

        private int score;

        #endregion

        #region Getters and setters

        public Vector2 Position { get => position; set => position = value; }
        public int Score { get => score; set => score = value; }
        public bool HitBottom { get => hitBottom; set => hitBottom = value; }
        public bool HitTop { get => hitTop; set => hitTop = value; }
        public bool IsClear { get => isClear; set => isClear = value; }
        public bool IsFalling { get => isFalling; set => isFalling = value; }
        public bool IsDead { get => isDead; set => isDead = value; }

        #endregion

        /// <summary>
        /// A constructor for Player object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="tex">Player Texture</param>
        /// <param name="position">Player position</param>
        /// <param name="jumpSound">Jump sound</param>
        /// <param name="gameOverSound">Game over sound</param>
        public Player(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position,
            SoundEffect jumpSound,
            SoundEffect gameOverSound) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
            this.jumpSound = jumpSound;
            this.gameOverSound = gameOverSound;

            this.Width = PLAYER_WIDTH;
            this.Height = PLAYER_HEIGHT;
            this.Speed = SPEED;

            this.score = Shared.currentScore;
            this.oldPosition = position;
            this.dimension = new Vector2(FRAME_WIDTH, FRAME_HEIGHT);
            this.frames = CreateFrames(dimension, ROW, COLUMN);
        }

        /// <summary>
        /// An overriding method that reads any update in game world and makes player move
        /// </summary>
        /// <param name="gameTime">GameTime</param>
		public override void Update(GameTime gameTime)
        {
            #region player animation

            if (isWalking && isGrounded)
            {
                // if it starts walking, set frame index to zero
                if (isWalking != wasWalking)
                    frameIndex = 0;

                delayCounter++;

                if (delayCounter > DELAY)
                {
                    frameIndex++;

                    if (frameIndex > JUMPING_INDEX - WALKING_INDEX - 1)
                        frameIndex = 0;

                    delayCounter = 0;
                }
            }

            if (isJumping && !isFalling)
                frameIndex = JUMPING_INDEX;

            if (isFalling)
                frameIndex = FALLING_INDEX;

            // if it's not either walking or jumping, set idle
            if (!isWalking && !isJumping && !isFalling && isGrounded)
            {
                // if it stopped what it was doing, start animation from the first frame of idle
                if (isWalking != wasWalking || isJumping != wasJumping)
                {
                    frameIndex = IDLE_INDEX;
                    wasWalking = false;
                    wasJumping = false;
                }

                delayCounter++;

                if (delayCounter > DELAY)
                {
                    frameIndex++;

                    if (frameIndex > ROW * COLUMN - 1)
                        frameIndex = IDLE_INDEX;

                    delayCounter = 0;
                }
            }

            #endregion

            #region Player moves based on bool values

            if (!isGrounded)
            {
                if (position.Y == oldPosition.Y || hitTop)
                {
                    isFalling = false;
                    isGrounded = true;
                    isJumping = false;
                    oldPosition = position;

                    wasJumping = isJumping;
                    wasWalking = isWalking;
                    hitTop = false;
                }

                else if (position.Y == oldPosition.Y - JUMPPOWER || hitBottom || position.Y == 0)
                {
                    isJumping = true;
                    isFalling = true;
                    PlayerFall();
                    hitBottom = false;
                }

                else if (position.Y > oldPosition.Y - JUMPPOWER)
                {
                    if (!isFalling)
                        PlayerJump();
                    else
                        PlayerFall();
                }
            }

            else if (!hitBottom && !hitTop)
            {
                isGrounded = false;
                isFalling = true;
                PlayerFall();
            }

            if (position.Y > Shared.stage.Y)
            {
                gameOverSound.Play();
                isDead = true;
            }

            if (isDead)
            {
                this.Enabled = false;
                this.Visible = false;
            }

            #endregion

            #region Player moves on keyboard control

            KeyboardState ks = Keyboard.GetState();

            if (isGrounded && ks.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space))
            {
                jumpSound.Play();

                oldPosition = position;
                isGrounded = false;
                isJumping = true;
                isWalking = false;
                isFalling = false;

                PlayerJump();

                wasJumping = isJumping;
                wasWalking = isWalking;
                oldState = ks;
            }

            if (ks.IsKeyUp(Keys.Space))
            {
                oldState = ks;
            }

            if (ks.IsKeyDown(Keys.Left))
            {
                if (isGrounded)
                {
                    isJumping = false;
                    isWalking = true;
                }
                
                if (Speed > 0)
                    Speed = -Speed;

                Flip();
                position.X += Speed;

                if (position.X < -MARGIN)
                    position.X = -MARGIN;

                wasJumping = isJumping;
                wasWalking = isWalking;
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                if (isGrounded)
                {
                    isJumping = false;
                    isWalking = true;
                }

                if (Speed < 0)
                    Speed = -Speed;

                Flip();
                position.X += Speed;

                if (position.X + PLAYER_WIDTH + MARGIN > Shared.stage.X)
                    position.X = Shared.stage.X - PLAYER_WIDTH - MARGIN;

                wasJumping = isJumping;
                wasWalking = isWalking;
            }

            if (ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Right) && (ks.IsKeyUp(Keys.Space) && isGrounded))
            {
                isJumping = false;
                isWalking = false;
            }

            #endregion

            #region Manage score
            // if score gets over 8 digits
            if (score > 99999999)
                score = 99999999;

            base.Update(gameTime); 
            #endregion
        }

        /// <summary>
        /// An overriding method that draws player animation on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
		public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, position, frames[frameIndex], Color.White, 0f, new Vector2(0), 1f, Effect, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Methods

        /// <summary>
        /// A method that makes player jump
        /// </summary>
        public void PlayerJump()
        {
            position.Y -= JUMPSTEP;
            isGrounded = false;
            hitTop = false;
        }

        /// <summary>
        /// A method that makes player fall
        /// </summary>
		public void PlayerFall()
        {
            position.Y += JUMPSTEP;
        }

        /// <summary>
        ///  A method that grabs rectangle of object
        /// </summary>
        /// <returns>Rectangle of player</returns>
		public override Rectangle GetRectangle(Vector2 position)
        {
            return new Rectangle((int)position.X + MARGIN, (int)position.Y, Width, Height);
        }

        /// <summary>
        /// A method that returns a bigger rectangle of object to detect collision
        /// </summary>
        /// <returns>1 pixel bigger rectangle of player</returns>
		public override Rectangle GetCollisionRect(Vector2 position)
        {
            return new Rectangle((int)position.X + MARGIN - COLLISION_MARGIN, (int)position.Y - COLLISION_MARGIN,
                Width + COLLISION_MARGIN * 2, Height + COLLISION_MARGIN * 2);
        }
        #endregion
    }
}
