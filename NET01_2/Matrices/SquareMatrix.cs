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
    public class SquareMatrix<T>
    {
        //Event for track matrix change
        private delegate void MatrixHandler(string message);
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

        protected SquareMatrix(int rank, bool flag)
        {
            if (rank < 0)
            {
                throw new ArgumentException("Invalid size of matrix");
            }

            Rank = rank;
        }

        /* Indexer of Square Matrix:
             - all matrix keeps as a single array
             _matrix[_size * j + i] == _matrix[i,j] */
        public virtual T this[int i, int j]
        {
            get
            {
                IndexCheck(i, j);
                return Matrix[Rank * j + i];
            }
            set
            {
                IndexCheck(i, j);
                MatrixChange(value, i, j);
                Matrix[Rank * j + i] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder matrixString = new StringBuilder("Matrix: \n");
            for (int i = 0; i < Rank; i++)
            {
                for (int j = 0; j < Rank; j++)
                {
                    matrixString.Append(this[i, j] + " ");
                }

                matrixString.Append('\n');
            }

            return matrixString.ToString();
        }

        protected virtual void MatrixChange(T value, int i, int j)
        {
            Notify += Console.WriteLine;
            if (!value.Equals(this[i, j]))
            {
                Notify?.Invoke($"Changed: square matrix, {i}, {j}");
            }
        }

        protected void IndexCheck(int i, int j)
        {
            if (i >= Rank || j >= Rank || i < 0 || j < 0)
            {
                throw new ArgumentException("Invalid index");
            }
        }
    }
}