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
    // A class of HighScoreScene
	public class HighScoreScene : GameScene
	{
        // variables
		private SpriteBatch spriteBatch;
		private SpriteFont font;

		private const int INIT_ROW = 3;
		private const int INIT_COL = 2;
		private int gridHeight = Shared.GRID_HEIGHT;

        /// <summary>
        /// A constructor for HighScoreScene object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="font">SpriteFont</param>
		public HighScoreScene(Game game,
			SpriteBatch spriteBatch,
			SpriteFont font) : base(game)
		{
			this.spriteBatch = spriteBatch;
			this.font = font;
		}

        /// <summary>
        /// An overriding method that draws high scores on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();

			for (int i = 0; i < Shared.scores.Length; i++)
			{
				string line = $"{Shared.names[i]}  {string.Format("{0:D8}", Shared.scores[i])}\n";

				Vector2 position;
				position.X = (Shared.stage.Y - font.MeasureString(line).Y) / 2; 
				position.Y = (INIT_ROW + i) * gridHeight;
				spriteBatch.DrawString(font, line, position, Color.Black);
			}
			
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
