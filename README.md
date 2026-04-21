# ShoneDrawing
This project aims to produce a MewUI based crossplatform implementation Compatible with System.Drawing.Common. Many codes forks from SkiaDrawing, replacing all Skia canvas related drawing to MewUI IGraphicsContext implementations. <br>

This is an implementation of the methods of System.Drawing in MewUI. It is a work in progress and is not yet complete. The goal is to provide a drop-in replacement for System.Drawing that uses MewUI graphics crossplatform backends for rendering.

# Gloal
- Performance: should boost by 10~100 times when using Direct2D backend on windows platform.
- Crossplatform: support all X11, OpenGL etc. backends by MewUI.
- Compatability: implements a commonly used subset of interfaces of System.Drawing.Common.
- publish: one code base, two dlls for different namespace using.<br>
  （1）Shone.System.Drawing.dll: keep System.Drawing namespace in direct replacement for System.Drawing.Common.dll. No codes need changed, but may conflict with other dlls such as winforms.<br>
  （2）Shone.Drawing.dll: use different Shone.Drawing namespace. Need some namespace relected codes changed.
  
## Usage

To use this library, you need to add a reference to the ShoneDrawing and MewUI projects in your project. Then, you can use the ShoneDrawing or System.Drawing (in direct portable use case) namespace in your code.

It is meant to be a drop-in replacement for System.Drawing, so you can use it in the same way. For example, you can create a new bitmap like this:

```csharp

using Sysem.Drawing;  //when references Shone.System.Drawing.dll
//using Shone.Drawing; //when references Shone.Drawing.dll

Bitmap bitmap = new Bitmap(100, 100);

```

You can then use the Graphics class to draw on the bitmap:

```csharp

using (Graphics g = Graphics.FromImage(bitmap))
{
    g.Clear(Color.White);
    g.DrawLine(Pens.Black, 0, 0, 100, 100);
}

```

You can also use the Graphics class to draw on a control:

```csharp

using (Graphics g = e.Graphics)
{
    g.Clear(Color.White);
    g.DrawLine(Pens.Black, 0, 0, 100, 100);
}

```

## Supported Features

The following features are currently supported:

- Bitmap
- Color
- Graphics
- Pen
- Brush
- Font
- StringFormat
- Rectangle
- Point
- Size
- Image
- ImageFormat

The following features are not yet supported:

- Region
- Path
- Matrix

## No intended features
- ScreenCapture
