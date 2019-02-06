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
    // A class of Map
	public class Map : DrawableGameComponent
	{
        // Variables
		private SpriteBatch spriteBatch;
		private Texture2D tex;
		private Vector2 position;

		private List<Rectangle> grounds;
		public List<Rectangle> Grounds { get => grounds; set => grounds = value; }
		private int[,] markers = Shared.mapMarkers;

        /// <summary>
        /// A constructor for Map object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="tex">Map Texture</param>
		public Map(Game game,
			SpriteBatch spriteBatch,
			Texture2D tex) : base(game)
		{
			this.spriteBatch = spriteBatch;
			this.tex = tex;
			position = new Vector2(0);
			grounds = new List<Rectangle>();

			GetRectangles();
		}

        /// <summary>
        /// A method that grabs grounds from mapMarker, 
        /// creates rectangles and add them to list
        /// </summary>
		private void GetRectangles()
		{
			for (int i = 0; i < markers.GetLength(0); i++)
			{
				Rectangle r;
				int x = 0;
				int y = 0;
				int width = 0;
				int height = Shared.GRID_HEIGHT;

				for (int j = 0; j < markers.GetLength(1); j++)
				{
					if (markers[i, j] == (int)Shared.GridType.ground)
					{
                        // Grab the starting grid of the ground
						if (j == 0 || markers[i, j - 1] != (int)Shared.GridType.ground)
							x = j * Shared.GRID_WIDTH;
						y = i * Shared.GRID_HEIGHT;
						width += Shared.GRID_WIDTH;

						// if the rectangle is in the end of the row,
						// add it to the list
						if (j + 1 == markers.GetLength(1))
						{
							r = new Rectangle(x, y, width, height);
							grounds.Add(r);
						}
					}

					else
					{
						if (width != 0) // This means a set of grounds ended
										// add a rectangle to the list and reset variables
						{
							r = new Rectangle(x, y, width, height);
							grounds.Add(r);

							x = 0;
							y = 0;
							width = 0;
						}
					}
				}
			}
		}

        /// <summary>
        /// An overriding method that draws map on the game display
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
