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
    // A class of StartScene
    public class StartScene : GameScene
    {
        // variables 
        public MenuComponent Menu { get; set; }
        private SpriteBatch spriteBatch;

        private string[] menuItems = { "START GAME", "HELP", "HIGH SCORES", "CREDIT", "QUIT"};

        /// <summary>
        /// A constructor of StartScene object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        public StartScene(Game game,
            SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;

            // menu
            SpriteFont regularFont = game.Content.Load<SpriteFont>("fonts/regularFont");
            SpriteFont highlightFont = game.Content.Load<SpriteFont>("fonts/highlightFont");

            Menu = new MenuComponent(game, spriteBatch, regularFont, highlightFont, menuItems);
            this.Components.Add(Menu);
        }
    }
}
