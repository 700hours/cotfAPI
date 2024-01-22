using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cotf
{
    internal class Texture
    {
        public static Bitmap[,] SplitImage(Bitmap bitmap, Size size)
        {
            size.Width -= bitmap.Width % size.Width;
            size.Height -= bitmap.Height % size.Height;
            Bitmap[,] value = new Bitmap[bitmap.Width / size.Width, bitmap.Height / size.Height];
            for (int i = 0; i < bitmap.Width; i += size.Width)
            {
                for (int j = 0; j < bitmap.Height; j += size.Height)
                {
                    using (Bitmap @new = new Bitmap(size.Width, size.Height))
                    { 
                        using (Graphics g = Graphics.FromImage(@new))
                        {
                            g.DrawImage(bitmap, new Rectangle(0, 0, size.Width, size.Height), new Rectangle(i, j, size.Width, size.Height), GraphicsUnit.Pixel);
                        }
                        value[i / size.Width, j / size.Height] = (Bitmap)@new.Clone();
                    }
                }
            }
            return value;
        }
        public static void OutputArrayToFiles(string prefix, Bitmap bitmap, Size size)
        {
            Bitmap[,] value = SplitImage(bitmap, size);
            for (int i = 0; i < bitmap.Width; i += size.Width)
            {
                for (int j = 0; j < bitmap.Height; j += size.Height)
                {
                    i /= size.Width;
                    j /= size.Height;
                    value[i, j].Save(Path.Combine(Lib.TexturePath, $"{prefix}{i}{j}.png"), ImageFormat.Png);
                }
            }
        }
    }
}
