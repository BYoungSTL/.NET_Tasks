using System;
using System.Text;

namespace NET01_2.Matrices
{
    /// <summary>
    /// Diagonal matrix:
    /// all elements that are not on the main diagonal (i==j) have a default value
    /// keeps only main diagonal, all other elements doesn't keep
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DiagonalMatrix<T> : SquareMatrix<T>
    {
        //Event for track matrix change
        private event MatrixHandler Notify;

        private delegate void MatrixHandler(string message);

        public DiagonalMatrix(int rank, out bool flag) : base(rank, flag = true)
        {
            if (rank < 0)
            {
                throw new ArgumentException("Invalid size of matrix");
            }

            Rank = rank;
            Matrix = new T[rank];
        }

        public override T this[int i, int j]
        {
            get
            {
                IndexCheck(i, j);
                return i == j ? Matrix[i] : default;
            }
            set
            {
                IndexCheck(i, j);
                if (i == j)
                {
                    MatrixChange(value, i, j);
                    Matrix[i] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Invalid index(i isn't equal j)");
                }
            }
        }

        protected override void MatrixChange(T value, int i, int j)
        {
            Notify += Console.WriteLine;

            if (i != j) return;
            if (!value.Equals(this[i, j]))
            {
                Notify?.Invoke($"Changed: diagonal matrix, {i}, {j}");
            }
        }
    }
}