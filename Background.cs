using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
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
        internal Image? texture;
        private static bool init;
        public static Background? Instance;
        private Background()
        {
        }
        public Background(int i, int j, float range, Size size, Margin margin)
        {
            name = "Background";
            this.margin = margin;
            active = true;
            Width = size.Width;
            Height = size.Height;
            color = DefaultColor;
            position = new Vector2(i * Width, j * Height);
            alpha = 0f;
            this.range = range;
            this.i = i;
            this.j = j;
            texture = Asset<Bitmap>.GetTexture(Texture).Value;
        }
        public static void Load()
        {
            if (!init)
            {
                Instance = new Background();
                init = true;
            }
        }
        public override string TexturePrefix => "background";
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
            if (!active || !PreUpdate())
                return;
            if (texture == null)
                return;
            base.PostFX();
            Lightmap map;
            (map = Lib.lightmap[i, j]).Update(this);
            Drawing.TextureLighting(texture, Hitbox, map, this, gamma, alpha, graphics);
        }
    }
}
