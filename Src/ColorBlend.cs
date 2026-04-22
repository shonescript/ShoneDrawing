#if SystemDrawing
namespace System.Drawing;
#else
namespace Shone.Drawing;
#endif

public class ColorBlend
{
    private Color[] colors;

    private float[] positions;

    public Color[] Colors
    {
        get
        {
            return colors;
        }
        set
        {
            colors = value;
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

    public ColorBlend()
    {
        colors = new Color[1];
        positions = new float[1];
    }

    public ColorBlend(int count)
    {
        colors = new Color[count];
        positions = new float[count];
    }
}