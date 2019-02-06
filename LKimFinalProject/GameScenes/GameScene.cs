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
    // An abstract class for GameScene
    public abstract class GameScene : DrawableGameComponent
    {
        private List<GameComponent> components;
        public List<GameComponent> Components { get => components; set => components = value; }

        /// <summary>
        /// A constructor for GameScene object
        /// Automatically hides the object
        /// </summary>
        /// <param name="game"></param>
        public GameScene(Game game) : base(game)
        {
            components = new List<GameComponent>();
			Hide();
        }

        /// <summary>
        /// A virtual method that shows the object
        /// </summary>
        public virtual void Show()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        /// <summary>
        /// A virtual method that hides the object
        /// </summary>
        public virtual void Hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

		/// <summary>
		/// An overriding method that reads any update in game world
		/// </summary>
		/// <param name="gameTime">GameTime</param>
		public override void Update(GameTime gameTime)
        {
            foreach (GameComponent item in components)
            {
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// An overriding method that draws object on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent comp = null;

            foreach (GameComponent item in components)
            {
                if (item is DrawableGameComponent)
                {
                    comp = (DrawableGameComponent)item;

                    if (comp.Visible)
                        comp.Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }
    }
}
