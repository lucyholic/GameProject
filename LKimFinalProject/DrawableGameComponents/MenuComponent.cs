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
    // A class of MenuComponent
    public class MenuComponent : DrawableGameComponent
    {
        #region Variables

        private SpriteBatch spriteBatch;
        private SpriteFont regularFont;
        private SpriteFont highlightFont;
        private List<string> menuItems;

        public int SelectedIndex { get; set; } = 0;

        private Vector2 position;
        private Color regularColor = Color.SlateGray;
        private Color highlightColor = Color.Tomato;

        private KeyboardState oldState;

        #endregion

        /// <summary>
        /// A constructor for Menucomponent object
        /// </summary>
        /// <param name="game">Game</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="regularFont">SpriteFont for not selected item</param>
        /// <param name="highlightFont">SpriteFont for selected item</param>
        /// <param name="menus">Array of menu items</param>
        public MenuComponent(Game game,
            SpriteBatch spriteBatch, 
            SpriteFont regularFont, 
            SpriteFont highlightFont,
            string[] menus) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.regularFont = regularFont;
            this.highlightFont = highlightFont;
            this.menuItems = menus.ToList<string>();

            this.position = new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2);
        }

		/// <summary>
		/// AAn overriding method that reads keyboard inputs and selects the item
		/// </summary>
		/// <param name="gameTime">GameTime</param>
		public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                SelectedIndex++;

                if (SelectedIndex >= menuItems.Count)
                {
                    SelectedIndex = 0;
                }
            }

            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                SelectedIndex--;

                if (SelectedIndex < 0)
                {
                    SelectedIndex = menuItems.Count - 1;
                }
            }

            oldState = ks;
            base.Update(gameTime);
        }

        /// <summary>
        /// An overriding method that draws menu components on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            Vector2 tempPos = position;

            spriteBatch.Begin();

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (SelectedIndex == i)
                {
                    Vector2 size = highlightFont.MeasureString(menuItems[i]);
                    tempPos.X = (Shared.stage.X - size.X) / 2;
                    spriteBatch.DrawString(highlightFont, menuItems[i], tempPos, highlightColor);
                    tempPos.Y += highlightFont.LineSpacing;
                }

                else
                {
                    Vector2 size = regularFont.MeasureString(menuItems[i]);
                    tempPos.X = (Shared.stage.X - size.X) / 2;
                    spriteBatch.DrawString(regularFont, menuItems[i], tempPos, regularColor);
                    tempPos.Y += regularFont.LineSpacing;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
