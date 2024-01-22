using cotf.Base;
using cotf.World;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace cotf
{
    public class Lib
    {
        internal static int OutputWidth;
        internal static int OutputHeight;
        internal static Lightmap[,] lightmap = new Lightmap[,] { };
        internal static Tile[,] tile = new Tile[,] { };
        internal static Bitmap[,] texture = new Bitmap[,] { };
        internal static Background[,] background = new Background[,] { };
        internal static Lamp[] lamp = new Lamp[81];
        internal static string TexturePath = "";
        public static rand rand = new rand();
        internal void Initialize(int width, int height, Size tileSize)
        {
            lightmap = new Lightmap[width / tileSize.Width, height / tileSize.Height];
            tile = new Tile[width / tileSize.Width, height / tileSize.Height];
            texture = new Bitmap[width / tileSize.Width, height / tileSize.Height];
            background = new Background[width / tileSize.Width, height / tileSize.Height];
            if (!Directory.Exists(TexturePath = Path.Combine(Directory.GetCurrentDirectory(), "Textures")))
            {
                Directory.CreateDirectory(TexturePath);
            }
            OutputWidth = width;
            OutputHeight = height;
        }
        public Tile[,] CreateLayout()
        {

        }
    }
}
