using System;
using System.Text;

namespace NET01_2.Matrices
{
    /// <summary>
    /// Square Matrix
    /// keeps all elements as opposed to Diagonal Matrix
    /// Sizes(Rank) of matrices are equal
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SquareMatrix<T> : ICloneable
    {
        //Event for track matrix change
        private event MatrixHandler Notify;
        
        protected int Rank;
        protected T[] Matrix;

        public SquareMatrix(int rank)
        {
            if (rank < 0)
            {
                throw new ArgumentException("Invalid size of matrix");
            }

            Rank = rank;
            Matrix = new T[rank * rank];
        }

        /* Indexer of Square Matrix:
             - all matrix keeps as a single array
             _matrix[_size * j + i] == _matrix[i,j] */
        public virtual T this[int i, int j]
        {
            get
            {
                if (i > Rank || j > Rank || i < 0 || j < 0)
                {
                    throw new ArgumentException("Invalid index");
                }

                return Matrix[Rank * j + i];
            }
            set
            {
                if (i > Rank || j > Rank || i < 0 || j < 0)
                {
                    throw new ArgumentException("Invalid index");
                }

                Matrix[Rank * j + i] = value;
            }
        }
        
        /* deep clone, Matrix[i].Copy() it is the extension method to Generic copying*/
        public virtual object Clone()
        {
            T[] newMatrix = new T[Rank * Rank];
            for (int i = 0; i < Rank * Rank; i++)
            {
                newMatrix[i] = Matrix[i].Copy();
            }
            return new SquareMatrix<T>(Rank)
            {
                Matrix = newMatrix,
                Rank = Rank
            };
        }

        public override string ToString()
        {
            StringBuilder matrixString = new StringBuilder("Matrix: \n");
            for (int i = 0; i < Rank; i++)
            {
                for (int j = 0; j < Rank; j++)
                {
                    matrixString.Append(this[i,j] + " ");
                }

                matrixString.Append('\n');
            }

            return matrixString.ToString();
        }

        public virtual void MatrixChange(SquareMatrix<T> newDiagonalMatrix)
        {
            Notify += Console.WriteLine;
            for (int i = 0; i < Rank; i++)
            {
                for (int j = 0; j < Rank; j++)
                {
                    if (this[i, j] == null)
                    {
                        continue;
                    }
                    if (!this[i,j].Equals(newDiagonalMatrix[i,j]))
                    {
                        Notify?.Invoke($"Changed: square matrix, {i}, {j}");
                    }
                }
            }
        }
    }
}