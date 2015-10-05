using System.Collections.Generic;
using System.Drawing;
using AForge;
using AForge.Math.Geometry;
using AForge.Vision.GlyphRecognition;

namespace Glyphs
{
    public class GlyphImageProcessor
    {
        #region Settings 

        private readonly Font _defaultFont = new Font(FontFamily.GenericSerif, 15, FontStyle.Bold);

        #endregion

        #region Ctor

        private readonly object _sync = new object();

        private readonly GlyphRecognizer _recognizer;
        private readonly GlyphTracker _glyphTracker = new GlyphTracker();

        public GlyphImageProcessor(GlyphDatabase database)
        {
            _recognizer = new GlyphRecognizer(database);
        }

        #endregion

        #region Helper methods

        // Convert list of AForge.NET framework's points to array of .NET's points
        private static System.Drawing.Point[] ToPointsArray(IReadOnlyList<IntPoint> points)
        {
            var count = points.Count;
            var pointsArray = new System.Drawing.Point[count];

            for (var i = 0; i < count; i++)
                pointsArray[i] = new System.Drawing.Point(points[i].X, points[i].Y);

            return pointsArray;
        }

        #endregion

        // Process image searching for glyphs and highlighting them
        public List<ExtractedGlyphData> ProcessImage(Bitmap image)
        {
            var glyphs = new List<ExtractedGlyphData>();

            lock (_sync)
            {
                _glyphTracker.ImageSize = image.Size;

                // get list of recognized glyphs
                glyphs.AddRange(_recognizer.FindGlyphs(image));
                if (glyphs.Count <= 0) return glyphs;

                // visualizing glyphs
                var glyphIDs = _glyphTracker.TrackGlyphs(glyphs);

                var g = Graphics.FromImage(image);
                var i = 0;

                // highlight each found glyph
                foreach (var glyphData in glyphs)
                {
                    var glyphPoints = (glyphData.RecognizedGlyph == null)
                        ? glyphData.Quadrilateral
                        : glyphData.RecognizedQuadrilateral;

                    var pen = new Pen(Color.Red);

                    // highlight border
                    g.DrawPolygon(pen, ToPointsArray(glyphPoints));

                    // prepare glyph's title
                    var glyphTitle = glyphData.RecognizedGlyph != null
                        ? glyphData.RecognizedGlyph.Name
                        : string.Format("ID: {0}", glyphIDs[i]);

                    // show glyph's title
                    if (!string.IsNullOrEmpty(glyphTitle))
                    {
                        // get glyph's center point
                        IntPoint minXY, maxXY;
                        PointsCloud.GetBoundingRectangle(glyphPoints, out minXY, out maxXY);
                        var center = (minXY + maxXY)/2;

                        // glyph's name size
                        var nameSize = g.MeasureString(glyphTitle, _defaultFont);

                        // paint the name
                        var brush = new SolidBrush(pen.Color);

                        g.DrawString(glyphTitle, _defaultFont, brush,
                            new System.Drawing.Point(center.X - (int) nameSize.Width/2,
                                center.Y - (int) nameSize.Height/2));

                        brush.Dispose();
                    }

                    i++;
                    pen.Dispose();
                }
            }

            return glyphs;
        }

        // Reset glyph processor to initial state
        public void Reset()
        {
            _glyphTracker.Reset();
        }
    }
}
