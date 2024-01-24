using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using cotf.Assets;
using cotf.Base;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace cotf
{
    public class Tile : Entity
    {
        int i, j;
        Image? texture;
        public bool Occlude { get; set;}
        private static bool init;
        public static Tile? Instance;
        private Tile()
        {
        }
        public Tile(int i, int j, float range, Size size, bool occlude)
        {
            Load();
            name = "Tile";
            active = true;
            Width = size.Width;
            Height = size.Height;
            color = DefaultColor;
            position = new Vector2(i * Width, j * Height);
            alpha = 0f;
            this.range = range;
            this.i = i;
            this.j = j;
            Occlude = occlude;
            texture = Asset<Bitmap>.GetTexture(Texture).Value;
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
        public override string Texture => $"{TexturePrefix}{i}{j}";
        public override Color DefaultColor => Color.Gray;
        public override bool PreUpdate()
        {
            return onScreen =
                position.X < Lib.OutputWidth &&
                position.X >= 0 &&
                position.Y < Lib.OutputHeight &&
                position.Y >= 0;
        }
        public override void Update()
        {
        }
        public override void Draw(Graphics graphics)
        {
            if (active && PreUpdate())
            {
                if (texture == null)
                    return;
                base.PostFX();
                Lightmap map;
                (map = Lib.lightmap[i, j]).Update(this);
                Drawing.TextureLighting(texture, Hitbox, map, this, gamma, alpha, graphics);
            }
        }
        public static Tile GetTile(float x, float y, Size size)
        {
            return Lib.tile[(int)x / size.Width, (int)y / size.Height];
        }
    }
}
