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
    // A class of coin
    public class Coin : GameObject
    {
        #region Variables

        private const int FRAME_WIDTH = 64;
        private const int FRAME_HEIGHT = 70;
        private const int FRAME_ROW = 4;
        private const int FRAME_COLUMN = 4;
        private const int DELAY = 5;

        private const int NUMBER_OF_COINS = 10;

        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position;
        private Vector2 dimension;
        private List<Rectangle> frames;

        private int frameIndex = 0;
        private int delayCounter;

        public Vector2 Position { get => position; set => position = value; }

        #endregion

        /// <summary>
        /// A constructor for Coin object
        /// </summary>
        /// <param name="game">Game1</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="tex">Chest Texture</param>
        /// <param name="row">The row of the chest in grid</param>
        /// <param name="column">The column of the chest in grid</param>
        public Coin(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            int row, int column) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;

            Width = FRAME_WIDTH;
            Height = FRAME_HEIGHT;
            this.position = GetPosition(row, column);

            dimension = new Vector2(FRAME_WIDTH, FRAME_HEIGHT);
            frames = CreateFrames(dimension, FRAME_ROW, FRAME_COLUMN);
        }

        /// <summary>
        /// An overriding method that draws coin animation on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, position, frames[frameIndex], Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Coin rotates as game world updated
        /// </summary>
        /// <param name="gameTime">GameTime</param>
		public override void Update(GameTime gameTime)
        {
            delayCounter++;

            if (delayCounter > DELAY)
            {
                frameIndex++;

                if (frameIndex > FRAME_ROW * FRAME_COLUMN - 1)
                    frameIndex = 0;

                delayCounter = 0;
            }

            base.Update(gameTime);
        }
    }
}
