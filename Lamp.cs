using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using cotfAPI.Base;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using cotfAPI;

namespace cotfAPI
{
    public class Lamp : Entity
    {
        static bool init;
        public static Lamp Instance;
        public int owner = 255;
        public bool staticLamp;
        private Lamp()
        {
        }
        public Lamp(float x, float y, float range, Color color)
        {
            Load();
            this.range = range;
            this.X = x;
            this.Y = y;
            this.color = color;
            this.active = true;
        }
        public static void Load()
        {
            if (!init)
            {
                Instance = new Lamp();
                init = true;
            }
        }
        public static Color RandomLight()
        {
            int len = Enum.GetNames(typeof(KnownColor)).Length;
            KnownColor c = (KnownColor)Enum.Parse(typeof(KnownColor), Enum.GetNames(typeof(KnownColor))[new rand().Next(len)]);
            return Color.FromKnownColor(c);
        }
    }
}
