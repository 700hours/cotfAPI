using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using cotf.Assets;
using cotf.Base;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;


namespace cotf
{
    public class Background : Entity
    {
        int i, j;
        internal Image texture;
        public Background(int i, int j, Size size, Margin margin)
        {
            name = "Background";
            this.margin = margin;
            active = true;
            Width = size.Width;
            Height = size.Height;
            color = DefaultColor;
            position = new Vector2(i * Width, j * Height);
            alpha = 0f;
            this.i = i;
            this.j = j;
            texture = Asset<Bitmap>.Request(Texture);
        }
        public override string Texture => $"Textures\\background{i}{j}";
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
            if (!active || !onScreen)
                return;
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
        public static Background GetSafely(int i, int j)
        {
            return Lib.background[Math.Max(Math.Min(i, Lib.background.GetLength(0) - 1), 0), Math.Max(Math.Min(j, Lib.background.GetLength(1) - 1), 0)];
        }
    }
}
