using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using cotf.Base;

namespace cotf
{
    public class Entity
    {
        public bool staticLamp;
        public bool lit;
        public bool onScreen;
        public bool preRendered;
        public bool active;
        public int Width
        {
            get { return size.Width; }
            set { size.Width = value; }
        }
        public int Height
        {
            get { return size.Height; }
            set { size.Height = value; }
        }
        public float X
        {
            get => position.X;
            set => position.X = value;
        }
        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float range;
        public float alpha;
        public string? name;
        public virtual string Texture => "";
        public Color color;
        public Vector2 position;
        public Vector2 Center
        {
            get { return new Vector2(position.X + Width / 2, position.Y + Height / 2); }
            set { position = new Vector2(value.X - Width / 2, value.Y - Height / 2); }
        }
        public Rectangle Hitbox => new Rectangle((int)position.X, (int)position.Y, Width, Height);
        public Margin margin;
        public SizeF scale;
        private Size size;
    }
}
