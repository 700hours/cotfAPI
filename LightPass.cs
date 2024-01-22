using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using cotf.Base;
using System.Threading;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace cotf
{
    public sealed class LightPass
    {
        static List<Tile> NearbyTile(Lamp lamp)
        {
            List<Tile> brush = new List<Tile>();
            for (int i = 0; i < Lib.tile.GetLength(0); i++)
            {
                for (int j = 0; j < Lib.tile.GetLength(1); j++)
                {
                    if (Lib.tile[i, j] != null && Lib.tile[i, j].active)
                    {
                        if (Helper.Distance(Lib.tile[i, j].Center, lamp.position) < lamp.range)
                        {
                            brush.Add(Lib.tile[i, j]);
                        }
                    }
                }
            }
            return brush;
        }
        static List<Background> NearbyFloor(Lamp lamp)
        {
            List<Background> brush = new List<Background>();
            for (int i = 0; i < Lib.background.GetLength(0); i++)
            {
                for (int j = 0; j < Lib.background.GetLength(1); j++)
                {
                    if (Lib.background[i, j] != null && Lib.background[i, j].active)
                    {
                        if (Helper.Distance(Lib.background[i, j].Center, lamp.position) < lamp.range)
                        {
                            brush.Add(Lib.background[i, j]);
                        }
                    }
                }
            }
            return brush;
        }
        public static void PreProcessing()
        {
            for (int n = 0; n < Lib.lamp.Length; n++)
            {
                Lamp lamp = Lib.lamp[n];
                if (lamp == null || !lamp.active || lamp.owner != 255)
                    continue;

                List<Tile> brush = NearbyTile(lamp);

                for (int i = 0; i < Lib.background.GetLength(0); i++)
                {
                    for (int j = 0; j < Lib.background.GetLength(1); j++)
                    {
                        if (Lib.background[i, j] == null || !Lib.background[i, j].active)
                            continue;
                        if (Helper.Distance(Lib.background[i, j].Center, lamp.Center) <= lamp.range)
                        {
                            Lib.background[i, j].texture = Drawing.Lightpass0(brush, (Bitmap)Lib.background[i, j].texture, Lib.background[i, j].position, lamp, lamp.range);
                        }
                    }
                }
            }
        }
    }
}
