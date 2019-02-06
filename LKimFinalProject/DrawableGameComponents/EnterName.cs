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
    // A class of enter name screen
    public class EnterName : DrawableGameComponent
    {
        #region Variables

        // const variables
        private const int INIT_ROW = 2;
        private const int INIT_COL = 2;
        private const int NUMBER_OF_COLS = 12;

        // start point of each element
        private const int TITLE_ROW = 1;
        private const int TITLE_COL = 3;
        private const int NAME_ROW = 6;
        private const int NAME_COL = 3;
        private const int SCORE_ROW = 6;
        private const int SCORE_COL = 7;

        private int gridWidth = Shared.GRID_WIDTH;
        private int gridHeight = Shared.GRID_HEIGHT;

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private string title;
        private string save;
        private char[] alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        private string[] nameChars;
        private string score;
        private string name;

        private Vector2 scorePos;

        private MouseState ms;
        private MouseState oldState; 

        #endregion

        /// <summary>
        /// A constructor for EnterName object
        /// </summary>
        /// <param name="game">Game1</param>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="font">SpriteFont</param>
        public EnterName(Game game,
            SpriteBatch spriteBatch,
            SpriteFont font) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            nameChars = new string[] { "o", "o", "o" };
            name = "";
            title = "Enter Your Name";
            save = "ESC to save and exit";

            scorePos = new Vector2(SCORE_COL * gridWidth, SCORE_ROW * gridHeight);
        }

        /// <summary>
        /// An overriding method that draws alphabets and strings on the game display
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // set position for title and save string
            Vector2 titlePos = new Vector2((Shared.stage.X - font.MeasureString(title).X)/2, TITLE_ROW * gridHeight);
            Vector2 savePos = new Vector2((Shared.stage.X - font.MeasureString(save).X) / 2, (NAME_ROW + 1) *gridHeight);

            // title
            spriteBatch.DrawString(font, title, titlePos, Color.Black);

            // display alphabets on grid
            for (int i = 0; i < alphabets.Length; i++)
            {
                Vector2 position;
                int row = i / NUMBER_OF_COLS + INIT_ROW;
                int col = i % NUMBER_OF_COLS + INIT_COL;
                int width = (int)font.MeasureString(alphabets[i].ToString()).X;
                int height = (int)font.MeasureString(alphabets[i].ToString()).Y;

                position.X = col * gridWidth + (gridWidth - width) / 2;
                position.Y = row * gridHeight + (gridHeight - height) / 2;

                spriteBatch.DrawString(font, alphabets[i].ToString(), position, Color.Black);
            }

            // display placeholder for player name
            for (int i = 0; i < nameChars.Length; i++)
            {
                Vector2 position;
                int row = NAME_ROW;
                int col = NAME_COL + i;
                int width = (int)font.MeasureString(nameChars[i].ToString()).X;
                int height = (int)font.MeasureString(nameChars[i].ToString()).Y;

                position.X = col * gridWidth;
                position.Y = row * gridHeight;

                spriteBatch.DrawString(font, nameChars[i].ToString(), position, Color.Black);

            }

            // display player score
            spriteBatch.DrawString(font, score, scorePos, Color.Black);

            // display save string
            spriteBatch.DrawString(font, save, savePos, Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

		/// <summary>
		/// An overriding method that reads mouse input upon game world updated
		/// </summary>
		/// <param name="gameTime">GameTime</param>
		public override void Update(GameTime gameTime)
        {
            score = string.Format("{0:D8}", Shared.currentScore);
            name = "";

            // get player name by mouse input
            ms = Mouse.GetState();
            
            if (ms.LeftButton == ButtonState.Pressed && ms != oldState)
            {
                int row = ms.Y / gridHeight - INIT_ROW;
                int col = ms.X / gridWidth - INIT_COL;
                int index = row * NUMBER_OF_COLS + col;

                oldState = ms;

                if (index >= 0 && index < alphabets.Length)
                {
                    // replace placeholder("o") with selected character
                    for (int i = 0; i < nameChars.Length; i++)
                    {
                        if (nameChars[i] == "o") 
                        {
                            nameChars[i] = alphabets[index].ToString();
                            break;
                        }
                    }
                }

                foreach (string c in nameChars)
                {
                    name += c;
                }

                Shared.names[Shared.index] = name;
            }

            base.Update(gameTime);
        }
    }
}
