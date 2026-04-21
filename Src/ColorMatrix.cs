

/// <summary>
/// Mimics the System.Drawing.Imaging.ColorMatrix (a 5x5 matrix for ARGB transformations).
/// Also provides a method to convert to a MewUI color filter.
/// </summary>
public class ColorMatrix
{
    // Internally store a 5x5 float matrix: [row, column].
    private readonly float[,] matrix = new float[5, 5];

    /// <summary>
    /// Creates a new ColorMatrix initialized to the identity matrix.
    /// </summary>
    public ColorMatrix()
    {
        SetIdentity();
    }

    /// <summary>
    /// Creates a new ColorMatrix by copying values from a 5x5 float array.
    /// The array must be at least 5x5 in size.
    /// </summary>
    public ColorMatrix(float[,] values)
    {
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                matrix[r, c] = values[r, c];
            }
        }
    }

    #region Indexer

    /// <summary>
    /// Gets or sets an individual element in the color matrix.
    /// row and column should be in [0..4].
    /// </summary>
    public float this[int row, int column]
    {
        get
        {
            return matrix[row, column];
        }
        set
        {
            matrix[row, column] = value;
        }
    }

    #endregion

    #region Matrix33 Property

    /// <summary>
    /// The element at row=3, column=3 in the 5x5 matrix,
    /// matching System.Drawing.Imaging.ColorMatrix.Matrix33.
    /// This typically affects the alpha channel scaling (in ARGB transforms).
    /// </summary>
    public float Matrix33
    {
        get => matrix[3, 3];
        set => matrix[3, 3] = value;
    }

    #endregion

    #region Identity / Helper

    /// <summary>
    /// Sets this color matrix to the identity matrix.
    /// (This causes no color change if applied.)
    /// </summary>
    public void SetIdentity()
    {
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                matrix[r, c] = (r == c) ? 1f : 0f;
            }
        }
    }

    #endregion

    #region Convert to MewUI Filter

    /// <summary>
    /// Creates a color filter array using the current 5x5 matrix.
    /// This can be used to apply the color transform in drawing operations.
    /// </summary>
    public float[] ToColorFilterArray()
    {
        // MewUI expects a 20-element float array in row-major order for the top 4 rows x 5 columns.
        float[] mewArray = new float[20];
        int index = 0;
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                mewArray[index++] = matrix[r, c];
            }
        }
        return mewArray;
    }

    #endregion

    #region Copy / To2DArray

    /// <summary>
    /// Returns a copy of the internal 5x5 float array for external usage.
    /// </summary>
    public float[,] GetMatrixCopy()
    {
        float[,] copy = new float[5, 5];
        for (int r = 0; r < 5; r++)
        {
            for (int c = 0; c < 5; c++)
            {
                copy[r, c] = matrix[r, c];
            }
        }
        return copy;
    }

    #endregion

    #region Equals / GetHashCode / ToString

    public override bool Equals(object obj)
    {
        if (obj is ColorMatrix cm)
        {
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (matrix[r, c] != cm.matrix[r, c])
                        return false;
                }
            }
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    hash = (hash * 31) + matrix[r, c].GetHashCode();
                }
            }
            return hash;
        }
    }

    public override string ToString()
    {
        return "ColorMatrix(5x5)";
    }

    #endregion
}
