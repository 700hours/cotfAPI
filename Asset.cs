using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using cotf.Base;
using cotf.World;
using cotf.World.Traps;

namespace cotf.Assets
{
    public sealed class Asset<T> where T : Image
    {
        public static bool init;
        public static void Initialize(int num = 2, int num2 = 1, int num3 = 1, int num4 = 1, int num5 = 1, int num6 = 1, int num7 = 1)
        {
            _Background.Init(_Background.Length = num2);
            
        }
        public static T Request(string name)
        {
            return (T)Bitmap.FromFile("./Textures/" + name + ".png");
        }
        public static T Request(string name, string extension)
        {
            return (T)Bitmap.FromFile("./Textures/" + name + extension);
        }
        public static T Get(Type type, int style)
        {
            if (!init)
            {
                Initialize()
            }
            switch (type.Name)
            {
                case nameof(Background):
                    return _Background.Texture[style];
                case nameof(Tile):
                    return _
                default:
                    return (T)Main.pixel;
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
                            g.FillRectangle(Brushes.White, new Rectangle(0, 0, World.Tile.Size, World.Tile.Size));
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
