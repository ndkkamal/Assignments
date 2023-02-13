using System;
using System.Data;
using System.IO;
using System.Xml;

namespace FISWordSearch
{
    class Program
    {
        static char[,] Grid = new char[,] {
            {'C', 'P', 'K', 'X', 'O', 'I', 'G', 'H', 'S', 'F', 'C', 'H'},
            {'Y', 'G', 'W', 'R', 'I', 'A', 'H', 'C', 'Q', 'R', 'X', 'K'},
            {'M', 'A', 'X', 'I', 'M', 'I', 'Z', 'A', 'T', 'I', 'O', 'N'},
            {'E', 'T', 'W', 'Z', 'N', 'L', 'W', 'G', 'E', 'D', 'Y', 'W'},
            {'M', 'C', 'L', 'E', 'L', 'D', 'N', 'V', 'L', 'G', 'P', 'T'},
            {'O', 'J', 'A', 'A', 'V', 'I', 'O', 'T', 'E', 'E', 'P', 'X'},
            {'C', 'D', 'B', 'P', 'H', 'I', 'A', 'W', 'V', 'X', 'U', 'I'},
            {'L', 'G', 'O', 'S', 'S', 'B', 'R', 'Q', 'I', 'A', 'P', 'K'},
            {'E', 'O', 'I', 'G', 'L', 'P', 'S', 'D', 'S', 'F', 'W', 'P'},
            {'W', 'F', 'K', 'E', 'G', 'O', 'L', 'F', 'I', 'F', 'R', 'S'},
            {'O', 'T', 'R', 'U', 'O', 'C', 'D', 'O', 'O', 'F', 'T', 'P'},
            {'C', 'A', 'R', 'P', 'E', 'T', 'R', 'W', 'N', 'G', 'V', 'Z'}
        };



        static string[] Words = new string[]
        {
            "CARPET",
            "CHAIR",
            "DOG",
            "BALL",
            "DRIVEWAY",
            "FISHING",
            "FOODCOURT",
            "FRIDGE",
            "GOLF",
            "MAXIMIZATION",
            "PUPPY",
            "SPACE",
            "TABLE",
            "TELEVISION",
            "WELCOME",
            "WINDOW"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Word Search");

            for (int y = 0; y < 12; y++)
            {
                //Console.Write(y+ " ");
                for (int x = 0; x < 12; x++)
                {
                    
                    Console.Write(Grid[y, x]);
                    Console.Write(' ');
                }
                Console.WriteLine("");

            }

            Console.WriteLine("");
            Console.WriteLine("Found Words");
            Console.WriteLine("------------------------------");

            FindWords();

            Console.WriteLine("------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }

        private static void FindWords()
        {
            //Find each of the words in the grid, outputting the start and end location of each word, e.g.
            //PUPPY found at (10,7) to (10, 3) 


            int rows = Grid.GetLength(0);
            int columns = Grid.GetLength(1);

            int xVal = 0, yVal = 0;

            foreach (string word in Words)
            {
                bool isFound = false;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (Grid[i, j] == word[0])
                        {


                            int[,] directions = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };


                            for (int x=0; x < directions.GetLength(0); x++)
                            {                                
                                for (int y = 0; y < 2; y++) {

                                    if (y== 0) xVal = directions[x, 0];
                                    if (y == 1) yVal = directions[x, 1];
                                }
                                int k = 1;

                                int r = i + xVal;
                                int c = j + yVal;
                                while (r >= 0 && r < rows && c >= 0 && c < columns && k < word.Length && Grid[r, c] == word[k])
                                {
                                    r += xVal;
                                    c += yVal;
                                    k++;
                                }
                                if (k == word.Length)
                                {
                                    Console.WriteLine("Word '" + word + "' found at (" + (i + 1) + "," + (j + 1) + ") to (" + (r - xVal + 1) + "," + (c - yVal + 1) + ")");
                                    isFound = true;
                                    break;
                                }
                            }

                            if (isFound)
                            {
                                break;
                            }
                        }
                    }
                    if (isFound)
                    {
                        break;
                    }
                }
            }

                    }
    }
}
