using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using cotf.Base;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace cotf
{
    public class Lamp : Entity
    {
        public int owner = 255;
        public bool staticLamp;
        private Lamp(float range)
        {
            this.range = range;
            this.active = true;
        }
        public override bool PreUpdate()
        {
            return onScreen =
                position.X < Lib.OutputWidth &&
                position.X >= 0 &&
                position.Y < Lib.OutputHeight &&
                position.Y >= 0;
        }
        public static Color RandomLight()
        {
            int len = Enum.GetNames<KnownColor>().Length;
            KnownColor c = Enum.Parse<KnownColor>(Enum.GetNames(typeof(KnownColor))[Lib.rand.Next(len)]);
            return Color.FromKnownColor(c);
        }
        public static int AddLamp(Lamp lamp)
        {
            int num = Lib.lamp.Length - 1;
            for (int i = 0; i < Lib.lamp.Length; i++)
            {
                if (Lib.lamp[i] == null || !Lib.lamp[i].active)
                {
                    num = i;
                    break;
                }
            }
            Lib.lamp[num] = lamp;
            Lib.lamp[num].whoAmI = num;
            return num;
        }
        public static int NewLamp(float x, float y, float range, Color color, bool staticLamp = false, int owner = 255)
        {
            int num = Lib.lamp.Length - 1;
            for (int i = 0; i < Lib.lamp.Length; i++)
            {
                if (Lib.lamp[i] == null || !Lib.lamp[i].active)
                {
                    num = i;
                    break;
                }
            }
            Lib.lamp[num] = new Lamp(range);
            Lib.lamp[num].active = true;
            Lib.lamp[num].position = new Vector2(x, y);
            Lib.lamp[num].range = range;
            Lib.lamp[num].whoAmI = num;
            Lib.lamp[num].owner = owner;
            Lib.lamp[num].color = color;
            Lib.lamp[num].staticLamp = staticLamp;
            return num;
        }
    }
}
