using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cotf
{
    public class ThreadBitmap
    {
        Color[,] Pixels = new Color[,] {};
        public int Width => Pixels.GetLength(0);
        public int Height => Pixels.GetLength(1);
        public ThreadBitmap(Bitmap bitmap)
        {
            Pixels = new Color[bitmap.Width, bitmap.Height];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Pixels[i, j] = bitmap.GetPixel(i, j);
                }
            }
        }
        public Bitmap GetBitmap()
        {
            Bitmap result = new Bitmap(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    result.SetPixel(i, j, GetPixel(i, j));
                }
            }
            return result;
        }
        public Color GetPixel(int x, int y)
        {
            return Pixels[x, y];
        }
        public void SetPixel(int x, int y, Color color)
        {
            Pixels[x, y] = color;
        }
    }
}
