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
    // A class of HelpScene
    public class HelpScene : GameScene
    {
        // Variables
        private SpriteBatch spriteBatch;
        private SceneLayer help;

        /// <summary>
        /// A constructor for HelpScene object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        public HelpScene(Game game,
            SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;

            Texture2D helpTex = game.Content.Load<Texture2D>("images/helpscene");
            help = new SceneLayer(game, spriteBatch, helpTex);
            this.Components.Add(help);

        }
    }
}
