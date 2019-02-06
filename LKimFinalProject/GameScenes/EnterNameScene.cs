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
    // A class of EnterNameScene
    public class EnterNameScene : GameScene
    {
        // Variables
        private SpriteBatch spriteBatch;
        private EnterName e;

        /// <summary>
        /// A constructor for EnterNameScene object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        public EnterNameScene(Game game,
            SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;

            // Loads EnterName object
            SpriteFont font = game.Content.Load<SpriteFont>("fonts/nameFont");
            e = new EnterName(game, spriteBatch, font);
            this.Components.Add(e);
        }
    }

}
