using System;
using NET01_2.Matrices;

namespace NET01_2
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter size of matrix");
            int size = int.Parse(Console.ReadLine() ?? string.Empty);
            
            
            /* Matrices initialization:
                    Square Matrix init by Random numbers(0 - 100)
                    Diagonal Matrix init by their own indices */
            
            SquareMatrix<int> squareMatrix = new SquareMatrix<int>(size);
            DiagonalMatrix<string> diagonalMatrix = new DiagonalMatrix<string>(size, out bool flag);
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    squareMatrix[i, j] = new Random().Next(101);
                    diagonalMatrix[i, j] = $"{i}, {j}";
                }
            }

            Console.WriteLine("Square: " + squareMatrix.ToString());
            Console.WriteLine("Diagonal: " + diagonalMatrix.ToString());
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    squareMatrix[i, j] = new Random().Next(101);
                    diagonalMatrix[i, j] = $"{i}, {j + 1}";
                }
            }

            Console.WriteLine("Square: " + squareMatrix.ToString());
            Console.WriteLine("Diagonal: " + diagonalMatrix.ToString());
        }
    }
}