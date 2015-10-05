using System.Drawing;
using AForge.Vision.GlyphRecognition;

namespace Glyphs
{
    public static class GlyphExtensions
    {
        public static Bitmap ToImage(this Glyph glyph, int width)
        {
            var bitmap = new Bitmap(width, width, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            var cellSize = width/glyph.Size;
            var glyphSize = glyph.Size;

            for (var i = 0; i < width; i++)
            {
                var yCell = i/cellSize;

                for (var j = 0; j < width; j++)
                {
                    var xCell = j/cellSize;

                    if ((yCell >= glyphSize) || (xCell >= glyphSize))
                    {
                        // set pixel to transparent if it outside of the glyph
                        bitmap.SetPixel(j, i, Color.Transparent);
                    }
                    else
                    {
                        // set pixel to black or white depending on glyph value
                        bitmap.SetPixel(j, i, (glyph.Data[yCell, xCell] == 0) ? Color.Black : Color.White);
                    }
                }
            }

            return bitmap;
        }
    }
}