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
        public bool lit;
        public bool onScreen;
        public bool preRendered;
        public bool active;
        public bool inShadow;
        public int whoAmI;
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
        public float gamma;
        public string? name;
        public Color color;
        public Vector2 position;
        public Vector2 Center
        {
            get { return new Vector2(position.X + Width / 2, position.Y + Height / 2); }
            set { position = new Vector2(value.X - Width / 2, value.Y - Height / 2); }
        }
        public Rectangle Hitbox => new Rectangle((int)position.X, (int)position.Y, Width, Height);
        public SizeF scale;
        private Size size;
        public virtual string TexturePrefix => "";
        public virtual string Texture => "";
        public virtual Color DefaultColor => Color.FromArgb(255, 20, 20, 20);
        public virtual bool PreUpdate() => true;
        public virtual void Update() { }
        public virtual bool PreDraw(Graphics graphics) => true;
        public virtual void Draw(Graphics graphics) { }
        public virtual void Draw(Graphics graphics, Lightmap[,] lightmap) { }
        public virtual void PostDraw(Graphics graphics) { }
        public virtual void PostFX() { }
        public void LampEffect(Lamp lamp)
        {
            if (!PreUpdate() || !active)
                return;
            float num = 0;
            num = Helper.RangeNormal(lamp.Center, this.Center, range);
            if (num == 0f)
                return;
            AdjustColor(num, lamp);
        }
        void AdjustColor(float num, Lamp lamp)
        {
            alpha = 0f;
            alpha += Math.Max(0, num);
            alpha = Math.Min(alpha, 1f);
            //color = Ext.AdditiveV2(color, lamp.color, num / 2f);
        }
    }
    sealed class OutOfBoundsException : Exception
    {
        int i, j;
        public OutOfBoundsException(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
        public override string Message => $"Attempting to access an index of i:{i}, j:{j} outside the array.";
    }
}
