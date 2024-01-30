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
using FoundationR;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;


namespace cotf
{
    public class Wall : Entity
    {
        public REW texture;
        private static bool init;
        public static Wall Instance;
        private Wall()
        {
        }
        public Wall(int i, int j, float range, Size size)
        {
            name = $"wall {i}:{j}";
            active = true;
            Width = size.Width;
            Height = size.Height;
            color = DefaultColor;
            alpha = 0f;
            this.range = range;
            X = i * Width;
            Y = j * Height;
            texture = Asset<REW>.Load("Textures/tile");
        }
        public override string ToString()
        {
            return name;
        }
        public static void Load()
        {
            if (!init)
            {
                Instance = new Wall();
                init = true;
            }
        }
        public override string TexturePrefix => "wall";
        public override string Texture => $"{TexturePrefix}{(int)X / Width}{(int)Y / Height}";
        public override Color DefaultColor => Color.Gray;
        public override void Update()
        {
        }
    }
}
