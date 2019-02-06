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
    // An abstract class of game object
    public abstract class GameObject : DrawableGameComponent
    {
        #region Variables

        private const int COLLISION_MARGIN = 1;
        private int speed;
        private int width;
        private int height;

        private SpriteEffects effect;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public int Speed { get => speed; set => speed = value; }
        public SpriteEffects Effect { get => effect; set => effect = value; } 
        
        #endregion

        /// <summary>
        /// A constructor for GameObject class
        /// </summary>
        /// <param name="game">GameTime</param>
        public GameObject(Game game) : base(game)
        {
            this.speed = 0;
            this.width = 0;
            this.height = 0;
        }

        /// <summary>
        ///  A method that grabs rectangle of object
        /// </summary>
        /// <returns>Rectangle of object</returns>
		public virtual Rectangle GetRectangle(Vector2 position)
        {
            return new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        /// <summary>
        /// A method that returns a bigger rectangle of object to detect collision
        /// </summary>
        /// <returns>1 pixel bigger rectangle of object</returns>
		public virtual Rectangle GetCollisionRect(Vector2 position)
        {
            return new Rectangle((int)position.X - COLLISION_MARGIN, (int)position.Y - COLLISION_MARGIN,
                width + COLLISION_MARGIN * 2, height + COLLISION_MARGIN * 2);
        }

        /// <summary>
        /// A method that creates frames from animation sheet
        /// </summary>
        /// <param name="dimension">Dimension of frame</param>
        /// <param name="row">Number of frames in a rows</param>
        /// <param name="column">Number of frames in a columns</param>
        /// <returns>List of frames</returns>
        public List<Rectangle> CreateFrames(Vector2 dimension, int row, int column)
        {
            List<Rectangle> frames = new List<Rectangle>();

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    int x = j * (int)dimension.X;
                    int y = i * (int)dimension.Y;

                    Rectangle r = new Rectangle(x, y, (int)dimension.X, (int)dimension.Y);
                    frames.Add(r);
                }
            }

            return frames;
        }

        /// <summary>
        /// A method that returns vector2 as position from row and column of the object
        /// </summary>
        /// <param name="row">The row of the object in grid</param>
        /// <param name="column">The column of the object in grid</param>
        /// <returns>Position of the object</returns>
		public Vector2 GetPosition(int row, int column)
        {
            Vector2 pos;

            pos.X = column * Shared.GRID_WIDTH + (Shared.GRID_WIDTH - width) / 2;
            pos.Y = row * Shared.GRID_HEIGHT + (Shared.GRID_HEIGHT - height) / 2;

            return pos;
        }

        /// <summary>
        /// A method that makes object flip
        /// </summary>
        public void Flip()
        {
            if (speed < 0)
                effect = SpriteEffects.FlipHorizontally;
            else
                effect = SpriteEffects.None;
        }

        /// <summary>
        /// A method that makes object switch its direction 
        /// </summary>
        public void ChangeDirection()
        {
            speed = -speed;
            Flip();
        }

    }
}
