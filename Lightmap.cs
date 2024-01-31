using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using cotfAPI.Base;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace cotfAPI.Base
{
    public class Lightmap : Entity
    {
        static bool init;
        public static Lightmap Instance;
        Entity parent;
        private Lightmap()
        {
        }
        public Lightmap(int i, int j, float range, Size size)
        {
            Load();
            name = $"lightmap {i}:{j}";
            active = true;
            Width = size.Width;
            Height = size.Height;
            color = DefaultColor;
            this.X = i * Width;
            this.Y = j * Height;
            alpha = 0f;
            this.range = range;
        }
        public static void Load()
        {
            if (!init)
            {
                Instance = new Lightmap();
                init = true;
            }
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
