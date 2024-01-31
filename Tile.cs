using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using cotfAPI.Assets;
using cotfAPI.Base;
using FoundationR;
using REW = FoundationR.REW;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using System.Windows.Media;

namespace cotfAPI
{
    public class Tile : Entity
    {
        public REW texture;
        public bool occlude { get; private set;}
        private static bool init;
        public static Tile Instance;
        private Tile()
        {
        }
        public Tile(int i, int j, float range, Size size, bool occlude)
        {
            Load();
            name = $"tile {i}:{j}";
            active = true;
            Width = size.Width;
            Height = size.Height;
            color = DefaultColor;
            alpha = 0f;
            this.range = range;
            this.occlude = occlude;
            texture = REW.Create(Width, Height, Color.GhostWhite, PixelFormats.Bgr32);
        }
        public override string ToString()
        {
            return name;
        }
        public static void Load()
        {
            if (!init)
            {
                Instance = new Tile();
                init = true;
            }
        }
        public override string TexturePrefix => "tile";
        public override string Texture => $"{TexturePrefix}{(int)X / Width}{(int)Y / Height}";
        public override Color DefaultColor => Color.Gray;
        public override void Update()
        {
        }
    }
}
