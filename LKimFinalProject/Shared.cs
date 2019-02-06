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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LKimFinalProject
{
    // A class of Shared
	public class Shared
	{
        #region Static variables

        // Stage
        public static Vector2 stage;

        // virtual grid on the screen
        public const int GRID_ROWS = 9;
        public const int GRID_COLUMNS = 16;
        public const int GRID_WIDTH = 90;
        public const int GRID_HEIGHT = 90;
        public Vector2[,] grids = new Vector2[GRID_ROWS, GRID_COLUMNS];

        // numbers are used for mapMarkers
        public enum GridType
        {
            none = 0,
            ground = 1,
            coin = 2,
            chest = 3
        }

        public static int[,] mapMarkers = new int[GRID_ROWS, GRID_COLUMNS]
        {
            { 0, 2, 2, 0, 2, 2, 2, 0, 0, 2, 2, 2, 0, 0, 3, 0 },
            { 0, 1, 1, 0, 0, 0, 0, 2, 2, 0, 0, 0, 1, 1, 0, 2 },
            { 0, 2, 2, 2, 0, 0, 1, 1, 0, 1, 0, 2, 2, 2, 2, 0 },
            { 2, 0, 0, 2, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1 },
            { 1, 1, 1, 2, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 2, 2, 2, 0, 0, 1, 1, 1, 1, 0, 2, 2, 2, 2, 0 },
            { 0, 1, 1, 1, 2, 2, 0, 0, 0, 0, 2, 2, 1, 1, 1, 0 },
            { 0, 0, 0, 2, 0, 0, 2, 0, 0, 2, 0, 0, 2, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1 }
        };

        // Variables
        private const string FILENAME = "scores.txt";
        private const string PLACEHOLDER = "ooo";
        public static int level = 1;
        public static int[] scores = { 0, 0, 0, 0, 0 };
        public static string[] names = { PLACEHOLDER, PLACEHOLDER, PLACEHOLDER, PLACEHOLDER, PLACEHOLDER };
        public static int highScore;
        public static int currentScore;
        public static int index;
        public static bool isNextLevel;
        public static bool isHighScore;
        
        #endregion

        /// <summary>
        /// A method that reads scores from file and puts them in an array
        /// </summary>
        public static void ReadScores()
		{
            StreamReader reader;
			FileInfo file = new FileInfo(FILENAME);

			if (!file.Exists)
			{
				File.Create(FILENAME);
			}

            try
            {
                reader = new StreamReader(FILENAME);

                int i = 0;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] temp = line.Split(null);

                    if (line != "")
                    {
                        names[i] = temp[0];
                        scores[i] = int.Parse(temp[1]);
                    }

                    i++;
                }
            }
            catch (Exception)
            {
                return;
            }

            if(reader != null)
			    reader.Close();
		}

        /// <summary>
        /// A method that writes scores on file
        /// </summary>
		public static void RecordScores()
		{
            index = -1;

            StreamWriter writer;
			FileInfo file = new FileInfo(FILENAME);

			if (!file.Exists)
			{
				File.Create(FILENAME);
			}

            try
            {
                writer = new StreamWriter(FILENAME);

                for (int i = 0; i < scores.Length; i++)
                {
                    string line = $"{names[i]} {scores[i]}";
                    writer.WriteLine(line);
                }
            }
            catch (Exception)
            {
                return;
            }

            if(writer != null)
			    writer.Close();
		}

        /// <summary>
        /// A method that gets highest score from array
        /// </summary>
		public static void GetHighScore()
		{
			foreach (int score in scores)
			{
				if (score > highScore)
					highScore = score;
			}
		}

        /// <summary>
        /// A method that checks if new score hits top 5
        /// </summary>
        /// <param name="score">Player score</param>
        /// <returns>Index of new score in array. Returns -1 if it's not top 5</returns>
		public static int SortList(int score)
		{
            string name = PLACEHOLDER;

			for (int i = 0; i < scores.Length; i++)
			{
				if (scores[i] < score)
				{
					for (int j = scores.Length - 1; j > i; j--)
					{
						scores[j] = scores[j - 1];
						names[j] = names[j - 1];
					}

					scores[i] = score;
                    names[i] = name;

					return i;
				}
			}

			return -1;
		}
	}
}
