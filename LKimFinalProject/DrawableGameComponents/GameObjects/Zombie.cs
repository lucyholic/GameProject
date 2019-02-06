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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LKimFinalProject
{
    // A class of Zombie
	public class Zombie : GameObject
	{
        #region Variables

        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Texture2D fallingTex;
        private Texture2D walkingTex;
        private Vector2 position;

        private int speed;          // speed of zombie
        private int switchTime;     // time that zombie switches direction
        private int regenCol;       // zombie regen point
        private int timePassed;     // game world time count   

        private bool isGrounded;

        private const int WIDTH = 56;
        private const int HEIGHT = 50;
        private const int FALLING_STEP = 3;
        private const int MIN_SPEED = -5;
        private const int MAX_SPEED = 5;
        private const int MIN_SWITCHTIME = 120;
        private const int MAX_SWITCHTIME = 240;
        private const int MAX_REGENCOL = 15;

        private int gridWidth = Shared.GRID_WIDTH;

        public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
        public Vector2 Position { get => position; set => position = value; }

        #endregion

        /// <summary>
        /// A constructor for Zombie object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="fallingTex">Falling texture</param>
        /// <param name="walkingTex">Walking texture</param>
        public Zombie(Game game,
			SpriteBatch spriteBatch,
			Texture2D fallingTex,
			Texture2D walkingTex) : base(game)
		{
			this.spriteBatch = spriteBatch;
			this.fallingTex = fallingTex;
			this.walkingTex = walkingTex;

            Width = WIDTH;
            Height = HEIGHT;

            // create random numbers for speed, switchtime, and regenCol
            // speed cannot be zero
			Random r = new Random();

			while (speed == 0)
			{
				this.speed = r.Next(MIN_SPEED, MAX_SPEED);
			}

			this.switchTime = r.Next(MIN_SWITCHTIME, MAX_SWITCHTIME);
			this.regenCol = r.Next(0, MAX_REGENCOL);

			position = new Vector2(gridWidth * regenCol, 0);
			timePassed = 0;

            Speed = speed;
        }

        /// <summary>
        /// An overriding method that draws zombie on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(tex, position, null, Color.White, 0f, Vector2.Zero, 1f, Effect, 1f);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		/// <summary>
		/// An overriding method that reads any update in game world and makes zombie move
		/// </summary>
		/// <param name="gameTime">GameTime</param>
		public override void Update(GameTime gameTime)
		{
			timePassed++;
			Flip();

			// moves right or left when on the ground
			if (isGrounded)
			{
				tex = walkingTex;
				position.X += Speed;
			}

			else
			{
				tex = fallingTex;
				position.Y += FALLING_STEP;
			}

            // if switch time is up, change direction
			if (timePassed % switchTime == 0)
			{
				ChangeDirection();
			}

			if (position.X < 0)
			{
				position.X = 0;
				ChangeDirection();
			}

			if (position.X + WIDTH > Shared.stage.X)
			{
				position.X = Shared.stage.X - WIDTH;
				ChangeDirection();
			}

			base.Update(gameTime);
		}
	}
}
