using AForge.Vision.GlyphRecognition;

namespace Glyphs
{
    class GlyphDatabaseBuilder
    {
        public GlyphDatabase Database { get; private set; }

        public GlyphDatabaseBuilder()
        {
            Database = new GlyphDatabase(5);

            // 1
            Database.Add(new Glyph("Кролик", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 1, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 0, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 2
            Database.Add(new Glyph("Слон", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 0, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 3
            Database.Add(new Glyph("Белка", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 1, 0, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 4
            Database.Add(new Glyph("Динозавр", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 1, 0, 1, 0},
                {0, 1, 1, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 5
            Database.Add(new Glyph("Корова", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 6
            Database.Add(new Glyph("Змея", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 0, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 0, 0, 0, 0}
            }));

            // 7
            Database.Add(new Glyph("Выхухоль", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 1, 0, 0},
                {0, 1, 1, 0, 0},
                {0, 0, 0, 0, 0}
            }));


            // 8
            Database.Add(new Glyph("Дельфин", new byte[,]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 0, 0, 1, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 0, 0}
            }));
        }
    }
}
