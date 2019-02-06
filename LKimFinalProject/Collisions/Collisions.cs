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
    // A class of player collisions
	public class Collisions : GameComponent
	{
        #region Variables

        private const int COIN_SCORE = 20;
        private const int CHEST_SCORE = 500;

        private Player p;
        private Map m;
        private List<Coin> coins;
        private Chest chest;
        private SoundEffect coinSound;
        private SoundEffect gameClearSound;

        private int margin = Player.MARGIN;

        #endregion

        /// <summary>
        /// A constructure for Collision object
        /// </summary>
        /// <param name="game">Game1</param>
        /// <param name="p">Player</param>
        /// <param name="m">Map</param>
        /// <param name="coins">List of Coins</param>
        /// <param name="chest">Chest</param>
        /// <param name="coinSound">Clink sound for coins</param>
        /// <param name="gameClearSound">Game clear sound for chest</param>
        public Collisions(Game game,
			Player p,
			Map m,
			List<Coin> coins,
			Chest chest,
			SoundEffect coinSound,
			SoundEffect gameClearSound) : base(game)
		{
			this.p = p;
			this.m = m;
			this.coins = coins;
			this.chest = chest;
			this.coinSound = coinSound;
			this.gameClearSound = gameClearSound;
		}

		/// <summary>
		/// An overriding method that reads any update in game world and detects collisions
		/// </summary>
		/// <param name="gameTime">GameTime</param>
		public override void Update(GameTime gameTime)
		{
			GetHit(p, m);
			GetOverlap(p, coins, chest);
			base.Update(gameTime);
		}

        /// <summary>
        /// A method that checks collision between player and ground on the map
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="m">Map</param>
		public void GetHit(Player p, Map m)
		{
			List<Rectangle> grounds = m.Grounds;
			int count = 0;

			foreach (Rectangle rect in grounds)
			{
                Rectangle playerRect = p.GetRectangle(p.Position);
                Rectangle playerCollisionRect = p.GetCollisionRect(p.Position);

				if (rect.Intersects(playerCollisionRect))
				{
					count++;

					if (rect.Y + rect.Height == playerRect.Y) // player hits the bottom of rect
					{
						p.HitBottom = true;
						p.HitTop = false;
					}

					else if (playerRect.Y + playerRect.Height == rect.Y) // player hits the top of rect
					{
						p.HitTop = true;
						p.HitBottom = false;
					}

					else if (playerRect.X + playerRect.Width >= rect.X && rect.X > playerRect.X) // player hits the left side
						p.Position = new Vector2(rect.X - playerRect.Width - margin, p.Position.Y);

					else if (rect.X + rect.Width >= playerRect.X && rect.X < playerRect.X) // player hits the right side
						p.Position = new Vector2(rect.X + rect.Width - margin, p.Position.Y);
				}
			}

            // if player is not touching any ground
			if (count == 0)
			{
				p.HitTop = false;
				p.HitBottom = false;
			}
		}

        /// <summary>
        /// A method that checks collision between player and either coin or chest
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="coins">List of coins</param>
        /// <param name="chest">Chest</param>
		public void GetOverlap(Player p, List<Coin> coins, Chest chest)
		{
            Rectangle playerRect = p.GetRectangle(p.Position);
            Rectangle chestRect = chest.GetRectangle(chest.Position);

            // if coin, plus score and remove coin
			foreach (Coin coin in coins)
			{
				Rectangle coinRect = coin.GetRectangle(coin.Position);

				if (coinRect.Intersects(playerRect) && coin.Enabled)
				{
					p.Score += COIN_SCORE;
					coinSound.Play();

					coin.Enabled = false;
					coin.Visible = false;
				}
			}

            // if chest, plus score and level complete
			if (chestRect.Intersects(playerRect) && chest.Enabled)
			{
				p.Score += CHEST_SCORE;
				gameClearSound.Play();

				chest.Enabled = false;
				chest.Visible = false;
				p.Enabled = false;
				p.IsClear = true;
			}
		}
	}
}
