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
        Image texture;
        public bool Occlude { get; set;}
        public Tile(int i, int j, Size size, bool occlude)
        {
            name = "Tile";
            active = true;
            Width = size.Width;
            Height = size.Height;
            color = DefaultColor;
            position = new Vector2(i * Width, j * Height);
            alpha = 0f;
            this.i = i;
            this.j = j;
            Occlude = occlude;
            texture = Asset<Bitmap>.Request(Texture);
        }
        public override string Texture => $"Textures\\tile{i}{j}";
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
            if (active && onScreen)
            {
                if (texture == null)
                    return;
                base.PostFX();
                if (alpha > 0f)
                {
                    Lightmap map;
                    (map = Lib.lightmap[i, j]).Update(this);
                    Drawing.TextureLighting(texture, Hitbox, map, this, gamma, alpha, graphics);
                }
                if (alpha < 1f)
                {
                    alpha += 1f / 10f;
                }
            }
        }
    }
}
