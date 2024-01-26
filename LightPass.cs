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
using System.Numerics;
using System.ComponentModel;

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
        static ThreadBitmap[,] getThreadBmpArray()
        {
            var output = new ThreadBitmap[Lib.background.GetLength(0), Lib.background.GetLength(1)];
            for (int i = 0; i < Lib.background.GetLength(0); i++)
            {
                for (int j = 0; j < Lib.background.GetLength(1); j++)
                {
                    output[i, j] = new ThreadBitmap((Bitmap)Lib.background[i, j].texture);
                } 
            }
            return output;
        }
        static ThreadBitmap getThreadBmp(int i, int j)
        {
            return new ThreadBitmap((Bitmap)Lib.background[i, j].texture);
        }
        public static void PreProcessing()
        {
            for (int n = 0; n < Lib.lamp.Length; n++)
            {
                Lamp lamp = Lib.lamp[n];
                if (lamp == null || !lamp.active || lamp.owner != 255)
                    continue;

                List<Tile> brush = NearbyTile(lamp);
                
                int i = Lib.background.GetLength(0) - 1;
                int j = Lib.background.GetLength(1) - 1;
                int num = i + j;

                //  DEBUG
                lamp.range = 600f;
                while (num-- > 0)
                {
                    new Thread(() => 
                    {
                        BEGIN:
                        i--;
                        i = Math.Max(0, i);
                        j = Math.Max(0, j);
                        if (Lib.background[i, j] == null || !Lib.background[i, j].active)
                        {
                        }
                        else
                        {
                            if (Helper.Distance(Lib.background[i, j].Center, lamp.Center) <= lamp.range)
                            {
                                try
                                { 
                                    var tbmp = getThreadBmp(i, j);
                                    lock (tbmp)
                                    {
                                        var result = Drawing.Lightpass0(brush, tbmp, Lib.background[i, j].position, lamp, lamp.range);
                                        Lib.background[i, j].texture = tbmp.GetBitmap();
                                    }
                                }
                                catch 
                                {
                                    if (i == 0)
                                    {
                                        i = Lib.background.GetLength(0) - 1;
                                        --j;
                                        goto NEXT; 
                                    }
                                    else goto BEGIN;
                                }
                            }
                        }
                        NEXT:
                        i = Math.Max(0, i);
                        j = Math.Max(0, j);
                        if (Lib.background[i, j] == null || !Lib.background[i, j].active)
                        {
                        }
                        else 
                        {
                            if (Helper.Distance(Lib.background[i, j].Center, lamp.Center) <= lamp.range)
                            {
                                try
                                {
                                    var tbmp = getThreadBmp(i, j);
                                    lock (tbmp)
                                    {
                                        var result = Drawing.Lightpass0(brush, tbmp, Lib.background[i, j].position, lamp, lamp.range);
                                        Lib.background[i, j].texture = tbmp.GetBitmap();
                                    }
                                }
                                catch 
                                { 
                                }
                            }
                        }
                    }).Start();
                }
            }
        }
        public static void PreProcessing(ref Image texture)
        {
            for (int n = 0; n < Lib.lamp.Length; n++)
            {
                Lamp lamp = Lib.lamp[n];
                if (lamp == null || !lamp.active || lamp.owner != 255)
                    continue;
                List<Tile> brush = NearbyTile(lamp);
                texture = Drawing.Lightpass0(brush, (Bitmap)texture, Vector2.Zero, lamp, lamp.range);
            }
        }
    }
}
