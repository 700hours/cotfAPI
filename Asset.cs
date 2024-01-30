using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using cotf.Base;
using FoundationR;

namespace cotf.Assets
{
    public class TexAsset
    {
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, byte[] lpBits);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public string Name;
        public REW Value;
    }
    public sealed class Asset<T> where T : REW
    {
        public static List<TexAsset> Textures = new List<TexAsset>();
        public static Bitmap GetBitmap(T tex)
        {
            IntPtr hbitmap = TexAsset.CreateBitmap(tex.Width, tex.Height, 1, (ushort)tex.BitsPerPixel, tex.GetPixels());
            Bitmap map = Bitmap.FromHbitmap(hbitmap);
            TexAsset.DeleteObject(hbitmap);
            return map;
        }
        public static REW GetTexture(string name)
        {
            return Textures.Find(t => t.Name == name).Value;
        }
        public static T Load(string name)
        {
            return (T)new FileStream(Path.Combine(Directory.GetCurrentDirectory(), name + ".rew"), FileMode.Open, FileAccess.Read).ReadREW();
        }
        public static T Request(string name)
        {
            Bitmap map = (Bitmap)Bitmap.FromFile(name);
            T t = (T)REW.Extract(map, (short)map.PixelFormat.BytesPerPixel());
            map.Dispose();
            return t;
        }
        public static T Request(string name, string extension)
        {
            Bitmap map = (Bitmap)Bitmap.FromFile(name + extension);
            T t = (T)REW.Extract(map, (short)map.PixelFormat.BytesPerPixel());
            map.Dispose();
            return t;
        }
    }
}
