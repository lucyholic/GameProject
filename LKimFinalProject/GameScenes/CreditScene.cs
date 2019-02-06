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
    // A class of CreditScene
	class CreditScene : GameScene
	{
        // Variables
		private SpriteBatch spriteBatch;
		private SceneLayer credit;

        /// <summary>
        /// A constructor for CreditScene object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
		public CreditScene(Game game,
			SpriteBatch spriteBatch) : base(game)
		{
			this.spriteBatch = spriteBatch;

            // Load creditscene.png
			Texture2D creditTex = game.Content.Load<Texture2D>("images/creditScene");
			credit = new SceneLayer(game, spriteBatch, creditTex);
			this.Components.Add(credit);
		}
	}
}
