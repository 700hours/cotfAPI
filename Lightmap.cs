using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using cotf.Base;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace cotf.Base
{
    public class Lightmap : Entity
    {
        int i, j;
        static bool init;
        public static Lightmap? Instance;
        Entity? parent;
        private Lightmap()
        {
        }
        public Lightmap(int i, int j, float range, Size size, Margin margin)
        {
            name = "Lightmap";
            active = true;
            Width = size.Width;
            Height = size.Height;
            color = DefaultColor;
            position = new Vector2(i * Width, j * Height);
            alpha = 0f;
            this.range = range;
            this.margin = margin;
            this.i = i;
            this.j = j;
        }
        public static void Load()
        {
            if (!init)
            {
                Instance = new Lightmap();
                init = true;
            }
        }
        public override bool PreUpdate()
        {
            return onScreen =
                position.X < Lib.OutputWidth &&
                position.X >= 0 &&
                position.Y < Lib.OutputHeight &&
                position.Y >= 0;
        }
        public Color Update(Entity ent)
        {
            if (!active)
                return ent.color;
            ent.color = color;
            if (parent == null)
            {
                parent = ent;
            }
            return color;
        }
        public Color InnactiveCheck()
        {
            if (!active)
                return DefaultColor;
            return color;
        }
        public override string ToString()
        {
            return $"Color:{color}, Alpha:{alpha}, Active:{active}";
        }
    }
}
