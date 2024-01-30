using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using cotf.Assets;
using cotf.Base;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Rectangle = System.Drawing.Rectangle;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Matrix = System.Drawing.Drawing2D.Matrix;
using System.Numerics;
using FoundationR;

namespace cotf.Base
{
    public static class Ext
    {
        public static double Distance(this Vector2 one, Vector2 v2)
        {
            return Math.Sqrt(Math.Pow(v2.X - one.X, 2) + Math.Pow(v2.Y - one.Y, 2));
        }
        public static Vector2 ToVector2(this Point a)
        {
            return new Vector2(a.X, a.Y);
        }
        public static bool Contains(this Rectangle one, Vector2 position)
        {
            return position.X > one.Left && position.X < one.Right && position.Y > one.Top && position.Y < one.Bottom;
        }
        public static Vector2 Center(this Rectangle one)
        {
            return new Vector2(one.Location.X, one.Location.Y) + new Vector2(one.Width / 2, one.Height / 2);
        }
        public static float MaxNormal(this Vector2 v2)
        {
            return new float[] { Math.Abs(v2.X), Math.Abs(v2.Y) }.Max();
        }
        public static float Max(this Vector2 v2)
        {
            return new float[] { v2.X, v2.Y }.Max();
        }
        public static float Min(this Vector2 v2)
        {
            return new float[] { v2.X, v2.Y }.Min();
        }
        public static Color FromFloat(float a, float r, float g, float b)
        {
            int A = (int)Math.Min(255f * a, 255),
                R = (int)Math.Min(255f * r, 255),
                G = (int)Math.Min(255f * g, 255),
                B = (int)Math.Min(255f * b, 255);
            return Color.FromArgb(A, R, G, B);
        }
        public static Color Transparency(this Color one, float alpha)
        {
            int a = (int)(255f * alpha), 
                r = 0, 
                g = 0, 
                b = 0;
            r = one.R;
            g = one.G;
            b = one.B;
            return Color.FromArgb(a, r, g, b);
        }
        public static Color Average(this Color one, Color two)
        {
            int r = 0, g = 0, b = 0;
            r = one.R + two.R;
            g = one.G + two.G;
            b = one.B + two.B;
            r /= 2;
            g /= 2;
            b /= 2;
            return Color.FromArgb(r, g, b);
        }
        public static Color Average(Color[] array)
        {
            int r = 0, g = 0, b = 0;
            for (int i = 0; i < array.Length; i++)
            {
                r += array[i].R;
                g += array[i].G;
                b += array[i].B;
            }
            r /= array.Length;
            g /= array.Length;
            b /= array.Length;
            return Color.FromArgb(r, g, b);
        }
        public static Color Average(Color[,] array)
        {
            int r = 0, g = 0, b = 0;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                { 
                    r += array[i, j].R;
                    g += array[i, j].G;
                    b += array[i, j].B;
                }
            }
            r /= array.Length;
            g /= array.Length;
            b /= array.Length;
            return Color.FromArgb(r, g, b);
        }
        private static Color Subtractive(this Color one, Color two)
        {
            int r = (int)Math.Max(Math.Min(one.R - Math.Abs(two.R - 255f), 255), 0);
            int g = (int)Math.Max(Math.Min(one.G - Math.Abs(two.G - 255f), 255), 0);
            int b = (int)Math.Max(Math.Min(one.B - Math.Abs(two.B - 255f), 255), 0);
            return Color.FromArgb(r, g, b);
        }
        public static Color SubtractiveV2(this Color one, Color two)
        {
            int r = (int)Math.Max(Math.Min(one.R - two.R, 255), 0);
            int g = (int)Math.Max(Math.Min(one.G - two.G, 255), 0);
            int b = (int)Math.Max(Math.Min(one.B - two.B, 255), 0);
            return Color.FromArgb(r, g, b);
        }
        public static Color SubtractiveV2(this Color one, Color two, float distance)
        {
            int r = (int)Math.Max(Math.Min(one.R - two.R * distance, 255), 0);
            int g = (int)Math.Max(Math.Min(one.G - two.G * distance, 255), 0);
            int b = (int)Math.Max(Math.Min(one.B - two.B * distance, 255), 0);
            return Color.FromArgb(r, g, b);
        }
        private static Color Additive(this Color one, Color two)
        {
            int r = (int)Math.Min(one.R + Math.Abs(two.R - 255f), 255);
            int g = (int)Math.Min(one.G + Math.Abs(two.G - 255f), 255);
            int b = (int)Math.Min(one.B + Math.Abs(two.B - 255f), 255);
            return Color.FromArgb(r, g, b);
        }
        private static Color Additive(this Color color, Color newColor, float distance)
        {
            return Color.FromArgb(
                (int)(color.A * distance),
                (int)Math.Max(Math.Min(color.R + Math.Abs(newColor.R - 255f) * distance, 255), 0),
                (int)Math.Max(Math.Min(color.G + Math.Abs(newColor.G - 255f) * distance, 255), 0),
                (int)Math.Max(Math.Min(color.B + Math.Abs(newColor.B - 255f) * distance, 255), 0));
        }
        public static Color AdditiveV2(this Color color, Color newColor)
        {
            return Color.FromArgb(
                color.A,
                (int)Math.Min(color.R + newColor.R, 255),
                (int)Math.Min(color.G + newColor.G, 255),
                (int)Math.Min(color.B + newColor.B, 255));
        }
        public static Color AdditiveV2(this Color color, Color newColor, float distance)
        {
            return Color.FromArgb(
                color.A,
                (int)Math.Min(color.R + newColor.R * distance, 255),
                (int)Math.Min(color.G + newColor.G * distance, 255),
                (int)Math.Min(color.B + newColor.B * distance, 255));
        }
        public static Color Multiply(this Color one, Color two, float alpha)
        {
            int a = (int)Math.Max(Math.Min(255f * Math.Min(alpha, 1f), 255), 1f);
            int r = (int)Math.Min(Math.Max(one.R, 1f) * ((two.R / 255f) + 1), 255);
            int g = (int)Math.Min(Math.Max(one.G, 1f) * ((two.G / 255f) + 1), 255);
            int b = (int)Math.Min(Math.Max(one.B, 1f) * ((two.B / 255f) + 1), 255);
            return Color.FromArgb(a, r, g, b);
        }
        public static Color NonAlpha(this Color color)
        {
            int a = 255;
            int r = color.R; 
            int g = color.G;
            int b = color.B;
            return Color.FromArgb(a, r, g, b);
        }
        public static Color Clone(this Color copy)
        {
            int a = copy.A;
            int r = copy.R;
            int g = copy.G;
            int b = copy.B;
            return Color.FromArgb(a, r, g, b);
        }
        public static void Write(this BinaryWriter bw, Vector2 vector2)
        {
            bw.Write(vector2.X);
            bw.Write(vector2.Y);
        }
        public static void Write(this BinaryWriter bw, Color color)
        {
            bw.Write(color.A);
            bw.Write(color.R);
            bw.Write(color.G);
            bw.Write(color.B);
        }
        public static Vector2 ReadVector2(this BinaryReader br)
        {
            Vector2 v2 = Vector2.Zero;
            v2.X = br.ReadSingle();
            v2.Y = br.ReadSingle();
            return v2;
        }
        public static Color ReadColor(this BinaryReader br)
        {
            byte a = br.ReadByte();
            byte r = br.ReadByte();
            byte g = br.ReadByte();
            byte b = br.ReadByte();
            return Color.FromArgb(a, r, g, b);
        }
    }
    public static class Helper
    {
        public const float Radian = 0.017f;
        public static double ToRadian(double degrees)
        {
            return degrees * Radian;
        }
        public static float ToRadian(float degrees)
        {
            return degrees * Radian;
        }
        public static double ToDegrees(double radians)
        {
            return radians / Radian;
        }
        public static float ToDegrees(float radians)
        {
            return radians / Radian;
        }
        public static float Ratio(float width, float height)
        {
            return width / height;
        }
        public static float RatioConvert(float ratio, float width)
        {
            return width * ratio;
        }
        public static double Distance(Vector2 one, Vector2 two)
        {
            return Math.Sqrt(Math.Pow(two.X - one.X, 2) + Math.Pow(two.Y - one.Y, 2));
        }
        public static float NormalizedRadius(float distance, float radius)
        {
            return (float)Math.Abs(Math.Min(distance / radius, 1f) - 1f);
        }
        public static float NormalizedRadius(Vector2 one, Vector2 two, float radius)
        {
            return (float)Math.Abs(Math.Min(Helper.Distance(one, two) / radius, 1f) - 1f);
        }
        public static float AngleTo(Vector2 from, Vector2 to)
        {
            return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
        }
        public static Vector2 AngleToSpeed(float angle, float amount)
        {
            float cos = (float)(amount * Math.Cos(angle));
            float sine = (float)(amount * Math.Sin(angle));
            return new Vector2(cos, sine);
        }
        public static double WrapRadianAngle(double angle)
        {
            while (angle < Math.PI * -1 || angle > Math.PI)
            { 
                double rem = 0d;
                if (angle > Math.PI)
                {
                    rem = (Math.PI * -1) % (angle * -1);
                    angle = Math.PI * -1 + rem;
                }
                if (angle < Math.PI * -1)
                {
                    rem = Math.PI % (angle * -1);
                    angle = Math.PI * -1 + rem;
                }
            }
            return angle;
        }
        public static double[] GetAngle(int max)
        {
            double[] result = new double[max];
            double start = Math.PI * 2f / max;
            for (int i = 0; i < max; i++)
            {
                result[i] = start * (i + 1);
            }
            return result;
        }
        public static float RangeNormal(float value, float range = 100f)
        {
            return Math.Max((value * -1f + range) / range, 0);
        }
        public static float RangeNormal(Vector2 to, Vector2 from, float range = 100f)
        {
            return Math.Max(((float)Helper.Distance(from, to) * -1f + range) / range, 0);
        }
        //public static bool SightLine(Vector2 from, Entity target, Size size, int step)
        //{
        //    for (int n = 0; n < Helper.Distance(from, target.Center); n += step)
        //    {
        //        var v2 = from + Helper.AngleToSpeed(Helper.AngleTo(from, target.Center), n);
        //        if (Lib.tile[(int)v2.X / size.Width, (int)v2.Y / size.Height].active)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
    public static class Error
    {
        public static int[,] GetArray(int width, int height, int size = 16)
        {
            int i = width / size;
            int j = height / size;
            int[,] brush = new int[i, j];
            int num = -1;
            for (int n = 0; n < brush.GetLength(1); n++)
            {
                for (int m = 0; m < brush.GetLength(0); m++)
                {
                    if (n > 0 && m == 0)
                    {
                        num = brush[m, n - 1] * -1;
                        _write(ref brush, m, n, num);
                        continue;
                    }
                    _write(ref brush, m, n, num *= -1);
                }
            }
            return brush;
        }
        static void _write(ref int[,] brush, int m, int n, int value)
        {
            brush[m, n] = value;
        }
    }
    public static class Drawing
    {
        public static Bitmap ErrorResult(int width, int height, int size = 16)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(result))
            {
                int[,] brush = Error.GetArray(width, height, size);
                for (int i = 0; i < brush.GetLength(0); i++)
                {
                    for (int j = 0; j < brush.GetLength(1); j++)
                    {
                        int x = i * size;
                        int y = j * size;
                        switch (brush[i, j])
                        { 
                            case -1:
                                gfx.FillRectangle(Brushes.MediumPurple, new Rectangle(x, y, size, size));
                                gfx.DrawRectangle(Pens.Purple, new Rectangle(x, y, size - 1, size - 1));
                                break;
                            case 1:
                                gfx.FillRectangle(Brushes.Black, new Rectangle(x, y, size, size));
                                gfx.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(50, 50, 50))), new Rectangle(x, y, size - 1, size - 1));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return result;
        }
        public static Bitmap Mask_Circle(int size, Color mask)
        {
            float offset = 0.95f;
            Bitmap result = new Bitmap(size, size);
            using (Graphics gfx = Graphics.FromImage(result))
            {
                gfx.FillRectangle(new SolidBrush(mask), new RectangleF(0, 0, size, size));
                gfx.FillEllipse(Brushes.Black, new RectangleF(0, 0, size * offset, size * offset));
                result.MakeTransparent(Color.Black);
            }
            return result;
        }
        public static Image TextureMask(Bitmap image, Bitmap mask, Color transparency)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);
            using (Graphics _mask = Graphics.FromImage(mask))
            {
                using (Graphics _image = Graphics.FromImage(image))
                {
                    if (mask.Width < image.Width && mask.Height < image.Height)
                    {
                        _mask.ScaleTransform((float)image.Width / mask.Width, (float)image.Height / mask.Width);
                    }
                    _image.DrawImage(mask, Point.Empty);
                }
                image.MakeTransparent(transparency);
                Graphics gfx3 = Graphics.FromImage(result);
                gfx3.DrawImage(image, Point.Empty);
                gfx3.Dispose();
            }
            return result;
        }
        private static bool dynamic(List<Tile> brush, Vector2 pixel, Vector2 topLeft, Lamp light, float range)
        {
            Vector2 c = light.position;
            for (int n = 0; n < brush.Count; n++)
            {
                Vector2[] corner = new Vector2[]
                {
                    brush[n].position,
                    brush[n].position + new Vector2(brush[n].Width, 0),
                    brush[n].position + new Vector2(0, brush[n].Height),
                    brush[n].position + new Vector2(brush[n].Width, brush[n].Height)
                };
                corner = corner.OrderByDescending(t => Helper.Distance(c, t)).ToArray();
                Vector2[] v2 = new Vector2[] { corner[1], corner[2] };
                Vector2 _pixel = topLeft + pixel;
                double a0 = Helper.AngleTo(v2[0], c);
                double a1 = Helper.AngleTo(v2[1], c);
                double angle = Helper.AngleTo(_pixel, c);
                if (light.position.X < brush[n].Center.X)
                {
                    a0 = Helper.AngleTo(c, v2[0]);
                    a1 = Helper.AngleTo(c, v2[1]);
                    angle = Helper.AngleTo(c, _pixel);
                }
                Vector2[] _corner = corner.Concat(new Vector2[] { brush[n].Center }).ToArray();
                _corner = _corner.OrderBy(t => Helper.Distance(c, t)).ToArray();

                float dist = (float)Helper.Distance(c, _pixel);
                float dist2 = (float)Helper.Distance(c, _corner[1]);
                double[] _angle = new double[] { a0, a1 }.OrderByDescending(t => t).ToArray();
                if (angle > _angle[1] && angle < _angle[0])
                {                   
                    if (dist > dist2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static REW Lightpass0(List<Tile> brush, REW layer0, Vector2 topLeft, Lamp light, float range)
        {
            for (int i = 0; i < layer0.Width; i++)
            {
                for (int j = 0; j < layer0.Height; j++)
                {
                    float distance = (float)Helper.Distance(topLeft + new Vector2(i, j), light.position);
                    float radius = Helper.NormalizedRadius(distance, range);
                    if (radius > 0f && dynamic(brush, new Vector2(i, j), topLeft, light, range))
                    {
                        Color srcPixel = layer0.GetPixel(i, j).color;
                        layer0.SetPixel(i, j, Ext.Multiply(srcPixel, light.color, radius));
                    }
                }
            }
            return layer0;
        }
        public static Bitmap Lightpass0(List<Tile> brush, Bitmap bitmap, Vector2 topLeft, Lamp light, float range)
        {
            Bitmap layer0 = (Bitmap)bitmap.Clone();
            Bitmap layer1 = new Bitmap(bitmap.Width, bitmap.Height);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    float distance = (float)Helper.Distance(topLeft + new Vector2(i, j), light.position);
                    float radius = Helper.NormalizedRadius(distance, range);
                    if (radius > 0f && dynamic(brush, new Vector2(i, j), topLeft, light, range))
                    {
                        Color srcPixel = layer0.GetPixel(i, j);
                        layer1.SetPixel(i, j, Ext.Multiply(srcPixel, light.color, radius));
                    }
                }
            }
            using (Graphics gfx = Graphics.FromImage(layer0))
                gfx.DrawImage(layer1, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            return layer0;
        }
        public static void TextureLighting(Image texture, Rectangle hitbox, Entity ent, float gamma, Graphics graphics)
        {
            using (Bitmap bitmap = new Bitmap(ent.Width, ent.Height))
            { 
                using (Graphics gfx = Graphics.FromImage(bitmap))
                {
                    gfx.DrawImage(texture, new Rectangle(0, 0, ent.Width, ent.Height));
                    //graphics.DrawImage(bitmap, hitbox);                    
                    var colorTransform = Drawing.SetColor(Ext.AdditiveV2(ent.color, ent.DefaultColor));
                    if (ent.inShadow)
                    {
                        colorTransform.SetGamma(gamma);
                    }
                    graphics.DrawImage(bitmap, hitbox, 0, 0, hitbox.Width, hitbox.Height, GraphicsUnit.Pixel, colorTransform);
                }
            }
        }
        public static void TextureLighting(Image texture, Rectangle hitbox, Lightmap map, Entity ent, float gamma, float alpha, Graphics graphics)
        {
            if (alpha > 0f)
            {
                var colorTransform = Drawing.SetColor(Ext.AdditiveV2(ent.color, map.color, alpha));
                if (ent.inShadow)
                {
                    colorTransform.SetGamma(gamma);
                }
                graphics.DrawImage(texture, hitbox, 0, 0, hitbox.Width, hitbox.Height, GraphicsUnit.Pixel, colorTransform);
                colorTransform.Dispose();
            }
            else
            {
                graphics.DrawImage(texture, hitbox);
            }
            map.alpha = alpha;
            map.color = map.DefaultColor;
            ent.color = map.DefaultColor;
        }
        public static Color TranslucentColorShift(Color color, float distance)
        {
            return Color.FromArgb(
                (int)Math.Max(Math.Min(color.A * distance, 255), 0),
                color.R, 
                color.G,
                color.B);
        }
        public static void DrawScale(Image image, Vector2 position, int width, int height, Color transparency, Graphics graphics, ImageAttributes attr, float scaleX = 1f, float scaleY = 1f)
        {
            MemoryStream mem = new MemoryStream();
            using (Bitmap clone = (Bitmap)image.Clone())
            { 
                clone.MakeTransparent(transparency);
                using (Bitmap bmp = new Bitmap(image.Width, image.Height))
                {
                    using (Graphics gfx = Graphics.FromImage(bmp))
                    {
                        gfx.ScaleTransform(scaleX, scaleY);
                        gfx.DrawImage(clone, Point.Empty);
                        bmp.Save(mem, ImageFormat.Png);
                    }
                }
                graphics.DrawImage(Bitmap.FromStream(mem), new Rectangle((int)position.X, (int)position.Y, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attr);
            }
            mem.Dispose();
        }
        public static void DrawTexture(Image image, Rectangle rectangle, int width, int height, Graphics graphics, ImageAttributes attr)
        {
            graphics.DrawImage(image, rectangle, 0, 0, width, height, GraphicsUnit.Pixel, attr);
        }
        public static void DrawRotate(Image image, Vector2 position, RectangleF rectangle, float angle, PointF origin, Color transparency, RotateType type, Graphics graphics, float scale = 1f)
        {
            MemoryStream mem = new MemoryStream();
            using (Bitmap clone = (Bitmap)image.Clone())
            { 
                clone.MakeTransparent(transparency);
                using (Bitmap bmp = new Bitmap(image.Width, image.Height))
                {
                    using (Graphics gfx = Graphics.FromImage(bmp))
                    {
                        switch (type)
                        {
                            case RotateType.MatrixTransform:
                                var matrix = new Matrix();
                                matrix.RotateAt(angle, origin);
                                gfx.Transform = matrix;
                                break;
                            case RotateType.GraphicsTransform:
                                gfx.TranslateTransform(origin.X, origin.Y);
                                gfx.RotateTransform(angle);
                                gfx.TranslateTransform(-origin.X, -origin.Y);
                                break;
                            default:
                                break;
                        }
                        gfx.ScaleTransform(scale, scale);
                        gfx.DrawImage(clone, Point.Empty);
                        bmp.Save(mem, ImageFormat.Png);
                    }
                }
                graphics.DrawImage(Bitmap.FromStream(mem), position.X, position.Y, rectangle, GraphicsUnit.Pixel);
            }
            mem.Dispose();
            
        }
        public static void DrawRotate(Image image, Vector2 position, float angle, PointF origin, Color transparency, RotateType type, Graphics graphics, float scale = 1f)
        {
            MemoryStream mem = new MemoryStream();
            using (Bitmap clone = (Bitmap)image.Clone())
            { 
                clone.MakeTransparent(transparency);
                using (Bitmap bmp = new Bitmap(image.Width, image.Height))
                { 
                    using (Graphics gfx = Graphics.FromImage(bmp))
                    {
                        switch (type)
                        { 
                            case RotateType.MatrixTransform:
                                var matrix = new Matrix();
                                matrix.RotateAt(angle, origin);
                                gfx.Transform = matrix;
                                break;
                            case RotateType.GraphicsTransform:
                                gfx.TranslateTransform(origin.X, origin.Y);
                                gfx.RotateTransform(angle);
                                gfx.TranslateTransform(- origin.X, - origin.Y);
                                break;
                            default:
                                break;
                        }
                        gfx.ScaleTransform(scale, scale);
                        gfx.DrawImage(clone, Point.Empty);
                        bmp.Save(mem, ImageFormat.Png);
                    }
                }
                graphics.DrawImage(Bitmap.FromStream(mem), new PointF(position.X, position.Y));
            }
            mem.Dispose();
        }
        public static void DrawRotate(Image image, Rectangle rect, float angle, PointF origin, Color transparency, RotateType type, Graphics graphics)
        {
            MemoryStream mem = new MemoryStream();
            using (Bitmap clone = (Bitmap)image.Clone())
            { 
                clone.MakeTransparent(transparency);
                using (Bitmap bmp = new Bitmap(image.Width, image.Height))
                { 
                    using (Graphics gfx = Graphics.FromImage(bmp))
                    {
                        switch (type)
                        { 
                            case RotateType.MatrixTransform:
                                var matrix = new Matrix();
                                matrix.RotateAt(angle, origin);
                                gfx.Transform = matrix;
                                break;
                            case RotateType.GraphicsTransform:
                                gfx.TranslateTransform(origin.X, origin.Y);
                                gfx.RotateTransform(angle);
                                gfx.TranslateTransform(- origin.X, - origin.Y);
                                break;
                            default:
                                break;
                        }
                        gfx.DrawImage(clone, Point.Empty);
                        bmp.Save(mem, ImageFormat.Png);
                    }
                }
                graphics.DrawImage(Bitmap.FromStream(mem), rect);
            }
            mem.Dispose();
        }
        public static void DrawRotate(Image image, Rectangle rect, Rectangle sourceRect, float angle, PointF origin, Color newColor, Color transparency, RotateType type, Graphics graphics)
        {
            ImageAttributes attributes = new ImageAttributes();
            ColorMatrix transform = new ColorMatrix(new float[][]
            { 
                new float[] { newColor.R / 255f, 0, 0, 0, 0 },
                new float[] { 0, newColor.G / 255f, 0, 0, 0 },
                new float[] { 0, 0, newColor.B / 255f, 0, 0 },
                new float[] { 0, 0, 0, newColor.A / 255f, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            });
            attributes.SetColorMatrix(transform);                  

            MemoryStream mem = new MemoryStream();
            using (Bitmap clone = (Bitmap)image.Clone())
            { 
                clone.MakeTransparent(transparency);
                using (Bitmap bmp = new Bitmap(image.Width, image.Height))
                { 
                    using (Graphics gfx = Graphics.FromImage(bmp))
                    {
                        switch (type)
                        {
                            case RotateType.MatrixTransform:
                                var matrix = new Matrix();
                                matrix.RotateAt(angle, origin);
                                gfx.Transform = matrix;
                                break;
                            case RotateType.GraphicsTransform:
                                gfx.TranslateTransform(origin.X, origin.Y);
                                gfx.RotateTransform(angle);
                                gfx.TranslateTransform(- origin.X, - origin.Y);
                                break;
                            default:
                                break;
                        }
                        gfx.DrawImage(clone, Point.Empty);
                        bmp.Save(mem, ImageFormat.Png);
                    }
                }
                graphics.DrawImage(Bitmap.FromStream(mem), rect, sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height, GraphicsUnit.Pixel, attributes);
            }
            mem.Dispose();
        }
        public static ImageAttributes ReColor(Color color, Color newColor, float alpha = 1f)
        {
            ImageAttributes attributes = new ImageAttributes();
            ColorMatrix transform = new ColorMatrix(new float[][]
            {
                new float[] { color.R / 255f, 0, 0, 0, 0 },
                new float[] { 0, color.G / 255f, 0, 0, 0 },
                new float[] { 0, 0, color.B / 255f, 0, 0 },
                new float[] { 0, 0, 0, color.A / 255f, 0 },
                new float[] {   newColor.R / 255f, 
                                newColor.G / 255f, 
                                newColor.B / 255f,
                                alpha, 0 }
            });
            attributes.SetColorMatrix(transform);
            return attributes;
        }
        public static ImageAttributes SetColor(Color color, float alpha = 1f)
        {
            ImageAttributes attributes = new ImageAttributes();
            ColorMatrix transform = new ColorMatrix(new float[][]
            {
                new float[] { color.R / 255f, 0, 0, 0, 0 },
                new float[] { 0, color.G / 255f, 0, 0, 0 },
                new float[] { 0, 0, color.B / 255f, 0, 0 },
                new float[] { 0, 0, 0, Math.Max(0f, Math.Min(1f, alpha)), 0 },
                new float[] { 0, 0, 0, 0, 0 }
            });
            attributes.SetColorMatrix(transform);
            return attributes;
        }
        public static ImageAttributes SetColor(Color color)
        {
            ImageAttributes attributes = new ImageAttributes();
            ColorMatrix transform = new ColorMatrix(new float[][]
            {
                new float[] { color.R / 255f, 0, 0, 0, 0 },
                new float[] { 0, color.G / 255f, 0, 0, 0 },
                new float[] { 0, 0, color.B / 255f, 0, 0 },
                new float[] { 0, 0, 0, color.A / 255f, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            });
            attributes.SetColorMatrix(transform);
            return attributes;
        }
        public static SolidBrush Opacity(Color color, float value)
        {  
            byte min = (byte)(Math.Min(value, 1f) * 255);
            return new SolidBrush(Color.FromArgb(min, color.R, color.G, color.B));
        }
        public static void DrawColorTransform(Image image, Vector2 one, Vector2 two, Rectangle hitbox, Color start, Color end, float radius, Graphics graphics)
        {
            float f = Helper.NormalizedRadius(one, two, radius);
            if (f < 1f && f != 0f)
            {
                graphics.DrawImage(image, hitbox, 0, 0, hitbox.Width, hitbox.Height, GraphicsUnit.Pixel, Drawing.ReColor(start, end, f / 2f));
            }
        }
        public static void DrawColorOverlay(Vector2 one, Vector2 two, Rectangle hitbox, Color start, Color end, float radius, Graphics graphics)
        {
            float f = Helper.NormalizedRadius(one, two, radius);
            if (f < 1f && f != 0f)
            {
                graphics.DrawImage(new Bitmap(hitbox.Width, hitbox.Height), hitbox, 0, 0, hitbox.Width, hitbox.Height, GraphicsUnit.Pixel, Drawing.ReColor(start, end, f));
            }
        }
    }
    public enum RotateType
    {
        MatrixTransform,
        GraphicsTransform
    }
}
