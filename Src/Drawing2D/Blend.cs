#if SystemDrawing
namespace System.Drawing.Drawing2D;
#else
namespace Shone.Drawing.Drawing2D;
#endif

public class Blend
{
    private float[] factors;

    private float[] positions;

    public float[] Factors
    {
        get
        {
            return factors;
        }
        set
        {
            factors = value;
        }
    }

    public float[] Positions
    {
        get
        {
            return positions;
        }
        set
        {
            positions = value;
        }
    }

    public Blend()
    {
        factors = new float[1];
        positions = new float[1];
    }

    public Blend(int count)
    {
        factors = new float[count];
        positions = new float[count];
    }
}