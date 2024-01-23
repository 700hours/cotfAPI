using cotf.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace cotf
{
    public sealed class Lib
    {
        internal static string TexturePath = "";
        internal static Size size;
        public static int OutputWidth;
        public static int OutputHeight;
        public static Lightmap[,] lightmap = new Lightmap[,] { };
        public static Tile[,] tile = new Tile[,] { };
        public static Background[,] background = new Background[,] { };
        public static Lamp[] lamp = new Lamp[] { };
        public static List<Texture> texture = new List<Texture>();
        public static rand rand = new rand();
        public static void SetDimensions(int width, int height)
        {
            OutputWidth = width;
            OutputHeight = height;
        }
        public static void Initialize(int lampNum, Size tileSize)
        {
            size = tileSize;
            int width = OutputWidth;
            int height = OutputHeight;
            Background.Load();
            Lamp.Load();
            Lightmap.Load();
            Tile.Load();
            lightmap = new Lightmap[width / tileSize.Width, height / tileSize.Height];
            tile = new Tile[width / tileSize.Width, height / tileSize.Height];
            background = new Background[width / tileSize.Width, height / tileSize.Height];
            lamp = new Lamp[lampNum];
            InitArray(width, height, tileSize);
        }
        static void InitArray(int width, int height, Size tileSize)
        {
            float range = 300f;
            for (int m = 0; m < width; m += tileSize.Width)
            {
                for (int n = 0; n < height; n += tileSize.Height)
                {
                    int i = m / tileSize.Width;
                    int j = n / tileSize.Height;
                    background[i, j] = new Background(i, j, range, tileSize, new Margin(tileSize.Width));
                    tile[i, j] = new Tile(i, j, range, tileSize, true);
                    lightmap[i, j] = new Lightmap(i, j, range, tileSize, new Margin(tileSize.Width));
                }
            }
        }
        public static void UpdateLampMaps(int size)
        {
            foreach (Lamp lamp in Lib.lamp)
            {
                if (lamp == null || !lamp.active) 
                    continue;
                int radius = (int)lamp.range / 2;
                for (int i = (int)lamp.Center.X - radius; i <= lamp.Center.X + radius; i += size)
                {
                    for (int j = (int)lamp.Center.Y - radius; j <= lamp.Center.Y + radius; j += size)
                    {
                        int x = Math.Min(Math.Max(i / size, 0), Lib.OutputWidth / size - 1);
                        int y = Math.Min(Math.Max(j / size, 0), Lib.OutputHeight / size - 1);
                        Lib.lightmap[x, y].LampEffect(lamp);
                    }
                }
            }
        }
        public static void Render(ref Image input)
        {
            UpdateLampMaps(size.Width);
            using (Graphics g = Graphics.FromImage(input))
            {
                LightPass.PreProcessing();
                foreach (var item in Lib.background)
                {
                    item?.Draw(g);
                }
                foreach (var item in Lib.tile)
                {
                    item?.Draw(g);
                }
            }
        }
        public static void UnloadAll()
        {
            TexturePath = "";
            size = Size.Empty;
            OutputWidth = 0;
            OutputHeight = 0;
            lightmap = null;
            tile = null;
            background = null;
            lamp = null;
            Background.Instance = null;;
            Lightmap.Instance = null;
            Tile.Instance = null;
            Lamp.Instance = null;
            texture.Clear();
        }
    }
}
