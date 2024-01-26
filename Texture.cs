using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cotf
{
    public class Texture
    {
        internal int i, j;
        public string Name { get; set; }
        public Bitmap Value { get; set; }
        private Texture(Bitmap texture, string name)
        {
            this.Name = name;
            this.Value = texture;
        }
        public static Texture NewTexture(Bitmap tex, int i, int j, string name)
        {
            var _tex = new Texture(tex, name) { i = i, j = j };
            Lib.texture.Add(_tex);
            return _tex;
        }
        public static Bitmap[,] SplitImage(Bitmap bitmap, Size size, string prefix = "background")
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
                        int m = i / size.Width;
                        int n = j / size.Height;
                        NewTexture((Bitmap)@new.Clone(), m, n, $"{prefix}{m}{n}");
                    }
                }
            }
            return value;
        }
        public static void OutputArrayToFiles(string prefix, Bitmap bitmap, Size size)
        {
            if (!Directory.Exists(Lib.TexturePath = Path.Combine(Directory.GetCurrentDirectory(), "Textures")))
            {
                Directory.CreateDirectory(Lib.TexturePath);
            }
            Bitmap[,] value = SplitImage(bitmap, size);
            for (int m = 0; m < bitmap.Width; m += size.Width)
            {
                for (int n = 0; n < bitmap.Height; n += size.Height)
                {
                    int i = m / size.Width;
                    int j = n / size.Height;
                    value[i, j].Save(Path.Combine(Lib.TexturePath, $"{prefix}{i}{j}.png"), ImageFormat.Png);
                }
            }
        }
        public static void GenerateColorTextureFiles(string prefix, Color color, Size size)
        {
            if (!Directory.Exists(Lib.TexturePath = Path.Combine(Directory.GetCurrentDirectory(), "Textures")))
            {
                Directory.CreateDirectory(Lib.TexturePath);
            }
            using (Bitmap bitmap = new Bitmap(Lib.OutputWidth, Lib.OutputHeight))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.FillRectangle(new SolidBrush(color), 0, 0, Lib.OutputWidth, Lib.OutputHeight);
                }
                for (int m = 0; m < bitmap.Width; m += size.Width)
                {
                    for (int n = 0; n < bitmap.Height; n += size.Height)
                    {
                        int i = m / size.Width;
                        int j = n / size.Height;
                        NewTexture((Bitmap)bitmap.Clone(), i, j, $"{prefix}{i}{j}");
                    }
                }
            }
        }
    }
}
