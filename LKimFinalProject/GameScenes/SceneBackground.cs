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

namespace LKimFinalProject
{
    // A class of scene background class
    public class SceneBackground : GameScene
    {
        // Variables
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position1;
        private Vector2 position2;
        private Vector2 speed;

        /// <summary>
        /// A constructor for SceneBackground object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="tex">Background texture</param>
        /// <param name="position">Postion of first texture</param>
        public SceneBackground(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position1 = position;
            this.position2 = new Vector2(position1.X + tex.Width, position.Y);
            this.speed = new Vector2(2, 0);
        }

        /// <summary>
        /// An overriding method that draws scrolling background on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, position1, Color.White);
            spriteBatch.Draw(tex, position2, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// An overriding method that checks game world time and makes background scrolling
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            position1 -= speed;
            position2 -= speed;

            if (position1.X < -tex.Width)
            {
                position1.X = position2.X + tex.Width;
            }

            if (position2.X < -tex.Width)
            {
                position2.X = position1.X + tex.Width;
            }

            base.Update(gameTime);
        }
    }
}
