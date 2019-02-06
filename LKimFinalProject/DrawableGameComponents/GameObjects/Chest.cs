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
    // A class of chest
	public class Chest : GameObject
	{
		private SpriteBatch spriteBatch;
		private Texture2D tex;
		private Vector2 position;

        public Vector2 Position { get => position; set => position = value; }

        /// <summary>
        /// A constructor for Chest object
        /// </summary>
        /// <param name="game">Game1</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="tex">Chest Texture</param>
        /// <param name="row">The row of the chest in grid</param>
        /// <param name="column">The column of the chest in grid</param>
        public Chest(Game game,
			SpriteBatch spriteBatch,
			Texture2D tex,
			int row, int column) : base(game)
		{
			this.spriteBatch = spriteBatch;
			this.tex = tex;

            Width = tex.Width;
            Height = tex.Height;

            this.position = GetPosition(row, column);
		}

        /// <summary>
        /// An overriding method that draws chest on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(tex, position, Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
