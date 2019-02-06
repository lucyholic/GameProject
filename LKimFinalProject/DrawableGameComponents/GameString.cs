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
    // A class of GameString
	public class GameString : DrawableGameComponent
	{
        #region Variables

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Color color;
        private string message;
        private Vector2 position;

        public Vector2 Position { get => position; set => position = value; }
        public string Message { get => message; set => message = value; } 

        #endregion

        /// <summary>
        /// A constructor for GameString object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="font">SpriteFont</param>
        /// <param name="color">String Color</param>
        public GameString(Game game,
			SpriteBatch spriteBatch,
			SpriteFont font,
            Color color) : base(game)
		{
			this.spriteBatch = spriteBatch;
			this.font = font;
            this.color = color;
		}

        /// <summary>
        /// An overriding method that draws string on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			spriteBatch.DrawString(font, message, position, color);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
