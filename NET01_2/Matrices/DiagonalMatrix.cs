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
    public class DiagonalMatrix<T> : SquareMatrix<T>, ICloneable
    {
        //Event for track matrix change
        private event MatrixHandler Notify;
        public DiagonalMatrix(int rank) : base(rank)
        {
            if (rank < 0)
            {
                throw new ArgumentException("Invalid size of matrix");
            }

            this.Rank = rank;
            this.Matrix = new T[rank];
        }
        
        public override T this[int i, int j]
        {
            get
            {
                if (i > Rank || j > Rank || i < 0 || j < 0)
                {
                    throw new ArgumentException("Invalid index");
                }

                if (i == j)
                {
                    return Matrix[i];
                }

                return default;
            }
            set
            {
                if (i > Rank || j > Rank || i < 0 || j < 0)
                {
                    throw new ArgumentException("Invalid index");
                }

                if (i == j)
                {
                    Matrix[i] = value;
                }
            }
        }
        

        /* deep clone, Matrix[i].Copy() it is the extension method to Generic copying*/
        public override object Clone()
        {
            T[] newDiagonal = new T[Rank];
            for (int i = 0; i < Rank; i++)
            {
                newDiagonal[i] = Matrix[i].Copy();
            }
            return new DiagonalMatrix<T>(Rank)
            {
                Matrix = newDiagonal,
                Rank = this.Rank
            };
            
        }
        
        public override void MatrixChange(SquareMatrix<T> newDiagonalMatrix)
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
                        Notify?.Invoke($"Changed: diagonal matrix, {i}, {j}");
                    }
                }
            }
        }
        
    }
}