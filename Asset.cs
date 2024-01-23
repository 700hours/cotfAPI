using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using cotf.Base;

namespace cotf.Assets
{
    public sealed class Asset<T> where T : Image
    {
        public static bool init;
        public static void Initialize(int num, int num2)
        {
            _Background<T>.Init(_Background<T>.Length = num);
            _Tile<T>.Init(_Tile<T>.Length = num2);
        }
        public static Bitmap GetBitmap(int i, int j)
        {
            return Lib.texture.Find(t => t.i == i && t.j == j).Value;
        }
        public static Texture GetTexture(string name)
        {
            return Lib.texture.Find(t => t.Name == name);
        }
        public static T Request(string name)
        {
            return (T)Bitmap.FromFile(name + ".png");
        }
        public static T Request(string name, string extension)
        {
            return (T)Bitmap.FromFile(name + extension);
        }
        public static T Get(Type type, int style)
        {
            if (!init)
            {
                Initialize(1, 1);
                init = true;
            }
            switch (type.Name)
            {
                case nameof(Background):
                    return _Background<T>.Texture[style];
                case nameof(Tile):
                    return _Tile<T>.Texture[style];
                default:
                    return null;
            }
        }
        static class _Background<T> where T : Image
        {
            internal static int Length;
            internal static T[] Texture = new T[Length];
            internal static void Init(int length)
            {
                Texture = new T[length];
                for (int i = 0; i < length; i++)
                {
                    using (Bitmap bmp = new Bitmap(10, 10))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        { 
                            g.FillRectangle(Brushes.White, new Rectangle(0, 0, 10, 10));
                            Texture[i] = (T)(Image)bmp;
                        }
                    }
                }
            }
        }
        static class _Tile<T> where T : Image
        {
            internal static int Length;
            internal static T[] Texture = new T[Length];
            internal static void Init(int length)
            {
                Texture = new T[length];
                for (int i = 0; i < length; i++)
                {
                    using (Bitmap bmp = new Bitmap(10, 10))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.FillRectangle(Brushes.White, new Rectangle(0, 0, 10, 10));
                            Texture[i] = (T)(Image)bmp;
                        }
                    }
                }
            }
        }
    }
    
    sealed class Content<T> where T : Image
    {
        public static T Request(string name)
        {
            return Assembly.GetExecutingAssembly().DefinedTypes.FirstOrDefault(t => t.Name == name).DeclaringType as T;
        }
        public static T Request()
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name + "." + typeof(T).Name;
            return Type.GetType(name) as T;
        }
        //public static T Request(string name)
        //{
        //    name = Assembly.GetExecutingAssembly().GetName().Name + "." + name;
        //    return Type.GetType(name) as T;
        //}
    }
}
