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
        Entity? parent;
        public Lightmap(int i, int j, Size size, Margin margin)
        {
            name = "Lightmap";
            active = true;
            Width = size.Width;
            Height = size.Height;
            color = DefaultColor;
            position = new Vector2(i * Width, j * Height);
            alpha = 0f;
            this.margin = margin;
            this.i = i;
            this.j = j;
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
        public void LampEffect(Lamp lamp)
        {
            if (!PreUpdate())
                return;
            float num = 0;
            if (!onScreen || !active)
                return;
            if (lamp.owner == 255 && parent != null && parent.GetType() == typeof(Background))
            {
                num = Helper.RangeNormal(lamp.Center, this.Center, range);
                if (num == 0f)
                    return;
                AdjustColor(num, lamp);
                return;
            }
            if (parent != null && !Helper.SightLine(lamp.Center, parent, margin.Right / 5))
                return;
            num = Helper.RangeNormal(lamp.Center, this.Center, range);
            if (num == 0f)
                return;
            AdjustColor(num, lamp);
        }
        private void AdjustColor(float num, Lamp lamp)
        {
            alpha = 0f;
            alpha += Math.Max(0, num);
            alpha = Math.Min(alpha, 1f);
            color = Ext.AdditiveV2(color, lamp.color, num / 2f);
        }
        public override string ToString()
        {
            return $"Color:{color}, Alpha:{alpha}, Active:{active}";
        }
    }
}
