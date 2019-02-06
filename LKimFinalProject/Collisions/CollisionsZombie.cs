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
    // A class of zombie collisions
	public class CollisionsZombie : GameComponent
	{
        #region Variables

        private const int ZOMBIE_SCORE = 100;

        private Zombie z;
        private Map m;
        private Player p;
        private SoundEffect stepSound;
        private SoundEffect gameOverSound;
        private Vector2 outPosition = new Vector2(-100); 

        #endregion

        /// <summary>
        ///  A constructor for CollisionsZombie object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="z">Zombie</param>
        /// <param name="m">Map</param>
        /// <param name="p">Player</param>
        /// <param name="stepSound">Sound effect for killing zombie</param>
        /// <param name="gameOverSound">Game over sound when player dies</param>
        public CollisionsZombie(Game game,
			Zombie z, Map m, Player p,
			SoundEffect stepSound, SoundEffect gameOverSound) : base(game)
		{
			this.z = z;
			this.m = m;
			this.p = p;
			this.stepSound = stepSound;
            this.gameOverSound = gameOverSound;
		}

		/// <summary>
		/// An overriding method that reads any update in game world and detects collisions
		/// </summary>
		/// <param name="gameTime">GameTime</param>
		public override void Update(GameTime gameTime)
		{
			HitGround(z, m);
			HitPlayer(z, p);
			base.Update(gameTime);
		}

        /// <summary>
        /// A method that checks collisions between zombie and ground on the map
        /// </summary>
        /// <param name="z">Zombie</param>
        /// <param name="m">Map</param>
		public void HitGround(Zombie z, Map m)
		{
			List<Rectangle> grounds = m.Grounds;
			int count = 0;

			foreach (Rectangle rect in grounds)
			{
				Rectangle zombieRect = z.GetRectangle(z.Position);
				Rectangle zombieCollisionRect = z.GetCollisionRect(z.Position);

				if (rect.Intersects(zombieCollisionRect))
				{
					count++;

					if (zombieRect.Y + zombieRect.Height >= rect.Y && zombieRect.Y < rect.Y) // zombie hits the top of rect
					{
						z.IsGrounded = true;

						if (zombieRect.Y + zombieRect.Height > rect.Y)
							z.Position = new Vector2(zombieRect.X, rect.Y - zombieRect.Height);
					}

                    // zombie hits the side
                    else if (zombieRect.X + zombieRect.Width >= rect.X && rect.X > zombieRect.X)
						z.Position = new Vector2(rect.X - zombieRect.Width, zombieRect.Y);
					else if (rect.X + rect.Width >= zombieRect.X && rect.X < zombieRect.X) 
						z.Position = new Vector2(rect.X + rect.Width, zombieRect.Y);
				}
			}

			// if zombie is not touching any ground
			if (count == 0)
			{
				z.IsGrounded = false;
			}

		}

        /// <summary>
        /// A method that checkes collisions between zombie and player
        /// </summary>
        /// <param name="z">Zombie</param>
        /// <param name="p">Player</param>
		public void HitPlayer(Zombie z, Player p)
		{
			Rectangle zombieRect = z.GetCollisionRect(z.Position);
			Rectangle playerRect = p.GetCollisionRect(p.Position);

			if (zombieRect.Intersects(playerRect))
			{
                // If player is stepping on the zombie, 
                // player gets score and zombie dies
				if (p.IsFalling)
				{
					stepSound.Play();
					z.Position = outPosition;
					z.Enabled = false;
					z.Visible = false;
					p.Score += ZOMBIE_SCORE;
				}

                // Otherwise, player dies
				else
				{
                    p.IsDead = true;
                    p.Enabled = false;
                    p.Visible = false;
                }

			}
		}
	}
}
